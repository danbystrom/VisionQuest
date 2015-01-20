using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Plata
{
	/// <summary>
	/// Summary description for frmErrorReport.
	/// </summary>
	public class frmErrorReport : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox txtFeedback;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtMessage;
		private string m_strFeedback;

		public frmErrorReport( string strErrorText )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			txtMessage.Text = "Hoppsan!\r\n\r\nDet blev visst lite fel nu. " +
				"Snälla, skriv en beskrivning av vad du gjorde när detta hände, så kommer felet tillsammans med " +
				"din beskrivning automatiskt att skickas med nästa jobb. " +
				"Vill du ha assistans innan dess, skriv av av de tre första raderna i felmeddelandet nedan och " +
				"vidarebefordra till Viron.\r\n\r\n" + strErrorText;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmErrorReport));
			this.txtMessage = new System.Windows.Forms.TextBox();
			this.txtFeedback = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txtMessage
			// 
			this.txtMessage.Location = new System.Drawing.Point(4, 4);
			this.txtMessage.Multiline = true;
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.ReadOnly = true;
			this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtMessage.Size = new System.Drawing.Size(548, 184);
			this.txtMessage.TabIndex = 0;
			this.txtMessage.TabStop = false;
			this.txtMessage.Text = "";
			// 
			// txtFeedback
			// 
			this.txtFeedback.Location = new System.Drawing.Point(4, 220);
			this.txtFeedback.Multiline = true;
			this.txtFeedback.Name = "txtFeedback";
			this.txtFeedback.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtFeedback.Size = new System.Drawing.Size(548, 104);
			this.txtFeedback.TabIndex = 1;
			this.txtFeedback.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 201);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(212, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Berätta vad du gjorde när felet inträffade:";
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.Abort;
			this.button1.Location = new System.Drawing.Point(124, 368);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(144, 28);
			this.button1.TabIndex = 3;
			this.button1.Text = "Avsluta PLÅTA";
			// 
			// button2
			// 
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Ignore;
			this.button2.Location = new System.Drawing.Point(292, 368);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(144, 28);
			this.button2.TabIndex = 4;
			this.button2.Text = "Fortsätt";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(70, 332);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(420, 32);
			this.label2.TabIndex = 5;
			this.label2.Text = "Om du vågar kan du försöka fortsätta arbeta med PLÅTA, men vi rekommenderar att d" +
				"u avslutar och startar om.";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// frmErrorReport
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(560, 402);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtFeedback);
			this.Controls.Add(this.txtMessage);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmErrorReport";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "PLÅTA - Programkörningsfel";
			this.ResumeLayout(false);

		}
		#endregion

		public string Feedback
		{
			get { return m_strFeedback; }
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			m_strFeedback = txtFeedback.Text;
			base.OnClosing (e);
		}

	}

}
