using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using PlataDM;

namespace Plata
{

	public class FAskWithCheckbox : vdUsr.baseGradientForm
	{
		private System.Windows.Forms.Button cmdYes;
		private System.Windows.Forms.Button cmdNo;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Label lblQuestion;
		private CheckBox chkConfirm;

		private FAskWithCheckbox()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
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
			this.cmdYes = new System.Windows.Forms.Button();
			this.cmdNo = new System.Windows.Forms.Button();
			this.lblQuestion = new System.Windows.Forms.Label();
			this.chkConfirm = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// cmdYes
			// 
			this.cmdYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.cmdYes.Enabled = false;
			this.cmdYes.Location = new System.Drawing.Point( 93, 100 );
			this.cmdYes.Name = "cmdYes";
			this.cmdYes.Size = new System.Drawing.Size( 80, 28 );
			this.cmdYes.TabIndex = 0;
			this.cmdYes.Text = "Ja";
			// 
			// cmdNo
			// 
			this.cmdNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdNo.DialogResult = System.Windows.Forms.DialogResult.No;
			this.cmdNo.Location = new System.Drawing.Point( 193, 100 );
			this.cmdNo.Name = "cmdNo";
			this.cmdNo.Size = new System.Drawing.Size( 80, 28 );
			this.cmdNo.TabIndex = 1;
			this.cmdNo.Text = "Nej";
			// 
			// lblQuestion
			// 
			this.lblQuestion.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.lblQuestion.Location = new System.Drawing.Point( 12, 9 );
			this.lblQuestion.Name = "lblQuestion";
			this.lblQuestion.Size = new System.Drawing.Size( 345, 65 );
			this.lblQuestion.TabIndex = 2;
			this.lblQuestion.Text = "-";
			// 
			// chkConfirm
			// 
			this.chkConfirm.AutoSize = true;
			this.chkConfirm.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.chkConfirm.Location = new System.Drawing.Point( 15, 77 );
			this.chkConfirm.Name = "chkConfirm";
			this.chkConfirm.Size = new System.Drawing.Size( 155, 17 );
			this.chkConfirm.TabIndex = 3;
			this.chkConfirm.Text = "Jag är säker på vad jag gör";
			this.chkConfirm.UseVisualStyleBackColor = false;
			this.chkConfirm.CheckedChanged += new System.EventHandler( this.chkConfirm_CheckedChanged );
			// 
			// FAskWithCheckbox
			// 
			this.AcceptButton = this.cmdYes;
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.CancelButton = this.cmdNo;
			this.ClientSize = new System.Drawing.Size( 369, 132 );
			this.Controls.Add( this.chkConfirm );
			this.Controls.Add( this.lblQuestion );
			this.Controls.Add( this.cmdNo );
			this.Controls.Add( this.cmdYes );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FAskWithCheckbox";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Bekräfta";
			this.ResumeLayout( false );
			this.PerformLayout();

		}
		#endregion

		public static DialogResult askDialog(
			Form parent,
			string strQuestion )
		{
			using ( FAskWithCheckbox dlg = new FAskWithCheckbox() )
			{
				dlg.lblQuestion.Text = strQuestion;
				return dlg.ShowDialog(parent);
			}
		}

		private void chkConfirm_CheckedChanged( object sender, EventArgs e )
		{
			cmdYes.Enabled = chkConfirm.Checked;
		}

	}

}
