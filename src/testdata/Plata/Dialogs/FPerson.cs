using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Plata
{
	/// <summary>
	/// Summary description for FPerson.
	/// </summary>

	public class FPerson : Form
	{
		private Timer timer1;
		private System.ComponentModel.IContainer components;

		private Bitmap m_bmp = null;
		private FlikKategori _zoomTyp;

		private Rectangle _rectFreeForPicture;
		private Rectangle _rectFreeForThumbs;
		private Rectangle _rectAdapted;

		private PlataDM.Thumbnails _tns;
		private PlataDM.Thumbnail _tn;

		private string _strTn1;
		private string _strTn2;
		private vdUsr.TaskPane taskPane1;
		private vdUsr.TaskPaneGroup tpgThumbs;
		private vdUsr.TaskPaneGroup tpgPUpg;

		private static FPerson _frmZoom = null;

		private FPerson()
		{
			InitializeComponent();
		}

		private FPerson(
			FlikKategori zoomTyp,
			PlataDM.Thumbnails tns )
		{
			_tns = (PlataDM.Thumbnails)tns.Clone();
			_tn = _tns[_strTn1];
			_zoomTyp = zoomTyp;
			InitializeComponent();
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
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.taskPane1 = new vdUsr.TaskPane();
			this.tpgThumbs = new vdUsr.TaskPaneGroup();
			this.tpgPUpg = new vdUsr.TaskPaneGroup();
			this.taskPane1.SuspendLayout();
			this.SuspendLayout();
			// 
			// timer1
			// 
			this.timer1.Interval = 150;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// taskPane1
			// 
			this.taskPane1.Caption = null;
			this.taskPane1.Controls.Add(this.tpgPUpg);
			this.taskPane1.Controls.Add(this.tpgThumbs);
			this.taskPane1.Dock = System.Windows.Forms.DockStyle.Left;
			this.taskPane1.Location = new System.Drawing.Point(0, 0);
			this.taskPane1.Name = "taskPane1";
			this.taskPane1.Size = new System.Drawing.Size(276, 584);
			this.taskPane1.TabIndex = 0;
			// 
			// tpgThumbs
			// 
			this.tpgThumbs.Caption = "Bilder";
			this.tpgThumbs.Image = null;
			this.tpgThumbs.Location = new System.Drawing.Point(4, 179);
			this.tpgThumbs.Name = "tpgThumbs";
			this.tpgThumbs.Size = new System.Drawing.Size(268, 100);
			this.tpgThumbs.TabIndex = 0;
			// 
			// tpgPUpg
			// 
			this.tpgPUpg.Caption = "Personuppgifter";
			this.tpgPUpg.Image = null;
			this.tpgPUpg.Location = new System.Drawing.Point(4, 27);
			this.tpgPUpg.Name = "tpgPUpg";
			this.tpgPUpg.Size = new System.Drawing.Size(268, 125);
			this.tpgPUpg.TabIndex = 1;
			// 
			// frmZoom
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(796, 584);
			this.ControlBox = false;
			this.Controls.Add(this.taskPane1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmZoom";
			this.ShowInTaskbar = false;
			this.Text = "frmZoom";
			this.Load += new System.EventHandler(this.frmZoom_Load);
			this.taskPane1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void frmZoom_Load(object sender, System.EventArgs e)
		{
			this.timer1.Enabled = true;

			Rectangle r = SystemInformation.WorkingArea;
			this.Location = r.Location;
			this.Size = r.Size;
		}

		private void loadImage( Bitmap bmp )
		{
			if ( _zoomTyp==FlikKategori.Porträtt )
				bmp.RotateFlip( Global.PorträttRotateFlipType );
			_rectAdapted = Global.adaptRect( _rectFreeForPicture, bmp.Size.Width, bmp.Size.Height );
			m_bmp = new Bitmap( bmp, _rectAdapted.Width, _rectAdapted.Height );
			_tns.KeyboardFocus = _tn;
			Invalidate();
		}

		private void loadImage()
		{
			this.Cursor = Cursors.WaitCursor;
			try 
			{
				using( Bitmap bmp = (Bitmap)Bitmap.FromFile(_tn.FilenameJPG) )
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

		protected override void OnPaint(PaintEventArgs e)
		{
			if ( m_bmp!=null )
			{
				e.Graphics.DrawImage( m_bmp, _rectAdapted.X, _rectAdapted.Y );
				switch ( _zoomTyp )
				{
					case FlikKategori.Gruppbild:
						Global.ritaGruppRam( e.Graphics, _rectAdapted );
						break;
					case FlikKategori.Porträtt:
						Global.ritaPorträttRam( e.Graphics, _rectAdapted );
						break;
				}
			}
			if ( _tns!=null )
				_tns.paintWithSelections( e.Graphics,
					_tn!=null ? _tn.Key : null,
					_strTn2,
					null,
					null,
					null );
		}

		private void resize()
		{
			int sh = SystemInformation.HorizontalScrollBarHeight;
			Size sz = this.ClientSize;

			if ( _tns==null )
				return;

			switch ( _zoomTyp )
			{
				case FlikKategori.Porträtt:
					_tns.layoutImages( sz.Width/2, 0, sz.Width, sz.Height );
					_rectFreeForPicture = new Rectangle( 0, 0, sz.Width/2, sz.Height );
					_rectFreeForThumbs = new Rectangle( sz.Width/2, 0, sz.Width/2, sz.Height );
					break;
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);
			resize();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown (e);
			PlataDM.Thumbnail tn = _tns.hitTest( e.X, e.Y );
			if ( tn!=null )
			{
				bool fNew = _tn!=tn;
				_tn = tn;
				switch ( e.Clicks )
				{
					case 1:
						if ( fNew )
							loadImage();
						break;
					case 2:
						this.DialogResult = DialogResult.OK; break;
				}
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown (e);
			switch ( e.KeyCode )
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
				case Keys.F5:
					if ( e.Modifiers==Keys.Control && _tn!=null )
						using ( frmKollaSkärpa dlg = new frmKollaSkärpa( _tn, _zoomTyp ) )
							dlg.ShowDialog(this);
					break;
				case Keys.Escape:
					e.Handled = true;
					this.Close();
					return;
				default:
					return;
			}

			e.Handled = true;
			_tn = _tns.KeyboardFocus;
			Invalidate( _rectFreeForThumbs );
			timer1.Enabled = false;
			timer1.Enabled = true;
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed (e);
			if ( m_bmp!=null )
				m_bmp.Dispose();
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
			FlikKategori zoomTyp,
			PlataDM.Thumbnails tns )
		{
			PlataDM.Thumbnail tnResult = null;

			_frmZoom = new FPerson(
				zoomTyp,
				tns );
			if ( _frmZoom.ShowDialog(parent) == DialogResult.OK )
				tnResult = _frmZoom._tn;
			_frmZoom.Dispose();
			_frmZoom = null;
			return tnResult;
		}

	}

}
