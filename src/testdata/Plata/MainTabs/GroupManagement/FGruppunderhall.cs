using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Photomic.Common;

namespace Plata
{
	/// <summary>
	/// Summary description for frmGruppunderhåll.
	/// </summary>
	public class FGruppunderhåll : vdUsr.baseGradientForm
	{
		private class clsPHelper
		{
			public PlataDM.Grupp _grupp;
			public PlataDM.Person m_person;
			public PlataDM.GruppPersonTyp m_typ;
			public clsPHelper( PlataDM.Grupp grupp, PlataDM.Person person, PlataDM.GruppPersonTyp typ )
			{
				_grupp = grupp;
				m_person = person;
				m_typ = typ;
			}
			public override string ToString()
			{
				return m_person.Namn;
			}

		}

		private TextBox txtGrupp;
		private ListBox lstGrupp1;
		private System.ComponentModel.IContainer components;
		private ListBox lstGrupp2;
		private ComboBox cboGrupper;
		private Button cmdMoveLeft;
		private Button cmdMoveRight;
		private Button cmdOK;
		private ImageList iml;
		private ToolTip toolTip1;
		private Button cmdCopyRight;
		private Button cmdCopyLeft;

		private PlataDM.Skola _skola;
		private System.Windows.Forms.CheckBox chkMakulera;
		private PlataDM.Grupp _grupp;
		private GroupBox groupBox2;
		private CheckBox chkGrupp;
		private CheckBox chkKatalog;
		private CheckBox chkSkyddadID;
		private CheckBox chkPloj;
		private Font _fntBold;

