using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Photomic.Common;
using Plata.Camera;

namespace Plata
{
    public class frmVimmel : Plata.baseFlikForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.VScrollBar vsb;
        private System.Windows.Forms.ContextMenu mnuVimmel;
        private System.Windows.Forms.MenuItem mnuLovad;
        private System.Windows.Forms.MenuItem mnuRadera;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem mnuFullskärm;

        private PlataDM.Vimmelbild _vbRightClicked;
        private System.Windows.Forms.MenuItem mnuVald;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.Timer tmrAuto;
        private MenuItem mnuVäljAlla;
        private MenuItem menuItem2;
        private MenuItem mnuVäljIngen;

        private delegateNyttFoto _delegateNyttFoto;

        public frmVimmel() : base()
        {
        }

        public frmVimmel(Form parent) : base(parent, FlikTyp.Vimmel)
        {
            _strCaption = "VIMMEL";
            InitializeComponent();
            this.Bounds = parent.ClientRectangle;
            this.PerformLayout();
            _presetType = eosPresets.PresetType.Environment;
            vsb.ValueChanged += new EventHandler(vsb_ValueChanged);

            addSubmenuForImageCopy(mnuVimmel);
        }

        protected override PlataDM.Thumbnail getSelectedThumbnail()
        {
            return _vbRightClicked.Thumbnail;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _delegateNyttFoto = new delegateNyttFoto(this.nyttFoto_del2);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.vsb = new System.Windows.Forms.VScrollBar();
            this.mnuVimmel = new System.Windows.Forms.ContextMenu();
            this.mnuVald = new System.Windows.Forms.MenuItem();
            this.mnuLovad = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.mnuVäljAlla = new System.Windows.Forms.MenuItem();
            this.mnuVäljIngen = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mnuRadera = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.mnuFullskärm = new System.Windows.Forms.MenuItem();
            this.tmrAuto = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // vsb
            // 
            this.vsb.LargeChange = 1;
            this.vsb.Location = new System.Drawing.Point(863, 9);
            this.vsb.Name = "vsb";
            this.vsb.Size = new System.Drawing.Size(20, 548);
            this.vsb.TabIndex = 0;
            // 
            // mnuVimmel
            // 
            this.mnuVimmel.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
                                                  {
                                                      this.mnuVald,
                                                      this.mnuLovad,
                                                      this.menuItem2,
                                                      this.mnuVäljAlla,
                                                      this.mnuVäljIngen,
                                                      this.menuItem1,
                                                      this.mnuRadera,
                                                      this.menuItem4,
                                                      this.mnuFullskärm
                                                  });
            // 
            // mnuVald
            // 
            this.mnuVald.Index = 0;
            this.mnuVald.Text = "Vald";
            this.mnuVald.Click += new System.EventHandler(this.mnuVald_Click);
            // 
            // mnuLovad
            // 
            this.mnuLovad.Index = 1;
            this.mnuLovad.Text = "Lovad";
            this.mnuLovad.Click += new System.EventHandler(this.mnuLovad_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 2;
            this.menuItem2.Text = "-";
            // 
            // mnuVäljAlla
            // 
            this.mnuVäljAlla.Index = 3;
            this.mnuVäljAlla.Text = "Välj ALLA!";
            this.mnuVäljAlla.Click += new System.EventHandler(this.mnuVäljAlla_Click);
            // 
            // mnuVäljIngen
            // 
            this.mnuVäljIngen.Index = 4;
            this.mnuVäljIngen.Text = "Välj INGEN!";
            this.mnuVäljIngen.Click += new System.EventHandler(this.mnuVäljIngen_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 5;
            this.menuItem1.Text = "-";
            // 
            // mnuRadera
            // 
            this.mnuRadera.Index = 6;
            this.mnuRadera.Text = "Radera";
            this.mnuRadera.Click += new System.EventHandler(this.mnuRadera_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 7;
            this.menuItem4.Text = "-";
            // 
            // mnuFullskärm
            // 
            this.mnuFullskärm.Index = 8;
            this.mnuFullskärm.Shortcut = System.Windows.Forms.Shortcut.F5;
            this.mnuFullskärm.Text = "Visa fullskärm";
            this.mnuFullskärm.Click += new System.EventHandler(this.mnuFullskär_Click);
            // 
            // tmrAuto
            // 
            this.tmrAuto.Interval = 5000;
            this.tmrAuto.Tick += new System.EventHandler(this.tmrAuto_Tick);
            // 
            // frmVimmel
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(892, 566);
            this.Controls.Add(this.vsb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmVimmel";
            this.ResumeLayout(false);

        }

        #endregion

        public override void activated()
        {
            eosPresets.ApplyPreset(_presetType, frmMain.Camera);
            showStat();
        }

        private void showStat()
        {
            int nAntal, nLovade, nValda;
            Global.Skola.Vimmel.räkna(out nAntal, out nLovade, out nValda);
            setStatusText(string.Format("Total {0} vimmelbilder, varav {1} lovade och {2} valda",
                                        nAntal, nLovade, nValda));
        }

        private void layout()
        {
            var tns = Global.Skola.Vimmel.Thumbnails;
            tns.layoutImages();
            var nMax = tns.MaxScroll;
            if (nMax > 0)
            {
                vsb.Visible = true;
                vsb.Value = Math.Min(vsb.Value, nMax);
                vsb.Maximum = nMax;
            }
            else
                vsb.Visible = false;
        }

        protected override void paint(PaintEventArgs e)
        {
            Global.Skola.Vimmel.paint(e.Graphics);
        }

        protected override void resize(Size sz)
        {
            int sw = SystemInformation.VerticalScrollBarWidth;
            vsb.Bounds = new Rectangle(sz.Width - sw, 0, sw, sz.Height);
            Global.Skola.Vimmel.Thumbnails.layoutImages(5, 5, sz.Width - sw - 5, sz.Height);
            layout();
        }

        private void visaFullskärm(PlataDM.Thumbnail tn)
        {
            frmZoom.showDialog(
                this,
                null,
                null,
                FlikKategori.Vimmel,
                Global.Skola.Vimmel.Thumbnails,
                tn != null ? tn.Key : null,
                null,
                null,
                null);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            var p = this.PointToClient(MousePosition);
            visaFullskärm(Global.Skola.Vimmel.Thumbnails.hitTest(p.X, p.Y));
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.F5)
                visaFullskärm(null);
            if (e.KeyCode == Keys.F12 && e.Modifiers == Keys.Control)
                tmrAuto.Enabled = !tmrAuto.Enabled;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            var p = PointToClient(MousePosition);
            var tn = Global.Skola.Vimmel.Thumbnails.hitTest(p.X, p.Y);
            if (e.Button == MouseButtons.Right && tn != null)
            {
                _vbRightClicked = Global.Skola.Vimmel[tn.Key];
                if (_vbRightClicked != null)
                {
                    mnuVald.Checked = _vbRightClicked.Status == VimmelStatus.Vald;
                    mnuLovad.Checked = _vbRightClicked.Status == VimmelStatus.Lovad;
                    mnuRadera.Checked = _vbRightClicked.Status == VimmelStatus.Raderad;
                    mnuVimmel.Show(this, new Point(e.X, e.Y));
                }
            }
        }

        private void mnuVald_Click(object sender, System.EventArgs e)
        {
            _vbRightClicked.Status = ((MenuItem) sender).Checked
                ? VimmelStatus.Normal
                : VimmelStatus.Vald;
            showStat();
            Invalidate();
        }

        private void mnuLovad_Click(object sender, System.EventArgs e)
        {
            _vbRightClicked.Status = ((MenuItem) sender).Checked
                ? VimmelStatus.Normal
                : VimmelStatus.Lovad;
            showStat();
            Invalidate();
        }

        private void mnuVäljAlla_Click(object sender, EventArgs e)
        {
            foreach (PlataDM.Vimmelbild vb in Global.Skola.Vimmel)
                if (vb.Status == VimmelStatus.Normal)
                    vb.Status = VimmelStatus.Vald;
            Invalidate();
        }

        private void mnuVäljIngen_Click(object sender, EventArgs e)
        {
            foreach (PlataDM.Vimmelbild vb in Global.Skola.Vimmel)
                if (vb.Status == VimmelStatus.Vald)
                    vb.Status = VimmelStatus.Normal;
            Invalidate();
        }

        private void mnuFullskär_Click(object sender, System.EventArgs e)
        {
            visaFullskärm(_vbRightClicked.Thumbnail);
        }

        private void mnuRadera_Click(object sender, System.EventArgs e)
        {
            if (Global.askMsgBox(this, "Är du säker på att du vill radera vimmelbilden?", true) != DialogResult.Yes)
                return;
            _vbRightClicked.Thumbnail.Delete(Global.Skola);
            Global.Skola.Vimmel.Remove(_vbRightClicked);
            showStat();
            Invalidate();
        }

        private void vsb_ValueChanged(object sender, EventArgs e)
        {
            var tns = Global.Skola.Vimmel.Thumbnails;
            tns.FirstImage = tns.ImagesOnOneRow*vsb.Value;
            Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            int nVal = vsb.Value - Math.Sign(e.Delta);
            vsb.Value = Math.Max(0, Math.Min(vsb.Maximum, nVal));
        }

        public override void nyttFoto(bool fInternal, byte[] jpgData, byte[] rawData)
        {
            this.Invoke(_delegateNyttFoto, new object[] {fInternal, jpgData, rawData});
        }

        private void nyttFoto_del2(bool fInternal, byte[] jpgData, byte[] rawData)
        {
            if (!fInternal && !FKollaKamera.kollaKamera(frmMain, eosPresets.PresetType.Environment))
                return;

            var jpgFile = Global.Skola.HomePathCombine(string.Format("v_{0}.jpg", Guid.NewGuid()));
            var bmp = new Bitmap(new MemoryStream(jpgData));
            var vb = Global.Skola.Vimmel.addImage(jpgFile, bmp, null);
            vb.Thumbnail.fixPortrait(
                bmp,
                new Size(240, 240*bmp.Height/bmp.Width),
                Global.Skola.Fotograf,
                RotateFlipType.RotateNoneFlipNone,
                null,
                null);
            layout();
            using (var g = CreateGraphics())
                vb.Thumbnail.paint(g, null, null);

            showStat();

            if (rawData != null)
                Global.showMsgBox(this, "RAW behövs inte när du tar vimmelbilder!");
        }

        private void tmrAuto_Tick(object sender, System.EventArgs e)
        {
            frmMain.Camera.TakePhoto();
        }

    }

}

