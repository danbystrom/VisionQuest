using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Photomic.Common;
using vdFtpFolderSync;
using vdUsr;
using vdXceed;
using Xceed.Grid;

namespace Plata
{
	public partial class FFTP : Plata.baseFlikForm
	{
		private vdFtp _sync;
		private TransferProgress _tp;

		private readonly Dictionary<string,InProgress> _dicInProgress = new Dictionary<string,InProgress>();

		private class InProgress
		{
			public string Path;
			public vdFtpFolder Folder;
			public DataRow Row;
			public vdFtpWorkUnit Work;
			public TimeEstimator TimeEstimator;
			public bool IsBackup;
			public int Retries;
		}

		public FFTP( Form parent )
			: base( parent, FlikTyp.FTP )
		{
			InitializeComponent();
			Bounds = parent.ClientRectangle;
			PerformLayout();
			_strCaption = "BACKUP";
			RightAligned = true;
			ButtonY = 10;

			initGrid( ugToDo, false );
			initGrid( ugDone, true );

			ugProgress.addColumn( "Order", 70 );
			ugProgress.addColumn( "Skola", 300 );
			ugProgress.addColumn( "Ort", 100 );
			ugProgress.addColumn( "Mb", typeof(int), 60 );
			ugProgress.addColumn( "%", typeof(double), 60 ).FormatSpecifier = "0.0";
			ugProgress.addColumn( "Start", typeof( DateTime ), 60 ).FormatSpecifier = "HH:mm";
			ugProgress.addColumn( "Klar ca", typeof( DateTime ), 60 ).FormatSpecifier = "HH:mm";
		}

		public override void paint_Flik( Graphics g, Region shape, Color color, Font font, int x )
		{
			base.paint_Flik( g, shape, Color.FromArgb( 128, 96, 96 ), font, x );
		}

		private void newSync()
		{
		    var ftg = Global.Preferences.Brand == Brand.Photomic
		                  ? "ftg"
		                  : "kngftg";
		    _sync = new vdFtp(
		        _tp = new TransferProgress(callback),
		        "ftp://ftp.viron.se",
		        string.Format("{0}{1:000}", ftg, Global.Preferences.Fotografnummer),
		        string.Format("{0:000}{1}", Global.Preferences.Fotografnummer, ftg));
		    //_sync.SimulateBadConnection = true;
		    //_sync.DelayTimeBetweenChunks = 20;
		}

	    protected override void OnLoad( EventArgs e )
		{
			base.OnLoad( e );
			newSync();
		}

		protected override void OnClosed( EventArgs e )
		{
			base.OnClosed( e );
			timer1.Stop();
			foreach ( var ip in _dicInProgress.Values )
				ip.Work.Abort();
		}

		public static void initGrid( vdPlainGrid ug, bool fDone )
		{
			ug.addColumn( "", typeof(bool), 30 );
			var c = ug.addColumn( "", typeof(int), 5 );
			c.Visible = false;
			c.SortDirection = SortDirection.Descending;
			if ( fDone )
				ug.addColumn( "När", 80 );
			else
				ug.addColumn( "Typ", 60 );
			ug.addColumn( "Order", 60 );
			ug.addColumn( "Skola", 300 );
			ug.addColumn( "Ort", 100 );
			ug.addColumn( "Skapad", 80 );
			ug.addColumn( "Ändrad", 80 );
			if ( !fDone )
				ug.addColumn( "Backup", typeof(DateTime), 80 ).FormatSpecifier = "yyyy-MM-dd";

			for ( var i = 0 ; i < ug.G.Columns.Count ; i++  )
				ug.G.Columns[i].ReadOnly = i != 0;
			ug.G.SingleClickEdit = true;
		}

		private void msg( string s, params object[] args )
		{
		    txtMsg.AppendText(args.Length != 0
                ? string.Format(s, args)
                : s);
		    txtMsg.AppendText( "\r\n" );
		}

	    private void callback(
			vdFtpWorkUnit unit,
			vdFtpTPStatus status,
			vdFtpFolder folder,
			vdFtpItem item,
			object data )
		{
			if ( InvokeRequired )
			{
				Invoke( _tp, unit, status, folder, item, data );
				return;
			}

			var ip = folder != null
                ? (InProgress)folder.Tag
                : null;

