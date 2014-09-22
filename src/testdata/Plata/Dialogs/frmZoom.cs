using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Photomic.Common;

namespace Plata
{
	/// <summary>
	/// Summary description for frmZoom.
	/// </summary>

	public class frmZoom : Form, IMessageFilter
	{
		private Timer timer1;
		private System.ComponentModel.IContainer components;

		private Bitmap _bmp = null;
		private bool _fRitaSiffror = true;
		private PlataDM.Siffror _siffror = null;
		private Font _fontSiffra = null;
		private System.Windows.Forms.HScrollBar scrThumb;
		private FlikKategori _zoomTyp;
		private ImageAttributes _ia = null;

		private Rectangle _rectFreeForPicture;
		private Rectangle _rectFreeForThumbs;
		private Rectangle _rectAdapted;

		private PlataDM.Thumbnails _tnsMaster;
		private PlataDM.Thumbnails _tns;
		private PlataDM.Thumbnail _tn;

		private string _strTn1;
		private string _strTn2;
		private string _strTnFatSelection;
		private string _strTnGraySelection;
		private System.Windows.Forms.Panel pnlVimmel;
		private System.Windows.Forms.Button cmdAdjust;
		private System.Windows.Forms.CheckBox chkVimmelLovad;
		private System.Windows.Forms.CheckBox chkVimmelVald;
		private System.Windows.Forms.CheckBox chkVimmel÷vrig;
		private PictureBox pictureBox1;
		private CheckBox chkDennaVald;
		private CheckBox chkDennaLovad;
		private Label label2;
		private Label label1;

		private static frmZoom _frmZoom = null;

		private frmZoom()
		{
			InitializeComponent();
		}

