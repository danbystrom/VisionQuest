using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PlataDM;

namespace Plata.MainTabs.Fardigstall
{
	public partial class TabPageÖversiktPorträtt : usrBildÖversikt, IBSTab
	{

		public TabPageÖversiktPorträtt()
		{
			InitializeComponent();
		}

		void IBSTab.load()
		{
			if ( Global.Skola == null )
				return;

			initialize(
				FlikKategori.Porträtt,
				lst.Right + 5 );

			var objSelected = lst.SelectedItem;
			lst.Items.Clear();
			foreach ( Grupp grupp in Global.Skola.Grupper.GrupperIOrdning() )
				addGroupToLst( grupp );
			if ( lst.Items.Count != 0 )
				if ( objSelected != null && lst.Items.Contains( objSelected ) )
					lst.SelectedItem = objSelected;
				else
					lst.SelectedIndex = 0;
		}

		void IBSTab.save()
		{
		}

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_tns == null)
                return;
            tmrLoadPictures.Enabled = _queItem.Count != 0;

            Person person = null;
            var group = new List<Thumbnail>();
            var focus = new List<Tuple<Thumbnail,bool>>();
            foreach (Thumbnail tn in _tns)
            {
                if ( tn.BeginLayoutGroup )
                    framePerson( e.Graphics, person, group );
                group.Add(tn);
                var data = (Tuple<Person, Thumbnail>) _dicTnKeyToItem[tn.Key].Data;
                person = data.Item1;
                var tnOriginal = data.Item2;
                tn.paint(e.Graphics, null, null );
                if ( person.ThumbnailKey == tnOriginal.Key)
                    focus.Add( new Tuple<Thumbnail,bool>( tn, person.ThumbnailLocked ));
            }
            framePerson(e.Graphics, person, group);

            var bmp = Images.bmp(Images.Img.Padlock);
            var r = new Rectangle(Point.Empty, bmp.Size);
            foreach (var tuple in focus)
            {
                var tn = tuple.Item1;
                tn.paintSelections(e.Graphics, tn.Key, new List<string> {tn.Key});
                if ( tuple.Item2 )
                    e.Graphics.DrawImage(
                        bmp,
                        new Rectangle(tn.Bounds.Right - r.Width - 3, tn.Bounds.Top + 3, r.Width, r.Height),
                        r,
                        GraphicsUnit.Pixel);
            }
        }

        private void framePerson(Graphics g, Person person, IList<Thumbnail> group)
        {
            if (group.Count==0)
                return;
            var onOtherLines = group.Where(tn => tn.Y != group[0].Y).ToList();
            if ( onOtherLines.Count != 0)
            {
                framePerson(g, person, group.Where(tn => tn.Y == group[0].Y).ToList());
                group.Clear();
                group = onOtherLines;
            }
            var r = new Rectangle(
                group[0].X,
                group[0].Y + group[0].Height - 18,
                group[group.Count - 1].X - group[0].X + group[group.Count - 1].Width,
                18);
            g.FillRectangle(Brushes.Black, r);
            for (var i = 1; i < group.Count; i++ )
                g.FillRectangle( Brushes.Black, group[i].X-7, group[i].Y, 8, group[i].Height );
            g.DrawString(person.Namn, this.Font, Brushes.White, r, vdUsr.Util.sfMC);
            g.DrawRectangle(Pens.Yellow, r.X, group[0].Y, r.Width, group[0].Height);
            group.Clear();
        }

		private void addGroupToLst( Grupp grupp )
		{
		    if (grupp.AllaPersoner.Any(person => person.HasPhoto))
		        lst.Items.Add( grupp );
		}

	    private void lst_SelectedIndexChanged( object sender, EventArgs e )
	    {
	        vsb.Value = 0;
	        vsb.Maximum = 5;
			reset();
		}

		public override void reload()
		{
			base.reload();
			var grupp = lst.SelectedItem as Grupp;
			if ( grupp == null )
				return;
            foreach (var pers in grupp.AllaPersoner.Where(pers => pers.HasPhoto))
            {
                var picCount = 0;
                foreach (Thumbnail tn in pers.Thumbnails)
                    addItem(
                        tn.ThumbnailImageFilename,
                        tn.FilenameJpg,
                        new Tuple<Person,Thumbnail>( pers, tn ),
                        null,
                        picCount++ == 0);
            }
		}

        protected override void OnDoubleClick(EventArgs e)
        {
            var p = PointToClient(MousePosition);
            var tnHit = _tns.hitTest(p.X, p.Y);
            if (tnHit == null)
                return;
            var data = (Tuple<Person, Thumbnail>) _dicTnKeyToItem[tnHit.Key].Data;
            zoom( data.Item1, data.Item2 );
        }

        private void zoom( Person person, Thumbnail tn )
        {
	        var tnSelected = frmZoom.showDialog(
                FindForm(),
                null,
                null,
                _flikKategori,
                person.Thumbnails,
                tn.Key,
                null,
                person.ThumbnailKey,
                null);
            if (tnSelected == null || tnSelected.Key == person.ThumbnailKey)
                return;
            if (Global.askMsgBox(this, "Vill du ändra den valda bilden?", true) != DialogResult.Yes)
                return;
            person.ThumbnailKey = tnSelected.Key;
            Invalidate();
        }

        private Tuple<Person, Thumbnail> _dataRightClicked;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            var tnHit = _tns.hitTest(e.Location.X,e.Location.Y);
            if (tnHit == null)
                return;
            _dataRightClicked = (Tuple<Person, Thumbnail>) _dicTnKeyToItem[tnHit.Key].Data;
            var isSelected = _dataRightClicked.Item1.ThumbnailKey == _dataRightClicked.Item2.Key;
            mnuMove.Enabled = !isSelected;
            //mnuSelect.Enabled = !isSelected;
            mnuDelete.Enabled = !isSelected;
            mnuThumbnail.Show(MousePosition);
        }

        private void mnuZoom_Click(object sender, EventArgs e)
        {
            zoom(_dataRightClicked.Item1, _dataRightClicked.Item2);
        }

        private void mnuSelect_Click(object sender, EventArgs e)
        {
            _dataRightClicked.Item1.ThumbnailLocked = true;
            _dataRightClicked.Item1.ThumbnailKey = _dataRightClicked.Item2.Key;
            Invalidate();
        }

        private void mnuDelete_Click(object sender, EventArgs e)
        {
            if (Global.askMsgBox(this, "Är du säker på att du vill radera bilden?", true) != DialogResult.Yes)
                return;
            _dataRightClicked.Item1.Thumbnails.Delete(_dataRightClicked.Item2);
            reset();
        }

	    private void mnuJump_Click(object sender, EventArgs e)
        {
            FMain.theOneForm.jumpToForm_Group_Person(
                FlikTyp.PorträttInne,
                _dataRightClicked.Item1.Grupp,
                _dataRightClicked.Item1);
        }

        private void mnuMove_Click(object sender, EventArgs e)
        {
            var person = _dataRightClicked.Item1;

            bool fHoppaTillVald;
            var persTill = FSelectPerson.showDialog(FindForm(), Global.Skola, person.Grupp, out fHoppaTillVald);
            if (persTill == null)
                return;

            person.Thumbnails.Remove(_dataRightClicked.Item2);
            persTill.Thumbnails.add(_dataRightClicked.Item2);
            persTill.ThumbnailKey = _dataRightClicked.Item2.Key;
            reset();
        }

	}

}
