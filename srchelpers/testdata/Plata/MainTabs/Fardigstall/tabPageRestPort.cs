using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Plata
{
	public partial class tabPageRestPort : UserControl, IBSTab
	{
		public tabPageRestPort()
		{
			InitializeComponent();
		}

		void IBSTab.load()
		{
			var fontBold = new Font( lvwRestPort.Font, FontStyle.Bold );
			ListViewItem itmG, itmP;

			lvwRestPort.Items.Clear();
			foreach ( var grupp in Global.Skola.Grupper )
			{
				itmG = new ListViewItem( grupp.Namn );
				itmG.Font = fontBold;
				itmG.Tag = new string[] { grupp.Namn, string.Empty };
				foreach ( var person in grupp.AllaPersoner )
					if ( !person.HasPhoto && !person.Personal )
					{
						if ( itmG != null )
						{
							lvwRestPort.Items.Add( itmG );
							itmG = null;
						}
						itmP = lvwRestPort.Items.Add( string.Empty );
						itmP.SubItems.Add( person.Namn );
						itmP.Tag = new string[] { grupp.Namn, person.Namn };
					}
			}
		}

		void IBSTab.save()
		{
		}

	}

}
