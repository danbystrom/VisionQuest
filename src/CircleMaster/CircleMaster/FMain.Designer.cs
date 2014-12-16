namespace CircleMasterApp
{
    partial class FMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.numRadius = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numRadius)).BeginInit();
            this.SuspendLayout();
            // 
            // numRadius
            // 
            this.numRadius.Location = new System.Drawing.Point(12, 12);
            this.numRadius.Name = "numRadius";
            this.numRadius.Size = new System.Drawing.Size(73, 26);
            this.numRadius.TabIndex = 0;
            this.numRadius.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // FMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2060, 1181);
            this.Controls.Add(this.numRadius);
            this.DoubleBuffered = true;
            this.Name = "FMain";
            this.Text = "CircleMaster";
            ((System.ComponentModel.ISupportInitialize)(this.numRadius)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numRadius;
    }
}

