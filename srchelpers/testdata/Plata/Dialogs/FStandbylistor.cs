using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using Xceed.Grid;

namespace Plata
{

	public class FStandbylistor : vdUsr.baseGradientForm
	{
		private System.ComponentModel.IContainer components;
		private ListBox lst;
		private Button cmdVisa;
		private System.Windows.Forms.Button cmdStäng;

		private FStandbylistor()
		{
			InitializeComponent();

			foreach ( string s in Directory.GetFiles( Global.GetTempPath(), "standbylista_*.pdf" ) )
				lst.Items.Insert( 0, s );
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
			this.cmdStäng = new System.Windows.Forms.Button();
			this.lst = new System.Windows.Forms.ListBox();
			this.cmdVisa = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cmdStäng
			// 
			this.cmdStäng.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdStäng.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdStäng.Location = new System.Drawing.Point( 284, 46 );
			this.cmdStäng.Name = "cmdStäng";
			this.cmdStäng.Size = new System.Drawing.Size( 80, 28 );
			this.cmdStäng.TabIndex = 2;
			this.cmdStäng.Text = "Stäng";
			// 
			// lst
			// 
			this.lst.FormattingEnabled = true;
			this.lst.Location = new System.Drawing.Point( 12, 12 );
			this.lst.Name = "lst";
			this.lst.Size = new System.Drawing.Size( 266, 446 );
			this.lst.TabIndex = 0;
			this.lst.DoubleClick += new System.EventHandler( this.lst_DoubleClick );
			this.lst.Format += new System.Windows.Forms.ListControlConvertEventHandler( this.lst_Format );
			// 
			// cmdVisa
			// 
			this.cmdVisa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdVisa.Location = new System.Drawing.Point( 284, 12 );
			this.cmdVisa.Name = "cmdVisa";
			this.cmdVisa.Size = new System.Drawing.Size( 80, 28 );
			this.cmdVisa.TabIndex = 1;
			this.cmdVisa.Text = "Visa";
			this.cmdVisa.Click += new System.EventHandler( this.cmdVisa_Click );
			// 
			// FStandbylistor
			// 
			this.AcceptButton = this.cmdVisa;
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.CancelButton = this.cmdStäng;
			this.ClientSize = new System.Drawing.Size( 376, 472 );
			this.Controls.Add( this.cmdVisa );
			this.Controls.Add( this.lst );
			this.Controls.Add( this.cmdStäng );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FStandbylistor";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Standbylistor";
			this.ResumeLayout( false );

		}
		#endregion

		public static DialogResult showDialog( Form parent )
		{
			using ( FStandbylistor dlg = new FStandbylistor() )
				return dlg.ShowDialog(parent);
		}

		private void lst_Format( object sender, ListControlConvertEventArgs e )
		{
			e.Value = Path.GetFileName( e.ListItem as string );
		}

		private void cmdVisa_Click( object sender, EventArgs e )
		{
			string s = lst.SelectedItem as string;
			if ( s!=null )
				try
				{
					System.Diagnostics.Process.Start( s );
					this.Close();
				}
				catch
				{
				}
		}

		private void lst_DoubleClick( object sender, EventArgs e )
		{
			cmdVisa.PerformClick();
		}

	}

}
