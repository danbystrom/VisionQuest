using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Photomic.Common;

namespace Plata
{
	public class frmPersonnamn : vdUsr.baseGradientForm
	{
		private Button cmdCancel;
		private Button cmdOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private TextBox txtFörnamn;
		private Label label1;
		private Label label2;
		private TextBox txtEfternamn;
		private System.Windows.Forms.ComboBox cboTitel;
		private System.Windows.Forms.CheckBox chkPersonal;
		private System.Windows.Forms.Button cmdDelete;

		private System.Windows.Forms.Label lblTitel;
		private System.Windows.Forms.PictureBox picChar;
		private System.Windows.Forms.ListView lv;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.ColumnHeader columnHeader7;
		private System.Windows.Forms.CheckBox chkAutoCorrect;

		private TextBox _txtInput;
		private RadioButton optKvinna;
		private TextBox txtPersnr2;
		private TextBox txtPersnr1;
		private Label label3;
		private RadioButton optMan;
		private TextBox txtAdress;
		private Label label5;
		private TextBox txtZip;
		private Label label6;
		private TextBox txtCity;
		private TextBox txtPhone;
		private Label label7;
		private TextBox txtEMail;
		private Label label8;
		private vdUsr.vdGroup grpProtected;
		private Label label10;
		private Label label9;
		private Label lblKatalogOchDVD;
		private ComboBox cboProtArchive;
		private ComboBox cboProtGroup;
		private ComboBox cboProtCatalog;
		private Label label4;
        private TextBox txtCustId;
		private CheckBox chkStudentCardIsPrinted;
		private CheckBox chkWantNewPPaper;
		private ComboBox cboCountry;
		private Label label11;

		private bool _fSkipAutoCorrect;

		private frmPersonnamn()
		{
			InitializeComponent();
		}

