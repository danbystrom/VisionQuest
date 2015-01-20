using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using PlataDM;

namespace Plata
{
	public partial class tabPageQuestionarie : UserControl, IBSTab
	{
		private ArrayList _alOptGrupper = new ArrayList();
		private ArrayList _alTexts = new ArrayList();

		private OptGroup _ogJaNej = null;
		private OptGroup _ogDåligtBra = null;

		public tabPageQuestionarie()
		{
			InitializeComponent();
			initieraFlikEnkät();
		}

		protected override void OnPaint( PaintEventArgs e )
		{
			base.OnPaint( e );

			if ( _ogJaNej != null )
			{
				e.Graphics.DrawString(
					"JA",
					this.Font,
					SystemBrushes.ControlText,
					_ogJaNej.RButtons[0].Left,
					_ogJaNej.RButtons[0].Top - 24 );
				e.Graphics.DrawString(
					"NEJ",
					this.Font,
					SystemBrushes.ControlText,
					_ogJaNej.RButtons[1].Right,
					_ogJaNej.RButtons[1].Top - 24,
					vdUsr.Util.sfUR );
			}

			if ( _ogDåligtBra != null )
			{
				e.Graphics.DrawString(
					"DÅLIGT",
					this.Font,
					SystemBrushes.ControlText,
					_ogDåligtBra.RButtons[0].Left,
					_ogDåligtBra.RButtons[0].Top - 24 );
				e.Graphics.DrawString(
					"BRA",
					this.Font,
					SystemBrushes.ControlText,
					_ogDåligtBra.RButtons[4].Right,
					_ogDåligtBra.RButtons[4].Top - 24,
					vdUsr.Util.sfUR );
			}

		}

		void IBSTab.load()
		{
			foreach ( OptGroup og in _alOptGrupper )
			{
				int nSelected;
				if ( !int.TryParse( Global.Skola.Enkät[og.Key], out nSelected ) )
					nSelected = -1;
				for ( int i = 0 ; i < og.RButtons.Length ; i++ )
					og.RButtons[i].Checked = i == nSelected;
			}
			foreach ( object[] aobj in _alTexts )
				(aobj[1] as TextBox).Text = Global.Skola.Enkät[aobj[0] as string];
		}

		void IBSTab.save()
		{
			foreach ( OptGroup og in _alOptGrupper )
				Global.Skola.Enkät[og.Key] = og.selectedIndex.ToString();
			foreach ( object[] aobj in _alTexts )
				Global.Skola.Enkät[aobj[0] as string] = (aobj[1] as TextBox).Text.Trim();
		}

		private void enkätRad( string[] astrText, int nY, int nOptions, EventHandler ehClick )
		{
			Label lbl = new Label();
			this.Controls.Add( lbl );

			OptGroup og = new OptGroup( astrText[1], nOptions );
			_alOptGrupper.Add( og );

			for ( int i = 0 ; i < nOptions ; i++ )
			{
				RadioButton opt = og.RButtons[i];
				opt.Location = new System.Drawing.Point( 384 + i * 32, nY );
				opt.AutoCheck = false;
				opt.AutoSize = true;
				opt.Click += ehClick;
				this.Controls.Add( opt );
			}

			lbl.AutoSize = true;
			lbl.Location = new System.Drawing.Point( 16, nY );
			lbl.Text = astrText[0];
		}


		private TextBox textfält( string[] astrText, ref int nY, int nH )
		{
			Label lbl = new Label();
			this.Controls.Add( lbl );
			lbl.AutoSize = true;
			lbl.Location = new System.Drawing.Point( 16, nY );
			lbl.Text = astrText[0];
			nY += 18;

			TextBox tb = new TextBox();
			this.Controls.Add( tb );
			tb.Bounds = new Rectangle( 16, nY, this.ClientSize.Width - 32, nH );
			tb.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
			tb.Multiline = true;
			nY += nH + 24;

			_alTexts.Add( new object[] { astrText[1], tb } );

			return tb;
		}

		private void initieraFlikEnkät()
		{
			var astrYN = new string[][]
			{
				new string[] {"Skolan/Förskolan har förberett ett fotoschema","Q1_1"},
				new string[] {"Skolan/Förskolan har använt Photomics fotoschema","Q1_2"},
				new string[] {"Skolan/Förskolan har följt angivna fototider","Q1_3"},
				new string[] {"Jag fick fotoschemat tillsänt mig i förväg","Q1_4"},
				new string[] {"Räckte den inplanerade tiden till?","Q1_5"},
				new string[] {"Det fanns planerade raster","Q1_6"},
				new string[] {"Jag fick Order-CD tillsänt mig i förväg","Q1_7"},
				new string[] {"Jag använde gruppbildsbakgrund/stativ","Q1_8"},
			};
			var astrSkala = new string[][]
			{
				new string[] {"Hur var elevernas/barnens attityd till fotograferingen?","Q2_1"},
				new string[] {"Jag som fotograf upplevde skolan/förskolan?","Q2_2"},
				new string[] {"Hur fungerade KM-foto?","Q2_3"},
				new string[] {"Fotolokalen var?","Q2_4"},
			};
			var astrTextfrågor = new string[][]
			{
				new string[] {"Fotograferingen utfördes i (lokal)","Q3_1"},
				new string[] {"Fotograferingen har varit","Q3_2"},
			};

			int nY = 16;
			foreach ( string[] astr in astrYN )
                enkätRad(astr, nY += 18, 2, optEnkätSvar_Click);
			_ogJaNej = _alOptGrupper[0] as OptGroup;

			nY += 28;
			foreach ( string[] astr in astrSkala )
                enkätRad(astr, nY += 18, 5, optEnkätSvar_Click);
			_ogDåligtBra = _alOptGrupper[_alOptGrupper.Count - 4] as OptGroup;

			nY += 28;
			foreach ( string[] astr in astrTextfrågor )
				textfält( astr, ref nY, 64 );

		}

		private OptGroup findOptGroup( RadioButton optFind )
		{
			foreach ( OptGroup og in _alOptGrupper )
				foreach ( RadioButton rb in og.RButtons )
					if ( rb == optFind )
						return og;
			return null;
		}

		private void optEnkätSvar_Click( object sender, EventArgs e )
		{
			RadioButton optClicked = sender as RadioButton;
			foreach ( RadioButton opt in findOptGroup(optClicked).RButtons )
				opt.Checked = opt == optClicked;
		}

		private class OptGroup
		{
			public readonly string Key;
			public readonly RadioButton[] RButtons;

			public OptGroup( string strKey, int nSize )
			{
				Key = strKey;
				RButtons = new RadioButton[nSize];
				for ( int i = 0 ; i < RButtons.Length ; i++ )
					RButtons[i] = new RadioButton();
			}

			public int selectedIndex
			{
				get
				{
					for ( int i = 0 ; i < RButtons.Length ; i++ )
						if ( RButtons[i].Checked )
							return i;
					return -1;
				}
			}

		}

	}

}