		private FGruppunderhåll( PlataDM.Grupp grupp )
		{
			InitializeComponent();

			_skola = grupp.Skola;

			_grupp = grupp;
            chkGrupp.Checked = (_grupp.Special & TypeOfGroupPhoto.Gruppbild) != TypeOfGroupPhoto.Ingen;
            chkKatalog.Checked = (_grupp.Special & TypeOfGroupPhoto.Katalog) != TypeOfGroupPhoto.Ingen;
            chkPloj.Checked = (_grupp.Special & TypeOfGroupPhoto.Spex) != TypeOfGroupPhoto.Ingen;
            chkSkyddadID.Checked = (_grupp.Special & TypeOfGroupPhoto.SkyddadId) != TypeOfGroupPhoto.Ingen;

			iml.Images.AddStrip( new Bitmap( this.GetType(), "grfx.moveandcopybuttons.bmp" ) );
			cmdMoveLeft.ImageIndex = 0;
			cmdMoveRight.ImageIndex = 1;
			cmdCopyLeft.ImageIndex = 2;
			cmdCopyRight.ImageIndex = 3;

			lstGrupp1.DoubleClick +=new EventHandler(lstGrupp1_DoubleClick);
			lstGrupp2.DoubleClick +=new EventHandler(lstGrupp2_DoubleClick);

			txtGrupp.Enabled = _grupp.GruppTyp == GruppTyp.GruppNormal;
			_fntBold = new Font( this.Font, FontStyle.Bold );
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

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed (e);
			_fntBold.Dispose();
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.txtGrupp = new System.Windows.Forms.TextBox();
			this.lstGrupp1 = new System.Windows.Forms.ListBox();
			this.chkMakulera = new System.Windows.Forms.CheckBox();
			this.lstGrupp2 = new System.Windows.Forms.ListBox();
			this.cboGrupper = new System.Windows.Forms.ComboBox();
			this.cmdMoveLeft = new System.Windows.Forms.Button();
			this.iml = new System.Windows.Forms.ImageList( this.components );
			this.cmdMoveRight = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip( this.components );
			this.cmdCopyRight = new System.Windows.Forms.Button();
			this.cmdCopyLeft = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.chkGrupp = new System.Windows.Forms.CheckBox();
			this.chkKatalog = new System.Windows.Forms.CheckBox();
			this.chkSkyddadID = new System.Windows.Forms.CheckBox();
			this.chkPloj = new System.Windows.Forms.CheckBox();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtGrupp
			// 
			this.txtGrupp.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txtGrupp.Location = new System.Drawing.Point( 12, 16 );
			this.txtGrupp.MaxLength = 50;
			this.txtGrupp.Name = "txtGrupp";
			this.txtGrupp.Size = new System.Drawing.Size( 200, 20 );
			this.txtGrupp.TabIndex = 0;
			// 
			// lstGrupp1
			// 
			this.lstGrupp1.Location = new System.Drawing.Point( 12, 44 );
			this.lstGrupp1.Name = "lstGrupp1";
			this.lstGrupp1.Size = new System.Drawing.Size( 200, 381 );
			this.lstGrupp1.Sorted = true;
			this.lstGrupp1.TabIndex = 1;
			// 
			// chkMakulera
			// 
			this.chkMakulera.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkMakulera.Location = new System.Drawing.Point( 380, 476 );
			this.chkMakulera.Name = "chkMakulera";
			this.chkMakulera.Size = new System.Drawing.Size( 108, 24 );
			this.chkMakulera.TabIndex = 5;
			this.chkMakulera.Text = "Makulera grupp";
			this.chkMakulera.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkMakulera.Click += new System.EventHandler( this.chkMakulera_Click );
			this.chkMakulera.CheckedChanged += new System.EventHandler( this.chkMakulera_CheckedChanged );
			// 
			// lstGrupp2
			// 
			this.lstGrupp2.Location = new System.Drawing.Point( 293, 44 );
			this.lstGrupp2.Name = "lstGrupp2";
			this.lstGrupp2.Size = new System.Drawing.Size( 200, 420 );
			this.lstGrupp2.Sorted = true;
			this.lstGrupp2.TabIndex = 4;
			// 
			// cboGrupper
			// 
			this.cboGrupper.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.cboGrupper.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboGrupper.Location = new System.Drawing.Point( 293, 16 );
			this.cboGrupper.MaxDropDownItems = 20;
			this.cboGrupper.Name = "cboGrupper";
			this.cboGrupper.Size = new System.Drawing.Size( 200, 21 );
			this.cboGrupper.TabIndex = 3;
			this.cboGrupper.DrawItem += new System.Windows.Forms.DrawItemEventHandler( this.cboGrupper_DrawItem );
			this.cboGrupper.SelectedIndexChanged += new System.EventHandler( this.cboGrupper_SelectedIndexChanged );
			// 
			// cmdMoveLeft
			// 
			this.cmdMoveLeft.ImageList = this.iml;
			this.cmdMoveLeft.Location = new System.Drawing.Point( 232, 76 );
			this.cmdMoveLeft.Name = "cmdMoveLeft";
			this.cmdMoveLeft.Size = new System.Drawing.Size( 37, 28 );
			this.cmdMoveLeft.TabIndex = 7;
			this.toolTip1.SetToolTip( this.cmdMoveLeft, "Flytta namn" );
			this.cmdMoveLeft.Click += new System.EventHandler( this.cmdLeft_Click );
			// 
			// iml
			// 
			this.iml.ColorDepth = System.Windows.Forms.ColorDepth.Depth4Bit;
			this.iml.ImageSize = new System.Drawing.Size( 32, 16 );
			this.iml.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// cmdMoveRight
			// 
			this.cmdMoveRight.ImageList = this.iml;
			this.cmdMoveRight.Location = new System.Drawing.Point( 232, 112 );
			this.cmdMoveRight.Name = "cmdMoveRight";
			this.cmdMoveRight.Size = new System.Drawing.Size( 37, 28 );
			this.cmdMoveRight.TabIndex = 8;
			this.toolTip1.SetToolTip( this.cmdMoveRight, "Flytta namn" );
			this.cmdMoveRight.Click += new System.EventHandler( this.cmdRight_Click );
			// 
			// cmdOK
			// 
			this.cmdOK.Location = new System.Drawing.Point( 520, 16 );
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size( 80, 28 );
			this.cmdOK.TabIndex = 6;
			this.cmdOK.Text = "Stäng";
			this.cmdOK.Click += new System.EventHandler( this.cmdOK_Click );
			// 
			// cmdCopyRight
			// 
			this.cmdCopyRight.ImageList = this.iml;
			this.cmdCopyRight.Location = new System.Drawing.Point( 232, 192 );
			this.cmdCopyRight.Name = "cmdCopyRight";
			this.cmdCopyRight.Size = new System.Drawing.Size( 37, 28 );
			this.cmdCopyRight.TabIndex = 10;
			this.toolTip1.SetToolTip( this.cmdCopyRight, "Kopiera namn" );
			this.cmdCopyRight.Click += new System.EventHandler( this.cmdRight_Click );
			// 
			// cmdCopyLeft
			// 
			this.cmdCopyLeft.ImageList = this.iml;
			this.cmdCopyLeft.Location = new System.Drawing.Point( 232, 156 );
			this.cmdCopyLeft.Name = "cmdCopyLeft";
			this.cmdCopyLeft.Size = new System.Drawing.Size( 37, 28 );
			this.cmdCopyLeft.TabIndex = 9;
			this.toolTip1.SetToolTip( this.cmdCopyLeft, "Kopiera namn" );
			this.cmdCopyLeft.Click += new System.EventHandler( this.cmdLeft_Click );
			// 
			// groupBox2
			// 
			this.groupBox2.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.groupBox2.Controls.Add( this.chkGrupp );
			this.groupBox2.Controls.Add( this.chkKatalog );
			this.groupBox2.Controls.Add( this.chkSkyddadID );
			this.groupBox2.Controls.Add( this.chkPloj );
			this.groupBox2.Location = new System.Drawing.Point( 12, 431 );
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size( 200, 65 );
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Bilden ska användas till...";
			// 
			// chkGrupp
			// 
			this.chkGrupp.AutoSize = true;
			this.chkGrupp.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.chkGrupp.Location = new System.Drawing.Point( 6, 19 );
			this.chkGrupp.Name = "chkGrupp";
			this.chkGrupp.Size = new System.Drawing.Size( 71, 17 );
			this.chkGrupp.TabIndex = 0;
			this.chkGrupp.Text = "Gruppbild";
			this.chkGrupp.UseVisualStyleBackColor = false;
			// 
			// chkKatalog
			// 
			this.chkKatalog.AutoSize = true;
			this.chkKatalog.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.chkKatalog.Location = new System.Drawing.Point( 6, 42 );
			this.chkKatalog.Name = "chkKatalog";
			this.chkKatalog.Size = new System.Drawing.Size( 62, 17 );
			this.chkKatalog.TabIndex = 1;
			this.chkKatalog.Text = "Katalog";
			this.chkKatalog.UseVisualStyleBackColor = false;
			// 
			// chkSkyddadID
			// 
			this.chkSkyddadID.AutoSize = true;
			this.chkSkyddadID.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.chkSkyddadID.Location = new System.Drawing.Point( 102, 42 );
			this.chkSkyddadID.Name = "chkSkyddadID";
			this.chkSkyddadID.Size = new System.Drawing.Size( 82, 17 );
			this.chkSkyddadID.TabIndex = 3;
			this.chkSkyddadID.Text = "Skyddad ID";
			this.chkSkyddadID.UseVisualStyleBackColor = false;
			// 
			// chkPloj
			// 
			this.chkPloj.AutoSize = true;
			this.chkPloj.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.chkPloj.Location = new System.Drawing.Point( 102, 19 );
			this.chkPloj.Name = "chkPloj";
			this.chkPloj.Size = new System.Drawing.Size( 43, 17 );
			this.chkPloj.TabIndex = 2;
			this.chkPloj.Text = "Ploj";
			this.chkPloj.UseVisualStyleBackColor = false;
			// 
			// FGruppunderhåll
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.ClientSize = new System.Drawing.Size( 618, 508 );
			this.Controls.Add( this.groupBox2 );
			this.Controls.Add( this.cmdCopyRight );
			this.Controls.Add( this.cmdCopyLeft );
			this.Controls.Add( this.cmdOK );
			this.Controls.Add( this.cmdMoveRight );
			this.Controls.Add( this.cmdMoveLeft );
			this.Controls.Add( this.cboGrupper );
			this.Controls.Add( this.chkMakulera );
			this.Controls.Add( this.lstGrupp2 );
			this.Controls.Add( this.lstGrupp1 );
			this.Controls.Add( this.txtGrupp );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FGruppunderhåll";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Gruppunderhåll";
			this.groupBox2.ResumeLayout( false );
			this.groupBox2.PerformLayout();
			this.ResumeLayout( false );
			this.PerformLayout();

		}
		#endregion

