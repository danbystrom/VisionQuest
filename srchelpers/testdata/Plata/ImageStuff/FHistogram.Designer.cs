﻿namespace Plata.Dialogs
{
	partial class FHistogram
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
			this.usrExifAndHistogram1 = new Photomic.Common.Img.usrExifAndHistogram();
			this.SuspendLayout();
			// 
			// usrExifAndHistogram1
			// 
			this.usrExifAndHistogram1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.usrExifAndHistogram1.Caption = null;
			this.usrExifAndHistogram1.Location = new System.Drawing.Point( 0, 0 );
			this.usrExifAndHistogram1.Name = "usrExifAndHistogram1";
			this.usrExifAndHistogram1.Size = new System.Drawing.Size( 283, 194 );
			this.usrExifAndHistogram1.TabIndex = 0;
			// 
			// FHistogram
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 283, 194 );
			this.Controls.Add( this.usrExifAndHistogram1 );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FHistogram";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Histogram";
			this.ResumeLayout( false );

		}

		#endregion

		private Photomic.Common.Img.usrExifAndHistogram usrExifAndHistogram1;
	}
}