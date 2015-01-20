using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Photomic.Common;
using Photomic.Common.Img;
using Plata.Camera;
using PlataDM;

namespace Plata
{
	public class baseGruppForm : Plata.baseFlikForm
	{
		protected const int vänsterbredd = 292;
		protected const int vänstermarginal = 8;

		protected System.Windows.Forms.ComboBox cboGrupp;
		protected System.Windows.Forms.ListView lv;
		protected System.Windows.Forms.ContextMenu mnuGrupp;
		protected System.Windows.Forms.MenuItem mnuGruppUnderhåll;
		protected System.Windows.Forms.ContextMenu mnuPerson;
		protected System.Windows.Forms.MenuItem mnuPersonLäggTill;
		protected System.Windows.Forms.MenuItem mnuPersonNamnge;
		protected System.Windows.Forms.MenuItem mnuPersonRadera;
		private System.ComponentModel.IContainer components = null;
		protected System.Windows.Forms.PictureBox picFoto;

		protected bool _fAvoidRecursionLV = false;
		protected bool _fAvoidRecursionLV2 = false;

		protected PlataDM.Grupp _grupp;
		protected PlataDM.Person _person;

		protected System.Windows.Forms.PictureBox picNames;
		protected System.Windows.Forms.Timer tmrDelayedResize;
		protected Image _imgFull;
		protected int _nNamnskyltteckenhöjd;
        protected System.Windows.Forms.HScrollBar scrThumb;
		protected System.Windows.Forms.TextBox txtSlogan;

		private string _selectedThumbnailkey;
        private List<string> _selectedThumbnailkeys = new List<string>();  
		protected System.Windows.Forms.ColumnHeader chFörnamn;
		protected System.Windows.Forms.ColumnHeader chEfternamn;
		protected System.Windows.Forms.Label lblAntalIKlass;
		private System.Windows.Forms.Timer tmrAuto;
		protected System.Windows.Forms.ContextMenu mnuThumb;
		protected System.Windows.Forms.MenuItem mnuThumbRadera;
		protected string _strThumbnailkeyRightClicked;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem mnuLäggTillGrupp;
		private System.Windows.Forms.MenuItem mnuSammanslagnaGrupper;
        private MenuItem menuItem2;
        protected Button cmdManageGroups;
        protected TextBox txtGratisEx;
		private MenuItem mnuKopieraGrupp;

		protected baseGruppForm() : base()
		{
			InitializeComponent();
		}

