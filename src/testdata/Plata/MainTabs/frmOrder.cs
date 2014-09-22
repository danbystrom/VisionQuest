using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;

namespace Plata
{
	public class frmOrder : Plata.baseFlikForm
	{
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.VScrollBar vsc;
		private System.Windows.Forms.Button cmdAnnoyingButImportant;

		private Metafile _mf;

		public frmOrder( Form parent ) : base( parent, FlikTyp.Order )
		{
			InitializeComponent();
			this.Bounds = parent.ClientRectangle;
			this.PerformLayout();
			_strCaption = "ORDER";
			cmdAnnoyingButImportant.Left = -1000;
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
			this.vsc = new System.Windows.Forms.VScrollBar();
			this.cmdAnnoyingButImportant = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// vsc
			// 
			this.vsc.LargeChange = 1;
			this.vsc.Location = new System.Drawing.Point(536, 56);
			this.vsc.Maximum = 3;
			this.vsc.Name = "vsc";
			this.vsc.Size = new System.Drawing.Size(20, 252);
			this.vsc.TabIndex = 1;
			this.vsc.Visible = false;
			this.vsc.ValueChanged += new System.EventHandler(this.vsc_ValueChanged);
			// 
			// cmdAnnoyingButImportant
			// 
			this.cmdAnnoyingButImportant.Location = new System.Drawing.Point(36, 32);
			this.cmdAnnoyingButImportant.Name = "cmdAnnoyingButImportant";
			this.cmdAnnoyingButImportant.Size = new System.Drawing.Size(24, 20);
			this.cmdAnnoyingButImportant.TabIndex = 0;
			// 
			// frmOrder
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(892, 566);
			this.Controls.Add(this.cmdAnnoyingButImportant);
			this.Controls.Add(this.vsc);
			this.KeyPreview = true;
			this.Name = "frmOrder";
			this.ResumeLayout(false);

		}
		#endregion

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			if ( _mf==null )
				base.OnPaintBackground (pevent);
			else
			{
				int nMFW = _mf.Width;
				int nCW = this.ClientSize.Width - (vsc.Visible ? SystemInformation.VerticalScrollBarWidth : 0);
				if ( nMFW<nCW )
				{
					pevent.Graphics.FillRectangle( Brushes.White, (nCW-nMFW)/2, 0, nMFW, this.ClientRectangle.Height );
					paintBackground( pevent.Graphics, new Rectangle( 0, 0, (nCW-nMFW)/2, this.ClientRectangle.Height ) );
					paintBackground( pevent.Graphics, new Rectangle( (nCW+nMFW)/2, 0, (nCW-nMFW)/2+30, this.ClientRectangle.Height ) );
				}
				else
					pevent.Graphics.FillRectangle( Brushes.White, pevent.ClipRectangle );
			}
		}

		protected override void paint(PaintEventArgs e)
		{
			if ( _mf==null )
				paintFallback( e );
			else
			{
				int nMFW = _mf.Width;
				int nMFH = _mf.Height;
				int nCW = this.ClientSize.Width - (vsc.Visible ? SystemInformation.VerticalScrollBarWidth : 0);
				if ( nMFW>nCW )
				{
					nMFH = nMFH * nCW/nMFW;
					nMFW = nCW;
				}
				e.Graphics.DrawImage( _mf,
					(nCW-nMFW)/2, -vsc.Value*(nMFH-this.ClientSize.Height)/vsc.Maximum,
					nMFW, nMFH );
			}
		}

