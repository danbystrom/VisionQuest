using System;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;

namespace Plata.Version
{
	/// <summary>
	/// Summary description for AutoUpdateMe.
	/// </summary>
	public class FileVersion
	{
		private struct VS_FIXEDFILEINFO
		{
			public uint dwSignature;
			public uint dwStrucVersion;         //  e.g. 0x00000042 = "0.42"
			public uint dwFileVersionMS;        //  e.g. 0x00030075 = "3.75"
			public uint dwFileVersionLS;        //  e.g. 0x00000031 = "0.31"
			public uint dwProductVersionMS;     //  e.g. 0x00030010 = "3.10"
			public uint dwProductVersionLS;     //  e.g. 0x00000031 = "0.31"
			public uint dwFileFlagsMask;        //  = 0x3F for version "0.42"
			public uint dwFileFlags;            //  e.g. VFF_DEBUG Or VFF_PRERELEASE
			public uint dwFileOS;               //  e.g. VOS_DOS_WINDOWS16
			public uint dwFileType;             //  e.g. VFT_DRIVER
			public uint dwFileSubtype;          //  e.g. VFT2_DRV_KEYBOARD
			public uint dwFileDateMS;           //  e.g. 0
			public uint dwFileDateLS;           //  e.g. 0
		}

		[DllImport( "version.dll" )]
		private static extern bool GetFileVersionInfo( string sFileName,
			int handle, int size, byte[] infoBuffer );
		[DllImport( "version.dll" )]
		private static extern int GetFileVersionInfoSize( string sFileName,
			out int handle );
		// The third parameter - "out string pValue" - is automatically
		// marshaled from ANSI to Unicode:
		[DllImport( "version.dll" )]
		private static extern bool VerQueryValue( byte[] pBlock,
			string pSubBlock, out string pValue, out uint len );
		// This VerQueryValue overload is marked with 'unsafe' because 
		// it uses a short*:
		[DllImport( "version.dll" )]
		private static extern bool VerQueryValue( byte[] pBlock,
			string pSubBlock, out uint ptr, out uint len );

		[DllImport( "Kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false )]
		private static extern void MoveMemory( out VS_FIXEDFILEINFO x, uint src, int size );

		public static int[] getFileVersion( string strFN )
		{
			int handle = 0;
			// Figure out how much version info there is:
			int size = GetFileVersionInfoSize( strFN, out handle );
			if ( size == 0 )
				return null;

			byte[] buffer = new byte[size];

			if ( !GetFileVersionInfo( strFN, handle, size, buffer ) )
				return null;

			uint len, pointer;
			if ( !VerQueryValue( buffer, "\\", out pointer, out len ) )
				return null;

			VS_FIXEDFILEINFO FileInfo;
			MoveMemory( out FileInfo, pointer, 13*4 );

			return new int[]
			{
				(int)(FileInfo.dwFileVersionMS >> 16),
				(int)(FileInfo.dwFileVersionMS & 0xFFFF),
				(int)(FileInfo.dwFileVersionLS >> 16),
				(int)(FileInfo.dwFileVersionLS & 0xFFFF),
				0
			};

		}

	}

}
