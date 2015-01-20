using System;
using System.Collections;
using System.Text;
using System.IO;

namespace Plata
{
	class Fotografer
	{
		private const string Filename = "fotografer.txt";
		private static Hashtable _hash = new Hashtable();

		public static void initFromDatafile()
		{
			try
			{
				foreach ( string strLine in File.ReadAllLines( Path.Combine( Global.DataPath, Filename ), Encoding.Default ) )
				{
					var astr = strLine.Split( '\t' );
					int nID;
					if ( astr.Length != 2 || !int.TryParse( astr[0], out nID ) || nID == 0 )
						continue;
					_hash.Add( nID, astr[1] );
				}
			}
			catch
			{
			}
		}

		public static void appendFromOrder( PlataDM.Skola skola )
		{
			foreach ( int id in skola.Fotografer.IDs )
			{
				if ( _hash.ContainsKey( id ) )
					_hash.Remove( id );
				_hash.Add( id, skola.Fotografer[id] );
			}

			try
			{
				using ( var sw = new StreamWriter( Path.Combine( Global.DataPath, Filename ), false, Encoding.Default ) )
					foreach ( int id in _hash.Keys )
						sw.WriteLine( "{0}\t{1}", id, _hash[id] );
			}
			catch
			{
			}
		}

		public static string Name( int id )
		{
			return _hash[id] as string ?? string.Format( "Fotograf {0}", id );
		}

	}

}
