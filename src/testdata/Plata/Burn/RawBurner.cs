using System;
using System.Windows.Forms;
using System.IO;

using XPBurn;

namespace Plata
{
	/// <summary>
	/// Summary description for RawBurner.
	/// </summary>
	public class RawBurner
	{
		private XPBurnCD _burn;

		public void RawBurn( Form frm, string strPath )
		{
			try
			{
				_burn = new XPBurnCD();
				_burn.BurnerDrive = (string)_burn.RecorderDrives[0];
			} 
			catch ( Exception ex )
			{
				Global.showMsgBox( frm, ex.Message );
				return;
			}

			long lSize = 0;
			foreach ( string strFN in Directory.GetFiles(strPath) )
				if ( !strFN.EndsWith(".emf") )
				{
					FileInfo fi = new FileInfo(strFN);
					lSize += 1+fi.Length/1024;
					_burn.AddFile( strFN, "\\" + Path.GetFileName(strFN) );
				}

			string strMediaType = (lSize/1024)>600 ? "DVD" : "CD";
			if ( MessageBox.Show( frm,
				"Sätt i en " + strMediaType + " i brännaren och tryck OK för att starta.",
				Global.AppName, MessageBoxButtons.OKCancel,
				MessageBoxIcon.Exclamation ) != DialogResult.OK )
				return;


			try
			{
				_burn.VolumeName = Path.GetFileName( strPath );
				using ( frmBränning dlg = new frmBränning(_burn) )
					dlg.ShowDialog();
			}
			catch ( Exception ex )
			{
				Global.showMsgBox( frm, ex.Message );
			}

		}

	}

}
