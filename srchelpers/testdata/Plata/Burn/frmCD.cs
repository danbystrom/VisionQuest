using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;

using XPBurn;

namespace Plata
{
	public class frmCDx : Plata.baseFlikForm
	{
		private System.ComponentModel.IContainer components = null;

		private XPBurnCD m_burn;
		private string m_strBurnError;
		private ComboBox cboCDP;
		private ComboBox cboCDT;
		private TreeView treeFiles;
		private Button cmdCreate;
		private Button cmdBurn;

		private string _strPathBase;
		private string _strPath;  //mapp som skapas / bränns
		private string m_strAktuelltSteg;  //för felrapportering: var i bränningen befinner vi oss?
		private string m_strAktuellFil;  //för felrapportering: vilken fil arbetar vi med?

		private ArrayList m_arrBurnobj;
		private int m_nCDTyp = 0;
		private System.Windows.Forms.RadioButton optPhotomicCD;
		private System.Windows.Forms.RadioButton optThieleCD;
		private System.Windows.Forms.RadioButton optPhotomicDVD;
		private System.Windows.Forms.RadioButton optThieleDVD;
		private System.Windows.Forms.RadioButton optWhoIsWho;
		private System.Windows.Forms.RadioButton optProvision;

		private PlataDM.Skola _skola;
		private string _strMediaType = "DVD";
		private System.Windows.Forms.Button cmdNewThiele;
		private System.Windows.Forms.Button cmdCompleteBurn;
		private int _nSizeOfJob;

		private int _nCD = 0;

		public frmCDx() : base()
		{
		}

		public frmCDx( Form parent ) : base( parent, FlikTyp._Ingen )
		{
			m_strCaption = "BRÄNN";
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			ImageList iml = new ImageList();
			iml.ColorDepth = ColorDepth.Depth24Bit;
			iml.TransparentColor = Color.Magenta;
			iml.ImageSize = new Size(16,16);
			iml.Images.AddStrip( new Bitmap(this.GetType(), "treeicons.bmp") );
			treeFiles.ImageList = iml;
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
			this.cmdCreate = new System.Windows.Forms.Button();
			this.cboCDP = new System.Windows.Forms.ComboBox();
			this.optPhotomicCD = new System.Windows.Forms.RadioButton();
			this.optThieleCD = new System.Windows.Forms.RadioButton();
			this.cboCDT = new System.Windows.Forms.ComboBox();
			this.treeFiles = new System.Windows.Forms.TreeView();
			this.cmdBurn = new System.Windows.Forms.Button();
			this.optPhotomicDVD = new System.Windows.Forms.RadioButton();
			this.optThieleDVD = new System.Windows.Forms.RadioButton();
			this.optWhoIsWho = new System.Windows.Forms.RadioButton();
			this.optProvision = new System.Windows.Forms.RadioButton();
			this.cmdNewThiele = new System.Windows.Forms.Button();
			this.cmdCompleteBurn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cmdCreate
			// 
			this.cmdCreate.Location = new System.Drawing.Point(736, 32);
			this.cmdCreate.Name = "cmdCreate";
			this.cmdCreate.Size = new System.Drawing.Size(140, 36);
			this.cmdCreate.TabIndex = 8;
			this.cmdCreate.Text = "Skapa CD / DVD";
			this.cmdCreate.Click += new System.EventHandler(this.cmdCreate_Click);
			// 
			// cboCDP
			// 
			this.cboCDP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboCDP.Location = new System.Drawing.Point(456, 28);
			this.cboCDP.Name = "cboCDP";
			this.cboCDP.Size = new System.Drawing.Size(180, 21);
			this.cboCDP.TabIndex = 4;
			this.cboCDP.TabStop = false;
			this.cboCDP.SelectedIndexChanged += new System.EventHandler(this.cboCDP_SelectedIndexChanged);
			// 
			// optPhotomicCD
			// 
			this.optPhotomicCD.BackColor = System.Drawing.Color.WhiteSmoke;
			this.optPhotomicCD.Location = new System.Drawing.Point(292, 32);
			this.optPhotomicCD.Name = "optPhotomicCD";
			this.optPhotomicCD.Size = new System.Drawing.Size(160, 14);
			this.optPhotomicCD.TabIndex = 2;
			this.optPhotomicCD.TabStop = true;
			this.optPhotomicCD.Text = "Backup-CD till Photomic";
			// 
			// optThieleCD
			// 
			this.optThieleCD.BackColor = System.Drawing.Color.WhiteSmoke;
			this.optThieleCD.Location = new System.Drawing.Point(292, 60);
			this.optThieleCD.Name = "optThieleCD";
			this.optThieleCD.Size = new System.Drawing.Size(160, 14);
			this.optThieleCD.TabIndex = 3;
			this.optThieleCD.TabStop = true;
			this.optThieleCD.Text = "Backup-CD till Thiele";
			// 
			// cboCDT
			// 
			this.cboCDT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboCDT.Location = new System.Drawing.Point(456, 56);
			this.cboCDT.Name = "cboCDT";
			this.cboCDT.Size = new System.Drawing.Size(180, 21);
			this.cboCDT.TabIndex = 5;
			this.cboCDT.TabStop = false;
			this.cboCDT.SelectedIndexChanged += new System.EventHandler(this.cboCDT_SelectedIndexChanged);
			// 
			// treeFiles
			// 
			this.treeFiles.AllowDrop = true;
			this.treeFiles.ImageIndex = -1;
			this.treeFiles.Location = new System.Drawing.Point(24, 160);
			this.treeFiles.Name = "treeFiles";
			this.treeFiles.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
																																					new System.Windows.Forms.TreeNode("Inga filer valda för bränning...")});
			this.treeFiles.SelectedImageIndex = -1;
			this.treeFiles.Size = new System.Drawing.Size(368, 248);
			this.treeFiles.TabIndex = 9;
			this.treeFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeFiles_KeyDown);
			this.treeFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeFiles_DragEnter);
			this.treeFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeFiles_DragDrop);
			// 
			// cmdBurn
			// 
			this.cmdBurn.Enabled = false;
			this.cmdBurn.Location = new System.Drawing.Point(408, 372);
			this.cmdBurn.Name = "cmdBurn";
			this.cmdBurn.Size = new System.Drawing.Size(140, 36);
			this.cmdBurn.TabIndex = 10;
			this.cmdBurn.Text = "BRÄNN";
			this.cmdBurn.Click += new System.EventHandler(this.cmdBurn_Click);
			// 
			// optPhotomicDVD
			// 
			this.optPhotomicDVD.BackColor = System.Drawing.Color.WhiteSmoke;
			this.optPhotomicDVD.Location = new System.Drawing.Point(28, 32);
			this.optPhotomicDVD.Name = "optPhotomicDVD";
			this.optPhotomicDVD.Size = new System.Drawing.Size(208, 14);
			this.optPhotomicDVD.TabIndex = 0;
			this.optPhotomicDVD.TabStop = true;
			this.optPhotomicDVD.Text = "DVD - Slutleverans till Photomic";
			// 
			// optThieleDVD
			// 
			this.optThieleDVD.BackColor = System.Drawing.Color.WhiteSmoke;
			this.optThieleDVD.Location = new System.Drawing.Point(28, 60);
			this.optThieleDVD.Name = "optThieleDVD";
			this.optThieleDVD.Size = new System.Drawing.Size(208, 14);
			this.optThieleDVD.TabIndex = 1;
			this.optThieleDVD.TabStop = true;
			this.optThieleDVD.Text = "DVD - Slutleverans till Thiele";
			// 
			// optWhoIsWho
			// 
			this.optWhoIsWho.BackColor = System.Drawing.Color.WhiteSmoke;
			this.optWhoIsWho.Enabled = false;
			this.optWhoIsWho.Location = new System.Drawing.Point(28, 92);
			this.optWhoIsWho.Name = "optWhoIsWho";
			this.optWhoIsWho.Size = new System.Drawing.Size(208, 14);
			this.optWhoIsWho.TabIndex = 6;
			this.optWhoIsWho.TabStop = true;
			this.optWhoIsWho.Text = "Who is who - Arkiv-CD";
			// 
			// optProvision
			// 
			this.optProvision.BackColor = System.Drawing.Color.WhiteSmoke;
			this.optProvision.Location = new System.Drawing.Point(292, 92);
			this.optProvision.Name = "optProvision";
			this.optProvision.Size = new System.Drawing.Size(208, 14);
			this.optProvision.TabIndex = 7;
			this.optProvision.TabStop = true;
			this.optProvision.Text = "Provisionsarkiv";
			// 
			// cmdNewThiele
			// 
			this.cmdNewThiele.Location = new System.Drawing.Point(544, 88);
			this.cmdNewThiele.Name = "cmdNewThiele";
			this.cmdNewThiele.Size = new System.Drawing.Size(216, 28);
			this.cmdNewThiele.TabIndex = 11;
			this.cmdNewThiele.Text = "Skapa och bränn Thieleskiva - SNABB!!!";
			// 
			// cmdCompleteBurn
			// 
			this.cmdCompleteBurn.Location = new System.Drawing.Point(768, 88);
			this.cmdCompleteBurn.Name = "cmdCompleteBurn";
			this.cmdCompleteBurn.Size = new System.Drawing.Size(96, 28);
			this.cmdCompleteBurn.TabIndex = 12;
			this.cmdCompleteBurn.Text = "Komplettbränn";
			this.cmdCompleteBurn.Click += new System.EventHandler(this.cmdCompleteBurn_Click);
			// 
			// frmCD
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(892, 566);
			this.Controls.Add(this.cmdCompleteBurn);
			this.Controls.Add(this.cmdNewThiele);
			this.Controls.Add(this.optProvision);
			this.Controls.Add(this.optWhoIsWho);
			this.Controls.Add(this.optThieleDVD);
			this.Controls.Add(this.optPhotomicDVD);
			this.Controls.Add(this.cmdBurn);
			this.Controls.Add(this.treeFiles);
			this.Controls.Add(this.optThieleCD);
			this.Controls.Add(this.cboCDT);
			this.Controls.Add(this.optPhotomicCD);
			this.Controls.Add(this.cboCDP);
			this.Controls.Add(this.cmdCreate);
			this.Name = "frmCD";
			this.ResumeLayout(false);

		}
		#endregion

		private void addFiles( string strHardDriveDirectory, string strCDDirectory )
		{
			foreach ( string s in Directory.GetFiles(strHardDriveDirectory) )
			{
				FileAttributes fa = File.GetAttributes(s);
				if ( (fa & FileAttributes.ReadOnly) != 0 )
					File.SetAttributes( s, fa&~FileAttributes.ReadOnly );
				m_burn.AddFile( s, Path.Combine(strCDDirectory,Path.GetFileName(s)) );
			}
			foreach ( string s in Directory.GetDirectories(strHardDriveDirectory) )
				addFiles( s, Path.Combine(strCDDirectory,Path.GetFileName(s)) );
		}

		private TreeNode viewBurnTreeNode( TreeNodeCollection nodes, string strFile, ref long lSize )
		{
			int nImage = 5;
			switch ( strFile.Substring(strFile.Length-3,3) )
			{
				case "txt": nImage = 2; break;
				case "jpg": nImage = 3; break;
				case "tif": case "cr2": nImage = 4; break;
				case "xml": nImage = 6; break;
				case "log": nImage = 2; break;
			}
			TreeNode tn = new TreeNode( Path.GetFileName(strFile), nImage, nImage );
			nodes.Add( tn );
			try
			{
				FileInfo fi = new FileInfo(strFile);
				lSize += 1+fi.Length/1024;
				return tn;
			}
			catch
			{
				return null;
			}
		}

		private TreeNode viewBurnTree( TreeNodeCollection nodes, string strPath, int nTreeImage, ref long lSize )
		{
			TreeNode tn = new TreeNode( Path.GetFileName(strPath), nTreeImage, nTreeImage );
			nodes.Add( tn );

			foreach ( string s in Directory.GetDirectories(strPath) )
				viewBurnTree( tn.Nodes, s, 1, ref lSize );
			foreach ( string s in Directory.GetFiles(strPath) )
				viewBurnTreeNode( tn.Nodes, s, ref lSize );
			return tn;
		}
