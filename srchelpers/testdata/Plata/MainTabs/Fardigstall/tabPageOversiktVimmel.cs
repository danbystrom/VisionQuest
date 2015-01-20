using Photomic.Common;

namespace Plata
{
	public partial class tabPageÖversiktVimmel : usrBildÖversikt, IBSTab
	{

		public tabPageÖversiktVimmel()
		{
			InitializeComponent();
		}

		void IBSTab.load()
		{
			if ( Global.Skola == null )
				return;
			initialize(
				FlikKategori.Annat,  // vill INTE ha merfunktionalitet för vimmel här!!!
				5 );
		}

		void IBSTab.save()
		{
		}

		public override void reload()
		{
			base.reload();
			foreach ( PlataDM.Vimmelbild vb in Global.Skola.Vimmel )
				if ( vb.Status == VimmelStatus.Vald || vb.Status == VimmelStatus.Lovad )
					addItem(
						vb.Thumbnail.FilenameJpg,
						vb.Thumbnail.FilenameJpg,
                        vb,
						vb.Status == VimmelStatus.Lovad ? "LOVAD" : "",
                        false);
		}

	}

}
