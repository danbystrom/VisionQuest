using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Plata.Dialogs
{

	public class FAskMoveOrCopyPerson : vdUsr.baseGradientForm
	{
		private System.Windows.Forms.Button cmdOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private RadioButton optMove;
		private RadioButton optCopy;
		private Label lblQuestion;

		private FAskMoveOrCopyPerson()
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
			this.cmdOK = new System.Windows.Forms.Button();
			this.lblQuestion = new System.Windows.Forms.Label();
			this.optMove = new System.Windows.Forms.RadioButton();
			this.optCopy = new System.Windows.Forms.RadioButton();
			this.SuspendLayout();
			// 
			// cmdOK
			// 
			this.cmdOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point( 109, 131 );
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size( 80, 28 );
			this.cmdOK.TabIndex = 0;
			this.cmdOK.Text = "OK";
			// 
			// lblQuestion
			// 
			this.lblQuestion.AutoSize = true;
			this.lblQuestion.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.lblQuestion.Location = new System.Drawing.Point( 12, 9 );
			this.lblQuestion.Name = "lblQuestion";
			this.lblQuestion.Size = new System.Drawing.Size( 42, 13 );
			this.lblQuestion.TabIndex = 2;
			this.lblQuestion.Text = "Jag vill:";
			// 
			// optMove
			// 
			this.optMove.AutoSize = true;
			this.optMove.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.optMove.Checked = true;
			this.optMove.Location = new System.Drawing.Point( 34, 31 );
			this.optMove.Name = "optMove";
			this.optMove.Size = new System.Drawing.Size( 192, 17 );
			this.optMove.TabIndex = 3;
			this.optMove.TabStop = true;
			this.optMove.Text = "Flytta personen till den nya gruppen";
			this.optMove.UseVisualStyleBackColor = false;
			// 
			// optCopy
			// 
			this.optCopy.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.optCopy.Location = new System.Drawing.Point( 34, 54 );
			this.optCopy.Name = "optCopy";
			this.optCopy.Size = new System.Drawing.Size( 253, 61 );
			this.optCopy.TabIndex = 4;
			this.optCopy.Text = "Jag är HELT SÄKER på att jag kommer att fotografera personen i båda grupperna och" +
					" därför vill jag kopiera istället för att flytta";
			this.optCopy.UseVisualStyleBackColor = false;
			// 
			// FAskMoveOrCopyPerson
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.ClientSize = new System.Drawing.Size( 299, 171 );
			this.Controls.Add( this.optCopy );
			this.Controls.Add( this.optMove );
			this.Controls.Add( this.lblQuestion );
			this.Controls.Add( this.cmdOK );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FAskMoveOrCopyPerson";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Flytta eller Kopiera?";
			this.ResumeLayout( false );
			this.PerformLayout();

		}
		#endregion

		public static DialogResult showDialog(
			Form parent,
			out bool fWantsToMove )
		{
			using ( FAskMoveOrCopyPerson dlg = new FAskMoveOrCopyPerson() )
			{
				DialogResult retVal = dlg.ShowDialog( parent );
				fWantsToMove = dlg.optMove.Checked;
				return retVal;
			}
		}

	}

}