/*
		private void resetDelivery( PlataDM.CDLeveranser cdlev )
		{
			System.GC.Collect();
			foreach ( string strDir in Directory.GetDirectories( Path.GetDirectoryName(_strPathBase), Path.GetFileName(_strPathBase)+"*") )
				Global.killDirectory( strDir );
			cdlev.Clear();
			_skola.clearCDT( m_nCDTyp );
			skolaUppdaterad();
		}
*/
		private void cmdCreate_Click(object sender, System.EventArgs e)
		{
			/*
			PlataDM.CDLeveranser cdlev;
			ComboBox cbo;
			bool fDVD;

			cmdBurn.Enabled = false;

			if ( optPhotomicCD.Checked || optPhotomicDVD.Checked )
			{
				fDVD = optPhotomicDVD.Checked;
				m_nCDTyp = 0;
				cdlev = _skola.CDLeveranserPhotomic;
				cbo = cboCDP;
				_strPathBase = Path.Combine( _skola.HomePath, "Photomic" );
			}
			else if ( optThieleCD.Checked || optThieleDVD.Checked )
			{
				fDVD = optThieleDVD.Checked;
				m_nCDTyp = 1;
				cdlev = _skola.CDLeveranserThiele;
				cbo = cboCDT;
				_strPathBase = Path.Combine( _skola.HomePath, "Thiele" );
			}
			else if ( optProvision.Checked )
			{
				fDVD = false;
				m_nCDTyp = 3;
				cdlev = null;
				cbo = null;
				_strPath = _strPathBase = Path.Combine( _skola.HomePath, "Provisionsarkiv" );
				Util.safeKillDirectory( _strPath );
			}
			else
				return;

			if ( fDVD )
			{
				_strPath = _strPathBase + " DVD SISTA";
				_nCD = 0;
			}
			else if ( cbo!=null )
			{
				_nCD = 1 + cdlev.Count - cbo.SelectedIndex;
				if ( _nCD==0 && !fDVD )
				{
					if ( Global.askMsgBox( this, "Är du säker?", true ) == DialogResult.Yes )
					{
						resetDelivery( cdlev );
						treeFiles.Nodes.Clear();
						_strPathBase = null;
					}
					return;
				}
				_strPath = _strPathBase + " CD " + _nCD.ToString();
			}

			try 
			{
				m_strAktuelltSteg = "Initiering";
				m_strAktuellFil = "";
				if ( !Directory.Exists(_strPath) )
				{
					switch ( m_nCDTyp )
					{
						case 0:
							if ( fDVD )
								if ( !isPreperedForFinalBurn() )
									return;
							this.Cursor = Cursors.WaitCursor;
							skapaFiler_Photomic();
							break;
						case 1:
							this.Cursor = Cursors.WaitCursor;
							skapaFiler_Thiele();
							break;
						case 3:
							this.Cursor = Cursors.WaitCursor;
							burnComission();
							break;
					}
					setStatusText( "Export klar" );
				}
				treeFiles.Nodes.Clear();
				long lSize = 0;
				viewBurnTree( treeFiles.Nodes, _strPath, 0, ref lSize );
				if ( m_nCDTyp==0 && fDVD )
					läggTillSlask( ref lSize );
				treeFiles.Nodes[0].Expand();
				_nSizeOfJob = (int)(lSize/1024);
				_strMediaType = _nSizeOfJob>600 ? "DVD" : "CD";
				cmdBurn.Enabled = m_burn!=null;
				Invalidate();
			}
			catch ( Exception ex )
			{
				setStatusText( ex.Message );
				string strMess = "Fel vid skapande av CD/DVD:\r\n  Steg: " + m_strAktuelltSteg +"\r\n  Fil: " + m_strAktuellFil;
				Global.storeErrorReport( ex, "Auto: " + strMess );
				Global.showMsgBox( null, strMess + "\r\n\r\nFelmeddelande: " + ex.Message );
				System.GC.Collect();
				Global.killDirectory(	_strPath );
				sättSteg( "" );
			}
			this.Cursor = Cursors.Default;
*/
					}

		private void sättSteg( string strSteg )
		{
			m_strAktuelltSteg = strSteg;
			setStatusText( strSteg + "..." );
		}

		private void myFileCopy( string strSrc, string strDst )
		{
			m_strAktuellFil = "Från: " + strSrc + " Till: " + strDst;
			File.Copy( strSrc, strDst, true );
		}

		#region Skapa filer till Photomic

		private void stuffHash( Hashtable hash, PlataDM.Thumbnails tns, string strKey )
		{
			PlataDM.Thumbnail tn = tns[strKey];
			if ( tn!=null )
			{
				string strFN;
				if ( !Util.isEmpty(tn.FilenameRAW) )
					strFN = Path.GetFileName(tn.FilenameRAW);
				else
					strFN = Path.GetFileName(tn.FilenameJPG);
				if ( !hash.ContainsKey(strFN) )
					hash.Add( strFN, strFN );
			}
		}
