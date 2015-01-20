using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

namespace Plata.Notes
{
	/// <summary>
	/// Summary description for FPassword.
	/// </summary>
	public class FNotes : vdUsr.baseGradientForm
	{
		private vdUsr.vdCalendar calendar;
		private IContainer components;
		private RichTextBox rtf;
		private Button cmdClose;
		private Button cmdNew;
		private Button cmdEdit;
		private Button cmdPrevMonth;
		private Button cmdNextMonth;

		private Notes _notes = new Notes();
		private CheckedListBox lstOrderFilter;
		private GroupBox groupBox1;
		private Timer tmrDelayedUpdate;
		private Label lblBoldFont;
		private DateTime _dateSelected;
 
		private FNotes()
		{
			InitializeComponent();
			calendar.FirstMonth = _dateSelected = DateTime.Now.Date;
		}

		protected override void OnResize( EventArgs e )
		{
			base.OnResize( e );
            if ( calendar != null)
    			calendar.CalendarDimensions = new Size( 1, 1 + calendar.Height / (int)(0.8*calendar.Width) );
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.rtf = new System.Windows.Forms.RichTextBox();
			this.cmdClose = new System.Windows.Forms.Button();
			this.cmdNew = new System.Windows.Forms.Button();
			this.cmdEdit = new System.Windows.Forms.Button();
			this.cmdPrevMonth = new System.Windows.Forms.Button();
			this.cmdNextMonth = new System.Windows.Forms.Button();
			this.lstOrderFilter = new System.Windows.Forms.CheckedListBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tmrDelayedUpdate = new System.Windows.Forms.Timer( this.components );
			this.lblBoldFont = new System.Windows.Forms.Label();
			this.calendar = new vdUsr.vdCalendar();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// rtf
			// 
			this.rtf.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.rtf.BackColor = System.Drawing.SystemColors.Window;
			this.rtf.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
			this.rtf.Location = new System.Drawing.Point( 208, 12 );
			this.rtf.Name = "rtf";
			this.rtf.ReadOnly = true;
			this.rtf.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
			this.rtf.ShowSelectionMargin = true;
			this.rtf.Size = new System.Drawing.Size( 570, 648 );
			this.rtf.TabIndex = 1;
			this.rtf.Text = "";
			// 
			// cmdClose
			// 
			this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdClose.Location = new System.Drawing.Point( 856, 629 );
			this.cmdClose.Name = "cmdClose";
			this.cmdClose.Size = new System.Drawing.Size( 80, 28 );
			this.cmdClose.TabIndex = 5;
			this.cmdClose.Text = "Stäng";
			// 
			// cmdNew
			// 
			this.cmdNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdNew.Location = new System.Drawing.Point( 784, 12 );
			this.cmdNew.Name = "cmdNew";
			this.cmdNew.Size = new System.Drawing.Size( 152, 28 );
			this.cmdNew.TabIndex = 6;
			this.cmdNew.Text = "Skriv ny anteckning";
			this.cmdNew.Click += new System.EventHandler( this.cmdNew_Click );
			// 
			// cmdEdit
			// 
			this.cmdEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdEdit.Location = new System.Drawing.Point( 784, 46 );
			this.cmdEdit.Name = "cmdEdit";
			this.cmdEdit.Size = new System.Drawing.Size( 152, 28 );
			this.cmdEdit.TabIndex = 7;
			this.cmdEdit.Text = "Redigera anteckning";
			this.cmdEdit.Click += new System.EventHandler( this.cmdEdit_Click );
			// 
			// cmdPrevMonth
			// 
			this.cmdPrevMonth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdPrevMonth.Location = new System.Drawing.Point( 12, 632 );
			this.cmdPrevMonth.Name = "cmdPrevMonth";
			this.cmdPrevMonth.Size = new System.Drawing.Size( 33, 28 );
			this.cmdPrevMonth.TabIndex = 9;
			this.cmdPrevMonth.Text = "<";
			this.cmdPrevMonth.Click += new System.EventHandler( this.cmdPrevMonth_Click );
			// 
			// cmdNextMonth
			// 
			this.cmdNextMonth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdNextMonth.Location = new System.Drawing.Point( 169, 632 );
			this.cmdNextMonth.Name = "cmdNextMonth";
			this.cmdNextMonth.Size = new System.Drawing.Size( 33, 28 );
			this.cmdNextMonth.TabIndex = 10;
			this.cmdNextMonth.Text = ">";
			this.cmdNextMonth.Click += new System.EventHandler( this.cmdNextMonth_Click );
			// 
			// lstOrderFilter
			// 
			this.lstOrderFilter.FormattingEnabled = true;
			this.lstOrderFilter.Location = new System.Drawing.Point( 6, 19 );
			this.lstOrderFilter.Name = "lstOrderFilter";
			this.lstOrderFilter.Size = new System.Drawing.Size( 140, 289 );
			this.lstOrderFilter.Sorted = true;
			this.lstOrderFilter.TabIndex = 11;
			this.lstOrderFilter.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler( this.lstOrderFilter_ItemCheck );
			this.lstOrderFilter.Format += new System.Windows.Forms.ListControlConvertEventHandler( this.lstOrderFilter_Format );
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.groupBox1.Controls.Add( this.lstOrderFilter );
			this.groupBox1.Location = new System.Drawing.Point( 784, 80 );
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size( 152, 314 );
			this.groupBox1.TabIndex = 12;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Filtrera ordrar";
			// 
			// tmrDelayedUpdate
			// 
			this.tmrDelayedUpdate.Tick += new System.EventHandler( this.tmrDelayedUpdate_Tick );
			// 
			// lblBoldFont
			// 
			this.lblBoldFont.AutoSize = true;
			this.lblBoldFont.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
			this.lblBoldFont.Location = new System.Drawing.Point( 70, 643 );
			this.lblBoldFont.Name = "lblBoldFont";
			this.lblBoldFont.Size = new System.Drawing.Size( 0, 14 );
			this.lblBoldFont.TabIndex = 13;
			this.lblBoldFont.Visible = false;
			// 
			// calendar
			// 
			this.calendar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)));
			this.calendar.BackgroundMode = vdUsr.BackgroundMode.Gradient;
			this.calendar.CalendarDimensions = new System.Drawing.Size( 1, 4 );
			this.calendar.FirstMonth = new System.DateTime( 2008, 7, 1, 0, 0, 0, 0 );
			this.calendar.Location = new System.Drawing.Point( 12, 12 );
			this.calendar.Name = "calendar";
			this.calendar.Size = new System.Drawing.Size( 190, 614 );
			this.calendar.TabIndex = 0;
			this.calendar.PaintCalendarDay += new vdUsr.PaintCalendarDay( this.calendar_PaintCalendarDay );
			this.calendar.MouseClick += new System.Windows.Forms.MouseEventHandler( this.calendar_MouseClick );
			// 
			// FNotes
			// 
			this.CancelButton = this.cmdClose;
			this.ClientSize = new System.Drawing.Size( 948, 672 );
			this.Controls.Add( this.lblBoldFont );
			this.Controls.Add( this.groupBox1 );
			this.Controls.Add( this.cmdNextMonth );
			this.Controls.Add( this.cmdPrevMonth );
			this.Controls.Add( this.cmdEdit );
			this.Controls.Add( this.cmdNew );
			this.Controls.Add( this.cmdClose );
			this.Controls.Add( this.rtf );
			this.Controls.Add( this.calendar );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FNotes";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Anteckningar";
			this.groupBox1.ResumeLayout( false );
			this.ResumeLayout( false );
			this.PerformLayout();

		}
		#endregion

		private void displayNotes()
		{
			List<int> listOrderFilter = new List<int>();
			foreach ( int i in lstOrderFilter.CheckedItems )
				if ( i != 0 )
					listOrderFilter.Add( i );
			List<Note> notes = _notes.GetFilteredNotes(
				DateTime.MinValue,
				DateTime.MaxValue,
				true,
				listOrderFilter );
			foreach ( Note note in _notes.AllNotes )
				note.StartingCharIndex = -1;
			rtf.Clear();
			foreach ( Note note in notes )
			{
				note.StartingCharIndex = rtf.SelectionStart;
				//rtf.SelectionProtected = true;
				StringBuilder sb = new StringBuilder();
				if ( note.OrderNumber!=0 )
					sb.AppendFormat( "Order:\t{0}\r\n", note.OrderNumber );
				if ( note.RegardingDate!=DateTime.MinValue )
					sb.AppendFormat( "Datum:\t{0}\r\n", vdUsr.DateHelper.YYYYMMDD( note.RegardingDate )  );
				sb.AppendFormat( "Skriven:\t{0}\r\n", vdUsr.DateHelper.YYYYMMDD( note.Created )  );
				if ( note.Changed!=DateTime.MinValue && note.Created!=note.Changed )
					sb.AppendFormat( "Ändrad:\t{0}\r\n", vdUsr.DateHelper.YYYYMMDD( note.Changed )  );
				string s = sb.ToString();
				rtf.SelectedText = s.Substring( 0, s.Length-2 );
				rtf.SelectionColor = rtf.BackColor;
				rtf.SelectedText = string.Format( "\t¤{0}¤\r\n", note.UniqueSessionID );
				rtf.SelectionColor = rtf.ForeColor;
				rtf.SelectionProtected = false;
				rtf.SelectionIndent += 30;
				rtf.SelectionFont = lblBoldFont.Font;
				rtf.SelectedText = note.Text;
				rtf.SelectionFont = rtf.Font;
				if ( !note.Text.EndsWith("\r\n") )
					rtf.SelectedText = "\r\n";
				rtf.SelectionIndent -= 30;
				//rtf.SelectionProtected = true;
				rtf.SelectedText = new string( '-', 150 ) + "\r\n";
			}
			notes = _notes.GetFilteredNotes( 
				_dateSelected, 
				DateTime.MaxValue, 
				true,
				listOrderFilter );
			if ( notes.Count != 0 )
				highlightNote( notes[0] );
		}

		public static DialogResult showDialog( Form parent )
		{
			using ( FNotes dlg = new FNotes() )
			{
				Rectangle r = SystemInformation.WorkingArea;
				r.Inflate( -5, -5 );
				dlg.Bounds = r;
				string strFN = System.IO.Path.Combine(
					Global.DataPath,
					"notes.xml" );
				dlg.loadNotes( strFN );
				dlg.displayNotes();
				dlg.ShowDialog( parent );
				dlg.saveNotes( strFN );
				return DialogResult.OK;
			}
		}

		private void calendar_PaintCalendarDay( object sender, vdUsr.PaintCalendarDayEventArgs e )
		{
			if ( e.fIsWeek )
			{
				e.Graphics.DrawString( e.number.ToString(), e.font, SystemBrushes.ControlDark, e.rect, e.format );
				return;
			}
			Rectangle r = e.rect;
			r.Offset( 2, 0 );
			if ( e.date.Date == _dateSelected )
				e.Graphics.FillEllipse( SystemBrushes.Highlight, r );
			if ( e.date.Date == DateTime.Now.Date )
				e.Graphics.DrawEllipse( Pens.Red, r );
			if ( _notes.hasRegardingDate( e.date ) )
				using ( Font font = new Font( e.font, FontStyle.Bold ) )
					e.Graphics.DrawString( e.number.ToString(), font, e.brush, e.rect, e.format );
			else
				e.Graphics.DrawString( e.number.ToString(), e.font, e.brush, e.rect, e.format );
		}

		private void loadNotes( string strFN )
		{
			try
			{
				if ( !System.IO.File.Exists( strFN ) )
					return;
				PlataDM.vdPersist po = new PlataDM.vdPersist();
				po.beginLoadFile( strFN );
				po.descend( "NOTES" );
				po.xcol( "NOTE", _notes );
				updateOrderFilter();
			}
			catch
			{
			}
		}

		private void saveNotes( string strFN )
		{
			PlataDM.vdPersist po = new PlataDM.vdPersist();
			po.beginSave();
			po.descend( "NOTES" );
			po.xcol( "NOTE", _notes );
			po.endSaveFile( strFN );
		}

		private void cmdPrevMonth_Click( object sender, EventArgs e )
		{
			calendar.FirstMonth = calendar.FirstMonth.AddMonths( -1 ); 
		}

		private void cmdNextMonth_Click( object sender, EventArgs e )
		{
			calendar.FirstMonth = calendar.FirstMonth.AddMonths( 1 );
		}

		private void editNote( Note note, bool fIsNew )
		{
			switch ( FNote.showDialog( this, note, !fIsNew ) )
			{
				case DialogResult.OK:
					if ( fIsNew )
						_notes.addNote( note );
					else
						note.Changed = DateTime.Now;
					break;
				case DialogResult.No:
					_notes.removeNote( note );
					break;
				default:
					return;
			}

			displayNotes();
			calendar.recreateBackground();
			updateOrderFilter();
		}

		private void cmdNew_Click( object sender, EventArgs e )
		{
			Note note = new Note( DateTime.Now );
			note.RegardingDate = _dateSelected;
			editNote( note, true );
		}

		private void calendar_MouseClick( object sender, MouseEventArgs e )
		{
			DateTime date;
			switch ( calendar.hitTest( e.X, e.Y, out date ) )
			{
				case vdUsr.vdCalendar.HitTestLocation.Day:
				case vdUsr.vdCalendar.HitTestLocation.DayOtherMonth:
					_dateSelected = date;
					List<Note> list = _notes.GetFilteredNotes(
						date,
						DateTime.MaxValue,
						false,
						null );
					if ( list.Count != 0 )
						highlightNote( list[0] );
					calendar.recreateBackground();
					break;
			}
		}

		private void updateOrderFilter()
		{
			lstOrderFilter.BeginUpdate();
			lstOrderFilter.Items.Clear();
			lstOrderFilter.Items.Add( 0 );
			foreach ( Note note in _notes.AllNotes )
				if ( note.OrderNumber != 0 && !lstOrderFilter.Items.Contains( note.OrderNumber ) )
					lstOrderFilter.Items.Add( note.OrderNumber );
			lstOrderFilter.SetItemChecked( 0, true );
			lstOrderFilter.EndUpdate();
		}

		private void highlightNote( Note note )
		{
			int n = rtf.Find( string.Format( "\t¤{0}¤", note.UniqueSessionID ) );
			if ( n < 0 )
				return;
			int nTheLineIndex = rtf.GetLineFromCharIndex( n );
			int nTheLineStartsAt = rtf.GetFirstCharIndexFromLine( nTheLineIndex + 1 );
			int n2 = rtf.Find( "¤", nTheLineStartsAt, RichTextBoxFinds.None );
			if ( n2 > 0 )
			{
				rtf.SelectionStart = n2;
				rtf.ScrollToCaret();
			}
			rtf.SelectionStart = rtf.GetFirstCharIndexFromLine(
				Math.Max( 0, nTheLineIndex-2 ) );
			rtf.ScrollToCaret();
			rtf.SelectionStart = nTheLineStartsAt;
		}

		private void lstOrderFilter_Format( object sender, ListControlConvertEventArgs e )
		{
			if ( (int)e.ListItem == 0 )
				e.Value = "(Alla)";
		}

		private void lstOrderFilter_ItemCheck( object sender, ItemCheckEventArgs e )
		{
			if ( e.Index == 0 && e.NewValue==CheckState.Checked )
				for ( int i = 1 ; i < lstOrderFilter.Items.Count ; i++ )
					lstOrderFilter.SetItemChecked( i, false );
			tmrDelayedUpdate.Enabled = true;
		}

		private void tmrDelayedUpdate_Tick( object sender, EventArgs e )
		{
			tmrDelayedUpdate.Enabled = false;
			int nRealChecks = lstOrderFilter.CheckedItems.Count - (lstOrderFilter.GetItemChecked( 0 ) ? 1 : 0);
			lstOrderFilter.SetItemChecked( 0, nRealChecks < 1 );
			displayNotes();
		}

		private void cmdEdit_Click( object sender, EventArgs e )
		{
			Note noteHit = null;
			foreach ( Note note in _notes.AllNotes )
				if ( note.StartingCharIndex < rtf.SelectionStart )
					if ( noteHit == null )
						noteHit = note;
					else if ( note.StartingCharIndex > noteHit.StartingCharIndex )
						noteHit = note;
			if ( noteHit!=null )
				editNote( noteHit, false );
		}

	}

}
