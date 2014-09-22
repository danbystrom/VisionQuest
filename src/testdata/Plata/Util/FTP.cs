using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading;

namespace PlataSkicka
{
	public delegate void TransferProgress(
		TPStatus tpStatus,
		int nPercent,
		string strData1,
		string strData2 );

	public enum TPStatus
	{
		Progress,
		VerifiedFolder,
		Complete,
		Error,
	}

	public enum ComparisonResult
	{
		Identical,
		MissingInFolder,
		SizeDifferent
	}

	class FTP
	{
		private bool LOCALHOST = false;

		private Thread _thread;
		private TransferProgress _tp;
//		private System.Windows.Forms.Control _controlSync;

		public string ServerName = "ftp://127.0.0.1/";
		public string Account = "anonymous";
		public string Password = "z@z.se";
		private NetworkCredential _networkCredential;

		private Folder _folder2Transfer;
		private Folder _folderServerRoot = null;

		static private Regex _rexSpaces = new Regex(" +");

		private Random _rnd = new Random(DateTime.Now.Millisecond);

		public volatile int DelayTimeBetweenFiles = 0;

		public long TotalJobSize;
		public long TransferredBytes;
		private bool _fCurrentJobIsBackup;

		private volatile bool _fPause;

		public FTP(
//			System.Windows.Forms.Control controlSync,
			TransferProgress tp )
		{
//			_controlSync = controlSync;
			_tp = tp;
		}

		public string run(
			string strLocalFolder,
			string strRemoteFolder,
			bool fBackup,
			IList listExcludeTheseFiles )
		{
			TransferredBytes = TotalJobSize = 0;
			_folder2Transfer = new Folder( strRemoteFolder );
			
			_fCurrentJobIsBackup = fBackup;
			foreach ( string strFN in Directory.GetFiles( strLocalFolder ) )
			{
				string strRemoteName = Path.GetFileName( strFN );
				if ( listExcludeTheseFiles != null )
					if ( listExcludeTheseFiles.Contains( strRemoteName ) )
						continue;
				if ( _fCurrentJobIsBackup && string.Compare( strRemoteName, "!order.plata", true ) == 0 )
					strRemoteName = "!order.plata_backup";
				TotalJobSize += _folder2Transfer.AddLocalItem( strFN, strRemoteName ).Size;
			}

			_thread = new Thread(new ThreadStart(threadProc));
			_thread.Start();

			return null;
		}

		public void pause( bool fPause )
		{
			_fPause = fPause;
		}

		public bool isRunning
		{
			get { return _thread!=null && _thread.IsAlive; }
		}

		public void Abort()
		{
			if ( isRunning )
				_thread.Abort();
		}

		private void callback(
			TPStatus status,
			int nPercent,
			string str1,
			string str2 )
		{
			/*
			if ( !_controlSync.IsDisposed && _controlSync.IsHandleCreated )
				try
				{
					_controlSync.Invoke(
						_tp,
						status,
						nPercent,
						str1,
						str2 );
				}
				catch
				{
				}
			 * */
			_tp(
				status,
				nPercent,
				str1,
				str2 );

		}

