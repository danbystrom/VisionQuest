using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Photomic.Common;
using Plata.Camera;
using PlataDM;

namespace Plata
{
	public class frmGruppbild : Plata.baseGruppForm
	{
		private System.ComponentModel.IContainer components = null;

		private static Pen[] s_apensFlash = new Pen[]
		{
			new Pen( Color.FromArgb(  0x00, 0x00, 0x00 ), 2 ),
			new Pen( Color.FromArgb(  0x80, 0x00, 0x00 ), 2 ),
			new Pen( Color.FromArgb(  0xFF, 0x00, 0x00 ), 2 ),
			new Pen( Color.FromArgb(  0xFF, 0x00, 0x00 ), 2 ),
			new Pen( Color.FromArgb(  0x80, 0x80, 0x00 ), 2 ),
			new Pen( Color.FromArgb(  0x00, 0xFF, 0x00 ), 2 ),
			new Pen( Color.FromArgb(  0x00, 0xFF, 0x00 ), 2 ),
			new Pen( Color.FromArgb(  0x00, 0x80, 0x80 ), 2 ),
			new Pen( Color.FromArgb(  0x00, 0x00, 0xFF ), 2 ),
			new Pen( Color.FromArgb(  0x00, 0x00, 0xFF ), 2 ),
			new Pen( Color.FromArgb(  0x00, 0x00, 0x80 ), 2 ),
		};

		private enum PortraitViewMode
		{
			None,
			Alphabetic,
			Lineup
		}

		private System.Windows.Forms.Timer tmrFlash;
		private System.Windows.Forms.ContextMenu mnuFoto;
		private System.Windows.Forms.ColumnHeader chTitel;
		private System.Windows.Forms.ColumnHeader chSiffra;

		private System.Windows.Forms.MenuItem mnuFotoRadera;

		private System.Windows.Forms.MenuItem mnuNyrad;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem mnuFotoInfoga;

		private int _nPenFlash = 0;
		private PlataDM.Siffra _siffraSelected;
		private bool _fDraggingCircle;
		private bool _fHideCircles;

		private Rectangle _rectStatusljus;

		static Font _fontNamnskylt = new Font("Arial",6.5f,FontStyle.Regular);
		static Font _fontSiffra = new Font("Arial",12,FontStyle.Bold);
		private System.Windows.Forms.ContextMenu mnuNumrering;
		private System.Windows.Forms.ContextMenu mnuPersonG;
		private System.Windows.Forms.MenuItem mnuStatus;
		private System.Windows.Forms.MenuItem mnuStatusF;
		private System.Windows.Forms.MenuItem mnuStatusS;
		private System.Windows.Forms.MenuItem mnuNum;
		private System.Windows.Forms.MenuItem mnuNumUtplacering;
		private System.Windows.Forms.MenuItem mnuNumNamns�ttning;
		private System.Windows.Forms.MenuItem mnuNumKlart;
		private System.Windows.Forms.MenuItem mnuNumEjNamn;
		private System.Windows.Forms.MenuItem mnuNumEjNum;
		private System.Windows.Forms.MenuItem mnuStatusN;

		private System.Windows.Forms.MenuItem[] mnuRadtyper;
		private System.Windows.Forms.MenuItem menuItem1;

		private string[] _strRadtyper =
		{
			"Bakre raden fr. v.",				//0
			"Mellersta raden fr. v.",		//1
			"Fr�mre raden fr. v.",			//2
			"L�ngst fram fr. v.",				//3
			"Liggande fr. v.",					//4
			"L�ngst bak fr. v.",				//5
			"St�ende fr. v.",						//6
			"Sittande fr. v.",					//7
			"St�ende",									//8
			"Sittande",									//9
			"L�ngst bak",								//10
			"L�ngst fram",							//11
			"Liggande",									//12
			"Fr�n v�nster",							//13
			"-", 
			"Ta bort radbrytning"
		};
		private int[][] _aRadTyperDefault =
			{
				new int[] { 13 },
				new int[] { 0, 2 },
				new int[] { 0, 1, 2 },
				new int[] { 5, 0, 2, 3 }
			};

		private System.Windows.Forms.ContextMenu mnuFoto2;
		private System.Windows.Forms.MenuItem mnuViewFullScreen;
		private System.Windows.Forms.MenuItem mnuClearDigits;
		private System.Windows.Forms.MenuItem menuItem2;
		private vdUsr.FlickerFreePanel pnlPortraits;

		private delegateNyttFoto _delegateNyttFoto;

		private PlataDM.Thumbnails _tnsPortraits;
		private Hashtable _hashPortrait = new Hashtable();  //map thumbnail -> person
		private PortraitViewMode _portraitViewMode = PortraitViewMode.None;
		private Rectangle _rectPV_None, _rectPV_Alphabetic, _rectPV_Lineup;
		private System.Windows.Forms.MenuItem mnuThumbFlytta;
		private System.Windows.Forms.MenuItem mnuGr�kort;
		private System.Windows.Forms.VScrollBar scrPortraits;

		//i VS2005 f�r jag inte l�gga till denna i designern? :-(
		private MenuItem _mnuThumbsLovad;
		private MenuItem _mnuThumbsReserv;

        private Rectangle _rectNamesLeftScrollButton, _rectNamesRightScrollButton;
		private int _nNameScroll;

		private frmGruppbild() : base()
		{
			InitializeComponent();
		}

		public frmGruppbild( Form parent, FlikTyp fliktyp ) : base(parent,fliktyp)
		{
			if ( fliktyp==FlikTyp.GruppbildInne )
			{
				_strCaption = "GRUPP INNE";
				_presetType = eosPresets.PresetType.IndoorGroup;
			}
			else
			{
				_strCaption = "GRUPP UTE";
				_presetType = eosPresets.PresetType.OutdoorGroup;
			}

			txtSlogan.Visible = true;  //inherited control

			// This call is required by the Windows Form Designer.
			InitializeComponent();
			this.Bounds = parent.ClientRectangle;
			this.PerformLayout();

			picFoto.MouseDown += new MouseEventHandler(picFoto_MouseDown);
			picFoto.MouseMove += new MouseEventHandler(picFoto_MouseMove);
			picFoto.Paint += new PaintEventHandler(picFoto_Paint);
			picNames.Paint += new PaintEventHandler(picNames_Paint);
			picNames.MouseDown += new MouseEventHandler(picNames_MouseDown);

			this.chF�rnamn.Width = 80;
			this.chEfternamn.Width = 80;

			this.chTitel = new System.Windows.Forms.ColumnHeader();
			this.chTitel.Text = "Titel";
			this.chTitel.Width = 64;
			this.lv.Columns.Add( chTitel );

			this.chSiffra = new System.Windows.Forms.ColumnHeader();
			this.chSiffra.Text = "Siffra";
			this.chSiffra.Width = 40;
			this.chSiffra.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.lv.Columns.Add( chSiffra );

			mnuPerson.Popup += new EventHandler(mnuPerson_Popup);
			mnuThumb.Popup +=new EventHandler(mnuThumb_Popup);

			mnuNumEjNamn.Click += new EventHandler(mnuNu_Click);
			mnuNumEjNum.Click += new EventHandler(mnuNu_Click);
			mnuNumKlart.Click += new EventHandler(mnuNu_Click);
			mnuNumNamns�ttning.Click += new EventHandler(mnuNu_Click);
			mnuNumUtplacering.Click += new EventHandler(mnuNu_Click);

			mnuNumEjNamn.Select += new EventHandler(mnuNu_Select);
			mnuNumEjNum.Select += new EventHandler(mnuNu_Select);
			mnuNumKlart.Select += new EventHandler(mnuNu_Select);
			mnuNumNamns�ttning.Select += new EventHandler(mnuNu_Select);
			mnuNumUtplacering.Select += new EventHandler(mnuNu_Select);

			mnuPerson.MenuItems.Add( 0, mnuStatus );
			mnuPerson.MenuItems.Add( 1, new MenuItem("-") );

			_mnuThumbsLovad = new MenuItem( "Lovad", mnuThumbsLovad_Click );
			mnuThumb.MenuItems.Add( 0, _mnuThumbsLovad );
			_mnuThumbsReserv = new MenuItem( "Reserv", mnuThumbsReserv_Click );
			mnuThumb.MenuItems.Add( 0, _mnuThumbsReserv );

			mnuRadtyper = new MenuItem[_strRadtyper.Length];
			for ( var i=0 ; i<_strRadtyper.Length ; i++ )
                mnuRadtyper[i] = new MenuItem(_strRadtyper[i], mnuRadtyper_Click);
			mnuNyrad.MenuItems.AddRange( mnuRadtyper );

			addSubmenuForImageCopy( mnuThumb );
		}

