using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace Plata
{
	/// <summary>
	/// Summary description for frmInställningar.
	/// </summary>
	public class frmInställningar : vdUsr.baseGradientForm
	{
		private GroupBox groupBox1;
		private ImageList imageList1;
		private TextBox txtKatalog;
		private Button cmdBrowseKatalog;
		private Button cmdEnd;
		private Button cmdOK;
		private RadioButton optMoturs;
		private RadioButton optMedurs;
		private System.Windows.Forms.GroupBox grp1;
		private System.Windows.Forms.GroupBox grp2;
		private System.Windows.Forms.GroupBox groupBox2;
		private vdUsr.vdNumUpDown numFotografnummer;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox chkPortFull;
		private System.Windows.Forms.CheckBox chkGruppFull;
		private System.Windows.Forms.Button cmdBrowseBackup;
		private System.Windows.Forms.TextBox txtBackupFolder;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cboLjudGrupp;
		private System.Windows.Forms.ComboBox cboLjudPort;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.RadioButton optEfternamn;
		private System.Windows.Forms.RadioButton optFörnamn;
		private Label lblFotograf;
        private GroupBox groupBox7;
		private RadioButton optNoCamera;
		private GroupBox grp3;
		private Button cmdAutoUpdate;
		private TextBox txtAutoUpdate;
		private GroupBox groupBox8;
		private RadioButton optISO_Newest;
		private RadioButton optISO_None;
		private RadioButton optISO_Oldest;
        private RadioButton optEd210Camera;
        private RadioButton optEd207Camera;
        private RadioButton optFujiS5;
		private System.ComponentModel.IContainer components;

		public frmInställningar()
		{
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInställningar));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.optMoturs = new System.Windows.Forms.RadioButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.optMedurs = new System.Windows.Forms.RadioButton();
            this.grp1 = new System.Windows.Forms.GroupBox();
            this.cmdBrowseKatalog = new System.Windows.Forms.Button();
            this.txtKatalog = new System.Windows.Forms.TextBox();
            this.grp2 = new System.Windows.Forms.GroupBox();
            this.cmdBrowseBackup = new System.Windows.Forms.Button();
            this.txtBackupFolder = new System.Windows.Forms.TextBox();
            this.cmdEnd = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblFotograf = new System.Windows.Forms.Label();
            this.numFotografnummer = new vdUsr.vdNumUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkPortFull = new System.Windows.Forms.CheckBox();
            this.chkGruppFull = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboLjudPort = new System.Windows.Forms.ComboBox();
            this.cboLjudGrupp = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.optEfternamn = new System.Windows.Forms.RadioButton();
            this.optFörnamn = new System.Windows.Forms.RadioButton();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.optFujiS5 = new System.Windows.Forms.RadioButton();
            this.optEd207Camera = new System.Windows.Forms.RadioButton();
            this.optEd210Camera = new System.Windows.Forms.RadioButton();
            this.optNoCamera = new System.Windows.Forms.RadioButton();
            this.grp3 = new System.Windows.Forms.GroupBox();
            this.cmdAutoUpdate = new System.Windows.Forms.Button();
            this.txtAutoUpdate = new System.Windows.Forms.TextBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.optISO_Oldest = new System.Windows.Forms.RadioButton();
            this.optISO_Newest = new System.Windows.Forms.RadioButton();
            this.optISO_None = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.grp1.SuspendLayout();
            this.grp2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFotografnummer)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.grp3.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.groupBox1.Controls.Add(this.optMoturs);
            this.groupBox1.Controls.Add(this.optMedurs);
            this.groupBox1.Location = new System.Drawing.Point(246, 268);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(278, 84);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rotering av porträttfoton";
            // 
            // optMoturs
            // 
            this.optMoturs.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.optMoturs.ImageIndex = 1;
            this.optMoturs.ImageList = this.imageList1;
            this.optMoturs.Location = new System.Drawing.Point(164, 18);
            this.optMoturs.Name = "optMoturs";
            this.optMoturs.Size = new System.Drawing.Size(108, 60);
            this.optMoturs.TabIndex = 1;
            this.optMoturs.Text = "Moturs";
            this.optMoturs.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            // 
            // optMedurs
            // 
            this.optMedurs.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.optMedurs.ImageIndex = 0;
            this.optMedurs.ImageList = this.imageList1;
            this.optMedurs.Location = new System.Drawing.Point(12, 18);
            this.optMedurs.Name = "optMedurs";
            this.optMedurs.Size = new System.Drawing.Size(112, 60);
            this.optMedurs.TabIndex = 0;
            this.optMedurs.Text = "Medurs";
            this.optMedurs.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // grp1
            // 
            this.grp1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grp1.Controls.Add(this.cmdBrowseKatalog);
            this.grp1.Controls.Add(this.txtKatalog);
            this.grp1.Location = new System.Drawing.Point(12, 16);
            this.grp1.Name = "grp1";
            this.grp1.Size = new System.Drawing.Size(250, 48);
            this.grp1.TabIndex = 0;
            this.grp1.TabStop = false;
            this.grp1.Text = "Arbetsmapp";
            // 
            // cmdBrowseKatalog
            // 
            this.cmdBrowseKatalog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseKatalog.BackColor = System.Drawing.SystemColors.Control;
            this.cmdBrowseKatalog.Location = new System.Drawing.Point(214, 20);
            this.cmdBrowseKatalog.Name = "cmdBrowseKatalog";
            this.cmdBrowseKatalog.Size = new System.Drawing.Size(28, 20);
            this.cmdBrowseKatalog.TabIndex = 1;
            this.cmdBrowseKatalog.Text = "...";
            this.cmdBrowseKatalog.UseVisualStyleBackColor = false;
            this.cmdBrowseKatalog.Click += new System.EventHandler(this.cmdBrowseKatalog_Click);
            // 
            // txtKatalog
            // 
            this.txtKatalog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtKatalog.Location = new System.Drawing.Point(8, 20);
            this.txtKatalog.Name = "txtKatalog";
            this.txtKatalog.Size = new System.Drawing.Size(202, 20);
            this.txtKatalog.TabIndex = 0;
            // 
            // grp2
            // 
            this.grp2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grp2.Controls.Add(this.cmdBrowseBackup);
            this.grp2.Controls.Add(this.txtBackupFolder);
            this.grp2.Location = new System.Drawing.Point(274, 16);
            this.grp2.Name = "grp2";
            this.grp2.Size = new System.Drawing.Size(250, 48);
            this.grp2.TabIndex = 1;
            this.grp2.TabStop = false;
            this.grp2.Text = "Backupmapp";
            // 
            // cmdBrowseBackup
            // 
            this.cmdBrowseBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseBackup.BackColor = System.Drawing.SystemColors.Control;
            this.cmdBrowseBackup.Location = new System.Drawing.Point(214, 20);
            this.cmdBrowseBackup.Name = "cmdBrowseBackup";
            this.cmdBrowseBackup.Size = new System.Drawing.Size(28, 20);
            this.cmdBrowseBackup.TabIndex = 1;
            this.cmdBrowseBackup.Text = "...";
            this.cmdBrowseBackup.UseVisualStyleBackColor = false;
            this.cmdBrowseBackup.Click += new System.EventHandler(this.cmdBrowseVimmel_Click);
            // 
            // txtBackupFolder
            // 
            this.txtBackupFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBackupFolder.Location = new System.Drawing.Point(8, 20);
            this.txtBackupFolder.Name = "txtBackupFolder";
            this.txtBackupFolder.Size = new System.Drawing.Size(202, 20);
            this.txtBackupFolder.TabIndex = 0;
            // 
            // cmdEnd
            // 
            this.cmdEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdEnd.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdEnd.Location = new System.Drawing.Point(444, 427);
            this.cmdEnd.Name = "cmdEnd";
            this.cmdEnd.Size = new System.Drawing.Size(80, 28);
            this.cmdEnd.TabIndex = 12;
            this.cmdEnd.Text = "Avbryt";
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(358, 427);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(80, 28);
            this.cmdOK.TabIndex = 11;
            this.cmdOK.Text = "OK";
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.groupBox2.Controls.Add(this.lblFotograf);
            this.groupBox2.Controls.Add(this.numFotografnummer);
            this.groupBox2.Location = new System.Drawing.Point(274, 70);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(250, 48);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Fotograf";
            // 
            // lblFotograf
            // 
            this.lblFotograf.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFotograf.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.lblFotograf.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblFotograf.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFotograf.Location = new System.Drawing.Point(58, 16);
            this.lblFotograf.Name = "lblFotograf";
            this.lblFotograf.Size = new System.Drawing.Size(186, 20);
            this.lblFotograf.TabIndex = 1;
            this.lblFotograf.Text = "-";
            this.lblFotograf.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numFotografnummer
            // 
            this.numFotografnummer.Location = new System.Drawing.Point(8, 16);
            this.numFotografnummer.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numFotografnummer.Name = "numFotografnummer";
            this.numFotografnummer.Size = new System.Drawing.Size(44, 20);
            this.numFotografnummer.TabIndex = 0;
            this.numFotografnummer.Value = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numFotografnummer.ValueChanged += new System.EventHandler(this.numFotografnummer_ValueChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.chkPortFull);
            this.groupBox3.Controls.Add(this.chkGruppFull);
            this.groupBox3.Location = new System.Drawing.Point(12, 124);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(512, 64);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Automatiskt fullskärmsläge vid fotografering";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(256, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(212, 44);
            this.label1.TabIndex = 2;
            this.label1.Text = "Den här funktionen gör att fullskärmsläget automatiskt aktiveras varje gång du ta" +
    "r en ny bild.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkPortFull
            // 
            this.chkPortFull.AutoSize = true;
            this.chkPortFull.Location = new System.Drawing.Point(16, 40);
            this.chkPortFull.Name = "chkPortFull";
            this.chkPortFull.Size = new System.Drawing.Size(136, 17);
            this.chkPortFull.TabIndex = 1;
            this.chkPortFull.Text = "Vid porträttfotografering";
            // 
            // chkGruppFull
            // 
            this.chkGruppFull.AutoSize = true;
            this.chkGruppFull.Location = new System.Drawing.Point(16, 20);
            this.chkGruppFull.Name = "chkGruppFull";
            this.chkGruppFull.Size = new System.Drawing.Size(130, 17);
            this.chkGruppFull.TabIndex = 0;
            this.chkGruppFull.Text = "Vid gruppfotografering";
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.cboLjudPort);
            this.groupBox5.Controls.Add(this.cboLjudGrupp);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Location = new System.Drawing.Point(12, 194);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(512, 68);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Spela ett ljud när en bild tagits emot";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Porträtt";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Grupp";
            // 
            // cboLjudPort
            // 
            this.cboLjudPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLjudPort.Location = new System.Drawing.Point(72, 40);
            this.cboLjudPort.Name = "cboLjudPort";
            this.cboLjudPort.Size = new System.Drawing.Size(124, 21);
            this.cboLjudPort.TabIndex = 3;
            this.cboLjudPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboLjud_KeyPress);
            // 
            // cboLjudGrupp
            // 
            this.cboLjudGrupp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLjudGrupp.Location = new System.Drawing.Point(72, 16);
            this.cboLjudGrupp.Name = "cboLjudGrupp";
            this.cboLjudGrupp.Size = new System.Drawing.Size(124, 21);
            this.cboLjudGrupp.TabIndex = 1;
            this.cboLjudGrupp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboLjud_KeyPress);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(236, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(272, 44);
            this.label2.TabIndex = 4;
            this.label2.Text = "Markera ett ljud och tryck blanksteg för att provlyssna. Ställ volymen genom att " +
    "dubbelklicka på högtalarikonen på statusraden.";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.groupBox6.Controls.Add(this.optEfternamn);
            this.groupBox6.Controls.Add(this.optFörnamn);
            this.groupBox6.Location = new System.Drawing.Point(246, 358);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(228, 38);
            this.groupBox6.TabIndex = 9;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Sorteringsordning Personer";
            // 
            // optEfternamn
            // 
            this.optEfternamn.Location = new System.Drawing.Point(104, 16);
            this.optEfternamn.Name = "optEfternamn";
            this.optEfternamn.Size = new System.Drawing.Size(80, 16);
            this.optEfternamn.TabIndex = 1;
            this.optEfternamn.Text = "Efternamn";
            // 
            // optFörnamn
            // 
            this.optFörnamn.Location = new System.Drawing.Point(8, 16);
            this.optFörnamn.Name = "optFörnamn";
            this.optFörnamn.Size = new System.Drawing.Size(80, 16);
            this.optFörnamn.TabIndex = 0;
            this.optFörnamn.Text = "Förnamn";
            // 
            // groupBox7
            // 
            this.groupBox7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.groupBox7.Controls.Add(this.optFujiS5);
            this.groupBox7.Controls.Add(this.optEd207Camera);
            this.groupBox7.Controls.Add(this.optEd210Camera);
            this.groupBox7.Controls.Add(this.optNoCamera);
            this.groupBox7.Location = new System.Drawing.Point(12, 268);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(228, 98);
            this.groupBox7.TabIndex = 6;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Kamera-API";
            // 
            // optFujiS5
            // 
            this.optFujiS5.AutoSize = true;
            this.optFujiS5.Location = new System.Drawing.Point(9, 76);
            this.optFujiS5.Name = "optFujiS5";
            this.optFujiS5.Size = new System.Drawing.Size(57, 17);
            this.optFujiS5.TabIndex = 4;
            this.optFujiS5.Text = "Fuji S5";
            // 
            // optEd207Camera
            // 
            this.optEd207Camera.AutoSize = true;
            this.optEd207Camera.Location = new System.Drawing.Point(9, 36);
            this.optEd207Camera.Name = "optEd207Camera";
            this.optEd207Camera.Size = new System.Drawing.Size(201, 17);
            this.optEd207Camera.TabIndex = 2;
            this.optEd207Camera.Text = "ED 2.7: EOS-1 Ds Mark II &&  EOS 5D";
            // 
            // optEd210Camera
            // 
            this.optEd210Camera.AutoSize = true;
            this.optEd210Camera.Location = new System.Drawing.Point(9, 56);
            this.optEd210Camera.Name = "optEd210Camera";
            this.optEd210Camera.Size = new System.Drawing.Size(145, 17);
            this.optEd210Camera.TabIndex = 3;
            this.optEd210Camera.Text = "ED 2.10: EOS 5D Mark II";
            // 
            // optNoCamera
            // 
            this.optNoCamera.AutoSize = true;
            this.optNoCamera.Location = new System.Drawing.Point(8, 16);
            this.optNoCamera.Name = "optNoCamera";
            this.optNoCamera.Size = new System.Drawing.Size(167, 17);
            this.optNoCamera.TabIndex = 0;
            this.optNoCamera.Text = "Ingen kamera (för internt bruk)";
            // 
            // grp3
            // 
            this.grp3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grp3.Controls.Add(this.cmdAutoUpdate);
            this.grp3.Controls.Add(this.txtAutoUpdate);
            this.grp3.Location = new System.Drawing.Point(12, 70);
            this.grp3.Name = "grp3";
            this.grp3.Size = new System.Drawing.Size(250, 48);
            this.grp3.TabIndex = 2;
            this.grp3.TabStop = false;
            this.grp3.Text = "Autouppdatera från mapp";
            // 
            // cmdAutoUpdate
            // 
            this.cmdAutoUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdAutoUpdate.BackColor = System.Drawing.SystemColors.Control;
            this.cmdAutoUpdate.Location = new System.Drawing.Point(214, 20);
            this.cmdAutoUpdate.Name = "cmdAutoUpdate";
            this.cmdAutoUpdate.Size = new System.Drawing.Size(28, 20);
            this.cmdAutoUpdate.TabIndex = 1;
            this.cmdAutoUpdate.Text = "...";
            this.cmdAutoUpdate.UseVisualStyleBackColor = false;
            this.cmdAutoUpdate.Click += new System.EventHandler(this.cmdAutoUpdate_Click);
            // 
            // txtAutoUpdate
            // 
            this.txtAutoUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAutoUpdate.Location = new System.Drawing.Point(8, 20);
            this.txtAutoUpdate.Name = "txtAutoUpdate";
            this.txtAutoUpdate.Size = new System.Drawing.Size(202, 20);
            this.txtAutoUpdate.TabIndex = 0;
            // 
            // groupBox8
            // 
            this.groupBox8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.groupBox8.Controls.Add(this.optISO_Oldest);
            this.groupBox8.Controls.Add(this.optISO_Newest);
            this.groupBox8.Controls.Add(this.optISO_None);
            this.groupBox8.Location = new System.Drawing.Point(12, 374);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(228, 78);
            this.groupBox8.TabIndex = 8;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Sorteringsordning Bildimport";
            // 
            // optISO_Oldest
            // 
            this.optISO_Oldest.AutoSize = true;
            this.optISO_Oldest.Location = new System.Drawing.Point(8, 56);
            this.optISO_Oldest.Name = "optISO_Oldest";
            this.optISO_Oldest.Size = new System.Drawing.Size(90, 17);
            this.optISO_Oldest.TabIndex = 2;
            this.optISO_Oldest.Text = "Äldst bild först";
            // 
            // optISO_Newest
            // 
            this.optISO_Newest.AutoSize = true;
            this.optISO_Newest.Location = new System.Drawing.Point(8, 36);
            this.optISO_Newest.Name = "optISO_Newest";
            this.optISO_Newest.Size = new System.Drawing.Size(94, 17);
            this.optISO_Newest.TabIndex = 1;
            this.optISO_Newest.Text = "Nyast bild först";
            // 
            // optISO_None
            // 
            this.optISO_None.AutoSize = true;
            this.optISO_None.Location = new System.Drawing.Point(8, 16);
            this.optISO_None.Name = "optISO_None";
            this.optISO_None.Size = new System.Drawing.Size(94, 17);
            this.optISO_None.TabIndex = 0;
            this.optISO_None.Text = "Ingen (filnamn)";
            // 
            // frmInställningar
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cmdEnd;
            this.ClientSize = new System.Drawing.Size(536, 467);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.grp3);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cmdEnd);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.grp2);
            this.Controls.Add(this.grp1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInställningar";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inställningar";
            this.groupBox1.ResumeLayout(false);
            this.grp1.ResumeLayout(false);
            this.grp1.PerformLayout();
            this.grp2.ResumeLayout(false);
            this.grp2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numFotografnummer)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.grp3.ResumeLayout(false);
            this.grp3.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void browse( TextBox txt, string strTitel )
		{
			using ( var dlg = new FolderBrowserDialog() )
			{
				dlg.Description = strTitel;
				dlg.ShowNewFolderButton = true;
				dlg.SelectedPath = txt.Text;
				if ( dlg.ShowDialog()==DialogResult.OK )
					txt.Text = dlg.SelectedPath;
			}
		}

		private void cmdBrowseKatalog_Click(object sender, System.EventArgs e)
		{
			browse( txtKatalog, grp1.Text );
		}

		private void cmdBrowseVimmel_Click(object sender, System.EventArgs e)
		{
			browse( txtBackupFolder, grp2.Text );
		}

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
		    var strKatalog = txtKatalog.Text;
			var strBackup = txtBackupFolder.Text;
			var strAutoUpdate = txtAutoUpdate.Text;
			if ( !Directory.Exists( strKatalog ) )
			{
				this.DialogResult = DialogResult.None;
				Global.showMsgBox( this, "Mappen \"" + strKatalog + "\" finns inte." );
				return;
			}
			if ( string.Compare(strBackup, Global.Preferences.BackupFolder, StringComparison.OrdinalIgnoreCase) != 0 )
				if ( strBackup.Length!=0 && !Directory.Exists(strBackup) )
				{
					this.DialogResult = DialogResult.None;
					Global.showMsgBox( this, "Mappen \"" + strBackup + "\" finns inte." );
					return;
				}
			if ( string.Compare( strBackup, 0, Application.ExecutablePath, 0, 3, true ) == 0 )
			{
				this.DialogResult = DialogResult.None;
				Global.showMsgBox( this, "Du kan väl inte lägga backuper på samma enhet som du har Plåta på, PUCKO! ;-)" );
				return;
			}
			if ( string.Compare(strAutoUpdate, Global.Preferences.AutoUpdateFolder, StringComparison.OrdinalIgnoreCase) != 0 )
				if ( strAutoUpdate.Length != 0 && !Directory.Exists( strAutoUpdate ) )
				{
					this.DialogResult = DialogResult.None;
					Global.showMsgBox( this, "Mappen \"" + strAutoUpdate + "\" finns inte." );
					return;
				}
            if (optEd207Camera.Checked)
                Global.Preferences.Camera = CameraSdk.Ed207Camera;
            else if (optEd210Camera.Checked)
                Global.Preferences.Camera = CameraSdk.Ed210Camera;
            else if (optFujiS5.Checked)
                Global.Preferences.Camera = CameraSdk.FujiCamera;
            else if (optNoCamera.Checked)
				Global.Preferences.Camera = CameraSdk.NoCamera;
			else
			{
				this.DialogResult = DialogResult.None;
				Global.showMsgBox(this, "Du måste välja ett kamera-API!");
				return;
			}

			Global.Preferences.MainPath = strKatalog;
			Global.Preferences.BackupFolder = strBackup;
			Global.Preferences.AutoUpdateFolder = strAutoUpdate;
			Global.Preferences.FullskärmslägeGruppfoto = chkGruppFull.Checked;
			Global.Preferences.FullskärmslägePorträttfoto = chkPortFull.Checked;
			Global.Preferences.Porträttrotering = optMedurs.Checked ? PlataDM.Rotering.Medurs : PlataDM.Rotering.Moturs;
			Global.Preferences.Fotografnummer = (int)numFotografnummer.Value;

			Global.Preferences.GroupSoundShort = cboLjudGrupp.SelectedIndex>=1 ? cboLjudGrupp.Text : null;
			Global.Preferences.PortraitSoundShort = cboLjudPort.SelectedIndex>=1 ? cboLjudPort.Text : null;
			Global.Preferences.SortOrderLastName = optEfternamn.Checked;

			if ( optISO_Newest.Checked )
				Global.Preferences.ImageSortOrder = ImageSortOrder.NewestFirst;
			else if ( optISO_Oldest.Checked )
				Global.Preferences.ImageSortOrder = ImageSortOrder.OldestFirst;
			else
				Global.Preferences.ImageSortOrder = ImageSortOrder.None;

			Global.sparaInställningar();
		}

		protected override void OnLoad( EventArgs e )
		{
			base.OnLoad( e );

			if ( string.IsNullOrEmpty( Global.Preferences.MainPath ) || !Directory.Exists( Global.Preferences.MainPath ) )
				if ( Directory.Exists( "c:\\PlåtaData" ) )
					Global.Preferences.MainPath = "c:\\PlåtaData";

			txtKatalog.Text = Global.Preferences.MainPath;
			txtBackupFolder.Text = Global.Preferences.BackupFolder;
			txtAutoUpdate.Text = Global.Preferences.AutoUpdateFolder;
			chkGruppFull.Checked = Global.Preferences.FullskärmslägeGruppfoto;
			chkPortFull.Checked = Global.Preferences.FullskärmslägePorträttfoto;
			switch ( Global.Preferences.Porträttrotering )
			{
				case PlataDM.Rotering.Medurs:
					optMedurs.Checked = true;
					break;
				case PlataDM.Rotering.Moturs:
					optMoturs.Checked = true;
					break;
			}

			optNoCamera.Checked = true;
            optEd207Camera.Enabled = vdCamera.vdCamera.CanEd207();
            optEd210Camera.Enabled = vdCamera.vdCamera.CanEd210();
            optFujiS5.Enabled = vdCamera.vdCamera.CanFujiS5();
            switch (Global.Preferences.Camera)
			{
                case CameraSdk.Ed207Camera:
                    if (optEd207Camera.Enabled)
                        optEd207Camera.Checked = true;
                    break;
                case CameraSdk.Ed210Camera:
                    if (optEd210Camera.Enabled)
                        optEd210Camera.Checked = true;
                    break;
                case CameraSdk.FujiCamera:
                    if (optFujiS5.Enabled)
                        optFujiS5.Checked = true;
                    break;
            }
			if ( Global.Fotografdator && Directory.Exists(txtKatalog.Text) )
			{
				txtKatalog.Enabled = false;
				cmdBrowseKatalog.Enabled = false;
			}
			numFotografnummer.Value = Global.Preferences.Fotografnummer;

			prepareSoundCombo( cboLjudGrupp, Global.Preferences.GroupSoundShort );
			prepareSoundCombo( cboLjudPort, Global.Preferences.PortraitSoundShort );
			if ( Global.Preferences.SortOrderLastName )
				optEfternamn.Checked = true;
			else
				optFörnamn.Checked = true;

			switch ( Global.Preferences.ImageSortOrder )
			{
				case ImageSortOrder.NewestFirst:
					optISO_Newest.Checked = true;
					break;
				case ImageSortOrder.OldestFirst:
					optISO_Oldest.Checked = true;
					break;
				default:
					optISO_None.Checked = true;
					break;
			}
		}

		private void prepareSoundCombo( ComboBox cbo, string strSelected )
		{
			cbo.Items.Add( "(inget)" );
			var strDir = UserPreferences.getSoundLong(null,true);
			if ( Directory.Exists( strDir ) )
				foreach ( var s in Directory.GetFiles(strDir,"*.mp3") )
					cbo.Items.Add( Path.GetFileNameWithoutExtension(s) );
			if ( strSelected!=null && cbo.Items.Contains(strSelected) )
				cbo.SelectedItem = strSelected;
			else
				cbo.SelectedIndex = 0;
		}

		private void cboLjud_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if ( e.KeyChar==' ' )
			{
                var cbo = (ComboBox)sender;
				if ( cbo.SelectedIndex<1 )
					return;
				Global.MediaPlayer.Open( UserPreferences.getSoundLong(cbo.Text,false) );
				Global.MediaPlayer.Play();
			}

		}

		private void numFotografnummer_ValueChanged( object sender, EventArgs e )
		{
			lblFotograf.Text = Fotografer.Name( (int)numFotografnummer.Value );
		}

		private void cmdAutoUpdate_Click( object sender, EventArgs e )
		{
			browse( txtAutoUpdate, grp3.Text );
		}

	}

}
