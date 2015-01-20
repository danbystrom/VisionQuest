namespace Plata
{
	partial class tabPageAnmärkningar
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtAnmDiverse = new System.Windows.Forms.TextBox();
			this.txtAnmKatalog = new System.Windows.Forms.TextBox();
			this.txtAnmBok = new System.Windows.Forms.TextBox();
			this.txtAnmFoto = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtAnmDiverse
			// 
			this.txtAnmDiverse.Location = new System.Drawing.Point( 3, 427 );
			this.txtAnmDiverse.Multiline = true;
			this.txtAnmDiverse.Name = "txtAnmDiverse";
			this.txtAnmDiverse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtAnmDiverse.Size = new System.Drawing.Size( 782, 115 );
			this.txtAnmDiverse.TabIndex = 9;
			// 
			// txtAnmKatalog
			// 
			this.txtAnmKatalog.Location = new System.Drawing.Point( 3, 293 );
			this.txtAnmKatalog.Multiline = true;
			this.txtAnmKatalog.Name = "txtAnmKatalog";
			this.txtAnmKatalog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtAnmKatalog.Size = new System.Drawing.Size( 782, 115 );
			this.txtAnmKatalog.TabIndex = 8;
			// 
			// txtAnmBok
			// 
			this.txtAnmBok.Location = new System.Drawing.Point( 3, 159 );
			this.txtAnmBok.Multiline = true;
			this.txtAnmBok.Name = "txtAnmBok";
			this.txtAnmBok.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtAnmBok.Size = new System.Drawing.Size( 782, 115 );
			this.txtAnmBok.TabIndex = 7;
			// 
			// txtAnmFoto
			// 
			this.txtAnmFoto.Location = new System.Drawing.Point( 3, 25 );
			this.txtAnmFoto.Multiline = true;
			this.txtAnmFoto.Name = "txtAnmFoto";
			this.txtAnmFoto.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtAnmFoto.Size = new System.Drawing.Size( 782, 115 );
			this.txtAnmFoto.TabIndex = 6;
			// 
			// tabPageAnmärkningar
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.txtAnmDiverse );
			this.Controls.Add( this.txtAnmKatalog );
			this.Controls.Add( this.txtAnmBok );
			this.Controls.Add( this.txtAnmFoto );
			this.Name = "tabPageAnmärkningar";
			this.Size = new System.Drawing.Size( 800, 600 );
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtAnmDiverse;
		private System.Windows.Forms.TextBox txtAnmKatalog;
		private System.Windows.Forms.TextBox txtAnmBok;
		private System.Windows.Forms.TextBox txtAnmFoto;
	}
}
