using System;
using System.Text;
using System.Windows.Forms;

namespace Plata
{
	public interface IBSTab
	{
		void load();
		void save();
	}

	public class BSTabInfo
	{
		public readonly TabPage TabPage;
		public IBSTab BSTab;
		public BSTabInfo( string strTitle, IBSTab bstab )
		{
			this.TabPage = new TabPage2();
			this.TabPage.Text = strTitle;
			BSTab = bstab;
			UserControl uc = bstab as UserControl;
			uc.Dock = DockStyle.Fill;
			this.TabPage.Controls.Add( uc );
		}

		public void save()
		{
			try
			{
				if ( BSTab != null )
					BSTab.save();
			}
			catch ( Exception ex )
			{
				Global.showMsgBox( TabPage.FindForm(), ex.ToString() );
			}
		}

		public void load()
		{
			try
			{
				if ( BSTab != null )
					BSTab.load();
			}
			catch ( Exception ex )
			{
				Global.showMsgBox( TabPage.FindForm(), ex.ToString() );
			}
		}

		private class TabPage2 : TabPage
		{
			protected override void OnPaintBackground( PaintEventArgs pevent )
			{
				if ( Controls.Count == 0 )
					base.OnPaintBackground( pevent );
			}
		}
	}

}
