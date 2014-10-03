namespace factor10.VisionQuest
{
    internal partial class FMain
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
            this.pnRenderControlPanel = new System.Windows.Forms.Panel();
            this.btnNewProject = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.optAllLines = new System.Windows.Forms.RadioButton();
            this.optNoLines = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnProperties = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnRenderControlPanel
            // 
            this.pnRenderControlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnRenderControlPanel.Location = new System.Drawing.Point(317, 13);
            this.pnRenderControlPanel.Margin = new System.Windows.Forms.Padding(4);
            this.pnRenderControlPanel.Name = "pnRenderControlPanel";
            this.pnRenderControlPanel.Size = new System.Drawing.Size(1389, 1055);
            this.pnRenderControlPanel.TabIndex = 3;
            // 
            // btnNewProject
            // 
            this.btnNewProject.Location = new System.Drawing.Point(12, 13);
            this.btnNewProject.Name = "btnNewProject";
            this.btnNewProject.Size = new System.Drawing.Size(146, 42);
            this.btnNewProject.TabIndex = 0;
            this.btnNewProject.Text = "New project...";
            this.btnNewProject.UseVisualStyleBackColor = true;
            this.btnNewProject.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 137);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 26);
            this.textBox1.TabIndex = 4;
            // 
            // optAllLines
            // 
            this.optAllLines.AutoSize = true;
            this.optAllLines.Location = new System.Drawing.Point(13, 25);
            this.optAllLines.Name = "optAllLines";
            this.optAllLines.Size = new System.Drawing.Size(51, 24);
            this.optAllLines.TabIndex = 5;
            this.optAllLines.TabStop = true;
            this.optAllLines.Text = "All";
            this.optAllLines.UseVisualStyleBackColor = true;
            // 
            // optNoLines
            // 
            this.optNoLines.AutoSize = true;
            this.optNoLines.Location = new System.Drawing.Point(13, 55);
            this.optNoLines.Name = "optNoLines";
            this.optNoLines.Size = new System.Drawing.Size(72, 24);
            this.optNoLines.TabIndex = 6;
            this.optNoLines.TabStop = true;
            this.optNoLines.Text = "None";
            this.optNoLines.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.optAllLines);
            this.groupBox1.Controls.Add(this.optNoLines);
            this.groupBox1.Location = new System.Drawing.Point(12, 460);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Lines";
            // 
            // btnProperties
            // 
            this.btnProperties.Location = new System.Drawing.Point(164, 13);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(146, 42);
            this.btnProperties.TabIndex = 8;
            this.btnProperties.Text = "Properties...";
            this.btnProperties.UseVisualStyleBackColor = true;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // FMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1719, 1081);
            this.Controls.Add(this.btnProperties);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnNewProject);
            this.Controls.Add(this.pnRenderControlPanel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FMain";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnRenderControlPanel;
        private System.Windows.Forms.Button btnNewProject;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RadioButton optAllLines;
        private System.Windows.Forms.RadioButton optNoLines;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnProperties;
    }
}