			try
			{
				switch ( status )
				{
					case vdFtpTPStatus.Sync_FolderInfo:
						{
							var row = ip.Row;
							row.Cells[3].Value = (int)(folder.TotalBytes / (1024 * 1024));
							row.Cells[4].Value = 100.0 * folder.AlreadySyncedBytes / folder.TotalBytes;
						}
						break;

					case vdFtpTPStatus.Sync_BeginFolder:
						{
							ip.TimeEstimator = new TimeEstimator( Global.Preferences.FTPSpeed * 1024 );
							ip.TimeEstimator.Start( folder.TotalBytes - folder.AlreadySyncedBytes );
							msg( "Påbörjar order {0}", folder.Name );
							ip.Row.Font = new Font( ip.Row.Font.FontFamily, ip.Row.Font.Size + 1, FontStyle.Bold );
                            ip.Row.Cells[5].Value = Global.Now;
						}
						break;

					case vdFtpTPStatus.Sync_BeginItem:
						//msg( "Begin \"{0}\", {1} bytes", item.Name, item.Size );
						break;

					case vdFtpTPStatus.Sync_Progress:
				        ip.TimeEstimator.MakeProgress((int) data);
							var row2 = (folder.Tag as InProgress).Row;
							row2.BeginEdit();
							row2.Cells[4].Value = 100.0 * (folder.TransferredBytes + folder.AlreadySyncedBytes) / folder.TotalBytes;
				        row2.Cells[6].Value = ip.TimeEstimator.EstimatedCompletionTime;
							row2.EndEdit();
						break;

					case vdFtpTPStatus.Sync_VerifiedFolder:
						{
							msg( "Order {0} klar, kl {1:HH:mm}", folder.Name, Global.Now );
							doneWithInProgress( ip, true );
							activated();
						}
						break;

					case vdFtpTPStatus.Sync_FolderVerificationFailed:
						{
							msg( "Order {0} misslyckades ({1}), försöker igen...", folder.Name, ip.Retries );
							doneWithInProgress( ip, false );
							if ( ip.Retries < 20 )
							{
								var w = _sync.getSync( true );
								go( w, ip.Path ).Retries = ip.Retries + 1;
								w.run();
							}
						}
						break;

					case vdFtpTPStatus.Error:
					case vdFtpTPStatus.Sync_NotComplete:
						{
							if ( data is string )
								msg( (string)data );
							var w = _sync.getSync( true );
							var stopThese = new List<InProgress>();
							foreach ( var ip2 in _dicInProgress.Values )
								if ( unit == ip2.Work )
									stopThese.Add( ip2 );
							foreach ( var ip2 in stopThese )
							{
								doneWithInProgress( ip2, false );
								if ( ip2.Retries<20 )
									go( w, ip2.Path ).Retries = ip2.Retries + 1;
							}
							w.run();
						}
						break;

					case vdFtpTPStatus.Sync_Complete:
						activated();
						timer1.Stop();
						break;

				}
			}
			catch ( Exception ex )
			{
				msg( ex.ToString() );
			}
		}

		private void doneWithInProgress( InProgress ip, bool fSuccess )
		{
			if ( ip.TimeEstimator!=null )
				Global.Preferences.FTPSpeed =
					(Global.Preferences.FTPSpeed + (int)(ip.TimeEstimator.UnitsPerSecond / 1024)) / 2;
			_dicInProgress.Remove( ip.Path );
			ugProgress.G.DataRows.Remove( ip.Row );

			if ( fSuccess )
			{
				PlataDM.Skola skola;
				if ( Global.Skola != null && string.Compare( ip.Path, Global.Skola.HomePath, true ) == 0 )
					skola = Global.Skola;
				else
				{
					skola = new PlataDM.Skola( null, 0 );
					skola.Open( ip.Path );
				}
				skola.BackupWhen = vdUsr.DateHelper.YYYYMMDDHHMM( Global.Now );
				skola.save( true );
			}
		}

		protected override void OnActivated( EventArgs e )
		{
			base.OnActivated (e);
			if ( this.WindowState!=FormWindowState.Minimized )
				try
				{
					resize( this.ClientSize );
					activated();
				}
				catch ( Exception ex )
				{
					Global.showMsgBox( this, ex.ToString() );
				}
		}

		public override void activated()
		{
			base.activated();

			var checkedJobs = new Dictionary<string,bool>();
			getChecked( ugToDo, checkedJobs );
			getChecked( ugDone, checkedJobs );

			ugToDo.beginCleanFillup();
			ugDone.beginCleanFillup();
			foreach ( var strPath in Directory.GetDirectories( Global.Preferences.MainPath ) )
				if ( !Path.GetFileName(strPath).StartsWith("_") )
					if ( !_dicInProgress.ContainsKey( strPath ) )
						addRowToGrid( strPath, checkedJobs );
			ugToDo.endFillup();
			ugDone.endFillup();
		}

