using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Core {
	/// <summary>
	/// 
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class GpuRunnable : Attribute, IGpuRunnable {
		/// <summary>
		/// Initializes a new instance of the <see cref="GpuRunnable"/> class.
		/// </summary>
		public GpuRunnable() {
			GpuProcessor = Consts.DefaultGpuProcessor;
			GpuProcessorMethod = Consts.DefaultGpuProcessorMethod;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GpuRunnable" /> class.
		/// </summary>
		/// <param name="selectedOperation">The selected operation.</param>
		/// <param name="gpuProcessor">The gpu processor.</param>
		/// <param name="gpuProcessorMethod">The gpu processor method.</param>
		public GpuRunnable(string gpuProcessor, string gpuProcessorMethod) {
			GpuProcessor = gpuProcessor;
			GpuProcessorMethod = GpuProcessorMethod;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GpuRunnable"/> class.
		/// </summary>
		/// <param name="gpuProcessor">The gpu processor.</param>
		/// <param name="gpuProcessorMethod">The gpu processor method.</param>
		/// <param name="calculationParameters">The calculation parameters.</param>
		public GpuRunnable(string gpuProcessor, string gpuProcessorMethod, string calculationParameters) {
			GpuProcessor = gpuProcessor;
			GpuProcessorMethod = gpuProcessorMethod;
			CalculationParameters = calculationParameters;
		}

		/// <summary>
		/// Gets or sets the gpu processor.
		/// </summary>
		/// <value>
		/// The gpu processor.
		/// </value>
		/// <exception cref="System.NotImplementedException">
		/// </exception>
		public string GpuProcessor {
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the gpu processor method.
		/// </summary>
		/// <value>
		/// The gpu processor method.
		/// </value>
		/// <exception cref="System.NotImplementedException">
		/// </exception>
		public string GpuProcessorMethod {
			get;
			private set;
		}

		/// <summary>
		/// Gets the calculation parameters.
		/// </summary>
		/// <value>
		/// The calculation parameters.
		/// </value>
		public string CalculationParameters {
			get;
			private set;
		}
	}
}
