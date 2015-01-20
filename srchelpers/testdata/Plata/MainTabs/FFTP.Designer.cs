namespace Plata
{
	partial class FFTP
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.ugToDo = new vdXceed.vdPlainGrid();
			this.ugDone = new vdXceed.vdPlainGrid();
			this.cmdUp = new System.Windows.Forms.Button();
			this.cmdDown = new System.Windows.Forms.Button();
			this.txtMsg = new System.Windows.Forms.TextBox();
			this.cmdGo = new System.Windows.Forms.Button();
			this.ugProgress = new vdXceed.vdPlainGrid();
			this.cmdStop = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer( this.components );
			this.cmdAll = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.ugToDo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ugDone)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ugProgress)).BeginInit();
			this.SuspendLayout();
			// 
			// ugToDo
			// 
			this.ugToDo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.ugToDo.Caption = null;
			this.ugToDo.Location = new System.Drawing.Point( 12, 12 );
			this.ugToDo.Name = "ugToDo";
			this.ugToDo.Size = new System.Drawing.Size( 875, 180 );
			this.ugToDo.TabIndex = 1;
			// 
			// ugDone
			// 
			this.ugDone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.ugDone.Caption = "Överförda";
			this.ugDone.Location = new System.Drawing.Point( 12, 198 );
			this.ugDone.Name = "ugDone";
			this.ugDone.Size = new System.Drawing.Size( 959, 180 );
			this.ugDone.TabIndex = 2;
			// 
			// cmdUp
			// 
			this.cmdUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdUp.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.cmdUp.Location = new System.Drawing.Point( 893, 138 );
			this.cmdUp.Name = "cmdUp";
			this.cmdUp.Size = new System.Drawing.Size( 77, 24 );
			this.cmdUp.TabIndex = 3;
			this.cmdUp.Text = "upp";
			this.cmdUp.UseVisualStyleBackColor = false;
			this.cmdUp.Click += new System.EventHandler( this.cmdUp_Click );
			// 
			// cmdDown
			// 
			this.cmdDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdDown.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.cmdDown.Location = new System.Drawing.Point( 893, 168 );
			this.cmdDown.Name = "cmdDown";
			this.cmdDown.Size = new System.Drawing.Size( 78, 24 );
			this.cmdDown.TabIndex = 4;
			this.cmdDown.Text = "ner";
			this.cmdDown.UseVisualStyleBackColor = false;
			this.cmdDown.Click += new System.EventHandler( this.cmdDown_Click );
			// 
			// txtMsg
			// 
			this.txtMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtMsg.BackColor = System.Drawing.SystemColors.Window;
			this.txtMsg.Location = new System.Drawing.Point( 12, 518 );
			this.txtMsg.Multiline = true;
			this.txtMsg.Name = "txtMsg";
			this.txtMsg.ReadOnly = true;
			this.txtMsg.Size = new System.Drawing.Size( 958, 150 );
			this.txtMsg.TabIndex = 5;
			// 
			// cmdGo
			// 
			this.cmdGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdGo.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.cmdGo.Location = new System.Drawing.Point( 893, 12 );
			this.cmdGo.Name = "cmdGo";
			this.cmdGo.Size = new System.Drawing.Size( 78, 30 );
			this.cmdGo.TabIndex = 6;
			this.cmdGo.Text = "KÖR!";
			this.cmdGo.UseVisualStyleBackColor = false;
			this.cmdGo.Click += new System.EventHandler( this.cmdGo_Click );
			// 
			// ugProgress
			// 
			this.ugProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.ugProgress.Caption = "Pågående";
			this.ugProgress.Location = new System.Drawing.Point( 11, 384 );
			this.ugProgress.Name = "ugProgress";
			this.ugProgress.ReadOnly = true;
			this.ugProgress.Size = new System.Drawing.Size( 959, 128 );
			this.ugProgress.TabIndex = 8;
			// 
			// cmdStop
			// 
			this.cmdStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdStop.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.cmdStop.Location = new System.Drawing.Point( 893, 48 );
			this.cmdStop.Name = "cmdStop";
			this.cmdStop.Size = new System.Drawing.Size( 78, 30 );
			this.cmdStop.TabIndex = 9;
			this.cmdStop.Text = "STOPP!";
			this.cmdStop.UseVisualStyleBackColor = false;
			this.cmdStop.Click += new System.EventHandler( this.cmdStop_Click );
			// 
			// timer1
			// 
			this.timer1.Interval = 10000;
			this.timer1.Tick += new System.EventHandler( this.timer1_Tick );
			// 
			// cmdAll
			// 
			this.cmdAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdAll.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.cmdAll.Location = new System.Drawing.Point( 893, 108 );
			this.cmdAll.Name = "cmdAll";
			this.cmdAll.Size = new System.Drawing.Size( 77, 24 );
			this.cmdAll.TabIndex = 10;
			this.cmdAll.Text = "alla";
			this.cmdAll.UseVisualStyleBackColor = false;
			this.cmdAll.Click += new System.EventHandler( this.cmdAll_Click );
			// 
			// FFTP
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 7F, 16F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 983, 680 );
			this.Controls.Add( this.cmdAll );
			this.Controls.Add( this.cmdStop );
			this.Controls.Add( this.ugProgress );
			this.Controls.Add( this.cmdGo );
			this.Controls.Add( this.txtMsg );
			this.Controls.Add( this.cmdDown );
			this.Controls.Add( this.cmdUp );
			this.Controls.Add( this.ugDone );
			this.Controls.Add( this.ugToDo );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "FFTP";
			((System.ComponentModel.ISupportInitialize)(this.ugToDo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ugDone)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ugProgress)).EndInit();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private vdXceed.vdPlainGrid ugToDo;
		private vdXceed.vdPlainGrid ugDone;
		private System.Windows.Forms.Button cmdUp;
		private System.Windows.Forms.Button cmdDown;
		private System.Windows.Forms.TextBox txtMsg;
		private System.Windows.Forms.Button cmdGo;
		private vdXceed.vdPlainGrid ugProgress;
		private System.Windows.Forms.Button cmdStop;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Button cmdAll;
	}
}