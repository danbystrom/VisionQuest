using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Plata
{
	/// <summary>
	/// Summary description for frmAbout.
	/// </summary>
	public class frmAbout : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button cmdClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private readonly Bitmap _bmp;

		public frmAbout()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            this.Text += " " + AppSpecifics.Name;

			_bmp = new Bitmap( this.GetType(), "grfx.about.jpg");
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
			this.cmdClose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cmdClose
			// 
			this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdClose.Location = new System.Drawing.Point(412, 276);
			this.cmdClose.Name = "cmdClose";
			this.cmdClose.Size = new System.Drawing.Size(80, 28);
			this.cmdClose.TabIndex = 0;
			this.cmdClose.Text = "Stäng";
			// 
			// frmAbout
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ClientSize = new System.Drawing.Size(500, 310);
			this.Controls.Add(this.cmdClose);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAbout";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Om Photomic";
			this.ResumeLayout(false);

		}
		#endregion

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			if ( _bmp!=null )
				pevent.Graphics.DrawImage( _bmp, this.ClientRectangle );
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);
            e.Graphics.DrawString("Version " + AppSpecifics.Version, this.Font, Brushes.Black, this.ClientSize.Width * 0.25f, this.ClientSize.Height * 0.5f);
		}

		protected override void OnClosed(EventArgs e)
		{
			if ( _bmp!=null )
				_bmp.Dispose();
			base.OnClosed (e);
		}

	}

}