		private frmZoom(
			PlataDM.Siffror siffror,
			Font fontSiffra,
			FlikKategori zoomTyp,
			PlataDM.Thumbnails tns,
			string strTn1,
			string strTn2,
			string strTnFatSelection,
			string strTnGraySelection )
		{
			InitializeComponent();

			_siffror = siffror;
			_fontSiffra = fontSiffra;

			pnlVimmel.Visible = zoomTyp==FlikKategori.Vimmel;

			_strTn1 = strTn1;
			_strTn2 = strTn2;
			_strTnFatSelection = strTnFatSelection;
			_strTnGraySelection = strTnGraySelection;
			_tnsMaster = tns;
			_tns = (PlataDM.Thumbnails)tns.Clone();
			_tn = _tns[_strTn1];
			_zoomTyp = zoomTyp;
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
			this.components = new System.ComponentModel.Container();
			this.timer1 = new System.Windows.Forms.Timer( this.components );
			this.scrThumb = new System.Windows.Forms.HScrollBar();
			this.pnlVimmel = new System.Windows.Forms.Panel();
			this.chkVimmel÷vrig = new System.Windows.Forms.CheckBox();
			this.chkVimmelVald = new System.Windows.Forms.CheckBox();
			this.chkVimmelLovad = new System.Windows.Forms.CheckBox();
			this.cmdAdjust = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.chkDennaVald = new System.Windows.Forms.CheckBox();
			this.chkDennaLovad = new System.Windows.Forms.CheckBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.pnlVimmel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// timer1
			// 
			this.timer1.Interval = 150;
			this.timer1.Tick += new System.EventHandler( this.timer1_Tick );
			// 
			// scrThumb
			// 
			this.scrThumb.LargeChange = 1;
			this.scrThumb.Location = new System.Drawing.Point( 248, 8 );
			this.scrThumb.Name = "scrThumb";
			this.scrThumb.Size = new System.Drawing.Size( 32, 16 );
			this.scrThumb.TabIndex = 1;
			this.scrThumb.ValueChanged += new System.EventHandler( this.scrThumb_ValueChanged );
			// 
			// pnlVimmel
			// 
			this.pnlVimmel.BackColor = System.Drawing.SystemColors.Control;
			this.pnlVimmel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pnlVimmel.Controls.Add( this.pictureBox1 );
			this.pnlVimmel.Controls.Add( this.chkDennaVald );
			this.pnlVimmel.Controls.Add( this.chkDennaLovad );
			this.pnlVimmel.Controls.Add( this.label2 );
			this.pnlVimmel.Controls.Add( this.label1 );
			this.pnlVimmel.Controls.Add( this.chkVimmel÷vrig );
			this.pnlVimmel.Controls.Add( this.chkVimmelVald );
			this.pnlVimmel.Controls.Add( this.chkVimmelLovad );
			this.pnlVimmel.Controls.Add( this.cmdAdjust );
			this.pnlVimmel.Location = new System.Drawing.Point( 4, 4 );
			this.pnlVimmel.Name = "pnlVimmel";
			this.pnlVimmel.Size = new System.Drawing.Size( 155, 81 );
			this.pnlVimmel.TabIndex = 0;
			// 
			// chkVimmel÷vrig
			// 
			this.chkVimmel÷vrig.Checked = true;
			this.chkVimmel÷vrig.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkVimmel÷vrig.Location = new System.Drawing.Point( 3, 49 );
			this.chkVimmel÷vrig.Name = "chkVimmel÷vrig";
			this.chkVimmel÷vrig.Size = new System.Drawing.Size( 64, 16 );
			this.chkVimmel÷vrig.TabIndex = 2;
			this.chkVimmel÷vrig.Text = "&÷vriga";
			this.chkVimmel÷vrig.CheckedChanged += new System.EventHandler( this.chkVimmel_CheckedChanged );
			// 
			// chkVimmelVald
			// 
			this.chkVimmelVald.Checked = true;
			this.chkVimmelVald.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkVimmelVald.Location = new System.Drawing.Point( 3, 33 );
			this.chkVimmelVald.Name = "chkVimmelVald";
			this.chkVimmelVald.Size = new System.Drawing.Size( 64, 16 );
			this.chkVimmelVald.TabIndex = 1;
			this.chkVimmelVald.Text = "&Valda";
			this.chkVimmelVald.CheckedChanged += new System.EventHandler( this.chkVimmel_CheckedChanged );
			// 
			// chkVimmelLovad
			// 
			this.chkVimmelLovad.Checked = true;
			this.chkVimmelLovad.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkVimmelLovad.Location = new System.Drawing.Point( 3, 17 );
			this.chkVimmelLovad.Name = "chkVimmelLovad";
			this.chkVimmelLovad.Size = new System.Drawing.Size( 64, 16 );
			this.chkVimmelLovad.TabIndex = 0;
			this.chkVimmelLovad.Text = "&Lovade";
			this.chkVimmelLovad.CheckedChanged += new System.EventHandler( this.chkVimmel_CheckedChanged );
			// 
			// cmdAdjust
			// 
			this.cmdAdjust.Location = new System.Drawing.Point( 82, 49 );
			this.cmdAdjust.Name = "cmdAdjust";
			this.cmdAdjust.Size = new System.Drawing.Size( 62, 23 );
			this.cmdAdjust.TabIndex = 3;
			this.cmdAdjust.Text = "&Justera";
			this.cmdAdjust.Click += new System.EventHandler( this.cmdAdjust_Click );
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 0, 2 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 30, 13 );
			this.label1.TabIndex = 4;
			this.label1.Text = "Visa:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point( 79, 2 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 61, 13 );
			this.label2.TabIndex = 5;
			this.label2.Text = "Aktuell bild:";
			// 
			// chkDennaVald
			// 
			this.chkDennaVald.Location = new System.Drawing.Point( 82, 33 );
			this.chkDennaVald.Name = "chkDennaVald";
			this.chkDennaVald.Size = new System.Drawing.Size( 64, 16 );
			this.chkDennaVald.TabIndex = 7;
			this.chkDennaVald.Text = "Vald";
			this.chkDennaVald.CheckedChanged += new System.EventHandler( this.chkDennaVald_CheckedChanged );
			// 
			// chkDennaLovad
			// 
			this.chkDennaLovad.Location = new System.Drawing.Point( 82, 17 );
			this.chkDennaLovad.Name = "chkDennaLovad";
			this.chkDennaLovad.Size = new System.Drawing.Size( 64, 16 );
			this.chkDennaLovad.TabIndex = 6;
			this.chkDennaLovad.Text = "Lovad";
			this.chkDennaLovad.CheckedChanged += new System.EventHandler( this.chkDennaLovad_CheckedChanged );
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Black;
			this.pictureBox1.Location = new System.Drawing.Point( 72, 3 );
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size( 3, 71 );
			this.pictureBox1.TabIndex = 8;
			this.pictureBox1.TabStop = false;
			// 
			// frmZoom
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size( 292, 266 );
			this.ControlBox = false;
			this.Controls.Add( this.pnlVimmel );
			this.Controls.Add( this.scrThumb );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmZoom";
			this.ShowInTaskbar = false;
			this.Text = "frmZoom";
			this.pnlVimmel.ResumeLayout( false );
			this.pnlVimmel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout( false );

		}
		#endregion

