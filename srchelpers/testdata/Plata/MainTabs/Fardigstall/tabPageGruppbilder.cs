using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using Photomic.Common;
using Xceed.Grid;

namespace Plata
{
	public partial class tabPageGruppbilder : UserControl, IBSTab
	{
		public tabPageGruppbilder()
		{
			InitializeComponent();

			Column col;
			var gcb = new Xceed.Grid.Editors.GridComboBox();
			//gcb.Sorted = true;
			ugGrupperEjFotade.G.ScrollBars = Xceed.Grid.GridScrollBars.ForcedVertical;
			ugGrupperEjFotade.addColumn( "Namn", 180 ).ReadOnly = true;
			col = ugGrupperEjFotade.addColumn( "Orsak", 120 );
			col.CellEditor = gcb;
			col.CellViewer = new RedCellPainter(false);
			ugGrupperEjFotade.addColumn( "Extra kommentar", 100 ).CellViewer = new RedCellPainter(true);
			ugGrupperEjFotade.setColumnFullWidth( 2 );

			foreach ( string s in Global.GrpNoPhoto_Reasons_Complete )
				gcb.Items.Add( s );
			foreach ( string s in Global.GrpNoPhoto_Reasons_NonComplete )
				gcb.Items.Add( s );

			frmFärdigställ.prepareGruppGrid( ugG, false );

			lblGOrsakHjälp.Text = lblGOrsakHjälp.Text.Replace( "|", "\r\n" );

			ugG.subscribeToEvents();
			ugGrupperEjFotade.subscribeToEvents();
		}

		protected override void OnResize( EventArgs e )
		{
			base.OnResize( e );

			try
			{
				int nW = this.ClientRectangle.Width / 2;
				int nH = this.ClientRectangle.Height - 8;
				ugG.Bounds = new Rectangle( 4, 4, nW - 6, nH );
				lblGOrsakHjälp.Bounds = new Rectangle( nW + 6, nH - lblGOrsakHjälp.Height, nW, lblGOrsakHjälp.Height );
				ugGrupperEjFotade.Bounds = new Rectangle( nW + 6, 4, nW - 10, nH - lblGOrsakHjälp.Height - 8 );
				pnlMultiDrag.Location = new Point( ugG.Right, ugG.Bottom - pnlMultiDrag.Height );
			}
			catch
			{
			}

			ugG.fixSpringColumn( 1 );
			ugGrupperEjFotade.fixSpringColumn( 1 );
		}

		void IBSTab.load()
		{
			ugGrupperEjFotade.beginCleanFillup();
			ugG.beginCleanFillup();

			foreach ( PlataDM.Grupp grupp in Global.Skola.Grupper.GrupperIOrdning() )
				if ( !string.IsNullOrEmpty( grupp.ThumbnailKey ) )
				{
					grupp.Ordning = ugG.G.DataRows.Count + 1;
					var row = ugG.addRow();
					row.Cells[0].Value = grupp.Ordning;
					row.Cells[1].Value = (int)grupp.Numrering;
					row.Cells[2].Value = grupp.SpecialText;
					row.Cells[3].Value = grupp.Namn;
					row.Cells[4].Value = grupp.Slogan;
					row.Cells[5].Value = grupp.PersonerNärvarande.Count;
					row.Cells[6].Value = grupp.PersonerFrånvarande.Count;
					row.Cells[7].Value = grupp.PersonerSlutat.Count;
                    if ( grupp.AntalGratisEx >= 0 )
                        row.Cells[8].Value = grupp.AntalGratisEx;
					row.Cells[9].Value = grupp.ThumbnailGrayKey != null ? "G" : "";
					row.EndEdit();
					row.Tag = grupp;
				}
				else
				{
					var row = ugGrupperEjFotade.addRow();
					row.Cells[0].Value = grupp.Namn;
					if ( grupp.GruppTyp != GruppTyp.GruppNormal )
						row.Cells[0].ForeColor = SystemColors.Highlight;
					row.Cells[1].Value = grupp.EjFotoOrsak1;
					row.Cells[2].Value = grupp.EjFotoOrsak2;
					row.EndEdit();
					row.Tag = grupp;
					row.KeyPress += row_KeyPress;
				}
			foreach ( PlataDM.Grupp grupp in Global.Skola.MakuleradeGrupper )
			{
				DataRow row = ugGrupperEjFotade.addRow();
				row.Cells[0].Value = "mak: " + grupp.Namn;
				row.Cells[0].ForeColor = Color.DarkRed;
				row.Cells[1].Value = grupp.EjFotoOrsak1;
				row.Cells[2].Value = grupp.EjFotoOrsak2;
				row.EndEdit();
				row.Tag = grupp;
			}
			ugGrupperEjFotade.endFillup();
			ugG.endFillup();
		}

