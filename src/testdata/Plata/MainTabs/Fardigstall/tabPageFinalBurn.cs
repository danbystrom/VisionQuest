using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using Photomic.ArchiveStuff.Core;
using Xceed.Grid;

namespace Plata
{
	public class tabPageFinalBurn : UserControl, IBSTab
	{
		private System.Windows.Forms.Button cmdFinalCopy;
		private System.Windows.Forms.Button cmdBurnDVD;
		private vdUsr.vdGroup grpRestfoto;
		private System.Windows.Forms.RadioButton optRestG_EjSvar;
		private System.Windows.Forms.RadioButton optRestG_Nej;
		private System.Windows.Forms.RadioButton optRestG_Ja;
		private System.Windows.Forms.TextBox txtStatus;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.CheckBox chkOrderCorrect;
		private vdXceed.vdPlainGrid ugG;
		private System.Windows.Forms.RadioButton optRestP_EjSvar;
		private System.Windows.Forms.RadioButton optRestP_Nej;
		private System.Windows.Forms.RadioButton optRestP_Ja;
		private vdUsr.vdGroup grpAnpassadCD;
		private System.Windows.Forms.RadioButton optAnp_EjSvar;
		private System.Windows.Forms.RadioButton optAnp_Nej;
		private System.Windows.Forms.RadioButton optAnp_Ja;
		private vdUsr.vdGroup grpPhotoArkiv;
		private System.Windows.Forms.RadioButton optPArkiv_EjSvar;
		private System.Windows.Forms.RadioButton optPArkiv_Nej;
		private System.Windows.Forms.RadioButton optPArkiv_Ja;
		private vdUsr.vdGroup grpVimmel;
		private System.Windows.Forms.RadioButton optV_EjSvar;
		private System.Windows.Forms.RadioButton optV_Nej;
		private System.Windows.Forms.RadioButton optV_Ja;
		private System.Windows.Forms.Label lblBurnTime;
		private Label label2;
		private TextBox txtAnp_Till;
		private Label label3;
		private TextBox txtPArkiv_Till;
		private Label label4;
		private TextBox txtV_Till;
		private vdUsr.vdGroup grpGranskning;
		private Label label5;
		private TextBox txtValidateGP_Av;
		private RadioButton optValidateGP_EjSvar;
		private RadioButton optValidateGP_Nej;
		private RadioButton optValidateGP_Ja;
		private Label lblGranskningSkaGöras;
		private CheckBox chkWhoAmI;
		private Button cmdFTP;
		private vdUsr.vdGroup grpLappar;
		private Label label1;
		private TextBox txtPlappar_Kommentar;
		private RadioButton optPlappar_EjSvar;
		private RadioButton optPlappar_Nej;
		private RadioButton optPlappar_Ja;
		private Label label7;
		private Label lblRestG;
		private Label label10;
		private RadioButton optRestI_EjSvar;
		private RadioButton optRestI_Nej;
		private RadioButton optRestI_Ja;
		private RadioButton optRestV_EjSvar;
		private Label label8;
		private RadioButton optRestV_Nej;
		private Label label9;
		private RadioButton optRestV_Ja;
		private RadioButton optRestO_EjSvar;
		private RadioButton optRestO_Nej;
		private RadioButton optRestO_Ja;

		private PlataDM.Skola _skola;
		private readonly Dictionary<string, List<RadioButton>> _optRest = new Dictionary<string, List<RadioButton>>();

		public tabPageFinalBurn()
		{
			InitializeComponent();

			frmFärdigställ.prepareGruppGrid( ugG, true );
            ugG.G.Font = new Font(ugG.G.Font.FontFamily, ugG.G.Font.Size - 2);

			_optRest.Add( "G", new List<RadioButton> { optRestG_Ja, optRestG_Nej, optRestG_EjSvar } );
			_optRest.Add( "P", new List<RadioButton> { optRestP_Ja, optRestP_Nej, optRestP_EjSvar } );
			_optRest.Add( "O", new List<RadioButton> { optRestO_Ja, optRestO_Nej, optRestO_EjSvar } );
			_optRest.Add( "V", new List<RadioButton> { optRestV_Ja, optRestV_Nej, optRestV_EjSvar } );
			_optRest.Add( "I", new List<RadioButton> { optRestI_Ja, optRestI_Nej, optRestI_EjSvar } );

            if ( Global.Preferences.Brand == Brand.Kungsfoto )
            {
                grpAnpassadCD.Visible = false;
                grpGranskning.Visible = false;
                grpLappar.Visible = false;
                grpPhotoArkiv.Visible = false;
                //grpRestfoto.Visible = false;
                grpVimmel.Visible = false;
                txtStatus.Top -= 100;
                chkOrderCorrect.Top -= 100;
                chkWhoAmI.Top -= 100;
                cmdBurnDVD.Top -= 100;
                cmdFTP.Top -= 100;
                cmdFinalCopy.Top -= 100;
                lblBurnTime.Top -= 100;

                foreach (Control c in grpRestfoto.Controls)
                    c.Visible = c.Name.Contains("RestG");
            }

		}

