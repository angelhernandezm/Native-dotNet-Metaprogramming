using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Core {
	public class MessageSink : IMessageSink, INativeModuleLoader {
		/// <summary>
		/// Initializes a new instance of the <see cref="MessageSink"/> class.
		/// </summary>
		/// <param name="nextSink">The next sink.</param>
		public MessageSink(IMessageSink nextSink) {
			NextSink = nextSink;
		}

		/// <summary>
		/// Asynchronously processes the given message.
		/// </summary>
		/// <param name="msg">The message to process.</param>
		/// <param name="replySink">The reply sink for the reply message.</param>
		/// <returns>
		/// Returns an <see cref="T:System.Runtime.Remoting.Messaging.IMessageCtrl" /> interface that provides a way to control asynchronous messages after they have been dispatched.
		/// </returns>
		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink) {
			return (NextSink.AsyncProcessMessage(msg, replySink));
		}

		/// <summary>
		/// Gets the next message sink in the sink chain.
		/// </summary>
		/// <returns>The next message sink in the sink chain.</returns>
		///   <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		///   </PermissionSet>
		public IMessageSink NextSink {
			get;
			private set;
		}

		/// <summary>
		/// Synchronously processes the given message.
		/// </summary>
		/// <param name="msg">The message to process.</param>
		/// <returns>
		/// A reply message in response to the request.
		/// </returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public IMessage SyncProcessMessage(IMessage msg) {
			var mcm = msg as IMethodCallMessage;
			RunInGpu(ref mcm);
			return (NextSink.SyncProcessMessage(msg) as IMethodReturnMessage);
		}

		/// <summary>
		/// Runs the in gpu.
		/// </summary>
		/// <param name="msg">The MSG.</param>
		private void RunInGpu(ref IMethodCallMessage msg) {
			var isGpuRunnable = msg.MethodBase.GetCustomAttributes(typeof(GpuRunnable), true).FirstOrDefault();

			if (isGpuRunnable != null) {
				LoadNativeModuleAndRun(isGpuRunnable as IGpuRunnable, msg);
			}
		}

		/// <summary>
		/// Loads the native module and run.
		/// </summary>
		/// <param name="gpuRunnable">The gpu runnable.</param>
		/// <param name="message">The message.</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public void LoadNativeModuleAndRun(IGpuRunnable gpuRunnable, IMethodCallMessage message) {
			if (!File.Exists(gpuRunnable.GpuProcessor))
				throw new FileNotFoundException(Consts.GpuProcessorNotFound, gpuRunnable.GpuProcessor);

            var hVcAmpLib =  Win32Helper.LoadLibrary("vcamp110.dll");
			var hModule = Win32Helper.LoadLibrary(gpuRunnable.GpuProcessor);
			var isMultiplication = string.IsNullOrEmpty(gpuRunnable.CalculationParameters);

			if (hModule != IntPtr.Zero) {
				var hProcAddress = Win32Helper.GetProcAddress(hModule, gpuRunnable.GpuProcessorMethod);
				if (hProcAddress != IntPtr.Zero) {
					if (isMultiplication) {
						var functor = Marshal.GetDelegateForFunctionPointer(hProcAddress, typeof(Win32Helper.GpuMultiplicationProcessorCallback)) as Win32Helper.GpuMultiplicationProcessorCallback;
						var matrixA = (float[])message.InArgs[0];
						var matrixB = (float[])message.InArgs[1];
						var notificationCallback = (Win32Helper.ResultCallback)message.InArgs[2];

						functor(matrixA, matrixB, matrixA.Length, matrixB.Length, Consts.ExtentRowSizeOne, Consts.ExtentColumnSizeTwo,
								Consts.ExtentColumnSizeOne, Marshal.GetFunctionPointerForDelegate(notificationCallback));
					} else {
						var parameters = new ScholesCalculationParameters();
						var tokenized = gpuRunnable.CalculationParameters.Split(';').ToList();
						var paramType = parameters.GetType();
						var flag = BindingFlags.SetProperty | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public;
						var functor = Marshal.GetDelegateForFunctionPointer(hProcAddress, typeof(Win32Helper.GpuProcessorCalculateOptionPriceCallback)) as Win32Helper.GpuProcessorCalculateOptionPriceCallback;
						var matrixA = (float[])message.InArgs[0];
						var matrixB = (float[])message.InArgs[1];
						var notificationCallback = (Win32Helper.ResultCallback)message.InArgs[2];

						tokenized.ForEach(x => {
							var prop = paramType.GetProperty(x.Substring(0, x.IndexOf('=')), flag);
							prop.SetValue(parameters, float.Parse(x.Substring(x.IndexOf('=') + 1)));
						});

						functor(matrixA, matrixB, matrixA.Length, matrixB.Length, parameters.Rate, parameters.Volatility,
							parameters.Maturity, Marshal.GetFunctionPointerForDelegate(notificationCallback));
					}

				}

				Win32Helper.FreeLibrary(hModule);
			    Win32Helper.FreeLibrary(hVcAmpLib);
			}
		}
	}
}
