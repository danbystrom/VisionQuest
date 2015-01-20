using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Photomic.Common;

namespace Plata.GroupManagement
{
	/// <summary>
	/// Summary description for frmGruppunderhåll.
	/// </summary>
	public class FMakuleraGrupper : vdUsr.baseGradientForm
	{
		private ListBox lstGrupp1;
		private System.ComponentModel.IContainer components;
		private ListBox lstGrupp2;
		private Button cmdMoveLeft;
		private Button cmdMoveRight;
		private Button cmdOK;
		private Button cmdCancel;
		private Label label1;
		private Label label2;

		private FMakuleraGrupper()
		{
			InitializeComponent();

			foreach ( PlataDM.Grupp g in Global.Skola.Grupper )
				if ( g.GruppTyp == GruppTyp.GruppNormal )
					(g.Makulerad ? lstGrupp2 : lstGrupp1).Items.Add( g );
			foreach ( PlataDM.Grupp g in Global.Skola.MakuleradeGrupper )
				lstGrupp2.Items.Add( g );
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
			this.lstGrupp1 = new System.Windows.Forms.ListBox();
			this.lstGrupp2 = new System.Windows.Forms.ListBox();
			this.cmdMoveLeft = new System.Windows.Forms.Button();
			this.cmdMoveRight = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lstGrupp1
			// 
			this.lstGrupp1.Location = new System.Drawing.Point( 12, 27 );
			this.lstGrupp1.Name = "lstGrupp1";
			this.lstGrupp1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstGrupp1.Size = new System.Drawing.Size( 200, 420 );
			this.lstGrupp1.Sorted = true;
			this.lstGrupp1.TabIndex = 1;
			// 
			// lstGrupp2
			// 
			this.lstGrupp2.Location = new System.Drawing.Point( 261, 27 );
			this.lstGrupp2.Name = "lstGrupp2";
			this.lstGrupp2.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstGrupp2.Size = new System.Drawing.Size( 200, 420 );
			this.lstGrupp2.Sorted = true;
			this.lstGrupp2.TabIndex = 4;
			// 
			// cmdMoveLeft
			// 
			this.cmdMoveLeft.Location = new System.Drawing.Point( 218, 95 );
			this.cmdMoveLeft.Name = "cmdMoveLeft";
			this.cmdMoveLeft.Size = new System.Drawing.Size( 37, 28 );
			this.cmdMoveLeft.TabIndex = 7;
			this.cmdMoveLeft.Text = "<--";
			this.cmdMoveLeft.Click += new System.EventHandler( this.cmdLeft_Click );
			// 
			// cmdMoveRight
			// 
			this.cmdMoveRight.Location = new System.Drawing.Point( 218, 61 );
			this.cmdMoveRight.Name = "cmdMoveRight";
			this.cmdMoveRight.Size = new System.Drawing.Size( 37, 28 );
			this.cmdMoveRight.TabIndex = 8;
			this.cmdMoveRight.Text = "-->";
			this.cmdMoveRight.Click += new System.EventHandler( this.cmdRight_Click );
			// 
			// cmdOK
			// 
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point( 467, 27 );
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size( 80, 28 );
			this.cmdOK.TabIndex = 6;
			this.cmdOK.Text = "OK";
			this.cmdOK.Click += new System.EventHandler( this.cmdOK_Click );
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label1.Location = new System.Drawing.Point( 12, 9 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 81, 13 );
			this.label1.TabIndex = 9;
			this.label1.Text = "Vanliga grupper";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label2.Location = new System.Drawing.Point( 258, 9 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 102, 13 );
			this.label2.TabIndex = 10;
			this.label2.Text = "Makulerade grupper";
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point( 467, 61 );
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size( 80, 28 );
			this.cmdCancel.TabIndex = 11;
			this.cmdCancel.Text = "Avbryt";
			// 
			// FMakuleraGrupper
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size( 559, 459 );
			this.Controls.Add( this.cmdCancel );
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.cmdOK );
			this.Controls.Add( this.cmdMoveRight );
			this.Controls.Add( this.cmdMoveLeft );
			this.Controls.Add( this.lstGrupp2 );
			this.Controls.Add( this.lstGrupp1 );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FMakuleraGrupper";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Gruppunderhåll";
			this.ResumeLayout( false );
			this.PerformLayout();

		}
		#endregion

		private void cmdLeft_Click(object sender, System.EventArgs e)
		{
			move( lstGrupp2, lstGrupp1 );
		}

		private void cmdRight_Click(object sender, EventArgs e)
		{
			move( lstGrupp1, lstGrupp2 );
		}

		private void move( ListBox lstSource, ListBox lstDest )
		{
			List<PlataDM.Grupp> list = new List<PlataDM.Grupp>();
			foreach ( PlataDM.Grupp g in lstSource.SelectedItems )
				add( list, g );
			foreach ( PlataDM.Grupp g in list )
			{
				lstSource.Items.Remove( g );
				lstDest.Items.Add( g );
				lstDest.SelectedItems.Add( g );
			}
		}

		private void add( List<PlataDM.Grupp> list, PlataDM.Grupp g )
		{
			if ( list.Contains(g) )
				return;
			list.Add( g );
			if ( g.isAggregated )
				add( list, g.aggregate );
			if ( g.isAggregate )
				foreach ( PlataDM.Grupp g2 in g.aggregatedGroups )
					add( list, g2 );
		}

		private void lstGrupp1_DoubleClick(object sender, EventArgs e)
		{
			cmdMoveRight.PerformClick();
		}

		private void lstGrupp2_DoubleClick(object sender, EventArgs e)
		{
			cmdMoveLeft.PerformClick();
		}

		private bool checkOkToDeface( PlataDM.Grupp grupp )
		{
			bool fHasGroupPhoto = !string.IsNullOrEmpty( grupp.ThumbnailKey );
			bool fHasPortrait = false;
			foreach ( PlataDM.Person pers in grupp.AllaPersoner )
				if ( pers.HasPhoto )
				{
					fHasPortrait = true;
					break;
				}

			string strQ;
			switch ( (fHasGroupPhoto ? 2 : 0) + (fHasPortrait ? 1 : 0) )
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

			strQ = string.Format( "Gruppen \"{0}\" har {1}. Är du säker på att du vill makulera den ändå?", grupp.Namn, strQ );
			return Global.askMsgBox( this, strQ, true ) == DialogResult.Yes;
		}

		private void cmdOK_Click( object sender, EventArgs e )
		{
			foreach ( PlataDM.Grupp g in lstGrupp2.Items )
				if ( !checkOkToDeface( g ) )
				{
					this.DialogResult = DialogResult.None;
					return;
				}
		}

		public static DialogResult showDialog( Form parent )
		{
			using ( FMakuleraGrupper dlg = new FMakuleraGrupper() )
			{
				if ( dlg.ShowDialog( parent ) != DialogResult.OK )
					return DialogResult.Cancel;
				foreach ( PlataDM.Grupp g in dlg.lstGrupp1.Items )
					g.Makulerad = false;
				foreach ( PlataDM.Grupp g in dlg.lstGrupp2.Items )
					g.Makulerad = true;
				return DialogResult.OK;
			}
		}

	}

}