		private void paintFallback(PaintEventArgs e)
		{
			const int BoxX = 50;
			const int BoxW = 300;
			const int BoxH = 105;
			const int Box1Y = 50;
			const int Box2Y = 190;
			const int TextSpace = 18;

			using ( Font f = new Font(this.Font.Name,12,FontStyle.Bold) )
			{
				Util.paintBox( e.Graphics, "PRODUKTIONSORDER", f, BoxX, Box1Y, BoxW, BoxH );
				Util.paintBox( e.Graphics, "PRODUKTER", f, BoxX, Box2Y, BoxW, BoxH );
				Util.paintBox( e.Graphics, "ANMÄRKNINGAR", f, BoxX+BoxW+50, Box1Y, BoxW+BoxW/2, Box2Y+BoxH-Box1Y );
			}

			using ( Font f = new Font(this.Font.Name,10) )
			{
				int nY = Box1Y + 5;
				e.Graphics.DrawString( "Ordernr:"  , f, Brushes.Black, BoxX+5, nY+=TextSpace );
				e.Graphics.DrawString( "Skola:"    , f, Brushes.Black, BoxX+5, nY+=TextSpace );
				e.Graphics.DrawString( "Ort:"      , f, Brushes.Black, BoxX+5, nY+=TextSpace );
				e.Graphics.DrawString( "Säljare:"  , f, Brushes.Black, BoxX+5, nY+=TextSpace );

				nY = Box2Y + 5;
				e.Graphics.DrawString( "Katalog:"  , f, Brushes.Black, BoxX+5, nY+=TextSpace );
				e.Graphics.DrawString( "Porträtt:" , f, Brushes.Black, BoxX+5, nY+=TextSpace );
				e.Graphics.DrawString( "Gruppbild:", f, Brushes.Black, BoxX+5, nY+=TextSpace );
				e.Graphics.DrawString( "Arkiv:"    , f, Brushes.Black, BoxX+5, nY+=TextSpace );
			}

			if ( Global.Skola == null )
				return;

			using ( Font f = new Font(this.Font.Name,10,FontStyle.Bold) )
			{
				int nY = Box1Y + 5;
				e.Graphics.DrawString( Global.Skola.OrderNr, f, Brushes.Black, BoxX+70, nY+=TextSpace );
				e.Graphics.DrawString( Global.Skola.Namn   , f, Brushes.Black, BoxX+70, nY+=TextSpace );
				e.Graphics.DrawString( Global.Skola.Ort    , f, Brushes.Black, BoxX+70, nY+=TextSpace );

				nY = Box2Y + 5;
//				e.Graphics.DrawString( "Katalog:"  , f, Brushes.Black, BoxX+70, nY+=TextSpace );
//				e.Graphics.DrawString( "Porträtt:" , f, Brushes.Black, BoxX+70, nY+=TextSpace );
//				e.Graphics.DrawString( "Gruppbild:", f, Brushes.Black, BoxX+70, nY+=TextSpace );
//				e.Graphics.DrawString( "Akriv:"    , f, Brushes.Black, BoxX+70, nY+=TextSpace );
			}

		}

		public override void skolaUppdaterad()
		{
			if ( _mf!=null )
				_mf.Dispose();
			string strMF = Global.Skola.HomePathCombine( "!fotoorder.emf" );
			if ( System.IO.File.Exists(strMF) )
				_mf = new Metafile( strMF );
			else
				_mf = null;
			resize2(this.ClientSize);
			this.Invalidate();
		}

		protected override void resize(Size sz)
		{
			base.resize( sz );
			resize2(sz);
		}

		private void resize2(Size sz)
		{
			base.resize(sz);
			vsc.Value = 0;
			if (_mf != null && _mf.Height > sz.Height)
			{
				int sw = SystemInformation.VerticalScrollBarWidth;
				Rectangle rect = new Rectangle((sz.Width - sw + _mf.Width) / 2, 0, sw, sz.Height);
				if (rect.Right > sz.Width)
					rect.X = sz.Width - sw;
				vsc.Bounds = rect;
				vsc.Visible = true;
			}
			else
				vsc.Visible = false;
		}

		private void vsc_ValueChanged(object sender, System.EventArgs e)
		{
			this.Invalidate();
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel (e);
			if ( e.Delta>0 )
				vsc.Value = Math.Max( vsc.Minimum, vsc.Value-1 );
			else
				vsc.Value = Math.Min( vsc.Maximum, vsc.Value+1 );
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if ( vsc.Focused )
				return;
			switch ( e.KeyCode )
			{
				case Keys.Up:
					vsc.Value = Math.Max( vsc.Minimum, vsc.Value-1 );
					return;
				case Keys.Down:
					vsc.Value = Math.Min( vsc.Maximum, vsc.Value+1 );
					return;
				case Keys.PageUp:
					vsc.Value = vsc.Minimum;
					return;
				case Keys.PageDown:
					vsc.Value = vsc.Maximum;
					return;
			}
		}

	}

}
