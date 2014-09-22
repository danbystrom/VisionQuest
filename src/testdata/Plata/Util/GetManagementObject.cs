using System;
using System.Management;
using System.Threading;
using System.Windows.Forms;

namespace Plata
{
	/// <summary>
	/// Summary description for GetManagementObject.
	/// </summary>
	public class GetManagementObject
	{
		public delegate void ManagementClassFound( ManagementObject managementObject );

		private Control _synkObject;
		private ManagementClassFound _callback;
		private string _strDrive;

		public GetManagementObject( Control synkObject, ManagementClassFound callback, string strDrive )
		{
			_synkObject = synkObject;
			_callback = callback;
			_strDrive = strDrive.Substring(0,2);
			Thread t = new Thread( new ThreadStart(search) );
			t.Start();
		}

		private void search()
		{
			try
			{
				ManagementClass diskClass = new ManagementClass("Win32_LogicalDisk");
				foreach ( ManagementObject disk in diskClass.GetInstances() )
					if ( string.Compare( (string)disk["Name"], _strDrive, true ) == 0 )
					{
						_synkObject.Invoke( _callback, new object[] { disk } );
						return;
					}
				_synkObject.Invoke( _callback, new object[] { null } );
			}
			catch
			{
			}
		}

	}

}
