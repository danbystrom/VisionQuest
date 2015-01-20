using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Plata.Camera;
using vdCamera;

namespace Plata
{

	/// <summary>
	/// Summary description for FCameraSettings.
	/// </summary>
	///  
	public class FCameraSettings : vdUsr.baseGradientForm
	{
		private IContainer components;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.RadioButton optNoChange;
		private System.Windows.Forms.RadioButton optGroupIn;
		private System.Windows.Forms.RadioButton optPortraitIn;
		private System.Windows.Forms.RadioButton optVimmel;
		private System.Windows.Forms.RadioButton optGroupOut;
		private System.Windows.Forms.RadioButton optFree;
		private System.Windows.Forms.Button cmdClose;
		private System.Windows.Forms.Button cmdApply;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox cboSlutartid;
		private System.Windows.Forms.TextBox txtSlutartid;
		private System.Windows.Forms.TextBox txtBl�ndarsteg;
		private System.Windows.Forms.ComboBox cboBl�ndarsteg;
		private System.Windows.Forms.TextBox txtVitbalans;
		private System.Windows.Forms.ComboBox cboVitbalans;
		private System.Windows.Forms.TextBox txtISO;
		private System.Windows.Forms.ComboBox cboISO;
		private System.Windows.Forms.TextBox txtFilformat2;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtFilformat1;
		private System.Windows.Forms.TextBox txtMode;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox cboMode;
		private System.Windows.Forms.TextBox txtColorMatrix;
		private System.Windows.Forms.TextBox txtParameter;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.ComboBox cboColorMatrix;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.ComboBox cboParameter;

        private readonly vdCamera.vdCamera _camera;
		private System.Windows.Forms.TextBox txtKelvin2;
		private System.Windows.Forms.TextBox txtKelvin1;
		private System.Windows.Forms.Label label12;
		private Timer timer1;
		private Label label13;
		private TextBox txtSk�rpa1;
		private TextBox txtSk�rpa2;
		private Label label14;
		private TextBox txtKontrast1;
		private TextBox txtKontrast2;
		private TextBox txtF�rgton1;
		private TextBox txtF�rgton2;
		private TextBox txtM�ttnad1;
		private TextBox txtM�ttnad2;
		//private RadioButton optGroupOut;
		private RadioButton optPortraitOut;
        private vdCamera.CameraType _cameraType;

		private FCameraSettings()
		{
			InitializeComponent();
		}