		private void threadProc()
		{
			int nItems = 0;
			string strWhere = "";

			_networkCredential = new NetworkCredential( Account, Password );

			try
			{
				strWhere = "Get remote root folder";
				string strDummy;
				_folderServerRoot = GetRemoteFolder( "", true, out strDummy );
				strWhere = string.Format( "Create folder \"{0}\"", _folder2Transfer.Name );
				createFolderOnServer( _folder2Transfer.Name );
				strWhere = string.Format( "Get remote folder for transfer \"{0}\"", _folder2Transfer.Name );
				Folder folderRemote = GetRemoteFolder( _folder2Transfer.Name, true, out strDummy );
				foreach ( Item item in _folder2Transfer.Items )
				{
					callback(
						TPStatus.Progress,
						(nItems * 100) / _folder2Transfer.Items.Count,
						_folder2Transfer.Name,
						item.Name );
					switch ( folderRemote.hasItem( item ) )
					{
						case ComparisonResult.MissingInFolder:
						case ComparisonResult.SizeDifferent:
							strWhere = string.Format( "Uploading \"{0}\" to \"{1}\"", item.FullName, _folder2Transfer.Name + "/" + item.Name );
							UploadItem( item, _folder2Transfer.Name + "/" + item.Name, true );
							break;
						default:
							System.Diagnostics.Debug.WriteLine( "Already existing: " + item.Name);
							break;
					}
					nItems++;
					TransferredBytes += item.Size;

					while ( _fPause )
						Thread.Sleep( 500 );
				}

				//alla mapparna överförda - först nu verifierar vi dem alla på en gång
				strWhere = string.Format( "Get remote folder for verification \"{0}\"", _folder2Transfer.Name );
				string strServerResponse = string.Empty;
				for ( int i = 0 ; i < 3 ; i++ )
				{
					folderRemote = GetRemoteFolder( _folder2Transfer.Name, false, out strServerResponse );
					if ( !string.IsNullOrEmpty( strServerResponse ) )
						break;
				}
				foreach ( Item item in _folder2Transfer.Items )
					if ( folderRemote.hasItem( item ) != ComparisonResult.Identical )
					{
						callback(
							TPStatus.Error,
							-1,
							string.Format( "Misslyckades med jämförelse av filen \"{0}\"", item.Name ),
							string.Empty );
						return;
					}
				callback( TPStatus.VerifiedFolder, 99, _folder2Transfer.Name, strServerResponse );

				//alla mapparna verifierade - först nu överför vi end-filerna
				if ( !_fCurrentJobIsBackup )
				{
					strWhere = "Uploading zzlast";
					UploadItem(
						null,
						folderRemote.Name + "/zzlast",
						false );
				}

				callback( TPStatus.Complete, 100, "", "" );
			}
			catch ( Exception ex )
			{
				callback( TPStatus.Error, -1, ex.ToString(), strWhere );
			}
		}

		private bool folderExistsOnServer( string strFolder )
		{
			return _folderServerRoot.hasItem(strFolder, true, 0) != ComparisonResult.MissingInFolder;
		}

		private FtpWebRequest createRequest(
			string requestString )
		{
			/*
			using ( MemoryStream ms = new MemoryStream() )
				{
					using ( StreamWriter sw = new StreamWriter( ms, System.Text.Encoding.Default ) )
						sw.Write( requestString );
					byte[] buffer = ms.GetBuffer();
					System.Text.StringBuilder sb = new System.Text.StringBuilder();
					for ( int i=0 ; i<buffer.Length && buffer[i]!=0 ; i++ )
					{
						char c = (char)buffer[i];
						if ( c != '?' )
							sb.Append( c );
					}
					requestString = sb.ToString();
				}
			 */
			FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ServerName + requestString);
			if ( _rnd.Next(200) == 1 && LOCALHOST )
				request = (FtpWebRequest)WebRequest.Create("ftp://a.bad.url/" + requestString);
			request.Credentials = _networkCredential;
			return request;
		}

		private void createFolderOnServer( string strFolder )
		{
			if ( folderExistsOnServer(strFolder) )
				return;

			FtpWebRequest request;
			FtpWebResponse response;
			for ( int i = 1 ; ; i++ )
				try
				{
					request = createRequest( strFolder );
					request.Method = WebRequestMethods.Ftp.MakeDirectory;
					response = (FtpWebResponse)request.GetResponse();
					break;
				}
				catch ( Exception ex )
				{
					if ( ex.ToString().IndexOf( "(500)" ) < 0 || i > 2 )
						throw ex;
					Thread.Sleep( i * 500 );
				}

			response.Close();

			_folderServerRoot.AddFakeItem( strFolder, strFolder, 0, true );
		}

		private Folder GetRemoteFolder(
			string strFolder,
			bool fKeepAlive,
			out string strResponseText )
		{
			if ( !string.IsNullOrEmpty(strFolder) )
				if ( !folderExistsOnServer( strFolder ) )
				{
					strResponseText = null;
					return null;
				}

			FtpWebRequest request;
			FtpWebResponse ftpResponse;
			Stream ftpStream;
			for ( int i = 1 ; ; i++ )
				try
				{
					request = createRequest( strFolder );
					request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
					request.KeepAlive = fKeepAlive && i==1;
					ftpResponse = (FtpWebResponse)request.GetResponse();
					ftpStream = ftpResponse.GetResponseStream();
					break;
				}
				catch ( Exception ex )
				{
					if ( ex.ToString().IndexOf("(500)")<0 || i>2 )
						throw ex;
					Thread.Sleep( i * 500 );
				}

			using( StreamReader sr = new StreamReader(ftpStream,System.Text.Encoding.Default) )
				strResponseText = sr.ReadToEnd();
			ftpStream.Close();
			ftpResponse.Close();

			Folder folder = new Folder(strFolder);
			if ( !string.IsNullOrEmpty( strResponseText ) )
				foreach ( string strLine in strResponseText.Replace( '\r', '\n' ).Split( '\n' ) )
					if ( strLine.Length!=0 )
						folder.AddRemoteItem( strFolder, strLine );

			return folder;
		}

