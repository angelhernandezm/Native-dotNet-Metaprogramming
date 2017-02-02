using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;

namespace Metadata.Core {
	public interface INativeModuleLoader {
		/// <summary>
		/// Loads the native module and run.
		/// </summary>
		/// <param name="gpuRunnable">The gpu runnable.</param>
		/// <param name="message">The message.</param>
		void LoadNativeModuleAndRun(IGpuRunnable gpuRunnable, IMethodCallMessage message);
	}
}
