using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Plata
{

	public class FAskChangePersonStatus : vdUsr.baseGradientForm
	{
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private RadioButton optFrånvarande;
		private RadioButton optUtgått;
		private Label lblQuestion;

		private FAskChangePersonStatus()
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
			this.cmdCancel = new System.Windows.Forms.Button();
			this.lblQuestion = new System.Windows.Forms.Label();
			this.optFrånvarande = new System.Windows.Forms.RadioButton();
			this.optUtgått = new System.Windows.Forms.RadioButton();
			this.SuspendLayout();
			// 
			// cmdOK
			// 
			this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point( 45, 92 );
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size( 80, 28 );
			this.cmdOK.TabIndex = 0;
			this.cmdOK.Text = "OK";
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point( 145, 92 );
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size( 80, 28 );
			this.cmdCancel.TabIndex = 1;
			this.cmdCancel.Text = "Avbryt";
			// 
			// lblQuestion
			// 
			this.lblQuestion.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.lblQuestion.Location = new System.Drawing.Point( 12, 9 );
			this.lblQuestion.Name = "lblQuestion";
			this.lblQuestion.Size = new System.Drawing.Size( 345, 19 );
			this.lblQuestion.TabIndex = 2;
			this.lblQuestion.Text = "Markera onumrerade personer som:";
			// 
			// optFrånvarande
			// 
			this.optFrånvarande.AutoSize = true;
			this.optFrånvarande.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.optFrånvarande.Checked = true;
			this.optFrånvarande.Location = new System.Drawing.Point( 34, 31 );
			this.optFrånvarande.Name = "optFrånvarande";
			this.optFrånvarande.Size = new System.Drawing.Size( 85, 17 );
			this.optFrånvarande.TabIndex = 3;
			this.optFrånvarande.TabStop = true;
			this.optFrånvarande.Text = "Frånvarande";
			this.optFrånvarande.UseVisualStyleBackColor = false;
			// 
			// optUtgått
			// 
			this.optUtgått.AutoSize = true;
			this.optUtgått.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.optUtgått.Location = new System.Drawing.Point( 34, 54 );
			this.optUtgått.Name = "optUtgått";
			this.optUtgått.Size = new System.Drawing.Size( 54, 17 );
			this.optUtgått.TabIndex = 4;
			this.optUtgått.TabStop = true;
			this.optUtgått.Text = "Utgått";
			this.optUtgått.UseVisualStyleBackColor = false;
			// 
			// FAskChangePersonStatus
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size( 269, 132 );
			this.Controls.Add( this.optUtgått );
			this.Controls.Add( this.optFrånvarande );
			this.Controls.Add( this.lblQuestion );
			this.Controls.Add( this.cmdCancel );
			this.Controls.Add( this.cmdOK );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FAskChangePersonStatus";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Numrering klar";
			this.ResumeLayout( false );
			this.PerformLayout();

		}
		#endregion

		public static DialogResult showDialog(
			Form parent,
			out PlataDM.GruppPersonTyp gt )
		{
			using ( var dlg = new FAskChangePersonStatus() )
			{
				if ( dlg.ShowDialog(parent) != DialogResult.OK )
				{
				gt = PlataDM.GruppPersonTyp.PersonNormal;
					return DialogResult.Cancel;
				}
				gt = dlg.optFrånvarande.Checked ?
					PlataDM.GruppPersonTyp.PersonFrånvarande :
					PlataDM.GruppPersonTyp.PersonSlutat;
				return DialogResult.OK;
			}
		}

	}

}
