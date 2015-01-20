using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace Plata.OpenDialog
{
	/// <summary>
	/// Summary description for usrOpenPhotographers.
	/// </summary>
	public class usrOpenImportOrderFile : baseUsrTab
	{
		private enum Z
		{
			ReturnFalse,
			ReturnTrue,
			HitMeAgain
		}

		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtOrder;
		private Button cmdBrowse;
		private RadioButton optSearch;
		private RadioButton optUseFolder;

		private string _strFN = null;  // om file browsern använts

		private StringBuilder _sbLog;

		public usrOpenImportOrderFile()
		{
			InitializeComponent();
			Text = "Från orderfil";
			
			txtOrder.Text = Global.Preferences.OpenOrderFolder;
			if (Global.Preferences.UseOpenOrderFolder)
				optUseFolder.Checked = true;
			else
				optSearch.Checked = true;
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label4 = new System.Windows.Forms.Label();
			this.txtOrder = new System.Windows.Forms.TextBox();
			this.cmdBrowse = new System.Windows.Forms.Button();
			this.optSearch = new System.Windows.Forms.RadioButton();
			this.optUseFolder = new System.Windows.Forms.RadioButton();
			this.SuspendLayout();
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font( "Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
			this.label4.Location = new System.Drawing.Point( 10, 8 );
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size( 387, 40 );
			this.label4.TabIndex = 8;
			this.label4.Text = "Placera CD-skivan eller USB-minnet med orderfilen på i datorn och tryck OK.";
			// 
			// txtOrder
			// 
			this.txtOrder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtOrder.Location = new System.Drawing.Point( 29, 96 );
			this.txtOrder.Name = "txtOrder";
			this.txtOrder.Size = new System.Drawing.Size( 530, 20 );
			this.txtOrder.TabIndex = 7;
			this.txtOrder.Text = "d:\\";
			this.txtOrder.TextChanged += new System.EventHandler( this.txtOrder_TextChanged );
			// 
			// cmdBrowse
			// 
			this.cmdBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdBrowse.Location = new System.Drawing.Point( 565, 96 );
			this.cmdBrowse.Name = "cmdBrowse";
			this.cmdBrowse.Size = new System.Drawing.Size( 28, 22 );
			this.cmdBrowse.TabIndex = 11;
			this.cmdBrowse.Text = "...";
			this.cmdBrowse.Click += new System.EventHandler( this.cmdBrowse_Click );
			// 
			// optSearch
			// 
			this.optSearch.AutoSize = true;
			this.optSearch.Location = new System.Drawing.Point( 13, 51 );
			this.optSearch.Name = "optSearch";
			this.optSearch.Size = new System.Drawing.Size( 164, 18 );
			this.optSearch.TabIndex = 12;
			this.optSearch.TabStop = true;
			this.optSearch.Text = "Sök bland tillgängliga enheter";
			this.optSearch.UseVisualStyleBackColor = true;
			// 
			// optUseFolder
			// 
			this.optUseFolder.AutoSize = true;
			this.optUseFolder.Location = new System.Drawing.Point( 13, 75 );
			this.optUseFolder.Name = "optUseFolder";
			this.optUseFolder.Size = new System.Drawing.Size( 113, 18 );
			this.optUseFolder.TabIndex = 13;
			this.optUseFolder.TabStop = true;
			this.optUseFolder.Text = "Sök i denna mapp:";
			this.optUseFolder.UseVisualStyleBackColor = true;
			// 
			// usrOpenNewOrderFile
			// 
			this.Controls.Add( this.optUseFolder );
			this.Controls.Add( this.optSearch );
			this.Controls.Add( this.cmdBrowse );
			this.Controls.Add( this.label4 );
			this.Controls.Add( this.txtOrder );
			this.Name = "usrOpenNewOrderFile";
			this.ResumeLayout( false );
			this.PerformLayout();

		}
		#endregion

		public override bool openOrder( PlataDM.Skola skola )
		{
			_sbLog = new StringBuilder();
			if (!openOrder2(skola))
			{
				Global.showMsgBox( this, "Hittar ingen orderfil!\r\n\r\n{0}",
					Form.ModifierKeys==Keys.Shift ? _sbLog.ToString() : "" );
				return false;
			}

			string strSenasteVersion = skola.Versioner.senasteVersion;
			if (!string.IsNullOrEmpty(strSenasteVersion))
			{
                if (strSenasteVersion.CompareTo(AppSpecifics.Version) > 0)
					Global.showMsgBox(this, "OBS! OBS! OBS!!!\r\nDet finns en nyare version av Plåta att hämta hem från fotografwebben! Gör det så snart som möjligt!");
			}
			return true;
		}

		private bool openOrder2(PlataDM.Skola skola)
		{
			if ( optUseFolder.Checked )
			{
				switch ( openOrder3( txtOrder.Text, skola ) )
				{
					case Z.ReturnTrue:
						Global.Preferences.UseOpenOrderFolder = true;
						Global.Preferences.OpenOrderFolder = txtOrder.Text;
						return true;
					case Z.ReturnFalse:
						return false;
				}
			}
			else
				foreach ( string s in Directory.GetLogicalDrives() )
					try
					{
						switch (openOrder3(s, skola))
						{
							case Z.ReturnTrue:
								Global.Preferences.UseOpenOrderFolder = false;
								return true;
							case Z.ReturnFalse:
								return false;
						}
					}
					catch (PlataDM.PlataException ex)
					{
						throw new Exception( string.Format( "Försökte öppna order från enhet \"{0}\"", s ), ex );
					}
					catch
					{
					}
			return false;
		}

		private Z openOrder3(
			string strFolder,
			PlataDM.Skola skola )
		{
			Z z = openOrder3_logged( strFolder, skola );
			_sbLog.AppendFormat( "openOrder3, koll av \"{0}\" gav \"{1}\"\r\n",
				strFolder,
				z );
			return z;
		}

		private Z openOrder3_logged(
			string strFolder,
			PlataDM.Skola skola )
		{
			if ( Upgrader.runUpgrade( strFolder ) )
				FMain.theOneForm.endApp( false );


			bool fSuccess;
			ContentOfOrderFile coof;
			if ( investigateFolder( strFolder, out coof ) )
				if ( _fImport )
					fSuccess = coof.importOrder( skola );
				else
					fSuccess = coof.createOrder( skola );
			else
				fSuccess = false;

			if (!fSuccess)
				return Z.HitMeAgain;

			return Z.ReturnTrue;
		}

		private bool investigateFolder(
			string strFolder,
			out ContentOfOrderFile coof )
		{
			bool f = investigateFolder_logged( strFolder, out coof );
			_sbLog.AppendFormat( "investigateFolder, koll av \"{0}\" gav \"{1}\"\r\n",
				strFolder,
				f );
			return f;
		}

		private bool investigateFolder_logged(
			string strFolder,
			out ContentOfOrderFile coof )
		{
			ArrayList al = new ArrayList();

			coof = new ContentOfOrderFile();

			foreach ( string strFN in Directory.GetFiles( strFolder, "order_*.*" ) )
				switch ( Path.GetExtension( strFN ).ToLower() )
				{
					case ".xml":
					case ".zip":
					case ".plorund":
						if ( Regex.IsMatch( strFN, @"\\order_\d{5,6}\.|_", RegexOptions.IgnoreCase ) )
						{
							_sbLog.AppendFormat( "found: {0}\r\n", strFN );
							al.Add( strFN.ToLower() );
						}
						break;
				}

			string strFil = null;

			switch ( al.Count )
			{
				case 0:
					return false;
				case 1:
					strFil = al[0] as string;
					break;
				default:
					foreach ( string s in al )
						if ( Path.GetFileName( s ).CompareTo( _strFN ) == 0 )
							strFil = s;
					if ( strFil==null )
						if ( FSelectOrder.showDialog( this.FindForm(), al, out strFil ) != DialogResult.OK )
							return false;
					break;
			}

			switch ( Path.GetExtension( strFil ) )
			{
				case ".zip":
				case ".plorund":
					// en ZIP-fil vald
					coof.loadFromZipFile( File.ReadAllBytes( strFil ) );
					return coof.getFileWithType(ContentOfOrderFile.FileType.OrderXml)!=null;
			}

			// en XML-fil vald
			coof.Files.Add( new ContentOfOrderFile.File(
				ContentOfOrderFile.FileType.OrderXml,
				strFil,
				File.ReadAllBytes( strFil ) ) );
			strFil = Path.ChangeExtension( strFil, ".emf" );
			if ( File.Exists( strFil ) )
				coof.Files.Add( new ContentOfOrderFile.File(
					ContentOfOrderFile.FileType.OrderEmf,
					strFil,
					File.ReadAllBytes( strFil ) ) );
			return true;
		}

		private void cmdBrowse_Click( object sender, EventArgs e )
		{
			using ( OpenFileDialog dlg = new OpenFileDialog() )
			{
				dlg.CheckFileExists = true;
				dlg.DefaultExt = ".zip";
				dlg.Filter = "Orderfil|order_*.zip;order_*.xml;order_*.plorund";
				dlg.FileName = Path.Combine( txtOrder.Text, "order_*.zip" );
				if ( dlg.ShowDialog( this.FindForm() ) == DialogResult.OK )
				{
					txtOrder.Text = Path.GetDirectoryName( dlg.FileName );
					_strFN = Path.GetFileName( dlg.FileName );
					fireExecute();
				}

			}
		}

		private void txtOrder_TextChanged(object sender, EventArgs e)
		{
			optUseFolder.Checked = true;
		}

	}

}
