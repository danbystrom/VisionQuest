using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Photomic.Common;
using Xceed.Grid;

namespace Plata
{
	/// <summary>
	/// Summary description for PlataDM.Util.
	/// </summary>
	public class Util
	{
		public static StringFormat sfUL = new StringFormat();
		public static StringFormat sfUC = new StringFormat();
		public static StringFormat sfUR = new StringFormat();
		public static StringFormat sfML = new StringFormat();
		public static StringFormat sfMC = new StringFormat();
		public static StringFormat sfMR = new StringFormat();
		public static StringFormat sfLL = new StringFormat();
		public static StringFormat sfLC = new StringFormat();
		public static StringFormat sfLR = new StringFormat();

		static Util()
		{
			sfUL.Alignment = StringAlignment.Near;
			sfUL.LineAlignment = StringAlignment.Near;
			sfUC.Alignment = StringAlignment.Center;
			sfUC.LineAlignment = StringAlignment.Near;
			sfUR.Alignment = StringAlignment.Far;
			sfUR.LineAlignment = StringAlignment.Near;

			sfML.Alignment = StringAlignment.Near;
			sfML.LineAlignment = StringAlignment.Center;
			sfMC.Alignment = StringAlignment.Center;
			sfMC.LineAlignment = StringAlignment.Center;
			sfMR.Alignment = StringAlignment.Far;
			sfMR.LineAlignment = StringAlignment.Center;

			sfLL.Alignment = StringAlignment.Near;
			sfLL.LineAlignment = StringAlignment.Far;
			sfLC.Alignment = StringAlignment.Center;
			sfLC.LineAlignment = StringAlignment.Far;
			sfLR.Alignment = StringAlignment.Far;
			sfLR.LineAlignment = StringAlignment.Far;
		}

		public static int checksumDigit( string strCode )
		{
			int nSum = 0;
			for ( int i = 0, nT = 1 ; i < strCode.Length ; i++, nT = 3 - nT )
			{
				int nNumber = (int)(strCode[i] - '0') * nT;
				nSum += (nNumber / 10) + (nNumber % 10);
			}
			return (10 - nSum % 10) % 10;
		}

		public static bool verifyScanCode( string strCode )
		{
			return
				checksumDigit( strCode.Substring( 0, strCode.Length - 1 ) ) ==
				(int)(strCode[strCode.Length - 1] - '0');
		}

		public static void drawEllipseButton(
			Graphics g,
			Rectangle rect,
			Color colorDark,
			Color colorLight,
			Color colorTextNotSelected,
			Color colorTextSelected,
			Font font,
			string strText,
			bool fPressed )
		{
			if ( fPressed )
			{
				using ( var br = new SolidBrush(colorLight) )
					g.FillEllipse( br, rect );
				using ( var pen = new Pen(colorDark) )
					g.DrawArc( pen, rect, 135, 180 );
			}
			else
			{
				using ( var pen = new Pen(colorDark) )
					g.DrawArc( pen, rect, -45, 180 );
				using ( var pen = new Pen(colorLight) )
					g.DrawArc( pen, rect, 135, 180 );
			}

			if ( strText!=null )
				using ( Brush br = new SolidBrush(fPressed?colorTextSelected:colorTextNotSelected) )
					g.DrawString( strText, font, br, rect, sfMC );
		}

		static public void paintBox( Graphics g, string S, Font font, int x, int y, int w, int h )
		{
			g.FillRectangle( Brushes.WhiteSmoke, x, y, w, h );
			g.FillRectangle( Brushes.Gray, x+w, y+10, 10, h );
			g.FillRectangle( Brushes.Gray, x+10, y+h, w, 10 );
			if ( S!=null && font!=null )
				g.DrawString( S, font, Brushes.Black, x, y );
		}

		static public bool safeKillFile( string fn )
		{
			if ( !isEmpty(fn) )
				try 
				{
					if ( File.Exists(fn) )
						File.Delete( fn );
				}
				catch
				{
					return false;
				}
			return true;
		}

		static public bool safeKillDirectory( string strDir )
		{
			if ( !isEmpty(strDir) )
				try 
				{
					if ( Directory.Exists(strDir) )
						Directory.Delete( strDir, true );
				}
				catch
				{
					return false;
				}
			return true;
		}

		static public string[] safeGetDirectories( string strPath )
		{
			try
			{
				if ( Directory.Exists( strPath ) )
					return Directory.GetDirectories(strPath);
			}
			catch
			{
			}
			return new string[0];
		}

