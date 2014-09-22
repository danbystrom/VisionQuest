using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Plata
{
	public partial class tabPageStatistics : UserControl, IBSTab
	{
		public tabPageStatistics()
		{
			InitializeComponent();
		}

		void IBSTab.load()
		{
			int nAntal, nLovade, nValda;
			Global.Skola.Vimmel.räkna( out nAntal, out nLovade, out nValda );
			lvVimmel.Items[0].SubItems[1].Text = nAntal.ToString();
			lvVimmel.Items[1].SubItems[1].Text = nLovade.ToString();
			lvVimmel.Items[2].SubItems[1].Text = nValda.ToString();

			int nTotalt = 0, nVarav = 0;
			foreach ( PlataDM.Grupp grupp in Global.Skola.Grupper )
				foreach ( PlataDM.Person pers in grupp.AllaPersoner )
				{
					nTotalt += pers.Thumbnails.Count;
					if ( !string.IsNullOrEmpty( pers.ThumbnailKey ) )
						nVarav++;
				}
			lvStatistik.Items[0].SubItems[1].Text = nTotalt.ToString();
			lvStatistik.Items[1].SubItems[1].Text = nVarav.ToString();
		}

		void IBSTab.save()
		{
		}

	}

}
