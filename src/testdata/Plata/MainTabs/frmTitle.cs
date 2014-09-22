using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Plata
{
	public class frmTitle : Plata.baseFlikForm
	{
		private System.ComponentModel.IContainer components = null;

		public frmTitle( Form parent ) : base( parent, FlikTyp._Ingen )
		{
			InitializeComponent();
			this.Bounds = parent.ClientRectangle;
			this.PerformLayout();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// frmTitle
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(892, 566);
			this.Name = "frmTitle";

		}
		#endregion

		public override void skolaUppdaterad()
		{
		}

	}

}

