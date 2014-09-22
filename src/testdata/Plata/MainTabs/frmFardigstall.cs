using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Plata.MainTabs.Fardigstall;
using Xceed.Grid;

namespace Plata
{
	public class frmFärdigställ : Plata.baseFlikForm
	{
		private TabControl tab;
		private System.ComponentModel.IContainer components = null;

		private bool _fHasInitialized;

		private readonly List<BSTabInfo> _flikar = new List<BSTabInfo>();
		private BSTabInfo _flikAktiv = null;

		private frmFärdigställ() : base()
		{
		}

		public frmFärdigställ( Form parent ) : base( parent, FlikTyp.Färdigställ )
		{
		    _strCaption = "FÄRDIGSTÄLL";
		    InitializeComponent();


		    _flikar.Add(new BSTabInfo("Anmärkningar", new tabPageAnmärkningar()));
		    _flikar.Add(new BSTabInfo("Gruppbilder", new tabPageGruppbilder()));
		    _flikar.Add(new BSTabInfo("Restfoto grupp", new tabPageRestGrupp()));
		    _flikar.Add(new BSTabInfo("Restfoto porträtt", new tabPageRestPort()));
		    _flikar.Add(new BSTabInfo("Statistik", new tabPageStatistics()));
		    //_flikar.Add(new BSTabInfo("Enkät", new tabPageQuestionarie()));
		    _flikar.Add(new BSTabInfo("Granska gruppbilder", new tabPageÖversiktGrupp()));
		    _flikar.Add(new BSTabInfo("Granska porträtt", new TabPageÖversiktPorträtt()));
		    _flikar.Add(new BSTabInfo("Granska vimmelbilder", new tabPageÖversiktVimmel()));
		    _flikar.Add(new BSTabInfo("Lagra", new tabPageFinalBurn()));

		    foreach (var flik in _flikar)
		        tab.TabPages.Add(flik.TabPage);
		    _flikAktiv = _flikar[0];
		}

	    public static void prepareGruppGrid( vdXceed.vdPlainGrid ug, bool readOnly )
		{
		    ug.G.ScrollBars = GridScrollBars.ForcedVertical;
		    ug.addColumn("#", typeof (int), 25);
		    var col = ug.addColumn("", typeof (int), 30);
		    col.HorizontalAlignment = Xceed.Grid.HorizontalAlignment.Center;
		    col.CellViewer = new GridStatusCellPainter();
		    ug.addColumn("Typ", 35);
		    ug.addColumn("Grupp", 150);
		    ug.addColumn("Slogan", 70);
		    ug.addColumn("På b", typeof (int), 35);
		    ug.addColumn("Frv", typeof (int), 35);
		    ug.addColumn("Utg", typeof (int), 35);
		    ug.addColumn("GEx", typeof (int), 35);
		    ug.addColumn("", 20);
            ug.addColumn("", 20);
            foreach (Column c in ug.G.Columns)
		        c.ReadOnly = readOnly || (c.Index != 0 && c.Index != 8);
		    ug.setColumnFullWidth(3);
		}

	    /// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tab = new System.Windows.Forms.TabControl();
			this.SuspendLayout();
			// 
			// tab
			// 
			this.tab.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tab.Location = new System.Drawing.Point( 0, 0 );
			this.tab.Name = "tab";
			this.tab.SelectedIndex = 0;
			this.tab.Size = new System.Drawing.Size( 1219, 680 );
			this.tab.TabIndex = 4;
			this.tab.SelectedIndexChanged += new System.EventHandler( this.tab_SelectedIndexChanged );
			// 
			// frmFärdigställ
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size( 6, 15 );
			this.ClientSize = new System.Drawing.Size( 1219, 680 );
			this.Controls.Add( this.tab );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmFärdigställ";
			this.ResumeLayout( false );

		}
		#endregion

		protected override void resize( Size sz )
		{
			var rect = this.ClientRectangle;
			rect.Inflate( -10, -10 );
			if ( rect.Width>10 && rect.Height>10 )
				tab.SetBounds( rect.X, rect.Y, rect.Width, rect.Height );
		}

		public override void activated()
		{
			base.activated();

			foreach ( var flik in _flikar )
				flik.load();

			_fHasInitialized = true;
		}

		public override void save()
		{
			base.save ();

			var skola = Global.Skola;
			if ( skola==null || !_fHasInitialized )
				return;

			foreach ( var flik in _flikar )
				flik.save();
		}

		protected override void OnDeactivate(EventArgs e)
		{
			base.OnDeactivate (e);
			save();
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed (e);
			save();
		}

		public override void skolaUppdaterad()
		{
			base.skolaUppdaterad ();
			_fHasInitialized = false;
			if ( frmMain.ActiveMdiChild == this )
				activated();
		}


		private void tab_SelectedIndexChanged( object sender, System.EventArgs e )
		{
			Cursor = Cursors.WaitCursor;
			if ( _flikAktiv != null )
				_flikAktiv.save();
			_flikAktiv = _flikar[tab.SelectedIndex];
			_flikAktiv.load();
			Cursor = Cursors.Default;
		}

	}

}

