using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Plata.Dialogs
{
	public partial class FHistogram : Form
	{
		public FHistogram( Point p )
		{
			InitializeComponent();
			p.Offset( -this.Width, 0 );
			this.Location = p;
		}

		public void setData( Bitmap bmp )
		{
			usrExifAndHistogram1.setData( bmp, bmp, false );
		}

	}
}
