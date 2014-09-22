using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using Xceed.Grid;

namespace Plata
{
	/// <summary>
	/// Summary description for FBackupLog.
	/// </summary>
	public class FBackupLog : vdUsr.baseGradientForm
	{
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Button cmdOK;
		private vdXceed.vdPlainGrid ug;

		private FBackupLog()
		{
			InitializeComponent();

			ug.G.ScrollBars = GridScrollBars.ForcedVertical;
			ug.addColumn( "Order", 70 );
			ug.addColumn( "Skola", 100 );
			ug.addColumn( "När", 120 ).SortDirection = SortDirection.Descending;
			ug.setColumnFullWidth( 1 );
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			ug.beginFillup();
			string strFN = Path.Combine( Global.Preferences.MainPath, "_backup.log" );
			if ( File.Exists(strFN) )
				using ( StreamReader sr = new StreamReader(strFN) )
					for ( string s=sr.ReadLine() ; s!=null ; s=sr.ReadLine() )
					{
						string[] astr = s.Split('\t');
						if ( astr.Length!=3 )
							continue;
						DataRow row = ug.addRow();
						row.Cells[0].Value = astr[0];
						row.Cells[1].Value = astr[1];
						row.Cells[2].Value = astr[2];
						row.EndEdit();
					}
			ug.endFillup();
			ug.fixSpringColumn( 1 );
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
			this.ug = new vdXceed.vdPlainGrid();
			this.cmdOK = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.ug)).BeginInit();
			this.SuspendLayout();
			// 
			// ug
			// 
			this.ug.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.ug.Caption = null;
			this.ug.Location = new System.Drawing.Point(8, 8);
			this.ug.Name = "ug";
			this.ug.ReadOnly = true;
			this.ug.Size = new System.Drawing.Size(528, 432);
			this.ug.TabIndex = 1;
			// 
			// cmdOK
			// 
			this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point(448, 448);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(80, 28);
			this.cmdOK.TabIndex = 2;
			this.cmdOK.Text = "Stäng";
			// 
			// FBackupLog
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cmdOK;
			this.ClientSize = new System.Drawing.Size(544, 480);
			this.Controls.Add(this.cmdOK);
			this.Controls.Add(this.ug);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FBackupLog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Backuphistorik";
			((System.ComponentModel.ISupportInitialize)(this.ug)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		public static DialogResult showDialog( Form parent )
		{
			using ( FBackupLog dlg = new FBackupLog() )
				return dlg.ShowDialog(parent);
		}

	}

}