		protected override void OnLoad( EventArgs e )
		{
			this.timer1.Enabled = true;

			Rectangle r = SystemInformation.WorkingArea;
			this.Location = r.Location;
			this.Size = r.Size;

			Application.AddMessageFilter( this );
		}

		protected override void OnClosed( EventArgs e )
		{
			base.OnClosed( e );
			if ( _bmp != null )
				_bmp.Dispose();
			_siffror = null;
			_fontSiffra = null;

			Application.RemoveMessageFilter( this );
		}

		private void loadImage( Bitmap bmp )
		{
			_rectAdapted = vdUsr.ImgHelper.adaptProportionalRect( _rectFreeForPicture, bmp.Size.Width, bmp.Size.Height );
			_bmp = new Bitmap( _rectAdapted.Width, _rectAdapted.Height );
			using ( Graphics g = Graphics.FromImage(_bmp) )
				g.DrawImage(
					bmp,
					new Rectangle( Point.Empty, _rectAdapted.Size ),
					0, 0, bmp.Width, bmp.Height,
					GraphicsUnit.Pixel );
			_tns.KeyboardFocus = _tn;
			Invalidate();

			if ( _zoomTyp == FlikKategori.Vimmel )
			{
				PlataDM.Vimmelbild vb = findCurrentVimmelbild();
				if ( vb!=null )
				{
					chkDennaLovad.Checked = vb.Status == VimmelStatus.Lovad;
					chkDennaVald.Checked = vb.Status == VimmelStatus.Vald;
				}
			}
		}

		private PlataDM.Vimmelbild findCurrentVimmelbild()
		{
			if ( _tn!=null )
				foreach ( PlataDM.Vimmelbild vb in Global.Skola.Vimmel )
					if ( string.Compare( vb.Thumbnail.Key, _tn.Key ) == 0 )
						return vb;
			return null;
		}

		private void loadImage()
		{
			this.Cursor = Cursors.WaitCursor;
			try 
			{
				using ( var bmp = _tn.loadFullImage() )
					loadImage( bmp );
			}
			catch ( Exception ex )
			{
				System.Diagnostics.Debug.WriteLine( ex.Message );
			}
			this.Cursor = Cursors.Default;
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			timer1.Enabled = false;
			loadImage();
		}

		private void drawImage( Graphics g )
		{
			if ( _bmp==null )
				return;

			if ( _ia==null )
				g.DrawImage( _bmp, _rectAdapted.X, _rectAdapted.Y );
			else
				g.DrawImage(
					_bmp,
					_rectAdapted,
					0, 0, _bmp.Width, _bmp.Height,
					GraphicsUnit.Pixel,
					_ia );

			if ( _siffror!=null && _fontSiffra!=null && _fRitaSiffror )
			{
				g.TranslateTransform( _rectAdapted.X, _rectAdapted.Y );
				_siffror.paint( g, _fontSiffra, new Rectangle( Point.Empty, _bmp.Size ), null );
				g.TranslateTransform( -_rectAdapted.X, -_rectAdapted.Y );
			}
			switch ( _zoomTyp )
			{
				case FlikKategori.Gruppbild:
					Global.ritaGruppRam( g, _rectAdapted );
					break;
				case FlikKategori.Portr‰tt:
					Global.ritaPortr‰ttRam( g, _rectAdapted );
					break;
			}
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if ( _bmp==null )
				e.Graphics.FillRectangle( Brushes.Black, e.ClipRectangle );
			else
			{
				var nW = this.ClientRectangle.Width;
				var nH = this.ClientRectangle.Height;
				e.Graphics.FillRectangle( Brushes.Black, 0, 0, nW, _rectAdapted.Top );
				e.Graphics.FillRectangle( Brushes.Black, 0, _rectAdapted.Bottom, nW, nH-_rectAdapted.Bottom );
				e.Graphics.FillRectangle( Brushes.Black, 0, _rectAdapted.Top, _rectAdapted.Left, _rectAdapted.Height );
				e.Graphics.FillRectangle( Brushes.Black, _rectAdapted.Right, _rectAdapted.Top, nW-_rectAdapted.Right, _rectAdapted.Height );
				drawImage( e.Graphics );
			}

			if ( _tns!=null )
				_tns.paintWithSelections(
                    e.Graphics,
					_tn!=null ? new List<string> {_tn.Key} : new List<string>(),
					_strTnFatSelection,
					_strTnGraySelection );
		}

