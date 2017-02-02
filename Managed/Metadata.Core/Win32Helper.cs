using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Core {
	public class Win32Helper {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="results">The results.</param>
		/// <param name="itemCount">The item count.</param>
		/// <param name="operation">The operation.</param>
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void ResultCallback(IntPtr results, int itemCount, int operation);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="matrixA">The matrix A.</param>
		/// <param name="matrixB">The matrix B.</param>
		/// <param name="matrixALength">Length of the matrix A.</param>
		/// <param name="matrixBLength">Length of the matrix B.</param>
		/// <param name="M">The M.</param>
		/// <param name="N">The N.</param>
		/// <param name="W">The W.</param>
		/// <param name="notificationCallback">The notification callback.</param>
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void GpuMultiplicationProcessorCallback([MarshalAs(UnmanagedType.LPArray)] float[] matrixA,
												  [MarshalAs(UnmanagedType.LPArray)] float[] matrixB, int matrixALength,
												  int matrixBLength, int M, int N, int W, IntPtr notificationCallback);


		/// <summary>
		/// 
		/// </summary>
		/// <param name="matrixA">The matrix A.</param>
		/// <param name="matrixB">The matrix B.</param>
		/// <param name="matrixALength">Length of the matrix A.</param>
		/// <param name="matrixBLength">Length of the matrix B.</param>
		/// <param name="rate">The rate.</param>
		/// <param name="volatility">The volatility.</param>
		/// <param name="maturity">The maturity.</param>
		/// <param name="notificationCallback">The notification callback.</param>
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void GpuProcessorCalculateOptionPriceCallback([MarshalAs(UnmanagedType.LPArray)] float[] matrixA,
												  [MarshalAs(UnmanagedType.LPArray)] float[] matrixB, int matrixALength,
												  int matrixBLength, float rate, float volatility, float maturity,
												  IntPtr notificationCallback);

		/// <summary>
		/// Loads the library.
		/// </summary>
		/// <param name="lpFileName">Name of the lp file.</param>
		/// <returns></returns>
		[DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
		public static extern IntPtr LoadLibrary(string lpFileName);

		/// <summary>
		/// Closes the handle.
		/// </summary>
		/// <param name="hHandle">The h handle.</param>
		/// <returns></returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool CloseHandle(IntPtr hHandle);

		/// <summary>
		/// Frees the library.
		/// </summary>
		/// <param name="hModule">The h module.</param>
		/// <returns></returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FreeLibrary(IntPtr hModule);

		/// <summary>
		/// Gets the proc address.
		/// </summary>
		/// <param name="hModule">The h module.</param>
		/// <param name="procName">Name of the proc.</param>
		/// <returns></returns>
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
	}
}