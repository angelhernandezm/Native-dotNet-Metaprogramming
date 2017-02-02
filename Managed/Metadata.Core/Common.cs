using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Core {
	/// <summary>
	/// 
	/// </summary>
	public enum Operation {
		MatrixMultiplication = 0,
		OptionPriceCallBlackScholes
	}

	/// <summary>
	/// 
	/// </summary>
	public class Consts {
		public const int MaxItemCount = 50000;
		public const int ExtentRowSizeOne = 64;
		public const int ExtentColumnSizeOne = 256;
		public const int ExtentColumnSizeTwo = 512;
		public const string ContextName = "NativeExecutionContext";
		public const string DefaultGpuProcessorMethod = "RunInGpu";
		public const string DefaultGpuProcessor = "GPUProcessor.dll";
		public const string GpuProcessorNotFound = "Dynamic Link Library with GpuProcessor not found";
		public const string CalculateOptionPriceMethod = "CalculateOptionPriceWithBlackScholesInGpu";
	}


	/// <summary>
	/// 
	/// </summary>
	public class ScholesCalculationParameters {
		public float Rate {
			get;
			set;
		}
		public float Volatility {
			get;
			set;
		}
		public float Maturity {
			get;
			set;
		}
	}
}
