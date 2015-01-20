using System.Collections.Generic;
using System.Windows.Forms;
using PlataDM;

namespace Plata
{
	/// <summary>
	/// Summary description for FSelectPerson.
	/// </summary>
	public class FSelectPersons : vdUsr.baseGradientForm
	{
        private System.Windows.Forms.ComboBox cboGrupp;
		private System.Windows.Forms.ListView lvPers;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private Label label1;
        private Button button2;
        private Button cmdOK;
        private ListView lvSelection;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader7;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

	    private List<Person> _selection = new List<Person>();

		private FSelectPersons()
		{
			InitializeComponent();
            var lviComp = new Util.ListViewItemComparer { ColumnIndex = Global.Preferences.SortOrderLastName ? 1 : 2 };
            lvPers.ListViewItemSorter = lviComp;
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
            this.cboGrupp = new System.Windows.Forms.ComboBox();
            this.lvPers = new System.Windows.Forms.ListView();
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.lvSelection = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // cboGrupp
            // 
            this.cboGrupp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGrupp.Location = new System.Drawing.Point(8, 8);
            this.cboGrupp.Name = "cboGrupp";
            this.cboGrupp.Size = new System.Drawing.Size(268, 21);
            this.cboGrupp.Sorted = true;
            this.cboGrupp.TabIndex = 0;
            this.cboGrupp.SelectedIndexChanged += new System.EventHandler(this.cboGrupp_SelectedIndexChanged);
            // 
            // lvPers
            // 
            this.lvPers.CheckBoxes = true;
            this.lvPers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader2,
            this.columnHeader1,
            this.columnHeader3});
            this.lvPers.FullRowSelect = true;
            this.lvPers.Location = new System.Drawing.Point(8, 36);
            this.lvPers.MultiSelect = false;
            this.lvPers.Name = "lvPers";
            this.lvPers.Size = new System.Drawing.Size(295, 492);
            this.lvPers.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvPers.TabIndex = 1;
            this.lvPers.UseCompatibleStateImageBehavior = false;
            this.lvPers.View = System.Windows.Forms.View.Details;
            this.lvPers.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvPers_ItemChecked);
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "";
            this.columnHeader7.Width = 25;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Efternamn";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Förnamn";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Bilder";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader3.Width = 45;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(306, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Valda personer:";
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(556, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 28);
            this.button2.TabIndex = 10;
            this.button2.Text = "Avbryt";
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(469, 12);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(80, 28);
            this.cmdOK.TabIndex = 9;
            this.cmdOK.Text = "OK";
            // 
            // lvSelection
            // 
            this.lvSelection.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.lvSelection.FullRowSelect = true;
            this.lvSelection.Location = new System.Drawing.Point(309, 52);
            this.lvSelection.MultiSelect = false;
            this.lvSelection.Name = "lvSelection";
            this.lvSelection.Size = new System.Drawing.Size(327, 476);
            this.lvSelection.TabIndex = 11;
            this.lvSelection.UseCompatibleStateImageBehavior = false;
            this.lvSelection.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Efternamn";
            this.columnHeader4.Width = 100;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Förnamn";
            this.columnHeader5.Width = 100;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Grupp";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader6.Width = 100;
            // 
            // FSelectPersons
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(642, 542);
            this.Controls.Add(this.lvSelection);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvPers);
            this.Controls.Add(this.cboGrupp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FSelectPersons";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Välj person(er)";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		public static List<Person> showDialog(
			Form parent,
			Skola skola,
			Grupp gruppDefault )
		{
			using ( var dlg = new FSelectPersons() )
			{
				foreach ( var grupp in skola.Grupper )
					dlg.cboGrupp.Items.Add( grupp );
				if ( gruppDefault!=null )
					dlg.cboGrupp.SelectedItem = gruppDefault;
				return dlg.ShowDialog( parent ) == DialogResult.OK
                    ? dlg._selection
                    : null;
			}
		}

		private void cboGrupp_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			var grupp = cboGrupp.SelectedItem as Grupp;
			lvPers.Items.Clear();
			if ( grupp==null )
				return;
            lvPers.BeginUpdate();
			foreach ( var pers in grupp.AllaPersoner )
				if ( pers.Efternamn!="_slask" )
				{
				    var lvi = new ListViewItem {Checked = _selection.Contains(pers)};
				    lvi.SubItems.Add(pers.Efternamn);
                    lvi.SubItems.Add(pers.Förnamn);
					lvi.SubItems.Add( pers.Thumbnails.Count.ToString() );
					lvi.Tag = pers;
					lvPers.Items.Add( lvi );
				}
            lvPers.EndUpdate();
		}

        private void displaySelection()
        {
            lvSelection.BeginUpdate();
            lvSelection.Items.Clear();

            foreach (var pers in _selection )
                if (pers.Efternamn != "_slask")
                {
                    var lvi = new ListViewItem(pers.Efternamn);
                    lvi.SubItems.Add(pers.Förnamn);
                    lvi.SubItems.Add(pers.Grupp.Namn);
                    lvSelection.Items.Add(lvi);
                }

            lvSelection.EndUpdate();
        }

        private void lvPers_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            var p = (Person) e.Item.Tag;
            if (e.Item.Checked && !_selection.Contains(p))
                _selection.Add(p);
            else if (!e.Item.Checked && _selection.Contains(p))
                _selection.Remove(p);
            displaySelection();
        }

	}

}
