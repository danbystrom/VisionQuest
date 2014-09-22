namespace Plata
{
	partial class tabPageStatistics
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
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem( new string[] {
            "Totalt antal fotografier",
            "-"}, -1 );
			System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem( new string[] {
            "Varav valda",
            "-"}, -1 );
			System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem( new string[] {
            "Totalt antal vimmelbilder",
            "-"}, -1 );
			System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem( new string[] {
            "Varav lovade",
            "-"}, -1 );
			System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem( new string[] {
            "Varav valda",
            "-"}, -1 );
			this.lvStatistik = new System.Windows.Forms.ListView();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.lvVimmel = new System.Windows.Forms.ListView();
			this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// lvStatistik
			// 
			this.lvStatistik.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6} );
			this.lvStatistik.Items.AddRange( new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2} );
			this.lvStatistik.Location = new System.Drawing.Point( 3, 105 );
			this.lvStatistik.Name = "lvStatistik";
			this.lvStatistik.Size = new System.Drawing.Size( 274, 83 );
			this.lvStatistik.TabIndex = 6;
			this.lvStatistik.UseCompatibleStateImageBehavior = false;
			this.lvStatistik.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Porträtt";
			this.columnHeader5.Width = 150;
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "Antal";
			this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.columnHeader6.Width = 45;
			// 
			// lvVimmel
			// 
			this.lvVimmel.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader8} );
			this.lvVimmel.Items.AddRange( new System.Windows.Forms.ListViewItem[] {
            listViewItem3,
            listViewItem4,
            listViewItem5} );
			this.lvVimmel.Location = new System.Drawing.Point( 3, 3 );
			this.lvVimmel.Name = "lvVimmel";
			this.lvVimmel.Size = new System.Drawing.Size( 274, 92 );
			this.lvVimmel.TabIndex = 5;
			this.lvVimmel.UseCompatibleStateImageBehavior = false;
			this.lvVimmel.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader7
			// 
			this.columnHeader7.Text = "Vimmelbilder";
			this.columnHeader7.Width = 150;
			// 
			// columnHeader8
			// 
			this.columnHeader8.Text = "Antal";
			this.columnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.columnHeader8.Width = 45;
			// 
			// tabPageStatistics
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.lvStatistik );
			this.Controls.Add( this.lvVimmel );
			this.Name = "tabPageStatistics";
			this.Size = new System.Drawing.Size( 529, 417 );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.ListView lvStatistik;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.ListView lvVimmel;
		private System.Windows.Forms.ColumnHeader columnHeader7;
		private System.Windows.Forms.ColumnHeader columnHeader8;
	}
}
