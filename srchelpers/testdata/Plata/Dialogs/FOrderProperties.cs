using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PlataDM;

namespace Plata.Dialogs
{
	/// <summary>
	/// Summary description for FPassword.
	/// </summary>
	public class FOrderProperties : vdUsr.baseGradientForm
	{
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdOK;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private GroupBox grpFtg;
		private RadioButton optCompanyPhotomic;
		private RadioButton optCompanySmallImage;
		private RadioButton optCompanyNo;
        private RichTextBox rtf;
        private Controls.SelectBackdrop selectBackdrop1;
		private GroupBox groupBox2;

		private FOrderProperties()
		{
			InitializeComponent();
		}

		private FOrderProperties( Skola skola )
		{
			InitializeComponent();

            if (Global.Preferences.Brand == Brand.Kungsfoto)
                grpFtg.Visible = false;

		    selectBackdrop1.Backdrop = skola.Backdrop;

			switch ( skola.CompanyOrder )
			{
				case CompanyOrder.SmallImage:
					optCompanySmallImage.Checked = true;
					break;
				case CompanyOrder.Photomic:
					optCompanyPhotomic.Checked = true;
					break;
			}

			int nAntalG = 0, nValdaG = 0;
			int nAntalP = 0, nValdaP = 0;
			int nAntalV, nValdaV, nLovadeV;
			foreach ( var grupp in Global.Skola.Grupper )
			{
				nAntalG += grupp.Thumbnails.Count;
				if ( !string.IsNullOrEmpty( grupp.ThumbnailKey ) )
					nValdaG++;
				foreach ( var pers in grupp.AllaPersoner )
				{
					nAntalP += pers.Thumbnails.Count;
					if ( !string.IsNullOrEmpty( pers.ThumbnailKey ) )
						nValdaP++;
				}
			}

			rtf.SelectionTabs = new [] { 10, 80, 120, 160 };

			rtf.SelectionFont = new Font( this.Font, FontStyle.Bold );
			rtf.AppendText( "Gruppbilder\r\n" );
			rtf.SelectionFont = this.Font;
			rtf.AppendText( string.Format( "\tAntal:\t{0}\r\n", nAntalG ) );
			rtf.AppendText( string.Format( "\tValda:\t{0}\r\n", nValdaG ) );

			rtf.SelectionFont = new Font( this.Font, FontStyle.Bold );
			rtf.AppendText( "\r\nPorträtt\r\n" );
			rtf.SelectionFont = this.Font;
			rtf.AppendText( string.Format( "\tAntal:\t{0}\r\n", nAntalP ) );
			rtf.AppendText( string.Format( "\tValda:\t{0}\r\n", nValdaP ) );

			Global.Skola.Vimmel.räkna( out nAntalV, out nLovadeV, out nValdaV );

			rtf.SelectionFont = new Font( this.Font, FontStyle.Bold );
			rtf.AppendText( "\r\nVimmel\r\n" );
			rtf.SelectionFont = this.Font;
			rtf.AppendText( string.Format( "\tTotalt:\t{0}\r\n", nAntalV ) );
			rtf.AppendText( string.Format( "\tValda:\t{0}\r\n", nValdaV ) );
			rtf.AppendText( string.Format( "\tLovade:\t{0}\r\n", nLovadeV ) );
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
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.grpFtg = new System.Windows.Forms.GroupBox();
            this.optCompanyPhotomic = new System.Windows.Forms.RadioButton();
            this.optCompanySmallImage = new System.Windows.Forms.RadioButton();
            this.optCompanyNo = new System.Windows.Forms.RadioButton();
            this.rtf = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.selectBackdrop1 = new Plata.Controls.SelectBackdrop();
            this.grpFtg.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(442, 249);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(80, 28);
            this.cmdCancel.TabIndex = 8;
            this.cmdCancel.Text = "Avbryt";
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(442, 215);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(80, 28);
            this.cmdOK.TabIndex = 7;
            this.cmdOK.Text = "OK";
            // 
            // grpFtg
            // 
            this.grpFtg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpFtg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grpFtg.Controls.Add(this.optCompanyPhotomic);
            this.grpFtg.Controls.Add(this.optCompanySmallImage);
            this.grpFtg.Controls.Add(this.optCompanyNo);
            this.grpFtg.Location = new System.Drawing.Point(372, 66);
            this.grpFtg.Name = "grpFtg";
            this.grpFtg.Size = new System.Drawing.Size(150, 87);
            this.grpFtg.TabIndex = 9;
            this.grpFtg.TabStop = false;
            this.grpFtg.Text = "Företagsfoto";
            // 
            // optCompanyPhotomic
            // 
            this.optCompanyPhotomic.AutoSize = true;
            this.optCompanyPhotomic.Location = new System.Drawing.Point(29, 60);
            this.optCompanyPhotomic.Name = "optCompanyPhotomic";
            this.optCompanyPhotomic.Size = new System.Drawing.Size(69, 17);
            this.optCompanyPhotomic.TabIndex = 2;
            this.optCompanyPhotomic.Text = "Photomic";
            this.optCompanyPhotomic.UseVisualStyleBackColor = true;
            // 
            // optCompanySmallImage
            // 
            this.optCompanySmallImage.AutoSize = true;
            this.optCompanySmallImage.Location = new System.Drawing.Point(29, 40);
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
            this.optCompanyNo.Location = new System.Drawing.Point(6, 18);
            this.optCompanyNo.Name = "optCompanyNo";
            this.optCompanyNo.Size = new System.Drawing.Size(41, 17);
            this.optCompanyNo.TabIndex = 0;
            this.optCompanyNo.TabStop = true;
            this.optCompanyNo.Text = "Nej";
            this.optCompanyNo.UseVisualStyleBackColor = true;
            // 
            // rtf
            // 
            this.rtf.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtf.Location = new System.Drawing.Point(6, 19);
            this.rtf.Name = "rtf";
            this.rtf.ReadOnly = true;
            this.rtf.Size = new System.Drawing.Size(342, 240);
            this.rtf.TabIndex = 10;
            this.rtf.Text = "";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.groupBox2.Controls.Add(this.rtf);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(354, 265);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Statistik";
            // 
            // selectBackdrop1
            // 
            this.selectBackdrop1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.selectBackdrop1.Backdrop = "";
            this.selectBackdrop1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.selectBackdrop1.Location = new System.Drawing.Point(372, 12);
            this.selectBackdrop1.Name = "selectBackdrop1";
            this.selectBackdrop1.Size = new System.Drawing.Size(150, 48);
            this.selectBackdrop1.TabIndex = 13;
            // 
            // FOrderProperties
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(534, 289);
            this.Controls.Add(this.selectBackdrop1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpFtg);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FOrderProperties";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Egenskaper";
            this.grpFtg.ResumeLayout(false);
            this.grpFtg.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		public static DialogResult showDialog( Form parent, Skola skola )
		{
			using ( var dlg = new FOrderProperties( skola ) )
			{
				var retVal = dlg.ShowDialog( parent );

			    var backdrop = dlg.selectBackdrop1.Backdrop;
                CompanyOrder co;
				if ( dlg.optCompanyPhotomic.Checked )
					co = CompanyOrder.Photomic;
				else if ( dlg.optCompanySmallImage.Checked )
					co = CompanyOrder.SmallImage;
				else
					co = CompanyOrder.No;
				if ( retVal == DialogResult.Cancel || (co == skola.CompanyOrder && backdrop==skola.Backdrop) )
					return DialogResult.Cancel;
			    skola.Backdrop = backdrop;
				skola.CompanyOrder = co;
				return DialogResult.OK;
			}
		}

	}

}
