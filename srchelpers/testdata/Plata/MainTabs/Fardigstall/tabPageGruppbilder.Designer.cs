namespace Plata
{
	partial class tabPageGruppbilder
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( tabPageGruppbilder ) );
			this.lblGOrsakHjälp = new System.Windows.Forms.Label();
			this.ugG = new vdXceed.vdPlainGrid();
			this.mnuGrupp = new System.Windows.Forms.ContextMenuStrip( this.components );
			this.mnuSortAlpha = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSortDigit = new System.Windows.Forms.ToolStripMenuItem();
			this.ugGrupperEjFotade = new vdXceed.vdPlainGrid();
			this.pnlMultiDrag = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.ugG)).BeginInit();
			this.mnuGrupp.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ugGrupperEjFotade)).BeginInit();
			this.SuspendLayout();
			// 
			// lblGOrsakHjälp
			// 
			this.lblGOrsakHjälp.Location = new System.Drawing.Point( 630, 598 );
			this.lblGOrsakHjälp.Name = "lblGOrsakHjälp";
			this.lblGOrsakHjälp.Size = new System.Drawing.Size( 553, 64 );
			this.lblGOrsakHjälp.TabIndex = 6;
			this.lblGOrsakHjälp.Text = resources.GetString( "lblGOrsakHjälp.Text" );
			// 
			// ugG
			// 
			this.ugG.CanBeSorted = false;
			this.ugG.Caption = null;
			this.ugG.ContextMenuStrip = this.mnuGrupp;
			this.ugG.Location = new System.Drawing.Point( 3, 3 );
			this.ugG.Name = "ugG";
			this.ugG.Size = new System.Drawing.Size( 620, 659 );
			this.ugG.SupportDragDrop = true;
			this.ugG.TabIndex = 5;
			this.ugG.DragDropEvent += new vdXceed.DragDropEvent( this.ugG_DragDropEvent );
			this.ugG.CellValueChanged += new System.EventHandler( this.ugG_CellValueChanged );
			this.ugG.SelectedRowsChanged += new System.EventHandler( this.ugG_SelectedRowsChanged );
			this.ugG.CellDoubleClick += new System.EventHandler( this.ugG_CellDoubleClick );
			// 
			// mnuGrupp
			// 
			this.mnuGrupp.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.mnuSortAlpha,
            this.mnuSortDigit} );
			this.mnuGrupp.Name = "mnuGrupp";
			this.mnuGrupp.Size = new System.Drawing.Size( 215, 48 );
			// 
			// mnuSortAlpha
			// 
			this.mnuSortAlpha.Name = "mnuSortAlpha";
			this.mnuSortAlpha.Size = new System.Drawing.Size( 214, 22 );
			this.mnuSortAlpha.Text = "Sortera alfabetiskt";
			this.mnuSortAlpha.Click += new System.EventHandler( this.mnuSortAlpha_Click );
			// 
			// mnuSortDigit
			// 
			this.mnuSortDigit.Name = "mnuSortDigit";
			this.mnuSortDigit.Size = new System.Drawing.Size( 214, 22 );
			this.mnuSortDigit.Text = "Sortera efter första siffran";
			this.mnuSortDigit.Click += new System.EventHandler( this.mnuSortDigit_Click );
			// 
			// ugGrupperEjFotade
			// 
			this.ugGrupperEjFotade.Caption = "Ej fotade grupper";
			this.ugGrupperEjFotade.Location = new System.Drawing.Point( 633, 3 );
			this.ugGrupperEjFotade.Name = "ugGrupperEjFotade";
			this.ugGrupperEjFotade.Size = new System.Drawing.Size( 556, 592 );
			this.ugGrupperEjFotade.TabIndex = 4;
			this.ugGrupperEjFotade.CellValueChanged += new System.EventHandler( this.ugGrupperEjFotade_CellValueChanged );
			// 
			// pnlMultiDrag
			// 
			this.pnlMultiDrag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.pnlMultiDrag.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(128)))) );
			this.pnlMultiDrag.Location = new System.Drawing.Point( 624, 646 );
			this.pnlMultiDrag.Name = "pnlMultiDrag";
			this.pnlMultiDrag.Size = new System.Drawing.Size( 16, 16 );
			this.pnlMultiDrag.TabIndex = 7;
			this.pnlMultiDrag.MouseMove += new System.Windows.Forms.MouseEventHandler( this.pnlMultiDrag_MouseMove );
			this.pnlMultiDrag.Paint += new System.Windows.Forms.PaintEventHandler( this.pnlMultiDrag_Paint );
			// 
			// tabPageGruppbilder
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.pnlMultiDrag );
			this.Controls.Add( this.lblGOrsakHjälp );
			this.Controls.Add( this.ugG );
			this.Controls.Add( this.ugGrupperEjFotade );
			this.Name = "tabPageGruppbilder";
			this.Size = new System.Drawing.Size( 1200, 680 );
			((System.ComponentModel.ISupportInitialize)(this.ugG)).EndInit();
			this.mnuGrupp.ResumeLayout( false );
			((System.ComponentModel.ISupportInitialize)(this.ugGrupperEjFotade)).EndInit();
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.Label lblGOrsakHjälp;
		private vdXceed.vdPlainGrid ugG;
		private vdXceed.vdPlainGrid ugGrupperEjFotade;
		private System.Windows.Forms.ContextMenuStrip mnuGrupp;
		private System.Windows.Forms.ToolStripMenuItem mnuSortAlpha;
		private System.Windows.Forms.ToolStripMenuItem mnuSortDigit;
		private System.Windows.Forms.Panel pnlMultiDrag;
	}
}
