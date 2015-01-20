using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Photomic.Bildmodul.Generator;
using Photomic.Common;
using Photomic.Common.Img;
using Plata.Camera;
using PlataDM;

namespace Plata
{

	public class frmPortratt : Plata.baseGruppForm
	{
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ContextMenu mnuFoto;
		private System.Windows.Forms.ContextMenu mnuPerson2;
		private System.Windows.Forms.MenuItem mnuFotoTöm;
		private System.Windows.Forms.MenuItem mnuFotoSwap;
		private System.Windows.Forms.MenuItem mnuFotoFullskärm;

		private System.Windows.Forms.MenuItem mnuThumbDuplicate;
		private System.Windows.Forms.MenuItem mnuPersonKopieraSpecial;
		private System.Windows.Forms.MenuItem mnuPersonKopieraInfällningar;
		private System.Windows.Forms.MenuItem mnuPersonKopieraTill;
		private System.Windows.Forms.MenuItem mnuThumbFlytta;

		static readonly Font _fontBigName = new Font("Arial",16,FontStyle.Bold);

		private int _nStudentCardTemplate = 0;
		private readonly List<Rectangle> _listStudentCardTemplates = new List<Rectangle>();

        private Bitmap _backdroppedImage;
	    private MenuItem mnuThumbCopyToPersons;

        private readonly vdBitmaps _backdrop = new vdBitmaps(
            1,
            Size.Empty,
            ImgHelpers.ResizeMode.ProportionalClipExpand,
            ImgHelpers.WhenNotFound.ReturnNull);


		public frmPortratt()
		{
			InitializeComponent();
		}

		public frmPortratt( Form parent, FlikTyp fliktyp ) : base(parent,fliktyp)
		{
			switch ( fliktyp )
			{
				case FlikTyp.PorträttInne:
					_strCaption = "PORTRÄTT INNE";
					_presetType = eosPresets.PresetType.IndoorPortrait;
					break;
				case FlikTyp.PorträttUte:
					_strCaption = "PORTRÄTT UTE";
					_presetType = eosPresets.PresetType.OutdoorPortrait;
					break;
				case FlikTyp.Personal:
					_strCaption = "PERSONAL";
					_presetType = eosPresets.PresetType.IndoorPortrait;
					break;
				case FlikTyp.Infällning:
					_strCaption = "INFÄLLNING";
					_presetType = eosPresets.PresetType.IndoorPortrait;
					break;
			}
			// This call is required by the Windows Form Designer.
			InitializeComponent();
			this.Bounds = parent.ClientRectangle;
			this.PerformLayout();

			picFoto.MouseDown += picFoto_MouseDown;
			picFoto.Paint += picFoto_Paint;

			this.chFörnamn.Width = 72;
			this.chEfternamn.Width = 72;
			if ( _FlikTyp!=FlikTyp.Infällning )
			{
				var chNummer = new ColumnHeader();
				chNummer.Text = "Kundnr";
				chNummer.Width = 96;
				lv.Columns.Add( chNummer );
				var chAntal = new ColumnHeader();
				chAntal.Text = "";
				chAntal.Width = 27;
				chAntal.TextAlign = HorizontalAlignment.Right;
				lv.Columns.Add( chAntal );
			}
			else
			{
				var chNummer = new ColumnHeader();
				chNummer.Text = "Grupp";
				chNummer.Width = 85;
				lv.Columns.Add( chNummer );
				var chInfBock = new ColumnHeader();
				chInfBock.Text = string.Empty;
				chInfBock.Width = 20;
				lv.Columns.Add( chInfBock );
			}

			mnuPerson.Popup += mnuPerson_Popup;
			mnuPerson.MenuItems.Add( 0, mnuPersonKopieraTill );
			mnuPerson.MenuItems[0].MenuItems.Add( 0, mnuPersonKopieraSpecial );
			mnuPerson.MenuItems[0].MenuItems.Add( 1, mnuPersonKopieraInfällningar );
			mnuPerson.MenuItems.Add( 1, new MenuItem("-") );

            mnuThumbCopyToPersons = mnuThumb.MenuItems.Add("Kopiera till person(er)");
            mnuThumbCopyToPersons.Click += mnuClickCopyToPersons;

			addSubmenuForImageCopy( mnuThumb );
		}