        void IBSTab.load()
        {
            _skola = Global.Skola;

            chkWhoAmI.Text = string.Format("Jag är {0}", Fotografer.Name(Global.Preferences.Fotografnummer));
            chkWhoAmI.Enabled = Global.Preferences.Fotografnummer != 0;

            string strText;
            bool severeError;

            ugG.beginCleanFillup();
            Burn.FDirectBurnMenu.checkErrorsX(_skola, ugG, out strText, out severeError);
            ugG.endFillup();
            cmdFTP.Enabled = !severeError;
            cmdBurnDVD.Enabled = !severeError;
            cmdFinalCopy.Enabled = !severeError;
            txtStatus.Text = strText;
            txtStatus.SelectionStart = 0;

            //foreach (string s in _skola.Enkät.Keys)
            //    if ("-1".Equals(_skola.Enkät[s]))
            //    {
            //        txtStatus.AppendText("\r\nOBS! OBS! Du har inte fyllt i fotografenkäten!");
            //        break;
            //    }

            Global.optCheck(_skola.RestfotoGrupp, optRestG_Ja, optRestG_Nej, optRestG_EjSvar);
            Global.optCheck(_skola.RestfotoPorträtt, optRestP_Ja, optRestP_Nej, optRestP_EjSvar);
            Global.optCheck(_skola.RestfotoOmslag, optRestO_Ja, optRestO_Nej, optRestO_EjSvar);
            Global.optCheck(_skola.RestfotoVimmel, optRestV_Ja, optRestV_Nej, optRestV_EjSvar);
            Global.optCheck(_skola.RestfotoInfällning, optRestI_Ja, optRestI_Nej, optRestI_EjSvar);

            Global.optCheck(_skola.DidBurnCustomProgCD, optAnp_Ja, optAnp_Nej, optAnp_EjSvar);
            Global.optCheck(_skola.DidBurnPhotoArkivCD, optPArkiv_Ja, optPArkiv_Nej, optPArkiv_EjSvar);
            Global.optCheck(_skola.DidBurnEnvironmentCD, optV_Ja, optV_Nej, optV_EjSvar);
            Global.optCheck(_skola.DidValidateGroupPictures, optValidateGP_Ja, optValidateGP_Nej, optValidateGP_EjSvar);
            Global.optCheck(_skola.DidHandoutPPapers, optPlappar_Ja, optPlappar_Nej, optPlappar_EjSvar);
            lblGranskningSkaGöras.Visible = _skola.GroupPicturesShallBeValidated;

            txtAnp_Till.Text = _skola.ReceiverOfCustomProgCD;
            txtPArkiv_Till.Text = _skola.ReceiverOfPhotoArkivCD;
            txtV_Till.Text = _skola.ReceiverOfEnvironmentCD;
            txtValidateGP_Av.Text = _skola.GroupPicturesValidatedBy;
            txtPlappar_Kommentar.Text = _skola.PPaperHandoutComment;

            _skola.BurnedWhen = vdUsr.DateHelper.YYYYMMDDHHMM(Global.Now);
            lblBurnTime.Text = string.Format("Tid för bränning:\r\n{0}", _skola.BurnedWhen);
        }

