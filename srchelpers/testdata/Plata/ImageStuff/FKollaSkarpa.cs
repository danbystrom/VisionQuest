using System;
using System.Drawing;
using System.Windows.Forms;

namespace Plata
{
	/// <summary>
	/// Summary description for frmZoom.
	/// </summary>

	public class FKollaSkarpa : Form
	{
		private Timer timer1;
		private System.ComponentModel.IContainer components;

		private Bitmap _bmp = null;
		private readonly PlataDM.Thumbnail _tn;
		private int _nDragX, _nDragY, _nScrollX, _nScrollY;

		private FKollaSkarpa()
		{
			InitializeComponent();
		}

		public FKollaSkarpa( PlataDM.Thumbnail tn )
		{
			_tn = tn;
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
			this.SuspendLayout();
			// 
			// timer1
			// 
			this.timer1.Interval = 150;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// frmZoom
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.ControlBox = false;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FKollaSkarpa";
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);

		}
		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			this.timer1.Enabled = true;

			var r = SystemInformation.WorkingArea;
			this.Location = r.Location;
			this.Size = r.Size;
		}

		private void loadImage()
		{
			this.Cursor = Cursors.WaitCursor;
			try 
			{
				_bmp = _tn.loadFullImage();
				_nScrollX = (this.ClientSize.Width-_bmp.Size.Width)/2;
				_nScrollY = (this.ClientSize.Height-_bmp.Size.Height)/2;
			    scrollPicture(0, 0);
				Invalidate();
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
		    if (_bmp == null)
                return;
		    var sz = this.ClientSize;
		    var rectDest = new Rectangle( _nScrollX, _nScrollY, _bmp.Size.Width, _bmp.Size.Height );
		    if ( _nScrollX>0 )
		        e.Graphics.FillRectangle( Brushes.Black, 0, 0, _nScrollX, sz.Height );
		    if ( rectDest.Right<sz.Width )
		        e.Graphics.FillRectangle( Brushes.Black, rectDest.Right, 0, sz.Width-rectDest.Right, sz.Height );
		    e.Graphics.DrawImage( _bmp, rectDest, 0, 0, _bmp.Size.Width, _bmp.Size.Height, GraphicsUnit.Pixel );
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown (e);
			_nDragX  = e.X;
			_nDragY  = e.Y;
		}

		private void scrollPicture( int dx, int dy )
		{
			_nScrollX += dx;
			_nScrollY += dy;
			if ( _nScrollX < this.ClientSize.Width-_bmp.Size.Width )
				_nScrollX = this.ClientSize.Width-_bmp.Size.Width;
            if (_nScrollX > 0)
                _nScrollX = 0;
			if ( _nScrollY < this.ClientSize.Height-_bmp.Size.Height )
				_nScrollY = this.ClientSize.Height-_bmp.Size.Height;
            if (_nScrollY > 0)
                _nScrollY = 0;
            Invalidate();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove (e);
			if ( e.Button==MouseButtons.Left )
			{
				scrollPicture( (e.X-_nDragX)*2, (e.Y-_nDragY)*2 );
				_nDragX  = e.X;
				_nDragY  = e.Y;
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown (e);
			switch ( e.KeyCode )
			{
				case Keys.Up:
					scrollPicture( 0, 32 );
					break;
				case Keys.Down:
					scrollPicture( 0, -32 );
					break;
				case Keys.Left:
					scrollPicture( 32, 0 );
					break;
				case Keys.Right:
					scrollPicture( -32, 0 );
					break;
				default:
					this.Close();
					break;
			}
			e.Handled = true;
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed (e);
			if ( _bmp!=null )
				_bmp.Dispose();
		}

	}

}
