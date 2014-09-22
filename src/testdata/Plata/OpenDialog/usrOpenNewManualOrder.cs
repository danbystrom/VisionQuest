using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace Plata.OpenDialog
{
	/// <summary>
	/// Summary description for usrOpenPhotographers.
	/// </summary>
	public class usrOpenNewManual : baseUsrTab
	{
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtOrt;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtOrderNr;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtNamn;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private IContainer components;
		private GroupBox grpFtg;
		private RadioButton optCompanyPhotomic;
		private RadioButton optCompanySmallImage;
		private RadioButton optCompanyNo;
        private Controls.SelectBackdrop selectBackdrop1;

		private bool[] _fNewTextFieldsOK = new bool[3];

		public usrOpenNewManual()
		{
			InitializeComponent();
			Text = "Manuell order";
            if (Global.Preferences.Brand == Brand.Kungsfoto)
                grpFtg.Visible = false;
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.label3 = new System.Windows.Forms.Label();
            this.txtOrt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtOrderNr = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNamn = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.grpFtg = new System.Windows.Forms.GroupBox();
            this.optCompanyPhotomic = new System.Windows.Forms.RadioButton();
            this.optCompanySmallImage = new System.Windows.Forms.RadioButton();
            this.optCompanyNo = new System.Windows.Forms.RadioButton();
            this.selectBackdrop1 = new Plata.Controls.SelectBackdrop();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.grpFtg.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(16, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Ort";
            // 
            // txtOrt
            // 
            this.txtOrt.Location = new System.Drawing.Point(12, 112);
            this.txtOrt.Name = "txtOrt";
            this.txtOrt.Size = new System.Drawing.Size(220, 20);
            this.txtOrt.TabIndex = 5;
            this.txtOrt.TextChanged += new System.EventHandler(this.txtOrt_TextChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Ordernummer";
            // 
            // txtOrderNr
            // 
            this.txtOrderNr.Location = new System.Drawing.Point(12, 68);
            this.txtOrderNr.MaxLength = 6;
            this.txtOrderNr.Name = "txtOrderNr";
            this.txtOrderNr.Size = new System.Drawing.Size(220, 20);
            this.txtOrderNr.TabIndex = 3;
            this.txtOrderNr.TextChanged += new System.EventHandler(this.txtOrderNr_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Namn";
            // 
            // txtNamn
            // 
            this.txtNamn.Location = new System.Drawing.Point(12, 24);
            this.txtNamn.Name = "txtNamn";
            this.txtNamn.Size = new System.Drawing.Size(220, 20);
            this.txtNamn.TabIndex = 1;
            this.txtNamn.TextChanged += new System.EventHandler(this.txtNamn_TextChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // grpFtg
            // 
            this.grpFtg.Controls.Add(this.optCompanyPhotomic);
            this.grpFtg.Controls.Add(this.optCompanySmallImage);
            this.grpFtg.Controls.Add(this.optCompanyNo);
            this.grpFtg.Location = new System.Drawing.Point(238, 62);
            this.grpFtg.Name = "grpFtg";
            this.grpFtg.Size = new System.Drawing.Size(150, 75);
            this.grpFtg.TabIndex = 6;
            this.grpFtg.TabStop = false;
            this.grpFtg.Text = "Företagsfoto";
            // 
            // optCompanyPhotomic
            // 
            this.optCompanyPhotomic.AutoSize = true;
            this.optCompanyPhotomic.Location = new System.Drawing.Point(29, 54);
            this.optCompanyPhotomic.Name = "optCompanyPhotomic";
            this.optCompanyPhotomic.Size = new System.Drawing.Size(69, 17);
            this.optCompanyPhotomic.TabIndex = 2;
            this.optCompanyPhotomic.Text = "Photomic";
            this.optCompanyPhotomic.UseVisualStyleBackColor = true;
            // 
            // optCompanySmallImage
            // 
            this.optCompanySmallImage.AutoSize = true;
            this.optCompanySmallImage.Location = new System.Drawing.Point(29, 34);
            this.optCompanySmallImage.Name = "optCompanySmallImage";
            this.optCompanySmallImage.Size = new System.Drawing.Size(82, 17);
            this.optCompanySmallImage.TabIndex = 1;
            this.optCompanySmallImage.Text = "Small Image";
            this.optCompanySmallImage.UseVisualStyleBackColor = true;
            // 
            // optCompanyNo
            // 
            this.optCompanyNo.AutoSize = true;
            this.optCompanyNo.Checked = true;
            this.optCompanyNo.Location = new System.Drawing.Point(6, 15);
            this.optCompanyNo.Name = "optCompanyNo";
            this.optCompanyNo.Size = new System.Drawing.Size(41, 17);
            this.optCompanyNo.TabIndex = 0;
            this.optCompanyNo.TabStop = true;
            this.optCompanyNo.Text = "Nej";
            this.optCompanyNo.UseVisualStyleBackColor = true;
            // 
            // selectBackdrop1
            // 
            this.selectBackdrop1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.selectBackdrop1.Backdrop = "";
            this.selectBackdrop1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.selectBackdrop1.Location = new System.Drawing.Point(238, 8);
            this.selectBackdrop1.Name = "selectBackdrop1";
            this.selectBackdrop1.Size = new System.Drawing.Size(150, 48);
            this.selectBackdrop1.TabIndex = 14;
            // 
            // usrOpenNewManual
            // 
            this.Controls.Add(this.selectBackdrop1);
            this.Controls.Add(this.grpFtg);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtOrt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtOrderNr);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNamn);
            this.Name = "usrOpenNewManual";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.grpFtg.ResumeLayout(false);
            this.grpFtg.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		public override void activate()
		{
			base.activate();
			txtNamn.Focus();
		}

		public override bool openOrder( PlataDM.Skola skola )
		{
			PlataDM.CompanyOrder co;
			if ( optCompanyPhotomic.Checked )
				co = PlataDM.CompanyOrder.Photomic;
			else if ( optCompanySmallImage.Checked )
				co = PlataDM.CompanyOrder.SmallImage;
			else
				co = PlataDM.CompanyOrder.No;
			var retVal = skola.createNew(
				Global.Preferences.MainPath, 
				txtNamn.Text, 
				txtOrderNr.Text, 
				txtOrt.Text,
				co );
		    skola.Backdrop = selectBackdrop1.Backdrop;
		    return retVal;
		}

		private void txtNamn_TextChanged(object sender, System.EventArgs e)
		{
			_fNewTextFieldsOK[0] = txtNamn.Text.CompareTo( Global.skapaSäkertFilnamn(txtNamn.Text) ) == 0;
			errorProvider1.SetError( txtNamn, _fNewTextFieldsOK[0] ? "" : "Otillåtet tecken!" );
			fireOK();
		}

		private void txtOrderNr_TextChanged(object sender, System.EventArgs e)
		{
			_fNewTextFieldsOK[1] = true;
			foreach ( char c in txtOrderNr.Text.ToCharArray() )
				if ( c<'0' || c>'9' )
					_fNewTextFieldsOK[1] = false;
			errorProvider1.SetError( txtOrderNr, _fNewTextFieldsOK[1] ? "" : "Endast siffror!" );
			fireOK();
		}

		private void txtOrt_TextChanged(object sender, System.EventArgs e)
		{
			fireOK();
		}

		private bool containsNonNumericChars( string s )
		{
			foreach ( char c in s.ToCharArray() )
				if ( c<'0' || c>'9' )
					return true;
			return false;
		}

		public override bool isOK
		{
			get
			{
				if ( txtOrderNr.Text.Length<3 || txtNamn.Text.Length<3 || txtOrt.Text.Length<2 )
					return false;
				else
					return _fNewTextFieldsOK[0] & _fNewTextFieldsOK[1];
			}
		}

	}

}
