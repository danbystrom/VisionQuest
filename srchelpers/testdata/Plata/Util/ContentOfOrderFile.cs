using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace Plata
{
	public class ContentOfOrderFile
	{
		public enum FileType
		{
			OrderXml,
			OrderEmf,
			StudentCard
		}

		public class File
		{
			public readonly FileType FileType;
			public readonly string Filename;
			public readonly byte[] Content;

			public File( 
				FileType filetype,
				string filename, 
				byte[] content )
			{
				FileType = filetype;
				Filename = filename;
				Content = content;
			}

			public File(
				FileType filetype,
				string filename,
				Stream stream,
				int length )
			{
				FileType = filetype;
				Filename = filename;
				Content = new byte[length];
				stream.Read( Content, 0, length );
			}

			public string contentAsString
			{
				get
				{
					using ( var ms = new MemoryStream( Content ) )
					using ( var sr = new StreamReader( ms ) )
						return sr.ReadToEnd();
				}
			}

		}

		public readonly List<File> Files = new List<File>();

		public File getFileWithType( FileType filetype )
		{
			foreach ( File file in Files )
				if ( file.FileType == filetype )
					return file;
			return null;
		}

		public void loadFromZipFile(
			byte[] abZipFileContent )
		{
			using ( Stream stream = new MemoryStream( abZipFileContent ) )
				loadFromZipFile( stream );
		}

		public void loadFromZipFile(
			Stream stream )
		{
			ZipEntry theEntry;

			using ( var zis = new ZipInputStream( stream ) )
				while ( (theEntry = zis.GetNextEntry()) != null )
				{
					var strFN = theEntry.Name.ToLower();
					if ( Path.GetFileNameWithoutExtension( strFN ).StartsWith( "order_" ) )
						switch ( Path.GetExtension( strFN ) )
						{
							case ".xml":
								Files.Add( new File(
									FileType.OrderXml,
									strFN,
									zis,
									(int)zis.Length ) );
								break;
							case ".emf":
								Files.Add( new File(
									FileType.OrderEmf,
									strFN,
									zis,
									(int)zis.Length ) );
								break;
							case ".zip":
							case ".plorund":
								{
									var ab = new byte[zis.Length];
									zis.Read( ab, 0, (int)zis.Length );
									loadFromZipFile( ab );
									return;
								}
						}
					else if ( strFN.StartsWith( "studentcard" ) )
						Files.Add( new File(
							FileType.StudentCard,
							strFN,
							zis,
							(int)zis.Length ) );
				}
		}

		private void storeStudentCardInfo(
			string strFolder )
		{
			foreach ( var file in Files )
				if ( file.FileType == FileType.StudentCard )
				{
					if ( !Directory.Exists( strFolder ) )
						Directory.CreateDirectory( strFolder );
					System.IO.File.WriteAllBytes(
						Path.Combine( strFolder, Path.GetFileName( file.Filename ) ),
						file.Content );
				}
		}

		public bool importOrder( PlataDM.Skola skola )
		{
			return skola.openXML(
				getFileWithType(FileType.OrderXml).contentAsString, true ) ;
		}

		public bool createOrder( PlataDM.Skola skola )
		{
			var filXML = getFileWithType( FileType.OrderXml );
			var filEMF = getFileWithType( FileType.OrderEmf );

			if ( filXML == null )
				return false;
			if ( !skola.createFromOrder(
					filXML.contentAsString,
					filEMF != null ? filEMF.Content : null,
					Global.Preferences.MainPath ) )
				return false;

			storeStudentCardInfo(
				Path.Combine( skola.HomePath, "StudentCard" ) );

			Fotografer.appendFromOrder( skola );

			return true;
		}

	}
}