		static public bool safeSelectComboItem( ComboBox cbo, object obj, bool fSelectFirstIfNoMatch )
		{
			if ( obj!=null && cbo.Items.Contains(obj) )
				cbo.SelectedItem = obj;
			else if ( fSelectFirstIfNoMatch && cbo.Items.Count>=1 )
				cbo.SelectedIndex = 0;
			return cbo.SelectedItem!=null;
		}

		static public bool isEmpty( string s )
		{
			return s==null || s.Length==0;
		}

		static public string properCase( string s )
		{
			if ( isEmpty(s) )
				return s;
			string strUpper = s.ToUpper();
			char[] acRes = s.ToLower().ToCharArray();
			for ( int i=acRes.Length-1 ; i>0 ; i-- )
				if ( acRes[i-1]<'A' )
					acRes[i] = strUpper[i];
			acRes[0] = strUpper[0];
			return new string(acRes);
		}

		static public void UniteRectangle( ref Rectangle r, Rectangle r2 )
		{
			int nLeft = Math.Min( r.Left, r2.Left );
			int nTop = Math.Min( r.Top, r2.Top );
			int nRight = Math.Max( r.Right, r2.Right );
			int nBottom = Math.Max( r.Bottom, r2.Bottom );
			r = new Rectangle( nLeft, nTop, nRight-nLeft, nBottom-nTop );
		}

		static public void setPictureBoxImageWithDispose( PictureBox picBox, Image imgNew )
		{
			var imgOld = picBox.Image;
			picBox.Image = imgNew;
			if ( imgOld!=null )
				imgOld.Dispose();
		}

		public struct SystemPowerStatus
		{
			public byte ACLineStatus;
			public byte batteryFlag;
			public byte batteryLifePercent;
			public byte reserved1;
			public int  batteryLifeTime;
			public int  batteryFullLifeTime;
		}
		[DllImport("kernel32.dll")]
		public static extern bool GetSystemPowerStatus( out SystemPowerStatus systemPowerStatus );

		static public Image selectLargestPage( Image img )
		{
			if ( img==null )
				return null;
			int nLargestIndex = 0;
			int nLargestSize = 0;
			int nCount = img.GetFrameCount( FrameDimension.Page );
			if ( nCount==1 )
				return img;
			for ( var i=0 ; i<nCount ; i++ )
			{
				img.SelectActiveFrame( FrameDimension.Page, i );
				if ( img.Size.Width > nLargestSize )
				{
					nLargestSize = img.Size.Width;
					nLargestIndex = i;
				}
			}
			img.SelectActiveFrame( FrameDimension.Page, nLargestIndex );
			return img;
		}

		public static void paintComboBoxGroup(
			ComboBox cbo,
			System.Windows.Forms.DrawItemEventArgs e )
		{
			string strText;
			var clr = e.ForeColor;
			var rectText = e.Bounds;
			PlataDM.Grupp grupp = null;

			e.DrawBackground();

			if ( e.Index >= 0 )
				grupp = cbo.Items[e.Index] as PlataDM.Grupp;

			if ( grupp != null )
			{
				if ( (e.State & DrawItemState.ComboBoxEdit) == 0 )
				{
					if ( grupp.GruppTyp != GruppTyp.GruppNormal )
						if ( (e.State & DrawItemState.Focus) == 0 )
							clr = SystemColors.Highlight;
					rectText = new Rectangle(
						e.Bounds.Left + e.Bounds.Height, e.Bounds.Top,
						e.Bounds.Width - e.Bounds.Height, e.Bounds.Height );

					PlataDM.Util.paintGroupNumberingSymbol(
						e.Graphics,
						e.Font,
						grupp,
						new Rectangle( e.Bounds.Left + 2, e.Bounds.Top, e.Bounds.Height - 2, e.Bounds.Height - 2 ) );
				}

				var s = string.Empty;
                if ((grupp.Special & TypeOfGroupPhoto.Gruppbild) != 0)
					s += "G";
                if ((grupp.Special & TypeOfGroupPhoto.Katalog) != 0)
					s += "K";
                if ((grupp.Special & TypeOfGroupPhoto.SkyddadId) != 0)
					s += "S";
                if ((grupp.Special & TypeOfGroupPhoto.Spex) != 0)
					s += "P";
				e.Graphics.DrawString( s, SystemFonts.IconTitleFont, Brushes.White, rectText.X+1, rectText.Y+1 );
				e.Graphics.DrawString( s, SystemFonts.IconTitleFont, Brushes.LightBlue, rectText.X, rectText.Y );
				rectText.Offset( 20, 0 );
				rectText.Width -= 20;

				strText = grupp.Namn;
				if ( grupp.Makulerad )
					strText = "mak: " + strText;

			}
			else
				strText = "(välj grupp)";

			using ( Brush br = new SolidBrush( clr ) )
				e.Graphics.DrawString( strText, e.Font, br, rectText );
			e.DrawFocusRectangle();
		}

