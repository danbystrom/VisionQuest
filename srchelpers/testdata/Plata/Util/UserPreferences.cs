using System;
using System.Collections;
using System.IO;

namespace Plata
{

	public enum CameraSdk
	{
		Null,
		NoCamera,
        FujiCamera,
		Ed207Camera,
        Ed210Camera,
	}

	public enum ImageSortOrder
	{
		None,
		NewestFirst,
		OldestFirst,
	}

	public class UserPreferences : PlataDM.IvdPersistable
	{
		private string _strGroupSound;
		private string _strPortraitSound;

		public bool SortOrderLastName = false;

		public string MainPath;
		public string BackupFolder;
		public string AutoUpdateFolder;
		public PlataDM.Rotering Porträttrotering = PlataDM.Rotering.Medurs;
		public string InternalPhotographerFolder;
		public string InternalPhotoWorkFolder;
		public string LastImportFolder;
		public string OpenOrderFolder;
		public bool UseOpenOrderFolder;

		public ArrayList SenasteGranskningPath = new ArrayList();
		public ArrayList listTitlarEgna = new ArrayList();

		public bool FullskärmslägeGruppfoto;
		public bool FullskärmslägePorträttfoto;

		public int Fotografnummer;
		public CameraSdk Camera;
		public ImageSortOrder ImageSortOrder;

		public string FakeCDPath;

		public int FTPSpeed;

	    public Brand Brand;
 
		public string GroupSoundShort
		{
			get { return _strGroupSound; }
			set { _strGroupSound = Path.GetFileNameWithoutExtension(value); }
		}

		public string PortraitSoundShort
		{
			get { return _strPortraitSound; }
			set { _strPortraitSound = Path.GetFileNameWithoutExtension(value); }
		}

		public string GroupSoundLong
		{
			get { return getSoundLong(_strGroupSound,false); }
			set { _strGroupSound = Path.GetFileNameWithoutExtension(value); }
		}

		public string PortraitSoundLong
		{
			get { return getSoundLong(_strPortraitSound,false); }
			set { _strPortraitSound = Path.GetFileNameWithoutExtension(value); }
		}

		public static string getSoundLong( string strFN, bool fReturnPathOnly )
		{
			string strPath = Global.getAppPath( "ljud" );
			if ( fReturnPathOnly )
				return strPath;
			if ( !string.IsNullOrEmpty(strFN) )
				return Path.Combine( strPath, strFN + ".mp3" );
		    return null;
		}

		public void loadXML( string strXML )
		{
			if ( !string.IsNullOrEmpty(strXML) )
				try
				{
					var p = new PlataDM.vdPersist();
					p.beginLoadXML( strXML );
					x(p);
				}
				catch ( Exception )
				{
				}
		}

		public string saveXML()
		{
			var p = new PlataDM.vdPersist();
			p.beginSave();
			x(p);
			return p.endSaveXML();
		}

		private void x( PlataDM.vdPersist po )
		{
			po.x( "USERPREFERENCES", this );
		}

		void PlataDM.IvdPersistable.Persist(PlataDM.vdPersist po)
		{
			po.x( "fotografnummer", ref Fotografnummer );
			po.x( "mainpath", ref MainPath );
			po.x( "backupfolder", ref BackupFolder );
			po.x( "autoupdatefolder", ref AutoUpdateFolder, string.Empty );
			po.x( "lastimportfolder", ref LastImportFolder );
			po.x( "inhousephotoworkfolder", ref InternalPhotoWorkFolder );
			po.x( "inhousephotographerfolder", ref InternalPhotographerFolder );
			po.x( "openorderfolder", ref OpenOrderFolder );
			po.x( "useopenorderfolder", ref UseOpenOrderFolder );

			po.x( "groupsound", ref _strGroupSound );
			po.x( "portsound", ref _strPortraitSound );

			po.x( "sortorderlastname", ref SortOrderLastName );

			po.x( "fullgrupp", ref FullskärmslägeGruppfoto );
			po.x( "fullport", ref FullskärmslägePorträttfoto );
			Porträttrotering = (PlataDM.Rotering)po.xenum( "portrot", (int)Porträttrotering, (int)PlataDM.Rotering.Medurs );
			Camera = (CameraSdk)po.xenum("camera", (int)Camera );
			ImageSortOrder = (ImageSortOrder)po.xenum( "imagesortorder", (int)ImageSortOrder );

			po.x( "ftpspeed", ref FTPSpeed );
		    Brand = (Brand) po.xenum("brand", (int) Brand);

			po.x( "fakecdpath", ref FakeCDPath, @"c:\" );

			if ( po.isLoading )
			{
				po.descendCollection( "VIEW" );
				while ( po.nextInCollection() )
					SenasteGranskningPath.Add( po.getValueAsString( "path" ) );
				po.descendCollection( "TITLE" );
				while ( po.nextInCollection() )
					listTitlarEgna.Add( po.getValueAsString( "name" ) );
				}
			else
			{
				foreach ( string s in SenasteGranskningPath )
				{
					po.descend( "VIEW" );
					po.writeValue( "path", s );
					po.ascend();
				}
				foreach ( string s in listTitlarEgna )
				{
					po.descend( "TITLE" );
					po.writeValue( "name", s );
					po.ascend();
				}
			}

		}

	}

}
