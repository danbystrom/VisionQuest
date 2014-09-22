using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Plata
{
	public partial class tabPageRestGrupp : UserControl, IBSTab
	{
		public tabPageRestGrupp()
		{
			InitializeComponent();
		}

		void IBSTab.load()
		{
			Font fontBold = new Font( lvwRestGrupp.Font, FontStyle.Bold );
			ListViewItem itm;

			lvwRestGrupp.Items.Clear();
			foreach ( PlataDM.Grupp grupp in Global.Skola.Grupper )
				if ( grupp.PersonerFrånvarande.Count >= 1 )
				{

					itm = lvwRestGrupp.Items.Add( grupp.Namn );
					itm.Font = fontBold;
					itm.Tag = new string[] { grupp.Namn, string.Empty };
					foreach ( PlataDM.Person person in grupp.PersonerFrånvarande )
					{
						itm = lvwRestGrupp.Items.Add( "" );
						itm.SubItems.Add( person.Namn );
						itm.Tag = new string[] { grupp.Namn, person.Namn };
					}
				}
		}

		void IBSTab.save()
		{
		}

	}
}