		private void visaPersoner2( ListBox lst, PlataDM.Grupp grupp, PlataDM.GruppPersonTyp typ, Hashtable valda )
		{
			foreach( PlataDM.Person person in grupp.PersonerVal(typ) )
			{
				int nIndex = lst.Items.Add( new clsPHelper( grupp, person, typ ) );
				if ( valda!=null && valda.ContainsKey(person) )
					lst.SetSelected( nIndex, true );
			}
		}

		private void visaPersoner( ListBox lst, PlataDM.Grupp grupp, Hashtable valda )
		{
			lst.Items.Clear();
			visaPersoner2( lst, grupp, PlataDM.GruppPersonTyp.PersonNormal, valda );
			visaPersoner2( lst, grupp, PlataDM.GruppPersonTyp.PersonFrånvarande, valda  );
			visaPersoner2( lst, grupp, PlataDM.GruppPersonTyp.PersonSlutat, valda  );
			if ( lst!=lstGrupp1 )
			{
				chkMakulera.Enabled = grupp.GruppTyp==GruppTyp.GruppNormal;
				chkMakulera.Checked = grupp.Makulerad;
				chkMakulera.BackColor = grupp.Makulerad ? SystemColors.Window : SystemColors.Control;
			}
		}

		private void visaAllt( bool fGrupperÄndrade, Hashtable valda )
		{
			if ( fGrupperÄndrade )
			{
				txtGrupp.Text = _grupp.Namn;
				ArrayList al = new ArrayList( _skola.Grupper );
				al.AddRange( _skola.MakuleradeGrupper );
				al.Remove( _grupp );
				for ( int i = al.Count - 1 ; i >= 0 ; i-- )
					if ( (al[i] as PlataDM.Grupp).isAggregate )
						al.RemoveAt( i );
				al.Sort();
				cboGrupper.Items.Clear();
				cboGrupper.Items.AddRange( al.ToArray() );

				if ( cboGrupper.Items.Count>=1 )
					cboGrupper.SelectedIndex = 0;
			}
			else
				visaPersoner( lstGrupp2, (PlataDM.Grupp)cboGrupper.SelectedItem, valda );
			visaPersoner( lstGrupp1, _grupp, valda );
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			visaAllt( true, null );
		}

