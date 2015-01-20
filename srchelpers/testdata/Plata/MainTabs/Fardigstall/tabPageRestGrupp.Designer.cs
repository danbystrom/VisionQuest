namespace Plata
{
	partial class tabPageRestGrupp
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
			this.lvwRestGrupp = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// lvwRestGrupp
			// 
			this.lvwRestGrupp.AllowDrop = true;
			this.lvwRestGrupp.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2} );
			this.lvwRestGrupp.Dock = System.Windows.Forms.DockStyle.Left;
			this.lvwRestGrupp.FullRowSelect = true;
			this.lvwRestGrupp.HideSelection = false;
			this.lvwRestGrupp.Location = new System.Drawing.Point( 0, 0 );
			this.lvwRestGrupp.Name = "lvwRestGrupp";
			this.lvwRestGrupp.Size = new System.Drawing.Size( 590, 600 );
			this.lvwRestGrupp.TabIndex = 3;
			this.lvwRestGrupp.UseCompatibleStateImageBehavior = false;
			this.lvwRestGrupp.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Grupp";
			this.columnHeader1.Width = 130;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Frånvarande personer";
			this.columnHeader2.Width = 180;
			// 
			// tabPageRestGrupp
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.lvwRestGrupp );
			this.Name = "tabPageRestGrupp";
			this.Size = new System.Drawing.Size( 600, 600 );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.ListView lvwRestGrupp;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
	}
}
