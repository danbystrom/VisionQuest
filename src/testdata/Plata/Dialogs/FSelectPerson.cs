using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Plata
{
	/// <summary>
	/// Summary description for FSelectPerson.
	/// </summary>
	public class FSelectPerson : vdUsr.baseGradientForm
	{
		private System.Windows.Forms.ComboBox cboGrupp;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.ListView lvPers;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private Button button1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private FSelectPerson()
		{
			InitializeComponent();
			lvPers.ListViewItemSorter = new Util.ListViewItemComparer();
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
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.lvPers = new System.Windows.Forms.ListView();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cboGrupp
			// 
			this.cboGrupp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboGrupp.Location = new System.Drawing.Point( 8, 8 );
			this.cboGrupp.Name = "cboGrupp";
			this.cboGrupp.Size = new System.Drawing.Size( 268, 21 );
			this.cboGrupp.Sorted = true;
			this.cboGrupp.TabIndex = 0;
			this.cboGrupp.SelectedIndexChanged += new System.EventHandler( this.cboGrupp_SelectedIndexChanged );
			// 
			// cmdOK
			// 
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.cmdOK.Location = new System.Drawing.Point( 12, 534 );
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size( 260, 28 );
			this.cmdOK.TabIndex = 2;
			this.cmdOK.Text = "OK - Flytta och hoppa till den här personen";
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point( 12, 602 );
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size( 260, 28 );
			this.cmdCancel.TabIndex = 4;
			this.cmdCancel.Text = "Avbryt";
			// 
			// lvPers
			// 
			this.lvPers.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader1,
            this.columnHeader3} );
			this.lvPers.FullRowSelect = true;
			this.lvPers.Location = new System.Drawing.Point( 8, 36 );
			this.lvPers.MultiSelect = false;
			this.lvPers.Name = "lvPers";
			this.lvPers.Size = new System.Drawing.Size( 268, 492 );
			this.lvPers.TabIndex = 1;
			this.lvPers.UseCompatibleStateImageBehavior = false;
			this.lvPers.View = System.Windows.Forms.View.Details;
			this.lvPers.DoubleClick += new System.EventHandler( this.lvPers_DoubleClick );
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
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.No;
			this.button1.Location = new System.Drawing.Point( 12, 568 );
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size( 260, 28 );
			this.button1.TabIndex = 3;
			this.button1.Text = "OK - Flytta men stanna vid aktuell person";
			// 
			// FSelectPerson
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size( 284, 641 );
			this.Controls.Add( this.button1 );
			this.Controls.Add( this.lvPers );
			this.Controls.Add( this.cmdCancel );
			this.Controls.Add( this.cmdOK );
			this.Controls.Add( this.cboGrupp );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FSelectPerson";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Välj person";
			this.ResumeLayout( false );

		}
		#endregion

		public static PlataDM.Person showDialog(
			Form parent,
			PlataDM.Skola skola,
			PlataDM.Grupp gruppDefault,
			out bool fHoppaTillNy )
		{
			fHoppaTillNy = false;
			using ( FSelectPerson dlg = new FSelectPerson() )
			{
				foreach ( PlataDM.Grupp grupp in skola.Grupper )
					dlg.cboGrupp.Items.Add( grupp );
				if ( gruppDefault!=null )
					dlg.cboGrupp.SelectedItem = gruppDefault;
				switch ( dlg.ShowDialog( parent ) )
				{
					case DialogResult.Yes:
						fHoppaTillNy = true;
						break;
					case DialogResult.No:
						break;
					default:
						return null;
				}
				if ( dlg.lvPers.SelectedItems.Count!=1 )
					return null;
				return dlg.lvPers.SelectedItems[0].Tag as PlataDM.Person;
			}
		}

		private void cboGrupp_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			PlataDM.Grupp grupp = cboGrupp.SelectedItem as PlataDM.Grupp;
			lvPers.Items.Clear();
			if ( grupp==null )
				return;
			foreach ( PlataDM.Person pers in grupp.AllaPersoner )
				if ( pers.Efternamn!="_slask" )
				{
					ListViewItem lvi = new ListViewItem( pers.Efternamn );
					lvi.SubItems.Add( pers.Förnamn );
					lvi.SubItems.Add( pers.Thumbnails.Count.ToString() );
					lvi.Tag = pers;
					lvPers.Items.Add( lvi );
				}
		}

		private void lvPers_DoubleClick(object sender, System.EventArgs e)
		{
			cmdOK.PerformClick();
		}

	}

}
