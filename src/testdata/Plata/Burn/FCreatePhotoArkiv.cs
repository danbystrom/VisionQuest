using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Photomic.ArchiveStuff.Core;
using Photomic.Common;
using Photomic.Common.Img;
using PlataDM;

namespace Plata.Burn
{
    /// <summary>
    /// Summary description for FBurnCustom.
    /// </summary>
    public class FCreatePhotoArkiv : vdUsr.baseGradientForm
    {
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Timer timer1;

        private Skola _skola;
        private CreatePhotoArkiv _cp;
        private List<BurnFileInfo> _alVimmel = new List<BurnFileInfo>();

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.ProgressBar pbr;

        private JpegSaver _jpgSaver;
        private Button cmdOK;
        private CheckBox chkVimmel;
        private CheckBox chkFotobok;
        private Label lblMsgDone;

        private readonly CustomGenerator _generatorVimmel;

        private bool _fArchiveDone = false;
        private bool _fVimmelDone = false;
        private Label lblVersion;
        private RadioButton optBurn;
        private RadioButton optToDrive;
        private ComboBox cboDrive;
        private string _strFotobok;

        private FCreatePhotoArkiv()
        {
            InitializeComponent();
        }

        private FCreatePhotoArkiv(Skola skola)
        {
            InitializeComponent();

            _skola = skola;

            _jpgSaver = new JpegSaver(88);

            _strFotobok = Global.getAppPath("Fotobok");
            if (!Directory.Exists(_strFotobok) || Directory.GetFiles(_strFotobok).Length == 0)
            {
                chkFotobok.Enabled = false;
                chkFotobok.Checked = false;
            }

            _generatorVimmel = new CustomGenerator(
                Global.Skola.Namn,
                new [] {Global.Skola},
                null,
                new Size(Global.Porträttfotobredd, Global.Porträttfotobredd*3/2),
                null);
            _generatorVimmel.selectPreset(_generatorVimmel.PresetVimmel);
            _generatorVimmel.GenerateFiles(false, false, true);

            var installEXE = Global.getAppPath("PhotoArkiv\\install.exe");
            var strInstVer = FileVersion.getFileVersion(installEXE);
            string instFolder = null;
            if (string.IsNullOrEmpty(strInstVer))
                lblVersion.Text = "CD:n kommer att brännas utan installationsprogram eftersom det saknas i denna Plåta-installtion!";
            else
            {
                lblVersion.Text = string.Format("PhotoArkiv-version: {0}.", strInstVer);
                instFolder = Path.GetDirectoryName(installEXE);
            }
            _cp = new CreatePhotoArkiv(
                DateTime.Now,
                instFolder,
                Global.GetTempPath());
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
            this.cmdCancel = new System.Windows.Forms.Button();
            this.pbr = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cmdOK = new System.Windows.Forms.Button();
            this.chkVimmel = new System.Windows.Forms.CheckBox();
            this.chkFotobok = new System.Windows.Forms.CheckBox();
            this.lblMsgDone = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.optBurn = new System.Windows.Forms.RadioButton();
            this.optToDrive = new System.Windows.Forms.RadioButton();
            this.cboDrive = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(492, 163);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(80, 28);
            this.cmdCancel.TabIndex = 11;
            this.cmdCancel.Text = "Avbryt";
            // 
            // pbr
            // 
            this.pbr.Location = new System.Drawing.Point(8, 12);
            this.pbr.Name = "pbr";
            this.pbr.Size = new System.Drawing.Size(564, 36);
            this.pbr.TabIndex = 12;
            // 
            // timer1
            // 
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // cmdOK
            // 
            this.cmdOK.Enabled = false;
            this.cmdOK.Location = new System.Drawing.Point(406, 164);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(80, 28);
            this.cmdOK.TabIndex = 13;
            this.cmdOK.Text = "STARTA";
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // chkVimmel
            // 
            this.chkVimmel.AutoSize = true;
            this.chkVimmel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkVimmel.Checked = true;
            this.chkVimmel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkVimmel.Location = new System.Drawing.Point(31, 67);
            this.chkVimmel.Name = "chkVimmel";
            this.chkVimmel.Size = new System.Drawing.Size(236, 17);
            this.chkVimmel.TabIndex = 14;
            this.chkVimmel.Text = "Inkludera vimmelbilder på den här CD-skivan";
            this.chkVimmel.UseVisualStyleBackColor = false;
            this.chkVimmel.CheckedChanged += new System.EventHandler(this.chkVimmel_CheckedChanged);
            // 
            // chkFotobok
            // 
            this.chkFotobok.AutoSize = true;
            this.chkFotobok.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkFotobok.Checked = true;
            this.chkFotobok.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFotobok.Location = new System.Drawing.Point(31, 90);
            this.chkFotobok.Name = "chkFotobok";
            this.chkFotobok.Size = new System.Drawing.Size(288, 17);
            this.chkFotobok.TabIndex = 15;
            this.chkFotobok.Text = "Inkludera Fotoboksprogramvaran på den här CD-skivan";
            this.chkFotobok.UseVisualStyleBackColor = false;
            // 
            // lblMsgDone
            // 
            this.lblMsgDone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblMsgDone.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsgDone.Location = new System.Drawing.Point(5, 9);
            this.lblMsgDone.Name = "lblMsgDone";
            this.lblMsgDone.Size = new System.Drawing.Size(567, 39);
            this.lblMsgDone.TabIndex = 16;
            this.lblMsgDone.Text = "TRYCK \"BRÄNN\" FÖR ATT BRÄNNA ARKIVET TILL CD";
            this.lblMsgDone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMsgDone.Visible = false;
            // 
            // lblVersion
            // 
            this.lblVersion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblVersion.Location = new System.Drawing.Point(28, 116);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(431, 32);
            this.lblVersion.TabIndex = 17;
            this.lblVersion.Text = "-";
            // 
            // optBurn
            // 
            this.optBurn.AutoSize = true;
            this.optBurn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.optBurn.Checked = true;
            this.optBurn.Location = new System.Drawing.Point(31, 151);
            this.optBurn.Name = "optBurn";
            this.optBurn.Size = new System.Drawing.Size(111, 17);
            this.optBurn.TabIndex = 18;
            this.optBurn.TabStop = true;
            this.optBurn.Text = "Bränn till CD/DVD";
            this.optBurn.UseVisualStyleBackColor = false;
            // 
            // optToDrive
            // 
            this.optToDrive.AutoSize = true;
            this.optToDrive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.optToDrive.Location = new System.Drawing.Point(31, 174);
            this.optToDrive.Name = "optToDrive";
            this.optToDrive.Size = new System.Drawing.Size(68, 17);
            this.optToDrive.TabIndex = 19;
            this.optToDrive.Text = "Spara till:";
            this.optToDrive.UseVisualStyleBackColor = false;
            // 
            // cboDrive
            // 
            this.cboDrive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDrive.FormattingEnabled = true;
            this.cboDrive.Location = new System.Drawing.Point(105, 173);
            this.cboDrive.Name = "cboDrive";
            this.cboDrive.Size = new System.Drawing.Size(51, 21);
            this.cboDrive.TabIndex = 20;
            // 
            // FCreatePhotoArkiv
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(578, 204);
            this.Controls.Add(this.cboDrive);
            this.Controls.Add(this.optToDrive);
            this.Controls.Add(this.optBurn);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.chkFotobok);
            this.Controls.Add(this.chkVimmel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.pbr);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.lblMsgDone);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FCreatePhotoArkiv";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Skapar Photomic PhotoArkiv-CD";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (_fArchiveDone)
            {
                pbr.Maximum = _generatorVimmel.ResultArray.Count;
                timer1.Enabled = true;
                return;
            }

