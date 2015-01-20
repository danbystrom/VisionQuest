using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Photomic.ArchiveStuff.Core;

namespace Plata.Burn
{
	public partial class FAskAboutSaveCDToFolder : vdUsr.baseGradientForm
	{
		private List<BurnFileInfo> _list;

		private FAskAboutSaveCDToFolder()
		{
			InitializeComponent();
		}

		public static DialogResult showDialog(
			Form parent,
			string strName,
			List<BurnFileInfo> list )
		{
			using ( var dlg = new FAskAboutSaveCDToFolder() )
			{
				dlg._list = list;
				dlg.txtExistingFolder.Text = Global.Preferences.FakeCDPath;
				dlg.txtNewFolderName.Text = strName;
				dlg.optToCD.Checked = true;
				return dlg.ShowDialog( parent );
			}
		}

		private void cmdOK_Click( object sender, EventArgs e )
		{
			if ( optToCD.Checked )
			{
				this.DialogResult = DialogResult.OK;
				return;
			}

			string strDest = txtExistingFolder.Text.Trim();
			if ( !Directory.Exists( strDest ) )
			{
				if ( Global.askMsgBox(
						this,
						string.Format( "Mappen \"{0}\" finns inte. Vill du skapa den?", strDest ),
						true ) != DialogResult.Yes )
					return;
				Directory.CreateDirectory( strDest );
			}
			Global.Preferences.FakeCDPath = strDest;
			strDest = Path.Combine( strDest, txtNewFolderName.Text );

			if ( Directory.Exists( strDest ) )
			{
				if ( Global.askMsgBox(
						this,
						string.Format( "Mappen \"{0}\" finns redan. Vill du skriva över den?", strDest ),
						true ) != DialogResult.Yes )
					return;
				Directory.Delete( strDest, true );
			}

			try
			{
				this.Cursor = Cursors.WaitCursor;
				Directory.CreateDirectory( strDest );
				pbr.Maximum = _list.Count;
				this.Refresh();
				foreach ( BurnFileInfo bfi in _list )
				{
					string strDFN = Path.Combine( strDest, bfi.CDFullFileName.Substring( 1 ) );
					if ( !Directory.Exists( Path.GetDirectoryName(strDFN) ) )
						Directory.CreateDirectory( Path.GetDirectoryName(strDFN) );
					if ( bfi.IsTemp )
					{
						int nI = bfi.LocalFullFileName.IndexOf(':');
						if ( nI == 1 &&  (bfi.LocalFullFileName[0] & 0x1F) == (strDest[0] & 0x1F) )
							File.Move( bfi.LocalFullFileName, strDFN );
						else
						{
							File.Copy( bfi.LocalFullFileName, strDFN );
							File.Delete( bfi.LocalFullFileName );
						}
					}
					else
						File.Copy( bfi.LocalFullFileName, strDFN );
					pbr.Increment( 1 );
					pbr.Refresh();
				}

				this.Cursor = Cursors.Default;
				this.DialogResult = DialogResult.Cancel; // this means that we cancel the BURN!
			}
			catch ( Exception ex )
			{
				this.Cursor = Cursors.Default;
				Global.showMsgBox( this, ex.ToString() );
			}

		}

		private void cmdBrowse_Click( object sender, EventArgs e )
		{
			using ( FolderBrowserDialog dlg = new FolderBrowserDialog() )
			{
				dlg.SelectedPath = txtExistingFolder.Text.Trim();
				if ( dlg.ShowDialog( this ) == DialogResult.OK )
					txtExistingFolder.Text = dlg.SelectedPath;
			}
		}

		private void txtExistingFolder_TextChanged( object sender, EventArgs e )
		{
			optFolder.Checked = true;
		}

		private void txtNewFolderName_TextChanged( object sender, EventArgs e )
		{
			optFolder.Checked = true;
		}

	}
}