		public static void saveMultiframeTIF( string strFN, Bitmap bmpLarge, Bitmap bmpSmall )
		{
			ImageCodecInfo myImageCodecInfo;
			Encoder myEncoder;
			EncoderParameter myEncoderParameter;
			EncoderParameters myEncoderParameters;

			// Get an ImageCodecInfo object that represents the TIFF codec.
			myImageCodecInfo = GetEncoderInfo("image/tiff");
			// Create an Encoder object based on the GUID
			// for the SaveFlag parameter category.
			myEncoder = Encoder.SaveFlag;
			// Create an EncoderParameters object.
			// An EncoderParameters object has an array of EncoderParameter
			// objects. In this case, there is only one
			// EncoderParameter object in the array.

			myEncoderParameters = new EncoderParameters(1);
			// Save the first page (frame).
			myEncoderParameter = new EncoderParameter( myEncoder, (long)EncoderValue.MultiFrame );
			myEncoderParameters.Param[0] = myEncoderParameter;

			Image imgFile = new Bitmap( bmpSmall );
			imgFile.Save( strFN, myImageCodecInfo, myEncoderParameters);

			// Save the second page (frame).
			myEncoderParameter = new EncoderParameter( myEncoder, (long)EncoderValue.FrameDimensionPage );
			myEncoderParameters.Param[0] = myEncoderParameter;
			imgFile.SaveAdd( bmpLarge, myEncoderParameters );

			imgFile.Dispose();
		}

		public static ImageCodecInfo GetEncoderInfo(String mimeType)
		{
			int j;
			ImageCodecInfo[] encoders;
			encoders = ImageCodecInfo.GetImageEncoders();
			for(j = 0; j < encoders.Length; ++j)
			{
				if(encoders[j].MimeType == mimeType)
					return encoders[j];
			}
			return null;
		}

		private static int arrConv( byte[] arr, int nStart, int nLen )
		{
			int nResult = 0;
			for ( int nPos=nStart+nLen-1 ; nLen>0 ; nLen--, nPos-- )
				nResult = (nResult<<8) + arr[nPos];
			return nResult;
		}

		private static PropertyItem getPropertyItem( Image img, int nID )
		{
			try 
			{
				for ( int i=0 ; i<img.PropertyIdList.Length ; i++ )
					if ( img.PropertyIdList[i] == nID )
						return img.PropertyItems[i];
			}
			catch 
			{
			}
			return null;
		}

		public static int imgPropertyItemShort( Image img, int nID )
		{
			PropertyItem itm = getPropertyItem(img,nID);
			if ( itm==null )
				return 0;
			return arrConv(itm.Value,0,2);
		}

		public static double imgPropertyItemRational( Image img, int nID )
		{
			PropertyItem itm = getPropertyItem(img,nID);
			if ( itm==null )
				return 0;
			return (double)arrConv(itm.Value,0,4)/(double)arrConv(itm.Value,4,4);
		}

		[DllImport("user32.dll",EntryPoint="FindWindow")]
        public static extern IntPtr FindWindow(string className, string windowName);
		[DllImport("user32.dll",EntryPoint="ShowWindow")]
        public static extern IntPtr ShowWindow(IntPtr hWnd, int nCode);

