using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace Plata
{

	public class FHistorik : System.Windows.Forms.Form
	{
		private System.Windows.Forms.RichTextBox rtf;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FHistorik()
		{
			InitializeComponent();

			try
			{
				rtf.Rtf = File.ReadAllText( Path.Combine(
					Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().Location ),
					"verhist.rtf" ) );
			}
			catch ( Exception ex )
			{
				MessageBox.Show( ex.ToString() );
			} 

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
			this.rtf = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// rtf
			// 
			this.rtf.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtf.Location = new System.Drawing.Point( 0, 0 );
			this.rtf.Name = "rtf";
			this.rtf.ReadOnly = true;
			this.rtf.Size = new System.Drawing.Size( 448, 470 );
			this.rtf.TabIndex = 0;
			this.rtf.Text = "";
			// 
			// FTangentbord
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.ClientSize = new System.Drawing.Size( 448, 470 );
			this.Controls.Add( this.rtf );
			this.KeyPreview = true;
			this.MinimizeBox = false;
			this.Name = "FTangentbord";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Versionshistorik";
			this.ResumeLayout( false );

		}
		#endregion

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown (e);
			if ( e.KeyCode==Keys.Escape || e.KeyCode==Keys.Enter )
			{
				e.Handled = true;
				this.Close();
			}
		}

	}

}
