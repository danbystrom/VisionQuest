using System;
using System.Drawing;
using System.Windows.Forms;
using Photomic.Bildmodul.Generator;
using Photomic.Common;
using PlataDM;

namespace Plata
{

	public class FPrintStudentCards : vdUsr.baseGradientForm
	{
		private System.Windows.Forms.Button cmdClose;
		private System.Windows.Forms.Button cmdPrint;
		private ComboBox cboGrupp;
		private CheckedListBox lstPerson;
		private CheckBox chkPreview;
		private ComboBox cboMallar;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Painter _painter;
		private bool _fHasImageToShow;
		private Button cmdAllSelect;
		private Person _selectedPerson;

		private FPrintStudentCards()
		{
			InitializeComponent();

			foreach ( Template mall in Global.Skola.StudentCardTemplates )
				cboMallar.Items.Add( mall );
			cboMallar.SelectedIndex = 0;
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
			this.cmdClose = new System.Windows.Forms.Button();
			this.cmdPrint = new System.Windows.Forms.Button();
			this.cboGrupp = new System.Windows.Forms.ComboBox();
			this.lstPerson = new System.Windows.Forms.CheckedListBox();
			this.chkPreview = new System.Windows.Forms.CheckBox();
			this.cboMallar = new System.Windows.Forms.ComboBox();
			this.cmdAllSelect = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cmdClose
			// 
			this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdClose.Location = new System.Drawing.Point( 646, 12 );
			this.cmdClose.Name = "cmdClose";
			this.cmdClose.Size = new System.Drawing.Size( 80, 28 );
			this.cmdClose.TabIndex = 2;
			this.cmdClose.Text = "Stäng";
			// 
			// cmdPrint
			// 
			this.cmdPrint.Location = new System.Drawing.Point( 419, 12 );
			this.cmdPrint.Name = "cmdPrint";
			this.cmdPrint.Size = new System.Drawing.Size( 120, 28 );
			this.cmdPrint.TabIndex = 3;
			this.cmdPrint.Text = "Skriv ut";
			this.cmdPrint.Click += new System.EventHandler( this.cmdPrint_Click );
			// 
			// cboGrupp
			// 
			this.cboGrupp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboGrupp.FormattingEnabled = true;
			this.cboGrupp.Location = new System.Drawing.Point( 12, 12 );
			this.cboGrupp.Name = "cboGrupp";
			this.cboGrupp.Size = new System.Drawing.Size( 211, 21 );
			this.cboGrupp.Sorted = true;
			this.cboGrupp.TabIndex = 13;
			this.cboGrupp.SelectedIndexChanged += new System.EventHandler( this.cboGrupp_SelectedIndexChanged );
			// 
			// lstPerson
			// 
			this.lstPerson.FormattingEnabled = true;
			this.lstPerson.Location = new System.Drawing.Point( 12, 39 );
			this.lstPerson.Name = "lstPerson";
			this.lstPerson.Size = new System.Drawing.Size( 211, 379 );
			this.lstPerson.Sorted = true;
			this.lstPerson.TabIndex = 14;
			this.lstPerson.SelectedIndexChanged += new System.EventHandler( this.lstPerson_SelectedIndexChanged );
			this.lstPerson.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler( this.lstPerson_ItemCheck );
			// 
			// chkPreview
			// 
			this.chkPreview.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkPreview.Image = global::Plata.Properties.Resources.eye2;
			this.chkPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.chkPreview.Location = new System.Drawing.Point( 545, 12 );
			this.chkPreview.Name = "chkPreview";
			this.chkPreview.Size = new System.Drawing.Size( 80, 28 );
			this.chkPreview.TabIndex = 16;
			this.chkPreview.Text = "Visa ";
			this.chkPreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkPreview.UseVisualStyleBackColor = true;
			this.chkPreview.CheckedChanged += new System.EventHandler( this.chkPreview_CheckedChanged );
			// 
			// cboMallar
			// 
			this.cboMallar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMallar.FormattingEnabled = true;
			this.cboMallar.Location = new System.Drawing.Point( 229, 12 );
			this.cboMallar.Name = "cboMallar";
			this.cboMallar.Size = new System.Drawing.Size( 184, 21 );
			this.cboMallar.TabIndex = 17;
			this.cboMallar.SelectedIndexChanged += new System.EventHandler( this.cboMallar_SelectedIndexChanged );
			// 
			// cmdAllSelect
			// 
			this.cmdAllSelect.Location = new System.Drawing.Point( 12, 424 );
			this.cmdAllSelect.Name = "cmdAllSelect";
			this.cmdAllSelect.Size = new System.Drawing.Size( 211, 28 );
			this.cmdAllSelect.TabIndex = 18;
			this.cmdAllSelect.Text = "Markera / Avmarkera alla";
			this.cmdAllSelect.Click += new System.EventHandler( this.cmdAllSelect_Click );
			// 
			// FPrintStudentCards
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.CancelButton = this.cmdClose;
			this.ClientSize = new System.Drawing.Size( 738, 460 );
			this.Controls.Add( this.cmdAllSelect );
			this.Controls.Add( this.cboMallar );
			this.Controls.Add( this.chkPreview );
			this.Controls.Add( this.lstPerson );
			this.Controls.Add( this.cboGrupp );
			this.Controls.Add( this.cmdPrint );
			this.Controls.Add( this.cmdClose );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FPrintStudentCards";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Skriv ut StudentCard";
			this.ResumeLayout( false );

		}
		#endregion

