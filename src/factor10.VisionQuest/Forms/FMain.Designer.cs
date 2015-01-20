namespace factor10.VisionQuest.Forms
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
            this.btnMangeProjects = new System.Windows.Forms.Button();
            this.txtSearchMethod = new System.Windows.Forms.TextBox();
            this.optAllLines = new System.Windows.Forms.RadioButton();
            this.optNoLines = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnProperties = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numSurfaceScale = new System.Windows.Forms.NumericUpDown();
            this.numSurfaceSize = new System.Windows.Forms.NumericUpDown();
            this.chkHiddenWater = new System.Windows.Forms.CheckBox();
            this.lstMethods = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSurfaceScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSurfaceSize)).BeginInit();
            this.SuspendLayout();
            // 
            // pnRenderControlPanel
            // 
            this.pnRenderControlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnRenderControlPanel.Location = new System.Drawing.Point(317, 0);
            this.pnRenderControlPanel.Margin = new System.Windows.Forms.Padding(4);
            this.pnRenderControlPanel.Name = "pnRenderControlPanel";
            this.pnRenderControlPanel.Size = new System.Drawing.Size(1403, 1081);
            this.pnRenderControlPanel.TabIndex = 3;
            // 
            // btnMangeProjects
            // 
            this.btnMangeProjects.Location = new System.Drawing.Point(12, 13);
            this.btnMangeProjects.Name = "btnMangeProjects";
            this.btnMangeProjects.Size = new System.Drawing.Size(146, 42);
            this.btnMangeProjects.TabIndex = 0;
            this.btnMangeProjects.Text = "Projects...";
            this.btnMangeProjects.UseVisualStyleBackColor = true;
            this.btnMangeProjects.Click += new System.EventHandler(this.btnMangeProjects_Click);
            // 
            // txtSearchMethod
            // 
            this.txtSearchMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSearchMethod.Location = new System.Drawing.Point(12, 873);
            this.txtSearchMethod.Name = "txtSearchMethod";
            this.txtSearchMethod.Size = new System.Drawing.Size(298, 26);
            this.txtSearchMethod.TabIndex = 4;
            this.txtSearchMethod.TextChanged += new System.EventHandler(this.txtSearchMethod_TextChanged);
            this.txtSearchMethod.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchMethod_KeyDown);
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
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numSurfaceScale);
            this.groupBox2.Controls.Add(this.numSurfaceSize);
            this.groupBox2.Controls.Add(this.chkHiddenWater);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox2.Location = new System.Drawing.Point(12, 566);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 126);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Water";
            // 
            // numSurfaceScale
            // 
            this.numSurfaceScale.Location = new System.Drawing.Point(13, 87);
            this.numSurfaceScale.Name = "numSurfaceScale";
            this.numSurfaceScale.Size = new System.Drawing.Size(87, 26);
            this.numSurfaceScale.TabIndex = 12;
            this.numSurfaceScale.ValueChanged += new System.EventHandler(this.numSurcafeScale_ValueChanged);
            // 
            // numSurfaceSize
            // 
            this.numSurfaceSize.Location = new System.Drawing.Point(13, 55);
            this.numSurfaceSize.Name = "numSurfaceSize";
            this.numSurfaceSize.Size = new System.Drawing.Size(87, 26);
            this.numSurfaceSize.TabIndex = 11;
            this.numSurfaceSize.ValueChanged += new System.EventHandler(this.numSurfaceSize_ValueChanged);
            // 
            // chkHiddenWater
            // 
            this.chkHiddenWater.AutoSize = true;
            this.chkHiddenWater.Location = new System.Drawing.Point(13, 25);
            this.chkHiddenWater.Name = "chkHiddenWater";
            this.chkHiddenWater.Size = new System.Drawing.Size(68, 24);
            this.chkHiddenWater.TabIndex = 10;
            this.chkHiddenWater.Text = "Hide";
            this.chkHiddenWater.UseVisualStyleBackColor = true;
            this.chkHiddenWater.CheckedChanged += new System.EventHandler(this.chkHiddenVater_CheckedChanged);
            // 
            // lstMethods
            // 
            this.lstMethods.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lstMethods.FormattingEnabled = true;
            this.lstMethods.ItemHeight = 20;
            this.lstMethods.Location = new System.Drawing.Point(12, 905);
            this.lstMethods.Name = "lstMethods";
            this.lstMethods.Size = new System.Drawing.Size(298, 164);
            this.lstMethods.TabIndex = 10;
            this.lstMethods.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.lstMethods_Format);
            this.lstMethods.DoubleClick += new System.EventHandler(this.lstMethods_DoubleClick);
            // 
            // FMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1719, 1081);
            this.Controls.Add(this.lstMethods);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnProperties);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtSearchMethod);
            this.Controls.Add(this.btnMangeProjects);
            this.Controls.Add(this.pnRenderControlPanel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VisionQuest by Dan Byström - factor10 Solution AB";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSurfaceScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSurfaceSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnRenderControlPanel;
        private System.Windows.Forms.Button btnMangeProjects;
        private System.Windows.Forms.TextBox txtSearchMethod;
        private System.Windows.Forms.RadioButton optAllLines;
        private System.Windows.Forms.RadioButton optNoLines;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnProperties;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numSurfaceScale;
        private System.Windows.Forms.NumericUpDown numSurfaceSize;
        private System.Windows.Forms.CheckBox chkHiddenWater;
        private System.Windows.Forms.ListBox lstMethods;
    }
}