		private void getChecked( vdPlainGrid ug, Dictionary<string,bool> jobs )
		{
			foreach ( DataRow row in ug.G.DataRows )
				jobs.Add( row.Tag as string, (bool)row.Cells[0].Value );
		}

		private void addRowToGrid( 
			string strPath, 
			Dictionary<string, bool> checkedJobs )
		{
			try
			{
				var strFN = Path.Combine( strPath, "!order.info" );
				if ( !File.Exists( strFN ) )
					return;
				var x = new vdSimpleXMLReader();
				x.loadFile( strFN );
				x.descend( "INFO" );

				var strBW = x.getValueAsString( "backupwhen" );
				var dateBackup = !string.IsNullOrEmpty(strBW)
                    ? DateTime.Parse( strBW, System.Globalization.DateTimeFormatInfo.InvariantInfo )
                    : DateTime.MinValue;

				var strZZLast = Path.Combine( strPath, "zzlast" );
				var fDone = File.Exists( strZZLast );
				var fVerifierad = x.getValueAsBool("verifierad");
				bool fChecked;
				if ( !checkedJobs.TryGetValue( strPath, out fChecked ) )
					if ( !fDone )
						fChecked = dateBackup.Date != Global.Now.Date;
					else
						fChecked = false;

				var ug = fDone ? ugDone : ugToDo;
				var row = ug.addRow();
				row.Cells[0].Value = fChecked;
				if ( fDone )
					row.Cells[2].Value = vdUsr.DateHelper.YYYYMMDD( File.GetCreationTime( strZZLast ) );
				else
				{
					row.Cells[2].Value = fVerifierad ? "Klar" : "backup";
					row.Cells[8].Value = dateBackup;
					row.Cells[1].Value = ug.G.DataRows.Count + (fVerifierad ? 100 : 0) + (dateBackup.Date==Global.Now.Date ? -50 : 0);
				}
				row.Cells[3].Value = x.getValueAsString( "ordernr" );
				row.Cells[4].Value = x.getValueAsString( "namn" );
				row.Cells[5].Value = x.getValueAsString( "ort" );
				row.Cells[6].Value = x.getValueAsString( "skapad" ).Split(' ')[0];
				row.Cells[7].Value = x.getValueAsString( "andrad" ).Split( ' ' )[0];
				row.Tag = strPath;
				row.EndEdit();
			}
			catch ( Exception ex )
			{
				int i = 0;
			}
		}

		public bool BeginUpload( PlataDM.Skola skola )
		{
			if ( _dicInProgress.ContainsKey( skola.HomePath ) )
				return false;

			DataRow rowFound = null;
			foreach ( DataRow row in ugToDo.G.DataRows )
				if ( (string)row.Tag == skola.HomePath )
					rowFound = row;

			if (rowFound == null)
				return false;

			ugToDo.G.DataRows.Remove( rowFound );
			var w = _sync.getSync( true );
			go( w, skola.HomePath );
			w.run();
			timer1.Start();

			return true;
		}

		private List<string> findFilesToExclude( PlataDM.Skola skola )
		{
			var result = new List<string>();
			foreach ( var grupp in skola.Grupper )
			{
				var possibleExclude = new List<string>();
				var nReserver = 0;
				foreach ( PlataDM.Thumbnail tn in grupp.Thumbnails )
				{
					if ( tn.Key.Equals( grupp.ThumbnailKey ) )
						continue;
					if ( tn.Key.Equals( grupp.ThumbnailGrayKey ) )
						continue;
					if ( tn.Reserv )
						nReserver++;
					else
						possibleExclude.Add( Path.GetFileName( tn.FilenameRaw ).ToLower() );
				}
				if ( nReserver >= 1 )
					result.AddRange( possibleExclude );
			}
			return result;
		}