            _cp.AddOrder(_skola);
            _cp.Execute();
            if (_cp.TotalNumberOfPersons != 0)
            {
                pbr.Maximum = _cp.TotalNumberOfPersons + _generatorVimmel.ResultArray.Count;
                timer1.Enabled = true;
            }
            else
            {
                Global.showMsgBox(this, "Det går inte att skapa ett tomt PhotoArkiv!");
                DialogResult = DialogResult.Cancel;
            }

            foreach (var drive in Directory.GetLogicalDrives())
                if (string.Compare(drive, 0, Application.ExecutablePath, 0, 1, true) != 0)
                {
                    var di = new DriveInfo(drive);
                    cboDrive.Items.Add(drive);
                    if (di.DriveType == DriveType.Removable)
                        cboDrive.SelectedItem = drive;
                }
        }

        public static void showDialog(Form parent, Skola skola, bool fSkipArchive)
        {
            using (var dlg = new FCreatePhotoArkiv(skola))
            {
                dlg._fArchiveDone = fSkipArchive;
                if (dlg.ShowDialog(parent) == DialogResult.Cancel)
                {
                    var dest = new List<BurnFileInfo>(dlg._cp.Result);
                    dest.AddRange(dlg._alVimmel);
                    foreach (var bfi in dest.Where(bfi => bfi.IsTemp))
                        Util.safeKillFile(bfi.LocalFullFileName);
                }
            }
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            try
            {
                if (!_fArchiveDone)
                {
                    if (_cp.generateNext())
                    {
                        _fArchiveDone = true;
                        stateChanged();
                    }
                }
                else if (!_fVimmelDone && chkVimmel.Checked)
                    timerVimmelTick();
            }
            catch (Exception ex)
            {
                timer1.Enabled = false;
                Global.showMsgBox(this, "Kunde inte skapa fotoarkiv!\r\n\r\n{0}", ex.ToString());
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void timerVimmelTick()
        {
            var cn = _generatorVimmel.dequeResultNode();
            if (cn == null)
            {
                _fVimmelDone = true;
                stateChanged();
            }
            else
            {
                _alVimmel.Add(BurnFileInfo.CreateImageFile(
                    Global.GetTempPath(),
                    cn,
                    _jpgSaver));
                pbr.Increment(1);
            }
        }

        private void stateChanged()
        {
            cmdOK.Enabled = _fArchiveDone && (!chkVimmel.Checked || _fVimmelDone);
            lblMsgDone.Visible = cmdOK.Enabled;
            pbr.Visible = !cmdOK.Enabled;
        }

        private class FileVersion
        {
            private struct VS_FIXEDFILEINFO
            {
                public uint dwSignature;
                public uint dwStrucVersion; //  e.g. 0x00000042 = "0.42"
                public uint dwFileVersionMS; //  e.g. 0x00030075 = "3.75"
                public uint dwFileVersionLS; //  e.g. 0x00000031 = "0.31"
                public uint dwProductVersionMS; //  e.g. 0x00030010 = "3.10"
                public uint dwProductVersionLS; //  e.g. 0x00000031 = "0.31"
                public uint dwFileFlagsMask; //  = 0x3F for version "0.42"
                public uint dwFileFlags; //  e.g. VFF_DEBUG Or VFF_PRERELEASE
                public uint dwFileOS; //  e.g. VOS_DOS_WINDOWS16
                public uint dwFileType; //  e.g. VFT_DRIVER
                public uint dwFileSubtype; //  e.g. VFT2_DRV_KEYBOARD
                public uint dwFileDateMS; //  e.g. 0
                public uint dwFileDateLS; //  e.g. 0
            }

            [DllImport("version.dll")]
            private static extern bool GetFileVersionInfo(string sFileName,
                                                          int handle, int size, byte[] infoBuffer);

            [DllImport("version.dll")]
            private static extern int GetFileVersionInfoSize(string sFileName,
                                                             out int handle);

            // The third parameter - "out string pValue" - is automatically
            // marshaled from ANSI to Unicode:
            [DllImport("version.dll")]
            private static extern unsafe bool VerQueryValue(byte[] pBlock,
                                                            string pSubBlock, out string pValue, out uint len);

            // This VerQueryValue overload is marked with 'unsafe' because 
            // it uses a short*:
            [DllImport("version.dll")]
            private static extern unsafe bool VerQueryValue(byte[] pBlock,
                                                            string pSubBlock, out VS_FIXEDFILEINFO* pValue, out uint len);

            public static unsafe string getFileVersion(string strFN)
            {
                int handle = 0;
                // Figure out how much version info there is:
                int size = GetFileVersionInfoSize(strFN, out handle);
                if (size == 0)
                    return null;

                var buffer = new byte[size];

                if (!GetFileVersionInfo(strFN, handle, size, buffer))
                    return null;

                VS_FIXEDFILEINFO* pFileInfo;
                uint len = 0;
                if (!VerQueryValue(buffer, "\\", out pFileInfo, out len))
                    return null;

                return string.Format("{0}-{1:00}-{2:00}",
                                     2006 + (pFileInfo->dwFileVersionMS >> 16), pFileInfo->dwFileVersionMS & 0xFF,
                                     pFileInfo->dwFileVersionLS >> 16, pFileInfo->dwFileVersionLS & 0xFF);
            }

        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            var dest = new List<BurnFileInfo>(_cp.Result);
            if (chkVimmel.Checked)
                dest.AddRange(_alVimmel);
            if (chkFotobok.Checked)
                foreach (var s in Directory.GetFiles(_strFotobok))
                    dest.Add(new BurnFileInfo(s, "\\Fotobok", Path.GetFileName(s), false));

            if (dest.Count != 0)
            {
                if (!chkVimmel.Checked)
                    foreach (var bfi in _alVimmel)
                        Util.safeKillFile(bfi.LocalFullFileName);
                if (optBurn.Checked)
                    BurnHelpers.theNewAndFunBurn(
                        this,
                        dest,
                        "A",
                        "Photomic PhotoArkiv");
                else if (cboDrive.SelectedItem == null)
                    return;
                else
                    ArchiveHelpers.CopyArchiveToFolder(
                        (string)cboDrive.SelectedItem,
                        dest);
                DialogResult = DialogResult.OK;
            }
            else
                DialogResult = DialogResult.Cancel;
        }

        private void chkVimmel_CheckedChanged(object sender, EventArgs e)
        {
            stateChanged();
        }

    }

}