		protected override Thumbnail getSelectedThumbnail()
		{
			return _grupp.Thumbnails[_strThumbnailkeyRightClicked];
		}

        protected override IEnumerable<Thumbnail> getSelectedThumbnails()
        {
            return SelectedThumbnailKeys.Select(key => _grupp.Thumbnails[key]);
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
            this.components = new System.ComponentModel.Container();
            this.tmrFlash = new System.Windows.Forms.Timer(this.components);
            this.mnuFoto = new System.Windows.Forms.ContextMenu();
            this.mnuNyrad = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.mnuFotoInfoga = new System.Windows.Forms.MenuItem();
            this.mnuFotoRadera = new System.Windows.Forms.MenuItem();
            this.mnuNumrering = new System.Windows.Forms.ContextMenu();
            this.mnuNumUtplacering = new System.Windows.Forms.MenuItem();
            this.mnuNumNamns�ttning = new System.Windows.Forms.MenuItem();
            this.mnuNumKlart = new System.Windows.Forms.MenuItem();
            this.mnuNum = new System.Windows.Forms.MenuItem();
            this.mnuNumEjNamn = new System.Windows.Forms.MenuItem();
            this.mnuNumEjNum = new System.Windows.Forms.MenuItem();
            this.mnuPersonG = new System.Windows.Forms.ContextMenu();
            this.mnuStatus = new System.Windows.Forms.MenuItem();
            this.mnuStatusF = new System.Windows.Forms.MenuItem();
            this.mnuStatusS = new System.Windows.Forms.MenuItem();
            this.mnuStatusN = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mnuFoto2 = new System.Windows.Forms.ContextMenu();
            this.mnuViewFullScreen = new System.Windows.Forms.MenuItem();
            this.mnuClearDigits = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.pnlPortraits = new vdUsr.FlickerFreePanel();
            this.scrPortraits = new System.Windows.Forms.VScrollBar();
            this.mnuThumbFlytta = new System.Windows.Forms.MenuItem();
            this.mnuGr�kort = new System.Windows.Forms.MenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.picFoto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNames)).BeginInit();
            this.pnlPortraits.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboGrupp
            // 
            this.cboGrupp.Size = new System.Drawing.Size(228, 22);
            this.toolTip1.SetToolTip(this.cboGrupp, "Grupper");
            // 
            // txtSlogan
            // 
            this.txtSlogan.Size = new System.Drawing.Size(228, 21);
            this.toolTip1.SetToolTip(this.txtSlogan, "Slogan");
            this.txtSlogan.TextChanged += new System.EventHandler(this.txtSlogan_TextChanged);
            // 
            // lblAntalIKlass
            // 
            this.toolTip1.SetToolTip(this.lblAntalIKlass, "Antal personer i gruppen");
            // 
            // mnuThumb
            // 
            this.mnuThumb.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuGr�kort,
            this.mnuThumbFlytta});
            // 
            // mnuThumbRadera
            // 
            this.mnuThumbRadera.Index = 1;
            // 
            // tmrFlash
            // 
            this.tmrFlash.Interval = 200;
            this.tmrFlash.Tick += new System.EventHandler(this.tmrFlash_Tick);
            // 
            // mnuFoto
            // 
            this.mnuFoto.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuNyrad,
            this.menuItem3,
            this.mnuFotoInfoga,
            this.mnuFotoRadera});
            // 
            // mnuNyrad
            // 
            this.mnuNyrad.Index = 0;
            this.mnuNyrad.Text = "Nyrad";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "-";
            // 
            // mnuFotoInfoga
            // 
            this.mnuFotoInfoga.Index = 2;
            this.mnuFotoInfoga.Text = "Infoga";
            this.mnuFotoInfoga.Click += new System.EventHandler(this.mnuFotoInfoga_Click);
            // 
            // mnuFotoRadera
            // 
            this.mnuFotoRadera.Index = 3;
            this.mnuFotoRadera.Text = "Radera";
            this.mnuFotoRadera.Click += new System.EventHandler(this.mnuFotoRadera_Click);
            // 
            // mnuNumrering
            // 
            this.mnuNumrering.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuNumUtplacering,
            this.mnuNumNamns�ttning,
            this.mnuNumKlart,
            this.mnuNum,
            this.mnuNumEjNamn,
            this.mnuNumEjNum});
            // 
            // mnuNumUtplacering
            // 
            this.mnuNumUtplacering.Index = 0;
            this.mnuNumUtplacering.Text = "Utplacering";
            // 
            // mnuNumNamns�ttning
            // 
            this.mnuNumNamns�ttning.Index = 1;
            this.mnuNumNamns�ttning.Text = "Namns�ttning";
            // 
            // mnuNumKlart
            // 
            this.mnuNumKlart.Index = 2;
            this.mnuNumKlart.Text = "Klart";
            // 
            // mnuNum
            // 
            this.mnuNum.Index = 3;
            this.mnuNum.Text = "-";
            // 
            // mnuNumEjNamn
            // 
            this.mnuNumEjNamn.Index = 4;
            this.mnuNumEjNamn.Text = "Ska inte namns�ttas";
            // 
            // mnuNumEjNum
            // 
            this.mnuNumEjNum.Index = 5;
            this.mnuNumEjNum.Text = "Ska inte numreras";
            // 
            // mnuPersonG
            // 
            this.mnuPersonG.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuStatus});
            // 
            // mnuStatus
            // 
            this.mnuStatus.Index = 0;
            this.mnuStatus.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuStatusF,
            this.mnuStatusS,
            this.mnuStatusN});
            this.mnuStatus.Text = "�ndra status";
            // 
            // mnuStatusF
            // 
            this.mnuStatusF.Index = 0;
            this.mnuStatusF.Text = "Fr�nvarande";
            this.mnuStatusF.Click += new System.EventHandler(this.mnuStatusF_Click);
            // 
            // mnuStatusS
            // 
            this.mnuStatusS.Index = 1;
            this.mnuStatusS.Text = "Slutat";
            this.mnuStatusS.Click += new System.EventHandler(this.mnuStatusS_Click);
            // 
            // mnuStatusN
            // 
            this.mnuStatusN.Index = 2;
            this.mnuStatusN.Text = "Normal";
            this.mnuStatusN.Click += new System.EventHandler(this.mnuStatusN_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = -1;
            this.menuItem1.Text = "-";
            // 
            // mnuFoto2
            // 
            this.mnuFoto2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuViewFullScreen,
            this.mnuClearDigits});
            // 
            // mnuViewFullScreen
            // 
            this.mnuViewFullScreen.Index = 0;
            this.mnuViewFullScreen.Shortcut = System.Windows.Forms.Shortcut.F5;
            this.mnuViewFullScreen.Text = "Visa fullsk�rm";
            this.mnuViewFullScreen.Click += new System.EventHandler(this.mnuViewFullScreen_Click);
            // 
            // mnuClearDigits
            // 
            this.mnuClearDigits.Index = 1;
            this.mnuClearDigits.Text = "Radera alla siffror";
            this.mnuClearDigits.Click += new System.EventHandler(this.mnuClearDigits_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = -1;
            this.menuItem2.Text = "-";
            // 
            // pnlPortraits
            // 
            this.pnlPortraits.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(90)))), ((int)(((byte)(135)))));
            this.pnlPortraits.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            this.pnlPortraits.BackgroundMode = vdUsr.BackgroundMode.Gradient;
            this.pnlPortraits.Controls.Add(this.scrPortraits);
            this.pnlPortraits.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlPortraits.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlPortraits.Location = new System.Drawing.Point(684, 0);
            this.pnlPortraits.Name = "pnlPortraits";
            this.pnlPortraits.Size = new System.Drawing.Size(208, 566);
            this.pnlPortraits.TabIndex = 11;
            this.pnlPortraits.Visible = false;
            this.pnlPortraits.PaintShadowImage += new vdUsr.PaintShadowImage(this.pnlPortraits_PaintShadowImage);
            this.pnlPortraits.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlPortraits_MouseDown);
            // 
            // scrPortraits
            // 
            this.scrPortraits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.scrPortraits.LargeChange = 1;
            this.scrPortraits.Location = new System.Drawing.Point(188, 536);
            this.scrPortraits.Name = "scrPortraits";
            this.scrPortraits.Size = new System.Drawing.Size(16, 28);
            this.scrPortraits.TabIndex = 0;
            this.scrPortraits.ValueChanged += new System.EventHandler(this.scrPortraits_ValueChanged);
            // 
            // mnuThumbFlytta
            // 
            this.mnuThumbFlytta.Index = 2;
            this.mnuThumbFlytta.Text = "Flytta till annan grupp";
            this.mnuThumbFlytta.Click += new System.EventHandler(this.mnuThumbFlytta_Click);


		    txtGratisEx.Visible = true;

            // 
            // mnuGr�kort
            // 
            this.mnuGr�kort.Index = 0;
            this.mnuGr�kort.Text = "Gr�kort";
            this.mnuGr�kort.Click += new System.EventHandler(this.mnuGr�kort_Click);
            // 
            // frmGruppbild
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(892, 566);
            this.Controls.Add(this.pnlPortraits);
            this.Name = "frmGruppbild";
            this.Load += new System.EventHandler(this.frmGruppbild_Load);
            this.Controls.SetChildIndex(this.pnlPortraits, 0);
            this.Controls.SetChildIndex(this.lv, 0);
            this.Controls.SetChildIndex(this.cboGrupp, 0);
            this.Controls.SetChildIndex(this.picFoto, 0);
            this.Controls.SetChildIndex(this.picNames, 0);
            this.Controls.SetChildIndex(this.scrThumb, 0);
            this.Controls.SetChildIndex(this.txtSlogan, 0);
            this.Controls.SetChildIndex(this.lblAntalIKlass, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picFoto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNames)).EndInit();
            this.pnlPortraits.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region Events

		private void frmGruppbild_Load(object sender, System.EventArgs e)
		{
			tmrFlash.Enabled = true;
			using ( Graphics g = picNames.CreateGraphics() )
				_nNamnskyltteckenh�jd = (int)g.MeasureString( " ", _fontNamnskylt ).Height-1;
			_delegateNyttFoto = nyttFoto_del2;
		}

		private void picFoto_MouseDown(object sender, MouseEventArgs e)
		{
            if (_grupp == null || SelectedThumbnailKey == null)
				return;

            if (_grupp.Numrering == GruppNumrering.Klar)
                return;

            if (SelectedThumbnailKey.Equals(_grupp.ThumbnailGrayKey))
			{
				Global.showMsgBox( this, "Detta �r gr�kortet! Numrera inte det �r du sn�ll!" );
				return;
			}

			if ( _fHideCircles )
			{
				_fHideCircles = false;
				picFoto.Invalidate();
			}

			_siffraSelected = _grupp.Siffror.hitTest( e.X, e.Y, picFoto.ClientRectangle, out _fDraggingCircle );
			if ( e.Button==MouseButtons.Left && _siffraSelected==null )
			{
				switch ( _grupp.Numrering ) //v�nsterklick p� tom yta
				{
					case GruppNumrering.Utplacering:
					case GruppNumrering.Namns�ttning:
					case GruppNumrering.Klar:
						var siffra = _grupp.Siffror.Add( e.X, e.Y, picFoto.ClientRectangle );
						if ( siffra.NyradText!=null )
						{
							int nRader = _grupp.Siffror.r�knaRader()-1;
							int nNyttIndex = 11;
							switch ( nRader )
							{
								case 0: case 1: case 2: case 3:
									for ( int i=0 ; i<nRader ; i++ )
									{
										var siffraTest =  _grupp.Siffror.h�mtaNyrad(i);
										if ( string.Compare( siffraTest.NyradText, _strRadtyper[_aRadTyperDefault[nRader-1][i] ] ) == 0 )
											siffraTest.NyradText = _strRadtyper[_aRadTyperDefault[nRader][i]];
									}
									nNyttIndex = _aRadTyperDefault[nRader][nRader];
									break;
								case 4:
									Global.showMsgBox( this, "Kom ih�g att kontrollera radbeteckningarna!!!" );
									break;
							}
							siffra.NyradText = _strRadtyper[nNyttIndex];
							resize( this.ClientSize );
						}
						break;
				}
				if ( _grupp.PersonerN�rvarande.Count==_grupp.Siffror.Count )
				{
					_grupp.Numrering = GruppNumrering.Namns�ttning;
					_grupp.AktivSiffraN�sta();
				}
				paintStatusLight( null );
				picNames.Invalidate();
				picFoto.Invalidate();
				resize( this.ClientSize );
			}
			else if ( e.Button==MouseButtons.Left && _siffraSelected!=null )
			{
				//v�nsterklick p� siffra
				if ( _grupp.Numrering==GruppNumrering.Namns�ttning )
				{
					_grupp.AktivSiffra = _siffraSelected;
					picFoto.Invalidate();
				}
				else
					selectPerson( _siffraSelected.Person );
			}
			else if ( e.Button==MouseButtons.Right && _siffraSelected==null )
			{
				//h�gerklick p� tom yta
				mnuFoto2.Show( picFoto, new Point(e.X, e.Y) );
			}
			else if ( e.Button==MouseButtons.Right && _siffraSelected!=null )
			{
				//h�gerklick p� siffra
				//mnuFotoInfoga.Enabled = _grupp.Siffror.Count < _grupp.Personer.Count;
				mnuFotoRadera.Text = "Radera " + _siffraSelected.Etikett;
				mnuFoto.Show( picFoto, new Point(e.X, e.Y) );
			}

		}

		private void picFoto_Paint(object sender, PaintEventArgs e)
		{
			if ( _mode!=FlikMode.Active || _grupp==null )
				return;

			Global.ritaGruppRam( e.Graphics, picFoto.ClientRectangle );

			if ( !_fHideCircles )
				switch ( _grupp.Numrering )
				{
					case GruppNumrering.Utplacering:
					case GruppNumrering.Namns�ttning:
					case GruppNumrering.Klar:
						_grupp.Siffror.paint( e.Graphics, _fontSiffra, picFoto.ClientRectangle, _person );
						break;
				}
		}

		private void tmrFlash_Tick(object sender, System.EventArgs e)
		{
			if ( _grupp==null )
				return;
			if ( _grupp.AktivSiffra==null )
				return;
			using ( var g = picFoto.CreateGraphics() )
			{
				_nPenFlash = (_nPenFlash+1) & s_apensFlash.Length;
				_grupp.AktivSiffra.paint( g, _fontSiffra, picFoto.ClientRectangle, s_apensFlash[_nPenFlash], null );
			}
		}

		private void kollaOmOmarkeradePersonerSkaBliFr�nvarande()
		{
			if ( _grupp.Siffror.Count >= _grupp.PersonerN�rvarande.Count )
				return;
			GruppPersonTyp gt;
			if ( FAskChangePersonStatus.showDialog( this, out gt ) != DialogResult.OK )
				return;
		again:
			foreach ( var person in _grupp.PersonerN�rvarande )
				if ( person.Siffra==null )
				{
					_grupp.�ndraPersontyp( person, gt );
					goto again;
				}
		}

		protected override void grupplista�ndrad()
		{
			�ndraNumreringsstatus( _grupp.Numrering );
		}

		protected override void personVald( bool fNy )
		{
		    pnlPortraits.recreateBackground();
		    picFoto.Invalidate();
		    picNames.Invalidate();

		    if (_person == null || _grupp.AktivSiffra == null)
		        return;
		    if (_grupp.AktivSiffra.Person != null)
		    {
		        if (_person == _grupp.AktivSiffra.Person)
		            return;
		        _grupp.�ndraPersontyp(_grupp.AktivSiffra.Person, GruppPersonTyp.PersonFr�nvarande);
		    }
		    if (_person.Siffra != null && _person.Siffra.Nummer + 1 == _grupp.AktivSiffra.Nummer)
		        return;
		    _grupp.�ndraPersontyp(_person, GruppPersonTyp.PersonNormal);
		    _person.Siffra = _grupp.AktivSiffra;
		    if (_grupp.AktivSiffraN�sta())
		        �ndraNumreringsstatus(GruppNumrering.Klar);
		    updateLV(null);
		}

	    protected override void nyGruppVald()
		{
            if (_grupp == null)
            {
                resizePhotoBox(true, null, false, rightMargin, ClientSize.Height);
                return;
            }

		    txtGratisEx.Text = _grupp.AntalGratisEx >= 0
                        ? _grupp.AntalGratisEx.ToString()
                        : "";
			var tn = _grupp.Thumbnails[_grupp.ThumbnailKey];
			if ( tn!=null )
			{
                SelectedThumbnailKey = tn.Key;
				resizePhotoBox( true, tn.loadViewImage(), false, rightMargin, ClientSize.Height );
			}
			else
			{
                SelectedThumbnailKey = null;
				resizePhotoBox( true, null, false, rightMargin, ClientSize.Height );
			}
			txtSlogan.Text = _grupp.Slogan;
			resize( ClientSize );
			if ( tn!=null )
			{
				_grupp.Thumbnails.ensureVisible( tn );
				if ( _grupp.Thumbnails.FirstImage<scrThumb.Maximum )
					scrThumb.Value = _grupp.Thumbnails.FirstImage;
			}
			Invalidate();
		}

		private void mnuFotoRadera_Click(object sender, System.EventArgs e)
		{
			if ( _siffraSelected==null )
				return;

			_grupp.Siffror.Remove( _siffraSelected );
			_grupp.AktivSiffra = null;
			�ndraNumreringsstatus( GruppNumrering.Utplacering );
		}

		protected override void paint(PaintEventArgs e)
		{
			if ( _grupp==null )
				return;

			if ( _portraitViewMode==PortraitViewMode.None )
			{
				int x = this.Width - pnlPortraits.Width;
				e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				drawPortraitButtons( e.Graphics, x+4, this.Width-4, 1 );
			}

			paintStatusLight( e.Graphics );
			_grupp.Thumbnails.paintWithSelections(
				e.Graphics,
                SelectedThumbnailKeys,
				_grupp.ThumbnailKey,
				_grupp.ThumbnailGrayKey );
			var tn = _grupp.Thumbnails[_grupp.ThumbnailKey];
			if ( tn != null && _grupp.Lovad )
			{
				e.Graphics.FillEllipse( Brushes.Yellow, tn.Bounds.X + 2, tn.Bounds.Y + 2, 11, 11 );
				e.Graphics.DrawEllipse( Pens.DarkBlue, tn.Bounds.X + 2, tn.Bounds.Y + 2, 11, 11 );
			}

			if ( _grupp.ThumbnailLocked )
				paintLockedThumbnail( e.Graphics, _grupp.selectedThumbnail );
		}

		private void paintNamnRad( Graphics g, string s, int nY, ref int nLeftMost, ref int nRightMost )
		{
			int nTextWidth = (int)g.MeasureString( s, _fontNamnskylt ).Width;
			int nMiddle = picNames.ClientSize.Width/2 - _nNameScroll;
			g.DrawString( s, _fontNamnskylt, SystemBrushes.WindowText, nMiddle - nTextWidth/2, nY );
			nLeftMost = Math.Min( nLeftMost, nMiddle - nTextWidth/2 );
			nRightMost = Math.Max( nRightMost, nMiddle + nTextWidth/2 );
		}

		private void picNames_Paint(object sender, PaintEventArgs e)
		{
			if ( _mode!=FlikMode.Active || _grupp==null )
				return;

			string strRadTitel = null;
			string strRad = null;
			Person person;

			int nLeftMost = 0;
			int nRightMost = 0;
			int nY = 0;

			switch ( _grupp.Numrering )
			{
				case GruppNumrering.EjNamns�ttning:
					return;
				case GruppNumrering.EjNumrering:
					e.Graphics.DrawString( "P� BILDEN: ...", _fontNamnskylt, SystemBrushes.WindowText, picNames.ClientRectangle, Util.sfUC );
					return;
				default:
					foreach( Siffra siffra in _grupp.Siffror )
					{
						if ( siffra.NyradText!=null )
						{
							if ( strRadTitel!=null )
							{
								paintNamnRad( e.Graphics, strRadTitel + ": " + strRad, nY, ref nLeftMost, ref nRightMost );
								nY += _nNamnskyltteckenh�jd;
								strRad = null;
							}
							strRadTitel = siffra.NyradText;
						}
						else if ( strRad!=null )
							strRad += "  ";
						strRad += siffra.Etikett + ". ";
						if ( (person=siffra.Person)!=null )
							strRad += person.Namn;
					}
					if ( strRad!=null )
						paintNamnRad( e.Graphics, strRadTitel + ": " + strRad, nY, ref nLeftMost, ref nRightMost );
					break;
			}

			int nBW = SystemInformation.HorizontalScrollBarArrowWidth;
			int nBH = SystemInformation.HorizontalScrollBarHeight;

			if ( nLeftMost<0 )
			{
				_rectNamesLeftScrollButton = new Rectangle( 1, picNames.ClientSize.Height-nBH-1, nBW, nBH );
				ControlPaint.DrawScrollButton( e.Graphics, _rectNamesLeftScrollButton, ScrollButton.Left, ButtonState.Normal );
			}
			else
				_rectNamesLeftScrollButton = Rectangle.Empty;

			if ( nRightMost>picNames.ClientSize.Width )
			{
				_rectNamesRightScrollButton = new Rectangle( picNames.ClientSize.Width-nBW-1, picNames.ClientSize.Height-nBH-1, nBW, nBH );
				ControlPaint.DrawScrollButton( e.Graphics, _rectNamesRightScrollButton, ScrollButton.Right, ButtonState.Normal );
			}
			else
				_rectNamesRightScrollButton = Rectangle.Empty;

		}

		private void mnuFotoInfoga_Click(object sender, System.EventArgs e)
		{
			_grupp.Siffror.insertBefore( _siffraSelected );
			updateLV(null);
			picFoto.Invalidate();
			picNames.Invalidate();
			paintStatusLight(null);
		}

		private void picFoto_MouseMove(object sender, MouseEventArgs e)
		{
			if ( _grupp==null )
				return;
			switch ( e.Button )
			{
				case MouseButtons.None:
					bool fX;
					if ( _grupp.Siffror.hitTest( e.X, e.Y, picFoto.ClientRectangle, out fX ) != null )
						picFoto.Cursor = Cursors.Hand;
					else
						picFoto.Cursor = Cursors.Default;
					break;
				case MouseButtons.Left:
					if ( _siffraSelected!=null )
					{
						Rectangle rectInvalidate;
						if ( _fDraggingCircle )
							_siffraSelected.moveCircle( e.X, e.Y, picFoto.ClientRectangle, out rectInvalidate );
						else
							_siffraSelected.moveDigit( e.X, e.Y, picFoto.ClientRectangle, out rectInvalidate );
						picFoto.Invalidate( rectInvalidate );
					}
					break;
			}
		}

		#endregion

		override protected void thumbnailClicked( PlataDM.Thumbnail tn, bool fDoubleClick )
		{
            SelectedThumbnailKey = tn.Key;
			if ( fDoubleClick || _grupp.ThumbnailKey==null )
			{
                if (_grupp.ThumbnailKey != SelectedThumbnailKey)
				{
					if ( _grupp.ThumbnailLocked && Form.ModifierKeys != Keys.Control )
					{
						Global.showMsgBox( this, "Eftersom bilden �r l�st m�ste du h�lla in Ctrl-tangenten samtidigt som du dubbelklickar f�r att byta bild!" );
						return;
					}
                    _grupp.ThumbnailKey = SelectedThumbnailKey;
					updateLV( null );
				}
				_grupp.ThumbnailLocked = true;
			}
			resizePhotoBox( true, tn.loadViewImage(), false );
			Invalidate();
		}

		private int rightMargin
		{
			get
			{
				if ( pnlPortraits.Visible )
					return this.ClientSize.Width - pnlPortraits.Width;
				else
					return this.ClientSize.Width;
			}
		}

		protected override void resize( Size sz )
		{
			base.resize( sz );

			_rectStatusljus = new Rectangle( lblAntalIKlass.Right-20, txtSlogan.Top, 20, 20 );
            txtGratisEx.Bounds = new Rectangle(
                cmdManageGroups.Left,
                cboGrupp.Bottom + v�nstermarginal,
                cmdManageGroups.Width,
                txtSlogan.Height );
			txtSlogan.Bounds = new Rectangle(
				v�nstermarginal,
				cboGrupp.Bottom + v�nstermarginal,
                cboGrupp.Width,
				txtSlogan.Height );

			resizePhotoBoxes( true, false, rightMargin, sz.Height );

			if ( _tnsPortraits!=null )
			{
				_tnsPortraits.layoutImages( 2, 22, pnlPortraits.Width, pnlPortraits.Height-4 );
				try
				{
					scrPortraits.Value = 0;
					scrPortraits.Maximum = _tnsPortraits.MaxScroll;
				}
				catch
				{
				}
			}
		}

		public override void activated()
		{
			base.activated ();
			eosPresets.ApplyPreset(_presetType,frmMain.Camera);
		}

		public override void nyttFoto( bool fInternal, byte[] jpgData, byte[] rawData )
		{
            picFoto.Invoke(_delegateNyttFoto, new object[] { fInternal, jpgData, rawData });
		}

        private void nyttFoto_del2(bool fInternal, byte[] jpgData, byte[] rawData)
        {
            if (_grupp == null)
            {
                Global.showMsgBox(this, "Du m�ste v�lja en grupp f�rst!");
                return;
            }

            //vi m�ste klara oss utan RAW om kamerainst�llningen s� s�ger!
            if (rawData == null || rawData.Length == 0)
                if (!eosPresets.UsesRaw(_presetType, frmMain.Camera))
                    rawData = new byte[1000];

            if (jpgData == null || rawData == null || jpgData.Length == 0 || rawData.Length == 0)
            {
                Global.showMsgBox(this, "Till gruppbilder beh�vs b�de RAW och JPG!");
                return;
            }

            if (!fInternal && !FKollaKamera.kollaKamera(frmMain, _presetType))
                return;

            if (!fInternal)
            {
                Global.MediaPlayer.Open(Global.Preferences.GroupSoundLong);
                Global.MediaPlayer.Play();
            }

            var jpgFile = Global.Skola.HomePathCombine(string.Format("g_{0}.jpg", Guid.NewGuid()));
            File.WriteAllBytes(jpgFile, jpgData);
            var rawFile = Path.ChangeExtension(jpgFile, ".cr2");

            var bmpPlus = new Bitmap(new MemoryStream(jpgData));
            var bmpCache = new Bitmap(bmpPlus, Global.Gruppfotobredd, bmpPlus.Size.Height*Global.Gruppfotobredd/bmpPlus.Size.Width);
            bmpCache.Save(
                Path.Combine(Path.GetDirectoryName(jpgFile), "cache\\" + Path.GetFileName(jpgFile)),
                ImageFormat.Jpeg);
            var tn = _grupp.Thumbnails.addImage(jpgFile, rawFile, bmpCache);

            if (!fInternal)
                frmZoom.gotNewImage(tn, bmpPlus);
            bmpPlus.Dispose();

            //v�lj (n�stan) alltid den senast tagna bilden
            if (!_grupp.ThumbnailLocked)
                _grupp.ThumbnailKey = tn.Key;
            SelectedThumbnailKey = tn.Key;
            resizePhotoBox(true, bmpCache, false);
            resize(ClientSize);
            Invalidate();

            makeNewThumbnailVisible(tn);

            if (!fInternal && Global.Preferences.Fullsk�rmsl�geGruppfoto && !frmZoom.isVisible)
                visaFullsk�rm(tn.Key);

            File.WriteAllBytes(rawFile, rawData);
        }

	    protected override void updateLV( Person personVald )
		{
			base.updateLV( personVald );
			picFoto.Invalidate();
			picNames.Invalidate();
			setPortraitViewMode( _portraitViewMode );

			if ( _grupp!=null && _grupp.Numrering == GruppNumrering.Namns�ttning )
				_grupp.AktivSiffraN�sta();
		}

		private void paintStatusLight( Graphics g )
		{
			var fDisposeG = false;

			if ( g==null )
			{
				fDisposeG = true;
				g = this.CreateGraphics();
			}

			PlataDM.Util.paintGroupNumberingSymbol( g, Font, _grupp, _rectStatusljus );
			if ( fDisposeG )
				g.Dispose();
		}

		override protected ListViewItem l�ggPersonTillLV( Person person, string strEtikett )
		{
			var itm = base.l�ggPersonTillLV( person, strEtikett );
			if ( person.Personal && string.IsNullOrEmpty(person.Titel) )
				itm.SubItems.Add( "*personal" );
			else
				itm.SubItems.Add( person.Titel );
			if ( person.Siffra!= null &&
				  _grupp.Numrering!=GruppNumrering.EjNamns�ttning &&
				  _grupp.Numrering!=GruppNumrering.EjNumrering )
				itm.SubItems.Add( person.Siffra.Etikett );
			else
				itm.SubItems.Add( strEtikett );
			return itm;
		}

		override protected Thumbnails getThumbnails()
		{
			if ( _grupp!=null )
				return _grupp.Thumbnails;
			return null;
		}

		private void mnuPerson_Popup(object sender, EventArgs e)
		{
			mnuStatus.Enabled = _person!=null;
		}

		private void mnuStatusF_Click(object sender, System.EventArgs e)
		{
			_grupp.�ndraPersontyp( _person, GruppPersonTyp.PersonFr�nvarande );
			updateLV(null);
		}

		private void mnuStatusS_Click(object sender, System.EventArgs e)
		{
			_grupp.�ndraPersontyp( _person, GruppPersonTyp.PersonSlutat );
			updateLV(null);
		}

		private void mnuStatusN_Click(object sender, System.EventArgs e)
		{
			_grupp.�ndraPersontyp( _person, GruppPersonTyp.PersonNormal );
			updateLV(null);
		}

		private void �ndraNumreringsstatus( GruppNumrering num  )
		{
            switch ( num )
            {
                case GruppNumrering.Klar:
                    kollaOmOmarkeradePersonerSkaBliFr�nvarande();
                    while (_grupp.Siffror.Count > _grupp.PersonerN�rvarande.Count)
                        _grupp.Siffror.Remove(_grupp.Siffror[_grupp.Siffror.Count]);
                    if (_grupp.PersonerN�rvarande.Count != _grupp.Siffror.Count)
                    {
                        num = GruppNumrering.Namns�ttning;
                        break;
                    }
                    goto case GruppNumrering.EjFotad;
                case GruppNumrering.EjFotad:
                case GruppNumrering.EjNamns�ttning:
                    if (_grupp.AntalGratisEx >= 0 || Global.Preferences.Brand == Brand.Photomic )
                        break;
                    Global.showMsgBox(this, "Du m�ste f�rst ange antal �nskat antal gratisexemplar!");
                    return;
            }

		    _grupp.Numrering = num;
            if (num == GruppNumrering.Namns�ttning && _grupp.AktivSiffra == null)
            {
                lv.SelectedItems.Clear();
                _grupp.AktivSiffraN�sta();
            }
		    paintStatusLight(null);
			picFoto.Invalidate();
			picNames.Invalidate();
		    _fAvoidRecursionLV = true;
            updateLV(null);
            _fAvoidRecursionLV = false;
            cboGrupp.Update();
		}

		private void mnuNu_Click(object sender, System.EventArgs e)
		{
			if ( sender==mnuNumEjNamn )
				�ndraNumreringsstatus( GruppNumrering.EjNamns�ttning );
			if ( sender==mnuNumEjNum )
				�ndraNumreringsstatus( GruppNumrering.EjNumrering );
			if ( sender==mnuNumKlart )
				�ndraNumreringsstatus( GruppNumrering.Klar );
			if ( sender==mnuNumNamns�ttning )
				�ndraNumreringsstatus( GruppNumrering.Namns�ttning );
			if ( sender==mnuNumUtplacering )
				�ndraNumreringsstatus( GruppNumrering.Utplacering );
		}

		private void mnuNu_Select(object sender, System.EventArgs e)
		{
			string strText;
			if ( sender==mnuNumEjNamn )
				strText = "V�lj detta alternativ om det inte ska vara n�gra namn alls katalogen.";
			else if ( sender==mnuNumEjNum )
				strText = "V�lj detta alternativ om katalogtexten ska vara \"N�rvarande p� bilden\".";
			else if ( sender==mnuNumKlart )
				strText = "Tvinga fram klarmarkering. Det h�r ska egentligen ske med automatik. Kontrollera att det inte �r du som g�r fel!";
			else if ( sender==mnuNumNamns�ttning )
				strText = "Klicka p� namn i listan f�r att koppla samman med den siffra/cirkel som blinkar.";
			else if ( sender==mnuNumUtplacering )
				strText = "B�rja placera ut siffror/cirklar p� gruppbilden genom att klicka p� personernas ansikten.";
			else
				return;

			setStatusText( "\"" + ((MenuItem)sender).Text + "\": " + strText );
		}

		private void mnuRadtyper_Click( object sender, System.EventArgs e )
		{
			if ( _siffraSelected!=null )
			{
				if ( sender==mnuRadtyper[mnuRadtyper.Length-1] )
					_siffraSelected.NyradText = null;
				else
					_siffraSelected.NyradText = ((MenuItem)sender).Text;
				picNames.Invalidate();
				picFoto.Invalidate();
				resize( ClientSize );
				_siffraSelected = null;
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp (e);
			if ( _grupp==null )
				return;

			switch ( e.Button )
			{
				case  MouseButtons.Right:
					if ( _rectStatusljus.Contains(e.X,e.Y) )
					{
						var num = _grupp.Numrering;
						mnuNumEjNamn.Checked = num==GruppNumrering.EjNamns�ttning;
						mnuNumEjNum.Checked = num==GruppNumrering.EjNumrering;
						mnuNumKlart.Checked = num==GruppNumrering.Klar;
						mnuNumNamns�ttning.Checked = num==GruppNumrering.Namns�ttning;
						mnuNumUtplacering.Checked = num==GruppNumrering.Utplacering;
						mnuNumrering.Show( this, new Point(e.X,e.Y) );
						setStatusText( string.Empty );
					}
					break;
				case MouseButtons.Left:
					if ( _portraitViewMode==PortraitViewMode.None )
						checkPortraitButtons( e.X, e.Y );
					break;
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
		    base.OnKeyDown(e);
		    if (e.Handled)
		        return;
		    switch (e.KeyCode)
		    {
		        case Keys.Enter:
		            if (_grupp == null)
		                return;
		            switch (_grupp.Numrering)
		            {
		                case GruppNumrering.Utplacering:
		                    if (_grupp.Siffror.Count >= 1)
		                        �ndraNumreringsstatus(GruppNumrering.Namns�ttning);
		                    break;
                        case GruppNumrering.Namns�ttning:
		                    personVald(false);
                            break;
                    }
		            e.Handled = true;
		            break;
		        case Keys.F4:
		            _fHideCircles = !_fHideCircles;
		            picFoto.Invalidate();
		            e.Handled = true;
		            break;
		    }
		}

	    private void mnuGr�kort_Click(object sender, System.EventArgs e)
		{
			if ( _strThumbnailkeyRightClicked.Equals(_grupp.ThumbnailGrayKey) )
				_grupp.ThumbnailGrayKey = null;
			else
			{
				if ( _strThumbnailkeyRightClicked.Equals(_grupp.ThumbnailKey) )
					Global.showMsgBox( this, "Du har nu satt gr�kortet till valt fotografi! Gl�m inte att �ndra detta!!!" );
				_grupp.ThumbnailGrayKey = _strThumbnailkeyRightClicked;
			}
			Invalidate();
		}

		private void mnuThumb_Popup(object sender, EventArgs e)
		{
			bool fGray = false;
			bool fLovad = false;
			bool fReserv = false;

			var fEnabled = _strThumbnailkeyRightClicked!=null && _grupp!=null;
			if ( fEnabled )
			{
				fGray = _strThumbnailkeyRightClicked.Equals( _grupp.ThumbnailGrayKey );
				fLovad = _grupp.Lovad && _strThumbnailkeyRightClicked.Equals( _grupp.ThumbnailKey );
				fReserv = _grupp.Thumbnails[_strThumbnailkeyRightClicked].Reserv;
			}
			mnuGr�kort.Enabled = fEnabled;
			mnuGr�kort.Checked = fGray;
			_mnuThumbsLovad.Enabled = fEnabled;
			_mnuThumbsLovad.Checked = fLovad;
			_mnuThumbsReserv.Enabled = fEnabled && !fGray && !fLovad;
			_mnuThumbsReserv.Checked = fReserv;
		}

		private void mnuThumbsLovad_Click( object sender, EventArgs e )
		{
            _grupp.ThumbnailKey = SelectedThumbnailKey;
			_grupp.Lovad = !_grupp.Lovad;
			Invalidate();
		}

        public override void skolaUppdaterad()
        {
            base.skolaUppdaterad();
            _grupp = cboGrupp.SelectedItem as Grupp;
            nyGruppVald();
        }

		private void mnuThumbsReserv_Click( object sender, EventArgs e )
		{
			var tn = _grupp.Thumbnails[_strThumbnailkeyRightClicked];
			tn.Reserv = !tn.Reserv;
			Invalidate();
		}

		private void mnuClearDigits_Click(object sender, System.EventArgs e)
		{
			if ( _grupp.Siffror.Count==0 )
				return;
			_grupp.Siffror.Clear();
			_grupp.AktivSiffra = null;

			while ( _grupp.PersonerFr�nvarande.Count>0 )
				_grupp.�ndraPersontyp( _grupp.PersonerFr�nvarande[0], PlataDM.GruppPersonTyp.PersonNormal );
			while ( _grupp.PersonerSlutat.Count>0 )
				_grupp.�ndraPersontyp( _grupp.PersonerSlutat[0], PlataDM.GruppPersonTyp.PersonNormal );

			updateLV(null);
			�ndraNumreringsstatus( GruppNumrering.Utplacering );
			paintStatusLight(null);
			picFoto.Invalidate();
			picNames.Invalidate();
		}

		protected override void visaFullsk�rm(string tnKey)
		{
			if (_grupp == null)
				return;
			vdUsr.vdOneShotTimer.start(1, new EventHandler(visaFullsk�rm2), tnKey);
		}

		private void visaFullsk�rm2(
			object sender,
			EventArgs e)
		{
			var tn = frmZoom.showDialog(
				this,
				_grupp.Siffror, 
				_fontSiffra, 
				FlikKategori.Gruppbild, 
				getThumbnails(),
				(sender as vdUsr.vdOneShotTimer).Tag as string,
				null,
				_grupp.ThumbnailKey,
				_grupp.ThumbnailGrayKey );

			if ( tn!=null )
				thumbnailClicked( tn, true );
		}

		private void mnuViewFullScreen_Click(object sender, System.EventArgs e)
		{
            visaFullsk�rm(SelectedThumbnailKey);
		}

		private void txtSlogan_TextChanged(object sender, System.EventArgs e)
		{
			if ( _grupp!=null )
				_grupp.Slogan = txtSlogan.Text;
		}

		public override void selectGroupPerson(PlataDM.Grupp grupp, PlataDM.Person person)
		{
			if ( _grupp!=null && _grupp.AktivSiffra!=null )
				return;  //utplacering av siffror p�g�r... rubba inte mina cirklar! ;-)
			base.selectGroupPerson (grupp, person);
		}

		private void drawPortraitButtons( Graphics g, int x1, int x2, int y )
		{
			var r = new Rectangle( x2-18, y, 18, 18 );
			Color cD = Color.FromArgb( 10,30,45 );
			Color cL = Color.FromArgb( 60,180,255 );
			Color cTS = Color.White;
			Color cTN = Color.White;

			if ( _portraitViewMode==PortraitViewMode.None )
				Util.drawEllipseButton( g, r, cD, cL, cTN, cTS, pnlPortraits.Font, "\x25BC", true );
			else
				Util.drawEllipseButton( g, r, cD, cL, cTN, cTS, pnlPortraits.Font, "\x25B2", false );
			_rectPV_None = r;
			r.Offset( -22, 0 );
			Util.drawEllipseButton( g, r, cD, cL, cTN, cTS, pnlPortraits.Font, "A", _portraitViewMode==PortraitViewMode.Alphabetic );
			_rectPV_Alphabetic = r;
			r.Offset( -22, 0 );
			Util.drawEllipseButton( g, r, cD, cL, cTN, cTS, pnlPortraits.Font, "#", _portraitViewMode==PortraitViewMode.Lineup );
			_rectPV_Lineup = r;
		}

		private void paintPortraits( Graphics g )
		{
			string strKey = null;
			if ( _person!=null )
			{
				strKey = _person.ThumbnailKey;
				if ( strKey==null )
					foreach ( Thumbnail tn in _hashPortrait.Keys )
						if ( _hashPortrait[tn] == _person )
						{
							strKey = tn.Key;
							break;
						}
			}
		    _tnsPortraits.paint(g, new List<string> {strKey});

			foreach ( Thumbnail tn in _hashPortrait.Keys )
				if ( tn.isInView )
				{
					var pers = _hashPortrait[tn] as Person;
					g.DrawString( pers.F�rnamn + "\r\n" + pers.Efternamn, pnlPortraits.Font, Brushes.White,
						tn.Bounds, Util.sfUC );
					if ( _portraitViewMode==PortraitViewMode.Lineup && pers.Siffra!=null )
						g.DrawString( pers.Siffra.Etikett, pnlPortraits.Font, Brushes.White,
							tn.Bounds, Util.sfUL );
				}
		}

		private void pnlPortraits_PaintShadowImage(PaintEventArgs e, bool fResized)
		{
			if ( _tnsPortraits==null || _portraitViewMode==PortraitViewMode.None )
				return;

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			drawPortraitButtons( e.Graphics, 4, pnlPortraits.Width-4, 1 );
			e.Graphics.DrawString( "Portr�tt", this.Font, Brushes.White, 4, 3 );
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

			paintPortraits( e.Graphics );
		}

		private void setPortraitViewMode( PortraitViewMode pvm )
		{
			_portraitViewMode = pvm;
			_hashPortrait.Clear();
			switch ( pvm )
			{
				case PortraitViewMode.None:
					this.Invalidate();
					pnlPortraits.Visible = false;
					resize( this.ClientSize );
					_tnsPortraits = null;
					return;
				case PortraitViewMode.Alphabetic:
				{
					var al = new List<Person>( _grupp.AllaPersoner );
					al.Sort();
					_tnsPortraits = new Thumbnails( null, Global.Skola, 66, 100, 2 );
					foreach ( var pers in al )
						addPortraitThumbnail( pers );
					break;
				}
				case PortraitViewMode.Lineup:
				{
					var alAll = new List<Person>( _grupp.AllaPersoner );
					_tnsPortraits = new Thumbnails( null, Global.Skola, 66, 100, 2 );
					foreach ( Siffra siffra in _grupp.Siffror )
						if ( siffra.Person!=null )
						{
							addPortraitThumbnail( siffra.Person );
							alAll.Remove( siffra.Person );
						}
					alAll.Sort();
					foreach ( var pers in alAll )
						addPortraitThumbnail( pers );
					break;
				}
			}

			pnlPortraits.Visible = true;
			resize( this.ClientSize );
			pnlPortraits.recreateBackground();
		}

		private void addPortraitThumbnail( Person pers )
		{
			var tn = pers.Thumbnails[pers.ThumbnailKey];
			if ( tn==null )
				tn = new Thumbnail(Global.Skola.HomePath,66,100,_tnsPortraits.Count.ToString());
			_tnsPortraits.add( tn );
			_hashPortrait.Add( tn, pers );
		}

		private bool checkPortraitButtons( int x, int y )
		{
			if ( _rectPV_None.Contains(x,y) )
				setPortraitViewMode( PortraitViewMode.None );
			else if ( _rectPV_Alphabetic.Contains(x,y) )
				setPortraitViewMode( PortraitViewMode.Alphabetic );
			else if ( _rectPV_Lineup.Contains(x,y) )
				setPortraitViewMode( PortraitViewMode.Lineup );
			else
				return false;
			return true;
		}

		private void pnlPortraits_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if ( checkPortraitButtons( e.X, e.Y ) )
				return;
			if ( _tnsPortraits==null )
				return;
			var tn = _tnsPortraits.hitTest( e.X, e.Y );
			if ( tn==null )
				return;
			selectPerson( _hashPortrait[tn] as Person );
		}

		private void scrPortraits_ValueChanged(object sender, System.EventArgs e)
		{
			_tnsPortraits.FirstImage = scrPortraits.Value * _tnsPortraits.ImagesOnOneRow;
			pnlPortraits.recreateBackground();
		}

		protected override void tnRightClick( Thumbnail tn, Point p )
		{
			_strThumbnailkeyRightClicked = tn.Key;
			int nFlyttaIndex = mnuThumbFlytta.Index;

			mnuThumb.MenuItems.RemoveAt( nFlyttaIndex );
			mnuThumbFlytta.MenuItems.Clear();
			foreach ( var grupp in Global.Skola.Grupper )
				if ( grupp!=_grupp && grupp.GruppTyp==GruppTyp.GruppNormal )
                    mnuThumbFlytta.MenuItems.Add(new MenuItem(grupp.Namn, mnuThumbFlytta_Click));
			mnuThumb.MenuItems.Add( nFlyttaIndex, mnuThumbFlytta );
			base.tnRightClick( tn, p );
		}

		private void mnuThumbFlytta_Click(object sender, System.EventArgs e)
		{
			var strGruppnamnTill = ((MenuItem)sender).Text;

			foreach ( var gruppTill in Global.Skola.Grupper )
				if ( gruppTill.Namn==strGruppnamnTill )  //h�r f�rlitar jag mig p� att inte tv� grupper heter likadant! :-(
				{
					if ( _grupp.ThumbnailKey==_strThumbnailkeyRightClicked )
					{
						_grupp.ThumbnailKey = null;
                        SelectedThumbnailKey = null;
						resizePhotoBox( true, null, false );
					}
					if ( _grupp.ThumbnailGrayKey==_strThumbnailkeyRightClicked )
						_grupp.ThumbnailGrayKey = null;

					var tnsFr�n = getThumbnails();
					var tnsTill = gruppTill.Thumbnails;

					var tn = tnsFr�n[_strThumbnailkeyRightClicked];
					if ( tn==null )
						return;
					tnsFr�n.Remove( tn );
					tnsTill.add( tn );

					_strThumbnailkeyRightClicked = null;
					Invalidate();

					break;
				}
		}

		protected override bool deleteThumbnail(PlataDM.Thumbnail tn, bool fQuery )
		{
			if ( tn.Key == _grupp.ThumbnailKey && tn.Key == _grupp.ThumbnailGrayKey )
			{
				Global.showMsgBox( this, "Du f�r inte radera en bild som anv�nds!" );
				return false;
			}
			if ( fQuery )
				return true;

            if (SelectedThumbnailKey == _strThumbnailkeyRightClicked)
			{
                SelectedThumbnailKey = null;
				resizePhotoBox( true, null, false );
			}
			_grupp.Thumbnails.Delete( tn );
			Invalidate();
			return true;
		}

		private void picNames_MouseDown(object sender, MouseEventArgs e)
		{
			if ( _rectNamesLeftScrollButton.Contains(e.X,e.Y) )
				_nNameScroll -= 30;
			if ( _rectNamesRightScrollButton.Contains(e.X,e.Y) )
				_nNameScroll += 30;
			picNames.Invalidate();
		}
	}

}
