using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.IO;
using Xceed.Grid;

namespace Plata.OpenDialog
{
	/// <summary>
	/// Summary description for usrOpenPhotographers.
	/// </summary>
	public class usrOpenWork: baseUsrTab
	{

		private vdXceed.vdPlainGrid ug;
		private bool _fRestoreBackup;

		private usrOpenWork()
		{
			InitializeComponent();
		}

		public usrOpenWork( bool fRestoreBackup )
		{
			InitializeComponent();
			_fRestoreBackup = fRestoreBackup;
			Text = _fRestoreBackup ? "Återställ backup" : "Befintlig order";

			ug.addColumn( "", 20 );
			ug.addColumn( "Skola, ort", 200 );
			ug.addColumn( "Ordernr", 60 );
			ug.addColumn( "Skapad", typeof( DateTime ), 70 ).FormatSpecifier = "yyyy-MM-dd";
			Column c = ug.addColumn( "Ändrad", typeof( DateTime ), 70 );
			c.FormatSpecifier = "yyyy-MM-dd";
			c.SortDirection = SortDirection.Descending;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			ug.subscribeToEvents();

			try
			{
				string[] astrDirs = Directory.GetDirectories(
					_fRestoreBackup ?
						Global.Preferences.BackupFolder :
						Global.Preferences.MainPath );
				if ( astrDirs.Length > 200 )
					Global.showMsgBox( this, "Din arbetsmapp är felaktigt inställd!" );

				ug.beginCleanFillup();
				foreach ( string strDir in astrDirs )
					if ( !Path.GetFileName( strDir ).StartsWith( "_" ) )
						addLV( strDir );
				ug.endFillup();
			}
			catch ( Exception ex )
			{
				Global.showMsgBox( this, ex.ToString() );
			}
		}

		public override void activate()
		{
			base.activate ();
			ug.Select();
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
			this.ug = new vdXceed.vdPlainGrid();
			((System.ComponentModel.ISupportInitialize)(this.ug)).BeginInit();
			this.SuspendLayout();
			// 
			// ug
			// 
			this.ug.Caption = null;
			this.ug.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ug.Location = new System.Drawing.Point( 0, 0 );
			this.ug.Name = "ug";
			this.ug.ReadOnly = true;
			this.ug.Size = new System.Drawing.Size( 600, 200 );
			this.ug.TabIndex = 9;
			this.ug.SelectedRowsChanged += new System.EventHandler( this.ug_SelectedRowsChanged );
			this.ug.CellDoubleClick += new System.EventHandler( this.ug_CellDoubleClick );
			// 
			// usrOpenWork
			// 
			this.Controls.Add( this.ug );
			this.Name = "usrOpenWork";
			((System.ComponentModel.ISupportInitialize)(this.ug)).EndInit();
			this.ResumeLayout( false );

		}
		#endregion

		public override string selectedPath
		{
			get
			{
				string s = ug.selectedRowTag as string;
				if (string.IsNullOrEmpty(s))
					throw new Exception("selectedRowTag is empty!");
				return s;
			}
		}

		public override bool openOrder( PlataDM.Skola skola )
		{
			if ( _fRestoreBackup )
			{
				string strSrc = ug.selectedRowTag as string;
				string strDst = Path.Combine( Global.Preferences.MainPath, Path.GetFileName( strSrc ) );
				if ( FBackup.showDialog(
					this.FindForm(),
					true,
					string.Format( "Återställ {0}", ug.selectedDataRow.Cells[1].Value ),
					strSrc,
					strDst ) != DialogResult.OK )
					return false;
				skola.Open( strDst );
				return true;
			}
			else
			{
				skola.Open( selectedPath );
				if ( _fImport )
					return true;
				Directory.CreateDirectory( Path.Combine( selectedPath, "cache" ) );
				if ( skola.Verifierad || File.Exists( Path.Combine( skola.HomePath, "zzlast" ) ) )
					if ( Form.ModifierKeys != (Keys.Control | Keys.Shift) )
					{
						skola.ReadOnly = true;
						Global.showMsgBox( this, "Eftersom denna order är färdigställd så öppnas den i skrivskyddat läge!" );
					}
				return true;
			}
		}

