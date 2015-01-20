using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Photomic.Common;

namespace Plata
{
	/// <summary>
	/// Summary description for FSelectPerson.
	/// </summary>
	public class FCopyOrderFile : vdUsr.baseGradientForm
	{
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
		private Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private FCopyOrderFile()
		{
			InitializeComponent();
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( FCopyOrderFile ) );
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// cmdOK
			// 
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point( 111, 80 );
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size( 80, 28 );
			this.cmdOK.TabIndex = 2;
			this.cmdOK.Text = "OK";
			this.cmdOK.Click += new System.EventHandler( this.cmdOK_Click );
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point( 218, 80 );
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size( 80, 28 );
			this.cmdCancel.TabIndex = 3;
			this.cmdCancel.Text = "Avbryt";
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label1.Location = new System.Drawing.Point( 12, 9 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 380, 57 );
			this.label1.TabIndex = 4;
			this.label1.Text = resources.GetString( "label1.Text" );
			// 
			// FCopyOrderFile
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size( 404, 120 );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.cmdCancel );
			this.Controls.Add( this.cmdOK );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FCopyOrderFile";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Kopiera orderfil till annan Plåta";
			this.ResumeLayout( false );

		}
		#endregion

		public static DialogResult showDialog(
			Form parent )
		{
			using ( FCopyOrderFile dlg = new FCopyOrderFile() )
				return dlg.ShowDialog(parent);
		}


		private void cmdOK_Click(object sender, EventArgs e)
		{
			try
			{

			using ( SaveFileDialog sfd = new SaveFileDialog() )
			{
				sfd.Title = "Spara orderfil";
				sfd.Filter = "Plåta orderunderlag|*.zip";
				sfd.FileName = string.Format( "order_{0}_{1}.zip", Global.Skola.OrderNr, Global.skapaSäkertFilnamn(Global.Skola.Namn) );
				sfd.CheckPathExists = true;
				if ( sfd.ShowDialog( this ) == DialogResult.OK )
				{
					if ( Global.Fotografdator && string.Compare( sfd.FileName, 0, Global.Preferences.MainPath, 0, 3, true ) == 0 )
					{
						Global.showMsgBox( this, "Du måste ange en annan enhet!" );
						return;
					}
					File.WriteAllBytes(
						sfd.FileName,
						createOrderFile( Global.Skola ) );
				}
				Global.showMsgBox(this, "Klart");
			}
			}
			catch (Exception ex)
			{
				Global.showMsgBox(this, "Det gick inte att kopiera filerna, följande fel inträffade:\r\n\r\n{0}", ex.ToString());
			}

		}

		public static byte[] createOrderFile( PlataDM.Skola sk )
		{
			List<File2Store> list = new List<File2Store>();
			list.Add( new File2Store(
				string.Format( "order_{0}.xml", sk.OrderNr ),
				System.Text.Encoding.UTF8.GetBytes( createXMLFile( sk ) ) ) );

			string strEMF = Global.Skola.HomePathCombine( "!fotoorder.emf" );
			if ( File.Exists( strEMF ) )
				list.Add( new File2Store(
					string.Format( "order_{0}.emf", sk.OrderNr ),
					File.ReadAllBytes( strEMF ) ) );

			string strSCF = Path.Combine(
				sk.HomePath,
				"StudentCard" );
			int nStudentCardTemplates = 0;
			if ( Directory.Exists( strSCF ) )
				for ( int i = 1 ; ; i++ )
				{
					string strXML = Path.Combine( strSCF, string.Format( "{0}.xml", i ) );
					string strJPG = Path.Combine( strSCF, string.Format( "{0}.jpg", i ) );
					if ( !File.Exists( strXML ) || !File.Exists( strJPG ) )
						break;
					list.Add( new File2Store(
						strXML.Substring( 1 + sk.HomePath.Length ),
						File.ReadAllBytes( strXML ) ) );
					list.Add( new File2Store(
						strJPG.Substring( 1 + sk.HomePath.Length ),
						File.ReadAllBytes( strJPG ) ) );
					nStudentCardTemplates++;
				}

			return createTheZip(
				string.Format( "order_{0}.plorund", sk.OrderNr ),
				list );
		}

		private static byte[] createTheZip(
			string strWrapperName,
			List<File2Store> files )
		{
			byte[] abyteFile = createTheZip_2( files );

			using ( MemoryStream ms = new MemoryStream() )
			using ( ZipOutputStream zos = new ZipOutputStream( ms ) )
			{
				zos.SetLevel( 9 ); // 0 - store only to 9 - means best compression

				ZipEntry entry = new ZipEntry( strWrapperName );
				entry.DateTime = DateTime.Now;
				zos.PutNextEntry( entry );
				zos.Write( abyteFile, 0, abyteFile.Length );
				zos.Finish();
				abyteFile = new byte[ms.Length];
				Array.Copy( ms.GetBuffer(), abyteFile, abyteFile.Length );
			}
			return abyteFile;
		}

		private static byte[] createTheZip_2( List<File2Store> files )
		{
			byte[] abyteFile;

			using ( MemoryStream ms = new MemoryStream() )
			using ( ZipOutputStream zos = new ZipOutputStream( ms ) )
			{
				zos.SetLevel( 9 ); // 0 - store only to 9 - means best compression
				foreach ( File2Store f2s in files )
				{
					ZipEntry entry = new ZipEntry( f2s.Filename );
					entry.DateTime = DateTime.Now;
					zos.PutNextEntry( entry );

					zos.Write( f2s.Data, 0, (int)f2s.Data.Length );
				}
				zos.Finish();
				abyteFile = new byte[ms.Length];
				Array.Copy( ms.GetBuffer(), abyteFile, abyteFile.Length );
			}
			return abyteFile;
		}

		private static string createXMLFile( PlataDM.Skola sk )
		{
			var sx = new vdUsr.vdSimpleXMLWriter();

			sx.descend("ORDER");
			sx.writeValue("namn", sk.Namn);
			sx.writeValue("ort", sk.Ort);
			sx.writeValue("ordernr", sk.OrderNr);
			sx.writeValue("skapad", vdUsr.DateHelper.YYYYMMDD(Global.Now));
			sx.writeValue( "companyorder", ((int)sk.CompanyOrder).ToString() );
			sx.writeValue("typeoforder", (int)sk.TypeOfOrder);
			sx.writeValue("restp", (int)sk.RestfotoPorträtt);
			sx.writeValue("restg", (int)sk.RestfotoGrupp);

			sx.writeValue("skaha_photoarkiv", sk.ShallBurnPhotoCD);
			if (sk.ShallBurnProgCD)
			{
				sx.writeValue("skaha_progarkiv", true);
				sx.writeValue("progcd_format", (int)sk.CustomProgFormat);
				sx.writeValue("progcd_width", sk.CustomProgWidth);
				sx.writeValue("progcd_height", sk.CustomProgHeight);
				sx.writeValue("progcd_name", sk.CustomProgNaming);
			}

			foreach (PlataDM.Grupp grupp in sk.Grupper)
			{
				sx.descend("GRUPP");
				sx.writeValue("namn", grupp.Namn);
				sx.writeValue("packorder", grupp.PackOrder);
				sx.writeValue("grupptyp", (int)grupp.GruppTyp);
				sx.writeValue("guid", grupp.Id);
				foreach (PlataDM.Person pers in grupp.AllaPersoner)
				{
					sx.descend("PERSON");
					for (int i = 0; i < PersonInfo.XMLTags.Length; i++)
					{
						string s = pers.getInfo( (PersonInfo.Info)i );
						if (!string.IsNullOrEmpty(s))
							sx.writeValue( PersonInfo.XMLTags[i], s );
					}
					sx.writeValue( "filmnummer", pers.ScanCode, "" );
					sx.writeValue( "kon", pers.Kön );
					sx.writeValue( "id", pers.ID );
					sx.ascend();
				}
				sx.ascend();
			}

			sx.ascend();

			return sx.endSaveXML();
		}

		private class File2Store
		{
			public string Filename;
			public byte[] Data;
			public File2Store( string filename, byte[] data )
			{
				Filename = filename;
				Data = data;
			}
		}

	}

}
