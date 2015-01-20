using System;
using System.Windows.Forms;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace Plata
{
	class Upgrader
	{
		private bool _fCanUpgrade = false;
		private byte[] _abyteUpgradeBin;
		private byte[] _abyteTheZip;

		private Upgrader()
		{
		}

		public Upgrader( Stream stream )
		{
			init( stream );
		}

		private bool init( Stream stream )
		{
			_fCanUpgrade = false;

			var strMinimumDate = AppSpecifics.Version;
			var fFoundNewVersion = false;
			ZipEntry theEntry;

			try
			{
				stream.Seek( 0, SeekOrigin.Begin );
				_abyteTheZip = new byte[stream.Length];
				stream.Read( _abyteTheZip, 0, _abyteTheZip.Length );

				stream.Seek( 0, SeekOrigin.Begin );
				var zis = new ZipInputStream( stream );
				while ( ( theEntry = zis.GetNextEntry() ) != null )
				{
					var strEntry = theEntry.Name.ToLower();
					if ( strEntry.CompareTo( "upgrade.bin" ) == 0 )
					{
						_abyteUpgradeBin = new byte[theEntry.Size];
						zis.Read( _abyteUpgradeBin, 0, _abyteUpgradeBin.Length );
						break;
					}
				}

				stream.Seek( 0, SeekOrigin.Begin );
				zis = new ZipInputStream( stream );
				while ( ( theEntry = zis.GetNextEntry() ) != null )
				{
					var strEntry = theEntry.Name.ToLower();
					if ( strEntry.CompareTo( "upgrade.txt" ) == 0 )
					{
						byte[] ab = new byte[theEntry.Size];
						zis.Read( ab, 0, ab.Length );
						string s = Encoding.Default.GetString( ab );
						if ( Regex.IsMatch( s, @"\d{4}-\d{2}-\d{2}" ) )
							fFoundNewVersion = s.CompareTo( strMinimumDate ) > 0;
						break;
					}
				}

				stream.Seek( 0, SeekOrigin.Begin );
				zis = new ZipInputStream( stream );
				while ( ( theEntry = zis.GetNextEntry() ) != null )
				{
					string strEntry = theEntry.Name.ToLower();
					if ( Regex.IsMatch( strEntry, @"^\d{4}-\d{2}-\d{2}.zip$" ) )
					{
						_abyteTheZip = null;
						fFoundNewVersion = strEntry.Substring( 0, 10 ).CompareTo( strMinimumDate ) > 0;
						if ( fFoundNewVersion )
						{
							_abyteTheZip = new byte[theEntry.Size];
							zis.Read( _abyteTheZip, 0, _abyteTheZip.Length );
						}
						break;
					}
				}

				if ( !fFoundNewVersion || _abyteUpgradeBin == null )
					return false;
				_fCanUpgrade = _abyteTheZip.Length > 10000;
			}
			catch ( Exception ex )
			{
				System.Diagnostics.Debug.WriteLine( ex.ToString() );
			}
			return _fCanUpgrade;
		}

		public bool run()
		{
			if ( !_fCanUpgrade )
				return false;
			try
			{
				string strExe = Global.getAppPath( "upgrade.exe" );
				string strZip = Path.Combine( Global.GetTempPath(), "upgrade.zip" );
				File.WriteAllBytes( strExe, _abyteUpgradeBin );
				File.WriteAllBytes( strZip, _abyteTheZip );

				var psi = new System.Diagnostics.ProcessStartInfo();
				psi.FileName = strExe;
				psi.WorkingDirectory = Path.GetDirectoryName( strExe );
				psi.Arguments = "\"" + strZip + "\"";
				System.Diagnostics.Process.Start( psi );
				return true;
			}
			catch ( Exception ex )
			{
				Global.showMsgBox( null, ex.ToString() );
				return false;
			}
		}

		public static Upgrader lookForUpgrade( string strPath )
 		{
			Upgrader upgrader = new Upgrader();

			try
			{
				string[] astrZIPs = Directory.GetFiles( strPath, "plata_20*.zip" );
				if ( astrZIPs.Length == 0 )
					return null;
				string strZipFile = astrZIPs[astrZIPs.Length - 1];

				using ( Stream stream = new FileStream( strZipFile, FileMode.Open, FileAccess.Read ) )
					if ( upgrader.init( stream ) )
						return upgrader;
			}
			catch
			{
			}
			return null;
		}

		public static bool runUpgrade(
			string strPath )
		{
			Upgrader upgrader = lookForUpgrade( strPath );
			if ( upgrader != null )
				return upgrader.run();
			else
				return false;
		}

		public static bool runUpgradeFromAnywhere()
		{
			foreach ( string s in Directory.GetLogicalDrives() )
				if ( runUpgrade( s ) )
					return true;
			return false;
		}

		public static string findSCardFromAnywhere()
		{
			foreach ( var s in Directory.GetLogicalDrives() )
				try
				{
					string strSCard = Path.Combine( s, "scard.bat" );
					if ( File.Exists( strSCard ) )
						return strSCard;
				}
				catch
				{
				}
			return null;
		}

	}

}
