using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace Plata
{
	/// <summary>
	/// Summary description for frmImport.
	/// </summary>
	public class FImport : Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private baseFlikForm m_AktivFlik;
        private readonly vdCamera.vdCamera _camera;

		private PlataDM.Thumbnails _thumbnails;
		private Dictionary<string,vdCamera.imageData> _dicImageData = new Dictionary<string,vdCamera.imageData>();

		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.VScrollBar vsb;
		private System.Windows.Forms.Button cmdSelectAll;
		private Button button1;
		private frmProgress m_progress;

		public struct SelectedImage
		{
			public string FileNameJPG;
            public vdCamera.imageData imgData;
		}

        public FImport(baseFlikForm AktivFlik, vdCamera.vdCamera camera)
		{
			m_AktivFlik = AktivFlik;
			_camera = camera;

			InitializeComponent();

			_thumbnails = m_AktivFlik.FlikKategori==FlikKategori.Porträtt
                ? new PlataDM.Thumbnails( null, Global.Skola, 120, 160, 10 )
                : new PlataDM.Thumbnails( null, Global.Skola, 160, 120, 10 );

			vsb.ValueChanged +=vsb_ValueChanged;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				_thumbnails.Dispose();
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
			this.vsb = new System.Windows.Forms.VScrollBar();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdSelectAll = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// vsb
			// 
			this.vsb.Dock = System.Windows.Forms.DockStyle.Right;
			this.vsb.LargeChange = 1;
			this.vsb.Location = new System.Drawing.Point( 324, 0 );
			this.vsb.Name = "vsb";
			this.vsb.Size = new System.Drawing.Size( 16, 278 );
			this.vsb.TabIndex = 0;
			this.vsb.ValueChanged += new System.EventHandler( this.vsb_ValueChanged );
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point( 236, 244 );
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size( 80, 28 );
			this.cmdCancel.TabIndex = 4;
			this.cmdCancel.Text = "Avbryt";
			// 
			// cmdOK
			// 
			this.cmdOK.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point( 140, 244 );
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size( 80, 28 );
			this.cmdOK.TabIndex = 3;
			this.cmdOK.Text = "OK";
			// 
			// cmdSelectAll
			// 
			this.cmdSelectAll.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.cmdSelectAll.Location = new System.Drawing.Point( 8, 244 );
			this.cmdSelectAll.Name = "cmdSelectAll";
			this.cmdSelectAll.Size = new System.Drawing.Size( 80, 28 );
			this.cmdSelectAll.TabIndex = 5;
			this.cmdSelectAll.Text = "Markera alla";
			this.cmdSelectAll.Click += new System.EventHandler( this.cmdSelectAll_Click );
			// 
			// button1
			// 
			this.button1.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.button1.Location = new System.Drawing.Point( 94, 244 );
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size( 80, 28 );
			this.button1.TabIndex = 6;
			this.button1.Text = "Debug";
			this.button1.Click += new System.EventHandler( this.button1_Click );
			// 
			// FImport
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.ClientSize = new System.Drawing.Size( 340, 278 );
			this.Controls.Add( this.button1 );
			this.Controls.Add( this.cmdSelectAll );
			this.Controls.Add( this.cmdCancel );
			this.Controls.Add( this.cmdOK );
			this.Controls.Add( this.vsb );
			this.KeyPreview = true;
			this.MinimizeBox = false;
			this.Name = "FImport";
			this.Text = "Importera bilder från kameran";
			this.ResumeLayout( false );

		}
		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			var r = SystemInformation.WorkingArea;
			r.Inflate( -r.Width/100, -r.Height/100 );
			this.Location = r.Location;
			this.Size = r.Size;

			m_progress = new frmProgress("Hämtar nya bilder...");
			m_progress.Owner = this;
			m_progress.Show();

			vdUsr.vdOneShotTimer.start( 100, new EventHandler(searchImages), null );
		}

		private void searchImages( object sender, EventArgs e )
		{
			var g = this.CreateGraphics();

			try
			{
				string strPath = Global.getMemoryCardCacheFolder();

			    var imageDatas = _camera.GetImageItems();

				if ( imageDatas==null )
				{
					Global.showMsgBox( this, "Hittar inga bilder i kameran!" );
					this.DialogResult = DialogResult.Cancel;
					return;
				}

				m_progress.setProgress( 0, imageDatas.Length );
				foreach ( var id in imageDatas )
				{
					System.Diagnostics.Debug.Print( id.Name );
					if ( id.hasJPG && (m_AktivFlik.FlikKategori!=FlikKategori.Gruppbild || id.hasRAW) )
					{
						var strFilename = Path.Combine(strPath, id.Name + ".jpg" );
						if ( !File.Exists(strFilename) )
							if ( !id.saveJPG( strFilename ) )
								continue;
						var tn = createThumbnail(
							_thumbnails,
							strFilename,
							160,
							120,
							m_AktivFlik.FlikKategori == FlikKategori.Porträtt );
						if ( tn != null )
						{
							layout();
							_thumbnails.paint( g, null );
							_dicImageData.Add( tn.Key, id );
						}
					}
					m_progress.increaseValue();
				}

			}
			catch ( Exception ex )
			{
				Global.showMsgBox( this, "searchImages:\r\n{0}", ex.ToString() );
			}
			m_progress.Close();
			m_progress.Dispose();
			m_progress = null;
			g.Dispose();
		}

		public static PlataDM.Thumbnail createThumbnail(
			PlataDM.Thumbnails tns,
			string strFilename,
			int nWidth,
			int nHeight,
			bool fIsPortrait )
		{
		    string strSort = null;
		    Bitmap bmp;

		    try
		    {
		        using (var fs = new FileStream(strFilename, FileMode.Open, FileAccess.Read))
		        using (var img = Image.FromStream(fs, true, false))
		        {
		            if (Global.Fotografdator && (img.Width < 1664 || img.Height < 1664))
		                return null;
		            bmp = null;
		            foreach (var pi in img.PropertyItems)
		                switch (pi.Id)
		                {
		                    case 36867:
		                        if (pi.Len == 20 && Global.Preferences.ImageSortOrder != ImageSortOrder.None)
		                            strSort = System.Text.Encoding.ASCII.GetString(pi.Value, 0, pi.Len - 1);
		                        break;
		                    case 20507:
		                        try
		                        {
		                            bmp = (Bitmap) Image.FromStream(new MemoryStream(pi.Value));
		                        }
		                        catch
		                        {
		                        }
		                        break;
		                }
		            if (bmp == null)
		            {
		                var sz = vdUsr.ImgHelper.adaptProportionalSize(
		                    new Size(nWidth, nHeight),
		                    img.Size);
		                bmp = new Bitmap(img, sz.Width, sz.Height);
		            }
		        }

		        if (fIsPortrait && bmp.Width > bmp.Height)
		            bmp.RotateFlip(Global.PorträttRotateFlipType);

		        if (strSort == null)
		        {
		            strSort = Path.GetFileNameWithoutExtension(strFilename);
		            int nTest;
		            if (int.TryParse(strSort, out nTest))
		                strSort = nTest.ToString("00000");
		        }

		        var tn = tns.addImage(strFilename, null, bmp);
		        tn.SortData = strSort;
		        switch (Global.Preferences.ImageSortOrder)
		        {
		            case ImageSortOrder.NewestFirst:
		                tns.Sort(true);
		                break;
		            default:
		                tns.Sort(false);
		                break;
		        }
		        return tn;
		    }
		    catch (Exception ex)
		    {
		        return null;
		    }
		}

	    private void layout()
		{
			_thumbnails.layoutImages();
			int nMax = _thumbnails.MaxScroll;
			if ( nMax>0 )
			{
				vsb.Value = Math.Min( vsb.Value, nMax );
				vsb.Maximum = nMax;
			}
			else
				vsb.Maximum = vsb.Value = 0;
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);
			if ( _thumbnails!=null )
			{
				_thumbnails.layoutImages( 4, 4, vsb.Left-4, this.ClientRectangle.Height );
				layout();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);
			_thumbnails.paint( e.Graphics, null );
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown (e);
			var tn = _thumbnails.hitTest( e.X, e.Y );
		    if (tn == null)
                return;
		    tn.Selected = !tn.Selected;
		    using ( var g = this.CreateGraphics() )
		        tn.paint( g, null, null );
		    if ( e.Clicks==2 )
		    {
		        tn.Selected = true;
		        DialogResult = DialogResult.OK;
		    }
		}

		public SelectedImage[] SelectedImages
		{
			get
			{
				var nSelected = 0;

				foreach ( PlataDM.Thumbnail tn in _thumbnails )
					if ( tn.Selected )
						nSelected++;
				var selectedImages = new SelectedImage[nSelected];
				nSelected = 0;
				foreach ( PlataDM.Thumbnail tn in _thumbnails )
					if (tn.Selected)
					{
					    selectedImages[nSelected] = new SelectedImage
					                                    {
					                                        FileNameJPG = tn.FilenameJpg,
                                                            imgData = (vdCamera.imageData) _dicImageData[tn.Key]
					                                    };
					    nSelected++;
					}
				return selectedImages;
			}
		}

		private void vsb_ValueChanged(object sender, EventArgs e)
		{
			_thumbnails.FirstImage = _thumbnails.ImagesOnOneRow * vsb.Value;
			Invalidate();
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel (e);
			int nVal = vsb.Value - Math.Sign(e.Delta);
			vsb.Value = Math.Max( 0, Math.Min( vsb.Maximum, nVal ) );
		}

		private void cmdSelectAll_Click(object sender, System.EventArgs e)
		{
			foreach ( PlataDM.Thumbnail tn in _thumbnails )
				tn.Selected = true;
			Invalidate();
		}

		private void button1_Click( object sender, EventArgs e )
		{
			foreach ( var x in SelectedImages )
				x.imgData.saveRAW( x.FileNameJPG + ".cr2" );
		}

	}

}
