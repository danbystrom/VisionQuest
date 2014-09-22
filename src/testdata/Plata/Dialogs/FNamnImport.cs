using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using Photomic.Common;
using PlataDM;

namespace Plata
{
	/// <summary>
	/// Summary description for FNamnImport.
	/// </summary>
	public class FNamnImport : vdUsr.baseGradientForm
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TreeView tvwImport;
		private System.Windows.Forms.TreeView tvwBefintligt;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.ComponentModel.IContainer components;

		private PlataDM.Skola _skolaDest;
		private Dictionary<string, Dictionary<string,string>> _dicExistingGroups = new Dictionary<string,Dictionary<string,string>>();

		private FNamnImport()
		{
			InitializeComponent();
		}

		private FNamnImport( Skola skolaDest, Skola skolaSrc )
		{
			InitializeComponent();

			_skolaDest = skolaDest;
			foreach ( Grupp grupp in _skolaDest.Grupper )
			{
				var dicPers = new Dictionary<string,string>();
				foreach ( Person pers in grupp.AllaPersoner )
					if ( !dicPers.ContainsKey( pers.Namn ) )
						dicPers.Add( pers.Namn, pers.Namn );
				_dicExistingGroups.Add( grupp.Namn, dicPers );
			}

			displaySchool( tvwImport, skolaSrc );
			displaySchool( tvwBefintligt, _skolaDest );
			scanSimilarities();
		}

		private void displaySchool( TreeView twv, Skola skola )
		{
			var tnSkola = new TreeNode( skola.OrderNr + " - " + skola.Namn, 0, 0 );
			twv.Nodes.Add( tnSkola );
			var alGrupper = new List<Grupp>();
			alGrupper.AddRange( skola.Grupper );
			alGrupper.Sort();
			foreach ( Grupp grupp in alGrupper )
				switch ( grupp.GruppTyp )
				{
					case GruppTyp.GruppNormal:
					case GruppTyp.GruppPersonal:
						var tnGrupp = new TreeNode( grupp.Namn );
						tnSkola.Nodes.Add( tnGrupp );
						var alPersoner = new List<Person>( grupp.AllaPersoner );
						alPersoner.Sort();
						foreach ( var person in alPersoner )
						{
							var tns = tnGrupp.Nodes.Add( person.Namn );
							tns.Tag = person;
						}
						break;
				}
			tnSkola.Expand();
		}

		private void scanSimilarities()
		{
			foreach ( TreeNode tnGrp in tvwImport.Nodes[0].Nodes )
			{
				bool fGroupMatch = _dicExistingGroups.ContainsKey(tnGrp.Text);
				tnGrp.ImageIndex = tnGrp.SelectedImageIndex =  fGroupMatch ? 2 : 1;
				if ( fGroupMatch )
				{
					var h = _dicExistingGroups[tnGrp.Text];
					foreach ( TreeNode tnPers in tnGrp.Nodes )
						tnPers.ImageIndex = tnPers.SelectedImageIndex = h.ContainsKey(tnPers.Text) ? 2 : 1;
				}
				else
					foreach ( TreeNode tnPers in tnGrp.Nodes )
						tnPers.ImageIndex = tnPers.SelectedImageIndex = 1;
			}
		}

