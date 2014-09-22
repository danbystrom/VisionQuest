namespace Plata
{
	partial class tabPageRestPort
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
			this.lvwRestPort = new System.Windows.Forms.ListView();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// lvwRestPort
			// 
			this.lvwRestPort.AllowDrop = true;
			this.lvwRestPort.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4} );
			this.lvwRestPort.Dock = System.Windows.Forms.DockStyle.Left;
			this.lvwRestPort.FullRowSelect = true;
			this.lvwRestPort.HideSelection = false;
			this.lvwRestPort.Location = new System.Drawing.Point( 0, 0 );
			this.lvwRestPort.Name = "lvwRestPort";
			this.lvwRestPort.Size = new System.Drawing.Size( 590, 600 );
			this.lvwRestPort.TabIndex = 4;
			this.lvwRestPort.UseCompatibleStateImageBehavior = false;
			this.lvwRestPort.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Grupp";
			this.columnHeader3.Width = 130;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Ej fotograferade personer";
			this.columnHeader4.Width = 180;
			// 
			// tabPageRestPort
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.lvwRestPort );
			this.Name = "tabPageRestPort";
			this.Size = new System.Drawing.Size( 600, 600 );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.ListView lvwRestPort;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
	}
}
