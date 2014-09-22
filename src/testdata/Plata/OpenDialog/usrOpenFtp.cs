using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using Xceed.Grid;
using System.Text.RegularExpressions;
using vdFtpFolderSync;

namespace Plata.OpenDialog
{
	/// <summary>
	/// Summary description for usrOpenPhotographers.
	/// </summary>
	public class usrOpenFtp : baseUsrTab
	{
		private enum DownloadStatus
		{
			Idle,
			InProgress,
			Complete,
			Failed
		}

		private vdFtp _sync;
		private TransferProgress _tp;
		private string _strFtgFolder;
		private vdXceed.vdPlainGrid ug;
		private TextBox txtProgress;

		private DownloadStatus _downloadStatus;

		private ContentOfOrderFile _coof;

		private Upgrader _upgrader = null;

		private readonly List<int> _existingOrders = new List<int>();

		public usrOpenFtp()
		{
			InitializeComponent();
			Text = "Från Internet";

			ug.G.ScrollBars = GridScrollBars.ForcedVertical;
			ug.addColumn( "Namn", 150 );
			ug.addColumn( "Ordernr", 80 );
			Column c = ug.addColumn( "Skapad", typeof(DateTime), 80 );
			c.SortDirection = SortDirection.Descending;
			c.FormatSpecifier = "yyyy-MM-dd";
		}