		public static void showDialog(
			Form parent,
			Skola skola )
		{
			using ( var dlg = new FPrintStudentCards() )
			{
				foreach ( var grupp in skola.Grupper )
					if ( grupp.HasAnyPortrait )
						dlg.cboGrupp.Items.Add( grupp );
				if ( dlg.cboGrupp.Items.Count > 0 )
					dlg.cboGrupp.SelectedIndex = 0;
				dlg.ShowDialog( parent );
			}
		}

		protected override void OnPaint( PaintEventArgs e )
		{
			base.OnPaint( e );
			if ( _fHasImageToShow )
			{
				var r = vdUsr.ImgHelper.adaptProportionalRect(
					new Rectangle( cboMallar.Left, lstPerson.Top, cmdClose.Right - cboMallar.Left, lstPerson.Height ),
					_painter.Bitmap.Width, _painter.Bitmap.Height );
				e.Graphics.DrawImage(
					_painter.Bitmap,
					r,
					new Rectangle( Point.Empty, _painter.Bitmap.Size ),
					GraphicsUnit.Pixel );
			}
		}

		private void cboGrupp_SelectedIndexChanged( object sender, EventArgs e )
		{
			var grupp = cboGrupp.SelectedItem as Grupp;
			lstPerson.Items.Clear();
			foreach ( var person in grupp.AllaPersoner )
				if ( person.HasPhoto )
					lstPerson.Items.Add( person, !person.StudentCardIsPrinted && !person.AddedByPhotographer );
			lstPerson.SelectedIndex = 0;
		}

		private void lstPerson_ItemCheck( object sender, ItemCheckEventArgs e )
		{
			int nCount = lstPerson.CheckedItems.Count + (e.NewValue == CheckState.Checked ? 1 : -1);
			if ( nCount == 0 )
			{
				cmdPrint.Enabled = false;
				cmdPrint.Text = "Skriv ut";
			}
			else
			{
				cmdPrint.Enabled = true;
				cmdPrint.Text = string.Format( "Skriv ut {0} st", nCount );
			}
		}

		private void printPreview()
		{
			_fHasImageToShow = false;
			if ( chkPreview.Checked )
			{
				_painter.ClearBackground();
				if ( _selectedPerson != null && _selectedPerson.HasPhoto )
					using (var bmpPortrait = (Bitmap)Image.FromFile( _selectedPerson.getViewImageFileName( TypeOfViewImage.BackdroppedLo ) ))
						_painter.PaintPerson(
						0, 0, 0,
						pl => bmpPortrait,
                        null,
						_selectedPerson,
						true );
				else
					using ( var bmpBengt = new Bitmap( this.GetType(), "grfx.bengt.jpg" ) )
						_painter.PaintPerson(
							0, 0, 0,
                            pl => bmpBengt,
                            null,
							new Bengt(),
							true );
				_fHasImageToShow = true;
			}
			Invalidate();
		}

		private void cboMallar_SelectedIndexChanged( object sender, EventArgs e )
		{
			if ( _painter != null )
				_painter.Dispose();
			_painter = new Painter(
                300,
				(cboMallar.SelectedItem as Template).Page,
                null);
			printPreview();
		}