        public FCameraSettings(vdCamera.vdCamera camera)
		{
			InitializeComponent();

			_camera = camera;
			_cameraType = _camera.CameraType;
            //if (_cameraType != vdCamera.CameraType.EOS_5D_MarkII && _cameraType != vdCamera.CameraType.EOS_5D && _cameraType != vdCamera.CameraType.EOS_1Ds_MarkII)
			//	throw new Exception();

            optNoChange.CheckedChanged += opt_CheckedChanged;
            optGroupIn.CheckedChanged += opt_CheckedChanged;
            optGroupOut.CheckedChanged += opt_CheckedChanged;
            optPortraitIn.CheckedChanged += opt_CheckedChanged;
            optPortraitOut.CheckedChanged += opt_CheckedChanged;
            optVimmel.CheckedChanged += opt_CheckedChanged;
            optFree.CheckedChanged += opt_CheckedChanged;
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
            this.label1 = new System.Windows.Forms.Label();
            this.optNoChange = new System.Windows.Forms.RadioButton();
            this.optGroupIn = new System.Windows.Forms.RadioButton();
            this.optPortraitIn = new System.Windows.Forms.RadioButton();
            this.optVimmel = new System.Windows.Forms.RadioButton();
            this.optGroupOut = new System.Windows.Forms.RadioButton();
            this.optFree = new System.Windows.Forms.RadioButton();
            this.cboSlutartid = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSlutartid = new System.Windows.Forms.TextBox();
            this.cmdClose = new System.Windows.Forms.Button();
            this.cmdApply = new System.Windows.Forms.Button();
            this.txtBl�ndarsteg = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboBl�ndarsteg = new System.Windows.Forms.ComboBox();
            this.txtVitbalans = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cboVitbalans = new System.Windows.Forms.ComboBox();
            this.txtISO = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cboISO = new System.Windows.Forms.ComboBox();
            this.txtFilformat2 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtFilformat1 = new System.Windows.Forms.TextBox();
            this.txtMode = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cboMode = new System.Windows.Forms.ComboBox();
            this.txtColorMatrix = new System.Windows.Forms.TextBox();
            this.txtParameter = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cboColorMatrix = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cboParameter = new System.Windows.Forms.ComboBox();
            this.txtKelvin2 = new System.Windows.Forms.TextBox();
            this.txtKelvin1 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label13 = new System.Windows.Forms.Label();
            this.txtSk�rpa1 = new System.Windows.Forms.TextBox();
            this.txtSk�rpa2 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtKontrast1 = new System.Windows.Forms.TextBox();
            this.txtKontrast2 = new System.Windows.Forms.TextBox();
            this.txtF�rgton1 = new System.Windows.Forms.TextBox();
            this.txtF�rgton2 = new System.Windows.Forms.TextBox();
            this.txtM�ttnad1 = new System.Windows.Forms.TextBox();
            this.txtM�ttnad2 = new System.Windows.Forms.TextBox();
            this.optPortraitOut = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(16, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "V�lj �nskad inst�llning:";
            // 
            // optNoChange
            // 
            this.optNoChange.AutoSize = true;
            this.optNoChange.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.optNoChange.Checked = true;
            this.optNoChange.Location = new System.Drawing.Point(32, 32);
            this.optNoChange.Name = "optNoChange";
            this.optNoChange.Size = new System.Drawing.Size(78, 17);
            this.optNoChange.TabIndex = 1;
            this.optNoChange.TabStop = true;
            this.optNoChange.Text = "Of�r�ndrad";
            this.optNoChange.UseVisualStyleBackColor = false;
            // 
            // optGroupIn
            // 
            this.optGroupIn.AutoSize = true;
            this.optGroupIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.optGroupIn.Location = new System.Drawing.Point(32, 56);
            this.optGroupIn.Name = "optGroupIn";
            this.optGroupIn.Size = new System.Drawing.Size(94, 17);
            this.optGroupIn.TabIndex = 0;
            this.optGroupIn.Text = "Gruppbild Inne";
            this.optGroupIn.UseVisualStyleBackColor = false;
            // 
            // optPortraitIn
            // 
            this.optPortraitIn.AutoSize = true;
            this.optPortraitIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.optPortraitIn.Location = new System.Drawing.Point(32, 96);
            this.optPortraitIn.Name = "optPortraitIn";
            this.optPortraitIn.Size = new System.Drawing.Size(83, 17);
            this.optPortraitIn.TabIndex = 4;
            this.optPortraitIn.Text = "Portr�tt Inne";
            this.optPortraitIn.UseVisualStyleBackColor = false;
            // 
            // optVimmel
            // 
            this.optVimmel.AutoSize = true;
            this.optVimmel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.optVimmel.Location = new System.Drawing.Point(148, 76);
            this.optVimmel.Name = "optVimmel";
            this.optVimmel.Size = new System.Drawing.Size(76, 17);
            this.optVimmel.TabIndex = 7;
            this.optVimmel.Text = "Vimmelfoto";
            this.optVimmel.UseVisualStyleBackColor = false;
            // 
            // optGroupOut
            // 
            this.optGroupOut.AutoSize = true;
            this.optGroupOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.optGroupOut.Location = new System.Drawing.Point(32, 76);
            this.optGroupOut.Name = "optGroupOut";
            this.optGroupOut.Size = new System.Drawing.Size(90, 17);
            this.optGroupOut.TabIndex = 3;
            this.optGroupOut.Text = "Gruppbild Ute";
            this.optGroupOut.UseVisualStyleBackColor = false;
            // 
            // optFree
            // 
            this.optFree.AutoSize = true;
            this.optFree.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.optFree.Location = new System.Drawing.Point(148, 96);
            this.optFree.Name = "optFree";
            this.optFree.Size = new System.Drawing.Size(50, 17);
            this.optFree.TabIndex = 9;
            this.optFree.Text = "Egen";
            this.optFree.UseVisualStyleBackColor = false;
            // 
            // cboSlutartid
            // 
            this.cboSlutartid.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSlutartid.Enabled = false;
            this.cboSlutartid.Location = new System.Drawing.Point(111, 172);
            this.cboSlutartid.Name = "cboSlutartid";
            this.cboSlutartid.Size = new System.Drawing.Size(133, 21);
            this.cboSlutartid.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label2.Location = new System.Drawing.Point(12, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Slutartid";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label3.Location = new System.Drawing.Point(108, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "Ny inst�llning";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label4.Location = new System.Drawing.Point(284, 148);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "Aktuell inst�llning";
            // 
            // txtSlutartid
            // 
            this.txtSlutartid.Location = new System.Drawing.Point(284, 172);
            this.txtSlutartid.Name = "txtSlutartid";
            this.txtSlutartid.ReadOnly = true;
            this.txtSlutartid.Size = new System.Drawing.Size(120, 20);
            this.txtSlutartid.TabIndex = 14;
            // 
            // cmdClose
            // 
            this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdClose.Location = new System.Drawing.Point(312, 52);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(88, 28);
            this.cmdClose.TabIndex = 50;
            this.cmdClose.Text = "St�ng";
            // 
            // cmdApply
            // 
            this.cmdApply.Location = new System.Drawing.Point(312, 12);
            this.cmdApply.Name = "cmdApply";
            this.cmdApply.Size = new System.Drawing.Size(88, 28);
            this.cmdApply.TabIndex = 49;
            this.cmdApply.Text = "Verkst�ll";
            this.cmdApply.Click += new System.EventHandler(this.cmdApply_Click);
            // 
            // txtBl�ndarsteg
            // 
            this.txtBl�ndarsteg.Location = new System.Drawing.Point(284, 199);
            this.txtBl�ndarsteg.Name = "txtBl�ndarsteg";
            this.txtBl�ndarsteg.ReadOnly = true;
            this.txtBl�ndarsteg.Size = new System.Drawing.Size(120, 20);
            this.txtBl�ndarsteg.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label5.Location = new System.Drawing.Point(12, 203);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Bl�ndarsteg";
            // 
            // cboBl�ndarsteg
            // 
            this.cboBl�ndarsteg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBl�ndarsteg.Enabled = false;
            this.cboBl�ndarsteg.Location = new System.Drawing.Point(111, 199);
            this.cboBl�ndarsteg.Name = "cboBl�ndarsteg";
            this.cboBl�ndarsteg.Size = new System.Drawing.Size(133, 21);
            this.cboBl�ndarsteg.TabIndex = 16;
            // 
            // txtVitbalans
            // 
            this.txtVitbalans.Location = new System.Drawing.Point(284, 253);
            this.txtVitbalans.Name = "txtVitbalans";
            this.txtVitbalans.ReadOnly = true;
            this.txtVitbalans.Size = new System.Drawing.Size(120, 20);
            this.txtVitbalans.TabIndex = 23;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label6.Location = new System.Drawing.Point(12, 257);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Vitbalans";
            // 
            // cboVitbalans
            // 
            this.cboVitbalans.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVitbalans.Enabled = false;
            this.cboVitbalans.Location = new System.Drawing.Point(111, 253);
            this.cboVitbalans.Name = "cboVitbalans";
            this.cboVitbalans.Size = new System.Drawing.Size(133, 21);
            this.cboVitbalans.TabIndex = 22;
            // 
            // txtISO
            // 
            this.txtISO.Location = new System.Drawing.Point(284, 226);
            this.txtISO.Name = "txtISO";
            this.txtISO.ReadOnly = true;
            this.txtISO.Size = new System.Drawing.Size(120, 20);
            this.txtISO.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label7.Location = new System.Drawing.Point(12, 230);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "ISO-tal";
            // 
            // cboISO
            // 
            this.cboISO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboISO.Enabled = false;
            this.cboISO.Location = new System.Drawing.Point(111, 226);
            this.cboISO.Name = "cboISO";
            this.cboISO.Size = new System.Drawing.Size(133, 21);
            this.cboISO.TabIndex = 19;
            // 
            // txtFilformat2
            // 
            this.txtFilformat2.Location = new System.Drawing.Point(284, 439);
            this.txtFilformat2.Name = "txtFilformat2";
            this.txtFilformat2.ReadOnly = true;
            this.txtFilformat2.Size = new System.Drawing.Size(120, 20);
            this.txtFilformat2.TabIndex = 48;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label8.Location = new System.Drawing.Point(12, 446);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 46;
            this.label8.Text = "Filformat";
            // 
            // txtFilformat1
            // 
            this.txtFilformat1.Enabled = false;
            this.txtFilformat1.Location = new System.Drawing.Point(111, 439);
            this.txtFilformat1.Name = "txtFilformat1";
            this.txtFilformat1.Size = new System.Drawing.Size(133, 20);
            this.txtFilformat1.TabIndex = 47;
            // 
            // txtMode
            // 
            this.txtMode.Location = new System.Drawing.Point(284, 307);
            this.txtMode.Name = "txtMode";
            this.txtMode.ReadOnly = true;
            this.txtMode.Size = new System.Drawing.Size(120, 20);
            this.txtMode.TabIndex = 29;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label9.Location = new System.Drawing.Point(12, 311);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 13);
            this.label9.TabIndex = 27;
            this.label9.Text = "Mode";
            // 
            // cboMode
            // 
            this.cboMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMode.Enabled = false;
            this.cboMode.Location = new System.Drawing.Point(111, 306);
            this.cboMode.Name = "cboMode";
            this.cboMode.Size = new System.Drawing.Size(133, 21);
            this.cboMode.TabIndex = 28;
            // 
            // txtColorMatrix
            // 
            this.txtColorMatrix.Location = new System.Drawing.Point(284, 412);
            this.txtColorMatrix.Name = "txtColorMatrix";
            this.txtColorMatrix.ReadOnly = true;
            this.txtColorMatrix.Size = new System.Drawing.Size(120, 20);
            this.txtColorMatrix.TabIndex = 45;
            // 
            // txtParameter
            // 
            this.txtParameter.Location = new System.Drawing.Point(284, 334);
            this.txtParameter.Name = "txtParameter";
            this.txtParameter.ReadOnly = true;
            this.txtParameter.Size = new System.Drawing.Size(120, 20);
            this.txtParameter.TabIndex = 32;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label10.Location = new System.Drawing.Point(12, 419);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 13);
            this.label10.TabIndex = 43;
            this.label10.Text = "F�rgmatris";
            // 
            // cboColorMatrix
            // 
            this.cboColorMatrix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboColorMatrix.Enabled = false;
            this.cboColorMatrix.Location = new System.Drawing.Point(111, 412);
            this.cboColorMatrix.Name = "cboColorMatrix";
            this.cboColorMatrix.Size = new System.Drawing.Size(133, 21);
            this.cboColorMatrix.TabIndex = 44;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label11.Location = new System.Drawing.Point(12, 338);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 13);
            this.label11.TabIndex = 30;
            this.label11.Text = "Parameter";
            // 
            // cboParameter
            // 
            this.cboParameter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboParameter.Enabled = false;
            this.cboParameter.Location = new System.Drawing.Point(111, 333);
            this.cboParameter.Name = "cboParameter";
            this.cboParameter.Size = new System.Drawing.Size(133, 21);
            this.cboParameter.TabIndex = 31;
            // 
            // txtKelvin2
            // 
            this.txtKelvin2.Location = new System.Drawing.Point(284, 280);
            this.txtKelvin2.Name = "txtKelvin2";
            this.txtKelvin2.ReadOnly = true;
            this.txtKelvin2.Size = new System.Drawing.Size(120, 20);
            this.txtKelvin2.TabIndex = 26;
            // 
            // txtKelvin1
            // 
            this.txtKelvin1.Enabled = false;
            this.txtKelvin1.Location = new System.Drawing.Point(111, 280);
            this.txtKelvin1.Name = "txtKelvin1";
            this.txtKelvin1.Size = new System.Drawing.Size(133, 20);
            this.txtKelvin1.TabIndex = 25;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label12.Location = new System.Drawing.Point(12, 284);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(63, 13);
            this.label12.TabIndex = 24;
            this.label12.Text = "Kelvinv�rde";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label13.Location = new System.Drawing.Point(12, 365);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(91, 13);
            this.label13.TabIndex = 33;
            this.label13.Text = "Sk�rpa / Kontrast";
            // 
            // txtSk�rpa1
            // 
            this.txtSk�rpa1.Enabled = false;
            this.txtSk�rpa1.Location = new System.Drawing.Point(111, 360);
            this.txtSk�rpa1.Name = "txtSk�rpa1";
            this.txtSk�rpa1.Size = new System.Drawing.Size(65, 20);
            this.txtSk�rpa1.TabIndex = 34;
            // 
            // txtSk�rpa2
            // 
            this.txtSk�rpa2.Location = new System.Drawing.Point(284, 358);
            this.txtSk�rpa2.Name = "txtSk�rpa2";
            this.txtSk�rpa2.ReadOnly = true;
            this.txtSk�rpa2.Size = new System.Drawing.Size(58, 20);
            this.txtSk�rpa2.TabIndex = 36;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label14.Location = new System.Drawing.Point(12, 392);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(93, 13);
            this.label14.TabIndex = 38;
            this.label14.Text = "M�ttnad / F�rgton";
            // 
            // txtKontrast1
            // 
            this.txtKontrast1.Enabled = false;
            this.txtKontrast1.Location = new System.Drawing.Point(179, 360);
            this.txtKontrast1.Name = "txtKontrast1";
            this.txtKontrast1.Size = new System.Drawing.Size(65, 20);
            this.txtKontrast1.TabIndex = 35;
            // 
            // txtKontrast2
            // 
            this.txtKontrast2.Location = new System.Drawing.Point(348, 358);
            this.txtKontrast2.Name = "txtKontrast2";
            this.txtKontrast2.ReadOnly = true;
            this.txtKontrast2.Size = new System.Drawing.Size(58, 20);
            this.txtKontrast2.TabIndex = 37;
            // 
            // txtF�rgton1
            // 
            this.txtF�rgton1.Enabled = false;
            this.txtF�rgton1.Location = new System.Drawing.Point(179, 386);
            this.txtF�rgton1.Name = "txtF�rgton1";
            this.txtF�rgton1.Size = new System.Drawing.Size(65, 20);
            this.txtF�rgton1.TabIndex = 40;
            // 
            // txtF�rgton2
            // 
            this.txtF�rgton2.Location = new System.Drawing.Point(348, 384);
            this.txtF�rgton2.Name = "txtF�rgton2";
            this.txtF�rgton2.ReadOnly = true;
            this.txtF�rgton2.Size = new System.Drawing.Size(58, 20);
            this.txtF�rgton2.TabIndex = 42;
            // 
            // txtM�ttnad1
            // 
            this.txtM�ttnad1.Enabled = false;
            this.txtM�ttnad1.Location = new System.Drawing.Point(111, 386);
            this.txtM�ttnad1.Name = "txtM�ttnad1";
            this.txtM�ttnad1.Size = new System.Drawing.Size(65, 20);
            this.txtM�ttnad1.TabIndex = 39;
            // 
            // txtM�ttnad2
            // 
            this.txtM�ttnad2.Location = new System.Drawing.Point(284, 384);
            this.txtM�ttnad2.Name = "txtM�ttnad2";
            this.txtM�ttnad2.ReadOnly = true;
            this.txtM�ttnad2.Size = new System.Drawing.Size(58, 20);
            this.txtM�ttnad2.TabIndex = 41;
            // 
            // optPortraitOut
            // 
            this.optPortraitOut.AutoSize = true;
            this.optPortraitOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.optPortraitOut.Location = new System.Drawing.Point(148, 56);
            this.optPortraitOut.Name = "optPortraitOut";
            this.optPortraitOut.Size = new System.Drawing.Size(79, 17);
            this.optPortraitOut.TabIndex = 5;
            this.optPortraitOut.Text = "Portr�tt Ute";
            this.optPortraitOut.UseVisualStyleBackColor = false;
            // 
            // FCameraSettings
            // 
            this.AcceptButton = this.cmdApply;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cmdClose;
            this.ClientSize = new System.Drawing.Size(416, 480);
            this.Controls.Add(this.optPortraitOut);
            this.Controls.Add(this.txtF�rgton1);
            this.Controls.Add(this.txtF�rgton2);
            this.Controls.Add(this.txtM�ttnad1);
            this.Controls.Add(this.txtM�ttnad2);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtKontrast1);
            this.Controls.Add(this.txtKontrast2);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtSk�rpa1);
            this.Controls.Add(this.txtSk�rpa2);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtKelvin1);
            this.Controls.Add(this.txtKelvin2);
            this.Controls.Add(this.txtColorMatrix);
            this.Controls.Add(this.txtParameter);
            this.Controls.Add(this.txtFilformat2);
            this.Controls.Add(this.txtMode);
            this.Controls.Add(this.txtVitbalans);
            this.Controls.Add(this.txtISO);
            this.Controls.Add(this.txtBl�ndarsteg);
            this.Controls.Add(this.txtSlutartid);
            this.Controls.Add(this.optNoChange);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cboColorMatrix);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cboParameter);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtFilformat1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cboMode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cboVitbalans);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cboISO);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cboBl�ndarsteg);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.cmdApply);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboSlutartid);
            this.Controls.Add(this.optFree);
            this.Controls.Add(this.optGroupOut);
            this.Controls.Add(this.optVimmel);
            this.Controls.Add(this.optPortraitIn);
            this.Controls.Add(this.optGroupIn);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FCameraSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Kamerainst�llning";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		protected override void OnLoad(EventArgs e)
		{
		    base.OnLoad(e);

		    eosPresets.GetPreset(eosPresets.PresetType.Unknown, _cameraType);
		    if (Global.Fotografdator)
		    {
		        optFree.Enabled = false;
		        optGroupIn.Enabled = false;
		        optPortraitIn.Enabled = false;
		    }
		    var tr = _camera.Translation;
		    fillCombo(cboBl�ndarsteg, tr.Aperture.AllMaps());
		    fillCombo(cboISO, tr.ISO.AllMaps());
		    fillCombo(cboMode, tr.ShootingMode.AllMaps());
		    fillCombo(cboVitbalans, tr.WhiteBalance.AllMaps());
		    fillCombo(cboSlutartid, tr.ShutterSpeed.AllMaps());
		    fillCombo(cboParameter, tr.Parameters.AllMaps());
		    fillCombo(cboColorMatrix, tr.ColorMatrix.AllMaps());

		    viewCurrentSettings();
		}

	    private void viewCurrentSettings()
		{
			if ( !_camera.IsConnected )
			{
				txtBl�ndarsteg.Text =
					txtColorMatrix.Text =
					txtISO.Text =
					txtFilformat2.Text =
					txtMode.Text =
					txtParameter.Text =
					txtSlutartid.Text =
					txtVitbalans.Text =
						"Not connected";
				return;
			}

	        int nP, nSharpness, nContrast, nSaturation, nColorTone, whiteBalance, kelvin;
			_camera.GetParameterSharpnessContrast( out nP, out nSharpness, out nContrast, out nSaturation, out nColorTone );
	        _camera.GetWhiteBalance(out whiteBalance, out kelvin);

	        var tr = _camera.Translation;

            var tim = tr.Aperture.Map(_camera.GetAvValue());
			if ( tim!=null )
				txtBl�ndarsteg.Text = tim.Text;

            tim = tr.ISO.Map(_camera.GetISONumber());
			if ( tim!=null )
				txtISO.Text = tim.Text;

            tim = tr.ShootingMode.Map(_camera.GetShootingMode());
			if ( tim!=null )
				txtMode.Text = tim.Text;

            tim = tr.ShutterSpeed.Map(_camera.GetTvValue());
			if ( tim!=null )
				txtSlutartid.Text = tim.Text;

            tim = tr.WhiteBalance.Map(whiteBalance);
			if ( tim!=null )
				txtVitbalans.Text = tim.Text;

            tim = tr.Parameters.Map(nP);
			if ( tim!=null )
				txtParameter.Text = tim.Text;
			txtSk�rpa2.Text = nSharpness.ToString();
			txtKontrast2.Text = nContrast.ToString();
			txtM�ttnad2.Text = nSaturation.ToString();
			txtF�rgton2.Text = nColorTone.ToString();

            tim = tr.ColorMatrix.Map(_camera.GetColorMatrix());
			if ( tim!=null )
				txtColorMatrix.Text = tim.Text;

            if (kelvin > 0)
                txtKelvin2.Text = kelvin.ToString();

            txtFilformat2.Text = _camera.GetImageFormatAttribute();
		}

		private void fillCombo( ComboBox cbo, ICollection col )
		{
			foreach ( var obj in col )
				cbo.Items.Add( obj );
		}

		private void loadPresets( eosPresets.PresetType pt )
		{
			var p = eosPresets.GetPreset( pt, _cameraType );
		    var tr = _camera.Translation;

            var tim = tr.ShutterSpeed.Map(p.TV);
			if ( tim!=null )
				cboSlutartid.SelectedItem = tim;
            tim = tr.Aperture.Map(p.AV);
			if ( tim!=null )
				cboBl�ndarsteg.SelectedItem = tim;
            tim = tr.ISO.Map(p.ISO);
			if ( tim!=null )
				cboISO.SelectedItem = tim;
            tim = tr.ShootingMode.Map(p.Mode);
			if ( tim!=null )
				cboMode.SelectedItem = tim;
            tim = tr.WhiteBalance.Map(p.WB);
			txtKelvin1.Text = p.KelvinText;
			if ( tim!=null )
				cboVitbalans.SelectedItem = tim;
            tim = tr.Parameters.Map(p.ParameterSet);
			if ( tim!=null )
				cboParameter.SelectedItem = tim;
			txtSk�rpa1.Text = p.SharpnessText;
            txtKontrast1.Text = p.ContrastText;
            txtM�ttnad1.Text = p.SaturationText;
            txtF�rgton1.Text = p.ColorToneText;
            tim = tr.ColorMatrix.Map(p.ColorMatrix);
			if ( tim!=null )
				cboColorMatrix.SelectedItem = tim;

			txtFilformat1.Text = p.ImageTypeSize;
		}

		private void opt_CheckedChanged(object sender, EventArgs e)
		{
			bool fCustom = sender==optFree;
			cboBl�ndarsteg.Enabled = fCustom;
			cboISO.Enabled = fCustom;
			txtFilformat1.Enabled = fCustom;
			cboMode.Enabled = fCustom;
			cboSlutartid.Enabled = fCustom;
			cboVitbalans.Enabled = fCustom;
			txtKelvin1.Enabled = fCustom;
			cboParameter.Enabled = fCustom;
			txtSk�rpa1.Enabled = fCustom;
			txtKontrast1.Enabled = fCustom;
			txtM�ttnad1.Enabled = fCustom;
			txtF�rgton1.Enabled = fCustom;
			cboColorMatrix.Enabled = fCustom;

			if ( sender==optGroupIn )
				loadPresets( eosPresets.PresetType.IndoorGroup );
			else if ( sender==optGroupOut )
				loadPresets( eosPresets.PresetType.OutdoorGroup );
			else if ( sender==optPortraitIn )
				loadPresets( eosPresets.PresetType.IndoorPortrait );
			else if ( sender == optPortraitOut )
				loadPresets( eosPresets.PresetType.OutdoorPortrait );
			else if ( sender == optVimmel )
				loadPresets( eosPresets.PresetType.Environment );
		}

		private void cmdApply_Click(object sender, System.EventArgs e)
		{
			TextIntMap tim;

			tim = cboMode.SelectedItem as TextIntMap;
			if ( tim!=null )
				_camera.SetShootingMode( tim.Number );

			tim = cboSlutartid.SelectedItem as TextIntMap;
			if ( tim!=null )
                _camera.SetTvValue(tim.Number);

			tim = cboBl�ndarsteg.SelectedItem as TextIntMap;
			if ( tim!=null )
                _camera.SetAvValue(tim.Number);

			tim = cboISO.SelectedItem as TextIntMap;
			if ( tim!=null )
                _camera.SetISONumber(tim.Number);

			tim = cboVitbalans.SelectedItem as TextIntMap;
            if (tim != null)
                _camera.SetWhiteBalance(tim.Number, vdUsr.Util.safeParse(txtKelvin1.Text));

			int nS1, nC1, nS2, nC2;
			int.TryParse(txtSk�rpa1.Text, out nS1);
			int.TryParse(txtKontrast1.Text, out nC1);
			int.TryParse(txtM�ttnad1.Text, out nS2);
			int.TryParse(txtF�rgton1.Text, out nC2);
			tim = cboParameter.SelectedItem as TextIntMap;
			if ( tim!=null )
                _camera.SetParameterSharpnessContrast(tim.Number, nS1, nC1, nS2, nC2);

			tim = cboColorMatrix.SelectedItem as TextIntMap;
			if ( tim!=null && tim.Number != 0 )
                _camera.SetColorMatrix(tim.Number);

            if ( txtFilformat1.Text.Length != 0 )
    			_camera.SetImageFormatAttribute( txtFilformat1.Text );

			tim = cboMode.SelectedItem as TextIntMap;
			if (tim != null)
				_camera.SetShootingMode(tim.Number);

			viewCurrentSettings();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			viewCurrentSettings();
		}

	}

}
