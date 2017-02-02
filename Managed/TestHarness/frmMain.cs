using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Metadata.Core;
using System.Globalization;

namespace TestHarness {
	public partial class frmMain : Form {
		public frmMain() {
			InitializeComponent();
		}


		/// <summary>
		/// Handles the Click event of the button1 control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void button1_Click(object sender, EventArgs e) {
			var myContextObject = new ContextObject();
			var randomNumber = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), NumberStyles.HexNumber));
			var matrixA = Enumerable.Range(1, Consts.ExtentRowSizeOne * Consts.ExtentColumnSizeOne).Select(x => (float)randomNumber.Next(1, 250)).ToArray();
			var matrixB = Enumerable.Range(1, Consts.ExtentColumnSizeOne * Consts.ExtentColumnSizeTwo).Select(x => (float)randomNumber.Next(1, 250)).ToArray();
			myContextObject.OnGpuOperationComplete += ((object o, float[] res, int operation) => AppDomain.CurrentDomain.SetData(((Operation)operation).ToString(), res));
			myContextObject.MultiplyMatrices(matrixA, matrixB, myContextObject.GetResultsFromGpu);
		}

		/// <summary>
		/// Handles the Click event of the button2 control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void button2_Click(object sender, EventArgs e) {
			var myContextObject = new ContextObject();
			var randomNumber = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), NumberStyles.HexNumber));
			var matrixA = Enumerable.Range(1, Consts.MaxItemCount).Select(x => (float)randomNumber.Next(1, 500)).ToArray();
			var matrixB = Enumerable.Range(1, Consts.MaxItemCount).Select(x => (float)randomNumber.Next(1, 100)).ToArray();
			myContextObject.OnGpuOperationComplete += ((object o, float[] res, int operation) => AppDomain.CurrentDomain.SetData(((Operation)operation).ToString(), res));
			myContextObject.CalculateOptionPriceWithBlackScholes(matrixA, matrixB, myContextObject.GetResultsFromGpu);
		}
	}
}