		private void cboGrupper_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			PlataDM.Grupp gruppVald = (PlataDM.Grupp)cboGrupper.SelectedItem;
			visaPersoner( lstGrupp2, gruppVald, null );
			bool fOddball = _grupp.GruppTyp!=GruppTyp.GruppNormal || gruppVald.GruppTyp!=GruppTyp.GruppNormal;
			cmdMoveLeft.Enabled = !fOddball;
			cmdMoveRight.Enabled = !fOddball;
		}

		private void flyttaEllerKopieraPerson( PlataDM.Grupp gruppDest, clsPHelper ph, bool fKopiera, Hashtable valda )
		{
			if ( fKopiera )
			{
				PlataDM.Person person = gruppDest.PersonerNärvarande.Add( ph.m_person.Personal, ph.m_person.getInfos() );
				valda.Add( person, null );
			}
			else
			{
				ph._grupp.PersonerVal(ph.m_typ).Remove( ph.m_person );
				gruppDest.PersonerVal(ph.m_typ).Add( ph.m_person );
			}
			valda.Add( ph.m_person, null );
		}

		private void flyttaEllerKopieraVänster( bool fKopiera )
		{
			Hashtable valda = new Hashtable();
			foreach ( clsPHelper ph in lstGrupp2.SelectedItems )
				flyttaEllerKopieraPerson( _grupp, ph, fKopiera, valda );
			visaAllt( false, valda );
			if ( fKopiera )
				for ( int i=0 ; i<lstGrupp2.Items.Count ; i++ )
					lstGrupp2.SetSelected( i, false );
		}

		private void flyttaEllerKopieraHöger( bool fKopiera )
		{
			Hashtable valda = new Hashtable();
			if ( cboGrupper.SelectedItem!=null )
				foreach ( clsPHelper ph in lstGrupp1.SelectedItems )
					flyttaEllerKopieraPerson( (PlataDM.Grupp)cboGrupper.SelectedItem, ph, fKopiera, valda );
			visaAllt( false, valda );
			if ( fKopiera )
				for ( int i=0 ; i<lstGrupp1.Items.Count ; i++ )
					lstGrupp1.SetSelected( i, false );
		}

		private void cmdLeft_Click(object sender, System.EventArgs e)
		{
			flyttaEllerKopieraVänster( sender==cmdCopyLeft );
		}

		private void cmdRight_Click(object sender, EventArgs e)
		{
			flyttaEllerKopieraHöger( sender==cmdCopyRight );
		}