		void  IBSTab.save()
		{
		}

		private void ugGrupperEjFotade_CellValueChanged(object sender, System.EventArgs e)
		{
			var row = sender as DataRow;
			var grupp = row.Tag as PlataDM.Grupp;
			grupp.EjFotoOrsak1 = row.Cells[1].Value as string;
			grupp.EjFotoOrsak2 = row.Cells[2].Value as string;
		}

		private class RedCellPainter : ICellViewer
		{
			private readonly bool _fFreeTextColumn;
			public RedCellPainter( bool fFreeTextColumn )
			{
				_fFreeTextColumn = fFreeTextColumn;
			}
			int ICellViewer.GetFittedHeight( Cell cell, AutoHeightMode ahm )
			{
				return -1;
			}
			int ICellViewer.GetFittedWidth( Cell cell )
			{
				return -1;
			}
			bool ICellViewer.PaintCellValue( GridPaintEventArgs e, Cell cell )
			{
				var row = cell.ParentRow as DataRow;
				var grupp = row.Tag as PlataDM.Grupp;

				if ( grupp.GruppTyp!=GruppTyp.GruppNormal )
					return false;
				if ( Global.GrpNoPhoto_Reasons_Complete.Contains(grupp.EjFotoOrsak1) )
					return false;
				if ( !string.IsNullOrEmpty(grupp.EjFotoOrsak1) )
					if ( !_fFreeTextColumn || (grupp.EjFotoOrsak2!=null && grupp.EjFotoOrsak2.Length>=5) )
						return false;
				e.Graphics.FillRectangle( Brushes.Red, e.ClientRectangle );
				if ( _fFreeTextColumn && string.IsNullOrEmpty(grupp.EjFotoOrsak2) )
				{
					e.Graphics.DrawString( "kommentar måste anges!", cell.Font, Brushes.Yellow, e.ClientRectangle );
					return true;
				}
				return false;
			}
		}

		private void row_KeyPress(object sender, KeyPressEventArgs e)
		{
			foreach ( string s in (ugGrupperEjFotade.G.Columns[1].CellEditor as ComboBox).Items )
				if ( !string.IsNullOrEmpty(s) && (s[0]&0x1F)==(e.KeyChar&0x1F) )
				{
					foreach ( DataRow row in ugGrupperEjFotade.G.SelectedRows )
						row.Cells[1].Value = s;
					ugGrupperEjFotade.Refresh();
					break;
				}
		}

		private void ugG_CellValueChanged(object sender, System.EventArgs e)
		{
			var row = sender as DataRow;
			var grupp = row.Tag as PlataDM.Grupp;

			foreach ( var g in Global.Skola.Grupper )
				g.Ordning *= 2;
			grupp.Ordning = (int)row.Cells[0].Value*2 -1;
            if ( row.Cells[8].Value != null )
                grupp.AntalGratisEx = (int) row.Cells[8].Value;
			vdUsr.vdOneShotTimer.start( 1, delayedGroupRefresh, null );
		}

		private void delayedGroupRefresh( object sender, EventArgs e )
		{
			var grupp = ugG.selectedRowTag as PlataDM.Grupp;
			(this as IBSTab).load();
			foreach ( DataRow row in ugG.G.DataRows )
				if ( grupp == row.Tag )
				{
					ugG.selectedDataRow = row;
					row.BringIntoView();
					break;
				}
		}

		private Type getFirstPresentType( IDataObject Data, params object[] args )
		{
			foreach ( var s in Data.GetFormats() )
			{
				var type = Type.GetType(s);
				if ( type==null )
				{
					foreach ( Type typePassed in args )
						if ( typePassed.ToString() == s )
							return typePassed;
				}
				else
					foreach ( Type typePassed in args )
						if ( typePassed==type || type.IsSubclassOf(typePassed) )
							return type;
			}
			return null;
		}