	    void IBSTab.save()
		{
            _skola.RestfotoGrupp = Global.getOptCheck(optRestG_Ja, optRestG_Nej);
            _skola.RestfotoPorträtt = Global.getOptCheck(optRestP_Ja, optRestP_Nej);
            _skola.RestfotoOmslag = Global.getOptCheck(optRestO_Ja, optRestO_Nej);
            _skola.RestfotoVimmel = Global.getOptCheck(optRestV_Ja, optRestV_Nej);
            _skola.RestfotoInfällning = Global.getOptCheck(optRestI_Ja, optRestI_Nej);

            _skola.DidBurnCustomProgCD = Global.getOptCheck( optAnp_Ja, optAnp_Nej );
			_skola.DidBurnPhotoArkivCD = Global.getOptCheck( optPArkiv_Ja, optPArkiv_Nej );
			_skola.DidBurnEnvironmentCD = Global.getOptCheck( optV_Ja, optV_Nej );
			_skola.DidValidateGroupPictures = Global.getOptCheck( optValidateGP_Ja, optValidateGP_Nej );
			_skola.DidHandoutPPapers = Global.getOptCheck( optPlappar_Ja, optPlappar_Nej );

			_skola.ReceiverOfCustomProgCD = txtAnp_Till.Text.Trim();
			_skola.ReceiverOfPhotoArkivCD = txtPArkiv_Till.Text.Trim();
			_skola.ReceiverOfEnvironmentCD = txtV_Till.Text.Trim();
			_skola.GroupPicturesValidatedBy = txtValidateGP_Av.Text.Trim();
			_skola.PPaperHandoutComment = txtPlappar_Kommentar.Text.Trim();
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
            this.cmdFinalCopy = new System.Windows.Forms.Button();
            this.cmdBurnDVD = new System.Windows.Forms.Button();
            this.chkOrderCorrect = new System.Windows.Forms.CheckBox();
            this.grpRestfoto = new vdUsr.vdGroup();
            this.label10 = new System.Windows.Forms.Label();
            this.optRestI_EjSvar = new System.Windows.Forms.RadioButton();
            this.optRestI_Nej = new System.Windows.Forms.RadioButton();
            this.optRestI_Ja = new System.Windows.Forms.RadioButton();
            this.optRestV_EjSvar = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.optRestV_Nej = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.optRestV_Ja = new System.Windows.Forms.RadioButton();
            this.optRestO_EjSvar = new System.Windows.Forms.RadioButton();
            this.optRestO_Nej = new System.Windows.Forms.RadioButton();
            this.optRestO_Ja = new System.Windows.Forms.RadioButton();
            this.optRestP_EjSvar = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.optRestP_Nej = new System.Windows.Forms.RadioButton();
            this.lblRestG = new System.Windows.Forms.Label();
            this.optRestP_Ja = new System.Windows.Forms.RadioButton();
            this.optRestG_EjSvar = new System.Windows.Forms.RadioButton();
            this.optRestG_Nej = new System.Windows.Forms.RadioButton();
            this.optRestG_Ja = new System.Windows.Forms.RadioButton();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.ugG = new vdXceed.vdPlainGrid();
            this.grpAnpassadCD = new vdUsr.vdGroup();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAnp_Till = new System.Windows.Forms.TextBox();
            this.optAnp_EjSvar = new System.Windows.Forms.RadioButton();
            this.optAnp_Nej = new System.Windows.Forms.RadioButton();
            this.optAnp_Ja = new System.Windows.Forms.RadioButton();
            this.grpPhotoArkiv = new vdUsr.vdGroup();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPArkiv_Till = new System.Windows.Forms.TextBox();
            this.optPArkiv_EjSvar = new System.Windows.Forms.RadioButton();
            this.optPArkiv_Nej = new System.Windows.Forms.RadioButton();
            this.optPArkiv_Ja = new System.Windows.Forms.RadioButton();
            this.grpVimmel = new vdUsr.vdGroup();
            this.label4 = new System.Windows.Forms.Label();
            this.txtV_Till = new System.Windows.Forms.TextBox();
            this.optV_EjSvar = new System.Windows.Forms.RadioButton();
            this.optV_Nej = new System.Windows.Forms.RadioButton();
            this.optV_Ja = new System.Windows.Forms.RadioButton();
            this.lblBurnTime = new System.Windows.Forms.Label();
            this.grpGranskning = new vdUsr.vdGroup();
            this.lblGranskningSkaGöras = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtValidateGP_Av = new System.Windows.Forms.TextBox();
            this.optValidateGP_EjSvar = new System.Windows.Forms.RadioButton();
            this.optValidateGP_Nej = new System.Windows.Forms.RadioButton();
            this.optValidateGP_Ja = new System.Windows.Forms.RadioButton();
            this.chkWhoAmI = new System.Windows.Forms.CheckBox();
            this.cmdFTP = new System.Windows.Forms.Button();
            this.grpLappar = new vdUsr.vdGroup();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPlappar_Kommentar = new System.Windows.Forms.TextBox();
            this.optPlappar_EjSvar = new System.Windows.Forms.RadioButton();
            this.optPlappar_Nej = new System.Windows.Forms.RadioButton();
            this.optPlappar_Ja = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.grpRestfoto)).BeginInit();
            this.grpRestfoto.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpAnpassadCD)).BeginInit();
            this.grpAnpassadCD.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpPhotoArkiv)).BeginInit();
            this.grpPhotoArkiv.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpVimmel)).BeginInit();
            this.grpVimmel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpGranskning)).BeginInit();
            this.grpGranskning.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpLappar)).BeginInit();
            this.grpLappar.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdFinalCopy
            // 
            this.cmdFinalCopy.Location = new System.Drawing.Point(831, 594);
            this.cmdFinalCopy.Name = "cmdFinalCopy";
            this.cmdFinalCopy.Size = new System.Drawing.Size(185, 28);
            this.cmdFinalCopy.TabIndex = 13;
            this.cmdFinalCopy.Text = "Slutlagra på extern hårddisk";
            this.cmdFinalCopy.Click += new System.EventHandler(this.cmdFinalCopy_Click);
            // 
            // cmdBurnDVD
            // 
            this.cmdBurnDVD.Location = new System.Drawing.Point(831, 560);
            this.cmdBurnDVD.Name = "cmdBurnDVD";
            this.cmdBurnDVD.Size = new System.Drawing.Size(185, 28);
            this.cmdBurnDVD.TabIndex = 12;
            this.cmdBurnDVD.Text = "Slutlagra på CD/DVD";
            this.cmdBurnDVD.Click += new System.EventHandler(this.cmdBurnDVD_Click);
            // 
            // chkOrderCorrect
            // 
            this.chkOrderCorrect.AutoSize = true;
            this.chkOrderCorrect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkOrderCorrect.Location = new System.Drawing.Point(581, 526);
            this.chkOrderCorrect.Name = "chkOrderCorrect";
            this.chkOrderCorrect.Size = new System.Drawing.Size(177, 20);
            this.chkOrderCorrect.TabIndex = 9;
            this.chkOrderCorrect.Text = "Gruppordningen är korrekt";
            this.chkOrderCorrect.UseVisualStyleBackColor = false;
            // 
            // grpRestfoto
            // 
            this.grpRestfoto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grpRestfoto.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grpRestfoto.Caption = null;
            this.grpRestfoto.Controls.Add(this.label10);
            this.grpRestfoto.Controls.Add(this.optRestI_EjSvar);
            this.grpRestfoto.Controls.Add(this.optRestI_Nej);
            this.grpRestfoto.Controls.Add(this.optRestI_Ja);
            this.grpRestfoto.Controls.Add(this.optRestV_EjSvar);
            this.grpRestfoto.Controls.Add(this.label8);
            this.grpRestfoto.Controls.Add(this.optRestV_Nej);
            this.grpRestfoto.Controls.Add(this.label9);
            this.grpRestfoto.Controls.Add(this.optRestV_Ja);
            this.grpRestfoto.Controls.Add(this.optRestO_EjSvar);
            this.grpRestfoto.Controls.Add(this.optRestO_Nej);
            this.grpRestfoto.Controls.Add(this.optRestO_Ja);
            this.grpRestfoto.Controls.Add(this.optRestP_EjSvar);
            this.grpRestfoto.Controls.Add(this.label7);
            this.grpRestfoto.Controls.Add(this.optRestP_Nej);
            this.grpRestfoto.Controls.Add(this.lblRestG);
            this.grpRestfoto.Controls.Add(this.optRestP_Ja);
            this.grpRestfoto.Controls.Add(this.optRestG_EjSvar);
            this.grpRestfoto.Controls.Add(this.optRestG_Nej);
            this.grpRestfoto.Controls.Add(this.optRestG_Ja);
            this.grpRestfoto.Location = new System.Drawing.Point(573, 4);
            this.grpRestfoto.Name = "grpRestfoto";
            this.grpRestfoto.Size = new System.Drawing.Size(443, 83);
            this.grpRestfoto.TabIndex = 1;
            this.grpRestfoto.Text = "Önskas restfoto för ...";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 58);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 16);
            this.label10.TabIndex = 16;
            this.label10.Text = "Infäll. katalog";
            // 
            // optRestI_EjSvar
            // 
            this.optRestI_EjSvar.AutoCheck = false;
            this.optRestI_EjSvar.AutoSize = true;
            this.optRestI_EjSvar.Location = new System.Drawing.Point(200, 58);
            this.optRestI_EjSvar.Name = "optRestI_EjSvar";
            this.optRestI_EjSvar.Size = new System.Drawing.Size(33, 20);
            this.optRestI_EjSvar.TabIndex = 19;
            this.optRestI_EjSvar.Text = "?";
            this.optRestI_EjSvar.Click += new System.EventHandler(this.optRest_Click);
            // 
            // optRestI_Nej
            // 
            this.optRestI_Nej.AutoCheck = false;
            this.optRestI_Nej.AutoSize = true;
            this.optRestI_Nej.Location = new System.Drawing.Point(148, 58);
            this.optRestI_Nej.Name = "optRestI_Nej";
            this.optRestI_Nej.Size = new System.Drawing.Size(45, 20);
            this.optRestI_Nej.TabIndex = 18;
            this.optRestI_Nej.Text = "Nej";
            this.optRestI_Nej.Click += new System.EventHandler(this.optRest_Click);
            // 
            // optRestI_Ja
            // 
            this.optRestI_Ja.AutoCheck = false;
            this.optRestI_Ja.AutoSize = true;
            this.optRestI_Ja.Location = new System.Drawing.Point(104, 58);
            this.optRestI_Ja.Name = "optRestI_Ja";
            this.optRestI_Ja.Size = new System.Drawing.Size(39, 20);
            this.optRestI_Ja.TabIndex = 17;
            this.optRestI_Ja.Text = "Ja";
            this.optRestI_Ja.Click += new System.EventHandler(this.optRest_Click);
            // 
            // optRestV_EjSvar
            // 
            this.optRestV_EjSvar.AutoCheck = false;
            this.optRestV_EjSvar.AutoSize = true;
            this.optRestV_EjSvar.Location = new System.Drawing.Point(397, 38);
            this.optRestV_EjSvar.Name = "optRestV_EjSvar";
            this.optRestV_EjSvar.Size = new System.Drawing.Size(33, 20);
            this.optRestV_EjSvar.TabIndex = 15;
            this.optRestV_EjSvar.Text = "?";
            this.optRestV_EjSvar.Click += new System.EventHandler(this.optRest_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(241, 38);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 16);
            this.label8.TabIndex = 12;
            this.label8.Text = "Vimmel";
            // 
            // optRestV_Nej
            // 
            this.optRestV_Nej.AutoCheck = false;
            this.optRestV_Nej.AutoSize = true;
            this.optRestV_Nej.Location = new System.Drawing.Point(345, 38);
            this.optRestV_Nej.Name = "optRestV_Nej";
            this.optRestV_Nej.Size = new System.Drawing.Size(45, 20);
            this.optRestV_Nej.TabIndex = 14;
            this.optRestV_Nej.Text = "Nej";
            this.optRestV_Nej.Click += new System.EventHandler(this.optRest_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 38);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 16);
            this.label9.TabIndex = 8;
            this.label9.Text = "Omslag";
            // 
            // optRestV_Ja
            // 
            this.optRestV_Ja.AutoCheck = false;
            this.optRestV_Ja.AutoSize = true;
            this.optRestV_Ja.Location = new System.Drawing.Point(301, 38);
            this.optRestV_Ja.Name = "optRestV_Ja";
            this.optRestV_Ja.Size = new System.Drawing.Size(39, 20);
            this.optRestV_Ja.TabIndex = 13;
            this.optRestV_Ja.Text = "Ja";
            this.optRestV_Ja.Click += new System.EventHandler(this.optRest_Click);
            // 
            // optRestO_EjSvar
            // 
            this.optRestO_EjSvar.AutoCheck = false;
            this.optRestO_EjSvar.AutoSize = true;
            this.optRestO_EjSvar.Location = new System.Drawing.Point(200, 38);
            this.optRestO_EjSvar.Name = "optRestO_EjSvar";
            this.optRestO_EjSvar.Size = new System.Drawing.Size(33, 20);
            this.optRestO_EjSvar.TabIndex = 11;
            this.optRestO_EjSvar.Text = "?";
            this.optRestO_EjSvar.Click += new System.EventHandler(this.optRest_Click);
            // 
            // optRestO_Nej
            // 
            this.optRestO_Nej.AutoCheck = false;
            this.optRestO_Nej.AutoSize = true;
            this.optRestO_Nej.Location = new System.Drawing.Point(148, 38);
            this.optRestO_Nej.Name = "optRestO_Nej";
            this.optRestO_Nej.Size = new System.Drawing.Size(45, 20);
            this.optRestO_Nej.TabIndex = 10;
            this.optRestO_Nej.Text = "Nej";
            this.optRestO_Nej.Click += new System.EventHandler(this.optRest_Click);
            // 
            // optRestO_Ja
            // 
            this.optRestO_Ja.AutoCheck = false;
            this.optRestO_Ja.AutoSize = true;
            this.optRestO_Ja.Location = new System.Drawing.Point(104, 38);
            this.optRestO_Ja.Name = "optRestO_Ja";
            this.optRestO_Ja.Size = new System.Drawing.Size(39, 20);
            this.optRestO_Ja.TabIndex = 9;
            this.optRestO_Ja.Text = "Ja";
            this.optRestO_Ja.Click += new System.EventHandler(this.optRest_Click);
            // 
            // optRestP_EjSvar
            // 
            this.optRestP_EjSvar.AutoCheck = false;
            this.optRestP_EjSvar.AutoSize = true;
            this.optRestP_EjSvar.Location = new System.Drawing.Point(397, 18);
            this.optRestP_EjSvar.Name = "optRestP_EjSvar";
            this.optRestP_EjSvar.Size = new System.Drawing.Size(33, 20);
            this.optRestP_EjSvar.TabIndex = 7;
            this.optRestP_EjSvar.Text = "?";
            this.optRestP_EjSvar.Click += new System.EventHandler(this.optRest_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(241, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 16);
            this.label7.TabIndex = 4;
            this.label7.Text = "Porträtt";
            // 
            // optRestP_Nej
            // 
            this.optRestP_Nej.AutoCheck = false;
            this.optRestP_Nej.AutoSize = true;
            this.optRestP_Nej.Location = new System.Drawing.Point(345, 18);
            this.optRestP_Nej.Name = "optRestP_Nej";
            this.optRestP_Nej.Size = new System.Drawing.Size(45, 20);
            this.optRestP_Nej.TabIndex = 6;
            this.optRestP_Nej.Text = "Nej";
            this.optRestP_Nej.Click += new System.EventHandler(this.optRest_Click);
            // 
            // lblRestG
            // 
            this.lblRestG.AutoSize = true;
            this.lblRestG.Location = new System.Drawing.Point(6, 18);
            this.lblRestG.Name = "lblRestG";
            this.lblRestG.Size = new System.Drawing.Size(74, 16);
            this.lblRestG.TabIndex = 0;
            this.lblRestG.Text = "Gruppbilder";
            // 
            // optRestP_Ja
            // 
            this.optRestP_Ja.AutoCheck = false;
            this.optRestP_Ja.AutoSize = true;
            this.optRestP_Ja.Location = new System.Drawing.Point(301, 18);
            this.optRestP_Ja.Name = "optRestP_Ja";
            this.optRestP_Ja.Size = new System.Drawing.Size(39, 20);
            this.optRestP_Ja.TabIndex = 5;
            this.optRestP_Ja.Text = "Ja";
            this.optRestP_Ja.Click += new System.EventHandler(this.optRest_Click);
            // 
            // optRestG_EjSvar
            // 
            this.optRestG_EjSvar.AutoCheck = false;
            this.optRestG_EjSvar.AutoSize = true;
            this.optRestG_EjSvar.Location = new System.Drawing.Point(200, 18);
            this.optRestG_EjSvar.Name = "optRestG_EjSvar";
            this.optRestG_EjSvar.Size = new System.Drawing.Size(33, 20);
            this.optRestG_EjSvar.TabIndex = 3;
            this.optRestG_EjSvar.Text = "?";
            this.optRestG_EjSvar.Click += new System.EventHandler(this.optRest_Click);
            // 
            // optRestG_Nej
            // 
            this.optRestG_Nej.AutoCheck = false;
            this.optRestG_Nej.AutoSize = true;
            this.optRestG_Nej.Location = new System.Drawing.Point(148, 18);
            this.optRestG_Nej.Name = "optRestG_Nej";
            this.optRestG_Nej.Size = new System.Drawing.Size(45, 20);
            this.optRestG_Nej.TabIndex = 2;
            this.optRestG_Nej.Text = "Nej";
            this.optRestG_Nej.Click += new System.EventHandler(this.optRest_Click);
            // 
            // optRestG_Ja
            // 
            this.optRestG_Ja.AutoCheck = false;
            this.optRestG_Ja.AutoSize = true;
            this.optRestG_Ja.Location = new System.Drawing.Point(104, 18);
            this.optRestG_Ja.Name = "optRestG_Ja";
            this.optRestG_Ja.Size = new System.Drawing.Size(39, 20);
            this.optRestG_Ja.TabIndex = 1;
            this.optRestG_Ja.Text = "Ja";
            this.optRestG_Ja.Click += new System.EventHandler(this.optRest_Click);
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(575, 339);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatus.Size = new System.Drawing.Size(445, 181);
            this.txtStatus.TabIndex = 8;
            // 
            // ugG
            // 
            this.ugG.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ugG.Caption = null;
            this.ugG.Location = new System.Drawing.Point(4, 4);
            this.ugG.Name = "ugG";
            this.ugG.ReadOnly = true;
            this.ugG.Size = new System.Drawing.Size(558, 693);
            this.ugG.TabIndex = 0;
            // 
            // grpAnpassadCD
            // 
            this.grpAnpassadCD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grpAnpassadCD.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grpAnpassadCD.Caption = null;
            this.grpAnpassadCD.Controls.Add(this.label2);
            this.grpAnpassadCD.Controls.Add(this.txtAnp_Till);
            this.grpAnpassadCD.Controls.Add(this.optAnp_EjSvar);
            this.grpAnpassadCD.Controls.Add(this.optAnp_Nej);
            this.grpAnpassadCD.Controls.Add(this.optAnp_Ja);
            this.grpAnpassadCD.Location = new System.Drawing.Point(573, 93);
            this.grpAnpassadCD.Name = "grpAnpassadCD";
            this.grpAnpassadCD.Size = new System.Drawing.Size(443, 40);
            this.grpAnpassadCD.TabIndex = 3;
            this.grpAnpassadCD.Text = "Har Programanpassad CD överlämnats?";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(220, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Till:";
            // 
            // txtAnp_Till
            // 
            this.txtAnp_Till.Location = new System.Drawing.Point(254, 14);
            this.txtAnp_Till.Name = "txtAnp_Till";
            this.txtAnp_Till.Size = new System.Drawing.Size(181, 22);
            this.txtAnp_Till.TabIndex = 4;
            // 
            // optAnp_EjSvar
            // 
            this.optAnp_EjSvar.AutoSize = true;
            this.optAnp_EjSvar.Location = new System.Drawing.Point(104, 16);
            this.optAnp_EjSvar.Name = "optAnp_EjSvar";
            this.optAnp_EjSvar.Size = new System.Drawing.Size(33, 20);
            this.optAnp_EjSvar.TabIndex = 2;
            this.optAnp_EjSvar.Text = "?";
            // 
            // optAnp_Nej
            // 
            this.optAnp_Nej.AutoSize = true;
            this.optAnp_Nej.Location = new System.Drawing.Point(52, 16);
            this.optAnp_Nej.Name = "optAnp_Nej";
            this.optAnp_Nej.Size = new System.Drawing.Size(45, 20);
            this.optAnp_Nej.TabIndex = 1;
            this.optAnp_Nej.Text = "Nej";
            // 
            // optAnp_Ja
            // 
            this.optAnp_Ja.AutoSize = true;
            this.optAnp_Ja.Location = new System.Drawing.Point(8, 16);
            this.optAnp_Ja.Name = "optAnp_Ja";
            this.optAnp_Ja.Size = new System.Drawing.Size(39, 20);
            this.optAnp_Ja.TabIndex = 0;
            this.optAnp_Ja.Text = "Ja";
            // 
            // grpPhotoArkiv
            // 
            this.grpPhotoArkiv.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grpPhotoArkiv.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grpPhotoArkiv.Caption = null;
            this.grpPhotoArkiv.Controls.Add(this.label3);
            this.grpPhotoArkiv.Controls.Add(this.txtPArkiv_Till);
            this.grpPhotoArkiv.Controls.Add(this.optPArkiv_EjSvar);
            this.grpPhotoArkiv.Controls.Add(this.optPArkiv_Nej);
            this.grpPhotoArkiv.Controls.Add(this.optPArkiv_Ja);
            this.grpPhotoArkiv.Location = new System.Drawing.Point(573, 139);
            this.grpPhotoArkiv.Name = "grpPhotoArkiv";
            this.grpPhotoArkiv.Size = new System.Drawing.Size(443, 40);
            this.grpPhotoArkiv.TabIndex = 4;
            this.grpPhotoArkiv.Text = "Har PhotoArkiv-CD överlämnats?";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(220, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Till:";
            // 
            // txtPArkiv_Till
            // 
            this.txtPArkiv_Till.Location = new System.Drawing.Point(254, 14);
            this.txtPArkiv_Till.Name = "txtPArkiv_Till";
            this.txtPArkiv_Till.Size = new System.Drawing.Size(181, 22);
            this.txtPArkiv_Till.TabIndex = 4;
            // 
            // optPArkiv_EjSvar
            // 
            this.optPArkiv_EjSvar.AutoSize = true;
            this.optPArkiv_EjSvar.Location = new System.Drawing.Point(104, 16);
            this.optPArkiv_EjSvar.Name = "optPArkiv_EjSvar";
            this.optPArkiv_EjSvar.Size = new System.Drawing.Size(33, 20);
            this.optPArkiv_EjSvar.TabIndex = 2;
            this.optPArkiv_EjSvar.Text = "?";
            // 
            // optPArkiv_Nej
            // 
            this.optPArkiv_Nej.AutoSize = true;
            this.optPArkiv_Nej.Location = new System.Drawing.Point(52, 16);
            this.optPArkiv_Nej.Name = "optPArkiv_Nej";
            this.optPArkiv_Nej.Size = new System.Drawing.Size(45, 20);
            this.optPArkiv_Nej.TabIndex = 1;
            this.optPArkiv_Nej.Text = "Nej";
            // 
            // optPArkiv_Ja
            // 
            this.optPArkiv_Ja.AutoSize = true;
            this.optPArkiv_Ja.Location = new System.Drawing.Point(8, 16);
            this.optPArkiv_Ja.Name = "optPArkiv_Ja";
            this.optPArkiv_Ja.Size = new System.Drawing.Size(39, 20);
            this.optPArkiv_Ja.TabIndex = 0;
            this.optPArkiv_Ja.Text = "Ja";
            // 
            // grpVimmel
            // 
            this.grpVimmel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grpVimmel.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grpVimmel.Caption = null;
            this.grpVimmel.Controls.Add(this.label4);
            this.grpVimmel.Controls.Add(this.txtV_Till);
            this.grpVimmel.Controls.Add(this.optV_EjSvar);
            this.grpVimmel.Controls.Add(this.optV_Nej);
            this.grpVimmel.Controls.Add(this.optV_Ja);
            this.grpVimmel.Location = new System.Drawing.Point(573, 185);
            this.grpVimmel.Name = "grpVimmel";
            this.grpVimmel.Size = new System.Drawing.Size(443, 40);
            this.grpVimmel.TabIndex = 5;
            this.grpVimmel.Text = "Har Vimmel-CD överlämnats?";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(220, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Till:";
            // 
            // txtV_Till
            // 
            this.txtV_Till.Location = new System.Drawing.Point(254, 14);
            this.txtV_Till.Name = "txtV_Till";
            this.txtV_Till.Size = new System.Drawing.Size(181, 22);
            this.txtV_Till.TabIndex = 4;
            // 
            // optV_EjSvar
            // 
            this.optV_EjSvar.AutoSize = true;
            this.optV_EjSvar.Location = new System.Drawing.Point(104, 16);
            this.optV_EjSvar.Name = "optV_EjSvar";
            this.optV_EjSvar.Size = new System.Drawing.Size(33, 20);
            this.optV_EjSvar.TabIndex = 2;
            this.optV_EjSvar.Text = "?";
            // 
            // optV_Nej
            // 
            this.optV_Nej.AutoSize = true;
            this.optV_Nej.Location = new System.Drawing.Point(52, 16);
            this.optV_Nej.Name = "optV_Nej";
            this.optV_Nej.Size = new System.Drawing.Size(45, 20);
            this.optV_Nej.TabIndex = 1;
            this.optV_Nej.Text = "Nej";
            // 
            // optV_Ja
            // 
            this.optV_Ja.AutoSize = true;
            this.optV_Ja.Location = new System.Drawing.Point(8, 16);
            this.optV_Ja.Name = "optV_Ja";
            this.optV_Ja.Size = new System.Drawing.Size(39, 20);
            this.optV_Ja.TabIndex = 0;
            this.optV_Ja.Text = "Ja";
            // 
            // lblBurnTime
            // 
            this.lblBurnTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblBurnTime.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBurnTime.ForeColor = System.Drawing.Color.Red;
            this.lblBurnTime.Location = new System.Drawing.Point(1022, 550);
            this.lblBurnTime.Name = "lblBurnTime";
            this.lblBurnTime.Size = new System.Drawing.Size(193, 32);
            this.lblBurnTime.TabIndex = 13;
            this.lblBurnTime.Text = "datumfält";
            this.lblBurnTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpGranskning
            // 
            this.grpGranskning.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grpGranskning.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grpGranskning.Caption = null;
            this.grpGranskning.Controls.Add(this.lblGranskningSkaGöras);
            this.grpGranskning.Controls.Add(this.label5);
            this.grpGranskning.Controls.Add(this.txtValidateGP_Av);
            this.grpGranskning.Controls.Add(this.optValidateGP_EjSvar);
            this.grpGranskning.Controls.Add(this.optValidateGP_Nej);
            this.grpGranskning.Controls.Add(this.optValidateGP_Ja);
            this.grpGranskning.Location = new System.Drawing.Point(573, 277);
            this.grpGranskning.Name = "grpGranskning";
            this.grpGranskning.Size = new System.Drawing.Size(443, 56);
            this.grpGranskning.TabIndex = 7;
            this.grpGranskning.Text = "Har skolan granskat och godkänt gruppbilderna?";
            // 
            // lblGranskningSkaGöras
            // 
            this.lblGranskningSkaGöras.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblGranskningSkaGöras.ForeColor = System.Drawing.Color.Maroon;
            this.lblGranskningSkaGöras.Location = new System.Drawing.Point(5, 38);
            this.lblGranskningSkaGöras.Name = "lblGranskningSkaGöras";
            this.lblGranskningSkaGöras.Size = new System.Drawing.Size(430, 14);
            this.lblGranskningSkaGöras.TabIndex = 5;
            this.lblGranskningSkaGöras.Text = "ENLIGT FOTOORDERN SKA DETTA GÖRAS!";
            this.lblGranskningSkaGöras.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(220, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 16);
            this.label5.TabIndex = 3;
            this.label5.Text = "Av:";
            // 
            // txtValidateGP_Av
            // 
            this.txtValidateGP_Av.Location = new System.Drawing.Point(254, 14);
            this.txtValidateGP_Av.Name = "txtValidateGP_Av";
            this.txtValidateGP_Av.Size = new System.Drawing.Size(181, 22);
            this.txtValidateGP_Av.TabIndex = 4;
            // 
            // optValidateGP_EjSvar
            // 
            this.optValidateGP_EjSvar.AutoSize = true;
            this.optValidateGP_EjSvar.Location = new System.Drawing.Point(104, 16);
            this.optValidateGP_EjSvar.Name = "optValidateGP_EjSvar";
            this.optValidateGP_EjSvar.Size = new System.Drawing.Size(33, 20);
            this.optValidateGP_EjSvar.TabIndex = 2;
            this.optValidateGP_EjSvar.Text = "?";
            // 
            // optValidateGP_Nej
            // 
            this.optValidateGP_Nej.AutoSize = true;
            this.optValidateGP_Nej.Location = new System.Drawing.Point(52, 16);
            this.optValidateGP_Nej.Name = "optValidateGP_Nej";
            this.optValidateGP_Nej.Size = new System.Drawing.Size(45, 20);
            this.optValidateGP_Nej.TabIndex = 1;
            this.optValidateGP_Nej.Text = "Nej";
            // 
            // optValidateGP_Ja
            // 
            this.optValidateGP_Ja.AutoSize = true;
            this.optValidateGP_Ja.Location = new System.Drawing.Point(8, 16);
            this.optValidateGP_Ja.Name = "optValidateGP_Ja";
            this.optValidateGP_Ja.Size = new System.Drawing.Size(39, 20);
            this.optValidateGP_Ja.TabIndex = 0;
            this.optValidateGP_Ja.Text = "Ja";
            // 
            // chkWhoAmI
            // 
            this.chkWhoAmI.AutoSize = true;
            this.chkWhoAmI.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkWhoAmI.Location = new System.Drawing.Point(581, 552);
            this.chkWhoAmI.Name = "chkWhoAmI";
            this.chkWhoAmI.Size = new System.Drawing.Size(62, 20);
            this.chkWhoAmI.TabIndex = 10;
            this.chkWhoAmI.Text = "Jag är";
            this.chkWhoAmI.UseVisualStyleBackColor = false;
            // 
            // cmdFTP
            // 
            this.cmdFTP.Location = new System.Drawing.Point(831, 526);
            this.cmdFTP.Name = "cmdFTP";
            this.cmdFTP.Size = new System.Drawing.Size(185, 28);
            this.cmdFTP.TabIndex = 11;
            this.cmdFTP.Text = "Slutlagra via FTP";
            this.cmdFTP.Click += new System.EventHandler(this.cmdFTP_Click);
            // 
            // grpLappar
            // 
            this.grpLappar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grpLappar.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grpLappar.Caption = null;
            this.grpLappar.Controls.Add(this.label1);
            this.grpLappar.Controls.Add(this.txtPlappar_Kommentar);
            this.grpLappar.Controls.Add(this.optPlappar_EjSvar);
            this.grpLappar.Controls.Add(this.optPlappar_Nej);
            this.grpLappar.Controls.Add(this.optPlappar_Ja);
            this.grpLappar.Location = new System.Drawing.Point(573, 231);
            this.grpLappar.Name = "grpLappar";
            this.grpLappar.Size = new System.Drawing.Size(443, 40);
            this.grpLappar.TabIndex = 6;
            this.grpLappar.Text = "Har några påseendelappar delats ut?";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(169, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Kommentar:";
            // 
            // txtPlappar_Kommentar
            // 
            this.txtPlappar_Kommentar.Location = new System.Drawing.Point(254, 14);
            this.txtPlappar_Kommentar.Name = "txtPlappar_Kommentar";
            this.txtPlappar_Kommentar.Size = new System.Drawing.Size(181, 22);
            this.txtPlappar_Kommentar.TabIndex = 4;
            // 
            // optPlappar_EjSvar
            // 
            this.optPlappar_EjSvar.AutoSize = true;
            this.optPlappar_EjSvar.Location = new System.Drawing.Point(104, 16);
            this.optPlappar_EjSvar.Name = "optPlappar_EjSvar";
            this.optPlappar_EjSvar.Size = new System.Drawing.Size(33, 20);
            this.optPlappar_EjSvar.TabIndex = 2;
            this.optPlappar_EjSvar.Text = "?";
            // 
            // optPlappar_Nej
            // 
            this.optPlappar_Nej.AutoSize = true;
            this.optPlappar_Nej.Location = new System.Drawing.Point(52, 16);
            this.optPlappar_Nej.Name = "optPlappar_Nej";
            this.optPlappar_Nej.Size = new System.Drawing.Size(45, 20);
            this.optPlappar_Nej.TabIndex = 1;
            this.optPlappar_Nej.Text = "Nej";
            // 
            // optPlappar_Ja
            // 
            this.optPlappar_Ja.AutoSize = true;
            this.optPlappar_Ja.Location = new System.Drawing.Point(8, 16);
            this.optPlappar_Ja.Name = "optPlappar_Ja";
            this.optPlappar_Ja.Size = new System.Drawing.Size(39, 20);
            this.optPlappar_Ja.TabIndex = 0;
            this.optPlappar_Ja.Text = "Ja";
            // 
            // tabPageFinalBurn
            // 
            this.Controls.Add(this.grpLappar);
            this.Controls.Add(this.cmdFTP);
            this.Controls.Add(this.chkWhoAmI);
            this.Controls.Add(this.grpGranskning);
            this.Controls.Add(this.lblBurnTime);
            this.Controls.Add(this.grpVimmel);
            this.Controls.Add(this.grpPhotoArkiv);
            this.Controls.Add(this.grpAnpassadCD);
            this.Controls.Add(this.ugG);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.grpRestfoto);
            this.Controls.Add(this.chkOrderCorrect);
            this.Controls.Add(this.cmdFinalCopy);
            this.Controls.Add(this.cmdBurnDVD);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "tabPageFinalBurn";
            this.Size = new System.Drawing.Size(1024, 700);
            ((System.ComponentModel.ISupportInitialize)(this.grpRestfoto)).EndInit();
            this.grpRestfoto.ResumeLayout(false);
            this.grpRestfoto.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpAnpassadCD)).EndInit();
            this.grpAnpassadCD.ResumeLayout(false);
            this.grpAnpassadCD.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpPhotoArkiv)).EndInit();
            this.grpPhotoArkiv.ResumeLayout(false);
            this.grpPhotoArkiv.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpVimmel)).EndInit();
            this.grpVimmel.ResumeLayout(false);
            this.grpVimmel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpGranskning)).EndInit();
            this.grpGranskning.ResumeLayout(false);
            this.grpGranskning.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpLappar)).EndInit();
            this.grpLappar.ResumeLayout(false);
            this.grpLappar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private bool checkOK()
		{
			(this as IBSTab).save();

		    var fReAsk =
                _skola.RestfotoGrupp == PlataDM.Answer.Unknown ||
                !chkOrderCorrect.Checked ||
		        !chkWhoAmI.Checked;
            if (Global.Preferences.Brand == Brand.Photomic)
            {
                fReAsk |=
                    _skola.RestfotoPorträtt == PlataDM.Answer.Unknown ||
                    _skola.RestfotoOmslag == PlataDM.Answer.Unknown ||
                    _skola.RestfotoVimmel == PlataDM.Answer.Unknown ||
                    _skola.RestfotoInfällning == PlataDM.Answer.Unknown ||
                    _skola.DidBurnCustomProgCD == PlataDM.Answer.Unknown ||
                    _skola.DidBurnPhotoArkivCD == PlataDM.Answer.Unknown ||
                    _skola.DidBurnEnvironmentCD == PlataDM.Answer.Unknown ||
                    _skola.DidValidateGroupPictures == PlataDM.Answer.Unknown ||
                    _skola.DidHandoutPPapers == PlataDM.Answer.Unknown;

                if (_skola.DidBurnCustomProgCD == PlataDM.Answer.Yes)
                    fReAsk |= _skola.ReceiverOfCustomProgCD.Length < 3;
                if (_skola.DidBurnPhotoArkivCD == PlataDM.Answer.Yes)
                    fReAsk |= _skola.ReceiverOfPhotoArkivCD.Length < 3;
                if (_skola.DidBurnEnvironmentCD == PlataDM.Answer.Yes)
                    fReAsk |= _skola.ReceiverOfEnvironmentCD.Length < 3;
                if (_skola.DidValidateGroupPictures == PlataDM.Answer.Yes)
                    fReAsk |= _skola.GroupPicturesValidatedBy.Length < 3;
            }

		    if ( fReAsk )
		    {
		        var msg = "Du har inte besvarat alla frågor än!";
                if (Global.Preferences.Brand == Brand.Photomic)
                    msg +=
                        " Du måste svara Ja eller Nej på alla (och om du svarar Ja måste du i förekommande fall änven ange vilken person (minst tre tecken krävs!) du överlämnat material till eller som granskat gruppbilderna)";
				Global.showMsgBox( this, msg );
				return false;
			}

		    var dr = FAskWithCheckbox.askDialog(
		        this.FindForm(),
		        "Det här innebär att du är helt klar med ordern och att du inte kan arbeta med den längre. Ordern skickas slutgiltigt över till Viron och den skrivskyddas på din dator. Är du säker på att du är helt klar?");
			if (dr != DialogResult.Yes)
				return false;

			_skola.Fotograf = Global.Skola.Fotograf;
			return true;
		}

		private void cmdBurnDVD_Click( object sender, EventArgs e )
		{
		    if (!checkOK())
		        return;

		    _skola.Verifierad = true;
		    _skola.save(true);
		    var alFiles = new List<BurnFileInfo>();
		    foreach (var s in Directory.GetFiles(Global.Skola.HomePath))
		    {
		        var strFN = Path.GetFileName(s);
		        var bfi = new BurnFileInfo(s, null, strFN, false);
		        switch (Path.GetFileName(strFN).ToLower())
		        {
		            case "!fotoorder.emf":
		                continue;
		            case "!order.plata":
		                bfi.OnAll = true;
		                break;
		        }
		        alFiles.Add(bfi);
		    }
		    Burn.BurnHelpers.theNewAndFunBurn(
		        this.FindForm(),
		        alFiles,
		        "K",
		        string.Format("{0}_{1}_K",
		                      Global.Skola.OrderNr,
		                      Global.Preferences.Fotografnummer));
		}

	    private void cmdFinalCopy_Click( object sender, EventArgs e )
		{
            if (string.IsNullOrEmpty(Global.Preferences.BackupFolder) || !Directory.Exists(Global.Preferences.BackupFolder))
            {
                Global.showMsgBox(this, "Du måste ange en existerande mapp under Programinställningar!");
                return;
            }

			if ( !checkOK() )
				return;

			_skola.Verifierad = true;
			_skola.save( true );
            _skola.ReadOnly = true;

			if ( FBackup.showDialog(
				    this.FindForm(),
				    false,
				    string.Format( "Slutlagring av {0} {1}", Global.Skola.OrderNr, Global.Skola.Namn ),
				    Global.Skola.HomePath,
				    Path.Combine( Global.Preferences.BackupFolder, Path.GetFileName( Global.Skola.HomePath ) ) ) != DialogResult.OK  )
			{
                _skola.Verifierad = false;
                _skola.ReadOnly = false;
			}

		}

		private void cmdFTP_Click( object sender, EventArgs e )
		{
			if ( !checkOK() )
				return;

			_skola.Verifierad = true;
			_skola.save( true );
			_skola.ReadOnly = true;
			(FMain.theOneForm.jumpToForm( FlikTyp.FTP ) as FFTP).BeginUpload( _skola );
		}

		private void optRest_Click( object sender, EventArgs e )
		{
			var r = sender as RadioButton;
			foreach ( var v in _optRest.Values )
			{
				var i = v.IndexOf( r );
				if ( i < 0 )
					continue;
				r.Checked = true;
				v[(i + 1) % 3].Checked = false;
				v[(i + 2) % 3].Checked = false;
				break;
			}
		}

	}

}