		private bool checkOkToDeface( PlataDM.Grupp grupp )
		{
			if ( grupp.isAggregated )
			{
				Global.showMsgBox( this, "Du får inte makulera en grupp som är sammanslagen med annan grupp!" );
				return false;
			}

			bool fHasGroupPhoto = !string.IsNullOrEmpty( grupp.ThumbnailKey );
			bool fHasPortrait = false;
			foreach ( PlataDM.Person pers in grupp.AllaPersoner )
				if ( pers.HasPhoto )
				{
					fHasPortrait = true;
					break;
				}

			string strQ;
			switch ( (fHasGroupPhoto?2:0) + (fHasPortrait?1:0) )
			{
				case 1:
					strQ = "porträtt";
					break;
				case 2:
					strQ = "gruppbild";
					break;
				case 3:
					strQ = "gruppbild och porträtt";
					break;
				default:
					return true;
			}

			strQ = string.Format( "Denna grupp har {0}. Är du säker på att du vill makulera den ändå?", strQ );
			return Global.askMsgBox( this, strQ, true ) == DialogResult.Yes;
		}

		private void chkMakulera_Click(object sender, System.EventArgs e)
		{
			PlataDM.Grupp grupp = (PlataDM.Grupp)cboGrupper.SelectedItem;

			if ( chkMakulera.Checked )
				if ( !checkOkToDeface(grupp) )
				{
					chkMakulera.Checked = false;
					return;
				}

			grupp.Makulerad = chkMakulera.Checked;
			visaAllt( true, null );
		}

		private bool spara()
		{
			if ( _grupp==null || !txtGrupp.Enabled )
				return true;

			string strNyttNamn = txtGrupp.Text.Trim();
			if ( strNyttNamn.Length<1 )
			{
				Global.showMsgBox( this, "Du kan inte ha ett tomt gruppnamn!" );
				return false;
			}

			foreach ( PlataDM.Grupp grupp in _skola.Grupper )
				if ( grupp!=_grupp && string.Compare(strNyttNamn,grupp.Namn)==0 )
				{
					Global.showMsgBox( this, "Det finns redan en grupp med detta namn. Välj ett annan namn!" );
					return false;
				}

			PlataDM.Grupp grpInf = _skola.Grupper.GruppMedTyp( GruppTyp.GruppInfällning );
			if ( grpInf!=null && grpInf!=_grupp )
				foreach ( PlataDM.Person pers in grpInf.AllaPersoner )
					if ( string.Compare(pers.Titel,_grupp.Namn)==0 )
						pers.Titel = strNyttNamn;

			_grupp.Namn = strNyttNamn;

            var gs = TypeOfGroupPhoto.Ingen;
			if ( chkGrupp.Checked )
                gs |= TypeOfGroupPhoto.Gruppbild;
			if ( chkKatalog.Checked )
                gs |= TypeOfGroupPhoto.Katalog;
			if ( chkPloj.Checked )
                gs |= TypeOfGroupPhoto.Spex;
			if ( chkSkyddadID.Checked )
                gs |= TypeOfGroupPhoto.SkyddadId;
			_grupp.Special = gs;

			return true;
		}

		private void lstGrupp1_DoubleClick(object sender, EventArgs e)
		{
			flyttaEllerKopieraHöger( !cmdMoveRight.Enabled );
		}

		private void lstGrupp2_DoubleClick(object sender, EventArgs e)
		{
			flyttaEllerKopieraVänster( !cmdMoveLeft.Enabled );
		}

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			if ( spara() )
				this.DialogResult = DialogResult.OK;
		}

		public static void showDialog( Form parent, PlataDM.Grupp grupp )
		{
			using ( FGruppunderhåll dlg = new FGruppunderhåll(grupp) )
				dlg.ShowDialog(parent);
		}

		private void cboGrupper_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			Util.paintComboBoxGroup( (ComboBox)sender, e );
		}

		private void chkMakulera_CheckedChanged(object sender, System.EventArgs e)
		{
			chkMakulera.BackColor = chkMakulera.Checked ? SystemColors.Window : SystemColors.Control;
			chkMakulera.Font = chkMakulera.Checked ? _fntBold : this.Font;
			chkMakulera.Text = chkMakulera.Checked ? "Återställ grupp" : "Makulera grupp";
		}

	}

}