		private InProgress go( vdFtpWorkUnit w, string strPath )
		{
			var skola = new PlataDM.Skola( null, 0 );
			skola.Open( strPath );
			var f = w.addFolder( skola.OrderNr );

			var excludeThese = findFilesToExclude( skola );
			foreach ( var strFullFN in Directory.GetFiles( skola.HomePath ) )
			{
				var plainFileName = Path.GetFileName( strFullFN ).ToLower();
				switch ( plainFileName )
				{
					case "!fotoorder.emf":
						break;
					case "!order.plata":
						if ( skola.Verifierad )
							goto default;
						f.AddLocalItem(
							strFullFN,
							string.Format( "!order_{0:yyyyMMddHHmmss}.plata", Global.Now ) );
						break;
					default:
						if ( !excludeThese.Contains( plainFileName ) )
							f.AddLocalItem( strFullFN, plainFileName );
						break;
				}
			}

			var strZZLast = Path.Combine( skola.HomePath, "zzlast" );
			Util.safeKillFile( strZZLast );
			if ( skola.Verifierad )
				f.AddLocalItem(
					strZZLast,
					"zzlast" ).FileType = vdFtpFileType.EndFileForOneFolder;

			var row = ugProgress.addRow();
			row.Cells[0].Value = skola.OrderNr;
			row.Cells[1].Value = skola.Namn;
			row.Cells[2].Value = skola.Ort;
			row.EndEdit();

			var ip = new InProgress();
			f.Tag = ip;
			ip.Path = strPath;
			ip.Folder = f;
			ip.Row = row;
			ip.Work = w;
			ip.IsBackup = !skola.Verifierad;
			row.Tag = ip;
			_dicInProgress.Add( strPath, ip );
			return ip;
		}

		private void go( vdFtpWorkUnit w, vdPlainGrid ug )
		{
			var rows = new List<DataRow>();
			foreach ( DataRow row in ug.G.GetSortedDataRows( true ) )
				rows.Add( row );
			foreach ( var row in rows )
				if ( (bool)row.Cells[0].Value )
				{
					go( w, row.Tag as string );
					ug.G.DataRows.Remove( row );
				}
		}

		private void cmdGo_Click( object sender, EventArgs e )
		{
			var w = _sync.getSync( true );
			go( w, ugToDo );
			go( w, ugDone );
			w.run();
			timer1.Start();
		}

		private void cmdStop_Click( object sender, EventArgs e )
		{
			msg( "Stoppar..." );			
			timer1.Stop();
			foreach ( var ip in _dicInProgress.Values )
				ip.Work.Abort();
			_dicInProgress.Clear();
			ugProgress.G.DataRows.Clear();
			newSync();
			activated();
			msg( "Stoppad" );
		}

		private void timer1_Tick( object sender, EventArgs e )
		{
			if ( ugProgress.G.DataRows.Count < 2 )
				return;

			try
			{
				var te = (ugProgress.G.DataRows[0].Tag as InProgress).TimeEstimator;
				if ( te==null )
					return;
				var d = te.EstimatedCompletionTime;
				for ( var i = 1 ; i < ugProgress.G.DataRows.Count ; i++ )
				{
					var row = ugProgress.G.DataRows[i];
					var ip = row.Tag as InProgress;
					if ( ip.TimeEstimator == null )
					{
						row.Cells[5].Value = d;
						d = d.AddSeconds( (ip.Folder.TotalBytes - ip.Folder.AlreadySyncedBytes) / te.UnitsPerSecond );
						row.Cells[6].Value = d;
					}
				}
			}
			catch
			{
			}
		}

		private void rowSwap( int nDirection )
		{
			var list = new List<DataRow>();
			foreach ( DataRow row in ugToDo.G.GetSortedDataRows( false ) )
				list.Add( row );
			var nIndex1 = list.IndexOf( ugToDo.selectedDataRow );
			var nIndex2 = nIndex1 + nDirection;
			if ( nIndex1 < 0 || nIndex2 < 0 || nIndex2 >= list.Count )
				return;
			var nS = (int)list[nIndex1].Cells[1].Value;
			list[nIndex1].Cells[1].Value = (int)list[nIndex2].Cells[1].Value;
			list[nIndex2].Cells[1].Value = nS;
		}

		private void cmdUp_Click( object sender, EventArgs e )
		{
			rowSwap( -1 );
		}

		private void cmdDown_Click( object sender, EventArgs e )
		{
			rowSwap( 1 );
		}

		private void cmdAll_Click( object sender, EventArgs e )
		{
			var nChecked = 0;
			foreach ( DataRow row in ugToDo.DataRows )
				if ( (bool)row.Cells[0].Value )
					nChecked++;
			var fCheck = nChecked != ugToDo.DataRows.Count;
			foreach ( DataRow row in ugToDo.DataRows )
				row.Cells[0].Value = fCheck;
		}

	}


}