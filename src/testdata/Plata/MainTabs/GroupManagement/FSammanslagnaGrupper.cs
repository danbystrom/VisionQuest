using System;
using System.Collections;
using System.Windows.Forms;
using Photomic.Common;
using PlataDM;
using Xceed.Grid;

namespace Plata
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class FSammanslagnaGrupper : vdUsr.baseGradientForm
	{
		private Button cmdNy;
		private Button cmdCancel;
		private Button cmdOK;
		private vdXceed.vdPlainGrid ugG;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Skola _skola;
		private ArrayList _alNormalGroups;
		private Label lblInfo;
		private bool _fNoRecur;

		private FSammanslagnaGrupper()
		{
			InitializeComponent();

			lblInfo.Text =
				"För varje grupp markerar du vad gruppbilden är tänkt att användas till, enligt följande:\r\n" +
				"  G = Gruppbild\r\n" +
				"  K = Katalogbild\r\n" +
				"  S = Skyddad ID\r\n" +
				"  P = Plojbild";
			ugG.addColumn( "", typeof( int ), 10 ).Visible = false;
//			col.Visible = false;
//			col.SortDirection = SortDirection.Ascending;

			ugG.addColumn( "Gruppnamn", 100 );
			ugG.addColumn( "G", typeof( bool ), 20 );
			ugG.addColumn( "K", typeof( bool ), 20 );
			ugG.addColumn( "S", typeof( bool ), 20 );
			ugG.addColumn( "P", typeof( bool ), 20 );

			//Xceed.Grid.Editors.CheckBoxEditor cbe = (ugG.G.Columns[2] as Column).CellEditor;

			ugG.G.Sorted += new EventHandler( G_Sorted );
			G_Sorted( null, null );

			ugG.setColumnFullWidth( 1 );
			ugG.fixSpringColumn( 10 );
		}

		void G_Sorted( object sender, EventArgs e )
		{
			if ( !_fNoRecur )
			{
				_fNoRecur = true;
				ugG.G.SortedColumns.Clear();
				ugG.G.SortedColumns.Add( ugG.G.Columns[0] );
				ugG.G.SortedColumns.Add( ugG.G.Columns[1] );
				_fNoRecur = false;
			}
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
			this.cmdNy = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.ugG = new vdXceed.vdPlainGrid();
			this.lblInfo = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.ugG)).BeginInit();
			this.SuspendLayout();
			// 
			// cmdNy
			// 
			this.cmdNy.Location = new System.Drawing.Point( 272, 282 );
			this.cmdNy.Name = "cmdNy";
			this.cmdNy.Size = new System.Drawing.Size( 80, 29 );
			this.cmdNy.TabIndex = 3;
			this.cmdNy.Text = "Ny";
			this.cmdNy.UseVisualStyleBackColor = true;
			this.cmdNy.Click += new System.EventHandler( this.cmdNy_Click );
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point( 358, 13 );
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size( 80, 28 );
			this.cmdCancel.TabIndex = 12;
			this.cmdCancel.Text = "Avbryt";
			// 
			// cmdOK
			// 
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point( 272, 13 );
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size( 80, 28 );
			this.cmdOK.TabIndex = 11;
			this.cmdOK.Text = "OK";
			// 
			// ugG
			// 
			this.ugG.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)));
			this.ugG.Caption = null;
			this.ugG.Location = new System.Drawing.Point( 12, 12 );
			this.ugG.Name = "ugG";
			this.ugG.Size = new System.Drawing.Size( 254, 299 );
			this.ugG.TabIndex = 17;
			this.ugG.CellValueChanged += new System.EventHandler( this.ugG_CellValueChanged );
			// 
			// lblInfo
			// 
			this.lblInfo.BackColor = System.Drawing.SystemColors.Info;
			this.lblInfo.ForeColor = System.Drawing.SystemColors.InfoText;
			this.lblInfo.Location = new System.Drawing.Point( 272, 65 );
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size( 166, 97 );
			this.lblInfo.TabIndex = 18;
			this.lblInfo.Text = "-";
			this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FSammanslagnaGrupper
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size( 442, 323 );
			this.Controls.Add( this.lblInfo );
			this.Controls.Add( this.ugG );
			this.Controls.Add( this.cmdCancel );
			this.Controls.Add( this.cmdOK );
			this.Controls.Add( this.cmdNy );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "FSammanslagnaGrupper";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Sammanslagna grupper";
			((System.ComponentModel.ISupportInitialize)(this.ugG)).EndInit();
			this.ResumeLayout( false );

		}
		#endregion

		public static DialogResult showDialog( Form parent, Skola skola )
		{
			using ( FSammanslagnaGrupper dlg = new FSammanslagnaGrupper() )
			{
				dlg.load( skola );
				if ( dlg.ShowDialog( parent ) == DialogResult.OK )
				{
					dlg.save();
					return DialogResult.OK;
				}
				return DialogResult.Cancel;
			}
		}

		private DataRow addRow( Grupp g, DataRow parentRow, bool fNy )
		{
			int nIndex = parentRow!=null ? (int)parentRow.Cells[0].Value+1 : ugG.G.DataRows.Count;
			DataRow row = ugG.addRow();
			row.Cells[0].Value = nIndex;
			if ( g.isAggregate )
				row.Cells[1].Value = g.Namn;
			else
			{
				row.Cells[1].Value = "  -  " + g.Namn;
				row.Cells[1].ReadOnly = true;
			}
            row.Cells[2].Value = !fNy & (g.Special & TypeOfGroupPhoto.Gruppbild) != 0;
            row.Cells[3].Value = fNy | (g.Special & TypeOfGroupPhoto.Katalog) != 0;
            row.Cells[4].Value = !fNy & (g.Special & TypeOfGroupPhoto.SkyddadId) != 0;
            row.Cells[5].Value = !fNy & (g.Special & TypeOfGroupPhoto.Spex) != 0;
			row.EndEdit();
			row.Tag = new XTag( parentRow, g, fNy );
			return row;
		}

		private void load( Skola skola )
		{
			_skola = skola;
			foreach ( Grupp g in skola.Grupper )
				if ( g.isAggregate )
				{
					DataRow row = addRow( g, null, false );
					foreach ( Grupp g2 in g.aggregatedGroups )
						addRow( g2, row, false );
				}

			_alNormalGroups = new ArrayList();
			foreach ( Grupp g in _skola.Grupper )
				if ( g.GruppTyp == GruppTyp.GruppNormal )
					if ( !g.isAggregate && !g.isAggregated )
						_alNormalGroups.Add( g );

		}

		private static TypeOfGroupPhoto getSpecial( DataRow row )
		{
            var gs = TypeOfGroupPhoto.Ingen;
			if ( toBool( row.Cells[2].Value ) )
                gs |= TypeOfGroupPhoto.Gruppbild;
			if ( toBool( row.Cells[3].Value ) )
                gs |= TypeOfGroupPhoto.Katalog;
			if ( toBool( row.Cells[4].Value ) )
                gs |= TypeOfGroupPhoto.SkyddadId;
			if ( toBool( row.Cells[5].Value ) )
                gs |= TypeOfGroupPhoto.Spex;
			return gs;
		}

		private void save()
		{
			IList list = ugG.G.GetSortedDataRows(false);
			for ( int i=0 ; i<list.Count ; i++ )
			{
				var row = list[i] as DataRow;
				var g = (row.Tag as XTag).Grupp;
				if ( g != null )
				{
					g.Special = getSpecial( row );
					continue;
				}

				g = _skola.Grupper.Add( row.Cells[1].Value as string, GruppTyp.GruppNormal );
				g.Special = getSpecial( row );
				for ( i++ ; i < list.Count ; i++ )
				{
					DataRow row2 = list[i] as DataRow;
					XTag xtag = row2.Tag as XTag;
					if ( xtag.ParentRow != row )
						break;

					Grupp g2 = xtag.Grupp;
					g2.Special = getSpecial( row2 );
					if ( xtag.Ny )
					{
						g.addAggregatedGroup( g2 );
						foreach ( Person p in g2.AllaPersoner )
						{
							Person p2 = g.PersonerNärvarande.Add( p.Personal, p.getInfos() );
							p.ProtArchive = p.ProtArchive;
							p.ProtGroup = p.ProtGroup;
							p.ProtCatalog = p.ProtCatalog;
						}
					}
				}
			}
		}

		private void cmdNy_Click( object sender, EventArgs e )
		{
			IList valda;
			string strNamn;
			if ( FSlåSammanGrupper.showDialog( this, _alNormalGroups, out valda, out strNamn ) != DialogResult.OK )
				return;
			if ( valda.Count == 0 )
				return;

			DataRow row = ugG.addRow();
			row.Cells[0].Value = ugG.G.DataRows.Count;
			row.Cells[1].Value = strNamn;
			row.Cells[2].Value = true;
			row.Cells[3].Value = false;
			row.Cells[4].Value = false;
			row.Cells[5].Value = false;
			row.EndEdit();
			row.Tag = new XTag( null, null, true );

			foreach ( Grupp g in valda )
			{
				DataRow r2 = addRow( g, row, true );
				(r2.Tag as XTag).Ny = true;
				r2.ReadOnly = true;
			}

		}

		private static bool toBool( object obj )
		{
			if ( obj is bool )
				return (bool)obj;
			return false;
		}

		private void ugG_CellValueChanged( object sender, EventArgs e )
		{
			if ( _fNoRecur )
				return;
			_fNoRecur = true;

			bool fG = false;

			foreach ( DataRow row in ugG.G.GetSortedDataRows( false ) )
			{
				XTag xtag = row.Tag as XTag;
				if ( xtag.ParentRow == null )
					fG = toBool(row.Cells[2].Value);
				else if ( toBool(row.Cells[2].Value) && fG )
					if ( row == (DataRow)sender )
						xtag.ParentRow.Cells[2].Value = fG = false;
					else
						row.Cells[2].Value = false;
			}

			_fNoRecur = false;
		}

		private class XTag
		{
			public DataRow ParentRow;
			public Grupp Grupp;
			public bool Ny;
//			public bool Raderad;
			public XTag( DataRow parentRow, Grupp grupp, bool fNy )
			{
				ParentRow = parentRow;
				Grupp = grupp;
				Ny = fNy;
			}
		}

	}

}
