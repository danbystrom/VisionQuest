using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Photomic.ArchiveStuff.Core;

namespace Plata.Burn
{

	public class BurnHelpers
	{
		private BurnHelpers()
		{
		}

		public static void theNewAndFunBurn(
			Form parent,
			List<BurnFileInfo> alFiles,
			string strType,
			string strLabel )
		{

			if ( !Global.Fotografdator )
				if ( FAskAboutSaveCDToFolder.showDialog(
						parent,
						string.Format( strLabel, 1, 1 ),
						alFiles ) != DialogResult.OK )
					return;

			string strInstructionFile = Global.GetTempFileName();
			string strLogFile = Global.GetTempFileName();

			var x = new vdUsr.vdSimpleXMLWriter();
			x.descend( "PLATAXHG" );

			x.descend( "INFO" );
			x.writeValue( "school", Global.Skola.HomePath );
			x.writeValue( "type", strType );
			x.writeValue( "label", strLabel + "_{0}_{1}" );
			x.writeValue( "returnapp", System.Reflection.Assembly.GetExecutingAssembly().Location );
			x.writeValue( "returnarg", string.Format( "\"plataxhg={0}\"", strInstructionFile ) );
			x.writeValue( "resultfile", strLogFile );
			x.writeValue( "windowcaption", string.Empty );
			x.writeValue( "recordcontentsfile", Path.Combine( Global.Preferences.MainPath, "_burnhistory" ) );
			x.ascend();

			var checkDuplicates = new Dictionary<string, string>();
			x.descend( "FILES" );
			foreach ( var bfi in alFiles )
			{
				if ( checkDuplicates.ContainsKey( bfi.LocalFullFileName ) )
					continue;
				checkDuplicates.Add( bfi.LocalFullFileName, bfi.LocalFullFileName );

				var fi = new FileInfo( bfi.LocalFullFileName );
				var lSize = (fi.Length + 1023) / 1024;
				if ( (fi.Attributes & FileAttributes.ReadOnly) != 0 )
					File.SetAttributes( bfi.LocalFullFileName, fi.Attributes & ~FileAttributes.ReadOnly );
				x.descend( "FILE" );
				x.writeValue( "local", bfi.LocalFullFileName );
				x.writeValue( "oncd", bfi.CDFullFileName );
				x.writeValue( "size", (int)lSize );
				if ( bfi.IsTemp )
					x.writeValue( "tmp", "1" );
				if ( bfi.OnAll )
					x.writeValue( "onall", "1" );
				x.ascend();
			}
			x.ascend();

			x.ascend();
			x.endSaveFile( strInstructionFile );

			var info = new System.Diagnostics.ProcessStartInfo();
			info.FileName = Global.getAppPath( "vdStandAloneBurn.exe" );
			info.Arguments = strInstructionFile;
			var process = System.Diagnostics.Process.Start( info );
			FMain.theOneForm.endApp( false );
		}

	}

}