		protected override Thumbnail getSelectedThumbnail()
		{
			return _person.Thumbnails[_strThumbnailkeyRightClicked];
		}

        protected override IEnumerable<Thumbnail> getSelectedThumbnails()
        {
            return SelectedThumbnailKeys.Select(key => _person.Thumbnails[key]);
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
			this.mnuFoto = new System.Windows.Forms.ContextMenu();
			this.mnuFotoFullskärm = new System.Windows.Forms.MenuItem();
			this.mnuFotoTöm = new System.Windows.Forms.MenuItem();
			this.mnuFotoSwap = new System.Windows.Forms.MenuItem();
			this.mnuPerson2 = new System.Windows.Forms.ContextMenu();
			this.mnuPersonKopieraTill = new System.Windows.Forms.MenuItem();
			this.mnuPersonKopieraSpecial = new System.Windows.Forms.MenuItem();
			this.mnuPersonKopieraInfällningar = new System.Windows.Forms.MenuItem();
			this.mnuThumbDuplicate = new System.Windows.Forms.MenuItem();
			this.mnuThumbFlytta = new System.Windows.Forms.MenuItem();
			((System.ComponentModel.ISupportInitialize)(this.picFoto)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picNames)).BeginInit();
			this.SuspendLayout();
			// 
			// cboGrupp
			// 
			this.cboGrupp.Location = new System.Drawing.Point(10, 9);
			this.cboGrupp.Size = new System.Drawing.Size(270, 22);
			this.toolTip1.SetToolTip(this.cboGrupp, "Grupper");
			// 
			// lv
			// 
			this.lv.Location = new System.Drawing.Point(10, 69);
			this.lv.Size = new System.Drawing.Size(326, 500);
			// 
			// picFoto
			// 
			this.picFoto.Location = new System.Drawing.Point(365, 65);
			this.picFoto.Size = new System.Drawing.Size(585, 340);
			// 
			// picNames
			// 
			this.picNames.Location = new System.Drawing.Point(341, 439);
			this.picNames.Size = new System.Drawing.Size(686, 39);
			// 
			// scrThumb
			// 
			this.scrThumb.Location = new System.Drawing.Point(1018, 586);
			// 
			// lblAntalIKlass
			// 
			this.lblAntalIKlass.Location = new System.Drawing.Point(296, 8);
			this.lblAntalIKlass.Size = new System.Drawing.Size(38, 22);
			this.toolTip1.SetToolTip(this.lblAntalIKlass, "Antal personer i gruppen");
			// 
			// mnuThumb
			// 
			this.mnuThumb.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																						 this.mnuThumbFlytta,
																																						 this.mnuThumbDuplicate});
			// 
			// mnuFoto
			// 
			this.mnuFoto.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFotoFullskärm,
            this.mnuFotoTöm,
            this.mnuFotoSwap});
			// 
			// mnuFotoFullskärm
			// 
			this.mnuFotoFullskärm.Index = 0;
			this.mnuFotoFullskärm.Shortcut = System.Windows.Forms.Shortcut.F5;
			this.mnuFotoFullskärm.Text = "Visa fullskärm";
			this.mnuFotoFullskärm.Click += new System.EventHandler(this.mnuFotoFullskärm_Click);
			// 
			// mnuFotoTöm
			// 
			this.mnuFotoTöm.Index = 1;
			this.mnuFotoTöm.Text = "Töm";
			this.mnuFotoTöm.Click += new System.EventHandler(this.mnuFotoTöm_Click);
			// 
			// mnuFotoSwap
			// 
			this.mnuFotoSwap.Index = 2;
			this.mnuFotoSwap.Text = "Byt plats";
			// 
			// mnuPerson2
			// 
			this.mnuPerson2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuPersonKopieraTill});
			// 
			// mnuPersonKopieraTill
			// 
			this.mnuPersonKopieraTill.Index = 0;
			this.mnuPersonKopieraTill.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuPersonKopieraSpecial,
            this.mnuPersonKopieraInfällningar});
			this.mnuPersonKopieraTill.Text = "Kopiera namn och bild till";
			// 
			// mnuPersonKopieraSpecial
			// 
			this.mnuPersonKopieraSpecial.Index = 0;
			this.mnuPersonKopieraSpecial.Text = "Special";
			this.mnuPersonKopieraSpecial.Click += new System.EventHandler(this.mnuPersonKopieraSpecial_Click);
			// 
			// mnuPersonKopieraInfällningar
			// 
			this.mnuPersonKopieraInfällningar.Index = 1;
			this.mnuPersonKopieraInfällningar.Text = "Infällningar";
			this.mnuPersonKopieraInfällningar.Click += new System.EventHandler(this.mnuPersonKopieraInfällningar_Click);
			// 
			// mnuThumbDuplicate
			// 
			this.mnuThumbDuplicate.Text = "Duplicera foto";
			this.mnuThumbDuplicate.Click += new System.EventHandler(this.mnuThumbDuplicate_Click);
			// 
			// mnuThumbFlytta
			// 
			this.mnuThumbFlytta.Text = "Flytta till annan person";
			this.mnuThumbFlytta.Click += new System.EventHandler(this.mnuThumbFlytta_Click);
			// 
			// frmPortratt
			// 
			this.AllowDrop = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(1070, 609);
			this.Name = "frmPortratt";
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmPortratt_DragDrop);
			this.DragOver += new System.Windows.Forms.DragEventHandler(this.frmPortratt_DragOver);
			((System.ComponentModel.ISupportInitialize)(this.picFoto)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picNames)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			_nNamnskyltteckenhöjd = 0;
			picNames.Visible = false;
		}

		protected override void resize( Size sz )
		{
			base.resize( sz );
			resizePhotoBoxes( true, false, sz.Width - 100, sz.Height );
		}

        private void loadPhotoBox( Bitmap bmp )
        {
            resizePhotoBox(true, bmp, false, ClientSize.Width - 100, ClientSize.Height);
            if ( _backdroppedImage != null )
                _backdroppedImage.Dispose();
            _backdroppedImage = null;
            Invalidate();
        }

        public override void nyttFoto(bool fInternal, byte[] jpgData, byte[] rawData)
        {
            if (_grupp == null || _person == null)
            {
                Global.showMsgBox(this, "Du måste välja en person först!");
                return;
            }

            if (Global.Skola.CompanyOrder == PlataDM.CompanyOrder.SmallImage)
                if (rawData == null || rawData.Length == 0)
                {
                    Global.showMsgBox(this, "Till SmallImage företagsfoto behövs både RAW och JPG!");
                    return;
                }

            if (!fInternal)
            {
                Global.MediaPlayer.Open(Global.Preferences.PortraitSoundLong);
                Global.MediaPlayer.Play();
            }

            if (FlikTyp != FlikTyp.PorträttUte)
                if (!fInternal && !FKollaKamera.kollaKamera(frmMain, _presetType))
                    return;

            var bmpPlus = new Bitmap(new MemoryStream(jpgData));
            var rotFlip = bmpPlus.Height < bmpPlus.Width ? Global.PorträttRotateFlipType : RotateFlipType.RotateNoneFlipNone;
            bmpPlus.RotateFlip(rotFlip);

            if (bmpPlus.Width < 1500 || bmpPlus.Height < 2300)
                if (
                    Global.askMsgBox(this,
                                     "Det är för låg upplösning på den här bilden för att den ska kunna användas! Vill du läsa in den ändå?",
                                     true) != DialogResult.Yes)
                {
                    bmpPlus.Dispose();
                    return;
                }

            var jpgFile = Global.Skola.HomePathCombine(string.Format("p_{0}.jpg", Guid.NewGuid()));
            string rawFile = null;
            if (rawData != null)
                if (Global.Skola.CompanyOrder != PlataDM.CompanyOrder.SmallImage)
                    Global.showMsgBox(this, "RAW behövs inte när du tar porträttbilder!");
                else
                    rawFile = Path.ChangeExtension(jpgFile, ".cr2");

            var bmpCache = new Bitmap(
                bmpPlus,
                Global.Porträttfotobredd,
                bmpPlus.Size.Height*Global.Porträttfotobredd/bmpPlus.Size.Width);
            var tn = _person.Thumbnails.addImage(
                jpgFile,
                rawFile,
                bmpCache);

            if (!fInternal)
                frmZoom.gotNewImage(tn, bmpPlus);

            tn.fixPortrait(
                bmpPlus,
                bmpCache.Size,
                Global.Skola.Fotograf,
                rotFlip,
                adrenochrome,
                adrenochromeDone);

            if (!_person.ThumbnailLocked)
                _person.ThumbnailKey = tn.Key;
            SelectedThumbnailKey = tn.Key;
            loadPhotoBox(bmpCache);
            makeNewThumbnailVisible(tn);

            if (!fInternal && Global.Preferences.FullskärmslägePorträttfoto && !frmZoom.isVisible)
                visaFullskärm(tn.Key);

            updateLV(null);

            if (rawFile != null)
                File.WriteAllBytes(rawFile, rawData);
        }

	    private Bitmap adrenochrome( Bitmap bmp )
        {
            if (string.IsNullOrEmpty(Global.Skola.Backdrop))
                return null;
            _backdrop.SetMaxSizeAndClearIfDifferentFromPrevious(bmp.Size);
	        var vdbmp = _backdrop.Add(string.Format("{0}\\_backdrops\\{1}.jpg", Global.Preferences.MainPath, Global.Skola.Backdrop));
            if ( vdbmp.Bitmap == null )
                throw new Exception("Hittar inte backdroppen!");

	        Bitmap bmpNew;
            lock (vdbmp)
            {
                bmpNew = new Bitmap(vdbmp);
            }

	        var backmask = Adrenochrome.ChromaKey.ConstructBackgroundMask.Execute(bmp, 3, 30, 2, ThreadPriority.Lowest);
	        new Adrenochrome.ChromaKey.Blender()
	            .ExecuteSolidResultOnBackground(bmp, bmpNew, backmask);
            return bmpNew;
        }

	    private void adrenochromeDone(PlataDM.Thumbnail tn, string result)
	    {
	        if (InvokeRequired)
	        {
	            BeginInvoke((MethodInvoker) (() => adrenochromeDone(tn, result)));
	            return;
	        }

            if (result != null)
            {
                Global.showMsgBox(this, result);
                return;
            }

            if (tn.Key == SelectedThumbnailKey)
            {
                SelectedThumbnailKey = null;
                thumbnailClicked(tn, false);
            }
	    }

	    protected override bool deleteThumbnail( Thumbnail tn, bool fQuery )
		{
			if ( fQuery )
			{
				if ( _person.ThumbnailKey == tn.Key )
				{
					Global.showMsgBox( this, "Du får inte radera en vald bild!" );
					return false;
				}
				return true;
			}

            if (SelectedThumbnailKey == _strThumbnailkeyRightClicked)
			{
                SelectedThumbnailKey = null;
                loadPhotoBox(null);
			}
			if ( _person.ThumbnailKey == tn.Key )
				_person.ThumbnailKey = null;
			_person.Thumbnails.Delete( tn );
			Invalidate();
			updateLV( null );
			return false;
		}

		protected override void personVald( bool fNy )
		{
			if ( !fNy )
				return;

			var tn = _person != null
                ? _person.Thumbnails[_person.ThumbnailKey]
                : null;
			if ( tn!=null )
                thumbnailClicked( tn, false );
            else if (_grupp.sökThumbnailKey(SelectedThumbnailKey) != null)
			{
                SelectedThumbnailKey = null;
                loadPhotoBox(null);
			}
            else
			    Invalidate();

			resize( ClientSize );
			if ( tn!=null )
			{
				_person.Thumbnails.ensureVisible( tn );
				scrThumb.Value = Math.Min( scrThumb.Maximum, _person.Thumbnails.FirstImage );
			}
			Invalidate();
		}

		protected override void nyGruppVald()
		{
            SelectedThumbnailKey = null;
            loadPhotoBox(null);
			resize( ClientSize );
			Invalidate();
		}

        override protected void tnRightClick(Thumbnail tn, Point p)
        {
            if (!SelectedThumbnailKeys.Contains(tn.Key))
                thumbnailClicked(tn, false);
            var single = SelectedThumbnailKeys.Count == 1;
            mnuThumbRadera.Enabled = single;
            base.tnRightClick(tn, p);
        }

	    override protected void thumbnailClicked( Thumbnail tn, bool doubleClick )
        {
            if (tn == null)
                return;

            if (ModifierKeys == Keys.Control && !doubleClick)
            {
                if (SelectedThumbnailKey == tn.Key)
                    return;
                if (SelectedThumbnailKeys.Contains(tn.Key))
                    SelectedThumbnailKeys.Remove(tn.Key);
                else
                    SelectedThumbnailKeys.Add(tn.Key);
                Invalidate();
                return;
            }

            if (SelectedThumbnailKey != tn.Key)
            {
                SelectedThumbnailKey = tn.Key;
                loadPhotoBox(tn.loadViewImage());
                var fn = tn.GetBackdroppedFilename();
                if (fn != null)
                    _backdroppedImage = new Bitmap(fn);
            }

            if (doubleClick)
            {
                if (_person.ThumbnailKey != SelectedThumbnailKey)
                {
                    if (_person.ThumbnailLocked && ModifierKeys != Keys.Control)
                    {
                        Global.showMsgBox(this,
                                          "Eftersom bilden är låst måste du hålla in Ctrl-tangenten samtidigt som du dubbelklickar för att byta bild!");
                        return;
                    }
                    _person.ThumbnailKey = SelectedThumbnailKey;
                    updateLV(null);
                }
                _person.ThumbnailLocked = true;
            }

            Invalidate();
        }

	    protected override void paint( PaintEventArgs e )
		{
			if ( _person == null )
				return;
			_person.Thumbnails.paintWithSelections(
				e.Graphics,
                SelectedThumbnailKeys,
				_person.ThumbnailKey,
				null );

			if ( _person.ThumbnailLocked )
				paintLockedThumbnail( e.Graphics, _person.SelectedThumbnail );

			int nY = lblAntalIKlass.Top;

			string strKundnr;
			if ( !string.IsNullOrEmpty( _person.ScanCode ) )
				strKundnr = _person.ScanCode[0] != '0' ? _person.ScanCode : null;
			else if ( _person.Personal || _FlikTyp == FlikTyp.Personal )
				strKundnr = null;
			else 
				strKundnr = "(saknar kundnummer)";
			if ( strKundnr != null )
			{
				e.Graphics.DrawString(
					strKundnr,
					_fontBigName,
					Brushes.Black,
					lblAntalIKlass.Right + 10,
					nY );
				nY += 20;
			}
			e.Graphics.DrawString(
				_person.Namn,
				_fontBigName,
				Brushes.Black,
				lblAntalIKlass.Right + 10,
				nY );
			nY += 24;
			if ( !string.IsNullOrEmpty( _person.Address ) )
			{
				e.Graphics.DrawString( _person.Address, this.Font, Brushes.Black, lblAntalIKlass.Right + 10, nY );
				nY += 15;
				e.Graphics.DrawString( string.Format( "{0}  {1}",
					_person.Zip,
					_person.Town ),
					this.Font, Brushes.Black, lblAntalIKlass.Right + 10, nY );
				nY += 15;
			}
			if ( !string.IsNullOrEmpty( _person.SocialSecurity ) )
			{
				e.Graphics.DrawString( _person.SocialSecurity,
					this.Font, Brushes.Black, lblAntalIKlass.Right + 10, nY );
				nY += 15;
			}

			if ( !_person.HasPhoto || _person.AddedByPhotographer || Global.Skola.StudentCardTemplates.Count == 0 )
				return;

			var strText = "^W - Utskrift av StudentCard:";
			foreach ( Template mall in Global.Skola.StudentCardTemplates )
				strText += string.Format( "\r\n      {0}", mall.Namn );
			var sz = e.Graphics.MeasureString( strText, this.Font ).ToSize();
			var rct = new Rectangle(
				this.ClientSize.Width - sz.Width - 5 - 10,
				5,
				sz.Width + 10,
				sz.Height + 4 );
			e.Graphics.FillRectangle( Brushes.LightYellow, rct );
			rct.Inflate( -2, -2 );
			e.Graphics.DrawString(
				strText,
				this.Font,
				Brushes.Black,
				rct,
				vdUsr.Util.sfUL );
			rct.Inflate( 4, 4 );
			ControlPaint.DrawBorder(
				e.Graphics,
				rct,
				Color.LightYellow,
				ButtonBorderStyle.Outset );

			_listStudentCardTemplates.Clear();
			for ( int i = 0, y = rct.Top+6 ; i < Global.Skola.StudentCardTemplates.Count ; i++ )
			{
				y += (int)this.Font.GetHeight();
				var rect = new Rectangle( rct.Left+8, y, 14, 14 );
				ControlPaint.DrawRadioButton(
					e.Graphics,
					rect,
					i == _nStudentCardTemplate ? ButtonState.Checked : ButtonState.Normal );
				_listStudentCardTemplates.Add( rect );
			}

		}

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (_backdroppedImage != null)
            {
                var r = new Rectangle(
                    picFoto.Right + 10,
                    picFoto.Top,
                    256,
                    384);
                pevent.Graphics.DrawImage(
                    _backdroppedImage,
                    r,
                    new Rectangle(Point.Empty, _backdroppedImage.Size),
                    GraphicsUnit.Pixel);
                pevent.Graphics.ExcludeClip(r);
            }
            base.OnPaintBackground(pevent);
        }

		protected override void OnKeyPress( KeyPressEventArgs e )
		{
		    if (e.KeyChar == ('W' & 0x1F) &&
		        _person != null &&
		        _person.HasPhoto &&
		        !_person.AddedByPhotographer &&
		        Global.Skola.StudentCardTemplates.Count > _nStudentCardTemplate)
		    {
		        e.Handled = true;
		        if (_person.StudentCardIsPrinted)
		            if (Global.askMsgBox(this, "Är du säker på att du vill skriva ut den här personen en gång till?", true) !=
		                DialogResult.Yes)
		                return;
		        var mall = Global.Skola.StudentCardTemplates[_nStudentCardTemplate] as Template;
		        try
		        {
		            using (var painter = new Painter(300, mall.Page, null))
		            {
		                painter.ClearBackground();
		                using (var bmpPortrait = (Bitmap) Image.FromFile(_person.getViewImageFileName(TypeOfViewImage.BackdroppedHi)))
		                    painter.PaintPerson(
		                        0, 0, 0,
		                        pl => bmpPortrait,
                                null,
		                        _person,
		                        true);
		                Printer.print(
		                    painter.Bitmap,
		                    TextConv.ConvertText(mall.Magnet, _person));
		                _person.StudentCardIsPrinted = true;
		            }
		        }
		        catch (Exception ex)
		        {
		            Global.showMsgBox(this, ex.Message);
		        }
		        return;
		    }

		    base.OnKeyPress(e);
		}

	    protected override void OnMouseDown( MouseEventArgs e )
		{
			base.OnMouseDown( e );

			var i = 0;
			foreach ( var rect in _listStudentCardTemplates )
			{
				if ( rect.Contains( e.X, e.Y ) )
				{
					Invalidate( _listStudentCardTemplates[_nStudentCardTemplate] );
					_nStudentCardTemplate = i;
					Invalidate( _listStudentCardTemplates[_nStudentCardTemplate] );
				}
				i++;
			}
		}

		public override void activated()
		{
			base.activated ();
			eosPresets.ApplyPreset(_presetType,frmMain.Camera);
		}

		override protected ListViewItem läggPersonTillLV( PlataDM.Person person, string strEtikett )
		{
			var itm = base.läggPersonTillLV( person, strEtikett );
			if ( FlikTyp!=FlikTyp.Infällning )
			{
				string strText1;
				switch ( person.GruppPersonTyp )
				{
					case GruppPersonTyp.PersonNormal:
						strText1 = person.Personal ? "P " : string.Empty;
						break;
					case GruppPersonTyp.PersonFrånvarande:
						strText1 = "F ";
						break;
					default:
						strText1 = "U ";
						break;
				}

				itm.SubItems.Add( strText1 + person.ScanCode );
				itm.SubItems.Add( person.Thumbnails.Count!= 0 ? person.Thumbnails.Count.ToString() : "" );
			}
			else
			{
				itm.SubItems.Add( person.Titel );
				itm.SubItems.Add( !string.IsNullOrEmpty(person.ThumbnailKey) ? "x" : string.Empty );
			}
			return itm;
		}

		override protected PlataDM.Thumbnails getThumbnails()
		{
		    return _person!=null
                ?  _person.Thumbnails
                : null;
		}

	    private void picFoto_MouseDown(object sender, MouseEventArgs e)
		{
			if ( e.Button==MouseButtons.Right )
			{
                _strThumbnailkeyRightClicked = SelectedThumbnailKey;
				mnuFoto.Show( picFoto, new Point(e.X, e.Y) );
			}
		}

		private void mnuPerson_Popup(object sender, EventArgs e)
		{
			var fK = false;
			var fI = false;

			if ( _person!=null && !string.IsNullOrEmpty(_person.ThumbnailKey) )
			{
				fK = _grupp.GruppTyp!=GruppTyp.GruppKompis;
				fI = _grupp.GruppTyp!=GruppTyp.GruppInfällning;
			}
			mnuPersonKopieraSpecial.Enabled = fK;
			mnuPersonKopieraInfällningar.Enabled = fI;
		}

		private void mnuFotoTöm_Click(object sender, System.EventArgs e)
		{
			if (_person == null)
				return;
            if (SelectedThumbnailKey == _strThumbnailkeyRightClicked)
			{
                _person.ThumbnailKey = SelectedThumbnailKey = null;
                loadPhotoBox(null);
			}
			Invalidate();
		}

		protected override void visaFullskärm( string tnKey )
		{
			if ( _grupp==null )
				return;
			vdUsr.vdOneShotTimer.start(1, visaFullskärm2, tnKey);
		}

		private void visaFullskärm2(
			object sender,
			EventArgs e )
		{
			var tn = frmZoom.showDialog(
				this,
				null,
				null,
				FlikKategori.Porträtt,
				getThumbnails(),
                ((vdUsr.vdOneShotTimer)sender).Tag as string,
				null,
				null,
				null);
			if (tn != null && _person != null)
				thumbnailClicked(tn, true);
		}

		private void mnuFotoFullskärm_Click(object sender, System.EventArgs e)
		{
			visaFullskärm( _strThumbnailkeyRightClicked );
		}

		private void drawWarning( Graphics g )
		{
			g.DrawString(
				"Varning: Denna bild är inte den valda!",
				picFoto.Font,
				Brushes.Red,
				3, 3 );
		}

		private void picFoto_Paint(object sender, PaintEventArgs e)
		{
			Global.ritaPorträttRam( e.Graphics, picFoto.ClientRectangle );
            if (_person != null && SelectedThumbnailKey != _person.ThumbnailKey)
				drawWarning( e.Graphics );
		}

		private void mnuThumbDuplicate_Click(object sender, System.EventArgs e)
		{
		    if (_person == null)
                return;
            foreach (var tn in SelectedThumbnailKeys.Select(key => _person.Thumbnails[key]))
                _person.Thumbnails.duplicateThumbnail(tn);
		    resize( this.ClientSize );
		    Invalidate();
		}

		private void kopieranamnochbild(
			GruppTyp gt )
		{
			var grupp =	Global.Skola.Grupper.GruppMedTyp( gt );
			var persNy = grupp.PersonerNärvarande.Add(
				_person.Personal,
				_person.getInfos() );
			persNy.IST = string.Empty;
			persNy.ScanCode = string.Empty;
			if ( gt==GruppTyp.GruppInfällning )
				persNy.Titel = _grupp.Namn;

            foreach(Thumbnail tn in _person.Thumbnails)
			{
				var tnNy = persNy.Thumbnails.duplicateThumbnail( tn );
                if ( tn.Key == _person.ThumbnailKey )
				    persNy.ThumbnailKey = tnNy.Key;
			}

		}

		private void mnuPersonKopieraSpecial_Click(object sender, System.EventArgs e)
		{
			kopieranamnochbild( GruppTyp.GruppKompis );
		}

		private void mnuPersonKopieraInfällningar_Click(object sender, System.EventArgs e)
		{
			kopieranamnochbild( GruppTyp.GruppInfällning );
		}

		private void mnuThumbFlytta_Click(object sender, System.EventArgs e)
		{
			if ( _person==null )
				return;
			if ( SelectedThumbnailKeys.Count == 0 )
				return;

			bool fHoppaTillVald;
			var persTill = FSelectPerson.showDialog( this, Global.Skola, _grupp, out fHoppaTillVald );
			if ( persTill==null )
				return;

            foreach (var tn in SelectedThumbnailKeys.Select(key => _person.Thumbnails[key]))
            {
                _person.Thumbnails.Remove(tn);
                persTill.Thumbnails.add(tn);
                persTill.ThumbnailKey = tn.Key;
            }

		    SelectedThumbnailKey = null;
            if ( _person.Thumbnails[_person.ThumbnailKey] == null)
            {
                _person.ThumbnailKey = null;
                _person.ThumbnailLocked = false;
            }

		    updateLV( null );
			if ( fHoppaTillVald )
			{
				cboGrupp.SelectedItem = persTill.Grupp;
				updateLV( persTill );
			}

			_strThumbnailkeyRightClicked = null;
			Invalidate();
		}

        private void mnuClickCopyToPersons(object sender, System.EventArgs e)
        {
            if (_person == null)
                return;

            var persons = FSelectPersons.showDialog(this, Global.Skola, _grupp);
            if (persons == null)
                return;
            persons.Remove(_person);

            foreach ( var p in persons )
                foreach ( var tn in SelectedThumbnailKeys.Select( key => _person.Thumbnails[key]))
                    p.Thumbnails.add(new Thumbnail(tn, null));

            updateLV(null);
        }

	    public override void skolaUppdaterad()
		{
			base.skolaUppdaterad ();
			if ( Global.Skola == null )
				return;
			switch ( _FlikTyp )
			{
				case FlikTyp.Personal:
					if ( Global.Skola.CompanyOrder == PlataDM.CompanyOrder.No )
						goto case FlikTyp.PorträttInne;
					_presetType = eosPresets.PresetType.Unknown;
					break;
				case FlikTyp.PorträttInne:
					_presetType = Global.Skola.CompanyOrder == PlataDM.CompanyOrder.SmallImage ?
                        eosPresets.PresetType.IndoorPortraitCompany :
						eosPresets.PresetType.IndoorPortrait;
					break;
				case FlikTyp.PorträttUte:
					_presetType = Global.Skola.CompanyOrder == PlataDM.CompanyOrder.SmallImage ?
						eosPresets.PresetType.IndoorPortraitCompany :
						eosPresets.PresetType.OutdoorPortrait;
					break;
			}
			foreach ( Template mall in Global.Skola.StudentCardTemplates )
				if ( mall.Personal == (_FlikTyp == FlikTyp.Personal) )
					_nStudentCardTemplate = Global.Skola.StudentCardTemplates.IndexOf( mall );
		}

		private void frmPortratt_DragOver(object sender, DragEventArgs e)
		{
			var fOK = false;
			if (e.Data.GetDataPresent(typeof(PlataDM.Thumbnail)))
			{
				var p = PointToClient(new Point(e.X, e.Y));
				fOK = picFoto.Bounds.Contains(p);
			}
			e.Effect = fOK ? DragDropEffects.Copy : e.Effect = DragDropEffects.None;
		}

		private void frmPortratt_DragDrop(object sender, DragEventArgs e)
		{
			if ( _person==null || !e.Data.GetDataPresent(typeof(PlataDM.Thumbnail)) )
				return;
	
			var tn = e.Data.GetData(typeof(PlataDM.Thumbnail)) as PlataDM.Thumbnail;
			var p = PointToClient(new Point(e.X, e.Y));

			if (picFoto.Bounds.Contains(p))
			{
                _person.ThumbnailKey = SelectedThumbnailKey = tn.Key;
                loadPhotoBox(tn.loadViewImage());
			}
			else
				return;

			updateLV(null);
			Invalidate();

		}

	}

}