		private void performImport()
		{
			Dictionary<string,string> h;
			Grupp grpAddTo;

			foreach ( TreeNode tnGrp in tvwImport.Nodes[0].Nodes )
			{
				if ( _dicExistingGroups.ContainsKey(tnGrp.Text) )
				{
					h = _dicExistingGroups[tnGrp.Text];
					grpAddTo = null;

					foreach ( Grupp grp in _skolaDest.Grupper )
						if ( grp.Namn.Equals(tnGrp.Text) )
							grpAddTo = grp;
					if ( grpAddTo==null )
						continue;  //error?!
				}
				else
				{
					grpAddTo = _skolaDest.Grupper.Add( tnGrp.Text, GruppTyp.GruppNormal ); 
					h = null;
				}

				foreach ( TreeNode tnPers in tnGrp.Nodes )
					if ( h == null || !h.ContainsKey( tnPers.Text ) )
					{
						var pS = (Person)tnPers.Tag;
						var pD = grpAddTo.PersonerNärvarande.Add( pS.Personal, pS.getInfos() );
						pD.ScanCode = pS.ScanCode;
					}
			}

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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( FNamnImport ) );
			this.tvwImport = new System.Windows.Forms.TreeView();
			this.imageList1 = new System.Windows.Forms.ImageList( this.components );
			this.tvwBefintligt = new System.Windows.Forms.TreeView();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// tvwImport
			// 
			this.tvwImport.ImageIndex = 0;
			this.tvwImport.ImageList = this.imageList1;
			this.tvwImport.LabelEdit = true;
			this.tvwImport.Location = new System.Drawing.Point( 8, 24 );
			this.tvwImport.Name = "tvwImport";
			this.tvwImport.SelectedImageIndex = 0;
			this.tvwImport.Size = new System.Drawing.Size( 200, 392 );
			this.tvwImport.TabIndex = 0;
			this.tvwImport.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler( this.tvwImport_AfterLabelEdit );
			this.tvwImport.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler( this.tvwImport_BeforeLabelEdit );
			this.tvwImport.KeyDown += new System.Windows.Forms.KeyEventHandler( this.tvwImport_KeyDown );
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject( "imageList1.ImageStream" )));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName( 0, "" );
			this.imageList1.Images.SetKeyName( 1, "" );
			this.imageList1.Images.SetKeyName( 2, "" );
			// 
			// tvwBefintligt
			// 
			this.tvwBefintligt.Location = new System.Drawing.Point( 232, 24 );
			this.tvwBefintligt.Name = "tvwBefintligt";
			this.tvwBefintligt.Size = new System.Drawing.Size( 200, 392 );
			this.tvwBefintligt.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label1.Location = new System.Drawing.Point( 232, 8 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 200, 16 );
			this.label1.TabIndex = 2;
			this.label1.Text = "Befintliga namn och grupper:";
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label2.Location = new System.Drawing.Point( 8, 8 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 200, 16 );
			this.label2.TabIndex = 3;
			this.label2.Text = "Importera namn och grupper:";
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point( 456, 58 );
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size( 80, 28 );
			this.cmdCancel.TabIndex = 8;
			this.cmdCancel.Text = "Avbryt";
			// 
			// cmdOK
			// 
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point( 456, 24 );
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size( 80, 28 );
			this.cmdOK.TabIndex = 7;
			this.cmdOK.Text = "Importera";
			this.cmdOK.Click += new System.EventHandler( this.cmdOK_Click );
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label3.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
			this.label3.Location = new System.Drawing.Point( 8, 424 );
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size( 528, 16 );
			this.label3.TabIndex = 9;
			this.label3.Text = "De namn/grupper du INTE vill importera raderar du genom att markera och trycka De" +
					"l på tangentbordet.";
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label4.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
			this.label4.Location = new System.Drawing.Point( 8, 440 );
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size( 528, 56 );
			this.label4.TabIndex = 10;
			this.label4.Text = resources.GetString( "label4.Text" );
			// 
			// FNamnImport
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size( 544, 499 );
			this.Controls.Add( this.label4 );
			this.Controls.Add( this.label3 );
			this.Controls.Add( this.cmdCancel );
			this.Controls.Add( this.cmdOK );
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.tvwBefintligt );
			this.Controls.Add( this.tvwImport );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FNamnImport";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Importera grupper och namn från order";
			this.ResumeLayout( false );

		}
		#endregion

		public static DialogResult showDialog( Form parent, Skola skolaDest, Skola skolaSrc )
		{
			using ( var dlg = new FNamnImport(skolaDest,skolaSrc) )
				return dlg.ShowDialog(parent);
		}

		private void tvwImport_AfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
			if ( e.Label.Length>1 )
			{
				e.Node.Text = e.Label;
				scanSimilarities();
			}
			else
				e.CancelEdit = true;
		}

		private void tvwImport_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			TreeNode tns = tvwImport.SelectedNode;
			if ( tns==null || tns.Parent==null )
				return;

			switch ( e.KeyCode )
			{
				case Keys.Delete:
					tns.Remove();
					break;
				case Keys.F2:
					if ( tns.Parent.Parent==null )
						tns.BeginEdit();
					else
					{
						var person = new Person( null );
						person.setInfos( (string[])tns.Tag );
						if ( frmPersonnamn.redigera( this, person, null ) == DialogResult.OK )
						{
							tns.Tag = person.getInfos();
							tns.Text = person.Efternamn + ", " + person.Förnamn;
							scanSimilarities();
						}
					}
					break;
			}
		}

		private void tvwImport_BeforeLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
			TreeNode tns = tvwImport.SelectedNode;
			e.CancelEdit = tns.Parent!=tvwImport.Nodes[0];
		}

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			performImport();
		}

	}

}