/*
		private string gruppnamnFrånFilnamn( string strFN )
		{
			foreach ( PlataDM.Grupp grupp in _skola.Grupper )
			{
				foreach ( PlataDM.Thumbnail tn in grupp.ThumbnailsGrupp )
					if ( tn.hasFilename(strFN) )
						return grupp.Namn;
				foreach ( PlataDM.Thumbnail tn in grupp.ThumbnailsPorträtt )
					if ( tn.hasFilename(strFN) )
						return grupp.Namn;
			}
			return "saknar grupp";
		}

		private void läggTillSlask( ref long lSize )
		{
			Hashtable hashCopied = new Hashtable();
			Hashtable hashGrupper = new Hashtable();

			sättSteg( "Skapar slask" );

			TreeNode tnSlask = treeFiles.Nodes[0].Nodes.Add( "Slask" );

			tnSlask.SelectedImageIndex = tnSlask.ImageIndex = 1;

			foreach ( PlataDM.Grupp grupp in _skola.Grupper )
			{
				stuffHash( hashCopied, grupp.ThumbnailsGrupp, grupp.ThumbnailKey );
				stuffHash( hashCopied, grupp.ThumbnailsGrupp, grupp.ThumbnailGrayKey );
				foreach ( PlataDM.Person person in grupp.TotalListaPersoner() )
					if ( !Util.isEmpty(person.Filmrapport) )
					{
						stuffHash( hashCopied, grupp.ThumbnailsPorträtt, person.ThumbnailKey );
						stuffHash( hashCopied, grupp.ThumbnailsPorträtt, person.ThumbnailKey2 );
					}
			}
			foreach ( PlataDM.Vimmelbild vb in _skola.Vimmel )
				stuffHash( hashCopied, _skola.Vimmel.Thumbnails, vb.Key );

			foreach ( string strFN in Directory.GetFiles(_skola.HomePath) )
			{
				string s = Path.GetFileName( strFN );
				if ( s.IndexOf("_r_")>0 || s.IndexOf("_j_")>0 || s.IndexOf("_p_")>0 )
					if ( s.LastIndexOf("_thumb")<0 && s.LastIndexOf(".bmp")<0 )
						if ( !hashCopied.Contains(s) )
						{
							TreeNode tnGrupp;
							string strGrupp = gruppnamnFrånFilnamn(s);
							if ( hashGrupper.ContainsKey(strGrupp) )
								tnGrupp = (TreeNode)hashGrupper[strGrupp];
							else
							{
								tnGrupp = new TreeNode( strGrupp, 1, 0 );
								hashGrupper.Add( strGrupp, tnGrupp = tnGrupp );
								tnSlask.Nodes.Add( tnGrupp );
							}
							TreeNode tn = viewBurnTreeNode( tnGrupp.Nodes, strFN, ref lSize );
							if ( tn!=null )
								tn.Tag = new string[] { strFN, "\\slask\\" + Global.skapaSäkertFilnamn(strGrupp) + "\\" + s };
						}
			}

			sättSteg( "Klar för bränning" );
		}
*/

		private void skapaPersonSpecialLista( PlataDM.Grupper grupper, ref string strLista )
		{
			foreach ( PlataDM.Grupp grupp in grupper )
				foreach ( PlataDM.Person person in grupp.TotalListaPersoner() )
					switch ( person.Special )
					{
						case PlataDM.PersonSpecial.Skyddad:
							strLista += "*** \"" + person.Namn + "\" i \"" + grupp.Namn + "\" har skyddad identitet!\r\n";
							break;
						case PlataDM.PersonSpecial.Gökunge:
							strLista += "*** \"" + person.Namn + "\" i \"" + grupp.Namn + "\" ska ej vara med i katalogen!\r\n";
							break;
					}
		}

		private void skapaSäkertFilnamn( ref string strFN, ref string strRapport )
		{
			string strNyttFN = Global.skapaSäkertFilnamn(strFN);
			if ( strNyttFN!=strFN )
			{
				if ( strRapport!=null )
					strRapport += strFN + "  ->  " + strNyttFN + "\r\n";
				strFN = strNyttFN;
			}
		}
