﻿namespace Exempel4
{
    partial class Exempel4
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
            this.chkRotXy = new System.Windows.Forms.CheckBox();
            this.chkPause = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // chkRotXy
            // 
            this.chkRotXy.AutoSize = true;
            this.chkRotXy.Location = new System.Drawing.Point(12, 12);
            this.chkRotXy.Name = "chkRotXy";
            this.chkRotXy.Size = new System.Drawing.Size(129, 24);
            this.chkRotXy.TabIndex = 0;
            this.chkRotXy.Text = "Rotate X && Y";
            this.chkRotXy.UseVisualStyleBackColor = true;
            this.chkRotXy.CheckedChanged += new System.EventHandler(this.chkRotXy_CheckedChanged);
            // 
            // chkPause
            // 
            this.chkPause.AutoSize = true;
            this.chkPause.Location = new System.Drawing.Point(12, 42);
            this.chkPause.Name = "chkPause";
            this.chkPause.Size = new System.Drawing.Size(80, 24);
            this.chkPause.TabIndex = 2;
            this.chkPause.Text = "Pause";
            this.chkPause.UseVisualStyleBackColor = true;
            // 
            // Exempel4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1476, 797);
            this.Controls.Add(this.chkPause);
            this.Controls.Add(this.chkRotXy);
            this.DoubleBuffered = true;
            this.Name = "Exempel4";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkRotXy;
        private System.Windows.Forms.CheckBox chkPause;
    }
}

