using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;
using Microsoft.Win32;
using System.Drawing.Imaging;
using System.Collections;
using System.Windows.Forms;
using System.IO;

namespace Plata
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>

    public delegate void delegateNyttFoto(bool fInternal, byte[] jpgData, byte[] rawData);

    public enum Brand
    {
        Photomic,
        Kungsfoto
    }

	public class Global
	{
		static public readonly string RegistryKey = @"SOFTWARE\Photomic\Plåta";
		static public readonly string Felrapportfil = "felrapport.log";
		static public readonly string ActionsLog = "actions.log";

		public const int Porträttfotobredd = 600;
		public const int Gruppfotobredd = 900;

		static public PlataDM.Skola Skola;
		static public bool Fotografdator;

		static public Media.MP3Player MediaPlayer = new Media.MP3Player();
		static public UserPreferences Preferences = new UserPreferences();
		static public string DataPath;

		readonly static public string[] aTitlarBas = new []
		{
			"Klf", "Elev", "Mentor", "Lärare", "Kontaktlärare", "Elevass", "Rektor", "Studierektor", "Bitr Rektor",
			"Vikarie", "Ordf", "Sekr", "Kassör", "V Ordf", "V Sekr", "V Kassör", "Ledamot", "Suppl", "Fritidsledare",
			"Barnskötare", "Skolkurator","Skolpsykolog", "Datatekn", "Kanslist", "Skolass", "Vaktm", "Fritidspedagog",
			"Speciallärare", "Praktikant", "Skolsköterska", "Lärarkandidat", "Specialpedagog", "Bibliotekarie",
			"Biblioteksass", "Talpedagog", "Studie/yrkesvägled", "Verksamhetschef"
		};

		public static readonly ArrayList GrpNoPhoto_Reasons_Complete = new ArrayList(
			new string[] { "Ej på fotoschemat", "Annan fotograf", "Skall restfotas", "Sammanslagen grupp" } );
		public static readonly ArrayList GrpNoPhoto_Reasons_NonComplete = new ArrayList(
			new string[] { "Inte vara med", "Övrigt" } );

		static Global()
		{
		    Fotografdator = ((string) ReadRegistry(Registry.CurrentUser, RegistryKey, "fotograf", null) == "1");
		    Preferences.loadXML((string) ReadRegistry(Registry.CurrentUser, RegistryKey, "preferences", ""));
		}

	    public static DateTime Now
		{
            get { return DateTime.Now; }
		}

		public static void sparaInställningar()
		{
			WriteRegistry( Registry.CurrentUser, RegistryKey, "preferences", Preferences.saveXML() );
		}

		static public RotateFlipType PorträttRotateFlipType
		{
			get
			{
				switch ( Preferences.Porträttrotering )
				{
					case PlataDM.Rotering.Medurs: return RotateFlipType.Rotate90FlipNone;
					case PlataDM.Rotering.Moturs: return RotateFlipType.Rotate270FlipNone;
				}
				return RotateFlipType.RotateNoneFlipNone;
			}
		}

		public static void WriteRegistry(RegistryKey ParentKey , string SubKey , string ValueName, object Value)
		{
		    try 
			{
				var key = ParentKey.OpenSubKey(SubKey, true) ?? ParentKey.CreateSubKey(SubKey);
			    key.SetValue(ValueName, Value);
			} 
			catch ( Exception ex )
			{
				Console.WriteLine("Error occurs in WriteRegistry " + ex.Message);
			}
		}

		public static object ReadRegistry( RegistryKey ParentKey, string SubKey, string ValueName, object Default )
		{
		    try
			{
				var key = ParentKey.OpenSubKey(SubKey, true);
				if ( key==null )
					return Default;
				var retVal = key.GetValue(ValueName);
				return retVal ?? Default;
			}
			catch ( Exception ex )
			{
				Console.WriteLine("Error occurs in ReadRegistry " + ex.Message);
				return null;
			}

		}

		public static void showMsgBox( Form parent, string strText, params object[] args )
		{
			MessageBox.Show( parent, string.Format( strText, args ),
				AppSpecifics.Name, MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
		}

		public static void showMsgBox( Control control, string strText, params object[] args )
		{
			showMsgBox( control.FindForm(), string.Format( strText, args ) );
		}


		public static DialogResult askMsgBox( Form parent, string strText, bool fExcl )
		{
            return MessageBox.Show(parent, strText, AppSpecifics.Name, MessageBoxButtons.YesNo,
				fExcl ? MessageBoxIcon.Exclamation : MessageBoxIcon.Question );
		}

		public static DialogResult askMsgBox( Control control, string strText, bool fExcl )
		{
			return askMsgBox( control.FindForm(), strText, fExcl );
		}

		public static string skapaSäkertFilnamn( string strFN )
		{
			string strFrån = "/\\:*?\"<>";
			string strTill = "dbkafcms";

			for ( int i=0 ; i<strFrån.Length ; i++ )
				strFN = strFN.Replace( strFrån[i].ToString(), "("+strTill[i]+")" );

			return strFN;
		}

		public static void storeErrorReport( string strExceptionText , string strDescription )
		{
			if ( Skola==null )
				return;
			try 
			{
				var w = new StreamWriter( Skola.HomePathCombine(Felrapportfil), true );
                w.WriteLine("\r\n********** Program version: {0}\r\n", AppSpecifics.Version);
				w.WriteLine( "When: {0}", Global.Now.ToString() );
				w.WriteLine( "Exception: {0}", strExceptionText );
				w.WriteLine( "\r\nUser description: {0}", strDescription );
				w.Close();
			} 
			catch
			{
			}
		}

		public static void logAction( string strText, params object[] args )
		{
			if ( Skola==null )
				return;
			try 
			{
				var w = new StreamWriter( Skola.HomePathCombine(ActionsLog), true );
				w.WriteLine( "{0:yyyy-MM-dd HH:mm:ss} {1}", Global.Now, string.Format( strText, args ) );
				w.Close();
			} 
			catch
			{
			}
		}

		public static void storeErrorReport( Exception ex, string strDescription )
		{
			storeErrorReport( ex.ToString(), strDescription );
		}

		public static void ritaGruppRam( Graphics g, Rectangle rect )
		{
			var r = rect;
			r.Inflate( -(int)(r.Width*0.05), -(int)(r.Height*0.025) );
			using ( Pen penRed = new Pen(Brushes.Red,2) )
				g.DrawRectangle( penRed, r );
			int n1cm = (int)(rect.Width*0.0357);
			r.Inflate( -n1cm, -n1cm );
			g.DrawRectangle( Pens.Black, r );
			r.Inflate( -1, -1 );
			g.DrawRectangle( Pens.White, r );
		}

		public static void ritaPorträttRam( Graphics g, Rectangle rect )
		{
			int nY = (int)(rect.Height*0.13);
			g.DrawLine( Pens.Black, rect.Left, rect.Top+nY, rect.Left+rect.Width, rect.Top+nY );
			nY = (int)(rect.Height*0.90);
			g.DrawLine( Pens.Black, rect.Left, rect.Top+nY, rect.Left+rect.Width, rect.Top+nY );
		}

		public static void writeLog( string strFile, string strText )
		{
			try
			{
				using ( var writer = new StreamWriter( Skola.HomePathCombine(strFile), true ) )
					writer.WriteLine( strText );
			}
			catch
			{
			}
		}

		private static string strTempBase = null;
		private static int nTempBase = 0;

		public static string GetTempPath()
		{
			string strResult = Path.Combine( Preferences.MainPath, "_temp" );
			if ( strTempBase==null )
			{
				strTempBase = string.Format( "{0:00}{1:00}{2:00}", Global.Now.Day, Global.Now.Hour, Global.Now.Minute );
				nTempBase = Global.Now.Second*100;
				if ( !Directory.Exists(strResult) )
					Directory.CreateDirectory(strResult);
			}
			return strResult;
		}

		public static string GetTempFileName()
		{
			string strPath = GetTempPath();
			return string.Format( "{0}\\plata{1}{2}.tmp", strPath, strTempBase, nTempBase++ );
		}

		public static string RenameFile( string strFullPath, string strNewName )
		{
			string strNewFullName = Path.Combine( Path.GetDirectoryName(strFullPath), strNewName );
			File.Move( strFullPath, strNewFullName );
			return strNewFullName;
		}

		public static void saveSchool( bool fCreateBak )
		{
			if ( Skola!=null )
			{
				if ( Skola.PortRotering==PlataDM.Rotering.Okänd )
					Skola.PortRotering = Preferences.Porträttrotering;
				Skola.save( fCreateBak );
			}
		}

		public static void optCheck( bool fVal, RadioButton optYes, RadioButton optNo )
		{
			if ( fVal )
				optYes.Checked = true;
			else
				optNo.Checked = true;
		}

		public static void optCheck( PlataDM.Answer Val, RadioButton optYes, RadioButton optNo )
		{
			switch ( Val )
			{
				case PlataDM.Answer.Yes:
					optYes.Checked = true;
					break;
				case PlataDM.Answer.No:
					optNo.Checked = true;
					break;
				default:
					optYes.Checked = optNo.Checked = false;
					break;
			}
		}

		public static void optCheck( PlataDM.Answer Val, RadioButton optYes, RadioButton optNo, RadioButton optQ )
		{
			switch ( Val )
			{
				case PlataDM.Answer.Yes:
					optYes.Checked = true;
					break;
				case PlataDM.Answer.No:
					optNo.Checked = true;
					break;
				default:
					optQ.Checked = true;
					break;
			}
		}

		public static PlataDM.Answer getOptCheck( RadioButton optYes, RadioButton optNo )
		{
			if ( optYes.Checked )
				return PlataDM.Answer.Yes;
			if ( optNo.Checked )
				return PlataDM.Answer.No;
			return PlataDM.Answer.Unknown;
		}

		public static string getAppPath( string strFile )
		{
			string strAppPath = Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().Location );
			if ( string.IsNullOrEmpty( strFile ) )
				return strAppPath;
			return Path.Combine( strAppPath, strFile );
		}

		public static string getMemoryCardCacheFolder()
		{
				string strPath = Path.Combine(Preferences.MainPath, "_MemoryCard");
				if (!Directory.Exists(strPath))
					Directory.CreateDirectory(strPath);
				return strPath;
		}

		public static System.Diagnostics.Process runSA(
			string runThis,
			string arguments )
		{
		    var psi = new System.Diagnostics.ProcessStartInfo
		                  {
		                      FileName = runThis,
		                      WorkingDirectory = Path.GetDirectoryName(runThis),
		                      Arguments = arguments
		                  };
		    if ( Fotografdator || string.Compare(Environment.UserName, "plåta", StringComparison.OrdinalIgnoreCase)==0)
			{
				var ss = new System.Security.SecureString();
				for ( var i = 9 ; i >= 0 ; i-- )
					ss.AppendChar( "53sde64rfd"[i] );
				psi.UserName = "sysadm";
				psi.Password = ss;
				psi.UseShellExecute = false;
			}
			return System.Diagnostics.Process.Start( psi );
		}


	}

}