		private void ugG_DragDropEvent(vdXceed.vdPlainGrid sender, vdXceed.vdPlainGrid.DragDropAction action, Cell cell, DragEventArgs args, ref bool fCancel)
		{
			var p = args!=null ? ugG.PointToClient( new Point( args.X, args.Y ) ) : Point.Empty;
			Type typeFound;

			try
			{
				switch ( action )
				{
					case vdXceed.vdPlainGrid.DragDropAction.QueryBeginDrag:
						if ( ugG.selectedDataRow!=null )
						{
							var  grupper = new PlataDM.Grupp[ugG.G.SelectedRows.Count];
							for ( var i = 0 ; i < grupper.Length ; i++ )
								grupper[i] = ugG.G.SelectedRows[i].Tag as PlataDM.Grupp;
							this.DoDragDrop( new DataObject(grupper), DragDropEffects.Copy );
							ugG.HighlightRow = null;
						}
						break;

					case vdXceed.vdPlainGrid.DragDropAction.QueryDrop:
						ugG.HighlightRow = cell!=null ? cell.ParentRow : null;
						typeFound = getFirstPresentType( args.Data, typeof(PlataDM.Grupp[]) );
						if ( typeFound!=null )
							args.Effect = args.AllowedEffect;
						else
							args.Effect = DragDropEffects.None;
						if ( p.Y<15 )
							ugG.G.Scroll( ScrollDirection.Up );
						else if ( p.Y > ugG.Height-15 )
							ugG.G.Scroll( ScrollDirection.Down );
						//						System.Diagnostics.Debug.WriteLine( string.Format("{0} {1}", args.X, args.Y ) );
						break;

					case vdXceed.vdPlainGrid.DragDropAction.Dropped:
						ugG.HighlightRow = null;

						typeFound = getFirstPresentType( args.Data, typeof(PlataDM.Grupp[]) );
						if ( typeFound==null )
							return;

						foreach ( PlataDM.Grupp grupp in Global.Skola.Grupper )
							grupp.Ordning *= 100;
						int nDropAtIndex;
						var grupperDragged = (PlataDM.Grupp[])args.Data.GetData( typeFound );
						if ( cell==null )
						{
							if ( p.Y < 15 )
								nDropAtIndex = 0;
							else
								nDropAtIndex = 1000000;
						}
						else
							nDropAtIndex = ((cell.ParentRow as DataRow).Tag as PlataDM.Grupp).Ordning - 1;
						foreach ( var grupp in grupperDragged )
							grupp.Ordning = nDropAtIndex++;

						vdUsr.vdOneShotTimer.start( 1, new EventHandler(delayedGroupRefresh), null );
						break;
				}
			}
			catch ( Exception ex )
			{
				Global.showMsgBox( this, ex.ToString() );
			}
		}

		private void sorteraGrupper( bool fAlpha )
		{
			var grupper = Global.Skola.Grupper;
			ArrayList a = new ArrayList( grupper.Count );
			foreach ( PlataDM.Grupp grupp in grupper )
				a.Add( grupp );
			if ( fAlpha )
				a.Sort();
			else
				a.Sort( new GruppNumericCompare() );
			int nOrdning = 1;
			foreach ( PlataDM.Grupp grupp in a )
				grupp.Ordning = nOrdning++;
			(this as IBSTab).load();
		}

		private void mnuSortAlpha_Click( object sender, EventArgs e )
		{
			sorteraGrupper( true );
		}

		private void mnuSortDigit_Click( object sender, System.EventArgs e )
		{
			sorteraGrupper( false );
		}

		private class GruppNumericCompare : IComparer
		{
			public int Compare( object x, object y )
			{
				char[] digits = "0123456789".ToCharArray();
				string s1 = ((PlataDM.Grupp)x).Namn;
				string s2 = ((PlataDM.Grupp)y).Namn;
				int n1 = s1.IndexOfAny( digits );
				int n2 = s2.IndexOfAny( digits );

				if ( n1 >= 1 )
					s1 = s1[n1] + s1;
				if ( n2 >= 1 )
					s2 = s2[n2] + s2;
				return string.Compare( s1, s2 );
			}
		}

		private void ugG_CellDoubleClick( object sender, EventArgs e )
		{
			var cell = sender as Cell;
			if ( cell == null )
				return;
			var grupp = cell.ParentRow.Tag as PlataDM.Grupp;
			if ( grupp == null )
				return;
			var parent = this.ParentForm as baseFlikForm;
			parent.frmMain.jumpToForm_Group_Person( FlikTyp.GruppbildInne, grupp, null );
		}

		private void pnlMultiDrag_MouseMove( object sender, MouseEventArgs e )
		{
			if ( ugG.G.SelectedRows.Count==0 || e.Button != MouseButtons.Left )
				return;
			var grupper = new PlataDM.Grupp[ugG.G.SelectedRows.Count];
			for ( int i = 0 ; i < grupper.Length ; i++ )
				grupper[i] = ugG.G.SelectedRows[i].Tag as PlataDM.Grupp;
			this.DoDragDrop( new DataObject( grupper ), DragDropEffects.Copy );
			ugG.HighlightRow = null;
		}

		private void ugG_SelectedRowsChanged( object sender, EventArgs e )
		{
			pnlMultiDrag.Invalidate();
		}

		private void pnlMultiDrag_Paint( object sender, PaintEventArgs e )
		{
			e.Graphics.DrawString(
				ugG.G.SelectedRows.Count.ToString(),
				pnlMultiDrag.Font,
				Brushes.White,
				pnlMultiDrag.ClientRectangle,
				vdUsr.Util.sfMC );
		}

	}

}