		private frmPersonnamn( PlataDM.Grupper grupper, string strSelect )
		{
			InitializeComponent();

			if ( grupper==null )
			{
				foreach ( string s in Global.aTitlarBas )
					cboTitel.Items.Add( s );
				foreach ( string s in Global.Preferences.listTitlarEgna )
					cboTitel.Items.Add( s );
				cboTitel.Text = strSelect;
			}
			else
			{
				PlataDM.Grupp grpSelect = null;
				cboTitel.DropDownStyle = ComboBoxStyle.DropDownList;
				foreach ( PlataDM.Grupp grp in grupper )
					if ( grp.GruppTyp==GruppTyp.GruppNormal )
					{
						cboTitel.Items.Add( grp );
						if ( string.Compare(strSelect,grp.Namn)==0 )
							grpSelect = grp;
					}
				if ( grpSelect!=null )
					cboTitel.SelectedItem = grpSelect;
				lblTitel.Text = "Grupp";
				chkPersonal.Visible = false;
			}

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( frmPersonnamn ) );
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.txtFörnamn = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtEfternamn = new System.Windows.Forms.TextBox();
			this.cboTitel = new System.Windows.Forms.ComboBox();
			this.lblTitel = new System.Windows.Forms.Label();
			this.chkPersonal = new System.Windows.Forms.CheckBox();
			this.picChar = new System.Windows.Forms.PictureBox();
			this.cmdDelete = new System.Windows.Forms.Button();
			this.lv = new System.Windows.Forms.ListView();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
			this.chkAutoCorrect = new System.Windows.Forms.CheckBox();
			this.optMan = new System.Windows.Forms.RadioButton();
			this.optKvinna = new System.Windows.Forms.RadioButton();
			this.txtPersnr2 = new System.Windows.Forms.TextBox();
			this.txtPersnr1 = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtAdress = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtZip = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txtCity = new System.Windows.Forms.TextBox();
			this.txtPhone = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.txtEMail = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.grpProtected = new vdUsr.vdGroup();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.lblKatalogOchDVD = new System.Windows.Forms.Label();
			this.cboProtArchive = new System.Windows.Forms.ComboBox();
			this.cboProtGroup = new System.Windows.Forms.ComboBox();
			this.cboProtCatalog = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtCustId = new TextBox();
			this.chkStudentCardIsPrinted = new System.Windows.Forms.CheckBox();
			this.chkWantNewPPaper = new System.Windows.Forms.CheckBox();
			this.cboCountry = new System.Windows.Forms.ComboBox();
			this.label11 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.picChar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.grpProtected)).BeginInit();
			this.grpProtected.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point( 653, 351 );
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size( 80, 28 );
			this.cmdCancel.TabIndex = 30;
			this.cmdCancel.Text = "Avbryt";
			// 
			// cmdOK
			// 
			this.cmdOK.Location = new System.Drawing.Point( 567, 351 );
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size( 80, 28 );
			this.cmdOK.TabIndex = 29;
			this.cmdOK.Text = "OK";
			this.cmdOK.Click += new System.EventHandler( this.cmdOK_Click );
			// 
			// txtFörnamn
			// 
			this.txtFörnamn.Location = new System.Drawing.Point( 72, 32 );
			this.txtFörnamn.MaxLength = 30;
			this.txtFörnamn.Name = "txtFörnamn";
			this.txtFörnamn.Size = new System.Drawing.Size( 188, 20 );
			this.txtFörnamn.TabIndex = 1;
			this.txtFörnamn.TextChanged += new System.EventHandler( this.autocorrect_TextChanged );
			this.txtFörnamn.Enter += new System.EventHandler( this.txtFörnamn_Enter );
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label1.Location = new System.Drawing.Point( 12, 34 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 48, 13 );
			this.label1.TabIndex = 0;
			this.label1.Text = "&Förnamn";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label2.Location = new System.Drawing.Point( 12, 60 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 55, 13 );
			this.label2.TabIndex = 2;
			this.label2.Text = "&Efternamn";
			// 
			// txtEfternamn
			// 
			this.txtEfternamn.Location = new System.Drawing.Point( 72, 58 );
			this.txtEfternamn.MaxLength = 40;
			this.txtEfternamn.Name = "txtEfternamn";
			this.txtEfternamn.Size = new System.Drawing.Size( 188, 20 );
			this.txtEfternamn.TabIndex = 3;
			this.txtEfternamn.TextChanged += new System.EventHandler( this.autocorrect_TextChanged );
			this.txtEfternamn.Enter += new System.EventHandler( this.txtEfternamn_Enter );
			// 
			// cboTitel
			// 
			this.cboTitel.Location = new System.Drawing.Point( 72, 84 );
			this.cboTitel.Name = "cboTitel";
			this.cboTitel.Size = new System.Drawing.Size( 156, 21 );
			this.cboTitel.Sorted = true;
			this.cboTitel.TabIndex = 5;
			this.cboTitel.TextChanged += new System.EventHandler( this.cboTitel_TextChanged );
			// 
			// lblTitel
			// 
			this.lblTitel.AutoSize = true;
			this.lblTitel.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.lblTitel.Location = new System.Drawing.Point( 12, 86 );
			this.lblTitel.Name = "lblTitel";
			this.lblTitel.Size = new System.Drawing.Size( 27, 13 );
			this.lblTitel.TabIndex = 4;
			this.lblTitel.Text = "&Titel";
			// 
			// chkPersonal
			// 
			this.chkPersonal.AutoSize = true;
			this.chkPersonal.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.chkPersonal.Location = new System.Drawing.Point( 72, 112 );
			this.chkPersonal.Name = "chkPersonal";
			this.chkPersonal.Size = new System.Drawing.Size( 67, 17 );
			this.chkPersonal.TabIndex = 7;
			this.chkPersonal.Text = "&Personal";
			this.chkPersonal.UseVisualStyleBackColor = false;
			// 
			// picChar
			// 
			this.picChar.BackColor = System.Drawing.SystemColors.Window;
			this.picChar.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.picChar.Location = new System.Drawing.Point( 539, 8 );
			this.picChar.Name = "picChar";
			this.picChar.Size = new System.Drawing.Size( 193, 337 );
			this.picChar.TabIndex = 11;
			this.picChar.TabStop = false;
			this.picChar.MouseDown += new System.Windows.Forms.MouseEventHandler( this.picChar_MouseDown );
			this.picChar.Paint += new System.Windows.Forms.PaintEventHandler( this.picChar_Paint );
			// 
			// cmdDelete
			// 
			this.cmdDelete.Image = ((System.Drawing.Image)(resources.GetObject( "cmdDelete.Image" )));
			this.cmdDelete.Location = new System.Drawing.Point( 236, 84 );
			this.cmdDelete.Name = "cmdDelete";
			this.cmdDelete.Size = new System.Drawing.Size( 24, 21 );
			this.cmdDelete.TabIndex = 6;
			this.cmdDelete.TabStop = false;
			this.cmdDelete.Click += new System.EventHandler( this.cmdDelete_Click );
			// 
			// lv
			// 
			this.lv.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader2,
            this.columnHeader1,
            this.columnHeader5,
            this.columnHeader4,
            this.columnHeader6,
            this.columnHeader7} );
			this.lv.FullRowSelect = true;
			this.lv.Location = new System.Drawing.Point( 4, 271 );
			this.lv.Name = "lv";
			this.lv.Size = new System.Drawing.Size( 529, 108 );
			this.lv.TabIndex = 28;
			this.lv.UseCompatibleStateImageBehavior = false;
			this.lv.View = System.Windows.Forms.View.Details;
			this.lv.DoubleClick += new System.EventHandler( this.lv_DoubleClick );
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Klass";
			this.columnHeader3.Width = 70;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Efternamn";
			this.columnHeader2.Width = 100;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Förnamn";
			this.columnHeader1.Width = 90;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Titel";
			this.columnHeader5.Width = 80;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Bildnr";
			this.columnHeader4.Width = 80;
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "G";
			this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader6.Width = 24;
			// 
			// columnHeader7
			// 
			this.columnHeader7.Text = "P";
			this.columnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader7.Width = 24;
			// 
			// chkAutoCorrect
			// 
			this.chkAutoCorrect.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.chkAutoCorrect.Location = new System.Drawing.Point( 377, 174 );
			this.chkAutoCorrect.Name = "chkAutoCorrect";
			this.chkAutoCorrect.Size = new System.Drawing.Size( 157, 31 );
			this.chkAutoCorrect.TabIndex = 26;
			this.chkAutoCorrect.Text = "&Autokorrigera VERSALER/ gemener";
			this.chkAutoCorrect.UseVisualStyleBackColor = false;
			this.chkAutoCorrect.CheckedChanged += new System.EventHandler( this.chkAutoCorrect_CheckedChanged );
			// 
			// optMan
			// 
			this.optMan.AutoSize = true;
			this.optMan.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.optMan.Location = new System.Drawing.Point( 500, 140 );
			this.optMan.Name = "optMan";
			this.optMan.Size = new System.Drawing.Size( 34, 17 );
			this.optMan.TabIndex = 25;
			this.optMan.TabStop = true;
			this.optMan.Text = "M";
			this.optMan.UseVisualStyleBackColor = false;
			// 
			// optKvinna
			// 
			this.optKvinna.AutoSize = true;
			this.optKvinna.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.optKvinna.Location = new System.Drawing.Point( 461, 139 );
			this.optKvinna.Name = "optKvinna";
			this.optKvinna.Size = new System.Drawing.Size( 33, 17 );
			this.optKvinna.TabIndex = 24;
			this.optKvinna.TabStop = true;
			this.optKvinna.Text = "Q";
			this.optKvinna.UseVisualStyleBackColor = false;
			// 
			// txtPersnr2
			// 
			this.txtPersnr2.Location = new System.Drawing.Point( 409, 139 );
			this.txtPersnr2.MaxLength = 4;
			this.txtPersnr2.Name = "txtPersnr2";
			this.txtPersnr2.Size = new System.Drawing.Size( 37, 20 );
			this.txtPersnr2.TabIndex = 23;
			this.txtPersnr2.TextChanged += new System.EventHandler( this.txtPnr2_TextChanged );
			// 
			// txtPersnr1
			// 
			this.txtPersnr1.Location = new System.Drawing.Point( 345, 139 );
			this.txtPersnr1.MaxLength = 8;
			this.txtPersnr1.Name = "txtPersnr1";
			this.txtPersnr1.Size = new System.Drawing.Size( 58, 20 );
			this.txtPersnr1.TabIndex = 22;
			this.txtPersnr1.TextChanged += new System.EventHandler( this.txtPnr1_TextChanged );
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label3.Location = new System.Drawing.Point( 285, 142 );
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size( 49, 13 );
			this.label3.TabIndex = 21;
			this.label3.Text = "Person&nr";
			// 
			// txtAdress
			// 
			this.txtAdress.Location = new System.Drawing.Point( 345, 8 );
			this.txtAdress.Name = "txtAdress";
			this.txtAdress.Size = new System.Drawing.Size( 188, 20 );
			this.txtAdress.TabIndex = 11;
			this.txtAdress.TextChanged += new System.EventHandler( this.autocorrect_TextChanged );
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label5.Location = new System.Drawing.Point( 285, 11 );
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size( 39, 13 );
			this.label5.TabIndex = 10;
			this.label5.Text = "Adre&ss";
			// 
			// txtZip
			// 
			this.txtZip.Location = new System.Drawing.Point( 345, 34 );
			this.txtZip.Name = "txtZip";
			this.txtZip.Size = new System.Drawing.Size( 49, 20 );
			this.txtZip.TabIndex = 13;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label6.Location = new System.Drawing.Point( 285, 37 );
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size( 48, 13 );
			this.label6.TabIndex = 12;
			this.label6.Text = "Pnr / &Ort";
			// 
			// txtCity
			// 
			this.txtCity.Location = new System.Drawing.Point( 402, 34 );
			this.txtCity.Name = "txtCity";
			this.txtCity.Size = new System.Drawing.Size( 131, 20 );
			this.txtCity.TabIndex = 14;
			this.txtCity.TextChanged += new System.EventHandler( this.autocorrect_TextChanged );
			// 
			// txtPhone
			// 
			this.txtPhone.Location = new System.Drawing.Point( 345, 87 );
			this.txtPhone.Name = "txtPhone";
			this.txtPhone.Size = new System.Drawing.Size( 188, 20 );
			this.txtPhone.TabIndex = 18;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label7.Location = new System.Drawing.Point( 285, 90 );
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size( 43, 13 );
			this.label7.TabIndex = 17;
			this.label7.Text = "Tele&fon";
			// 
			// txtEMail
			// 
			this.txtEMail.Location = new System.Drawing.Point( 345, 113 );
			this.txtEMail.Name = "txtEMail";
			this.txtEMail.Size = new System.Drawing.Size( 188, 20 );
			this.txtEMail.TabIndex = 20;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label8.Location = new System.Drawing.Point( 285, 116 );
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size( 26, 13 );
			this.label8.TabIndex = 19;
			this.label8.Text = "&Mejl";
			// 
			// grpProtected
			// 
			this.grpProtected.BackColor1 = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.grpProtected.BackColor2 = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.grpProtected.Caption = null;
			this.grpProtected.Controls.Add( this.label10 );
			this.grpProtected.Controls.Add( this.label9 );
			this.grpProtected.Controls.Add( this.lblKatalogOchDVD );
			this.grpProtected.Controls.Add( this.cboProtArchive );
			this.grpProtected.Controls.Add( this.cboProtGroup );
			this.grpProtected.Controls.Add( this.cboProtCatalog );
			this.grpProtected.Location = new System.Drawing.Point( 5, 207 );
			this.grpProtected.Name = "grpProtected";
			this.grpProtected.Options = "Nej|Ja";
			this.grpProtected.Size = new System.Drawing.Size( 529, 58 );
			this.grpProtected.TabIndex = 27;
			this.grpProtected.TabStop = false;
			this.grpProtected.Text = "Skyddad identitet";
			this.grpProtected.Paint += new System.Windows.Forms.PaintEventHandler( this.grpProtected_Paint );
			this.grpProtected.CheckChanged += new System.EventHandler( this.grpProtected_CheckChanged );
			// 
			// label10
			// 
			this.label10.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label10.Location = new System.Drawing.Point( 349, 16 );
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size( 100, 12 );
			this.label10.TabIndex = 4;
			this.label10.Text = "PhotoArkiv";
			this.label10.Visible = false;
			// 
			// label9
			// 
			this.label9.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label9.Location = new System.Drawing.Point( 177, 16 );
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size( 100, 12 );
			this.label9.TabIndex = 2;
			this.label9.Text = "Gruppbild";
			this.label9.Visible = false;
			// 
			// lblKatalogOchDVD
			// 
			this.lblKatalogOchDVD.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.lblKatalogOchDVD.Location = new System.Drawing.Point( 5, 16 );
			this.lblKatalogOchDVD.Name = "lblKatalogOchDVD";
			this.lblKatalogOchDVD.Size = new System.Drawing.Size( 100, 12 );
			this.lblKatalogOchDVD.TabIndex = 0;
			this.lblKatalogOchDVD.Text = "Katalog och DVD";
			this.lblKatalogOchDVD.Visible = false;
			// 
			// cboProtArchive
			// 
			this.cboProtArchive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboProtArchive.FormattingEnabled = true;
			this.cboProtArchive.Location = new System.Drawing.Point( 352, 32 );
			this.cboProtArchive.Name = "cboProtArchive";
			this.cboProtArchive.Size = new System.Drawing.Size( 166, 21 );
			this.cboProtArchive.TabIndex = 5;
			this.cboProtArchive.Visible = false;
			// 
			// cboProtGroup
			// 
			this.cboProtGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboProtGroup.FormattingEnabled = true;
			this.cboProtGroup.Location = new System.Drawing.Point( 180, 32 );
			this.cboProtGroup.Name = "cboProtGroup";
			this.cboProtGroup.Size = new System.Drawing.Size( 166, 21 );
			this.cboProtGroup.TabIndex = 3;
			this.cboProtGroup.Visible = false;
			// 
			// cboProtCatalog
			// 
			this.cboProtCatalog.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboProtCatalog.FormattingEnabled = true;
			this.cboProtCatalog.Location = new System.Drawing.Point( 8, 32 );
			this.cboProtCatalog.Name = "cboProtCatalog";
			this.cboProtCatalog.Size = new System.Drawing.Size( 166, 21 );
			this.cboProtCatalog.TabIndex = 1;
			this.cboProtCatalog.Visible = false;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label4.Location = new System.Drawing.Point( 12, 10 );
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size( 41, 13 );
			this.label4.TabIndex = 31;
			this.label4.Text = "&Kundnr";
			// 
			// cboKundnr
			// 
			this.txtCustId.Location = new System.Drawing.Point( 72, 8 );
			this.txtCustId.Name = "txtCustId";
			this.txtCustId.ReadOnly = true;
			this.txtCustId.Size = new System.Drawing.Size( 156, 21 );
			this.txtCustId.TabIndex = 32;
			// 
			// chkStudentCardIsPrinted
			// 
			this.chkStudentCardIsPrinted.AutoSize = true;
			this.chkStudentCardIsPrinted.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.chkStudentCardIsPrinted.Location = new System.Drawing.Point( 72, 134 );
			this.chkStudentCardIsPrinted.Name = "chkStudentCardIsPrinted";
			this.chkStudentCardIsPrinted.Size = new System.Drawing.Size( 123, 17 );
			this.chkStudentCardIsPrinted.TabIndex = 8;
			this.chkStudentCardIsPrinted.Text = "Har fått &StudentCard";
			this.chkStudentCardIsPrinted.UseVisualStyleBackColor = false;
			// 
			// chkWantNewPPaper
			// 
			this.chkWantNewPPaper.AutoSize = true;
			this.chkWantNewPPaper.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.chkWantNewPPaper.Location = new System.Drawing.Point( 72, 156 );
			this.chkWantNewPPaper.Name = "chkWantNewPPaper";
			this.chkWantNewPPaper.Size = new System.Drawing.Size( 165, 17 );
			this.chkWantNewPPaper.TabIndex = 9;
			this.chkWantNewPPaper.Text = "&Vill ha ny P-Lapp hemskickad";
			this.chkWantNewPPaper.UseVisualStyleBackColor = false;
			// 
			// cboCountry
			// 
			this.cboCountry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboCountry.FormattingEnabled = true;
			this.cboCountry.Items.AddRange( new object[] {
            "Sverige",
            "Finland",
            "Norge"} );
			this.cboCountry.Location = new System.Drawing.Point( 345, 60 );
			this.cboCountry.Name = "cboCountry";
			this.cboCountry.Size = new System.Drawing.Size( 188, 21 );
			this.cboCountry.TabIndex = 16;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label11.Location = new System.Drawing.Point( 285, 63 );
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size( 31, 13 );
			this.label11.TabIndex = 15;
			this.label11.Text = "Land";
			// 
			// frmPersonnamn
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size( 744, 384 );
			this.Controls.Add( this.label11 );
			this.Controls.Add( this.cboCountry );
			this.Controls.Add( this.optMan );
			this.Controls.Add( this.chkWantNewPPaper );
			this.Controls.Add( this.optKvinna );
			this.Controls.Add( this.chkStudentCardIsPrinted );
			this.Controls.Add( this.txtPersnr2 );
			this.Controls.Add( this.label4 );
			this.Controls.Add( this.txtPersnr1 );
			this.Controls.Add( this.txtCustId );
			this.Controls.Add( this.label3 );
			this.Controls.Add( this.grpProtected );
			this.Controls.Add( this.txtEMail );
			this.Controls.Add( this.label8 );
			this.Controls.Add( this.txtPhone );
			this.Controls.Add( this.label7 );
			this.Controls.Add( this.txtCity );
			this.Controls.Add( this.txtZip );
			this.Controls.Add( this.label6 );
			this.Controls.Add( this.txtAdress );
			this.Controls.Add( this.label5 );
			this.Controls.Add( this.chkAutoCorrect );
			this.Controls.Add( this.lv );
			this.Controls.Add( this.cmdDelete );
			this.Controls.Add( this.picChar );
			this.Controls.Add( this.chkPersonal );
			this.Controls.Add( this.lblTitel );
			this.Controls.Add( this.cboTitel );
			this.Controls.Add( this.txtEfternamn );
			this.Controls.Add( this.txtFörnamn );
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.cmdCancel );
			this.Controls.Add( this.cmdOK );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmPersonnamn";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Personnamn";
			((System.ComponentModel.ISupportInitialize)(this.picChar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.grpProtected)).EndInit();
			this.grpProtected.ResumeLayout( false );
			this.ResumeLayout( false );
			this.PerformLayout();

		}
		#endregion

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed (e);
		}

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			string strFörnamn = txtFörnamn.Text.Trim();
			string strEfternamn = txtEfternamn.Text.Trim();
			string strTitel = cboTitel.Text.Trim();

			if ( !chkPersonal.Visible ) //HACK! betyder att det är infällning och att titel nu betyder grupp!!!
				if ( string.IsNullOrEmpty( strTitel ) )
				{
					Global.showMsgBox( this, "Du måste ange vilken grupp infällningen ska till!" );
					return;
				}

			if ( lv.SelectedItems.Count==1 )
			{
				this.DialogResult = DialogResult.Yes;
				return;
			}

			if ( chkPersonal.Visible && strTitel.Length != 0 && cboTitel.DropDownStyle != ComboBoxStyle.DropDownList )
			{
				bool fIsNew = true;
				foreach ( string s in Global.aTitlarBas )
					if ( s.CompareTo(strTitel)==0 )
						fIsNew = false;
				foreach ( string s in Global.Preferences.listTitlarEgna )
					if ( s.CompareTo(strTitel)==0 )
						fIsNew = false;
				if ( fIsNew )
				{
					Global.Preferences.listTitlarEgna.Add( strTitel );
					Global.sparaInställningar();
				}
			}

			if ( strFörnamn.Length>1 || strEfternamn.Length>1 )
				this.DialogResult = DialogResult.OK;
			else
				Global.showMsgBox( this, "Ange minst ett av förnamn och efternamn!" );

		}

		private void cboTitel_TextChanged(object sender, System.EventArgs e)
		{
			chkPersonal.Checked = true;
		}

		private void picChar_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			int nW = picChar.ClientSize.Width;
			int nH = picChar.ClientSize.Height;

			for ( int x=1 ; x<8 ; x++ )
				e.Graphics.DrawLine( SystemPens.WindowText, x*nW/8, 0, x*nW/8, nH );
			for ( int y=1 ; y<16 ; y++ )
				e.Graphics.DrawLine( SystemPens.WindowText, 0, y*nH/16, nW, y*nH/16 );
			int nChar = 192;
			for ( int y=0 ; y<16 ; y++ )
				for ( int x=0 ; x<8 ; x++ )
					e.Graphics.DrawString( ((char)nChar++).ToString(), picChar.Font, SystemBrushes.WindowText,
						new Rectangle( x*nW/8, y*nH/16, nW/8, nH/16 ), Util.sfMC );
		}

		private void cmdChar_Click(object sender, System.EventArgs e)
		{
			this.ClientSize = new Size( picChar.Right+8, this.ClientSize.Height );
		}

		private void picChar_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if ( e.Clicks==2 && _txtInput!=null )
			{
				int nX = (8*e.X)/picChar.ClientSize.Width;
				int nY = (16*e.Y)/picChar.ClientSize.Height;
				_txtInput.SelectedText = ((char)(192+nY*8+nX)).ToString();
			}
		}

		private void txtFörnamn_Enter(object sender, System.EventArgs e)
		{
			_txtInput = txtFörnamn;
		}

		private void txtEfternamn_Enter(object sender, System.EventArgs e)
		{
			_txtInput = txtEfternamn;
		}

		private void cmdDelete_Click(object sender, System.EventArgs e)
		{
			if ( cboTitel.DropDownStyle != ComboBoxStyle.DropDownList && Global.Preferences.listTitlarEgna.Contains( cboTitel.Text ) )
				if ( Global.askMsgBox( this, "Vill du radera denna titel från din lista med egna titlar?", true ) == DialogResult.Yes )
				{
					Global.Preferences.listTitlarEgna.Remove( cboTitel.Text );
					Global.sparaInställningar();
					cboTitel.Items.Remove( cboTitel.Text );
					cboTitel.Text = string.Empty;
				}
		}

		private void chkAutoCorrect_CheckedChanged(object sender, System.EventArgs e)
		{
			autoCorrect( txtFörnamn );
			autoCorrect( txtEfternamn );
			autoCorrect( txtAdress );
			autoCorrect( txtCity );
		}

		private void autoCorrect( TextBox txt )
		{
			if ( !chkAutoCorrect.Checked || _fSkipAutoCorrect )
				return;

			string strTextBefore = txt.Text;
			string strTextAfter = Util.properCase( strTextBefore );
			if ( strTextBefore == strTextAfter )
				return;

			_fSkipAutoCorrect = true;
			int nSelStart = txt.SelectionStart;
			txt.Text = strTextAfter;
			txt.SelectionStart = nSelStart;
			_fSkipAutoCorrect = false;
			chkAutoCorrect.Font = new Font( chkAutoCorrect.Font, FontStyle.Bold );
		}

		private void displayHits()
		{
			if ( !lv.Visible )
				return;
			string strF = txtFörnamn.Text;
			string strE = txtEfternamn.Text;
			lv.Items.Clear();
			if ( strF.Length+strE.Length < 1 )
				return;
			foreach ( PlataDM.Grupp grupp in Global.Skola.Grupper )
				foreach ( PlataDM.Person pers in grupp.AllaPersoner )
					if ( string.Compare( strF, 0, pers.Förnamn, 0, strF.Length, true )==0 &&
						string.Compare( strE, 0, pers.Efternamn, 0, strE.Length, true )==0 )
					{
						ListViewItem lvi = new ListViewItem( grupp.Namn );
						lvi.SubItems.Add( pers.Efternamn );
						lvi.SubItems.Add( pers.Förnamn );
						lvi.SubItems.Add( pers.Titel );
						lvi.SubItems.Add( pers.IST );
						lvi.SubItems.Add( pers.Siffra!=null ? "X" : string.Empty );
						lvi.SubItems.Add( pers.HasPhoto ? "X" : string.Empty );
						lv.Items.Add( lvi );
						lvi.Tag = pers;
					}
		}

		private void pickupStuff( PlataDM.Person p )
		{
			p.setInfo( PersonInfo.Info.Address, txtAdress.Text.Trim() );
			p.setInfo( PersonInfo.Info.Zip, txtZip.Text.Trim() );
			p.setInfo( PersonInfo.Info.Town, txtCity.Text.Trim() );
			p.setInfo( PersonInfo.Info.Phone, txtPhone.Text.Trim() );
			p.setInfo( PersonInfo.Info.EMail, txtEMail.Text.Trim() );
			p.setInfo( PersonInfo.Info.Country, cboCountry.Text );

			string s = null;
			if ( txtPersnr1.Text.Length == 6 )
				txtPersnr1.Text = "19" + txtPersnr1.Text;
			if ( txtPersnr1.Text.Length == 8 )
			{
				s = txtPersnr1.Text;
				if ( txtPersnr2.Text.Length == 4 )
					s += "-" + txtPersnr2.Text;
			}
			p.setInfo( PersonInfo.Info.SocialSecurity, s );
			if ( optKvinna.Checked )
				p.Kön = "Q";
			else if ( optMan.Checked )
				p.Kön = "M";
			else
				p.Kön = null;

			p.Personal = chkPersonal.Checked;
			p.StudentCardIsPrinted = chkStudentCardIsPrinted.Checked;
			p.WantNewPPaper = chkWantNewPPaper.Checked;

			if ( grpProtected.OptionSelected == 1 )
			{
				p.ProtArchive = ProtectedIdExtra.Value( cboProtArchive );
				p.ProtGroup = ProtectedIdExtra.Value( cboProtGroup );
				p.ProtCatalog = ProtectedIdExtra.Value( cboProtCatalog );
			}
			else
			{
				p.ProtArchive = PersonSkyddad.EjSkydd;
				p.ProtGroup = PersonSkyddad.EjSkydd;
				p.ProtCatalog = PersonSkyddad.EjSkydd;
			}
		}

		public static DialogResult redigera( 
			Form parent, 
			PlataDM.Person person, 
			PlataDM.Grupper grupper )
		{
			using ( var dlg = new frmPersonnamn(grupper,person.Titel) )
			{
				dlg.lv.Visible = false;

				dlg.txtCustId.Text = person.ScanCode;
				dlg.txtFörnamn.Text = person.Förnamn;
				dlg.txtEfternamn.Text = person.Efternamn;
				dlg.chkPersonal.Checked = person.Personal;
				dlg.chkStudentCardIsPrinted.Checked = person.StudentCardIsPrinted;
				dlg.chkWantNewPPaper.Checked = person.WantNewPPaper;

				dlg.txtAdress.Text = person.getInfo( PersonInfo.Info.Address );
				dlg.txtZip.Text = (Zip)person.getInfo( PersonInfo.Info.Zip );
				dlg.txtCity.Text = person.getInfo( PersonInfo.Info.Town );
				dlg.cboCountry.Text = person.getInfo( PersonInfo.Info.Country );
				dlg.txtPhone.Text = person.getInfo( PersonInfo.Info.Phone );
				dlg.txtEMail.Text = person.getInfo( PersonInfo.Info.EMail );
				var ss = person.getInfo( PersonInfo.Info.SocialSecurity );
				if ( !string.IsNullOrEmpty( ss ) )
					switch ( ss.Length )
					{
						case 6:
							dlg.txtPersnr1.Text = "19" + ss;
							break;
						case 8:
							dlg.txtPersnr1.Text = ss;
							break;
						case 11:
							dlg.txtPersnr1.Text = "19" + ss.Substring( 0, 6 );
							dlg.txtPersnr2.Text = ss.Substring( 7 );
							break;
						case 13:
							dlg.txtPersnr1.Text = ss.Substring( 0, 8 );
							dlg.txtPersnr2.Text = ss.Substring( 9 );
							break;
					}
				if ( "Q".CompareTo( person.Kön ) == 0 )
					dlg.optKvinna.Checked = true;
				else if ( "M".CompareTo( person.Kön ) == 0 )
					dlg.optMan.Checked = true;

				ProtectedIdExtra.fillComboBox( dlg.cboProtArchive, true, person.ProtArchive );
				ProtectedIdExtra.fillComboBox( dlg.cboProtCatalog, true, person.ProtCatalog );
				ProtectedIdExtra.fillComboBox( dlg.cboProtGroup, true, person.ProtGroup );
				if ( person.HarSkyddadId )
				{
					dlg.grpProtected.OptionSelected = 1;
					dlg.grpProtected_CheckChanged( dlg.grpProtected, EventArgs.Empty );
				}

				if ( dlg.ShowDialog( parent ) == DialogResult.OK )
				{
					person.ScanCode = dlg.txtCustId.Text;
					person.Förnamn = dlg.txtFörnamn.Text.Trim();
					person.Efternamn = dlg.txtEfternamn.Text.Trim();
					person.Titel = dlg.cboTitel.Text;
					dlg.pickupStuff( person );
					return DialogResult.OK;
				}
				return DialogResult.Cancel;
			}
		}

		public static PlataDM.Person läggTill( 
			Form parent, 
			PlataDM.Grupp grupp, 
			PlataDM.Grupper grupper )
		{
			using ( var dlg = new frmPersonnamn(grupper,string.Empty) )
			{
				dlg.chkAutoCorrect.Checked = true;
				if ( grupp.GruppTyp == GruppTyp.GruppPersonal )
					dlg.chkPersonal.Checked = true;

                ProtectedIdExtra.fillComboBox(dlg.cboProtArchive, true, PersonSkyddad.EjSkydd);
                ProtectedIdExtra.fillComboBox(dlg.cboProtCatalog, true, PersonSkyddad.EjSkydd);
                ProtectedIdExtra.fillComboBox(dlg.cboProtGroup, true, PersonSkyddad.EjSkydd);

                PlataDM.Person p;
				switch ( dlg.ShowDialog(parent) )
				{
					case DialogResult.OK:
						p = grupp.PersonerNärvarande.Add(
							dlg.chkPersonal.Checked,
							dlg.txtFörnamn.Text.Trim(),
							dlg.txtEfternamn.Text.Trim(),
							dlg.cboTitel.Text );
						p.ScanCode = dlg.txtCustId.Text;
						dlg.pickupStuff( p );
						p.AddedByPhotographer = true;
						break;

					case DialogResult.Yes:
						// dubbelklick på befintligt namn
						var pOrg = dlg.lv.SelectedItems[0].Tag as PlataDM.Person;
						p = grupp.PersonerNärvarande.Add(
							pOrg.Personal | pOrg.Grupp.GruppTyp == GruppTyp.GruppPersonal,
							pOrg.getInfos() );
						if ( p.Grupp.GruppTyp==GruppTyp.GruppInfällning )
							p.Titel = pOrg.Grupp.Namn;
						if ( p.Grupp.GruppTyp == GruppTyp.GruppNormal && pOrg.Grupp.GruppTyp == GruppTyp.GruppNormal )
						{
							p.ScanCode = pOrg.ScanCode;
							bool fOrgIsNumbered =
								pOrg.Siffra != null ||
								pOrg.GruppPersonTyp == PlataDM.GruppPersonTyp.PersonFrånvarande;
							bool fMove = !fOrgIsNumbered && !pOrg.HasPhoto;
							if ( fMove )
							{
								Dialogs.FAskMoveOrCopyPerson.showDialog( parent, out fMove );
								if ( fMove )
									pOrg.Grupp.raderaPerson( pOrg );
							}
						}
						else
							p.IST = "";
						break;

					default:
						p = null;
						break;
				}
				return p;
			}
		}

		private void lv_DoubleClick(object sender, System.EventArgs e)
		{
			if ( lv.SelectedItems.Count==1 )
				this.DialogResult = DialogResult.Yes;
		}

		private void fixNumeric( TextBox tb )
		{
			string s = tb.Text;
			for ( int i = 0 ; i < s.Length ; i++ )
				if ( s[i] < '0' || s[i] > '9' )
					s = s.Remove( i, 1 );
			if ( s.Length != tb.Text.Length )
				tb.Text = s;
		}

		private void checkPNum()
		{
			string s = txtPersnr1.Text + txtPersnr2.Text;
			if ( s.Length == 12 )
				s = s.Substring( 2 );
			bool fComplete = s.Length == 10;
			bool fCorrect = fComplete && Util.verifyScanCode( "0" + s );
			txtPersnr1.BackColor = txtPersnr2.BackColor =
				fComplete && !fCorrect ?
				Color.Red : SystemColors.Window;
			optKvinna.AutoCheck = optMan.AutoCheck = !fCorrect;
			if ( fCorrect )
			{
				optKvinna.Checked = optMan.Checked = false;
				Global.optCheck( (s[8] & 1) == 0, optKvinna, optMan );
			}
		}

		private void txtPnr1_TextChanged( object sender, EventArgs e )
		{
			fixNumeric( txtPersnr1 );
			if ( txtPersnr1.SelectionStart == 8 )
				txtPersnr2.Focus();
			checkPNum();
		}

		private void txtPnr2_TextChanged( object sender, EventArgs e )
		{
			fixNumeric( txtPersnr2 );
			checkPNum();
		}

		private void grpProtected_CheckChanged( object sender, EventArgs e )
		{
			bool fVisible = grpProtected.OptionSelected == 1;
			foreach ( Control C in grpProtected.Controls )
				C.Visible = fVisible;
			grpProtected.Invalidate();
		}

		private void grpProtected_Paint( object sender, PaintEventArgs e )
		{
			if ( grpProtected.OptionSelected == 0 )
				e.Graphics.DrawString(
					"Personen har inte skyddad identitet",
					grpProtected.Font,
					SystemBrushes.ControlText,
					lblKatalogOchDVD.Location );
		}

		private void autocorrect_TextChanged( object sender, EventArgs e )
		{
			autoCorrect( sender as TextBox );
			displayHits();
		}

	}

}