		private void resize()
		{
			var sh = SystemInformation.HorizontalScrollBarHeight;
			var sz = this.ClientSize;
			scrThumb.Bounds = new Rectangle( sz.Width-8-sh*2, sz.Height-8-sh, sh*2, sh );

			if ( _tns==null )
				return;

			switch ( _zoomTyp )
			{
				case FlikKategori.Gruppbild:
				case FlikKategori.Vimmel:
				case FlikKategori.Annat:
					_tns.layoutImages(
						8,
						sz.Height-_tns.Height-8,
						scrThumb.Left-8 );
					var nThH = _tns.Height+16;
					_rectFreeForPicture = new Rectangle( 0, 0, sz.Width, sz.Height-nThH );
					_rectFreeForThumbs = new Rectangle( 0, sz.Height-nThH, sz.Width, nThH );
					break;
				case FlikKategori.Portr‰tt:
					_tns.layoutImages( sz.Width/2, 0, sz.Width, sz.Height );
					_rectFreeForPicture = new Rectangle( 0, 0, sz.Width/2, sz.Height );
					_rectFreeForThumbs = new Rectangle( sz.Width/2, 0, sz.Width/2, sz.Height );
					break;
			}
			if ( _tns.MaxScroll==0 && scrThumb.Visible )
			{
				scrThumb.Visible = false;
				_tns.FirstImage = 0;
				scrThumb.Value = 0;
			}
			else if ( _tns.MaxScroll!=0 && !scrThumb.Visible )
				scrThumb.Visible = true;
			scrThumb.Maximum = _tns.MaxScroll;
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);
			resize();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
		    base.OnMouseDown(e);
		    var tn = _tns.hitTest(e.X, e.Y);
            if (tn == null)
            {
                if ( e.Button == MouseButtons.Right )
                    DialogResult = DialogResult.Cancel;
                return;
            }
		    var fNew = _tn != tn;
		    _tn = tn;
		    switch (e.Clicks)
		    {
		        case 1:
		            if (fNew)
		                loadImage();
		            break;
		        case 2:
		            DialogResult = DialogResult.OK;
		            break;
		    }
		}

	    private bool handleKeys(Keys key)
		{
			switch ( key )
			{
				case Keys.Left:
					_tns.moveKeyboardFocus( -1 );
					break;
				case Keys.Right:
					_tns.moveKeyboardFocus( 1 );
					break;
				case Keys.Up:
					_tns.moveKeyboardFocus( -Math.Max(-1,_tns.ImagesOnOneRow) );
					break;
				case Keys.Down:
					_tns.moveKeyboardFocus( Math.Max(1,_tns.ImagesOnOneRow) );
					break;
				case Keys.F4:
					_fRitaSiffror ^= true;
					Invalidate();
					return false;
				case Keys.F5:
					if ( ModifierKeys==Keys.Control && _tn != null )
						using ( var dlg = new FKollaSkarpa( _tn ) )
							dlg.ShowDialog(this);
					break;
				case Keys.Escape:
					this.DialogResult = DialogResult.Cancel;
					return true;
				case Keys.Enter:
					this.DialogResult = DialogResult.OK;
					return true;
				default:
					return false;
			}

			_tn = _tns.KeyboardFocus;
			Invalidate( _rectFreeForThumbs );
			timer1.Enabled = false;
			timer1.Enabled = true;
			return true;
		}

		private void adjustmentChangeCallback( object sender, ImageAttributes ia )
		{
			_ia = ia;
			Invalidate( _rectAdapted );
		}

	    protected override void OnMouseWheel(MouseEventArgs e)
		{
			if ( e.Delta>0 && scrThumb.Value>0 )
				scrThumb.Value--;
			else if ( e.Delta<0 && scrThumb.Value<scrThumb.Maximum )
				scrThumb.Value++;
		}

		private void scrThumb_ValueChanged(object sender, EventArgs e)
		{
			switch ( _zoomTyp )
			{
				case FlikKategori.Gruppbild:
				case FlikKategori.Vimmel:
					_tns.FirstImage = scrThumb.Value;
					break;
				case FlikKategori.Portr‰tt:
					_tns.FirstImage = scrThumb.Value * _tns.ImagesOnOneRow;
					break;
			}
			Invalidate( _rectFreeForThumbs );
		}

		static public bool isVisible
		{
			get { return _frmZoom!=null; }
		}

		static public void gotNewImage( PlataDM.Thumbnail tn, Bitmap bmpFullSize )
		{
			if ( _frmZoom==null )
				return;
			_frmZoom._tn = _frmZoom._tns.add( (PlataDM.Thumbnail)tn.Clone() );
			_frmZoom.loadImage( bmpFullSize );
			_frmZoom.resize();
			_frmZoom._tns.ensureVisible( _frmZoom._tn );
		}

		static public PlataDM.Thumbnail showDialog(
			Form parent,
			PlataDM.Siffror siffror,
			Font fontSiffra,
			FlikKategori zoomTyp,
			PlataDM.Thumbnails tns,
			string strTn1,
			string strTn2,
			string strTnFatSelection,
			string strTnGraySelection )
		{
			PlataDM.Thumbnail tnResult = null;

			_frmZoom = new frmZoom(
				siffror,
				fontSiffra,
				zoomTyp,
				tns,
				strTn1,
				strTn2,
				strTnFatSelection,
				strTnGraySelection );
			if ( _frmZoom.ShowDialog(parent) == DialogResult.OK )
				tnResult = _frmZoom._tn;
			_frmZoom.Dispose();
			_frmZoom = null;
			return tnResult;
		}

		private void chkVimmel_CheckedChanged(object sender, System.EventArgs e)
		{
			_tns = (PlataDM.Thumbnails)_tnsMaster.Clone();
			foreach ( PlataDM.Vimmelbild vb in Global.Skola.Vimmel )
			{
				bool fOK;
				switch ( vb.Status )
				{
					case VimmelStatus.Lovad:
						fOK = chkVimmelLovad.Checked;
						break;
					case VimmelStatus.Normal:
						fOK = chkVimmel÷vrig.Checked;
						break;
					case VimmelStatus.Vald:
						fOK = chkVimmelVald.Checked;
						break;
					default:
						fOK = false;
						break;
				}
				if ( !fOK )
				{
					PlataDM.Thumbnail tn = _tns[vb.Thumbnail.Key];
					if ( tn!=null )
						_tns.Remove( tn );
				}
			}
			resize();
			Invalidate();
		}

		private void cmdAdjust_Click( object sender, EventArgs e )
		{
			if ( _bmp == null || _zoomTyp != FlikKategori.Vimmel )
				return;
			Global.showMsgBox( this, "Vi har tagit bort denna funktion eftersom bildsk‰rmarna i fotografdatorerna inte Âterger f‰rger sÂ bra att du kan lita pÂ de ‰ndringar du gˆr h‰r. :-(" );
			//if ( FImageAdjust.showDialog( this, new FImageAdjust.AdjustmentChange( adjustmentChangeCallback ) ) == DialogResult.OK )
			//  createReadjustedImage();
			//_ia = null;
			//this.Invalidate();
		}


		bool IMessageFilter.PreFilterMessage( ref Message m )
		{
			const int WM_KEYDOWN = 0x100;
			if ( m.Msg == WM_KEYDOWN && Form.ActiveForm==this )
				return handleKeys( (Keys)(int)m.WParam & Keys.KeyCode );
			else
				return false;
		}

		private void chkDennaLovad_CheckedChanged( object sender, EventArgs e )
		{
			if ( chkDennaLovad.Checked )
				chkDennaVald.Checked = false;
			changeVimmelStatus();
		}

		private void chkDennaVald_CheckedChanged( object sender, EventArgs e )
		{
			if ( chkDennaVald.Checked )
				chkDennaLovad.Checked = false;
			changeVimmelStatus();
		}

		private void changeVimmelStatus()
		{
			PlataDM.Vimmelbild vb = findCurrentVimmelbild();
			if ( vb == null )
				return;
			if ( chkDennaVald.Checked )
				vb.Status = VimmelStatus.Vald;
			else if ( chkDennaLovad.Checked )
				vb.Status = VimmelStatus.Lovad;
			else
				vb.Status = VimmelStatus.Normal;
		}

	}

}