		private bool UploadItem(
			Item itmLocal,
			string strRemoteFileName,
			bool fKeepAlive )
		{
			FtpWebRequest request;
			Stream requestStream;

			//get the FTP upload stream
			for ( int i=1 ; ; i++ )
				try
				{
					request = createRequest( strRemoteFileName );
					request.Method = WebRequestMethods.Ftp.UploadFile;
					request.UseBinary = true;
					request.Timeout = 60 * 60 * 1000;
					request.KeepAlive = fKeepAlive && i==1;
					requestStream = request.GetRequestStream();
					break;
				}
				catch ( Exception ex )
				{
					if ( (ex.ToString().IndexOf( "(500)" )<0 && ex.ToString().IndexOf( "(550)" )<0) || i>2 )
						throw ex;
					Thread.Sleep( i*500 );
				}

			if ( itmLocal != null )
			{
				const long MAXBUF = 4 * 1024 * 1024;
				long lBytesToTransfer = itmLocal.Size;
				using ( FileStream fs = new FileStream( itmLocal.FullName, FileMode.Open, FileAccess.Read ) )
					while ( lBytesToTransfer > 0 )
					{
						byte[] buffer = new byte[Math.Min( lBytesToTransfer, MAXBUF )];
						fs.Read( buffer, 0, buffer.Length );

						//write the byte array to the stream
						if ( _rnd.Next( 500 ) == 1 && LOCALHOST )
							requestStream.Write( buffer, 0, buffer.Length / 2 );
						else
							requestStream.Write( buffer, 0, buffer.Length );

						lBytesToTransfer -= buffer.Length;
					}
			}
			requestStream.Close();

			FtpWebResponse response = (FtpWebResponse)request.GetResponse();
			response.Close();

			if ( DelayTimeBetweenFiles!=0 )
				Thread.Sleep(DelayTimeBetweenFiles);
			return true;
		}

		public class Folder
		{
			public readonly string Name;
			private ArrayList _items = new ArrayList();

			public Folder(string strName)
			{
				Name = strName;
			}

			public IList Items
			{
				get { return ArrayList.ReadOnly(_items); }
			}

			public ComparisonResult hasItem(Item item)
			{
				return hasItem(item.Name, item.IsFolder, item.Size);
			}

			public ComparisonResult hasItem( string strName, bool fFolder, long lSize)
			{
				foreach (Item item in _items)
					if (string.Compare(item.Name, strName, true) == 0 && item.IsFolder==fFolder )
						return item.Size == lSize ?
							ComparisonResult.Identical : ComparisonResult.SizeDifferent;
				return ComparisonResult.MissingInFolder;
			}

			public void AddRemoteItem(string strFolder, string strFTPLine)
			{
				_items.Add(new Item(strFolder, strFTPLine));
			}

			public Item AddLocalItem( string strFile, string strRemoteName )
			{
				Item item = new Item( new FileInfo( strFile ), strRemoteName );
				_items.Add( item );
				return item;
			}

			public void AddFakeItem(
				string Name,
				string FullName,
				long Size,
				bool IsFolder )
			{
				_items.Add( new Item(Name,FullName,Size,IsFolder) );
			}

		}

		public class Item : IComparable
		{
			public readonly string Name;
			public readonly string FullName;
			public readonly long Size;
			public readonly bool IsFolder;

			public Item( string strFolder, string strFTPLine )
			{
				string[] astr = _rexSpaces.Split(strFTPLine, 9);
				if ( astr.Length!=9 || !Int64.TryParse(astr[4], out Size) )
					return;
				IsFolder = strFTPLine[0] == 'd';
				Name = astr[8];
				FullName = strFolder + "/" + Name;
			}

			public Item( FileInfo fileInfo, string strRemoteName )
			{
				Name = strRemoteName==null ? fileInfo.Name : strRemoteName;
				FullName = fileInfo.FullName;
				Size = fileInfo.Exists ? fileInfo.Length : 0;
				IsFolder = false;
			}

			public Item(
				string Name,
				string FullName,
				long Size,
				bool IsFolder)

			{
				this.Name = Name;
				this.FullName = FullName;
				this.Size = Size;
				this.IsFolder = IsFolder;
			}

			int IComparable.CompareTo(object x)
			{
				return Name.CompareTo( (x as Item).Name );
			}

		}

	}
}