/*
		private void skapaFiler_Photomic()
		{
			PlataDM.Skola skola = _skola;
			string strBP_Anm, strBP_GHI, strBP_GLO, strBP_GTX, strBP_IHI, strBP_ITX, strBP_KOR, strBP_PLO, strBP_PTX, strBP_Vim;
			int nMaxAntalBilder, nBildräknare;
			ArrayList arrFRSUsed = new ArrayList();
			string strSOBas = Global.skapaSäkertFilnamn( _skola.OrderNr + "_" + _skola.Namn + "_" );
			string strBP = Path.Combine( _strPath, strSOBas );
			StreamWriter w;
			string strNamnbytesrapport = "";
			ArrayList alRader;

			//information för skapande av lågupplösta gruppbilder
			//			Size szGruppLo = new Size(682,496);
			//			int nGSWidth = 3622;
			//			int nGSHeight = nGSWidth*szGruppLo.Height/szGruppLo.Width;
			//			Rectangle rectGruppLo = new Rectangle((4064-nGSWidth)/2,2704-nGSHeight,nGSWidth,nGSHeight);
			//skapa encoder för gruppbilds-jpg
			ImageCodecInfo myImageCodecInfo = Util.GetEncoderInfo("image/jpeg");
			EncoderParameters myEncoderParameters = new EncoderParameters(1);
			myEncoderParameters.Param[0] = new EncoderParameter( Encoder.Quality, 88L );

			m_arrBurnobj = new ArrayList();

			sättSteg( "Skapar CD-struktur" );

			Directory.CreateDirectory( m_strAktuellFil = strBP_Anm = strBP + "anm" );
			Directory.CreateDirectory( m_strAktuellFil = strBP_GHI = strBP + "grupp_hi" );
			Directory.CreateDirectory( m_strAktuellFil = strBP_GLO = strBP + "grupp_lo" );
			Directory.CreateDirectory( m_strAktuellFil = strBP_GTX = strBP + "grupp_text" );
			Directory.CreateDirectory( m_strAktuellFil = strBP_IHI = strBP + "inf_hi" );
			Directory.CreateDirectory( m_strAktuellFil = strBP_ITX = strBP + "inf_text" );
			Directory.CreateDirectory( m_strAktuellFil = strBP_KOR = strBP + "klassordning" );
			Directory.CreateDirectory( m_strAktuellFil = strBP_PLO = Path.Combine( _strPath, "school" ) );
			Directory.CreateDirectory( m_strAktuellFil = strBP_PTX = strBP + "port_text" );
			Directory.CreateDirectory( m_strAktuellFil = strBP_Vim = strBP + "vimmel" );

			nBildräknare = 0;
			nMaxAntalBilder = skola.Grupper.Count + skola.Vimmel.Count;
			foreach ( PlataDM.Grupp grupp in skola.Grupper )
				nMaxAntalBilder += grupp.Personer.Count + grupp.PersonerFrånvarande.Count + grupp.PersonerSlutat.Count;

			//grupp_hi & grupp_lo
			sättSteg( "Exporterar grupper" );
			foreach ( PlataDM.Grupp grupp in skola.Grupper )
			{
				PlataDM.Thumbnail tnGrupp = grupp.ThumbnailsGrupp[grupp.ThumbnailKey];
				PlataDM.Thumbnail tnGrå = grupp.ThumbnailsGrupp[grupp.ThumbnailGrayKey];
				bool fExpGrupp = false;
				bool fExpGrå = false;

				if ( tnGrupp!=null )
					fExpGrupp = tnGrupp.getCD(m_nCDTyp)==0 || _nCD==0;
				if ( tnGrå!=null )
					fExpGrå = tnGrå.getCD(m_nCDTyp)==0 || _nCD==0;
				if ( fExpGrupp || fExpGrå )
				{
					string strFN = grupp.Namn + "_" + grupp.Siffror.Count.ToString();
					skapaSäkertFilnamn( ref strFN, ref strNamnbytesrapport );
					if ( fExpGrupp )
					{
						myFileCopy( tnGrupp.FilenameRAW, Path.Combine(strBP_GHI,strFN) + tnGrupp.correctRAWExt );
						tnGrupp.saveGroupLo( m_strAktuellFil = Path.Combine(strBP_GLO,strFN+".jpg"),
							myImageCodecInfo, myEncoderParameters, false );
						markBurn( tnGrupp );
					}
					if ( fExpGrå )
					{
						myFileCopy( tnGrå.FilenameRAW, Path.Combine(strBP_GHI,strFN+"_gray") + tnGrå.correctRAWExt );
						markBurn( tnGrå );
					}
				}
				frmMain.setStatusProgress( ++nBildräknare, nMaxAntalBilder );
				Application.DoEvents();
			}

			//port_lo
			sättSteg( "Exporterar porträtt" );
			foreach ( PlataDM.Grupp grupp in skola.Grupper )
				foreach ( PlataDM.Person person in grupp.TotalListaPersoner() )
				{
					if ( grupp.GruppTyp!=PlataDM.GruppTyp.GruppInfällning )
					{
						if ( (person.getCD(m_nCDTyp)==0 || _nCD==0) && !Util.isEmpty(person.Filmrapport) )
						{
							PlataDM.Thumbnail tn = grupp.ThumbnailsPorträtt[person.ThumbnailKey];
							PlataDM.Thumbnail tn2 = grupp.ThumbnailsPorträtt[person.ThumbnailKey2];
							if ( tn!=null || tn2!=null )
							{
								string strFR = person.Filmrapport.Split('_')[0];
								string strPath = Path.Combine(strBP_PLO, strFR ) + "\\small";
								if ( !Directory.Exists(strPath) )
									Directory.CreateDirectory( m_strAktuellFil = strPath );
								arrFRSUsed.Add( strFR );
								if ( tn!=null )
								{
									m_strAktuellFil = strPath + "\\" + person.Filmrapport + ".jpg";
									tn.saveResized( m_strAktuellFil, 384, 255, Global.PorträttRotateFlipType, false );
								}
								if ( tn2!=null )
								{
									m_strAktuellFil = strPath + "\\" + person.Filmrapport + "_2.jpg";
									tn2.saveResized( m_strAktuellFil, 384, 255, Global.PorträttRotateFlipType, false );
								}
								markBurn( person );
							}
						}
					}
					else  //infällning
						if ( (person.getCD(m_nCDTyp)==0 || _nCD==0) && !Util.isEmpty(person.ThumbnailKey) )
					{
						PlataDM.Thumbnail tn = grupp.ThumbnailsPorträtt[person.ThumbnailKey];
						PlataDM.Thumbnail tn2 = grupp.ThumbnailsPorträtt[person.ThumbnailKey2];
						string strNamn =
							Global.skapaSäkertFilnamn( person.Titel + " - " + person.Förnamn + " " + person.Efternamn );
						if ( tn!=null )
							myFileCopy( tn.FilenameJPG,Path.Combine(strBP_IHI,strNamn + ".jpg") );
						if ( tn2!=null )
							myFileCopy( tn2.FilenameJPG,Path.Combine(strBP_IHI,strNamn + "_2.jpg") );
						markBurn( person );
					}
					frmMain.setStatusProgress( ++nBildräknare, nMaxAntalBilder );
					Application.DoEvents();
				}
			//skapa tomma "large"-kataloger
			foreach ( string s in arrFRSUsed )
			{
				string strPath = Path.Combine(strBP_PLO, s ) + "\\large";
				Directory.CreateDirectory( m_strAktuellFil = strPath );
				w = new StreamWriter( m_strAktuellFil = strPath + "\\" + s + ".txt" );
				w.WriteLine( s );
				w.Close();
			}

			//vimmel
			sättSteg( "Exporterar vimmelfoton" );
			int nVimmelIndex = 0;
			foreach ( PlataDM.Vimmelbild vb in skola.Vimmel )
				try
				{
					PlataDM.Thumbnail tn = vb.Thumbnail;
					if ( (vb.getCD(m_nCDTyp)==0 || _nCD==0) && vb.Status!=PlataDM.VimmelStatus.Raderad && tn!=null )
					{
						string strFN = (++nVimmelIndex).ToString() + ".jpg"; // Path.GetFileName(tn.Key);
						bool fDidCopy = false;

						strFN = Global.skapaSäkertFilnamn(skola.Namn) + "_" + strFN;
						switch ( vb.Status )
						{
							case PlataDM.VimmelStatus.Lovad:
								strFN = "x_" + strFN;
								break;
							case PlataDM.VimmelStatus.Vald:
								strFN = "v_" + strFN;
								break;
						}
						strFN = Path.Combine(strBP_Vim,strFN);
						using ( Bitmap bmp = (Bitmap)Bitmap.FromFile(tn.FilenameJPG) )
							if ( bmp.Width>1024 || bmp.Height>768 )
							{
								Rectangle rectNew = Global.adaptRect( new Rectangle(0,0,1024,768),bmp.Width,bmp.Height );
								rectNew.Offset( -rectNew.Left, -rectNew.Top );
								using ( Bitmap bmpNew = new Bitmap(rectNew.Width,rectNew.Height) )
								{
									using ( Graphics g = Graphics.FromImage(bmpNew) )
									{
										g.InterpolationMode = InterpolationMode.HighQualityBicubic;
										g.DrawImage( bmp, rectNew, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel );
									}
									bmpNew.Save( strFN, myImageCodecInfo, myEncoderParameters );
								}
								fDidCopy = true;
							}
						if ( !fDidCopy )
							myFileCopy( tn.FilenameJPG, strFN );
						markBurn( vb );
					}
					frmMain.setStatusProgress( ++nBildräknare, nMaxAntalBilder );
					Application.DoEvents();
				}
				catch ( Exception ex )
				{
					Global.storeErrorReport( ex, "Fel vid bränning av vimmelfoto" );
					Global.showMsgBox( null, "Ett fel inträffade vid prepareringen av en vimmelbild! En rapport har skapats automatiskt och du kan fortsätta ändå!" );
				}

			sättSteg( "Exporterar textfiler" );
			//klassordning
			w = new StreamWriter( m_strAktuellFil = Path.Combine(strBP_KOR,strSOBas+"klassrapport.txt") ); 
			w.WriteLine( String.Format("{0}\t{1}\t{2}",skola.Namn,skola.Ort,skola.OrderNr).ToUpper() );
			w.WriteLine( "1. Vimmel" );
			int nRadnum = 2;
			foreach ( PlataDM.Grupp grupp in skola.Grupper.GrupperIOrdning() )
				if ( grupp.ThumbnailKey!=null && grupp.ThumbnailKey.Length!=0 )
					w.WriteLine( "{0}. {1}", nRadnum++, grupp.Namn.ToUpper() );
			w.Close();
			if ( !Util.isEmpty(strNamnbytesrapport) )
			{
				w = new StreamWriter( m_strAktuellFil = Path.Combine(strBP_KOR,strSOBas+"filnamnsbyten.txt") ); 
				w.WriteLine( strNamnbytesrapport );
				w.Close();
			}

			//grupp_text
			w = new StreamWriter( m_strAktuellFil = Path.Combine(strBP_GTX,strSOBas+"grupp_text.txt") ); 
			w.WriteLine( "10\t" + skola.Namn.ToUpper() + "\t" + skola.Ort.ToUpper() );
			foreach ( PlataDM.Grupp grupp in skola.Grupper )
				switch ( grupp.Numrering )
				{
					case PlataDM.GruppNumrering.Klar:
					{
						string[] astrTitlar;
						int[] anStartIndex;
						w.WriteLine( "20\t" + grupp.Namn.ToUpper() );
						foreach ( PlataDM.Person person in grupp.Personer  )
							w.WriteLine( "30\t{0}\t{1}\t{2}", person.FörnamnMedTitelFörst, person.Efternamn, person.SiffraEtikett );
						foreach ( PlataDM.Person person in grupp.PersonerFrånvarande  )
							w.WriteLine( "30\t{0}\t{1}\tF", person.FörnamnMedTitelFörst, person.Efternamn );
						foreach ( PlataDM.Person person in grupp.PersonerSlutat )
							w.WriteLine( "30\t{0}\t{1}\tU", person.FörnamnMedTitelFörst, person.Efternamn );
						grupp.Siffror.hämtaRadbeskrivningar( out astrTitlar, out anStartIndex );
						for ( int i=0 ; i<astrTitlar.Length ; i++ )
							w.WriteLine( "40\t{0}\t{1}-{2}", astrTitlar[i], anStartIndex[i], anStartIndex[i+1]-1 );
						break;
					}

					case PlataDM.GruppNumrering.EjNamnsättning:
					{
						w.WriteLine( "20\t" + grupp.Namn.ToUpper() );
						break;
					}

					case PlataDM.GruppNumrering.EjNumrering:
					{
						String[] arr = new String[grupp.Personer.Count];
						int i=0;
						foreach ( PlataDM.Person person in grupp.Personer  )
							arr[i++] = String.Format("30\t{0}\t{1}\t", person.FörnamnMedTitelFörst, person.Efternamn );
						Array.Sort( arr );

						w.WriteLine( "20\t" + grupp.Namn.ToUpper() );
						for ( i=0 ; i<arr.Length ; i++ )
							w.WriteLine( arr[i] + (i+1).ToString() );
						foreach ( PlataDM.Person person in grupp.PersonerFrånvarande  )
							w.WriteLine( "30\t{0}\t{1}\tF", person.FörnamnMedTitelFörst, person.Efternamn );
						foreach ( PlataDM.Person person in grupp.PersonerSlutat )
							w.WriteLine( "30\t{0}\t{1}\tU", person.FörnamnMedTitelFörst, person.Efternamn );
						w.WriteLine( "40\tPÅ BILDEN\t1-" + arr.Length.ToString() );
						break;
					}
				}
			w.Close();

			//inf_text
			try
			{
				w = new StreamWriter( m_strAktuellFil = Path.Combine(strBP_ITX,strSOBas+"inf_text.txt") ); 
				w.WriteLine( "10\t" + skola.Namn.ToUpper() + "\t" + skola.Ort.ToUpper() );
				alRader = new ArrayList();		
				foreach ( PlataDM.Person person in skola.Grupper.GruppMedTyp(PlataDM.GruppTyp.GruppInfällning).Personer )
				{
					string s = "\t" + person.Förnamn + "\t" + person.Efternamn;
					if ( !Util.isEmpty(person.Titel) )
						s = person.Titel.ToUpper() + s;
					alRader.Add( s );
				}
				alRader.Sort();
				string strLastGroup = string.Empty;
				foreach ( string s in alRader )
				{
					string[] astr = s.Split('\t');
					if ( astr[0].CompareTo(strLastGroup) != 0 )
					{
						w.WriteLine( "20\t" + astr[0] );
						strLastGroup = astr[0];
					}
					w.WriteLine( "30\t{0}\t{1}", astr[1], astr[2] );
				}
				w.Close();
			}
			catch ( Exception ex )
			{
				Global.storeErrorReport( ex, "Fel vid skapande av infällningstext" );
			}

			//port_text
			alRader = new ArrayList();		
			foreach ( PlataDM.Grupp grupp in skola.Grupper )
			{
				foreach ( PlataDM.Person person in grupp.TotalListaPersoner()  )
					if ( person.Filmrapport!=null && person.Filmrapport.Length!=0 )
					{
						string[] af = person.Filmrapport.Split('_');
						alRader.Add( af[0] + "\t" + af[1] + "\t" + grupp.Namn + "\t" + person.Förnamn+ "\t" + person.Efternamn );
					}
			}
			alRader.Sort();
			w = new StreamWriter( m_strAktuellFil = Path.Combine(strBP_PTX,strSOBas+"port_text.txt") );
			foreach ( string s in alRader )
				w.WriteLine( s );
			w.Close();

			//anmärkning
			try
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				sb.Append( DateTime.Now.ToString() );
				sb.AppendFormat( "\r\nFotograf: {0}\r\n", Global.FotografNummerEndast );
				sb.AppendFormat( "\r\nÖnskas restfoto grupp: {0}", skola.RestfotoGrupp==PlataDM.Answer.Yes ? "JA" : "NEJ" );
				sb.AppendFormat( "\r\nÖnskas restfoto porträtt: {0}", skola.RestfotoPorträtt==PlataDM.Answer.Yes ? "JA" : "NEJ" );
				sb.Append( "\r\n\r\n*** TILL FOTOAVDELNINGEN ***\r\n" );
				sb.Append( skola.Anmärkningar[PlataDM.Anmärkningar.Typ.Foto] );
				sb.Append( "\r\n\r\n*** TILL BOKNINSSERVICE/SÄLJARE ***\r\n" );
				sb.Append( skola.Anmärkningar[PlataDM.Anmärkningar.Typ.BS] );
				sb.Append( "\r\n\r\n*** TILL KATALOG/LABB ***\r\n" );
				sb.Append( skola.Anmärkningar[PlataDM.Anmärkningar.Typ.Katalog] );
				sb.Append( "\r\n\r\n*** ÖVRIGT PRODUKTION ***\r\n" );
				sb.Append( skola.Anmärkningar[PlataDM.Anmärkningar.Typ.Övrigt] );
				string strAnmSpecial = string.Empty;
				skapaPersonSpecialLista( skola.Grupper, ref strAnmSpecial );
				if ( strAnmSpecial.Length!=0 )
				{
					sb.Append( "\r\n\r\n*** SPECIAL ***\r\n" );
					sb.Append( strAnmSpecial );
				}
				w = new StreamWriter( m_strAktuellFil = Path.Combine(strBP_Anm,strSOBas+"anmarkning.txt") );
				w.WriteLine( sb.ToString() );
				w.Flush();
				w.Close();
			}
			catch ( Exception ex )
			{
				Global.storeErrorReport( ex, "Fel vid skapande av anmärkningstext" );
			}

			if ( _nCD==0 )
			{
				//gruppinfo
				int nAntalVimmel, nLovadeVimmel, nValdaVimmel;
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				skola.Vimmel.räkna( out nAntalVimmel, out nLovadeVimmel, out nValdaVimmel );
				w = new StreamWriter( m_strAktuellFil = Path.Combine(_strPath,strSOBas+"gruppinfo.txt") );
				w.WriteLine( "VIMMELBILDER" );
				w.WriteLine( "Totalt:\t{0}", nAntalVimmel );
				w.WriteLine( "Varav lovade:\t{0}", nLovadeVimmel );
				w.WriteLine( "Varav valda:\t{0}", nValdaVimmel );
				w.WriteLine();
				w.WriteLine( "GRUPPBILDER" );
				w.WriteLine( "Ordning\tNamn\tSlogan\tAntal\tFrånv\tUtgått" );
				foreach ( PlataDM.Grupp grupp in skola.Grupper.GrupperIOrdning() )
					if ( !Util.isEmpty(grupp.ThumbnailKey) )
					{
						w.Write( "{0}\t{1}\t{2}\t{3}\t{4}\t{5}", grupp.Ordning, grupp.Namn, grupp.Slogan,
							grupp.Personer.Count, grupp.PersonerFrånvarande.Count, grupp.PersonerSlutat.Count );
						if ( !Util.isEmpty(grupp.ThumbnailGrayKey) )
							w.Write( "\tG" );
						w.WriteLine();
					}
					else
						sb.AppendFormat( "\t{0}\t{1}\r\n", grupp.Namn, grupp.Slogan );
				if ( sb.Length>0 )
				{
					w.WriteLine();
					w.WriteLine( "EJ FOTADE GRUPPER" );
					w.WriteLine( sb );
				}
				w.Close();

				//restfotoport
				w = new StreamWriter( m_strAktuellFil = Path.Combine(_strPath,strSOBas+"restfotoport.txt") );
				foreach ( PlataDM.Grupp grupp in skola.Grupper )
				{
					bool fSkrivGruppnamn = true;
					foreach ( PlataDM.Person person in grupp.TotalListaPersoner() )
						if ( Util.isEmpty(person.Filmrapport) && !person.Personal )
						{
							if ( fSkrivGruppnamn )
							{
								w.WriteLine( grupp.Namn );
								fSkrivGruppnamn = false;
							}
							w.WriteLine( "\t{0}", person.Namn );
						}
				}
				w.Close();

				//restfotogrupp
				w = new StreamWriter( m_strAktuellFil = Path.Combine(_strPath,strSOBas+"restfotogrupp.txt") );
				foreach ( PlataDM.Grupp grupp in skola.Grupper )
					if ( grupp.PersonerFrånvarande.Count>=1 )
					{
						w.WriteLine( grupp.Namn );
						foreach ( PlataDM.Person person in grupp.PersonerFrånvarande )
							w.WriteLine( "\t{0}", person.Namn );
					}
				w.Close();
			}

			//CD-nummer
			string strNummer;
			if ( _nCD!=0 )
				strNummer = "CD nummer " + _nCD;
			else
				strNummer = "SLUTLEVERANS";
			w = new StreamWriter( m_strAktuellFil = Path.Combine(_strPath,strNummer) );
			w.Close();

			//error.log
			if ( File.Exists(m_strAktuellFil = skola.HomePathCombine(Global.Felrapportfil)) )
				myFileCopy( m_strAktuellFil, Path.Combine(_strPath,strSOBas+Global.Felrapportfil) );

			//plapp.log
			if ( File.Exists(m_strAktuellFil = skola.HomePathCombine(Global.PLappLog)) )
				myFileCopy( m_strAktuellFil, Path.Combine(_strPath,strSOBas+Global.PLappLog) );

			//skola
			foreach ( PlataDM.IBrännbar br in m_arrBurnobj )
				br.setCD( m_nCDTyp, _nCD );
			_skola.save( m_strAktuellFil = Path.Combine(_strPath,"plata_" + skola.OrderNr + ".xml") );

			sättSteg( "Filer skapade" );

			frmMain.setStatusProgress( 0, 0 );
		}
*/
		#endregion

		#region Skapa filer till Thiele

		private void skapaFiler_Thiele()
		{
			PlataDM.Skola skola = _skola;
			int nMaxAntalBilder, nBildräknare;
			string strPath1 = Path.Combine( _strPath, "1" );
			string strPath2 = Path.Combine( _strPath, "2" );
			string strPath3 = Path.Combine( _strPath, "3" );
			string strPath4 = Path.Combine( _strPath, "4" );
			ArrayList arrFRSUsed = new ArrayList();

			m_arrBurnobj = new ArrayList();

			nBildräknare = 0;
			nMaxAntalBilder = 0;
			foreach ( PlataDM.Grupp grupp in skola.Grupper )
				nMaxAntalBilder += grupp.PersonerNärvarande.Count + grupp.PersonerFrånvarande.Count + grupp.PersonerSlutat.Count;

			Directory.CreateDirectory( strPath1 );
			Directory.CreateDirectory( strPath2 );
			Directory.CreateDirectory( strPath3 );
			Directory.CreateDirectory( strPath4 );

			sättSteg( "Exporterar porträtt till Thiele" );
			foreach ( PlataDM.Grupp grupp in skola.Grupper )
				foreach ( PlataDM.Person person in grupp.TotalListaPersoner() )
				{
					if ( !Util.isEmpty(person.Scan) )
					{
						PlataDM.Thumbnail tn = person.Thumbnails[person.ThumbnailKey];
						PlataDM.Thumbnail tn2 = person.Thumbnails[person.ThumbnailKey2];
						if ( tn!=null )
						{
							string strDestPath;
							if ( grupp.GruppTyp==PlataDM.GruppTyp.GruppPersonal )
								strDestPath = strPath2;
							else if ( skola.PortraitOrDoubleSerie==PlataDM.PortraitOrDoubleSerie.DoubleSerie )
							{
								strDestPath = strPath3;
								if ( tn2!=null )
									myFileCopy( tn2.FilenameJPG, Path.Combine(strPath4, person.Scan + ".jpg") );
							}
							else
								strDestPath = strPath1;

							myFileCopy( tn.FilenameJPG, Path.Combine(strDestPath, person.Scan + ".jpg") );
						}
					}
					frmMain.setStatusProgress( ++nBildräknare, nMaxAntalBilder );
					Application.DoEvents();
				}

			sättSteg( "Skapar blackboards" );
			createBlackboards( strPath1, 1 );
			createBlackboards( strPath2, 2 );
			createBlackboards( strPath3, 3 );
			createBlackboards( strPath4, 4 );

			frmMain.setStatusProgress( 0, 0 );
		}

		private void createBlackboards( string strPath, int nFolder )
		{
			int nSize = _skola.PackSize;
			string[] astrFN = Directory.GetFiles( strPath );
			if ( astrFN.Length==0 )
				return;

			int nPacks = 1+(astrFN.Length-1)/nSize;
			createBlackboard( Path.Combine(strPath,"0.jpg"), 1, nPacks, nFolder );
			for ( int nIndex=nSize-1, nPack=2 ; nIndex<astrFN.Length ; nIndex+=nSize, nPack++ )
			{
				string strFN = Path.GetFileNameWithoutExtension( astrFN[nIndex] );
				createBlackboard( Path.Combine(strPath,strFN)+"x.jpg", nPack, nPacks, nFolder );
			}
		}

		private void createBlackboard( string strFN, int nPack, int nPacks, int nFolder )
		{
			StringFormat sf = new StringFormat();
			Rectangle r = new Rectangle(0,0,4064,2704);

			using ( Bitmap bmp = new Bitmap(r.Width,r.Height) )
			using ( Graphics g = Graphics.FromImage(bmp) )
			{
				g.FillRectangle( Brushes.Red, r );
				r.Inflate( -(int)(r.Width*0.10), -(int)(r.Height*0.20) );
				using ( Font f = new Font("Arial",256) )
				{
					sf.Alignment = StringAlignment.Near;
					sf.LineAlignment = StringAlignment.Near;
					g.DrawString( "Photomic", f, Brushes.White, r, Util.sfUL );

					sf.Alignment = StringAlignment.Near;
					sf.LineAlignment = StringAlignment.Center;
					g.DrawString( "Schoolno:", f, Brushes.White, r, sf );
					sf.Alignment = StringAlignment.Far;
					g.DrawString( _skola.OrderNr, f, Brushes.White, r, sf );

					sf.Alignment = StringAlignment.Near;
					sf.LineAlignment = StringAlignment.Far;
					g.DrawString( nPack.ToString() + " (" + nPacks.ToString() + ")", f, Brushes.White, r, sf );
					sf.Alignment = StringAlignment.Far;
					g.DrawString( "Folder " + nFolder.ToString(), f, Brushes.White, r, sf );
				}
				bmp.Save( strFN, System.Drawing.Imaging.ImageFormat.Jpeg );
			}
		}

		#endregion

		private void fyllCombo( ComboBox cbo, PlataDM.CDLeveranser lev )
		{
			cbo.Items.Clear();
			cbo.Items.Add( "CD" + (lev.Count+1).ToString() + ": Ny leverans" );
			for ( int i=lev.Count-1 ; i>=0 ; i-- )
				cbo.Items.Add( "Bränn om CD" + (i+1).ToString() + ": " + Global.ISODateTime(lev[i],true) );
			cbo.Items.Add( "---Återställ---" );
			cbo.SelectedIndex = 0;
		}
