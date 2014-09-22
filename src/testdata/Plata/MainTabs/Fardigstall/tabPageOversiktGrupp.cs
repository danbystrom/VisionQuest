using Photomic.Common;

namespace Plata
{
	public partial class tabPageÖversiktGrupp : usrBildÖversikt, IBSTab
	{

		public tabPageÖversiktGrupp()
		{
			InitializeComponent();
		}

		void IBSTab.load()
		{
			if ( Global.Skola == null )
				return;
			initialize(
				FlikKategori.Gruppbild,
				5 );
		}

		void IBSTab.save()
		{
		}

		public override void reload()
		{
			base.reload();
			foreach ( PlataDM.Grupp grupp in Global.Skola.Grupper.GrupperIOrdning() )
				addItem(
					grupp.getViewImageFileName( TypeOfViewImage.LoResPreferred ),
					grupp.getViewImageFileName( TypeOfViewImage.HiResPreferred ),
                    grupp,
					grupp.Namn,
                    false);
		}

	}

}
