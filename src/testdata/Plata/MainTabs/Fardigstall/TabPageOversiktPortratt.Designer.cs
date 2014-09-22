namespace Plata.MainTabs.Fardigstall
{
	partial class TabPageÖversiktPorträtt
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
            this.lst = new System.Windows.Forms.ListBox();
            this.mnuThumbnail = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuJump = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMove = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuThumbnail.SuspendLayout();
            this.SuspendLayout();
            // 
            // lst
            // 
            this.lst.Dock = System.Windows.Forms.DockStyle.Left;
            this.lst.FormattingEnabled = true;
            this.lst.Location = new System.Drawing.Point(0, 0);
            this.lst.Name = "lst";
            this.lst.Size = new System.Drawing.Size(161, 600);
            this.lst.Sorted = true;
            this.lst.TabIndex = 0;
            this.lst.SelectedIndexChanged += new System.EventHandler(this.lst_SelectedIndexChanged);
            // 
            // mnuThumbnail
            // 
            this.mnuThumbnail.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuZoom,
            this.mnuSelect,
            this.mnuDelete,
            this.mnuJump,
            this.mnuMove});
            this.mnuThumbnail.Name = "mnuThumbnail";
            this.mnuThumbnail.Size = new System.Drawing.Size(195, 136);
            // 
            // mnuZoom
            // 
            this.mnuZoom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnuZoom.Name = "mnuZoom";
            this.mnuZoom.Size = new System.Drawing.Size(194, 22);
            this.mnuZoom.Text = "Zoom";
            this.mnuZoom.Click += new System.EventHandler(this.mnuZoom_Click);
            // 
            // mnuSelect
            // 
            this.mnuSelect.Name = "mnuSelect";
            this.mnuSelect.Size = new System.Drawing.Size(194, 22);
            this.mnuSelect.Text = "Välj";
            this.mnuSelect.Click += new System.EventHandler(this.mnuSelect_Click);
            // 
            // mnuDelete
            // 
            this.mnuDelete.Name = "mnuDelete";
            this.mnuDelete.Size = new System.Drawing.Size(194, 22);
            this.mnuDelete.Text = "Radera";
            this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
            // 
            // mnuJump
            // 
            this.mnuJump.Name = "mnuJump";
            this.mnuJump.Size = new System.Drawing.Size(194, 22);
            this.mnuJump.Text = "Hoppa till person";
            this.mnuJump.Click += new System.EventHandler(this.mnuJump_Click);
            // 
            // mnuMove
            // 
            this.mnuMove.Name = "mnuMove";
            this.mnuMove.Size = new System.Drawing.Size(194, 22);
            this.mnuMove.Text = "Flytta till annan person";
            this.mnuMove.Click += new System.EventHandler(this.mnuMove_Click);
            // 
            // TabPageÖversiktPorträtt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lst);
            this.Name = "TabPageÖversiktPorträtt";
            this.Controls.SetChildIndex(this.lst, 0);
            this.mnuThumbnail.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox lst;
        private System.Windows.Forms.ContextMenuStrip mnuThumbnail;
        private System.Windows.Forms.ToolStripMenuItem mnuZoom;
        private System.Windows.Forms.ToolStripMenuItem mnuSelect;
        private System.Windows.Forms.ToolStripMenuItem mnuDelete;
        private System.Windows.Forms.ToolStripMenuItem mnuJump;
        private System.Windows.Forms.ToolStripMenuItem mnuMove;

	}
}