		private void lstPerson_SelectedIndexChanged( object sender, EventArgs e )
		{
			var p = lstPerson.SelectedItem as Person;
			if ( p != _selectedPerson )
			{
				_selectedPerson = p;
				printPreview();
			}
		}

		private void chkPreview_CheckedChanged( object sender, EventArgs e )
		{
			chkPreview.BackColor = chkPreview.Checked ? SystemColors.Window : SystemColors.Control;
			printPreview();
		}

		private void cmdPrint_Click( object sender, EventArgs e )
		{
			try
			{
				var mall = cboMallar.SelectedItem as Template;
				foreach ( Person p in lstPerson.CheckedItems )
				{
					if ( p.AddedByPhotographer )
					{
						string strQ = string.Format(
							"VARNING!!!\r\n\r\n\"{0}\" är en person som skapats i Plåta. Därför kan det saknas viktig information som behövs för att korrekt skriva ut kortet. Utskriften bör istället göras \"i huset\".\r\n\r\nVill du skriva ut ändå?",
							p.Namn );
						if ( Global.askMsgBox( this, strQ, true ) != DialogResult.Yes )
							continue;
					}
					_painter.ClearBackground();
                    using (var bmpPortrait = (Bitmap)Image.FromFile(p.getViewImageFileName(TypeOfViewImage.BackdroppedHi)))
						_painter.PaintPerson(
							0, 0, 0,
							pl => bmpPortrait,
                            null,
							p,
							true );
					Printer.print(
						_painter.Bitmap,
						TextConv.ConvertText( mall.Magnet, p ) );
					p.StudentCardIsPrinted = true;
				}
			}
			catch ( Exception ex )
			{
				Global.showMsgBox( this, ex.Message );
			}
		}

		private void cmdAllSelect_Click( object sender, EventArgs e )
		{
			bool fValue = lstPerson.CheckedIndices.Count != lstPerson.Items.Count;
			for ( int i=0 ; i<lstPerson.Items.Count ; i++ )
				lstPerson.SetItemChecked( i, fValue );
		}

		private class Bengt : IPersonInfo
		{
			public string ID
			{
				get { return ""; }
			}
			public string ClassName
			{
				get { return ""; }
			}
			public string FirstName
			{
				get { return "Bengt"; }
			}
			public string LastName
			{
				get { return "Hermansson"; }
			}
			public string Title
			{
				get { return "VD"; }
			}
			public string IST
			{
				get { return ""; }
			}
			public string SocialSecurity
			{
				get { return ""; }
			}
			public string Extra1
			{
				get { return ""; }
			}
			public string Extra2
			{
				get { return ""; }
			}
			public string Address
			{
				get { return "Gräsandsvägen 14"; }
			}
			public Zip Zip
			{
				get { return 37273; }
			}
			public string Town
			{
				get { return "RONNEBY"; }
			}
			public string Country
			{
				get { return "Sverige"; }
			}
			public string EMail
			{
				get { return "bengt.hermansson@photomic.com"; }
			}
			public string Phone
			{
				get { return "0457-12394"; }
			}
			public bool Personel
			{
				get { return true; }
			}
			public string OrganizationName
			{
				get { return "Photomic"; }
			}
			public string OrganizationTown
			{
				get { return "KARLSHAMN"; }
			}
			public string CustId
			{
				get { return ""; }
			}
			public string ClassList
			{
				get { return ""; }
			}
			public string Year
			{
				get { return "2010/11"; }
			}
			public string NegativeNo
			{
				get { return "113_13246839180"; }
			}
			public int PackOrder
			{
				get { return 113; }
			}
			public string ClassList1
			{
				get { return ""; }
			}

			public string ClassList2
			{
				get { return ""; }
			}


			#region IPersonInfo Members


			public PersonSkyddad ProtArchive
			{
				get { throw new NotImplementedException(); }
			}

			public PersonSkyddad ProtCatalog
			{
				get { throw new NotImplementedException(); }
			}

			public PersonSkyddad ProtGroup
			{
				get { throw new NotImplementedException(); }
			}

			public bool HasPhoto
			{
				get { throw new NotImplementedException(); }
			}

			#endregion

			#region IGetImageFilename Members

			public string ImageFilename( MotiveType2 motiveType, TypeOfViewImage tovi )
			{
				throw new NotImplementedException();
			}

			#endregion
		}

	}

}
