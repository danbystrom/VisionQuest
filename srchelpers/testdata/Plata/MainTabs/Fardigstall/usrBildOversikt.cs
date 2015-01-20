using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using PlataDM;

namespace Plata
{
	public partial class usrBildÖversikt : UserControl
	{
		protected class Item
		{
            public Thumbnail Thumbnail;
            
            public string LoResFile;
			public string HiResFile;
            public object Data;
            public string Text;
            public bool LineBreak;
        }

		protected Thumbnails _tns;
        protected readonly Dictionary<string, Item> _dicTnKeyToItem = new Dictionary<string, Item>();

		private readonly Dictionary<string, Image> _imageChache = new Dictionary<string, Image>();
		protected readonly Queue<Item> _queItem = new Queue<Item>();

		protected FlikKategori _flikKategori;
		private int _leftMarginal;
		private int _thumbnailWidth;

		public usrBildÖversikt()
		{
			InitializeComponent();
		}

		private void setProgress( string s )
		{
		}

		public void initialize(
			FlikKategori flikKategori,
			int leftMarginal )
		{
			_flikKategori = flikKategori;
			_leftMarginal = leftMarginal;
			reset();
		}

		public void reset()
		{
		    _dicTnKeyToItem.Clear();
		    _queItem.Clear();

		    reload();

		    var nAvailableWidth =
		        SystemInformation.WorkingArea.Width -
		        _leftMarginal -
		        SystemInformation.VerticalScrollBarWidth -
		        30;
		    var nImagesInOneRow = trackBar1.Value;
		    if (_flikKategori == FlikKategori.Porträtt)
		        nImagesInOneRow *= 2;
		    _thumbnailWidth = nAvailableWidth/nImagesInOneRow - 5;

		    _tns = new Thumbnails(null, Global.Skola, _thumbnailWidth, 1000, 7);
            Update();
		}

	    protected void addItem( string loResFile, string hiResFile, object data, string text, bool lineBreak )
		{
			if ( loResFile == null )
				return;
			_queItem.Enqueue( new Item()
			{
				LoResFile = loResFile,
				HiResFile = hiResFile,
				Data = data,
                Text = text,
                LineBreak = lineBreak
			} );
		}

		protected override void OnDoubleClick( EventArgs e )
		{
			var p = this.PointToClient( MousePosition );
			visaFullskärm( _tns.hitTest( p.X, p.Y ) );
		}

		protected void visaFullskärm( Thumbnail tn )
		{
			frmZoom.showDialog(
				this.FindForm(),
				null,
				null,
				_flikKategori,
				_tns,
				tn != null ? tn.Key : null,
				null,
				null,
				null );
		}

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_tns == null)
                return;
            tmrLoadPictures.Enabled = _queItem.Count != 0;
            _tns.paint(e.Graphics, null);
        }

        private void layout()
        {
            var sz = ClientSize;
            var sw = SystemInformation.VerticalScrollBarWidth;

            trackBar1.Location = new Point(
                sz.Width - trackBar1.Width,
                sz.Height - trackBar1.Height*2/3);
            vsb.Bounds = new Rectangle(sz.Width - sw, 0, sw, trackBar1.Top);

            _tns.layoutImages(_leftMarginal, 5, sz.Width - sw - 5, ClientSize.Height);
            _tns.layoutImages();

            var nMax = Math.Max(vsb.Maximum, _tns.MaxScroll);
            if (nMax > 0)
            {
                vsb.Visible = true;
                vsb.Value = Math.Min(vsb.Value, nMax);
                vsb.Maximum = nMax;
            }
            else
                vsb.Visible = false;
        }

	    protected override void OnResize( EventArgs e )
		{
			base.OnResize( e );
			if ( _tns != null )
				layout();
		}

		private void vsb_ValueChanged( object sender, EventArgs e )
		{
			_tns.FirstImage = _tns.ImagesOnOneRow * vsb.Value;
			Invalidate();
		}

		protected override void OnMouseWheel( MouseEventArgs e )
		{
			base.OnMouseWheel( e );
			var nVal = vsb.Value - Math.Sign( e.Delta );
			vsb.Value = Math.Max( 0, Math.Min( vsb.Maximum, nVal ) );
		}

		private void tmrLoadPictures_Tick( object sender, EventArgs e )
		{
		    var fGoToNextOne = true;
		    var nMaxIterations = 10;

            while (fGoToNextOne && _queItem.Count != 0 && --nMaxIterations > 0)
		    {
		        setProgress(string.Format("tmrLoadPictures_Tick {0} {1}", _queItem.Count, nMaxIterations));

		        var item = _queItem.Dequeue();

		        Image img;
		        if (!_imageChache.TryGetValue(item.LoResFile, out img))
		        {
		            try
		            {
                        using (var imgT = Image.FromFile(item.LoResFile))
                            img = PlataDM.Global.createAdaptedBitmap(imgT, _thumbnailWidth, 1000);
                    }
		            catch
		            {
		                img = new Bitmap(_thumbnailWidth, _thumbnailWidth*384/256);
                        using( var g = Graphics.FromImage(img))
                            g.Clear(Color.LightCoral);
		            }
		            _imageChache.Add(item.LoResFile, img);
		            fGoToNextOne = false;

		            if (!string.IsNullOrEmpty(item.Text))
		                using (var g = Graphics.FromImage(img))
		                {
		                    var r = new Rectangle(Point.Empty, img.Size);
		                    g.DrawString(item.Text, this.Font, Brushes.Black, r, vdUsr.Util.sfLL);
		                    r.Offset(-1, -1);
		                    g.DrawString(item.Text, this.Font, Brushes.White, r, vdUsr.Util.sfLL);
		                }
		        }

		        var tn = _tns.addImage(
		            item.HiResFile,
		            null,
		            img);
		        tn.BeginLayoutGroup = item.LineBreak;
		        item.Thumbnail = tn;
		        _dicTnKeyToItem.Add(tn.Key, item);
		    }
            layout();
            Invalidate();

		    tmrLoadPictures.Enabled = _queItem.Count != 0;
		    setProgress("");
		}

	    private void trackBar1_Scroll( object sender, EventArgs e )
		{
			_tns.Dispose();
			_tns = null;
			foreach ( var img in _imageChache.Values )
				img.Dispose();
			_imageChache.Clear();
			reset();
		}

		public virtual void reload()
		{
            _queItem.Clear();
			Invalidate();
		}

        private void tmrInvalidate_Tick(object sender, EventArgs e)
        {
            tmrInvalidate.Stop();
            Invalidate();
        }

	}

}
