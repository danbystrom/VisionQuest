using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using PlataDM;

namespace Plata.ImageStuff
{

	public class FAskAboutOrientation : vdUsr.baseGradientForm
	{
		private System.Windows.Forms.Button cmdYes;
		private System.Windows.Forms.Button cmdNo;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Label lblQuestion;

		private FAskAboutOrientation()
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
			this.SuspendLayout();
			// 
			// cmdYes
			// 
			this.cmdYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.cmdYes.Enabled = false;
			this.cmdYes.Location = new System.Drawing.Point( 15, 52 );
			this.cmdYes.Name = "cmdYes";
			this.cmdYes.Size = new System.Drawing.Size( 287, 28 );
			this.cmdYes.TabIndex = 1;
			this.cmdYes.Text = "&Ja, det är rätt. Det här ska vara en \"landscape\"-bild.";
			// 
			// cmdNo
			// 
			this.cmdNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdNo.DialogResult = System.Windows.Forms.DialogResult.No;
			this.cmdNo.Location = new System.Drawing.Point( 15, 86 );
			this.cmdNo.Name = "cmdNo";
			this.cmdNo.Size = new System.Drawing.Size( 287, 28 );
			this.cmdNo.TabIndex = 0;
			this.cmdNo.Text = "&Nej, det blev fel. Radera den!";
			// 
			// lblQuestion
			// 
			this.lblQuestion.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.lblQuestion.Location = new System.Drawing.Point( 12, 9 );
			this.lblQuestion.Name = "lblQuestion";
			this.lblQuestion.Size = new System.Drawing.Size( 290, 40 );
			this.lblQuestion.TabIndex = 2;
			this.lblQuestion.Text = "Denna bild är i \"landscape\"-format och inte i \"portrait\". Vill du använda bilden " +
					"ändå?";
			// 
			// FAskAboutOrientation
			// 
			this.AcceptButton = this.cmdYes;
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.CancelButton = this.cmdNo;
			this.ClientSize = new System.Drawing.Size( 322, 132 );
			this.Controls.Add( this.lblQuestion );
			this.Controls.Add( this.cmdNo );
			this.Controls.Add( this.cmdYes );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FAskAboutOrientation";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Bild i \"landscape\"-format";
			this.ResumeLayout( false );

		}
		#endregion

		public static DialogResult askDialog(
			Form parent,
			string strQuestion )
		{
			using ( FAskAboutOrientation dlg = new FAskAboutOrientation() )
			{
				return dlg.ShowDialog(parent);
			}
		}

	}

}
