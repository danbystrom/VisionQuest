using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace vdUsr
{
	public class PaintCalendarDayEventArgs
	{
		public	Graphics	Graphics;
		public	int				number;
		public	bool			fIsWeek;
		public	int				nWeek;
		public	DateTime	date;
		public	Rectangle	rect;
		public	Font			font;
		public	Brush			brush;
		public	StringFormat format;
	}
	public delegate void PaintCalendarDay( object sender, PaintCalendarDayEventArgs e );

	public class vdCalendar : vdUsr.FlickerFreePanel
	{
		public event PaintCalendarDay PaintCalendarDay;

		public enum HitTestLocation
		{
			None,
			Day,
			DayOtherMonth,
			Week,
			WeekOtherMonth,
			Head,
			WeekDayTitle,
		}

		private System.ComponentModel.Container components = null;

		private DateTime _dateFirstMonth = DateTime.Now.Date;
		private Size _szDimensions = new Size(1,1);
		private Rectangle[] _arectHit;

		private Color _colorTitleBack = SystemColors.ActiveCaption;
		private Color _colorTitleFore = SystemColors.ActiveCaptionText;

		public vdCalendar()
		{
			InitializeComponent();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		private Rectangle paintOneCalendar( Graphics g, DateTime date, int nX1, int nY1, int nX2, int nY2 )
		{
			int nFH = this.FontHeight;
			int nFHBig = 12*nFH/10;
	
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			sf.LineAlignment = StringAlignment.Center;

			{
				Rectangle rect = new Rectangle( nX1, nY1, nX2-nX1, nFHBig );
				using ( Brush brush = new SolidBrush(_colorTitleBack) )
					g.FillRectangle( brush, rect );
				using ( Brush brush = new SolidBrush(_colorTitleBack) )
				using ( Font fntBold = new Font(this.Font,FontStyle.Bold) )
					g.DrawString( date.ToString("MMM, yyyy"), fntBold, SystemBrushes.ActiveCaptionText, rect, sf );
			}

			nX1 -= nFH/2;
			int nW = nX2 - nX1;

			string[] z = new string[] { "", "Mån", "Tis", "Ons", "Tor", "Fre", "Lör", "Sön" };
//			rect = new Rectangle( nX1, nY1+nFHBig, nW, nFH );
			for ( int i=0 ; i<8 ; i++ )
			{
				Rectangle r = new Rectangle( nX1+i*nW/8-10, nY1+nFHBig, nW/8+20, nFH );
				g.DrawString( z[i], this.Font, SystemBrushes.ControlText, r, sf );
			}

			nY1 += nFHBig + nFH;
			int nH = nY2 - nY1;

			sf.Alignment = StringAlignment.Far;
			PaintCalendarDayEventArgs e = new PaintCalendarDayEventArgs();
			e.Graphics = g;
			e.date = vdUsr.DateHelper.getNearestDayOfWeekBefore( date, DayOfWeek.Monday );
			e.font = this.Font;
			e.format = sf;
			for ( int niW=0 ; niW<6 ; niW++ )
			{
				e.rect = new Rectangle( nX1, nY1+niW*nH/6, nW/8-2, nH/6 );
				e.brush = SystemBrushes.ControlText; //SystemBrushes.ActiveCaption;
				e.number = e.nWeek = vdUsr.DateHelper.Week(e.date.AddDays(6));
				e.fIsWeek = true;
				if ( PaintCalendarDay!=null )
					PaintCalendarDay( this, e );
				else
					g.DrawString( e.number.ToString(), e.font, e.brush, e.rect, sf );
				e.brush = SystemBrushes.ControlText;
				e.fIsWeek = false;
				for ( int i=1 ; i<8 ; i++ )
				{
					if ( e.date.Month==date.Month )
					{
						e.rect = new Rectangle( nX1+i*nW/8, nY1+niW*nH/6, nW/8-2, nH/6 );
						e.number = e.date.Day;
						if ( PaintCalendarDay!=null )
							PaintCalendarDay( this, e );
						else
							g.DrawString( e.number.ToString(), e.font, e.brush, e.rect, sf );
					}
					e.date = vdUsr.DateHelper.Tomorrow( e.date );
				}
			}

			g.DrawLine( SystemPens.ControlText, nX1+nW/8+2, nY1-1, nX2-2, nY1-1 );
			g.DrawLine( SystemPens.ControlText, nX1+nW/8, nY1+2, nX1+nW/8, nY2-2 );

			//			return new Rectangle( nX1+nW/8, nY1, nW*7/8, nH );
			return new Rectangle( nX1, nY1, nW, nH );
		}

		private void paint( PaintEventArgs pevent )
		{
			if ( _arectHit==null || _arectHit.Length==0 )
				return;

			Size szThis = new Size( this.ClientSize.Width+4, this.ClientSize.Height+4 );
			DateTime date = _dateFirstMonth;
			int i = 0;
			for ( int nY=0 ; nY<_szDimensions.Height ; nY++ )
				for ( int nX=0 ; nX<_szDimensions.Width ; nX++ )
				{
					_arectHit[i++] = paintOneCalendar(
						pevent.Graphics,
						date,
						nX*szThis.Width/_szDimensions.Width,
						nY*szThis.Height/_szDimensions.Height,
						(nX+1)*szThis.Width/_szDimensions.Width-4,
						(nY+1)*szThis.Height/_szDimensions.Height-4 );
					if ( date.Month==12 )
						date = new DateTime( date.Year+1, 1, 1 );
					else
						date = new DateTime( date.Year, date.Month+1, 1 );
				}
		}

		protected override void OnPaintShadowImage(PaintEventArgs pevent)
		{
			base.OnPaintShadowImage (pevent);
			paint( pevent );
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);
			if ( this.DesignMode )
				paint( e );
		}

		public DateTime FirstMonth
		{
			get { return _dateFirstMonth; }
			set
			{
				_dateFirstMonth = new DateTime( value.Year, value.Month, 1 );
				recreateBackground();
			}
		}

		public Size CalendarDimensions
		{
			get { return _szDimensions; }
			set
			{
				_szDimensions = value;
				_arectHit = new Rectangle[_szDimensions.Width*_szDimensions.Height];
				recreateBackground();
			}
		}

		[DefaultValue(typeof(Color),"ActiveCaption")]
		public Color TitleBackColor
		{
			get { return _colorTitleBack; }
			set {	_colorTitleBack = value; recreateBackground(); }
		}

		[DefaultValue(typeof(Color),"ActiveCaptionText")]
		public Color TitleBackFore
		{
			get { return _colorTitleFore; }
			set {	_colorTitleFore = value; recreateBackground(); }
		}

		public HitTestLocation hitTest( int x, int y, out DateTime date )
		{
			for ( int i=0 ; i<_arectHit.Length ; i++ )
				if ( _arectHit[i].Contains(x,y) )
				{
					int nX = 8*(x-_arectHit[i].Left)/_arectHit[i].Width;
					int nY = 6*(y-_arectHit[i].Top)/_arectHit[i].Height;
					int nThisYear = _dateFirstMonth.Year;
					int nThisMonth = _dateFirstMonth.Month + i;
					HitTestLocation htl = HitTestLocation.Day;
					for ( ; nThisMonth>12 ; nThisMonth-=12 )
						nThisYear++;
					DateTime dateThisMonth = new DateTime( nThisYear, nThisMonth, 1 );
					DateTime dateHit = vdUsr.DateHelper.getNearestDayOfWeekBefore( dateThisMonth, DayOfWeek.Monday );
					if ( nX>0 )
						dateHit += vdUsr.DateHelper.tsDays( nY*7+nX-1 );
					else
					{
						dateHit += vdUsr.DateHelper.tsDays( nY*7 );
						htl = HitTestLocation.Week;
					}
					date = dateHit;
					if ( dateHit.Month==dateThisMonth.Month )
						return htl;
					return htl==HitTestLocation.Day ? HitTestLocation.DayOtherMonth : HitTestLocation.WeekOtherMonth;
				}

			date = DateTime.MinValue;
			return HitTestLocation.None;
		}

	}

}
