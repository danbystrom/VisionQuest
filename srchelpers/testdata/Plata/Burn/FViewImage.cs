using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Plata.Burn
{
	/// <summary>
	/// Summary description for FViewImage.
	/// </summary>
	public class FViewImage : vdUsr.baseGradientForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Bitmap _bmp;

		private FViewImage()
		{
			InitializeComponent();
		}

		private FViewImage( Bitmap bmp )
		{
			InitializeComponent();
			_bmp = bmp;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			Rectangle r = SystemInformation.WorkingArea;
			this.Location = r.Location;
			this.Size = r.Size;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);
			try
			{
				vdUsr.ImgHelper.drawImageUnscaled(
					e.Graphics,
					_bmp,
					(this.ClientSize.Width - _bmp.Width) / 2,
					(this.ClientSize.Height - _bmp.Height) / 2
					);
			}
			catch
			{
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown (e);
			this.Close();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown (e);
			this.Close();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// FViewImage
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.ControlBox = false;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FViewImage";
			this.ShowInTaskbar = false;
			this.Text = "";

		}
		#endregion

		public static void showDialog( Form parent, Bitmap bmp )
		{
			using ( FViewImage dlg = new FViewImage(bmp) )
				dlg.ShowDialog(parent);
		}

	}

}
