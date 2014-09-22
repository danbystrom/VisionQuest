using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Plata.Camera;

namespace Plata
{
    public enum FlikTyp
    {
        _Ingen = -1,
        Order,
        GruppbildInne,
        GruppbildUte,
        PorträttInne,
        PorträttUte,
        Infällning,
        Personal,
        Vimmel,
        Färdigställ,
        FTP,
        _SökHopp
    }

    public enum FlikKategori
    {
        _Ingen,
        Gruppbild,
        Porträtt,
        Vimmel,
        Annat
    }

    public enum FlikMode
    {
        Normal,
        Hoover,
        Active,
        Disabled
    }

    /// <summary>
    /// Summary description for frmOrder.
    /// </summary>
    public class baseFlikForm : Form
    {
        public int ButtonY = 24;
        public bool RightAligned;

        protected FlikTyp _FlikTyp;

        protected string _strCaption;
        protected FlikMode _mode;
        protected RectangleF _rectFlik;

        private static Brush _brushGradient;

        protected eosPresets.PresetType _presetType = eosPresets.PresetType.Unknown;
        protected System.Windows.Forms.ToolTip toolTip1;
        private System.ComponentModel.IContainer components;

        public FlikMode FlikMode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        public FlikTyp FlikTyp
        {
            get { return _FlikTyp; }
        }

        public FlikKategori FlikKategori
        {
            get
            {
                switch (_FlikTyp)
                {
                    case FlikTyp.GruppbildInne:
                    case FlikTyp.GruppbildUte:
                        return FlikKategori.Gruppbild;
                    case FlikTyp.PorträttInne:
                    case FlikTyp.PorträttUte:
                    case FlikTyp.Infällning:
                    case FlikTyp.Personal:
                        return FlikKategori.Porträtt;
                    case FlikTyp.Vimmel:
                        return FlikKategori.Vimmel;
                    default:
                        return FlikKategori.Annat;
                }
            }
        }

        public eosPresets.PresetType PresetType
        {
            get { return _presetType; }
        }

        protected baseFlikForm()
        {
            InitializeComponent();
        }

        protected baseFlikForm(Form parent, FlikTyp fliktyp)
        {
            this.MdiParent = parent;
            _FlikTyp = fliktyp;

            InitializeComponent();

            this.ResizeRedraw = true;
            this.FormBorderStyle = FormBorderStyle.None;
        }

        public FMain frmMain
        {
            get { return (FMain) this.MdiParent; }
        }

