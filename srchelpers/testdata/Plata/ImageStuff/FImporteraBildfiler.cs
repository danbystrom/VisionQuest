using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Plata.Camera;
using PlataDM;

namespace Plata.ImageStuff
{
    /// <summary>
    /// Summary description for frmImport.
    /// </summary>
    public class FImporteraBildfiler : vdUsr.baseGradientForm
    {
        private System.ComponentModel.IContainer components;

        private baseFlikForm _aktivFlik;
        private string _strPath;

        private string[] _jpgFiles;
        private int _nImageLoadIndex;

        private PlataDM.Thumbnails _thumbnails;

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdSelectAll;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.VScrollBar vsb;
        private System.Windows.Forms.ProgressBar pbr;
        private System.Windows.Forms.Button cmdSelectFolder;
        private bool _requireRaw;

        private FImporteraBildfiler()
        {
            InitializeComponent();
            _strPath = Global.Preferences.LastImportFolder;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _thumbnails.Dispose();
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdSelectAll = new System.Windows.Forms.Button();
            this.cmdSelectFolder = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.vsb = new System.Windows.Forms.VScrollBar();
            this.pbr = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(600, 388);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(80, 28);
            this.cmdCancel.TabIndex = 4;
            this.cmdCancel.Text = "Avbryt";
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(504, 388);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(80, 28);
            this.cmdOK.TabIndex = 3;
            this.cmdOK.Text = "OK";
            // 
            // cmdSelectAll
            // 
            this.cmdSelectAll.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdSelectAll.Location = new System.Drawing.Point(8, 388);
            this.cmdSelectAll.Name = "cmdSelectAll";
            this.cmdSelectAll.Size = new System.Drawing.Size(80, 28);
            this.cmdSelectAll.TabIndex = 5;
            this.cmdSelectAll.Text = "Markera alla";
            this.cmdSelectAll.Click += new System.EventHandler(this.cmdSelectAll_Click);
            // 
            // cmdSelectFolder
            // 
            this.cmdSelectFolder.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdSelectFolder.Location = new System.Drawing.Point(104, 388);
            this.cmdSelectFolder.Name = "cmdSelectFolder";
            this.cmdSelectFolder.Size = new System.Drawing.Size(80, 28);
            this.cmdSelectFolder.TabIndex = 6;
            this.cmdSelectFolder.Text = "Byt mapp...";
            this.cmdSelectFolder.Click += new System.EventHandler(this.cmdSelectFolder_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // vsb
            // 
            this.vsb.Dock = System.Windows.Forms.DockStyle.Right;
            this.vsb.LargeChange = 1;
            this.vsb.Location = new System.Drawing.Point(688, 0);
            this.vsb.Name = "vsb";
            this.vsb.Size = new System.Drawing.Size(16, 422);
            this.vsb.TabIndex = 7;
            this.vsb.ValueChanged += new System.EventHandler(this.vsb_ValueChanged);
            // 
            // pbr
            // 
            this.pbr.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                   | System.Windows.Forms.AnchorStyles.Right)));
            this.pbr.Location = new System.Drawing.Point(196, 392);
            this.pbr.Name = "pbr";
            this.pbr.Size = new System.Drawing.Size(296, 20);
            this.pbr.TabIndex = 8;
            this.pbr.Visible = false;
            // 
            // FImporteraBildfiler
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(704, 422);
            this.Controls.Add(this.pbr);
            this.Controls.Add(this.vsb);
            this.Controls.Add(this.cmdSelectFolder);
            this.Controls.Add(this.cmdSelectAll);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "FImporteraBildfiler";
            this.Text = "Importera bilder - Ingen mapp vald";
            this.ResumeLayout(false);

        }

        #endregion

        public static DialogResult showDialog(
            Form parent,
            baseFlikForm aktivFlik,
            vdCamera.vdCamera camera)
        {
            using (var dlg = new FImporteraBildfiler())
            {
                dlg._aktivFlik = aktivFlik;
                dlg._requireRaw = eosPresets.UsesRaw( aktivFlik.PresetType, camera );
                var sz = aktivFlik.FlikKategori == FlikKategori.Porträtt
                                  ? new Size(120, 180)
                                  : new Size(180, 120);
               dlg._thumbnails = new Thumbnails(null, Global.Skola, sz.Width, sz.Height, 10);

                if (dlg.ShowDialog(parent) != DialogResult.OK)
                    return DialogResult.Cancel;

                Global.Preferences.LastImportFolder = dlg._strPath;
                Global.sparaInställningar();

                var images = dlg.SelectedImages.ToArray();
                var progress = new frmProgress("Hämtar bilder...", images.Length);
                progress.Owner = parent;
                progress.Show();
                try
                {
                    foreach (var filename in images)
                    {
                        byte[] rawData = null;
                        foreach (var ext in new[] {".raw", ".cr2", ".tif"})
                        {
                            var rawFile = Path.ChangeExtension(filename, ext);
                            if (File.Exists(rawFile))
                                rawData = File.ReadAllBytes(rawFile);
                        }
                        aktivFlik.nyttFoto(
                            true,
                            File.ReadAllBytes(filename),
                            rawData);
 
                        progress.increaseValue();
                    }
                    return DialogResult.OK;
                }
                catch (Exception ex)
                {
                    Global.showMsgBox(null, "mnuImporteraBildfiler:\r\n{0}", ex.ToString());
                }
                finally
                {
                    progress.Close();
                }

                return DialogResult.Cancel;
            }

        }

        protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			var r = SystemInformation.WorkingArea;
			r.Inflate( -r.Width/100, -r.Height/100 );
			this.Location = r.Location;
			this.Size = r.Size;

			vdUsr.vdOneShotTimer.start( 1, showBrowser, null );
		}

		private void showBrowser(object sender, System.EventArgs e)
		{
		    if (Directory.Exists(_strPath) && Directory.GetFiles(_strPath, "*.jpg").Count() != 0)
		        displayNewFolder();
		    else
		        cmdSelectFolder.PerformClick();
		}

        private void layout()
		{
			_thumbnails.layoutImages();
			var nMax = _thumbnails.MaxScroll;
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
			if ( tn!=null )
			{
				tn.Selected = !tn.Selected;
				using ( var g = this.CreateGraphics() )
					tn.paint( g, null, null );
				if ( e.Clicks==2 )
				{
					tn.Selected = true;
					this.DialogResult = DialogResult.OK;
				}
			}
		}

		public IEnumerable<string> SelectedImages
		{
			get
			{
			    return from Thumbnail tn in _thumbnails where tn.Selected select tn.FilenameJpg;
			}
		}

		private void cmdSelectAll_Click(object sender, System.EventArgs e)
		{
			foreach ( Thumbnail tn in _thumbnails )
				tn.Selected = true;
			Invalidate();
		}

		private void cmdSelectFolder_Click(object sender, System.EventArgs e)
		{
			using ( var dlg = new FolderBrowserDialog() )
			{
				dlg.Description = "Välj mapp med bilder";
				dlg.ShowNewFolderButton = false;
				if ( _strPath!=null && Directory.Exists(_strPath) )
					dlg.SelectedPath = _strPath;
				else
					dlg.SelectedPath = @"c:\";

                if ( dlg.ShowDialog()==DialogResult.OK )
				{
					_strPath = dlg.SelectedPath;
					displayNewFolder();
				}
			}

		}

		private void displayNewFolder()
		{
			try
			{
			    _jpgFiles = Directory.Exists(_strPath)
			                    ? Directory.GetFiles(_strPath, "*.jpg")
			                    : new string[0];
				if ( _requireRaw )
				{
					var al = new List<string>( _jpgFiles );
                    var raw = new List<string>();
                    if ( Directory.Exists(_strPath))
    					foreach ( var s in new [] { "*.raw", "*.cr2", "*.tif" } )
	    				    raw.AddRange(Directory.GetFiles(_strPath, s).Select(Path.GetFileNameWithoutExtension));
				    for ( var i=al.Count-1 ; i>=0 ; i-- )
						if ( !raw.Contains( Path.GetFileNameWithoutExtension( (string)al[i] ) ) )
							al.RemoveAt( i );
					_jpgFiles = al.ToArray();
				}

				pbr.Value = 0;
				pbr.Maximum = _jpgFiles.Length;
				pbr.Visible = true;
				_thumbnails.Clear();
				this.Invalidate();
				_nImageLoadIndex = 0;
				timer1.Enabled = true;
				this.Text = "Importera bilder från " + _strPath;
			}
			catch ( Exception ex )
			{
				Global.showMsgBox( this, "displayNewFolder:\r\n{0}", ex.ToString() );
			}
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			if ( _nImageLoadIndex>=_jpgFiles.Length )
			{
				timer1.Enabled = false;
				pbr.Visible = false;
				return;
			}

			try
			{
				var strFilename = _jpgFiles[_nImageLoadIndex++];
				FImport.createThumbnail(
					_thumbnails,
					strFilename,
					180,
					120,
					_aktivFlik.FlikKategori == FlikKategori.Porträtt );
				pbr.Increment( 1 );
				layout();
				using ( var g = this.CreateGraphics() )
					_thumbnails.paint( g, null );
			}
			catch ( Exception ex )
			{
				timer1.Enabled = false;
				Global.showMsgBox( this, ex.ToString() );
			}
		
		}

		private void vsb_ValueChanged(object sender, System.EventArgs e)
		{
			_thumbnails.FirstImage = _thumbnails.ImagesOnOneRow * vsb.Value;
			Invalidate();
		}

	}

}
