using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Core {
	public interface IGpuRunnable {
		/// <summary>
		/// Gets or sets the gpu processor.
		/// </summary>
		/// <value>
		/// The gpu processor.
		/// </value>
		string GpuProcessor {
			get;
		}

		/// <summary>
		/// Gets or sets the gpu processor method.
		/// </summary>
		/// <value>
		/// The gpu processor method.
		/// </value>
		string GpuProcessorMethod {
			get;
		}

		/// <summary>
		/// Gets the calculation parameters.
		/// </summary>
		/// <value>
		/// The calculation parameters.
		/// </value>
		string CalculationParameters {
			get;
		}
	}
}