		public override bool gotKeyDown(KeyEventArgs e)
		{
			if ( _fRestoreBackup )
				return false;

			if ( e.KeyCode==Keys.Delete && e.Modifiers==Keys.Shift )
			{
				if ( ug.G.SelectedRows.Count!=1 )
					return true;
				DataRow row = ug.selectedDataRow;
				if ( MessageBox.Show( this, "Du vill radera skolan \"" + row.Cells[1].Value + "\" som senast öppnades " +
					vdUsr.DateHelper.YYYYMMDDHHMM((DateTime)row.Cells[4].Value) + ".\r\n\r\nOm du raderar den finns det INGET sätt att återskapa den på. " +
					"Kontrollera först med Photomic att det är OK att radera den!!!", "Bekräfta radering",
					MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2 ) != DialogResult.OK )
					return true;
				if ( Global.askMsgBox( this, "Är du SÄKER på att du vill radera \"" + row.Cells[1].Value + "\"?", true ) != DialogResult.Yes )
					return true;
				try 
				{
					Directory.Delete( row.Tag as string, true );
					ug.G.DataRows.Remove( row );
				}
				catch ( Exception ex )
				{
					//Global.showMsgBox( this, ex.Message );
				}
				return true;
			}

			return false;
		}

		private void addLV( string strDir )
		{
			try 
			{
				string strFN = System.IO.Path.Combine( strDir, "!order.info" );
				if ( !File.Exists( strFN ) )
					return;

				vdUsr.vdSimpleXMLReader x = new vdUsr.vdSimpleXMLReader();
				x.loadFile( strFN );
				x.descend( "INFO" );

				DateTime dateSkapad, dateÄndrad;
				dateSkapad = DateTime.Parse( x.getValueAsString( "skapad" ), DateTimeFormatInfo.InvariantInfo );
				dateÄndrad = DateTime.Parse( x.getValueAsString( "andrad" ), DateTimeFormatInfo.InvariantInfo );

				DataRow row = ug.addRow();
				bool fCompletelyUploaded = File.Exists( System.IO.Path.Combine( strDir, "zzlast" ) );
				if ( x.getValueAsBool( "verifierad" ) || fCompletelyUploaded )
				{
					row.Cells[0].BackgroundImage = Images.bmp( Images.Img.Padlock );
					row.Cells[0].BackgroundImageAlignment = ContentAlignment.MiddleCenter;
				}
				row.Cells[1].Value = string.Format( "{0}, {1}", x.getValueAsString( "namn" ), x.getValueAsString( "ort" ) );
				row.Cells[2].Value = x.getValueAsString( "ordernr" );
				row.Cells[3].Value = dateSkapad;
				row.Cells[4].Value = dateÄndrad;
				row.Tag = strDir;
				row.EndEdit();

				if ( _fRestoreBackup || fCompletelyUploaded )
					return;

				DateTime dateBackup;
				string strBW = x.getValueAsString( "backupwhen" );
				if ( !string.IsNullOrEmpty( strBW ) )
					dateBackup = DateTime.Parse( strBW, DateTimeFormatInfo.InvariantInfo );
				else
					dateBackup = DateTime.MinValue;

				if ( (dateÄndrad != dateBackup) && (Global.Now - dateÄndrad).Days > 5 )
					row.BackColor = Color.Red;
			}
			catch ( Exception ex ) 
			{
				//Global.showMsgBox( this, "addLV:\r\n{0}", ex.ToString() );
			}
		}

		private void ug_SelectedRowsChanged( object sender, EventArgs e )
		{
			fireSelectionChanged();
		}

		private void ug_CellDoubleClick( object sender, EventArgs e )
		{
			fireExecute();
		}

	}

}
