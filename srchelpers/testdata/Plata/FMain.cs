using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Photomic.ArchiveStuff.Core;
using Photomic.Bildmodul.Generator;
using Photomic.Common;
using Plata.ImageStuff;
using Timer = System.Windows.Forms.Timer;

namespace Plata
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>

	public class FMain : Form
	{
		private PictureBox picTop;

		private Region _ButtonShape;
		private Font _ButtonFont;
		private Color _ButtonColor = Color.FromArgb( 160, 128, 128 );
		private MainMenu mnuMain;

		private frmTitle _frmDummy;
		private System.ComponentModel.IContainer components;

		private readonly baseFlikForm[] _flikar = new baseFlikForm[10];
		private baseFlikForm _aktivFlik;
		private MenuItem mnuArkivAvsluta;

		private StatusBar sbr;
		private ImageList imlSbr;
		private StatusBarPanel sbpIkon;
		private StatusBarPanel sbpMain;
		private StatusBarPanel sbpKameraInfo;
		private StatusBarPanel sbpTime;
		private StatusBarPanel sbpDate;
		private StatusBarPanel sbpSkola;
		private MenuItem mnuÖppnaSkola;
		private MenuItem mnuInställningar;
		private Timer tmrOneShot;
		private Timer tmrContinuous;

        private vdCamera.vdCamera _camera;
		private readonly Icon _icoKameraFull;
	    private readonly Icon _icoKameraHalv;
	    private int _nAutoSpar = 0;
		private int _nRefreshDiskFreeSpace = 0;
		private int _nStatusProgress;
		private MenuItem mnuUppdateraProgram;
		private MenuItem mnuVerktyg;
		private MenuItem mnuImporteraBilder;
		private bool _fEnded = false;
        private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem mnuDefragmentera;
		private System.Windows.Forms.MenuItem mnuArkiv;
		private System.Windows.Forms.MenuItem mnuFlikVal1;
		private System.Windows.Forms.MenuItem mnuFlikVal2;
		private System.Windows.Forms.MenuItem mnuHelpKeyboard;
		private System.Windows.Forms.MenuItem mnuHelpManual;
		private System.Windows.Forms.MenuItem mnuToolsCameraSettings;
		private System.Windows.Forms.MenuItem mnuFileImportNames;
		private System.Windows.Forms.MenuItem mnuFlik;
		private System.Windows.Forms.MenuItem mnuCamera;
		private System.Windows.Forms.MenuItem mnuDisconnectCamera;
		private System.Windows.Forms.MenuItem mnuVerktygOmstart;
		private System.Windows.Forms.StatusBarPanel sbpACDC;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem mnuImporteraBildfiler;
		private System.Windows.Forms.MenuItem mnuEditSearch;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem mnuSearchNext;
		private System.Windows.Forms.MenuItem mnuEdit;
		private System.Windows.Forms.MenuItem mnuFlikVal0;
		private System.Windows.Forms.StatusBarPanel sbpFotograf;
		private System.Windows.Forms.MenuItem mnuStorage;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem mnuBackup;
		private System.Windows.Forms.MenuItem mnuBurnDirect;

		private System.Windows.Forms.MenuItem mnuFlikVal4;
		private System.Windows.Forms.MenuItem mnuFlikVal5;
        private System.Windows.Forms.MenuItem mnuFlikVal6;
		private System.Windows.Forms.MenuItem mnuFlikVal8;
		private System.Windows.Forms.MenuItem mnuFlikVal9;
		private System.Windows.Forms.MenuItem mnuFlikVal3;
		private System.Windows.Forms.MenuItem mnuBurnOrder;
		private System.Windows.Forms.StatusBarPanel sbpSpeaker;

		private static bool s_fDontReboot = false;
		private Bitmap _bmpSpeaker;
		private System.Windows.Forms.StatusBarPanel sbpDisk;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem mnuHelpSport;
        private System.Windows.Forms.MenuItem mnuSave;
		private MenuItem menuItem6;
		private MenuItem mnuCopyOrderFile;
		private MenuItem menuItem5;
		private MenuItem munPhotoArkivPrint;
		private MenuItem menuItem7;
        private MenuItem mnuHelpVersions;
		private MenuItem mnuVerktygD50;
		private MenuItem mnuStandbylista;
		private MenuItem menuItem8;
		private MenuItem menuItem12;
		private MenuItem mnuStudentCardPrint;
		private MenuItem mnuStudentCardPrinterQue;
		private MenuItem menuItem10;
		private MenuItem mnuShowHistogram;

		public static FMain theOneForm;
		private MenuItem mnuArkivEgenskaper;
        private MenuItem mnuHelpEos5D;
        private MenuItem mnuHelpEos1DsMarkII;
        private MenuItem mnuKommunikation;
        private MenuItem mnuWiFiInloggning;

		private Dialogs.FHistogram _histogramForm = null;

		public FMain()
		{
			InitializeComponent();

			new ProtectedIdExtra();
			Images.load( this.GetType() );

		    Text = string.Format("{0} {1} ver: {2}", Global.Preferences.Brand,  AppSpecifics.Name, AppSpecifics.Version);
            theOneForm = this;

			picTop.Paint += new PaintEventHandler( this.picTop_Paint );
			picTop.MouseMove += new MouseEventHandler( picTop_MouseMove );
			picTop.MouseDown += new MouseEventHandler( picTop_MouseDown );
			picTop.MouseLeave += new EventHandler( picTop_MouseLeave );

			const int nH = 24, nW = 78;
			var r = new RectangleF( nH / 2, 0, nW, nH );
			var gp = new GraphicsPath( FillMode.Winding );
			gp.AddRectangle( r );
			gp.AddEllipse( 0, 0, nH, nH );
			gp.AddEllipse( nW, 0, nH, nH );
			_ButtonShape = new Region( gp );
			_ButtonFont = new Font( "Arial", 9, FontStyle.Bold );

			_icoKameraFull = new Icon( this.GetType(), "grfx.kamera.ico" );
			_icoKameraHalv = new Icon( this.GetType(), "grfx.kamera2.ico" );
			sbpIkon.Icon = _icoKameraHalv;

			_bmpSpeaker = new Bitmap( this.GetType(), "grfx.speaker.png" );

			sbpFotograf.Text = "F: " + Global.Preferences.Fotografnummer;
			sbr.DrawItem += sbr_DrawItem;

            if (Global.Preferences.Brand == Brand.Kungsfoto)
            {
                mnuBurnDirect.Enabled = false;
                mnuDefragmentera.Visible = false;
            }
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMain));
            this.mnuMain = new System.Windows.Forms.MainMenu(this.components);
            this.mnuArkiv = new System.Windows.Forms.MenuItem();
            this.mnuÖppnaSkola = new System.Windows.Forms.MenuItem();
            this.mnuSave = new System.Windows.Forms.MenuItem();
            this.mnuArkivEgenskaper = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.mnuStandbylista = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.munPhotoArkivPrint = new System.Windows.Forms.MenuItem();
            this.mnuStudentCardPrint = new System.Windows.Forms.MenuItem();
            this.mnuStudentCardPrinterQue = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.mnuArkivAvsluta = new System.Windows.Forms.MenuItem();
            this.mnuEdit = new System.Windows.Forms.MenuItem();
            this.mnuEditSearch = new System.Windows.Forms.MenuItem();
            this.mnuSearchNext = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.mnuFileImportNames = new System.Windows.Forms.MenuItem();
            this.mnuImporteraBildfiler = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.mnuShowHistogram = new System.Windows.Forms.MenuItem();
            this.mnuFlik = new System.Windows.Forms.MenuItem();
            this.mnuFlikVal1 = new System.Windows.Forms.MenuItem();
            this.mnuFlikVal2 = new System.Windows.Forms.MenuItem();
            this.mnuFlikVal3 = new System.Windows.Forms.MenuItem();
            this.mnuFlikVal4 = new System.Windows.Forms.MenuItem();
            this.mnuFlikVal5 = new System.Windows.Forms.MenuItem();
            this.mnuFlikVal6 = new System.Windows.Forms.MenuItem();
            this.mnuFlikVal8 = new System.Windows.Forms.MenuItem();
            this.mnuFlikVal9 = new System.Windows.Forms.MenuItem();
            this.mnuFlikVal0 = new System.Windows.Forms.MenuItem();
            this.mnuVerktyg = new System.Windows.Forms.MenuItem();
            this.mnuInställningar = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mnuVerktygD50 = new System.Windows.Forms.MenuItem();
            this.mnuDefragmentera = new System.Windows.Forms.MenuItem();
            this.mnuUppdateraProgram = new System.Windows.Forms.MenuItem();
            this.mnuVerktygOmstart = new System.Windows.Forms.MenuItem();
            this.mnuCamera = new System.Windows.Forms.MenuItem();
            this.mnuImporteraBilder = new System.Windows.Forms.MenuItem();
            this.mnuToolsCameraSettings = new System.Windows.Forms.MenuItem();
            this.mnuDisconnectCamera = new System.Windows.Forms.MenuItem();
            this.mnuStorage = new System.Windows.Forms.MenuItem();
            this.mnuBurnDirect = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.mnuBurnOrder = new System.Windows.Forms.MenuItem();
            this.mnuCopyOrderFile = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.mnuBackup = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.mnuKommunikation = new System.Windows.Forms.MenuItem();
            this.mnuWiFiInloggning = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.mnuHelpManual = new System.Windows.Forms.MenuItem();
            this.mnuHelpEos1DsMarkII = new System.Windows.Forms.MenuItem();
            this.mnuHelpEos5D = new System.Windows.Forms.MenuItem();
            this.mnuHelpSport = new System.Windows.Forms.MenuItem();
            this.mnuHelpKeyboard = new System.Windows.Forms.MenuItem();
            this.mnuHelpVersions = new System.Windows.Forms.MenuItem();
            this.tmrOneShot = new System.Windows.Forms.Timer(this.components);
            this.sbr = new System.Windows.Forms.StatusBar();
            this.sbpMain = new System.Windows.Forms.StatusBarPanel();
            this.sbpFotograf = new System.Windows.Forms.StatusBarPanel();
            this.sbpSkola = new System.Windows.Forms.StatusBarPanel();
            this.sbpIkon = new System.Windows.Forms.StatusBarPanel();
            this.sbpKameraInfo = new System.Windows.Forms.StatusBarPanel();
            this.sbpACDC = new System.Windows.Forms.StatusBarPanel();
            this.sbpDisk = new System.Windows.Forms.StatusBarPanel();
            this.sbpSpeaker = new System.Windows.Forms.StatusBarPanel();
            this.sbpDate = new System.Windows.Forms.StatusBarPanel();
            this.sbpTime = new System.Windows.Forms.StatusBarPanel();
            this.imlSbr = new System.Windows.Forms.ImageList(this.components);
            this.tmrContinuous = new System.Windows.Forms.Timer(this.components);
            this.picTop = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.sbpMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpFotograf)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpSkola)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpIkon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpKameraInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpACDC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpDisk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpSpeaker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTop)).BeginInit();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuArkiv,
            this.mnuEdit,
            this.menuItem10,
            this.mnuFlik,
            this.mnuVerktyg,
            this.mnuCamera,
            this.mnuStorage,
            this.mnuKommunikation,
            this.menuItem2});
            // 
            // mnuArkiv
            // 
            this.mnuArkiv.Index = 0;
            this.mnuArkiv.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuÖppnaSkola,
            this.mnuSave,
            this.mnuArkivEgenskaper,
            this.menuItem5,
            this.mnuStandbylista,
            this.menuItem12,
            this.menuItem8,
            this.munPhotoArkivPrint,
            this.mnuStudentCardPrint,
            this.mnuStudentCardPrinterQue,
            this.menuItem7,
            this.mnuArkivAvsluta});
            this.mnuArkiv.Text = "&Arkiv";
            // 
            // mnuÖppnaSkola
            // 
            this.mnuÖppnaSkola.Index = 0;
            this.mnuÖppnaSkola.Text = "&Öppna order...";
            this.mnuÖppnaSkola.Click += new System.EventHandler(this.mnuÖppnaSkola_Click);
            // 
            // mnuSave
            // 
            this.mnuSave.Index = 1;
            this.mnuSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.mnuSave.Text = "&Spara";
            this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
            // 
            // mnuArkivEgenskaper
            // 
            this.mnuArkivEgenskaper.Index = 2;
            this.mnuArkivEgenskaper.Text = "&Egenskaper";
            this.mnuArkivEgenskaper.Click += new System.EventHandler(this.mnuArkivEgenskaper_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 3;
            this.menuItem5.Text = "-";
            // 
            // mnuStandbylista
            // 
            this.mnuStandbylista.Index = 4;
            this.mnuStandbylista.Text = "Standbylistor...";
            this.mnuStandbylista.Click += new System.EventHandler(this.mnuStandbylista_Click);
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 5;
            this.menuItem12.Text = "&Anteckningar...";
            this.menuItem12.Click += new System.EventHandler(this.menuItem12_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 6;
            this.menuItem8.Text = "-";
            // 
            // munPhotoArkivPrint
            // 
            this.munPhotoArkivPrint.Index = 7;
            this.munPhotoArkivPrint.Text = "PhotoArkiv-utskrift...";
            this.munPhotoArkivPrint.Click += new System.EventHandler(this.munPhotoArkivPrint_Click);
            // 
            // mnuStudentCardPrint
            // 
            this.mnuStudentCardPrint.Index = 8;
            this.mnuStudentCardPrint.Text = "StudentCard-utskrift...";
            this.mnuStudentCardPrint.Click += new System.EventHandler(this.mnuStudentCardPrint_Click);
            // 
            // mnuStudentCardPrinterQue
            // 
            this.mnuStudentCardPrinterQue.Index = 9;
            this.mnuStudentCardPrinterQue.Text = "StudentCard skrivarkö";
            this.mnuStudentCardPrinterQue.Click += new System.EventHandler(this.mnuStudentCardPrinterQue_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 10;
            this.menuItem7.Text = "-";
            // 
            // mnuArkivAvsluta
            // 
            this.mnuArkivAvsluta.Index = 11;
            this.mnuArkivAvsluta.Text = "Avsluta";
            this.mnuArkivAvsluta.Click += new System.EventHandler(this.mnuArkivAvsluta_Click);
            // 
            // mnuEdit
            // 
            this.mnuEdit.Index = 1;
            this.mnuEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuEditSearch,
            this.mnuSearchNext,
            this.menuItem9,
            this.mnuFileImportNames,
            this.mnuImporteraBildfiler});
            this.mnuEdit.Text = "&Redigera";
            this.mnuEdit.Popup += new System.EventHandler(this.mnuEdit_Popup);
            // 
            // mnuEditSearch
            // 
            this.mnuEditSearch.Index = 0;
            this.mnuEditSearch.Shortcut = System.Windows.Forms.Shortcut.CtrlF;
            this.mnuEditSearch.Text = "&Sök...";
            this.mnuEditSearch.Click += new System.EventHandler(this.mnuEditSearch_Click);
            // 
            // mnuSearchNext
            // 
            this.mnuSearchNext.Index = 1;
            this.mnuSearchNext.Shortcut = System.Windows.Forms.Shortcut.F3;
            this.mnuSearchNext.Text = "Sök &nästa";
            this.mnuSearchNext.Click += new System.EventHandler(this.mnuSearchNext_Click);
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 2;
            this.menuItem9.Text = "-";
            // 
            // mnuFileImportNames
            // 
            this.mnuFileImportNames.Index = 3;
            this.mnuFileImportNames.Text = "Importera &namnlista...";
            this.mnuFileImportNames.Click += new System.EventHandler(this.mnuFileImportNames_Click);
            // 
            // mnuImporteraBildfiler
            // 
            this.mnuImporteraBildfiler.Index = 4;
            this.mnuImporteraBildfiler.Text = "&Importera bildfiler...";
            this.mnuImporteraBildfiler.Click += new System.EventHandler(this.mnuImporteraBildfiler_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 2;
            this.menuItem10.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuShowHistogram});
            this.menuItem10.Text = "Visa";
            this.menuItem10.Visible = false;
            // 
            // mnuShowHistogram
            // 
            this.mnuShowHistogram.Index = 0;
            this.mnuShowHistogram.Shortcut = System.Windows.Forms.Shortcut.CtrlH;
            this.mnuShowHistogram.Text = "Histogram";
            this.mnuShowHistogram.Click += new System.EventHandler(this.mnuShowHistogram_Click);
            // 
            // mnuFlik
            // 
            this.mnuFlik.Index = 3;
            this.mnuFlik.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFlikVal1,
            this.mnuFlikVal2,
            this.mnuFlikVal3,
            this.mnuFlikVal4,
            this.mnuFlikVal5,
            this.mnuFlikVal6,
            this.mnuFlikVal8,
            this.mnuFlikVal9,
            this.mnuFlikVal0});
            this.mnuFlik.Text = "&Flik";
            // 
            // mnuFlikVal1
            // 
            this.mnuFlikVal1.Index = 0;
            this.mnuFlikVal1.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.mnuFlikVal1.Text = "&Order";
            this.mnuFlikVal1.Click += new System.EventHandler(this.mnuFlikVal_Click);
            // 
            // mnuFlikVal2
            // 
            this.mnuFlikVal2.Index = 1;
            this.mnuFlikVal2.Shortcut = System.Windows.Forms.Shortcut.CtrlG;
            this.mnuFlikVal2.Text = "&Gruppbild inne";
            this.mnuFlikVal2.Click += new System.EventHandler(this.mnuFlikVal_Click);
            // 
            // mnuFlikVal3
            // 
            this.mnuFlikVal3.Index = 2;
            this.mnuFlikVal3.Shortcut = System.Windows.Forms.Shortcut.CtrlU;
            this.mnuFlikVal3.Text = "Gruppbild &ute";
            this.mnuFlikVal3.Click += new System.EventHandler(this.mnuFlikVal_Click);
            // 
            // mnuFlikVal4
            // 
            this.mnuFlikVal4.Index = 3;
            this.mnuFlikVal4.Shortcut = System.Windows.Forms.Shortcut.CtrlP;
            this.mnuFlikVal4.Text = "&Porträtt";
            this.mnuFlikVal4.Click += new System.EventHandler(this.mnuFlikVal_Click);
            // 
            // mnuFlikVal5
            // 
            this.mnuFlikVal5.Index = 4;
            this.mnuFlikVal5.Shortcut = System.Windows.Forms.Shortcut.CtrlI;
            this.mnuFlikVal5.Text = "&Infällning";
            this.mnuFlikVal5.Click += new System.EventHandler(this.mnuFlikVal_Click);
            // 
            // mnuFlikVal6
            // 
            this.mnuFlikVal6.Index = 5;
            this.mnuFlikVal6.Shortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.mnuFlikVal6.Text = "P&ersonal";
            this.mnuFlikVal6.Click += new System.EventHandler(this.mnuFlikVal_Click);
            // 
            // mnuFlikVal8
            // 
            this.mnuFlikVal8.Index = 6;
            this.mnuFlikVal8.Shortcut = System.Windows.Forms.Shortcut.CtrlM;
            this.mnuFlikVal8.Text = "Vi&mmel";
            this.mnuFlikVal8.Click += new System.EventHandler(this.mnuFlikVal_Click);
            // 
            // mnuFlikVal9
            // 
            this.mnuFlikVal9.Index = 7;
            this.mnuFlikVal9.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
            this.mnuFlikVal9.Text = "Fä&rdigställ";
            this.mnuFlikVal9.Click += new System.EventHandler(this.mnuFlikVal_Click);
            // 
            // mnuFlikVal0
            // 
            this.mnuFlikVal0.Index = 8;
            this.mnuFlikVal0.Shortcut = System.Windows.Forms.Shortcut.CtrlT;
            this.mnuFlikVal0.Text = "&Töm skärm";
            this.mnuFlikVal0.Click += new System.EventHandler(this.mnuFlikVal_Click);
            // 
            // mnuVerktyg
            // 
            this.mnuVerktyg.Index = 4;
            this.mnuVerktyg.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuInställningar,
            this.menuItem1,
            this.mnuVerktygD50,
            this.mnuDefragmentera,
            this.mnuUppdateraProgram,
            this.mnuVerktygOmstart});
            this.mnuVerktyg.Text = "&Verktyg";
            // 
            // mnuInställningar
            // 
            this.mnuInställningar.Index = 0;
            this.mnuInställningar.Text = "&Programinställningar...";
            this.mnuInställningar.Click += new System.EventHandler(this.mnuInställningar_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.Text = "-";
            // 
            // mnuVerktygD50
            // 
            this.mnuVerktygD50.Index = 2;
            this.mnuVerktygD50.Text = "D-50 3G";
            this.mnuVerktygD50.Click += new System.EventHandler(this.mnuVerktygD50_Click);
            // 
            // mnuDefragmentera
            // 
            this.mnuDefragmentera.Index = 3;
            this.mnuDefragmentera.Text = "&Defragmentera hårddisken";
            this.mnuDefragmentera.Click += new System.EventHandler(this.mnuDefragmentera_Click);
            // 
            // mnuUppdateraProgram
            // 
            this.mnuUppdateraProgram.Index = 4;
            this.mnuUppdateraProgram.Text = "&Uppdatera programvara";
            this.mnuUppdateraProgram.Click += new System.EventHandler(this.mnuUppdateraProgra_Click);
            // 
            // mnuVerktygOmstart
            // 
            this.mnuVerktygOmstart.Index = 5;
            this.mnuVerktygOmstart.Text = "&Omstart av Plåta";
            this.mnuVerktygOmstart.Click += new System.EventHandler(this.mnuVerktygOmstart_Click);
            // 
            // mnuCamera
            // 
            this.mnuCamera.Index = 5;
            this.mnuCamera.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuImporteraBilder,
            this.mnuToolsCameraSettings,
            this.mnuDisconnectCamera});
            this.mnuCamera.Text = "&Kamera";
            this.mnuCamera.Popup += new System.EventHandler(this.mnuCamera_Popup);
            // 
            // mnuImporteraBilder
            // 
            this.mnuImporteraBilder.Index = 0;
            this.mnuImporteraBilder.Text = "&Importera bilder...";
            this.mnuImporteraBilder.Click += new System.EventHandler(this.mnuImporteraBilder_Click);
            // 
            // mnuToolsCameraSettings
            // 
            this.mnuToolsCameraSettings.Index = 1;
            this.mnuToolsCameraSettings.Text = "&Kamerainställningar...";
            this.mnuToolsCameraSettings.Click += new System.EventHandler(this.mnuToolsCameraSettings_Click);
            // 
            // mnuDisconnectCamera
            // 
            this.mnuDisconnectCamera.Index = 2;
            this.mnuDisconnectCamera.Shortcut = System.Windows.Forms.Shortcut.CtrlK;
            this.mnuDisconnectCamera.Text = "K&oppla in/ur kamera";
            this.mnuDisconnectCamera.Click += new System.EventHandler(this.mnuDisconnectCamera_Click);
            // 
            // mnuStorage
            // 
            this.mnuStorage.Index = 6;
            this.mnuStorage.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuBurnDirect,
            this.menuItem6,
            this.mnuBurnOrder,
            this.mnuCopyOrderFile,
            this.menuItem11,
            this.mnuBackup,
            this.menuItem4});
            this.mnuStorage.Text = "&Lagring";
            // 
            // mnuBurnDirect
            // 
            this.mnuBurnDirect.Index = 0;
            this.mnuBurnDirect.Text = "Bränn &Direkt till kund...";
            this.mnuBurnDirect.Click += new System.EventHandler(this.mnuBurnDirect_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 1;
            this.menuItem6.Text = "-";
            // 
            // mnuBurnOrder
            // 
            this.mnuBurnOrder.Index = 2;
            this.mnuBurnOrder.Text = "Bränn &Orderskiva...";
            this.mnuBurnOrder.Click += new System.EventHandler(this.mnuBurnOrder_Click);
            // 
            // mnuCopyOrderFile
            // 
            this.mnuCopyOrderFile.Index = 3;
            this.mnuCopyOrderFile.Text = "&Kopiera orderfil...";
            this.mnuCopyOrderFile.Click += new System.EventHandler(this.mnuCopyOrderFile_Click);
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 4;
            this.menuItem11.Text = "-";
            // 
            // mnuBackup
            // 
            this.mnuBackup.Index = 5;
            this.mnuBackup.Text = "Ta &Backup till extern hårddisk...";
            this.mnuBackup.Click += new System.EventHandler(this.mnuBackup_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 6;
            this.menuItem4.Text = "&Visa gjorda backuper (till disk)";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // mnuKommunikation
            // 
            this.mnuKommunikation.Index = 7;
            this.mnuKommunikation.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuWiFiInloggning});
            this.mnuKommunikation.Text = "Kommunikation";
            // 
            // mnuWiFiInloggning
            // 
            this.mnuWiFiInloggning.Index = 0;
            this.mnuWiFiInloggning.Text = "WiFi / Inloggning";
            this.mnuWiFiInloggning.Click += new System.EventHandler(this.mnuWiFiInloggning_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 8;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuHelpManual,
            this.mnuHelpEos1DsMarkII,
            this.mnuHelpEos5D,
            this.mnuHelpSport,
            this.mnuHelpKeyboard,
            this.mnuHelpVersions});
            this.menuItem2.Text = "&Hjälp";
            // 
            // mnuHelpManual
            // 
            this.mnuHelpManual.Index = 0;
            this.mnuHelpManual.Shortcut = System.Windows.Forms.Shortcut.F1;
            this.mnuHelpManual.Text = "Fotografhandbok";
            this.mnuHelpManual.Click += new System.EventHandler(this.mnuHelpManual_Click);
            // 
            // mnuHelpEos1DsMarkII
            // 
            this.mnuHelpEos1DsMarkII.Index = 1;
            this.mnuHelpEos1DsMarkII.Text = "Kamera Eos-1 Ds Mark II ";
            this.mnuHelpEos1DsMarkII.Click += new System.EventHandler(this.mnuHelpEos1DsMarkII_Click);
            // 
            // mnuHelpEos5D
            // 
            this.mnuHelpEos5D.Index = 2;
            this.mnuHelpEos5D.Text = "Kamera Eos 5D";
            this.mnuHelpEos5D.Click += new System.EventHandler(this.mnuHelpEos5D_Click);
            // 
            // mnuHelpSport
            // 
            this.mnuHelpSport.Index = 3;
            this.mnuHelpSport.Text = "Sporthandbok";
            this.mnuHelpSport.Click += new System.EventHandler(this.mnuHelpSport_Click);
            // 
            // mnuHelpKeyboard
            // 
            this.mnuHelpKeyboard.Index = 4;
            this.mnuHelpKeyboard.Text = "Tangentbord...";
            this.mnuHelpKeyboard.Click += new System.EventHandler(this.mnuHelpKeyboard_Click);
            // 
            // mnuHelpVersions
            // 
            this.mnuHelpVersions.Index = 5;
            this.mnuHelpVersions.Text = "Versionshistorik...";
            this.mnuHelpVersions.Click += new System.EventHandler(this.mnuHelpVersions_Click);
            // 
            // tmrOneShot
            // 
            this.tmrOneShot.Interval = 50;
            this.tmrOneShot.Tick += new System.EventHandler(this.tmrOneShot_Tick);
            // 
            // sbr
            // 
            this.sbr.Location = new System.Drawing.Point(0, -26);
            this.sbr.Name = "sbr";
            this.sbr.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.sbpMain,
            this.sbpFotograf,
            this.sbpSkola,
            this.sbpIkon,
            this.sbpKameraInfo,
            this.sbpACDC,
            this.sbpDisk,
            this.sbpSpeaker,
            this.sbpDate,
            this.sbpTime});
            this.sbr.ShowPanels = true;
            this.sbr.Size = new System.Drawing.Size(146, 26);
            this.sbr.TabIndex = 2;
            this.sbr.PanelClick += new System.Windows.Forms.StatusBarPanelClickEventHandler(this.sbr_PanelClick);
            // 
            // sbpMain
            // 
            this.sbpMain.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.sbpMain.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
            this.sbpMain.Name = "sbpMain";
            this.sbpMain.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
            this.sbpMain.Width = 10;
            // 
            // sbpFotograf
            // 
            this.sbpFotograf.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.sbpFotograf.Name = "sbpFotograf";
            this.sbpFotograf.Width = 45;
            // 
            // sbpSkola
            // 
            this.sbpSkola.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.sbpSkola.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.sbpSkola.Name = "sbpSkola";
            this.sbpSkola.Text = "Ingen skola vald";
            this.sbpSkola.Width = 96;
            // 
            // sbpIkon
            // 
            this.sbpIkon.Name = "sbpIkon";
            this.sbpIkon.Width = 28;
            // 
            // sbpKameraInfo
            // 
            this.sbpKameraInfo.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.sbpKameraInfo.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.sbpKameraInfo.Name = "sbpKameraInfo";
            this.sbpKameraInfo.Text = "Kameran har inte kopplats";
            this.sbpKameraInfo.Width = 146;
            // 
            // sbpACDC
            // 
            this.sbpACDC.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.sbpACDC.Name = "sbpACDC";
            this.sbpACDC.Width = 32;
            // 
            // sbpDisk
            // 
            this.sbpDisk.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.sbpDisk.Name = "sbpDisk";
            this.sbpDisk.Width = 48;
            // 
            // sbpSpeaker
            // 
            this.sbpSpeaker.Name = "sbpSpeaker";
            this.sbpSpeaker.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
            this.sbpSpeaker.Width = 28;
            // 
            // sbpDate
            // 
            this.sbpDate.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.sbpDate.Name = "sbpDate";
            this.sbpDate.Text = "2004-04-26";
            this.sbpDate.Width = 65;
            // 
            // sbpTime
            // 
            this.sbpTime.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.sbpTime.Name = "sbpTime";
            this.sbpTime.Text = "14:00";
            this.sbpTime.Width = 40;
            // 
            // imlSbr
            // 
            this.imlSbr.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlSbr.ImageStream")));
            this.imlSbr.TransparentColor = System.Drawing.Color.Transparent;
            this.imlSbr.Images.SetKeyName(0, "");
            // 
            // tmrContinuous
            // 
            this.tmrContinuous.Enabled = true;
            this.tmrContinuous.Interval = 10000;
            this.tmrContinuous.Tick += new System.EventHandler(this.tmrContinuous_Tick);
            // 
            // picTop
            // 
            this.picTop.BackgroundImage = global::Plata.Properties.Resources.toprail;
            this.picTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.picTop.Location = new System.Drawing.Point(0, 0);
            this.picTop.Name = "picTop";
            this.picTop.Size = new System.Drawing.Size(146, 64);
            this.picTop.TabIndex = 0;
            this.picTop.TabStop = false;
            // 
            // FMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(146, 0);
            this.Controls.Add(this.sbr);
            this.Controls.Add(this.picTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(-200, 0);
            this.Menu = this.mnuMain;
            this.Name = "FMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            ((System.ComponentModel.ISupportInitialize)(this.sbpMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpFotograf)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpSkola)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpIkon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpKameraInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpACDC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpDisk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpSpeaker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTop)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
		    if (vdCamera.vdCamera.CanFindEdSdkInSearchPath())
		        MessageBox.Show(
		            "Felaktig Plåta-installation! Filen EDSDK.dll får INTE finnas i sökvägen! Du kan INTE använda kameran förrän detta är åtgärdat!");

		    if (!checkForBarCode())
		        return;

		    moveBackdrops();

		    try
		    {
		        var badFile = Global.getAppPath("Fotohandbok Photomic.pdf");
		        if (File.Exists(badFile))
		            File.Move(badFile, Global.getAppPath("Fotohandbok_Photomic.pdf"));
		    }
		    catch
		    {
		    }

		    Application.ThreadException += Application_ThreadException;
		    try
		    {
		        Application.Run(new FMain());
		    }
		    catch (Exception ex)
		    {
		        MessageBox.Show(ex.ToString(), "Ursäkta!");
		    }

		    if (Global.Fotografdator && !s_fDontReboot)
		        ShutdownWindows.DoExitWin(ShutdownWindows.EWX_POWEROFF | ShutdownWindows.EWX_FORCE);
		}

	    private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs ex ) 
		{
			using ( var dlg = new frmErrorReport(ex.Exception.ToString()) )
			{
				var result = dlg.ShowDialog();
				Global.storeErrorReport( ex.Exception, dlg.Feedback );
				if ( result == DialogResult.Abort )
				{
					if ( Global.Fotografdator )
						ShutdownWindows.DoExitWin( ShutdownWindows.EWX_POWEROFF | ShutdownWindows.EWX_FORCE );
					Application.Exit();
				}
			}
		}

		private static bool checkForBarCode()
		{
			try
			{
				if ((new System.Drawing.Text.InstalledFontCollection()).Families.Any(ff => ff.Name == "IDAutomationHC39M"))
				    return true;
				var installer = Global.getAppPath("installIDAutomationHC39M.EXE" );
				if ( File.Exists( installer ) )
					if ( Global.askMsgBox( null, "Streckkodstypsnittet har inte installerats korrekt. Vill du försöka igen?", true ) == DialogResult.Yes )
					{
						Global.runSA( installer, null ).WaitForExit();
						Application.Restart();
						return false;
					}
			}
			catch
			{
			}
			return true;
		}

        private static void moveBackdrops()
        {
            try
            {
                var src = Global.getAppPath("backdrop");
                var dst = Path.Combine(Global.Preferences.MainPath, "_backdrops");
                if (!Directory.Exists(src) || !Directory.Exists(dst))
                    return;
                foreach (var fn in Directory.GetFiles(src))
                {
                    var fnTo = Path.Combine(dst, Path.GetFileName(fn));
                    Util.safeKillFile(fnTo);
                    File.Move(fn, fnTo);
                }

            }
            catch (Exception ex)
            {
                MsgBox.showMsgBox(null, ex.ToString());
            }
        }

	    protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyData == (Keys.F11 | Keys.Control | Keys.Alt))
                Global.runSA("cmd.exe","");
        }

		protected override void OnLoad( EventArgs e )
		{
			base.OnLoad( e );

			if ( !string.IsNullOrEmpty(Global.Preferences.AutoUpdateFolder) )
				if ( Upgrader.runUpgrade( Global.Preferences.AutoUpdateFolder ) )
				{
					endApp( false );
					return;
				}

			try
			{
				foreach ( var s in Directory.GetFiles( Global.getMemoryCardCacheFolder() ) )
					Util.safeKillFile( s );
			}
			catch
			{
			}

			picTop.Height = picTop.BackgroundImage.Size.Height;

			_frmDummy = new frmTitle( this );
			_flikar[0] = new frmOrder( this );
			_flikar[1] = new frmGruppbild( this, FlikTyp.GruppbildInne );
			_flikar[2] = new frmGruppbild( this, FlikTyp.GruppbildUte );
			_flikar[3] = new frmPortratt( this, FlikTyp.PorträttInne );
			_flikar[4] = new frmPortratt( this, FlikTyp.PorträttUte );
			_flikar[5] = new frmPortratt( this, FlikTyp.Infällning );
			_flikar[6] = new frmPortratt( this, FlikTyp.Personal );
			_flikar[7] = new frmVimmel( this );
			_flikar[8] = new frmFärdigställ( this );
			_flikar[9] = new FFTP( this );
			_frmDummy.Show();
			for ( int i=_flikar.Length-1 ; i>=0 ; i-- )
				_flikar[i].Show();
			_frmDummy.Activate();
			displayTimeOnStatusbar();
			tmrOneShot.Enabled = true;

			if ( Global.Fotografdator )
			{
				var hWnd = Util.FindWindow("Shell_traywnd",null);
				if ( hWnd!=IntPtr.Zero )
					Util.ShowWindow( hWnd, 0 );
				this.MaximizeBox = false;
				this.MinimizeBox = false;
				this.WindowState = FormWindowState.Maximized;
			}
			else
			{
				var r = SystemInformation.WorkingArea;
				r.Inflate( -r.Width/200, -r.Height/200 );
				this.Location = r.Location;
				this.Size = r.Size;
			}
		}

		private void repaintTop( Graphics g )
		{
			try
			{
				if ( _flikar[0] == null )
					return;
				g.SmoothingMode = SmoothingMode.AntiAlias;
				int nX = 10;
				for ( int i = 0 ; i < _flikar.Length ; i++ )
				{
					if ( _flikar[i].RightAligned )
						nX = this.ClientSize.Width - (_flikar.Length - i) * 105 - 10;
					_flikar[i].paint_Flik(
						g,
						_ButtonShape,
						_ButtonColor,
						_ButtonFont,
						nX );
					nX += 105;
				}
			}
			catch ( Exception ex )
			{
				Global.showMsgBox( this, "{0}\r\n\r\n{1} {2}",
					ex, _ButtonShape != null, _ButtonFont != null );

			}
		}

		private void mouseMoveTop( int x, int y, int nClicks )
		{
			int nActivate = -1;

			for ( var i=0 ; i<_flikar.Length ; i++ )
				if ( Global.Skola!=null || i==(_flikar.Length-1) )
					if ( _flikar[i].updateHit( x, y, nClicks ) )
						{
							picTop.Invalidate();
							if ( _flikar[i].fmMode == FlikMode.Active )
								nActivate = i;
						}

			if ( nActivate!=-1 )
			{
				PlataDM.Grupp grupp = null;
				PlataDM.Person pers = null;
				var gf = _aktivFlik as baseGruppForm;
				if ( gf!=null )
					gf.getSelectedGroupPerson( out grupp, out pers );
				jumpToForm_Group_Person( (FlikTyp)nActivate, grupp, pers );
			}
		}

		private void picTop_Paint(object sender, PaintEventArgs e)
		{
			repaintTop( e.Graphics );
		}

		private void picTop_MouseMove(object sender, MouseEventArgs e)
		{
			mouseMoveTop( e.X, e.Y, 0 );
		}

		private void picTop_MouseLeave(object sender, EventArgs e)
		{
			mouseMoveTop( -1, -1, 0 );
		}

		private void picTop_MouseDown(object sender, MouseEventArgs e)
		{
			if ( e.Clicks==2 && (ModifierKeys==Keys.Control) && Global.Skola!=null )
				foreach (var flik in _flikar)
				    if ( flik.checkHitArea(e.X,e.Y) && flik.fmMode==FlikMode.Disabled )
				    {
				        switch ( flik.FlikTyp )
				        {
				            case FlikTyp.GruppbildInne:
				            case FlikTyp.GruppbildUte:
				                flik.fmMode = FlikMode.Normal;
				                Global.Skola.NoGroup = false;
				                break;
				            case FlikTyp.Personal:
				                flik.fmMode = FlikMode.Normal;
				                Global.Skola.NoStaff = false;
				                break;
				            case FlikTyp.PorträttInne:
				            case FlikTyp.PorträttUte:
				                flik.fmMode = FlikMode.Normal;
				                break;
				        }
				    }
		    mouseMoveTop( e.X, e.Y, e.Clicks );
		}

		private DialogResult visaInställningar()
		{
			using( var dlg = new frmInställningar() )
			{
				var retVal = dlg.ShowDialog(this);
				sbpFotograf.Text = string.Format( "F: {0}", Global.Preferences.Fotografnummer );
				picTop.Refresh();
				if ( retVal == DialogResult.OK && Global.Skola != null )
					rapportera_skolaUppdaterad();
				return retVal;
			}
		}

		public void rapportera_skolaUppdaterad()
		{
			foreach ( var frm in _flikar )
				try 
				{
					frm.skolaUppdaterad();
				} 
				catch ( Exception ex )
				{
					Global.showMsgBox( this, frm.Name + ":\r\n" +  ex );
				}
		}

		public baseFlikForm getFlik( FlikTyp typ )
		{
		    return _flikar.FirstOrDefault(frm => frm.FlikTyp == typ);
		}

	    private void enableFlik( FlikTyp typ, bool fEnable )
		{
			getFlik(typ).FlikMode = fEnable ? FlikMode.Normal : FlikMode.Disabled;
		}

		private void visaÖppnaSkola( string strPath )
		{
			visaÖppnaSkola1( strPath );
			if ( Global.Skola != null )
				visaÖppnaSkola2();
			_aktivFlik = _flikar[0];
			_aktivFlik.fmMode = FlikMode.Active;
			_aktivFlik.Activate();
			picTop.Invalidate();
			sbpFotograf.Text = string.Format( "F: {0}", Global.Preferences.Fotografnummer );
		}

		private void visaÖppnaSkola1( string strPath )
		{
			Global.saveSchool( true );

			if ( strPath==null )
				OpenDialog.FOpenDialog.showDialog( this, false, ref Global.Skola );
			else
			{
				if ( Global.Skola!=null )
					Global.Skola.Dispose();
                Global.Skola = new PlataDM.Skola(AppSpecifics.Version, Global.Preferences.Fotografnummer);
				Global.Skola.Open( strPath );
			}
		}

		private void visaÖppnaSkola2()
		{
			enableFlik( FlikTyp.PorträttInne, true );
			enableFlik( FlikTyp.PorträttUte, true );
			enableFlik( FlikTyp.GruppbildInne, !Global.Skola.NoGroup );
			enableFlik( FlikTyp.GruppbildUte, !Global.Skola.NoGroup );
			enableFlik( FlikTyp.Personal, !Global.Skola.NoStaff && Global.Skola.CompanyOrder==PlataDM.CompanyOrder.No );

			sbpSkola.Text = string.Format( "{0}{1} - {2}",
				Global.Skola.ReadOnly ? "SKRIVSKYDD: " : string.Empty, Global.Skola.OrderNr, Global.Skola.Namn );
			try
			{
				var strPath = Global.Skola.HomePath;
				strPath = Path.Combine( strPath, "StudentCard" );
				foreach ( var mall in Template.Mallar( strPath ) )
					Global.Skola.StudentCardTemplates.Add( mall );
			}
			catch
			{
			}
			rapportera_skolaUppdaterad();
		    var t = new Thread(preloadThumbnails) {Priority = ThreadPriority.Lowest};
		    t.Start();

			if ( !Global.Skola.ReadOnly )
			{
				var oc = Global.Skola.ScanForOrphanPictures();
				if ( oc != null )
				{
					Global.showMsgBox(
						this,
						"Hittade borttappade bilder!!!\r\n\r\n" +
						"Titta i gruppen \"_slask\" och försök genast flytta dem dit de hör hemma!\r\n\r\n" +
						"  Gruppbilder: {0} st\r\n" +
						"  Porträtt: {1} st\r\n" +
						"  Vimmlar: {2} st",
						oc.GroupPictures, oc.Portraits, oc.EnvironmentPictures );
					rapportera_skolaUppdaterad();
				}
			}
		}

		private void tmrOneShot_Tick(object sender, System.EventArgs e)
		{
			tmrOneShot.Enabled = false;
			if ( _frmDummy == null )
				return;

			while ( true )
			{
				var fBadSettings = string.IsNullOrEmpty( Global.Preferences.MainPath ) || !Directory.Exists( Global.Preferences.MainPath );
				if ( Global.Fotografdator )
					fBadSettings |= Global.Preferences.Fotografnummer == 0;
				if ( SystemInformation.TerminalServerSession )
					fBadSettings |= string.IsNullOrEmpty( Global.Preferences.AutoUpdateFolder ) || !Directory.Exists( Global.Preferences.AutoUpdateFolder );
				if ( !fBadSettings )
					break;
				if ( visaInställningar() != DialogResult.OK )
				{
					Close();
					return;
				}
			}

			Global.DataPath = Path.Combine( Global.Preferences.MainPath, "_data" );
			if ( !Directory.Exists( Global.DataPath ) )
				Directory.CreateDirectory( Global.DataPath );
			Fotografer.initFromDatafile();

			try 
			{
				initCamera();
			}
			catch ( Exception ex )
			{
				sbpKameraInfo.Text = ex.Message;
			}

			string strOpenSchool = null;
			string strResultFile = null;
			var arg = Environment.GetCommandLineArgs();
			for ( var i=1 ; i<arg.Length ; i++ )
			{
				var astr = arg[i].Split( new char[] {'='} ,2 );
				if ( astr.Length==2 )
					switch ( astr[0].ToLower() )
					{
						case "plataxhg":
						{
							var xmlDoc = new XmlDocument();
							xmlDoc.Load( astr[1] );
							var nodInfo = xmlDoc.GetElementsByTagName("INFO")[0];
							foreach ( XmlAttribute xa in nodInfo.Attributes )
								switch ( xa.Name )
								{
									case "school":
										strOpenSchool = xa.Value;
										break;
									case "resultfile":
										strResultFile = xa.Value;
										break;
								}
						}
							break;
						case "dellist":
							processDelList( astr[1] );
							break;
						case "result":
							strResultFile = astr[1];
							break;
					}
			}

			visaÖppnaSkola( strOpenSchool );

			if ( strResultFile!=null )
				processResultFile( strResultFile );

			displayDiskFreeSpaceOnStatusbar();
		}

		private void processDelList( string strDelList )
		{
			try
			{
				using ( var sr = new StreamReader( strDelList, System.Text.Encoding.Default ) )
					while ( true )
					{
						var s = sr.ReadLine();
						if ( s==null )
							break;
						Util.safeKillFile( s );
					}
				Util.safeKillFile( strDelList );
			}
			catch
			{
			}
		}

		private void processResultFile( string strResultFile )
		{
			try
			{
				using ( var sr = new StreamReader( strResultFile, System.Text.Encoding.Default ) )
				{
					var s = sr.ReadToEnd();
					if ( s.CompareTo("OK")!=0 )
					{
						Global.storeErrorReport( s, "Från brännarprogrammet" );
						Global.showMsgBox( this, s );
					}
					else
						Global.showMsgBox( this, "Bränningen är klar!" );
				}
			}
			catch
			{
			}
		}

		private void mnuArkivAvsluta_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if ( _camera != null )
			{
                if (_camera.IsConnected)
                {
                    _camera.Disconnect();
                    Thread.Sleep(1000);
                }
			}
			base.OnClosing(e);
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed (e);
			Global.sparaInställningar();
			Global.saveSchool( true );
			_fEnded = true;
		}

		protected override void OnResize(EventArgs e)
		{
			if (Global.Fotografdator && this.WindowState != FormWindowState.Maximized)
				if ( this.MdiChildren.GetLength(0) > 1 )
					this.WindowState = FormWindowState.Maximized;
			base.OnResize(e);
		}

		#region Kamera

		private void initCamera()
		{
			if ( _camera==null )
			{
				switch (Global.Preferences.Camera)
				{
                    case CameraSdk.Ed207Camera:
                        if (!vdCamera.vdCamera.CanEd207())
                            goto default;
                        _camera = vdCamera.vdCamera.GetEd207Camera(this);
                        break;
                    case CameraSdk.Ed210Camera:
                        if (!vdCamera.vdCamera.CanEd210())
							goto default;
                        _camera = vdCamera.vdCamera.GetEd210Camera(this);
						break;
                    case CameraSdk.FujiCamera:
                        if (!vdCamera.vdCamera.CanFujiS5())
                            goto default;
                        _camera = vdCamera.vdCamera.GetFujiCamera(this);
                        break;
                    default:
                        _camera = vdCamera.vdCamera.GetNullCamera(this);
						break;
				}

				_camera.OnPictureReady += _camera_OnPictureReady;
				_camera.OnCameraMessage += _camera_OnCameraMessage;
				_camera.OnIncomingPicture += _camera_OnIncomingPicture;
				_camera.OnConnectResult += _camera_OnConnectResult;
			}
			sbpKameraInfo.Text = "Kopplar kameran...";
			_camera.Connect();
		}

		private void _camera_OnPictureReady(
			byte[] jpgData,
			byte[] rawData )
		{
			System.Diagnostics.Debug.WriteLine( string.Format( "{0}:{1} IN", Global.Now.Second, Global.Now.Millisecond ) );
			try
			{
				if ( Global.Skola != null )
				{
					if ( !Global.Skola.ReadOnly )
                        _aktivFlik.nyttFoto(false, jpgData, rawData);
					else
						Global.showMsgBox( this, "Du kan inte ta bilder när skolan har öppnats för endast läsning!" );
				}
			}
			catch ( Exception ex )
			{
				MessageBox.Show( this, ex.Message );
			}
			displayCameraInfo( null );
			System.Diagnostics.Debug.WriteLine( string.Format( "{0}:{1} OUT", Global.Now.Second, Global.Now.Millisecond ) );
		}

		private void _camera_OnCameraMessage( string strMessage )
		{
			if ( !_camera.IsConnected )
				_camera.Disconnect();
			displayCameraInfo( strMessage );
		}

		private void _camera_OnIncomingPicture( int nCount )
		{
			System.Diagnostics.Debug.WriteLine( string.Format( "{0}:{1} {2}", Global.Now.Second, Global.Now.Millisecond, nCount ) );
			sbpKameraInfo.Text = "Hämtar bild...";
		}

		private void _camera_OnConnectResult( int resultCode, string description )
		{
			if ( _camera.IsConnected )
			{
				displayCameraInfo( null );
				if ( _aktivFlik != null )
					_aktivFlik.activated();
			}
			else
			{
                switch ( resultCode )
                {
                    case 128:
                        description += " (hittar ingen kamera)";
                        break;
                    case 4242:
                        description += " (fel API-version)";
                        break;
                    case 4243:
                        description += " (fel kameratyp för valt API)";
                        break;
                    case 0:
                        break;
                    default:
                        description += " (felkod: " + resultCode + ")";
                        break;
                }
				displayCameraInfo( description );
			}
		}

		private void displayCameraInfo( string strText )
		{
            if (InvokeRequired)
            {
            }
			if ( _camera.IsConnected )
			{
				sbpIkon.Icon = _icoKameraFull;
				if (strText == null)
					strText = string.Format( "Kameran är kopplad ({0})", _camera.ApiName );
			}
			else
			{
				sbpIkon.Icon = _icoKameraHalv;
					strText = "Kameran är inte kopplad" + (strText ?? "");
			}
			sbpKameraInfo.Text = strText;
            System.Diagnostics.Debug.Print(sbpKameraInfo.Text);
		}

        public vdCamera.vdCamera Camera
		{
			get { return _camera; }
		}

		#endregion

		private void mnuÖppnaSkola_Click(object sender, System.EventArgs e)
		{
			//bläddra till första sidan så att aktuell flik för OnDeactivate körd
			this.Cursor = Cursors.WaitCursor;
			_aktivFlik = _flikar[0];
			_aktivFlik.Activate();
			picTop.Invalidate();
			this.Cursor = Cursors.Default;

			visaÖppnaSkola( null );
		}

		private void mnuInställningar_Click(object sender, System.EventArgs e)
		{
			visaInställningar();
		}

		public void setStatusProgress( int nValue, int nMax )
		{
			try
			{
				if ( nValue<1 || nMax<1 || nValue>nMax )
					_nStatusProgress = 0;
				else
					_nStatusProgress = 100*nValue/nMax;
				sbr.Invalidate();
				sbr.Update();
			}
			catch
			{
			}
		}

		public void setStatusText( baseFlikForm flik, string strText )
		{
			if ( flik==_aktivFlik )
				sbpMain.Text = strText;
		}

		private void displayTimeOnStatusbar()
		{
			sbpDate.Text = Global.Now.ToString( "yyyy-MM-dd" );
			sbpTime.Text = Global.Now.ToString( "HH:mm" );
		}

		private void displayDiskFreeSpaceOnStatusbar()
		{
			if ( Global.Preferences.MainPath.Length < 3 )
				return;
			new GetManagementObject(
				this,
				checkDiskSpace,
				Global.Preferences.MainPath );
		}

		private void checkDiskSpace( System.Management.ManagementObject disk )
		{
			if ( disk==null )
				return;

			try
			{
				ulong lMb = ((ulong)disk["FreeSpace"]) / (ulong)(1024*1024);
				sbpDisk.Text = string.Format( "{0:0.0}Gb", lMb/1000.0 );
			}
			catch
			{
			}
		}

		private void tmrContinuous_Tick(object sender, System.EventArgs e)
		{
			displayTimeOnStatusbar();

			if ( ++_nAutoSpar>(6*2) )  //varannan minut
			{
				_nAutoSpar = 0;
				Global.saveSchool( true );
			}
			if ( ++_nRefreshDiskFreeSpace>(6*15) )  //var femtonde
			{
				_nRefreshDiskFreeSpace = 0;
				displayDiskFreeSpaceOnStatusbar();
			}

			Util.SystemPowerStatus sps;
			Util.GetSystemPowerStatus( out sps );
			sbpACDC.Text = sps.ACLineStatus==0 ? string.Format("{0}%",sps.batteryLifePercent) : "AC";
		}

		private bool preloadOneThumbnail( PlataDM.Thumbnail tn, int nAntalBilderTotalt, int nAntalLaddadeBilder )
		{
			tn.load();
			_nStatusProgress = 100*nAntalLaddadeBilder / nAntalBilderTotalt;
			sbr.Invalidate();
			return _fEnded;
		}

		private void preloadThumbnails()
		{
			int nAntalBilderTotalt = 0;
			int nAntalLaddadeBilder = 0;

			try
			{
				foreach ( var grupp in Global.Skola.Grupper )
				{
				    nAntalBilderTotalt += grupp.Thumbnails.Count;
				    nAntalBilderTotalt += grupp.AllaPersoner.Sum(pers => pers.Thumbnails.Count);
				}
			    nAntalBilderTotalt += Global.Skola.Vimmel.Count;

				foreach ( var grupp in Global.Skola.Grupper )
				{
					foreach ( PlataDM.Thumbnail tn in grupp.Thumbnails )
						if ( preloadOneThumbnail( tn, nAntalBilderTotalt, ++nAntalLaddadeBilder ) )
							return;
					foreach ( var pers in grupp.AllaPersoner )
						foreach ( PlataDM.Thumbnail tn in pers.Thumbnails )
							if ( preloadOneThumbnail( tn, nAntalBilderTotalt, ++nAntalLaddadeBilder ) )
								return;
				}
				foreach ( PlataDM.Thumbnail tn in Global.Skola.Vimmel.Thumbnails )
					if ( preloadOneThumbnail( tn, nAntalBilderTotalt, ++nAntalLaddadeBilder ) )
						return;
			}
			catch
			{
			}
			_nStatusProgress = 0;
			sbr.Invalidate();
		}

		private void sbr_DrawItem(object sender, StatusBarDrawItemEventArgs sbdevent)
		{
			var r = sbdevent.Bounds;

			if ( sbdevent.Panel==sbpMain )
			{
				if ( _nStatusProgress!=0 )
				{
					int nX = (r.Width*_nStatusProgress)/100;
					sbdevent.Graphics.FillRectangle( SystemBrushes.ControlDark, r.Left, r.Top, r.Left+nX, r.Height );
					r = new Rectangle( r.Left+nX, r.Top, r.Width-nX, r.Height );
				}

				using ( Brush brBack = new SolidBrush(sbdevent.BackColor) )
					sbdevent.Graphics.FillRectangle( brBack, r );
				using ( Brush brFore = new SolidBrush(sbdevent.ForeColor) )
					sbdevent.Graphics.DrawString( sbdevent.Panel.Text, sbdevent.Font, brFore, sbdevent.Bounds );
			}
			else if ( sbdevent.Panel==sbpSpeaker )
			{
				r = vdUsr.ImgHelper.adaptProportionalRect( r, _bmpSpeaker.Width, _bmpSpeaker.Height );
				sbdevent.Graphics.DrawImage( _bmpSpeaker, r, new Rectangle( Point.Empty, r.Size ), GraphicsUnit.Pixel );
			}

		}

		public void endApp( bool fReboot )
		{
			s_fDontReboot = !fReboot;
			Close();
		}

		private void mnuUppdateraProgra_Click(object sender, System.EventArgs e)
		{
			if ( Upgrader.runUpgradeFromAnywhere() )
			{
				endApp( false );
				return;
			}

			var strSC = Upgrader.findSCardFromAnywhere();
			if ( strSC != null )
			{
				Global.runSA( strSC, null ).WaitForExit();
				return;
			}

			Global.showMsgBox( this, "Hittar ingen uppgradering!!!" );
		}

		private void mnuCamera_Popup(object sender, System.EventArgs e)
		{
			mnuImporteraBilder.Enabled = false;
			mnuToolsCameraSettings.Enabled = _camera.IsConnected;
			if ( Global.Skola==null )
				return;
			if ( Global.Skola.ReadOnly )
				return;

			switch ( _aktivFlik.FlikTyp )
			{
				case FlikTyp.GruppbildInne:
				case FlikTyp.GruppbildUte:
				case FlikTyp.Infällning:
				case FlikTyp.Personal:
				case FlikTyp.PorträttInne:
				case FlikTyp.PorträttUte:
				case FlikTyp.Vimmel:
					mnuImporteraBilder.Enabled = _camera.CanGetImageItems;
					break;
				default:
					mnuImporteraBilder.Enabled = false;
					break;
			}
	
		}

		private void mnuImporteraBilder_Click(object sender, System.EventArgs e)
		{
			try
			{
				using ( var dlg = new FImport(_aktivFlik,_camera) )
					if ( dlg.ShowDialog(this)==DialogResult.OK )
					{
						var images = dlg.SelectedImages;
						Refresh();
						var progress = new frmProgress("Hämtar bilder...",images.Length);
						progress.Owner = this;
						progress.Show();
						foreach ( FImport.SelectedImage si in images )
						{
							switch ( _aktivFlik.FlikKategori )
							{
								case FlikKategori.Porträtt:
								case FlikKategori.Vimmel:
                                    _aktivFlik.nyttFoto(true, File.ReadAllBytes(si.FileNameJPG), null);
									break;
								case FlikKategori.Gruppbild:
							        var fn = Global.GetTempFileName();
                                    si.imgData.saveRAW(fn);
                                    _aktivFlik.nyttFoto(true, File.ReadAllBytes(si.FileNameJPG), File.ReadAllBytes(fn));
							        Util.safeKillFile(fn);
									break;
							}
							progress.increaseValue();
						}
						progress.Close();
						progress.Dispose();
					}
			}
			catch ( Exception ex )
			{
				Global.showMsgBox( this, "mnuImporteraBilder:\r\n{0}", ex.ToString() );
			}

		}

		private void mnuImporteraBildfiler_Click(object sender, System.EventArgs e)
		{
		    FImporteraBildfiler.showDialog(this, _aktivFlik, _camera );
		}

		private void sbr_PanelClick(object sender, StatusBarPanelClickEventArgs e)
		{
			if ( e.Clicks!=2 )
				return;

			if ( e.StatusBarPanel == sbpDate || e.StatusBarPanel == sbpTime )
			    Global.runSA("control.exe", "date/time");
			else if ( e.StatusBarPanel == sbpSpeaker )
				System.Diagnostics.Process.Start( "sndvol32.exe" );
			else if ( e.StatusBarPanel == sbpIkon )
				if ( _camera.IsConnected )
				{
					if ( Global.askMsgBox( this, "Koppla ur?", false ) == DialogResult.Yes )
					{
						_camera.Disconnect();
						displayCameraInfo( null );
					}
				}
				else
					initCamera();
		}

		private void mnuHelpAbout_Click(object sender, System.EventArgs e)
		{
			using ( var dlg = new frmAbout() )
				dlg.ShowDialog(this);
		}

		private void mnuDefragmentera_Click(object sender, System.EventArgs e)
		{
			Global.saveSchool( true );
			var strSysPath = Environment.GetFolderPath( Environment.SpecialFolder.System );
			System.Diagnostics.Process process = Global.runSA(
				Path.Combine( strSysPath, "mmc.exe" ),
				Path.Combine( strSysPath, "dfrg.msc" ) );
			this.Visible = false;
			process.WaitForExit();
			this.Visible = true;
		}

		private void mnuFlikVal_Click(object sender, System.EventArgs e)
		{
			int nFlik;

			if ( sender==mnuFlikVal1 )
				nFlik = 0;
			else if ( sender==mnuFlikVal2 )
				nFlik = 1;
			else if ( sender==mnuFlikVal3 )
				nFlik = 2;
			else if ( sender==mnuFlikVal4 )
				nFlik = 3;
			else if ( sender==mnuFlikVal5 )
				nFlik = 4;
			else if ( sender==mnuFlikVal6 )
				nFlik = 5;
			else if ( sender==mnuFlikVal8 )
				nFlik = 6;
			else if ( sender==mnuFlikVal9 )
				nFlik = 7;
			else if ( sender==mnuFlikVal0 )
			{
				_aktivFlik.FlikMode = FlikMode.Normal;
				_aktivFlik = _frmDummy;
				_aktivFlik.Activate();
				picTop.Invalidate();
				return;
			}
			else
				return;

			if ( _flikar[nFlik].FlikMode!=FlikMode.Disabled )
				jumpToForm_Group_Person( (FlikTyp)nFlik, null, null );
		}

		private void mnuHelpVersions_Click( object sender, EventArgs e )
		{
			using ( var dlg = new FHistorik() )
				dlg.ShowDialog( this );
		}

		private void mnuHelpKeyboard_Click( object sender, System.EventArgs e )
		{
			using ( var dlg = new FTangentbord() )
				dlg.ShowDialog(this);
		}

		private void mnuToolsCameraSettings_Click(object sender, System.EventArgs e)
		{
			using ( var dlg = new FCameraSettings(_camera) )
				dlg.ShowDialog(this);
		}

		private void runPDF( string strDoc )
		{
			strDoc = Global.getAppPath( strDoc );
			if ( File.Exists(strDoc) )
				System.Diagnostics.Process.Start( strDoc );
			else
				Global.showMsgBox( this, "Hittar inte filen \"{0}\"", strDoc );
		}

		private void mnuHelpManual_Click(object sender, System.EventArgs e)
		{
            runPDF(string.Format("fotohandbok_{0}.pdf", Global.Preferences.Brand));
		}

        private void mnuHelpEos1DsMarkII_Click(object sender, EventArgs e)
        {
            runPDF("kamerahandbok-eos1dsmarkii.pdf");
        }

        private void mnuHelpEos5D_Click(object sender, EventArgs e)
        {
            runPDF("kamerahandbok-eos5d.pdf");
        }

		private void mnuHelpSport_Click(object sender, System.EventArgs e)
		{
			runPDF( "sporthandbok.pdf" );
		}

		private void mnuFileImportNames_Click(object sender, System.EventArgs e)
		{
			var skola = new PlataDM.Skola( null, 0 );
			if ( OpenDialog.FOpenDialog.showDialog( this, true, ref skola ) != DialogResult.OK )
				return;
			if ( FNamnImport.showDialog( this, Global.Skola, skola ) == DialogResult.OK )
				rapportera_skolaUppdaterad();
		}

		private void mnuDisconnectCamera_Click( object sender, System.EventArgs e )
		{
            if ( _camera.IsConnected )
    			_camera.Disconnect();
            else
                _camera.Connect();
			displayCameraInfo(null);
		}

		private void mnuVerktygOmstart_Click(object sender, System.EventArgs e)
		{
            if (ModifierKeys == (Keys.Shift | Keys.Alt))
                Global.Preferences.Brand = (Brand) (((int) Global.Preferences.Brand + 1)%Enum.GetValues(typeof (Brand)).Length);
            endApp(false);
            Application.Restart();
		}

		public baseFlikForm jumpToForm( FlikTyp fliktyp )
		{
			_aktivFlik = _flikar[(int)fliktyp];
			_aktivFlik.fmMode = FlikMode.Active;
			if ( this.ActiveMdiChild != _aktivFlik )
				_aktivFlik.Activate();
			else
				_aktivFlik.activated();
			picTop.Invalidate();
			return _aktivFlik;
		}

		public void jumpToForm_Group_Person( FlikTyp fliktyp, PlataDM.Grupp grupp, PlataDM.Person person )
		{
			this.Cursor = Cursors.WaitCursor;

			if ( fliktyp==FlikTyp._SökHopp )
			{
				fliktyp = _aktivFlik.FlikTyp;
				switch ( grupp.GruppTyp )
				{
					case GruppTyp.GruppNormal:
					case GruppTyp.GruppKompis:
						if ( _aktivFlik.FlikKategori!=FlikKategori.Gruppbild )
							fliktyp = FlikTyp.PorträttInne;
						break;
					case GruppTyp.GruppInfällning:
						fliktyp = FlikTyp.Infällning;
						break;
					case GruppTyp.GruppPersonal:
						if ( _aktivFlik.FlikKategori==FlikKategori.Porträtt )
							fliktyp = FlikTyp.Personal;
						else
							fliktyp = FlikTyp.GruppbildInne;
						break;
				}						
			}

			if ( fliktyp!=_aktivFlik.FlikTyp )
			{
				_aktivFlik = _flikar[(int)fliktyp];
				_aktivFlik.fmMode = FlikMode.Active;
				if ( this.ActiveMdiChild!=_aktivFlik )
					_aktivFlik.Activate();
				else
					_aktivFlik.activated();
				picTop.Invalidate();
			}

			var gf = _aktivFlik as baseGruppForm;
			if ( gf!=null )
				try
				{
					Application.DoEvents();
					gf.selectGroupPerson( grupp, person );
				}
				catch
				{
				}
			this.Cursor = Cursors.Default;
		}

		private void mnuEditSearch_Click(object sender, System.EventArgs e)
		{
			if ( Global.Skola!=null )
				FSearchCriteria.search( this );
		}

		private void mnuSearchNext_Click(object sender, System.EventArgs e)
		{
		    if (Global.Skola == null)
                return;
		    if ( FSearchCriteria.canSearch )
		        FSearchCriteria.searchNext( this );
		    else
		        FSearchCriteria.search( this );
		}

	    private void mnuEdit_Popup(object sender, System.EventArgs e)
		{
			mnuEditSearch.Enabled = Global.Skola!=null;
			mnuSearchNext.Enabled = mnuEditSearch.Enabled && FSearchCriteria.canSearch;
		}

		private void mnuBackup_Click(object sender, System.EventArgs e)
		{
			if ( Global.Skola==null )
				return;
			if ( !string.IsNullOrEmpty(Global.Preferences.BackupFolder) && Directory.Exists( Global.Preferences.BackupFolder ) )
				FBackup.showDialog(
					this,
					false,
					string.Format( "Backup av {0} {1}", Global.Skola.OrderNr, Global.Skola.Namn ),
					Global.Skola.HomePath,
					Path.Combine( Global.Preferences.BackupFolder, Path.GetFileName( Global.Skola.HomePath ) ) );
			else
				Global.showMsgBox( this, "Du måste ange en existerande mapp under Programinställningar!" );
		}

		private void mnuBurnDirect_Click(object sender, System.EventArgs e)
		{
			Global.Skola.save( true );
			Burn.FDirectBurnMenu.showDialog( this );
		}

		private DialogResult askAboutOrderCopy(string strQ)
		{
			var s = string.Format( "Den här funktionen använder du om du av någon anledning behöver skapa en orderskiva " +
				"för att läsa in denna order på en annan dator.\r\n" +
				"OBS! Det blir inte en kopia av den ursprungliga skivan du fick, för alla ändringar du själv gjort, " +
				"som tex namnbyten, kommer att finnas med på den nya orderskivan.\r\n\r\nSvara Ja för att {0}.", strQ );
			return Global.askMsgBox( this, s, false );
		}

		private void mnuBurnOrder_Click(object sender, System.EventArgs e)
		{
			if ( Global.Skola==null )
				return;

			if ( askAboutOrderCopy( "bränna en Orderskiva" ) != DialogResult.Yes )
				return;

			var strFN = Global.GetTempFileName();
			File.WriteAllBytes(
				strFN,
				FCopyOrderFile.createOrderFile( Global.Skola ) );

			var al = new List<BurnFileInfo>();
			al.Add( new BurnFileInfo( strFN, null, string.Format( "order_{0}.zip", Global.Skola.OrderNr ), true ) );
			//al.Add( new BurnFileInfo( Global.Skola.HomePathCombine("!fotoorder.emf"), null, string.Format("order_{0}.emf",Global.Skola.OrderNr), false ) );
			//al.Add( new BurnFileInfo( strFN, null, string.Format("order_{0}.xml",Global.Skola.OrderNr), true ) );
			Burn.BurnHelpers.theNewAndFunBurn(
				this,
				al,
				"O",
				Global.Skola.OrderNr );
		}

		private void mnuCopyOrderFile_Click(object sender, EventArgs e)
		{
			if (Global.Skola != null)
				FCopyOrderFile.showDialog(this);
		}

		private void menuItem4_Click(object sender, System.EventArgs e)
		{
			FBackupLog.showDialog( this );
		}

		private void mnuSave_Click(object sender, System.EventArgs e)
		{
			if ( Global.Skola==null )
				return;
			Global.Skola.save(false);
			const string Insult = "Om du tror att det gör någon nytta, så...";
			if ( Insult.CompareTo(sbpMain.Text)!=0 )
				vdUsr.vdOneShotTimer.start( 1000, xxx, sbpMain.Text );
			sbpMain.Text = Insult;
		}

		private void xxx( object sender, EventArgs e )
		{
			sbpMain.Text = (sender as vdUsr.vdOneShotTimer).Tag as string;
		}

		private void munPhotoArkivPrint_Click( object sender, EventArgs e )
		{
            Photomic.ArchiveStuff.Printing.FPrint.showDialog(this, Global.Skola);
		}

		private void mnuVerktygD50_Click( object sender, EventArgs e )
		{
			try
			{
				var hWnd = Util.FindWindow( null, "D-50" );
				if ( hWnd != IntPtr.Zero )
				{
					Util.forceSetForegroundWindow( Handle, hWnd );
					return;
				}

				Global.runSA(
					Path.Combine(
						Environment.GetFolderPath( Environment.SpecialFolder.ProgramFiles ),
						@"D-50\D-50\bin\D-50.exe" ),
					null );
			}
			catch ( Exception ex )
			{
				Global.showMsgBox( this, "Kunde inte starta D-50\r\n\r\n{0}", ex.ToString() );
			}
		}

		private void mnuStandbylista_Click( object sender, EventArgs e )
		{
			FStandbylistor.showDialog( this );
		}

		private void mnuStudentCardPrint_Click( object sender, EventArgs e )
		{
			if ( Global.Skola != null && Global.Skola.StudentCardTemplates.Count != 0 )
				FPrintStudentCards.showDialog( this, Global.Skola );
			else
				Global.showMsgBox( this, "Denna order saknar StudentCard-mall!" );
		}

		private void menuItem12_Click( object sender, EventArgs e )
		{
			Notes.FNotes.showDialog( this );
		}

		private void printerAction( ShellLib.ShellApi.PrinterActions action )
		{
			if ( Printer.findPrinter() )
				ShellLib.ShellApi.SHInvokePrinterCommand(
					this.Handle,
					(uint)action,
					Printer.PrinterName,
					"",
					0 );
			else
				Global.showMsgBox( this, "Hittar ingen skrivare!" );
		}

		private void mnuStudentCardPrinterQue_Click( object sender, EventArgs e )
		{
			printerAction( ShellLib.ShellApi.PrinterActions.PRINTACTION_OPEN );
		}

		public void setHistogram( Bitmap bmp )
		{
			if ( _histogramForm != null && !_histogramForm.IsDisposed )
				_histogramForm.setData( bmp );
		}

		void histogram_FormClosed( object sender, FormClosedEventArgs e )
		{
			mnuShowHistogram.Checked = false;
			_histogramForm = null;
		}

		private void mnuShowHistogram_Click( object sender, EventArgs e )
		{
			mnuShowHistogram.Checked = !mnuShowHistogram.Checked;
			if ( mnuShowHistogram.Checked && _histogramForm == null )
			{
				var p = new Point(
					ClientSize.Width,
					picTop.Bottom );
				_histogramForm = new Dialogs.FHistogram( PointToScreen( p ) );
				_histogramForm.Show( this );
				Focus();
				_histogramForm.FormClosed += histogram_FormClosed;
				_histogramForm.setData( _aktivFlik.currentBitmap() );
			}
			else if ( !mnuShowHistogram.Checked && _histogramForm != null )
				_histogramForm.Close();
		}

		private void mnuArkivEgenskaper_Click( object sender, EventArgs e )
		{
			if ( Global.Skola == null )
				return;
			if ( Dialogs.FOrderProperties.showDialog( this, Global.Skola ) == DialogResult.OK )
			{
				rapportera_skolaUppdaterad();
				picTop.Invalidate();
			}
		}

        private void mnuWiFiInloggning_Click(object sender, EventArgs e)
        {
            WiFiInlogg.FWiFiInlogg.showDialog(this);
        }

	}

}