/*
		public override void skolaUppdaterad()
		{
			_skola = Global.Skola;
			fyllCombo( cboCDP, _skola.CDLeveranserPhotomic );
			fyllCombo( cboCDT, _skola.CDLeveranserThiele );
		}
*/
		public override void activated()
		{
			if ( m_strBurnError==null )
				try 
				{
					m_burn = new XPBurnCD();
					m_burn.BurnerDrive = (string)m_burn.RecorderDrives[0];
					m_strBurnError = "";
				} 
				catch ( Exception ex )
				{
					m_strBurnError = ex.Message;
					m_burn = null;
				}
			setStatusText( m_strBurnError );
		}


		protected override void paint(PaintEventArgs e)
		{
			Util.paintBox( e.Graphics, null, null, 10, 20, this.ClientSize.Width-30, 130 );
			Util.paintBox( e.Graphics, null, null, 10, 170, this.ClientSize.Width-30, this.ClientSize.Height-200 );

			string strDVD = "När jobbet är klart";
			string strCD = "Varje dag";
			using ( Font fnt = new Font( this.Font.FontFamily, 7.5f ) )
			{
				e.Graphics.DrawString( strDVD, fnt, SystemBrushes.WindowText, optPhotomicDVD.Left+15, optPhotomicDVD.Bottom-2 );
				e.Graphics.DrawString( strDVD, fnt, SystemBrushes.WindowText, optThieleDVD.Left+15, optThieleDVD.Bottom-2 );
				e.Graphics.DrawString( strCD, fnt, SystemBrushes.WindowText, optPhotomicCD.Left+15, optPhotomicCD.Bottom-2 );
				e.Graphics.DrawString( strCD, fnt, SystemBrushes.WindowText, optThieleCD.Left+15, optThieleCD.Bottom-2 );
				e.Graphics.DrawString( "Kommande funktion", fnt, SystemBrushes.ControlDark, optWhoIsWho.Left+15, optWhoIsWho.Bottom-2 );
				e.Graphics.DrawString( "Endast provisionsjobb", fnt, SystemBrushes.ControlDark, optProvision.Left+15, optProvision.Bottom-2 );
				if ( cmdBurn.Enabled )
				{
					Rectangle r = new Rectangle( cmdBurn.Left, 0, cmdBurn.Width, cmdBurn.Top-10 );
					StringFormat sf = new StringFormat();
					sf.Alignment = StringAlignment.Center;
					sf.LineAlignment = StringAlignment.Far;
					using ( Font fntBig = new Font( this.Font.FontFamily, 40 ) )
						e.Graphics.DrawString( _strMediaType, fntBig, Brushes.DarkGreen, r, sf );
					if ( _nSizeOfJob!=0 )
						e.Graphics.DrawString( _nSizeOfJob.ToString(), fnt, Brushes.DarkGreen, r, sf );
				}
			}
		}

		protected override void resize( Size sz )
		{
			treeFiles.Bounds = new Rectangle( 20, 180, sz.Width-cmdBurn.Width-60, sz.Height-220 );
			cmdBurn.Location = new Point( treeFiles.Right+10, sz.Height-40-cmdBurn.Height );
			cmdCreate.Location = new Point( cmdBurn.Left, 100-cmdBurn.Height );
		}

		private void addManualFiles( TreeNodeCollection tns )
		{
			foreach ( TreeNode tn in tns )
			{
				//				System.Diagnostics.Debug.Assert( tn.Text.CompareTo("Slask")!=0 );
				addManualFiles( tn.Nodes );
				string[] astr = tn.Tag as string[];
				if ( astr!=null )
					if ( Directory.Exists(astr[0]) )
						addFiles( astr[0], astr[1] );
					else
						m_burn.AddFile( astr[0], astr[1] );
			}
		}

		private void cmdBurn_Click(object sender, System.EventArgs e)
		{
			/*
			Util.SystemPowerStatus sps;

			Util.GetSystemPowerStatus( out sps );
			if ( sps.ACLineStatus == 0 )
				if ( MessageBox.Show( this, "Du bör inte bränna medan du kör på batteri!", Global.AppName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation ) == DialogResult.Cancel )
					return;

			try
			{
				bool fBurnReal = Form.ModifierKeys!=Keys.Shift;
				bool fBurnTrash = Form.ModifierKeys!=Keys.Control;

				if ( !fBurnReal )
					if ( Global.askMsgBox( this, "Vill du endast bränna slask?", false ) != DialogResult.Yes )
						return;
				if ( !fBurnTrash )
					if ( Global.askMsgBox( this, "Vill du hoppa över slask?", false ) != DialogResult.Yes )
						return;

				string strTypeOfDisc = "PTXX".Substring(m_nCDTyp,1);
				if ( _nCD!=0 )
					strTypeOfDisc = strTypeOfDisc.ToLower();
				m_burn.RemoveAllFiles();
				m_burn.VolumeName = string.Format( "{0}_{1}_{2}", _skola.OrderNr, Global.FotografNummerEndast, strTypeOfDisc );
				if ( fBurnReal )
					addFiles( _strPath, "\\" );
				if ( fBurnTrash )
					addManualFiles( treeFiles.Nodes );

				using ( frmBränning dlg = new frmBränning(m_burn) )
					switch ( dlg.ShowDialog() )
					{
						case DialogResult.OK:
						switch ( m_nCDTyp )
						{
							case 0:
								if ( _nCD==0 )
								{
									//bränning av slutleverans OK - sudda backup-images etc
									if ( fBurnReal && fBurnTrash )
									{
										resetDelivery( _skola.CDLeveranserPhotomic );
										treeFiles.Nodes.Clear();
									}
									_strPath = _strPathBase = null;
								}
								else
									if ( (_nCD-1)==_skola.CDLeveranserPhotomic.Count )
									_skola.CDLeveranserPhotomic.Add( DateTime.Now );
								break;
							case 1:
								if ( (_nCD-1)==_skola.CDLeveranserThiele.Count )
									_skola.CDLeveranserThiele.Add( DateTime.Now );
								break;
							case 3:
								Global.killDirectory( _strPath );
								treeFiles.Nodes.Clear();
								_strPath = _strPathBase = null;
								break;
						}
							skolaUppdaterad();
							break;
						case DialogResult.Cancel:
							break;
					}
			}
			catch ( Exception ex )
			{
				Global.storeErrorReport( ex, "Auto: CD-Bränning" );
				Global.showMsgBox( null, "Bränningen misslyckades!\r\n\r\n\t" + ex.Message );
			}
*/
		}

		private void cboCDP_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if ( cboCDP.Focused )
				optPhotomicCD.Checked = true;
		}

		private void cboCDT_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if ( cboCDT.Focused )
				optThieleCD.Checked = true;
		}

		private void treeFiles_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if ( e.KeyCode==Keys.Delete && e.Modifiers==Keys.Shift && treeFiles.SelectedNode!=null )
			{
				TreeNode nod = treeFiles.SelectedNode;
				if ( nod.Parent == null )
					return;
				string strFile = null;
				while ( nod.Parent!=null )
				{
					if ( strFile==null )
						strFile = nod.Text;
					else
						strFile = nod.Text + "\\" + strFile;
					nod = nod.Parent;
				}
				string strFull = _strPath + "\\" + strFile;
				if ( File.Exists(strFull) )
				{
					if ( Global.askMsgBox( this, "Bekräfta att du verkligen vill radera filen \"" + strFile + "\" innan du bränner!", true ) == DialogResult.Yes )
					{
						File.Delete( strFull );
						treeFiles.SelectedNode.Remove();
					}
				}
				else if ( Directory.Exists(strFull) )
				{
					if ( Global.askMsgBox( this, "Bekräfta att du verkligen vill radera mappen \"" + strFile + "\" innan du bränner!", true ) == DialogResult.Yes )
					{
						Directory.Delete( strFull, true );
						treeFiles.SelectedNode.Remove();
					}
				}
			}
		}

		private void treeFiles_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			string[] astr = (string[])e.Data.GetData(DataFormats.FileDrop);
			foreach ( string s in astr )
			{
				long lX = 0;
				TreeNode tn;
				if ( Directory.Exists(s) )
					tn = viewBurnTree( treeFiles.Nodes[0].Nodes, s, 1, ref lX );
				else
					tn = viewBurnTreeNode( treeFiles.Nodes[0].Nodes, s, ref lX );
				tn.Tag = s;
			}
		}

		private void treeFiles_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
		}

		public void burnComplete()
		{
			_skola = Global.Skola;

			if ( FBekräftaGruppordning.showDialog( null, _skola ) != DialogResult.OK )
				return;

			ArrayList alFiles = new ArrayList();
			Global.saveSchool( true );
			foreach ( string s in Directory.GetFiles(Global.Skola.HomePath) )
			{
				string strFN = Path.GetFileName(s);
				BurnFileInfo bfi = new BurnFileInfo( s, null, strFN, false );
				switch ( Path.GetFileName(strFN).ToLower() )
				{
					case "!fotoorder.emf":
						continue;
					case "!skola.xml":
						bfi.OnAll = true;
						break;
				}
				alFiles.Add( bfi );
			}
			theNewAndFunBurn( alFiles, "K", string.Format("{0}_{1}_K", _skola.OrderNr, Global.FotografNummerEndast) );
		}

		public void burnComission()
		{
			_skola = Global.Skola;

			ImageCodecInfo myImageCodecInfo = Util.GetEncoderInfo("image/jpeg");
			EncoderParameters myEncoderParameters = new EncoderParameters(1);
			myEncoderParameters.Param[0] = new EncoderParameter( Encoder.Quality, 88L );

			ArrayList alFiles = new ArrayList();
			Global.saveSchool( true );

			int nBildräknare = 0;
			int nMaxAntalBilder = 0;
			foreach ( PlataDM.Grupp grupp in Global.Skola.Grupper )
				nMaxAntalBilder += grupp.PersonerNärvarande.Count + grupp.PersonerFrånvarande.Count + grupp.PersonerSlutat.Count;

			sättSteg( "Exporterar bilder" );
			foreach ( PlataDM.Grupp grupp in Global.Skola.Grupper )
				if ( grupp.GruppTyp!=PlataDM.GruppTyp.GruppInfällning )
				{
					PlataDM.Thumbnail tnGrupp = grupp.Thumbnails[grupp.ThumbnailKey];
					if ( tnGrupp!=null )
					{
						string s = newBFI( "grupp", grupp.Namn+".jpg", alFiles );
						tnGrupp.saveGroupLo( s, myImageCodecInfo, myEncoderParameters, true );
						nBildräknare++;
					}
					foreach ( PlataDM.Person person in grupp.TotalListaPersoner() )
						if ( !Util.isEmpty(person.Scan) )
						{
							PlataDM.Thumbnail tn = person.Thumbnails[person.ThumbnailKey];
							PlataDM.Thumbnail tn2 = person.Thumbnails[person.ThumbnailKey2];
							if ( tn!=null )
							{
								string s = newBFI( "port", string.Format( "{0} {1}.jpg", person.Scan, person.Namn ), alFiles );
								tn.saveResized( s, 384, 255, Global.PorträttRotateFlipType, true );
							}
							if ( tn2!=null )
							{
								string s = newBFI( "port", string.Format( "{0} {1}_2.jpg", person.Scan, person.Namn ), alFiles );
								tn2.saveResized( s, 384, 255, Global.PorträttRotateFlipType, true );
							}
							frmMain.setStatusProgress( ++nBildräknare, nMaxAntalBilder );
							Application.DoEvents();
						}
				}
			sättSteg( "Filer skapade" );
			frmMain.setStatusProgress( 0, 0 );

			theNewAndFunBurn( alFiles, "A", "ARKIV" );
		}

		private string newBFI( string strPathOnCD, string strNameOnCD, ArrayList al )
		{
			BurnFileInfo bfi = new BurnFileInfo(
				Global.GetTempFileName(),
				strPathOnCD,
				Global.skapaSäkertFilnamn( strNameOnCD ),
				false );
			al.Add( bfi );
			m_strAktuellFil = bfi.CDFullFileName;
			return bfi.LocalFullFileName;
		}

		private void cmdCompleteBurn_Click(object sender, System.EventArgs e)
		{
			burnComplete();
		}

		public static void theNewAndFunBurn(
			ArrayList alFiles,
			string strType,
			string strLabel )
		{
			string strInstructionFile = Global.GetTempFileName();
			string strLogFile = Global.GetTempFileName();

			vdUsr.vdSimpleXMLWriter x = new vdUsr.vdSimpleXMLWriter();
			x.descend( "PLATAXHG" );

			x.descend( "INFO" );
			x.writeValue( "school", Global.Skola.HomePath );
			x.writeValue( "type", strType );
			x.writeValue( "label", strLabel + "_{0}_{1}" );
			x.writeValue( "returnapp", Application.ExecutablePath );
			x.writeValue( "returnarg", string.Format( "\"plataxhg={0}\"", strInstructionFile ) );
			x.writeValue( "resultfile", strLogFile );
			x.writeValue( "windowcaption", string.Empty );
			x.writeValue( "recordcontentsfile", Path.Combine(Global.MainPath,"_burnhistory") );
			x.ascend();

			x.descend( "FILES" );
			foreach ( BurnFileInfo bfi in alFiles )
			{
				FileInfo fi = new FileInfo( bfi.LocalFullFileName );
				long lSize = (fi.Length+1023)/1024;
				if ( (fi.Attributes&FileAttributes.ReadOnly)!=0 )
					File.SetAttributes(bfi.LocalFullFileName, fi.Attributes&~FileAttributes.ReadOnly );
				x.descend( "FILE" );
				x.writeValue( "local", bfi.LocalFullFileName );
				x.writeValue( "oncd", bfi.CDFullFileName );
				x.writeValue( "size", lSize.ToString() );
				if ( bfi.IsTemp )
					x.writeValue( "tmp", "1" );
				if ( bfi.OnAll )
					x.writeValue( "onall", "1" );
				x.ascend();
			}
			x.ascend();

			x.ascend();
			x.endSaveFile( strInstructionFile );

			System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
			info.FileName = Path.Combine( Path.GetDirectoryName(Application.ExecutablePath), "vdStandAloneBurn.exe" );
			info.Arguments = strInstructionFile;
			System.Diagnostics.Process process = System.Diagnostics.Process.Start( info );
			frmMain.theOneForm.endApp( false );
		}

		public class BurnFileInfo : IComparable
		{
			public readonly string LocalFullFileName;
			public readonly string CDPath;
			public readonly string CDName;
			public bool IsTemp;
			public bool OnAll;
			public BurnFileInfo( string LocalPath, string LocalName, string CDPath, string CDName, bool IsTemp ) :
				this( Path.Combine(LocalPath,LocalName), CDPath, CDName, IsTemp )
			{
			}
			public BurnFileInfo( string LocalFullFileName, string CDPath, string CDName, bool IsTemp )
			{
				this.LocalFullFileName = LocalFullFileName;
				if ( Util.isEmpty(CDPath) )
					CDPath = "\\";
				else if ( CDPath[0]!='\\' )
					CDPath = "\\" + CDPath;
				this.CDPath = CDPath;
				this.CDName = CDName;
				this.IsTemp = IsTemp;
			}
			public string CDFullFileName
			{
				get { return Path.Combine( CDPath, CDName ); }
			}
			public int CompareTo(object obj)
			{
				return CDName.CompareTo( ((BurnFileInfo)obj).CDName );
			}

		}

	}

}