		protected override void OnLoad( EventArgs e )
		{
			base.OnLoad( e );

			foreach ( var s in Directory.GetDirectories( Global.Preferences.MainPath ) )
				if ( Regex.IsMatch( s, @"_\d{5,6}$" ))
				    _existingOrders.Add(int.Parse(s.Substring(s.LastIndexOf('_') + 1)));

			ug.setColumnFullWidth( 0 );
			ug.subscribeToEvents();

			_sync = new vdFtp(
				_tp = callback,
				"ftp://ftp.photomic.com/",
				Global.Preferences.Brand == Brand.Photomic ? "plata" : "kngplata",
				"h6d9b5" );
			_strFtgFolder = string.Format( "ftg{0:000}", Global.Preferences.Fotografnummer );
			var w = _sync.getGetFolders();
			w.addFolder( _strFtgFolder );
			w.addFolder( "_uppdateringar" );
			w.addFolder( _strFtgFolder + "/standbylistor" );
			w.run();
			setMessage( "Hämtar data från Viron...\r\n" );
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
			this.txtProgress = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.ug)).BeginInit();
			this.SuspendLayout();
			// 
			// ug
			// 
			this.ug.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.ug.Caption = null;
			this.ug.Location = new System.Drawing.Point( 3, 3 );
			this.ug.Name = "ug";
			this.ug.ReadOnly = true;
			this.ug.Size = new System.Drawing.Size( 594, 197 );
			this.ug.TabIndex = 1;
			this.ug.SelectedRowsChanged += new System.EventHandler( this.ug_SelectedRowsChanged );
			this.ug.CellDoubleClick += new System.EventHandler( this.ug_CellDoubleClick );
			// 
			// txtProgress
			// 
			this.txtProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtProgress.Location = new System.Drawing.Point( 3, 3 );
			this.txtProgress.Multiline = true;
			this.txtProgress.Name = "txtProgress";
			this.txtProgress.Size = new System.Drawing.Size( 594, 194 );
			this.txtProgress.TabIndex = 2;
			// 
			// usrOpenFtp
			// 
			this.Controls.Add( this.txtProgress );
			this.Controls.Add( this.ug );
			this.Name = "usrOpenFtp";
			((System.ComponentModel.ISupportInitialize)(this.ug)).EndInit();
			this.ResumeLayout( false );
			this.PerformLayout();

		}
		#endregion

		private void setMessage( string s )
		{
			if (this.IsDisposed)
				return;
			if ( s == null )
			{
				ug.Visible = true;
				txtProgress.Visible = false;
				txtProgress.Clear();
			}
			else
			{
				txtProgress.AppendText( s );
				txtProgress.Visible = true;
				ug.Visible = false;
			}
		}

		private void photographerFolderComplete( vdFtpFolder folder )
		{
			ug.beginCleanFillup();
			foreach ( var itm in folder.Items )
			{
				var astr = System.Text.RegularExpressions.Regex.Split( itm.Name, @"^order_(\d{5,6})_(.+)\.zip$" );
				if ( astr.Length != 4 )
					continue;
				if ( !_fImport && _existingOrders.Contains( int.Parse( astr[1] ) ) )
					continue;
				var row = ug.addRow();
				row.Cells[0].Value = astr[2];
				row.Cells[1].Value = astr[1];
				row.Cells[2].Value = itm.Date;
				row.Tag = itm;
				row.EndEdit();
			}
			ug.endFillup();
			setMessage( null );
		}

		private void upgradeFolderComplete( vdFtpFolder folder )
		{
			if ( _downloadStatus==DownloadStatus.InProgress )
				return;
            var strBestVersion = AppSpecifics.Version;
			vdFtpItem itmFound = null;

			foreach ( var itm in folder.Items )
				if ( itm.Name.StartsWith( "plata_" ) )
				{
					var strThatVersion = string.Format( "{0}-{1}-{2}",
						itm.Name.Substring( 6, 4 ),
						itm.Name.Substring( 10, 2 ),
						itm.Name.Substring( 12, 2 ) );
					if ( strThatVersion.CompareTo( strBestVersion ) > 0 )
					{
						strBestVersion = strThatVersion;
						itmFound = itm;
					}
				}
			if ( itmFound != null )
			{
				var strQ = string.Format( "Det finns en ny version av Plåta att hämta. Vill du göra det nu?" );
				if ( Global.askMsgBox( this, strQ, false ) != DialogResult.Yes )
					return;
				if ( download( "_uppdateringar", itmFound, 120 ) )
					if ( _upgrader!=null )
						if ( _upgrader.run() )
						{
							FMain.theOneForm.endApp( false );
							return;
						}
				setMessage( string.Format( "Överföringsfel ({0})", _upgrader!=null ) );
			}
		}

		private void standbylistFolderComplete( vdFtpFolder folder )
		{
			vdFtpWorkUnit w = null;
			vdFtpFolder f = null;

			var strTemp = Global.GetTempPath();

			foreach ( var itm in folder.Items )
			{
				if ( !itm.Name.EndsWith( ".pdf" ) )
					continue;
				if ( File.Exists( Path.Combine( strTemp, itm.Name ) ) )
					continue;

				if ( w == null )
				{
					w = _sync.getDownloadFiles();
					f = w.addFolder( folder.Name );
				}
				f.AddItem( itm );
			}
			if ( w!=null )
				w.run();
		}

		private void callback( 
			vdFtpWorkUnit unit,
			vdFtpTPStatus status, 
			vdFtpFolder folder, 
			vdFtpItem item, 
			object data )
		{
			if ( this.InvokeRequired )
			{
				this.Invoke( _tp, unit, status, folder, item, data );
				return;
			}

			try
			{
				switch ( status )
				{
					case vdFtpTPStatus.GetFolder_Complete:
						if ( _strFtgFolder.CompareTo( folder.Name ) == 0 )
							photographerFolderComplete( folder );
						else if ( "_uppdateringar".CompareTo( folder.Name ) == 0 )
							upgradeFolderComplete( folder );
						else
							standbylistFolderComplete( folder );
						break;

					case vdFtpTPStatus.DownloadFile_Complete:
						if ( item.Name.StartsWith( "order_" ) )
						{
							_coof = new ContentOfOrderFile();
							_coof.loadFromZipFile( data as MemoryStream );
							_downloadStatus = _coof.getFileWithType( ContentOfOrderFile.FileType.OrderXml ) != null ?
								DownloadStatus.Complete : DownloadStatus.Failed;
						}
						else if ( item.Name.StartsWith( "plata_" ) )
						{
							_upgrader = new Upgrader( data as MemoryStream );
							_downloadStatus = DownloadStatus.Complete;
						}
						else if ( item.Name.EndsWith( ".pdf" ) )
						{
							string s =Path.Combine( Global.GetTempPath(), item.Name );
							using ( Stream stream = new FileStream( s, FileMode.Create ) )
								(data as MemoryStream).WriteTo( stream );
						}
						break;

					case vdFtpTPStatus.DownloadFile_Begin:
					case vdFtpTPStatus.DownloadFile_Progress:
						txtProgress.Text = string.Format( "Hämtar... {0}%", 100*(int)data/item.Size );
						break;

					case vdFtpTPStatus.Error:
						setMessage( data.ToString() );
						_downloadStatus = DownloadStatus.Failed;
						break;

				}
			}
			catch ( Exception ex )
			{
				setMessage( ex.ToString() );
				_downloadStatus = DownloadStatus.Failed;
			}
		}

		private void timeout( object sender, EventArgs e )
		{
			if ( _downloadStatus == DownloadStatus.InProgress )
			{
				setMessage( "Timeout\r\n" );
				((sender as vdUsr.vdOneShotTimer).Tag as vdFtpWorkUnit).Abort();
				_downloadStatus = DownloadStatus.Failed;
			}
		}

		private bool download(
			string strFolderName,
			vdFtpItem item, 
			int nTimeoutSec )
		{
			_downloadStatus = DownloadStatus.InProgress;
			var w = _sync.getDownloadFiles();
			w.addFolder( strFolderName ).AddItem( item );
			w.run();
			vdUsr.vdOneShotTimer.start( 1000*nTimeoutSec, new EventHandler( timeout ), w );

			setMessage( "Hämtar fil från Viron...\r\n" );
			while ( _downloadStatus == DownloadStatus.InProgress )
				Application.DoEvents();
			return _downloadStatus == DownloadStatus.Complete;
		}

		public override bool openOrder(PlataDM.Skola skola)
		{
			if ( _downloadStatus == DownloadStatus.InProgress )
				return false;

			if ( !download( _strFtgFolder, ug.selectedRowTag as vdFtpFolderSync.vdFtpItem, 15 ) )
				return false;

			return _fImport ?
				_coof.importOrder( skola ) :
				_coof.createOrder( skola );
		}

		public override bool isOK
		{
			get { return ug.selectedRowTag is vdFtpFolderSync.vdFtpItem; }
		}

		private void ug_SelectedRowsChanged( object sender, EventArgs e )
		{
			fireOK();
		}

		private void ug_CellDoubleClick( object sender, EventArgs e )
		{
			fireExecute();
		}

	}

}