		[DllImport("user32.dll", SetLastError=true)]
        static extern int GetWindowThreadProcessId(IntPtr hWnd, int lpdwProcessId);
		[DllImport( "user32.dll" )]
		static extern bool AttachThreadInput( int idAttach, int idAttachTo, int fAttach );
		[DllImport( "user32.dll" )]
        static extern IntPtr GetForegroundWindow();
		[DllImport( "user32.dll" )]
        static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        public static void forceSetForegroundWindow(IntPtr hWndMain, IntPtr hWndSwitchTo)
		{
			int dwForegroundThreadID;
			int dwMainThreadID = GetWindowThreadProcessId( hWndMain, 0 );

			dwForegroundThreadID = GetWindowThreadProcessId( GetForegroundWindow(), 0 );
			if ( dwForegroundThreadID != dwMainThreadID )
			{
				AttachThreadInput( dwMainThreadID, dwForegroundThreadID, -1 );
				SetForegroundWindow( hWndSwitchTo );
				AttachThreadInput( dwMainThreadID, dwForegroundThreadID, 0 );
			}
			else
				SetForegroundWindow( hWndSwitchTo );
		}

		public class ListViewItemComparer : System.Collections.IComparer 
		{
			private int _nColIndex;
			private SortOrder _SortOrder = SortOrder.Ascending;

			public ListViewItemComparer() 
			{
				_nColIndex=0;
			}
			public ListViewItemComparer( int column, SortOrder sortorder ) 
			{
				_nColIndex = column;
				_SortOrder = sortorder;
			}
			public int ColumnIndex
			{
				get { return _nColIndex; }
				set { _nColIndex=value; }
			}
			public SortOrder SortOrder
			{
				get { return _SortOrder; }
				set { _SortOrder=value; }
			}
			public void toggleSortOrder()
			{
				_SortOrder = _SortOrder==SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
			}
			public void ColumnClick( int nColumn )
			{
				if ( _nColIndex == nColumn )
					toggleSortOrder();
				else
				{
					_SortOrder = SortOrder.Ascending;
					_nColIndex = nColumn;
				}
			}
			public int Compare(object x, object y) 
			{
				ListViewItem l1 = (ListViewItem)x;
				ListViewItem l2 = (ListViewItem)y;
				if ( l1==null || l2==null )
					return 0;
				if ( l1.SubItems.Count<=_nColIndex || l2.SubItems.Count<=_nColIndex )
					return 0;
				string s1 = l1.SubItems[_nColIndex].Text;
				string s2 = l2.SubItems[_nColIndex].Text;
				bool fU1 = s1!=null && s1.StartsWith("_");
				bool fU2 = s2!=null && s2.StartsWith("_");
				if ( fU1!=fU2 )
					return (_SortOrder==SortOrder.Ascending?1:-1) * (fU1?1:-1);
				return (_SortOrder==SortOrder.Ascending?1:-1) * string.Compare(s1,s2);
			}
		
		}

	}

	public class ScreenSaver
	{
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref int pvParam, uint fWinIni);
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, int vParam, uint fWinIni);

		static public int Delay
		{
			get
			{
				int nTime=0;
				SystemParametersInfo( 14, 0, ref nTime, 0 );
				return nTime;
			}
			set
			{
				SystemParametersInfo( 15, (uint)value, 0, 0 );
			}
		}

	}
	
	public class GridStatusCellPainter : ICellViewer
	{
		int ICellViewer.GetFittedHeight( Cell cell, AutoHeightMode ahm )
		{
			return -1;
		}
		int ICellViewer.GetFittedWidth( Cell cell )
		{
			return -1;
		}
		bool ICellViewer.PaintCellValue(
			Xceed.Grid.GridPaintEventArgs e,
			Xceed.Grid.Cell cell )
		{
			/*
			if ( !(cell.Value is int) )
				return false;
			int nVal = (int)cell.Value;
			if ( nVal<100 )
			{
				GruppNumrering gn = (GruppNumrering)nVal;
				Rectangle rect = PlataDM.vdUsr.ImgHelper.adaptProportionalRect( e.ClientRectangle, 100, 100 );
				e.Graphics.FillEllipse( PlataDM.Global.GruppNumreringTillFärg(gn), rect );
				switch ( gn )
				{
					case GruppNumrering.EjNamnsättning:
					case GruppNumrering.EjNumrering:
						e.Graphics.DrawString( "ej", cell.Font, Brushes.Black, rect, vdUsr.Util.sfCenter );
						break;
				}
			}
			*/
			PlataDM.Util.paintGroupNumberingSymbol(
				e.Graphics,
				cell.Font,
				cell.ParentRow.Tag as PlataDM.Grupp,
				vdUsr.ImgHelper.adaptProportionalRect( e.ClientRectangle, e.ClientRectangle.Height - 2, e.ClientRectangle.Height - 2 ) );
			return true;
		}

	}

}