        protected bool paintBackground(Graphics g, Rectangle rect)
        {
            if (this.ClientRectangle.Width < 2 || this.ClientRectangle.Height < 2)
                return false;
            if (_brushGradient == null)
                if (!SystemInformation.TerminalServerSession)
                    _brushGradient = new LinearGradientBrush(
                        this.ClientRectangle,
                        Color.FromArgb(220, 220, 220),
                        Color.FromArgb(160, 160, 160),
                        45,
                        true);
                else
                    _brushGradient = new SolidBrush(Color.FromArgb(196, 196, 196));
            g.FillRectangle(_brushGradient, rect);
            return true;
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (!paintBackground(pevent.Graphics, pevent.ClipRectangle))
                base.OnPaintBackground(pevent);
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            // 
            // baseFlikForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(892, 566);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,
                                                ((System.Byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "baseFlikForm";
            this.ShowInTaskbar = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.baseFlikFor_Load);

        }

        #endregion

        protected virtual eosPresets.PresetType photoLocationHit(int x, int y)
        {
            return eosPresets.PresetType.Unknown;
        }

        public bool updateHit(int x, int y, int nClicks)
        {
            var ptOld = _presetType;
            var fHitOnPL = false;
            if (nClicks == 2)
            {
                var ptNew = photoLocationHit(x, y);
                if (ptNew != eosPresets.PresetType.Unknown)
                {
                    fHitOnPL = true;
                    _presetType = ptNew;
                }
            }

            switch (_mode)
            {
                case FlikMode.Active:
                    if (_presetType == ptOld)
                        return false;
                    activated(); //tvinga uppdatering av statusrad
                    break;
                case FlikMode.Disabled:
                    return false;
            }

            if (_rectFlik.Contains(x, y) || fHitOnPL)
            {
                if (nClicks != 0)
                {
                    _mode = FlikMode.Active;
                    return true;
                }
                if (_mode == FlikMode.Normal)
                {
                    _mode = FlikMode.Hoover;
                    return true;
                }
            }
            else if (_mode == FlikMode.Hoover)
            {
                _mode = FlikMode.Normal;
                return true;
            }
            return false;
        }

        public bool checkHitArea(int x, int y)
        {
            return _rectFlik.Contains(x, y);
        }

        public virtual void paint_Flik(
            Graphics g,
            Region shape,
            Color color,
            Font font,
            int x)
        {
            g.TranslateTransform(x, ButtonY);
            if (_mode == FlikMode.Active)
                color = Color.White;
            using (var br = new LinearGradientBrush(shape.GetBounds(g), Color.FromArgb(95, color), color, 0, false))
            {
                br.SetBlendTriangularShape(0.5f);
                g.FillRegion(br, shape);
            }
            g.TranslateTransform(-x, -ButtonY);

            _rectFlik = shape.GetBounds(g);
            _rectFlik.Offset(x + 2, ButtonY);
            var r = _rectFlik;
            r.Inflate(5, 0);

            switch (_mode)
            {
                case FlikMode.Normal:
                    g.DrawString(_strCaption, font, Brushes.White, r, Util.sfMC);
                    break;
                case FlikMode.Active:
                    g.DrawString(_strCaption, font, Brushes.Blue, r, Util.sfMC);
                    break;
                case FlikMode.Hoover:
                    g.DrawString(_strCaption, font, Brushes.Yellow, r, Util.sfMC);
                    break;
                case FlikMode.Disabled:
                    g.DrawString(_strCaption, font, Brushes.CadetBlue, r, Util.sfMC);
                    break;
            }
        }

        public FlikMode fmMode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        private void baseFlikFor_Load(object sender, System.EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (Global.Skola != null && this.WindowState != FormWindowState.Minimized)
                try
                {
                    resize(this.ClientSize);
                    activated();
                }
                catch (Exception ex)
                {
                    Global.showMsgBox(this, ex.ToString());
                }
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            _mode = FlikMode.Normal;
        }

        protected virtual void resize(Size sz)
        {
        }

        protected virtual void paint(PaintEventArgs e)
        {
        }

        public virtual void activated()
        {
            var s = "";
            if (_presetType != eosPresets.PresetType.Unknown)
            {
                var p = eosPresets.GetPreset(_presetType, frmMain.Camera.CameraType);
                if (p != null)
                {
                    var tr = frmMain.Camera.Translation;
                    s = string.Format(
                        "{0}  {1}  M:{2}  {3}  {4}  P/S/K/M/F:{5}/{6}/{7}/{8}/{9}  WB:{10}/{11}  Matrix:{12}{13}",
                        p.ImageTypeSize,
                        tr.ISO.Text(p.ISO),
                        tr.ShootingMode.Text(p.Mode),
                        tr.ShutterSpeed.Text(p.TV),
                        tr.Aperture.Text(p.AV),
                        tr.Parameters.Text(p.ParameterSet), p.SharpnessText, p.ContrastText, p.SaturationText, p.ColorToneText,
                        tr.WhiteBalance.Text(p.WB),
                        p.KelvinText,
                        tr.ColorMatrix.Text(p.ColorMatrix),
                        p.ColorSpace > 0 ? ("/" + tr.ColorSpace.Text(p.ColorSpace)) : "");
                }
            }

            frmMain.setStatusText(this, s);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            try
            {
                _brushGradient = null;
                if (Global.Skola != null && this.WindowState != FormWindowState.Minimized && fmMode == FlikMode.Active)
                {
                    this.SuspendLayout();
                    resize(this.ClientSize);
                    this.ResumeLayout();
                }
            }
            catch
            {
            }
        }

        public virtual void save()
        {
        }

        public virtual void skolaUppdaterad()
        {
        }

        public virtual void nyttFoto(bool fInternal, byte[] jpgData, byte[] rawData)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Global.Skola != null && fmMode == FlikMode.Active)
                paint(e);
        }

        protected void setStatusText(string strText)
        {
            frmMain.setStatusText(this, strText);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0112) //W_SYSCOMMAND, SC_NEXTWINDOW, SC_PREVWINDOW
            {
                switch (m.WParam.ToInt32())
                {
                    case 0xF040:
                    case 0xF050:
                    case 0xF060:
                        return;
                }
            }
//			System.Diagnostics.Debug.WriteLine( string.Format("{0} {1}", m.Msg, m.WParam ) );
            base.WndProc(ref m);
        }

        protected void addSubmenuForImageCopy(ContextMenu menu)
        {
            if (Global.Fotografdator)
                return;
            menu.MenuItems.Add("-");
            var mi = menu.MenuItems.Add("Kopiera fil(er)");
            mi.Click += mi_ClickCopyNativeFiles;
        }

        private void mi_ClickCopyNativeFiles(object sender, EventArgs e)
        {
            var tns = getSelectedThumbnails();
            if (tns == null)
                return;

            var list = new List<string>();
            foreach ( var tn in tns)
            {
                list.Add(tn.FilenameJpg);
                if ( !string.IsNullOrEmpty(tn.FilenameRaw))
                    list.Add(tn.FilenameRaw);
            }

            var objData = new DataObject();
            objData.SetData(DataFormats.FileDrop, true, list.ToArray());
            Clipboard.SetDataObject(objData, true);
        }

        protected virtual PlataDM.Thumbnail getSelectedThumbnail()
        {
            return null;
        }

        protected virtual IEnumerable<PlataDM.Thumbnail> getSelectedThumbnails()
        {
            return null;
        }

        public virtual Bitmap currentBitmap()
        {
            return null;
        }

    }

}
