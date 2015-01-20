using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Plata
{
	public partial class tabPageAnmärkningar : UserControl, IBSTab
	{
		public tabPageAnmärkningar()
		{
			InitializeComponent();
		}

		void IBSTab.load()
		{
			txtAnmFoto.Text = Global.Skola.Anmärkningar[PlataDM.Anmärkningar.Typ.Foto];
			txtAnmBok.Text = Global.Skola.Anmärkningar[PlataDM.Anmärkningar.Typ.BS];
			txtAnmKatalog.Text = Global.Skola.Anmärkningar[PlataDM.Anmärkningar.Typ.Katalog];
			txtAnmDiverse.Text = Global.Skola.Anmärkningar[PlataDM.Anmärkningar.Typ.Övrigt];
		}

		void IBSTab.save()
		{
			Global.Skola.Anmärkningar[PlataDM.Anmärkningar.Typ.Foto] = txtAnmFoto.Text;
			Global.Skola.Anmärkningar[PlataDM.Anmärkningar.Typ.BS] = txtAnmBok.Text;
			Global.Skola.Anmärkningar[PlataDM.Anmärkningar.Typ.Katalog] = txtAnmKatalog.Text;
			Global.Skola.Anmärkningar[PlataDM.Anmärkningar.Typ.Övrigt] = txtAnmDiverse.Text;
		}

		protected override void  OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.DrawString( "TILL FOTOSUPPORT", this.Font, SystemBrushes.WindowText, 8, txtAnmFoto.Top-20 );
			e.Graphics.DrawString( "TILL BOKNINSSERVICE/SÄLJARE", this.Font, SystemBrushes.WindowText, 8, txtAnmBok.Top - 20 );
			e.Graphics.DrawString( "TILL KATALOG/LABB", this.Font, SystemBrushes.WindowText, 8, txtAnmKatalog.Top - 20 );
			e.Graphics.DrawString( "ÖVRIGT PRODUKTION", this.Font, SystemBrushes.WindowText, 8, txtAnmDiverse.Top - 20 );
		}

		protected override void OnResize( EventArgs e )
		{
			base.OnResize( e );
			Size sz = this.ClientSize;
			int nHTot = (sz.Height - 24) / 4;
			int nHTxt = nHTot - 24;
			int nY = 24;

			txtAnmFoto.Bounds = new Rectangle( 0, nY, sz.Width, nHTxt );
			txtAnmBok.Bounds = new Rectangle( 0, nY += nHTot, sz.Width, nHTxt );
			txtAnmKatalog.Bounds = new Rectangle( 0, nY += nHTot, sz.Width, nHTxt );
			txtAnmDiverse.Bounds = new Rectangle( 0, nY += nHTot, sz.Width, nHTxt );
		}

	}

}
