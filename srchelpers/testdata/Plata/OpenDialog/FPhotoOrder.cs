using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace Plata.OpenDialog
{
	/// <summary>
	/// Summary description for FPhotoOrder.
	/// </summary>
	public class FPhotoOrder : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Metafile _mf;

		public FPhotoOrder()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			// FPhotoOrder
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Name = "FPhotoOrder";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Fotoorder";

		}
		#endregion

		protected override void OnPaint(PaintEventArgs e)
		{
			if ( _mf==null )
				return;

			int nMFW = _mf.Width;
			int nMFH = _mf.Height;
			int nCW = this.ClientSize.Width;
			if ( nMFW>nCW )
			{
				nMFH = nMFH * nCW/nMFW;
				nMFW = nCW;
			}
			e.Graphics.DrawImage( _mf,
				(nCW-nMFW)/2, 0,
				nMFW, nMFH );
		}

		public void visa( string strPath )
		{
			if ( _mf!=null )
				_mf.Dispose();
			_mf = null;
			if ( strPath!=null )
			{
				string strMF = Path.Combine( strPath, "!fotoorder.emf" );
				if ( System.IO.File.Exists(strMF) )
					_mf = new Metafile( strMF );
			}
			this.Invalidate();
		}

	}

}
