namespace Plata.Burn
{
	partial class FAskAboutSaveCDToFolder
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && (components != null) )
			{
				components.Dispose();
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
			this.optToCD = new System.Windows.Forms.RadioButton();
			this.optFolder = new System.Windows.Forms.RadioButton();
			this.txtExistingFolder = new System.Windows.Forms.TextBox();
			this.cmdOK = new System.Windows.Forms.Button();
			this.pbr = new System.Windows.Forms.ProgressBar();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdBrowse = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtNewFolderName = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// optToCD
			// 
			this.optToCD.AutoSize = true;
			this.optToCD.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.optToCD.Checked = true;
			this.optToCD.Location = new System.Drawing.Point( 12, 12 );
			this.optToCD.Name = "optToCD";
			this.optToCD.Size = new System.Drawing.Size( 188, 17 );
			this.optToCD.TabIndex = 0;
			this.optToCD.TabStop = true;
			this.optToCD.Text = "Jag vill bränna filerna till CD / DVD";
			this.optToCD.UseVisualStyleBackColor = false;
			// 
			// optFolder
			// 
			this.optFolder.AutoSize = true;
			this.optFolder.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.optFolder.Location = new System.Drawing.Point( 12, 35 );
			this.optFolder.Name = "optFolder";
			this.optFolder.Size = new System.Drawing.Size( 236, 17 );
			this.optFolder.TabIndex = 1;
			this.optFolder.Text = "Jag vill spara filerna här istället för att bränna:";
			this.optFolder.UseVisualStyleBackColor = false;
			// 
			// txtExistingFolder
			// 
			this.txtExistingFolder.Location = new System.Drawing.Point( 89, 58 );
			this.txtExistingFolder.Name = "txtExistingFolder";
			this.txtExistingFolder.Size = new System.Drawing.Size( 397, 20 );
			this.txtExistingFolder.TabIndex = 2;
			this.txtExistingFolder.TextChanged += new System.EventHandler( this.txtExistingFolder_TextChanged );
			// 
			// cmdOK
			// 
			this.cmdOK.Location = new System.Drawing.Point( 353, 12 );
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size( 80, 28 );
			this.cmdOK.TabIndex = 16;
			this.cmdOK.Text = "OK";
			this.cmdOK.Click += new System.EventHandler( this.cmdOK_Click );
			// 
			// pbr
			// 
			this.pbr.Location = new System.Drawing.Point( 12, 118 );
			this.pbr.Name = "pbr";
			this.pbr.Size = new System.Drawing.Size( 507, 36 );
			this.pbr.TabIndex = 15;
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point( 439, 12 );
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size( 80, 28 );
			this.cmdCancel.TabIndex = 14;
			this.cmdCancel.Text = "Avbryt";
			// 
			// cmdBrowse
			// 
			this.cmdBrowse.Location = new System.Drawing.Point( 492, 58 );
			this.cmdBrowse.Name = "cmdBrowse";
			this.cmdBrowse.Size = new System.Drawing.Size( 27, 20 );
			this.cmdBrowse.TabIndex = 17;
			this.cmdBrowse.Text = "...";
			this.cmdBrowse.UseVisualStyleBackColor = true;
			this.cmdBrowse.Click += new System.EventHandler( this.cmdBrowse_Click );
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label1.Location = new System.Drawing.Point( 32, 58 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 51, 13 );
			this.label1.TabIndex = 18;
			this.label1.Text = "Placering";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label2.Location = new System.Drawing.Point( 32, 84 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 49, 13 );
			this.label2.TabIndex = 20;
			this.label2.Text = "Ny mapp";
			// 
			// txtNewFolderName
			// 
			this.txtNewFolderName.Location = new System.Drawing.Point( 89, 84 );
			this.txtNewFolderName.Name = "txtNewFolderName";
			this.txtNewFolderName.Size = new System.Drawing.Size( 397, 20 );
			this.txtNewFolderName.TabIndex = 19;
			this.txtNewFolderName.TextChanged += new System.EventHandler( this.txtNewFolderName_TextChanged );
			// 
			// FAskAboutSaveCDToFolder
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size( 531, 166 );
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.txtNewFolderName );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.cmdBrowse );
			this.Controls.Add( this.cmdOK );
			this.Controls.Add( this.pbr );
			this.Controls.Add( this.cmdCancel );
			this.Controls.Add( this.txtExistingFolder );
			this.Controls.Add( this.optFolder );
			this.Controls.Add( this.optToCD );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "FAskAboutSaveCDToFolder";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Bränna eller spara?";
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RadioButton optToCD;
		private System.Windows.Forms.RadioButton optFolder;
		private System.Windows.Forms.TextBox txtExistingFolder;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.ProgressBar pbr;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdBrowse;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtNewFolderName;
	}
}