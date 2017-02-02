using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Metadata.Core;

namespace TestHarness {
	/// <summary>
	/// 
	/// </summary>
	/// <param name="sender">The sender.</param>
	/// <param name="results">The results.</param>
	/// <param name="operation">The operation.</param>
	public delegate void MultiplicationCompleteDelegate(object sender, float[] results, int operation);

	[Enhanceable]
	public class ContextObject : ContextBoundObject {
		public event MultiplicationCompleteDelegate OnGpuOperationComplete;

		/// <summary>
		/// Performs the calculations.
		/// </summary>
		/// <param name="matrixA">The matrix A.</param>
		/// <param name="matrixB">The matrix B.</param>
		/// <param name="results">The results.</param>
		[GpuRunnable]
		public void MultiplyMatrices(float[] matrixA, float[] matrixB, Win32Helper.ResultCallback results) {
			var res = AppDomain.CurrentDomain.GetData(Operation.MatrixMultiplication.ToString());

		}

		/// <summary>
		/// Calculates the option price with black scholes.
		/// </summary>
		/// <param name="matrixA">The matrix A.</param>
		/// <param name="matrixB">The matrix B.</param>
		/// <param name="results">The results.</param>
		[GpuRunnable(Consts.DefaultGpuProcessor, Consts.CalculateOptionPriceMethod, "Rate=0.5;Volatility=10.0;Maturity=10.0")]
		public void CalculateOptionPriceWithBlackScholes(float[] matrixA, float[] matrixB, Win32Helper.ResultCallback results) {
			var res = AppDomain.CurrentDomain.GetData(Operation.OptionPriceCallBlackScholes.ToString());

		}


		/// <summary>
		/// Gets the results from gpu.
		/// </summary>
		/// <param name="results">The results.</param>
		/// <param name="itemCount">The item count.</param>
		/// <param name="operation">The operation.</param>
		protected internal void GetResultsFromGpu(IntPtr results, int itemCount, int operation) {
			var calculationResults = new float[itemCount];
			Marshal.Copy(results, calculationResults, 0, itemCount);

			if (OnGpuOperationComplete != null)
				OnGpuOperationComplete(this, calculationResults, operation);
		}
	}
}
