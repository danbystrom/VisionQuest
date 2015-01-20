namespace Plata
{
	partial class usrBildÖversikt
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
            this.vsb = new System.Windows.Forms.VScrollBar();
            this.tmrLoadPictures = new System.Windows.Forms.Timer(this.components);
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.tmrInvalidate = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // vsb
            // 
            this.vsb.LargeChange = 1;
            this.vsb.Location = new System.Drawing.Point(768, 24);
            this.vsb.Maximum = 5;
            this.vsb.Name = "vsb";
            this.vsb.Size = new System.Drawing.Size(20, 494);
            this.vsb.TabIndex = 1;
            this.vsb.ValueChanged += new System.EventHandler(this.vsb_ValueChanged);
            // 
            // tmrLoadPictures
            // 
            this.tmrLoadPictures.Tick += new System.EventHandler(this.tmrLoadPictures_Tick);
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 1;
            this.trackBar1.Location = new System.Drawing.Point(697, 571);
            this.trackBar1.Maximum = 8;
            this.trackBar1.Minimum = 4;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(100, 45);
            this.trackBar1.TabIndex = 2;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.Value = 6;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // tmrInvalidate
            // 
            this.tmrInvalidate.Interval = 1000;
            this.tmrInvalidate.Tick += new System.EventHandler(this.tmrInvalidate_Tick);
            // 
            // usrBildÖversikt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.vsb);
            this.Name = "usrBildÖversikt";
            this.Size = new System.Drawing.Size(800, 600);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        protected System.Windows.Forms.Timer tmrLoadPictures;
		private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Timer tmrInvalidate;
        protected System.Windows.Forms.VScrollBar vsb;

	}
}
