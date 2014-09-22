using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace Plata.OpenDialog
{
	/// <summary>
	/// Summary description for usrOpenPhotographers.
	/// </summary>
	public class usrOpenRecovery : baseUsrTab
	{
		private System.Windows.Forms.ListView lvWorks;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;

		public usrOpenRecovery()
		{
			InitializeComponent();
			Text = "Rädda jobb";
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lvWorks = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// lvWorks
			// 
			this.lvWorks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																																							this.columnHeader1,
																																							this.columnHeader2});
			this.lvWorks.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvWorks.FullRowSelect = true;
			this.lvWorks.HideSelection = false;
			this.lvWorks.Location = new System.Drawing.Point(0, 0);
			this.lvWorks.MultiSelect = false;
			this.lvWorks.Name = "lvWorks";
			this.lvWorks.Size = new System.Drawing.Size(600, 200);
			this.lvWorks.TabIndex = 0;
			this.lvWorks.View = System.Windows.Forms.View.Details;
			this.lvWorks.DoubleClick += new System.EventHandler(this.lvWorks_DoubleClick);
			this.lvWorks.SelectedIndexChanged += new System.EventHandler(this.lvWorks_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Order";
			this.columnHeader1.Width = 256;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Status";
			this.columnHeader2.Width = 200;
			// 
			// usrOpenRecovery
			// 
			this.Controls.Add(this.lvWorks);
			this.Name = "usrOpenRecovery";
			this.ResumeLayout(false);

		}
		#endregion


		public override void activate()
		{
			base.activate ();

			if ( lvWorks.Items.Count!=0 )
				return;

			try
			{
				lvWorks.Items.Clear();
				foreach ( string strDir in Directory.GetDirectories( Global.Preferences.MainPath ) )
				{
					string strName = Path.GetFileName(strDir);
					if ( !strName.StartsWith("_") )
						investigate( strDir );
				}
			}
			catch ( Exception ex )
			{
				Global.showMsgBox( this.FindForm(), ex.ToString() );
			}

		}

		public override bool openOrder(PlataDM.Skola skola)
		{
			if ( lvWorks.SelectedItems.Count!=1 )
				return false;
			RecoveryInfo ri = lvWorks.SelectedItems[0].Tag as RecoveryInfo;
			if ( ri==null )
				return false;

			ArrayList alPossibleFiles = new ArrayList();
			if ( ri.fHasWorkingXml )
				alPossibleFiles.Add( new SortableFileInfo(ri.strXml) );
			if ( ri.fHasWorkingBk1 )
				alPossibleFiles.Add( new SortableFileInfo(ri.strBk1) );
			if ( ri.fHasWorkingBk2 )
				alPossibleFiles.Add( new SortableFileInfo(ri.strBk2) );

			alPossibleFiles.Sort();

			foreach ( SortableFileInfo sfi in alPossibleFiles )
				try
				{
					skola.Open( Path.GetDirectoryName(sfi.FileName), sfi.FileName );
					if ( skola.Namn.Length!=0 )
					{
						skola.save(null);
						return true;
					}
				}
				catch ( Exception ex )
				{
					Global.showMsgBox( this, ex.ToString() );
				}
			return false;
		}

		public override bool isOK
		{
			get
			{
				if ( lvWorks.SelectedItems.Count!=1 )
					return false;
				RecoveryInfo ri = lvWorks.SelectedItems[0].Tag as RecoveryInfo;
				if ( ri==null )
					return false;
				return ri.fHasWorkingXml | ri.fHasWorkingBk1 | ri.fHasWorkingBk2;
			}
		}

		private void investigate( string strPath )
		{
			string strStatus = "";
			RecoveryInfo ri = new RecoveryInfo();
			
			try
			{
				ri.gatherInfo( strPath );

				if ( !ri.fHasWorkingInf )
					strStatus += ", " + (ri.fHasInf ? "Skadad" : "Saknar") + " INF";
				if ( !ri.fHasWorkingXml )
					strStatus += ", " + (ri.fHasXml ? "Skadad" : "Saknar") + " SKOLA";
				if ( !ri.fHasWorkingBk1 )
					strStatus += ", " + (ri.fHasBk1 ? "Skadad" : "Saknar") + " BAK1";
				if ( !ri.fHasWorkingBk2 )
					strStatus += ", " + (ri.fHasBk2 ? "Skadad" : "Saknar") + " BAK2";

				if ( strStatus.Length==0 )
					strStatus = "OK";
				else
					strStatus = strStatus.Substring(2);
			}
			catch ( Exception ex )
			{
				strStatus = ex.Message;
			}

			ListViewItem lvi = lvWorks.Items.Add( Path.GetFileName(strPath) );
			lvi.SubItems.Add( strStatus );
			lvi.Tag = ri;
		}

		private void lvWorks_DoubleClick(object sender, System.EventArgs e)
		{
			fireExecute();
		}

		private void lvWorks_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fireOK();
		}

		private class RecoveryInfo
		{
			public string strInf;
			public string strXml;
			public string strBk1;
			public string strBk2;
			public bool fHasInf;
			public bool fHasXml;
			public bool fHasBk1;
			public bool fHasBk2;
			public bool fHasWorkingInf;
			public bool fHasWorkingXml;
			public bool fHasWorkingBk1;
			public bool fHasWorkingBk2;

			public void gatherInfo( string strPath )
			{
				strInf = Path.Combine( strPath, "!order.info" );
				strXml = Path.Combine( strPath, "!order.plata" );
				strBk1 = Path.Combine( strPath, "!order.bk1" );
				strBk2 = Path.Combine( strPath, "!order.bk2" );
				fHasInf = File.Exists( strInf );
				fHasXml = File.Exists( strXml );
				fHasBk1 = File.Exists( strBk1 );
				fHasBk2 = File.Exists( strBk2 );
				fHasWorkingInf = fHasInf && canOpenXML( strInf, "INFO" );
				fHasWorkingXml = fHasXml && canOpenXML( strXml, "SKOLA" );
				fHasWorkingBk1 = fHasBk1 && canOpenXML( strBk1, "SKOLA" );
				fHasWorkingBk2 = fHasBk2 && canOpenXML( strBk2, "SKOLA" );
			}

			private bool canOpenXML( string strFN, string strMainNode )
			{
				try
				{
					XmlDocument doc = new XmlDocument();
					doc.Load( strFN );
					return doc.SelectSingleNode(strMainNode).Attributes["namn"].Value.Length!=0;
				}
				catch
				{
					return false;
				}
			}

		}

		private class SortableFileInfo : IComparable
		{
			public readonly string FileName;
			public readonly long FileLength;

			public SortableFileInfo( string strFN )
			{
				try
				{
					FileInfo fi = new FileInfo(strFN);
					FileName = strFN;
					FileLength = fi.Length;
				}
				catch
				{
				}
			}

			int IComparable.CompareTo(object obj)
			{
				return (int)(((SortableFileInfo)obj).FileLength - FileLength);
			}


		}

	}

}