		public baseGruppForm( Form parent, FlikTyp fliktyp ) : base( parent, fliktyp )
		{
			InitializeComponent();

			lv.MouseUp += new MouseEventHandler(lv_MouseUp);
			lv.SelectedIndexChanged += new EventHandler(lv_SelectedIndexChanged);
			lv.ColumnClick +=new ColumnClickEventHandler(lv_ColumnClick);
			scrThumb.Scroll += new ScrollEventHandler(scrThumb_Scroll);

		    var lviComp = new Util.ListViewItemComparer {ColumnIndex = Global.Preferences.SortOrderLastName ? 0 : 1};
		    lv.ListViewItemSorter = lviComp;
			lv.KeyPress +=lv_KeyPress;
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
            this.cboGrupp = new System.Windows.Forms.ComboBox();
            this.lv = new System.Windows.Forms.ListView();
            this.chEfternamn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chFörnamn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mnuGrupp = new System.Windows.Forms.ContextMenu();
            this.mnuGruppUnderhåll = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mnuLäggTillGrupp = new System.Windows.Forms.MenuItem();
            this.mnuKopieraGrupp = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.mnuSammanslagnaGrupper = new System.Windows.Forms.MenuItem();
            this.mnuPerson = new System.Windows.Forms.ContextMenu();
            this.mnuPersonLäggTill = new System.Windows.Forms.MenuItem();
            this.mnuPersonNamnge = new System.Windows.Forms.MenuItem();
            this.mnuPersonRadera = new System.Windows.Forms.MenuItem();
            this.picFoto = new System.Windows.Forms.PictureBox();
            this.picNames = new System.Windows.Forms.PictureBox();
            this.tmrDelayedResize = new System.Windows.Forms.Timer(this.components);
            this.scrThumb = new System.Windows.Forms.HScrollBar();
            this.txtSlogan = new System.Windows.Forms.TextBox();
            this.lblAntalIKlass = new System.Windows.Forms.Label();
            this.tmrAuto = new System.Windows.Forms.Timer(this.components);
            this.mnuThumb = new System.Windows.Forms.ContextMenu();
            this.mnuThumbRadera = new System.Windows.Forms.MenuItem();
            this.cmdManageGroups = new System.Windows.Forms.Button();
            this.txtGratisEx = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picFoto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNames)).BeginInit();
            this.SuspendLayout();
            // 
            // cboGrupp
            // 
            this.cboGrupp.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboGrupp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGrupp.Location = new System.Drawing.Point(8, 8);
            this.cboGrupp.MaxDropDownItems = 20;
            this.cboGrupp.Name = "cboGrupp";
            this.cboGrupp.Size = new System.Drawing.Size(215, 22);
            this.cboGrupp.TabIndex = 3;
            this.toolTip1.SetToolTip(this.cboGrupp, "Grupper");
            this.cboGrupp.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cboGrupp_DrawItem);
            this.cboGrupp.SelectedIndexChanged += new System.EventHandler(this.cboGrupp_SelectedIndexChanged);
            this.cboGrupp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cboGrupp_MouseDown);
            // 
            // lv
            // 
            this.lv.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chEfternamn,
            this.chFörnamn});
            this.lv.FullRowSelect = true;
            this.lv.HideSelection = false;
            this.lv.LabelEdit = true;
            this.lv.Location = new System.Drawing.Point(8, 64);
            this.lv.MultiSelect = false;
            this.lv.Name = "lv";
            this.lv.Size = new System.Drawing.Size(284, 496);
            this.lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lv.TabIndex = 2;
            this.lv.UseCompatibleStateImageBehavior = false;
            this.lv.View = System.Windows.Forms.View.Details;
            // 
            // chEfternamn
            // 
            this.chEfternamn.Text = "Efternamn";
            this.chEfternamn.Width = 80;
            // 
            // chFörnamn
            // 
            this.chFörnamn.Text = "Förnamn";
            this.chFörnamn.Width = 80;
            // 
            // mnuGrupp
            // 
            this.mnuGrupp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuGruppUnderhåll,
            this.menuItem1,
            this.mnuLäggTillGrupp,
            this.mnuKopieraGrupp,
            this.menuItem2,
            this.mnuSammanslagnaGrupper});
            // 
            // mnuGruppUnderhåll
            // 
            this.mnuGruppUnderhåll.Index = 0;
            this.mnuGruppUnderhåll.Text = "Underhåll...";
            this.mnuGruppUnderhåll.Click += new System.EventHandler(this.mnuGruppUnderhåll_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.Text = "-";
            // 
            // mnuLäggTillGrupp
            // 
            this.mnuLäggTillGrupp.Index = 2;
            this.mnuLäggTillGrupp.Text = "Lägg till ny grupp...";
            this.mnuLäggTillGrupp.Click += new System.EventHandler(this.mnuLäggTillGrupp_Click);
            // 
            // mnuKopieraGrupp
            // 
            this.mnuKopieraGrupp.Index = 3;
            this.mnuKopieraGrupp.Text = "Kopiera denna grupp...";
            this.mnuKopieraGrupp.Click += new System.EventHandler(this.mnuKopieraGrupp_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 4;
            this.menuItem2.Text = "-";
            // 
            // mnuSammanslagnaGrupper
            // 
            this.mnuSammanslagnaGrupper.Index = 5;
            this.mnuSammanslagnaGrupper.Text = "Sammanslagna grupper";
            this.mnuSammanslagnaGrupper.Click += new System.EventHandler(this.mnuSammanslagnaGrupper_Click);
            // 
            // mnuPerson
            // 
            this.mnuPerson.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuPersonLäggTill,
            this.mnuPersonNamnge,
            this.mnuPersonRadera});
            // 
            // mnuPersonLäggTill
            // 
            this.mnuPersonLäggTill.Index = 0;
            this.mnuPersonLäggTill.Shortcut = System.Windows.Forms.Shortcut.Ins;
            this.mnuPersonLäggTill.Text = "Lägg till person";
            this.mnuPersonLäggTill.Click += new System.EventHandler(this.mnuPersonLäggTill_Click);
            // 
            // mnuPersonNamnge
            // 
            this.mnuPersonNamnge.Index = 1;
            this.mnuPersonNamnge.Shortcut = System.Windows.Forms.Shortcut.F2;
            this.mnuPersonNamnge.Text = "Ändra namn";
            this.mnuPersonNamnge.Click += new System.EventHandler(this.mnuPersonNamnge_Click);
            // 
            // mnuPersonRadera
            // 
            this.mnuPersonRadera.Index = 2;
            this.mnuPersonRadera.Text = "Radera person";
            this.mnuPersonRadera.Click += new System.EventHandler(this.mnuPersonRadera_Click);
            // 
            // picFoto
            // 
            this.picFoto.BackColor = System.Drawing.Color.Black;
            this.picFoto.Cursor = System.Windows.Forms.Cursors.Default;
            this.picFoto.Location = new System.Drawing.Point(304, 60);
            this.picFoto.Name = "picFoto";
            this.picFoto.Size = new System.Drawing.Size(488, 316);
            this.picFoto.TabIndex = 5;
            this.picFoto.TabStop = false;
            // 
            // picNames
            // 
            this.picNames.BackColor = System.Drawing.SystemColors.Window;
            this.picNames.Location = new System.Drawing.Point(296, 408);
            this.picNames.Name = "picNames";
            this.picNames.Size = new System.Drawing.Size(560, 36);
            this.picNames.TabIndex = 6;
            this.picNames.TabStop = false;
            // 
            // tmrDelayedResize
            // 
            this.tmrDelayedResize.Tick += new System.EventHandler(this.tmrDelayedResize_Tick);
            // 
            // scrThumb
            // 
            this.scrThumb.LargeChange = 1;
            this.scrThumb.Location = new System.Drawing.Point(848, 544);
            this.scrThumb.Maximum = 1000;
            this.scrThumb.Name = "scrThumb";
            this.scrThumb.Size = new System.Drawing.Size(34, 17);
            this.scrThumb.TabIndex = 7;
            // 
            // txtSlogan
            // 
            this.txtSlogan.Location = new System.Drawing.Point(8, 36);
            this.txtSlogan.Name = "txtSlogan";
            this.txtSlogan.Size = new System.Drawing.Size(196, 21);
            this.txtSlogan.TabIndex = 9;
            this.txtSlogan.Visible = false;
            // 
            // lblAntalIKlass
            // 
            this.lblAntalIKlass.BackColor = System.Drawing.SystemColors.Window;
            this.lblAntalIKlass.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblAntalIKlass.Location = new System.Drawing.Point(260, 8);
            this.lblAntalIKlass.Name = "lblAntalIKlass";
            this.lblAntalIKlass.Size = new System.Drawing.Size(32, 21);
            this.lblAntalIKlass.TabIndex = 10;
            this.lblAntalIKlass.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.lblAntalIKlass, "Antal personer i gruppen");
            // 
            // tmrAuto
            // 
            this.tmrAuto.Interval = 8000;
            this.tmrAuto.Tick += new System.EventHandler(this.tmrAuto_Tick);
            // 
            // mnuThumb
            // 
            this.mnuThumb.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuThumbRadera});
            // 
            // mnuThumbRadera
            // 
            this.mnuThumbRadera.Index = 0;
            this.mnuThumbRadera.Text = "Radera bilden";
            this.mnuThumbRadera.Click += new System.EventHandler(this.mnuThumbRadera_Click);
            // 
            // cmdManageGroups
            // 
            this.cmdManageGroups.Location = new System.Drawing.Point(229, 9);
            this.cmdManageGroups.Name = "cmdManageGroups";
            this.cmdManageGroups.Size = new System.Drawing.Size(25, 21);
            this.cmdManageGroups.TabIndex = 11;
            this.cmdManageGroups.Text = "&U>";
            this.toolTip1.SetToolTip(this.cmdManageGroups, "Gruppunderhåll (alt-U)");
            this.cmdManageGroups.UseVisualStyleBackColor = true;
            this.cmdManageGroups.Click += new System.EventHandler(this.cmdManageGroups_Click);
            // 
            // txtGratisEx
            // 
            this.txtGratisEx.Location = new System.Drawing.Point(210, 35);
            this.txtGratisEx.MaxLength = 2;
            this.txtGratisEx.Name = "txtGratisEx";
            this.txtGratisEx.Size = new System.Drawing.Size(46, 21);
            this.txtGratisEx.TabIndex = 13;
            this.toolTip1.SetToolTip(this.txtGratisEx, "Gratisex");
            this.txtGratisEx.Visible = false;
            this.txtGratisEx.TextChanged += new System.EventHandler(this.txtGratisEx_TextChanged);
            this.txtGratisEx.Enter += new System.EventHandler(this.txtGratisEx_Enter);
            // 
            // baseGruppForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(892, 566);
            this.Controls.Add(this.txtGratisEx);
            this.Controls.Add(this.cmdManageGroups);
            this.Controls.Add(this.lblAntalIKlass);
            this.Controls.Add(this.txtSlogan);
            this.Controls.Add(this.scrThumb);
            this.Controls.Add(this.picNames);
            this.Controls.Add(this.picFoto);
            this.Controls.Add(this.cboGrupp);
            this.Controls.Add(this.lv);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "baseGruppForm";
            ((System.ComponentModel.ISupportInitialize)(this.picFoto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNames)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		protected virtual void nyGruppVald()
		{
		}

		private void cboGrupp_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		    this.Cursor = Cursors.WaitCursor;
		    _grupp = cboGrupp.SelectedItem as Grupp;
		    if (_grupp != null && cboGrupp.Items.Contains(""))
		        cboGrupp.Items.Remove("");
		    nyGruppVald();
		    updateLV(null);
		    if (lv.Items.Count > 0)
		        selectPerson(lv.Items[0].Tag as Person);
		    else
		        selectPerson(null);
		    this.Cursor = Cursors.Default;
		}

        protected virtual void updateLV(Person personVald)
        {
            if (_fAvoidRecursionLV2 || _grupp == null)
                return;
            _fAvoidRecursionLV2 = true;

            if (personVald == null && lv.SelectedItems.Count == 1)
                personVald = (Person) lv.SelectedItems[0].Tag;
            var scrollTop = lv.TopItem != null ? lv.TopItem.Index : 0;
            ListViewItem lviSelect = null;
            lv.Items.Clear();
            lv.BeginUpdate();
            läggPersonerTillLV(_grupp.PersonerNärvarande, personVald, "-", ref lviSelect);
            läggPersonerTillLV(_grupp.PersonerSlutat, personVald, "U", ref lviSelect);
            läggPersonerTillLV(_grupp.PersonerFrånvarande, personVald, "F", ref lviSelect);
            lv.EndUpdate();
            if (scrollTop < lv.Items.Count)
                lv.EnsureVisible(scrollTop);
            if (lviSelect != null)
            {
                lviSelect.Selected = true;
                lv.EnsureVisible(lviSelect.Index);
            }
            lv.Refresh();

            lblAntalIKlass.Text = lv.Items.Count.ToString();
            _fAvoidRecursionLV2 = false;
        }

	    private void läggPersonerTillLV(
            Personer personer,
            Person personVald,
            string strEtikett,
            ref ListViewItem lviSelect)
        {
            foreach (var person in personer)
            {
                var itm = läggPersonTillLV(person, strEtikett);
                if (person == personVald)
                    lviSelect = itm;
            }
        }

	    protected virtual void grupplistaÄndrad()
		{
		}

		private void läggTillPerson()
		{
			if ( _grupp==null )
				return;
			Person person = frmPersonnamn.läggTill( this, _grupp, FlikTyp!=FlikTyp.Infällning?null:Global.Skola.Grupper );
			if ( person==null )
				return;
			updateLV( person );
			grupplistaÄndrad();

			if ( !person.Personal || person.Grupp.GruppTyp==GruppTyp.GruppPersonal )
				return;

			var gruppPersonal = _grupp.Skola.Grupper.GruppMedTyp(GruppTyp.GruppPersonal);
			foreach ( PlataDM.Person pers in gruppPersonal.AllaPersoner )
				if ( string.Compare( pers.Namn, person.Namn, true ) == 0 )
					return;

			var personer =	string.IsNullOrEmpty(gruppPersonal.ThumbnailKey) ? 
				gruppPersonal.PersonerNärvarande : gruppPersonal.PersonerFrånvarande;
			personer.Add( false, person.Förnamn, person.Efternamn, person.Titel );
		}

		private void mnuPersonLäggTill_Click(object sender, System.EventArgs e)
		{
			läggTillPerson();
		}

		private void mnuPersonRadera_Click(object sender, System.EventArgs e)
		{
			if ( lv.SelectedItems.Count!=1 )
				return;

			var person = (PlataDM.Person)lv.SelectedItems[0].Tag;
			if ( person.Siffra!=null )
			{
				Global.showMsgBox( this, "Personen är numrerad på gruppbildsfliken. Du måste ta bort numreringen innan personen kan raderas!" );
				return;
			}
			if ( person.Thumbnails.Count!=0 )
			{
				Global.showMsgBox( this, "Personen har bilder. Radera eller flytta dem till en annan person först!" );
				return;
			}

//TODO radera bilderna?
			_grupp.raderaPerson( person );
			_person = null;
			updateLV(null);
			grupplistaÄndrad();
		}

		private void namngePerson()
		{
			if ( lv.SelectedItems.Count!=1 )
				return;

			var person = (PlataDM.Person)lv.SelectedItems[0].Tag;
			if ( frmPersonnamn.redigera( this, person, FlikTyp != FlikTyp.Infällning ? null : Global.Skola.Grupper ) == DialogResult.OK )
			{
				updateLV( null );
				this.Invalidate();
			}
		}

		private void mnuPersonNamnge_Click(object sender, System.EventArgs e)
		{
			namngePerson();
		}

		private void mnuGruppUnderhåll_Click(object sender, System.EventArgs e)
		{
			FGruppunderhåll.showDialog( this, _grupp);
			frmMain.rapportera_skolaUppdaterad();
		}

		private void mnuLäggTillGrupp_Click(object sender, System.EventArgs e)
		{
			if ( GroupManagement.FNewOrRenameGroup.showDialog_New( this, out _grupp ) == DialogResult.OK )
				frmMain.rapportera_skolaUppdaterad();
		}

		private void cboGrupp_MouseDown( object sender, MouseEventArgs e )
		{
			if ( e.Button != MouseButtons.Right || Form.ModifierKeys != Keys.Control )
				return;
			bool fHasGroup = _grupp != null;
			switch ( FlikTyp )
			{
				case FlikTyp.GruppbildInne:
				case FlikTyp.GruppbildUte:
				case FlikTyp.PorträttInne:
				case FlikTyp.PorträttUte:
					mnuLäggTillGrupp.Enabled = true;
					break;
				default:
					mnuLäggTillGrupp.Enabled = false;
					break;
			}
			mnuGruppUnderhåll.Enabled = fHasGroup;
			mnuKopieraGrupp.Enabled = fHasGroup && !(_grupp.isAggregate || _grupp.isAggregated);
			mnuGrupp.Show( cboGrupp, new Point( e.X, e.Y ) );
		}

		private void lv_MouseUp(object sender, MouseEventArgs e)
		{
			if ( _grupp == null || e.Button != MouseButtons.Right )
				return;

			var fHarPerson = _person!=null;
			mnuPersonRadera.Enabled = fHarPerson;
			mnuPersonNamnge.Enabled = fHarPerson;
			mnuPerson.Show( lv, new Point(e.X,e.Y) );
		}

		private static void moveCboIndex( ComboBox cbo, int nDirection )
		{
			if ( cbo.Items.Count==0 )
				return;
			var nNewIndex = cbo.SelectedIndex + nDirection;
			if ( nNewIndex>=0 && nNewIndex<cbo.Items.Count )
				cbo.SelectedIndex = nNewIndex;
		}

		private void moveListViewIndex( int nDirection )
		{
			if ( lv.Items.Count==0 )
				return;
			if ( lv.SelectedItems.Count==0 )
				lv.Items[0].Selected = true;
			else
			{
				var nNewIndex = lv.SelectedItems[0].Index + nDirection;
				if ( nNewIndex >= 0 && nNewIndex < lv.Items.Count )
				{
				    _fAvoidRecursionLV = true;
					lv.Items[nNewIndex].Selected = true;
					lv.Items[nNewIndex].EnsureVisible();
				    _fAvoidRecursionLV = false;
                    if ( !(this is frmGruppbild))
                        personVald(true);
				}
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
		    base.OnKeyDown(e);
		    switch (e.KeyCode)
		    {
		        case Keys.Up:
		            switch (e.Modifiers)
		            {
		                case Keys.None:
		                    moveListViewIndex(-1);
		                    e.Handled = true;
		                    return;
		                case Keys.Control:
		                    moveCboIndex(cboGrupp, -1);
		                    e.Handled = true;
		                    return;
		            }
		            break;
		        case Keys.Down:
		            switch (e.Modifiers)
		            {
		                case Keys.None:
		                    moveListViewIndex(1);
		                    e.Handled = true;
		                    return;
		                case Keys.Control:
		                    moveCboIndex(cboGrupp, 1);
		                    e.Handled = true;
		                    return;
		            }
		            break;
		        case Keys.Left:
                    if (Photomic.Common.Util.findFocusedControl(this) is TextBox)
                        return;
		            switch (e.Modifiers)
		            {
		                case Keys.None:
		                    if (getThumbnails() == null)
		                        return;
		                    getThumbnails().moveKeyboardFocus(-1);
		                    var tn = getThumbnails().KeyboardFocus;
		                    if (tn != null)
		                        thumbnailClicked(tn, false);
		                    e.Handled = true;
		                    return;
		            }
		            break;
		        case Keys.Right:
                    if (Photomic.Common.Util.findFocusedControl(this) is TextBox)
                        return;
		            switch (e.Modifiers)
		            {
		                case Keys.None:
		                    if (getThumbnails() == null)
		                        return;
		                    getThumbnails().moveKeyboardFocus(1);
		                    var tn = getThumbnails().KeyboardFocus;
		                    if (tn != null)
		                        thumbnailClicked(tn, false);
		                    e.Handled = true;
		                    return;
		            }
		            break;
		        case Keys.Insert:
		            switch (e.Modifiers)
		            {
		                case Keys.None:
		                    läggTillPerson();
		                    return;
		            }
		            break;
		        case Keys.F2:
		            switch (e.Modifiers)
		            {
		                case Keys.None:
		                    namngePerson();
		                    return;
		                case Keys.Control:
		                    if (lv.SelectedItems.Count == 1)
		                    {
		                        var person = (PlataDM.Person) lv.SelectedItems[0].Tag;
		                        var strTemp = person.Förnamn;
		                        person.Förnamn = person.Efternamn;
		                        person.Efternamn = strTemp;
		                        updateLV(null);
		                    }
		                    return;
		            }
		            break;
		        case Keys.F5:
		            switch (e.Modifiers)
		            {
		                case Keys.None:
                            visaFullskärm(SelectedThumbnailKey);
		                    return;
		                case Keys.Control:
		                    {
		                        if (getThumbnails() == null)
		                            return;
                                var tn = getThumbnails()[SelectedThumbnailKey];
		                        if (tn != null)
		                            using (var dlg = new FKollaSkarpa(tn))
		                                dlg.ShowDialog(this);
		                        return;
		                    }
		            }
		            break;
		        case Keys.F12:
		            if (e.Modifiers == Keys.Control)
		                tmrAuto.Enabled = !tmrAuto.Enabled;
		            break;
		    }
		}

	    protected override void resize( Size sz )
		{
			var sh = SystemInformation.HorizontalScrollBarHeight;
			var nBSpace = 8;
			int nLVTop;

			if ( sz.Height<10 )
				return;

			cboGrupp.Bounds = new Rectangle( vänstermarginal, vänstermarginal, vänsterbredd-(32+5)*2, cboGrupp.Height );
			cmdManageGroups.Bounds = new Rectangle( cboGrupp.Right + 5, vänstermarginal, 32, cboGrupp.Height );
			lblAntalIKlass.Bounds = new Rectangle( cmdManageGroups.Right + 5, vänstermarginal, 32, cboGrupp.Height );

			switch ( _FlikTyp )
			{
				case FlikTyp.GruppbildInne:
				case FlikTyp.GruppbildUte:
					nLVTop = txtSlogan.Bottom + 8;
					break;
				default:
					nLVTop = cboGrupp.Bottom + 8;
					break;
			}

			lv.Bounds = new Rectangle(
				vänstermarginal,
				nLVTop,
				vänsterbredd, 
				Math.Max(10, sz.Height - nLVTop - nBSpace ) );
			scrThumb.Bounds = new Rectangle( ClientRectangle.Width-8-sh*2, sz.Height-8-sh, sh*2, sh );

			var tns = getThumbnails();
			if ( tns!=null )
			{
				tns.layoutImages(
					lv.Right+8,
					sz.Height-tns.Height-8,
					scrThumb.Right-8 );
				if ( tns.MaxScroll==0 && scrThumb.Visible )
				{
					scrThumb.Visible = false;
					tns.FirstImage = 0;
					scrThumb.Value = 0;
				}
				else if ( tns.MaxScroll!=0 && !scrThumb.Visible )
					scrThumb.Visible = true;
				scrThumb.Maximum = tns.MaxScroll;
			}
			picNames.Invalidate();
		}

		protected void createScaledImages()
		{
			if ( _imgFull!=null && picFoto.ClientRectangle.Width>20 && picFoto.ClientRectangle.Height>20 )
				Util.setPictureBoxImageWithDispose( picFoto, new Bitmap( _imgFull, picFoto.ClientRectangle.Size ) );
			else
				Util.setPictureBoxImageWithDispose( picFoto, null );
		}

		protected bool resizePhotoBox2( int nWidth, int nHeight)
		{
			var nRader=1;
		    var	nThumbHeight = 0;
			var fRetVal = false;

			if ( _grupp!=null )
			{
				nRader = _grupp.Siffror.räknaRader();
				var tns = getThumbnails();
				if ( tns!=null )
					nThumbHeight = tns.Height;
			}
			var nNamnskyltshöjd = Math.Max(3,nRader) * _nNamnskyltteckenhöjd+1;

			if ( _imgFull == null )
			    _imgFull = new Bitmap(
			        this.GetType(),
			        string.Format("grfx.storlogga_{0}.jpg", Global.Preferences.Brand.ToString().ToLower()));
			var rectLeft = vdUsr.ImgHelper.adaptProportionalRect(
                new Rectangle( lv.Right+8, 8, nWidth-(lv.Right+16), nHeight-nThumbHeight-35-nNamnskyltshöjd ),
                _imgFull.Size.Width,
                _imgFull.Size.Height );

			picNames.Location = new Point( lv.Right+8, nHeight-nThumbHeight-20-nNamnskyltshöjd );
			picNames.Size = new Size( Math.Max(20,nWidth-(lv.Right+16)), nNamnskyltshöjd );
			if ( picFoto.Bounds!=rectLeft )
			{
				picFoto.Location = rectLeft.Location;
				if ( rectLeft.Width>20 && rectLeft.Height>20 )
					picFoto.Size = rectLeft.Size;
				fRetVal = true;
			}
			return fRetVal;
		}

		protected void resizePhotoBoxes(
			bool fDelayResize,
			bool fForceRescale,
			int nWidth,
			int nHeight )
		{
			var fResized = resizePhotoBox2( nWidth, nHeight );
			if ( fForceRescale || (fResized && !fDelayResize) )
				createScaledImages();
			else if ( fResized )
				tmrDelayedResize.Start();
		}

		protected void resizePhotoBoxes(
			bool fDelayResize,
			bool fForceRescale )
		{
			resizePhotoBoxes( fDelayResize, fForceRescale, this.ClientSize.Width, this.ClientSize.Height );
		}

		private void tmrDelayedResize_Tick(object sender, System.EventArgs e)
		{
			tmrDelayedResize.Stop();
			createScaledImages();
		}

		protected void resizePhotoBox(
			bool fNewImg,
			Image img,
			bool fDelayResize,
			int nWidth,
			int nHeight )
		{
			if ( fNewImg )
			{
				if ( _imgFull!=null )
				{
					if ( picFoto.Image==_imgFull )
						picFoto.Image = null;
					_imgFull.Dispose();
				}
				_imgFull = Util.selectLargestPage( img );
			}
			resizePhotoBoxes( fDelayResize, fNewImg, nWidth, nHeight );
			frmMain.setHistogram( (Bitmap)picFoto.Image );
		}

		protected void resizePhotoBox(
			bool fNewImg,
			Image img,
			bool fDelayResize	)
		{
			resizePhotoBox( fNewImg, img, fDelayResize, ClientSize.Width, ClientSize.Height );
		}

		public override void skolaUppdaterad()
		{
			_person = null;
            SelectedThumbnailKey = null;
			_strThumbnailkeyRightClicked = null;

		    cboGrupp.Items.Clear();
			switch ( _FlikTyp )
			{
				case FlikTyp.Personal:
					cboGrupp.Items.Add( Global.Skola.Grupper.GruppMedTyp(GruppTyp.GruppPersonal) );
					break;
				case FlikTyp.GruppbildInne:
				case FlikTyp.GruppbildUte:
					cboGrupp.Items.Add( "" );
					läggSorteradeNormalaGrupperTill_cboGrupp( true, true );
					cboGrupp.Items.Add( Global.Skola.Grupper.GruppMedTyp(GruppTyp.GruppPersonal) );
					break;
				case FlikTyp.PorträttInne:
				case FlikTyp.PorträttUte:
					cboGrupp.Items.Add( "" );
					läggSorteradeNormalaGrupperTill_cboGrupp(true, true);
					cboGrupp.Items.Add( Global.Skola.Grupper.GruppMedTyp(GruppTyp.GruppKompis) );
					break;
				case FlikTyp.Infällning:
					var grupp = Global.Skola.Grupper.GruppMedTyp(GruppTyp.GruppInfällning);
					if ( grupp!=null )
						cboGrupp.Items.Add( grupp );
					break;
			}
			if ( fmMode==FlikMode.Active )
				Util.safeSelectComboItem( cboGrupp, _grupp, true );
			_grupp = cboGrupp.SelectedItem as PlataDM.Grupp;
			if ( _grupp==null )
				lv.Items.Clear();
		}

		private void läggSorteradeNormalaGrupperTill_cboGrupp(
			bool fIncludeAggregates,
			bool fIncludeAggregateds )
		{
			var al = new ArrayList();
			foreach ( var grupp in Global.Skola.Grupper )
				if ( grupp.GruppTyp==GruppTyp.GruppNormal )
					if ( (fIncludeAggregates || !grupp.isAggregate) && (fIncludeAggregateds || !grupp.isAggregated) )
						al.Add( grupp );
			al.Sort();
			foreach ( PlataDM.Grupp grupp in al )
				cboGrupp.Items.Add( grupp );
		}

		public override void activated()
		{
			base.activated();
			if ( !Util.safeSelectComboItem( cboGrupp, _grupp, true ) )
				resizePhotoBox( true, null, false );
			else
				updateLV(null);
			frmMain.setHistogram( currentBitmap() );
		}

		virtual protected ListViewItem läggPersonTillLV( Person person, string strEtikett )
		{
			string strENamn = person.Efternamn;
			string strFNamn = person.Förnamn;
			if ( person.HarSkyddadId )
			{
				strENamn = "(" + strENamn + ")";
				strFNamn = "(" + strFNamn + ")";
			}
			var itm = new ListViewItem( strENamn );
			itm.SubItems.Add( strFNamn );
			itm.Tag = person;
			lv.Items.Add( itm );
			return itm;
		}

	    virtual protected Thumbnails getThumbnails()
		{
			return null;
		}

		virtual protected void thumbnailClicked( Thumbnail tn, bool fDoubleClick )
		{
		}

		virtual protected void visaFullskärm( string tnKey )
		{
		}

		protected override void OnMouseWheel( MouseEventArgs e )
		{
			var nNewValue = scrThumb.Value + Math.Sign( e.Delta );
			if ( nNewValue >= scrThumb.Minimum && nNewValue <= scrThumb.Maximum )
				scrThumb.Value = nNewValue;
		}

		private bool _fDrag;
		private int _nClickX, _nClickY;

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			_fDrag = false;

			if (e.Clicks == 2)
			{
				click2(e.X, e.Y, e.Button, true);
				return;
			}

			if (click1(e.X, e.Y) == null)
				return;

			_nClickX = e.X;
			_nClickY = e.Y;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (_fDrag || e.Button != MouseButtons.Left)
				return;

			var nDX = e.X - _nClickX;
			var nDY = e.Y - _nClickY;
			if ( (nDX*nDX + nDY*nDY) < 4 )
				return;

			var tn = click1(_nClickX, _nClickY);
			if (tn != null)
				this.DoDragDrop(tn, DragDropEffects.Copy);

		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			click2(e.X, e.Y, e.Button, false);
		}

		private PlataDM.Thumbnail click1(int x, int y)
		{
			var thumbnails = getThumbnails();
			if (thumbnails == null)
				return null;
			thumbnails.KeyboardFocus = null;
			return thumbnails.hitTest(x, y);
		}

		private void click2( int x, int y, MouseButtons mb, bool fDoubleClick )
		{
			var tn = click1(x, y);
			if (tn == null)
				return;

			switch ( mb )
			{
				case MouseButtons.Left:
					thumbnailClicked( tn, fDoubleClick );
					break;

				case MouseButtons.Right:
					if ( ModifierKeys == Keys.Alt )
					{
						string s = "Key: " + tn.Key +
							"\r\nFilename JPG:" + tn.FilenameJpg +
							"\r\nFilename RAW:" + tn.FilenameRaw + " (" + tn.CorrectRawExt + ")";

						var tnSelected = _person.Thumbnails[_person.ThumbnailKey];
						if( tnSelected != null && picFoto.Image != null && tnSelected != tn )
							using( var bmp = tn.loadViewImage() )
								s = string.Format( "Histogramskillnad: {0}\r\n\r\n{1}",
								 new Histogram( bmp ).Distance( new Histogram( (Bitmap)picFoto.Image ) ),
								 s );

						Global.showMsgBox(this, s );
						return;
					}
					tnRightClick( tn, new Point(x, y) );
					break;
			}
		}

		protected virtual void tnRightClick( PlataDM.Thumbnail tn, Point p )
		{
			_strThumbnailkeyRightClicked = tn.Key;
			mnuThumb.Show( this, p );
		}

		protected virtual bool deleteThumbnail( PlataDM.Thumbnail tn, bool fQuery )
		{
			return false;
		}

		private void mnuThumbRadera_Click(object sender, System.EventArgs e)
		{
			var thumbnails = getThumbnails();
			if ( thumbnails == null )
				return;
			var tn = thumbnails[_strThumbnailkeyRightClicked];
			if ( tn==null )
				return;

			if ( !deleteThumbnail( tn, true ) )
				return;

			if ( Global.askMsgBox( this, "Är du säker på att du vill radera bilden?", true ) != DialogResult.Yes )
				return;

			deleteThumbnail( tn, false );
		}

		private void lv_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            if (lv.SelectedItems.Count != 1)
                return;
		    var person = (Person)lv.SelectedItems[0].Tag;
			var ny = _person!=person;
			_person = person;
            if (!_fAvoidRecursionLV)
            {
                _fAvoidRecursionLV = true;
                personVald(ny);
                _fAvoidRecursionLV = false;
            }
		}

		protected virtual void personVald( bool fNy )
		{
		}

		private void scrThumb_Scroll(object sender, ScrollEventArgs e)
		{
			try
			{
				var tns = getThumbnails();
				if ( tns != null )
				{
					tns.FirstImage = e.NewValue;
					Invalidate();
				}
			}
			catch
			{
			}
		}

		protected void makeNewThumbnailVisible( Thumbnail tn )
		{
			var tns = getThumbnails();

			resize( ClientSize );
			tns.ensureVisible( tn );
			scrThumb.Maximum = Math.Max( scrThumb.Maximum, tns.FirstImage );
			scrThumb.Value = tns.FirstImage;
			Invalidate();
		}

		private void lv_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			var lvic = (Util.ListViewItemComparer)lv.ListViewItemSorter;
			lvic.ColumnClick( e.Column );
			lv.Sort();
		}

		private void lv_KeyPress(object sender, KeyPressEventArgs e)
		{
			var nStart = lv.SelectedItems.Count==1 ? lv.SelectedItems[0].Index : 0;
			var uChar = new string(e.KeyChar,1).ToUpper()[0];
			for ( var i=1 ; i<lv.Items.Count ; i++ )
			{
				var lvi = lv.Items[ (nStart+i) % lv.Items.Count ];
				if ( lvi.SubItems[1].Text.ToUpper()[0] == uChar )
				{
					lv.SelectedItems.Clear();
					lvi.Selected = true;
					break;
				}
			}
			e.Handled = true;
		}

		private void cboGrupp_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			Util.paintComboBoxGroup( (ComboBox)sender, e );
		}

		private void tmrAuto_Tick(object sender, System.EventArgs e)
		{
			frmMain.Camera.TakePhoto();
		}

		private void paintCameraIcons( Graphics g )
		{
			var img1 = Images.Img.Null;
			var img2 = Images.Img.Null;
			var img3 = Images.Img.Null;
			switch ( _FlikTyp )
			{
				case FlikTyp.GruppbildInne:
					//img1 = Images.Img.HouseCB;
					break;
				case FlikTyp.GruppbildUte:
					//img1 = Images.Img.TreeCB;
					break;
				case FlikTyp.PorträttInne:
				case FlikTyp.PorträttUte:
					if ( Global.Skola != null && Global.Skola.CompanyOrder != PlataDM.CompanyOrder.No )
						img1 = Images.Img.FactoryCB;
					break;
				case FlikTyp.Personal:
					//if ( Global.Skola!=null && Global.Skola.CompanyOrder != PlataDM.CompanyOrder.No )
					//  img1 = Images.Img.Null;
					//else
					//  img1 = Images.Img.FrameCB;
					break;
				case FlikTyp.Infällning:
					img1 = _presetType==eosPresets.PresetType.IndoorInsets ? Images.Img.HouseCB : Images.Img.HouseBS;
					img2 = _presetType==eosPresets.PresetType.OutdoorInsets ? Images.Img.TreeCB : Images.Img.TreeBS;
					img3 = _presetType==eosPresets.PresetType.IndoorPortrait ? Images.Img.FrameCB : Images.Img.FrameBS;
					break;
				default:
					return;
			}

			if ( img1 != Images.Img.Null )
				g.DrawImage( Images.bmp( img1 ), _rectFlik.X, 4, 32, 24 );
			if ( img2!=Images.Img.Null )
				g.DrawImage( Images.bmp(img2), _rectFlik.X+32, 4, 32, 24 );
			if ( img3!=Images.Img.Null )
				g.DrawImage( Images.bmp(img3), _rectFlik.X+64, 4, 32, 24 );
		}

		public override void paint_Flik(Graphics g, Region shape, Color color, Font font, int x)
		{
			base.paint_Flik (g, shape, color, font, x);
			paintCameraIcons( g );
		}

		private eosPresets.PresetType photoLocationHit_Icons( int x, int y )
		{
			int nHit;
			if ( (new Rectangle((int)_rectFlik.X,0,32,24)).Contains(x,y) )
				nHit = 1;
			else if ( (new Rectangle((int)_rectFlik.X+32,0,32,24)).Contains(x,y) )
				nHit = 2;
			else if ( (new Rectangle((int)_rectFlik.X+64,0,32,24)).Contains(x,y) )
				nHit = 3;
			else
				return eosPresets.PresetType.Unknown;

			switch ( _FlikTyp )
			{
				case FlikTyp.GruppbildInne:
					break;
				case FlikTyp.GruppbildUte:
					break;
				case FlikTyp.PorträttInne:
				case FlikTyp.PorträttUte:
					break;
				case FlikTyp.Infällning:
					if ( nHit==1 )
						return eosPresets.PresetType.IndoorInsets;
					if ( nHit==2 )
						return eosPresets.PresetType.OutdoorInsets;
					if ( nHit==3 )
						return eosPresets.PresetType.IndoorPortrait;
					break;
			}

			return eosPresets.PresetType.Unknown;
		}

		protected override eosPresets.PresetType photoLocationHit( int x, int y )
		{
			return photoLocationHit_Icons( x, y );
		}

		public virtual void selectGroupPerson( PlataDM.Grupp grupp, PlataDM.Person person )
		{
			if ( grupp!=null && cboGrupp.Items.Contains(grupp) )
			{
				cboGrupp.SelectedItem = grupp;
				updateLV( person );
			}
		}

		public void getSelectedGroupPerson( out PlataDM.Grupp grupp, out PlataDM.Person person )
		{
			grupp = cboGrupp.SelectedItem as PlataDM.Grupp;
			if ( lv.SelectedItems.Count==1 )
				person = lv.SelectedItems[0].Tag as PlataDM.Person;
			else
				person = null;
		}

		protected void selectPerson( PlataDM.Person pers )
		{
		    _fAvoidRecursionLV = true;
			ListViewItem itmFound = null;
			foreach ( ListViewItem itm in lv.Items )
				if ( itm.Tag == pers )
					itmFound = itm;
		    var ny = _person!=pers;
            if ( itmFound!=null)
                itmFound.Selected = true;
			_person = pers;
            //if ( MouseButtons == MouseButtons.Left )
            personVald( ny );
            _fAvoidRecursionLV = false;
		}

		virtual public void OnBlipp(string strBlipp)
		{
		}

		private void mnuSammanslagnaGrupper_Click( object sender, EventArgs e )
		{
			FSammanslagnaGrupper.showDialog( this, Global.Skola );
			frmMain.rapportera_skolaUppdaterad();
		}

		private void mnuKopieraGrupp_Click( object sender, EventArgs e )
		{
			if ( _grupp == null )
				return;
			if ( FKopieraGrupp.showDialog( this, _grupp ) == DialogResult.OK )
				frmMain.rapportera_skolaUppdaterad();
		}

		private void cmdManageGroups_Click( object sender, EventArgs e )
		{
			Point pt = this.PointToScreen( new Point( cmdManageGroups.Right, cmdManageGroups.Top ) );
			if ( GroupManagement.FGroupManagement.showDialog(
					this,
					ref _grupp,
					pt ) == DialogResult.OK )
				frmMain.rapportera_skolaUppdaterad();
		}

		public override Bitmap currentBitmap()
		{
			return (Bitmap)picFoto.Image;
		}

		public void paintLockedThumbnail( Graphics g, PlataDM.Thumbnail tn )
		{
			if ( tn == null )
				return;
			var bmp = Images.bmp( Images.Img.Padlock );
			var r = new Rectangle( Point.Empty, bmp.Size );
			g.DrawImage(
				bmp,
				new Rectangle( tn.Bounds.Right - r.Width - 3, tn.Bounds.Top + 3, r.Width, r.Height ),
				r,
				GraphicsUnit.Pixel );
		}

        private void txtGratisEx_TextChanged(object sender, EventArgs e)
        {
            var grupp = cboGrupp.SelectedItem as PlataDM.Grupp;
            if (grupp == null)
            {
                txtGratisEx.Text = "";
                return;
            }

            var allowedText = txtGratisEx.Text.Where(char.IsDigit).Aggregate("", (current, c) => current + c);
            if (allowedText != txtGratisEx.Text)
            {
                txtGratisEx.Text = allowedText;
                txtGratisEx.SelectionStart = allowedText.Length;
            }

            if ( !int.TryParse( allowedText, out grupp.AntalGratisEx ) )
                grupp.AntalGratisEx = -1;
        }

        private void txtGratisEx_Enter(object sender, EventArgs e)
        {
            txtGratisEx.SelectAll();
        }

	    protected string SelectedThumbnailKey
	    {
            get { return _selectedThumbnailkey; }
            set { setSelectedThumbnailKey(value, false); }
	    }

	    protected List<string> SelectedThumbnailKeys
	    {
	        get { return _selectedThumbnailkeys; }
	    }

        protected void setSelectedThumbnailKey( string key, bool append )
        {
            _selectedThumbnailkey = key;
            if (!append)
                _selectedThumbnailkeys.Clear();
            if (_selectedThumbnailkey != null && !_selectedThumbnailkeys.Contains(key))
                _selectedThumbnailkeys.Add(_selectedThumbnailkey);
        }

	}

}

