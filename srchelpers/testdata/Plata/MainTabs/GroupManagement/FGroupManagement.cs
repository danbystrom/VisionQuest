using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Photomic.Common;
using PlataDM;

namespace Plata.GroupManagement
{

	public class FGroupManagement : vdUsr.baseGradientForm
	{
		private Button cmdNewMerged;
		private Button cmdNewGroup;
		private Button cmdChangeName;
		private Button cmdCopyGroup;
		private Button cmdCancelGroup;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Grupp _grupp;

		private FGroupManagement()
		{
			InitializeComponent();
		}

		private FGroupManagement( Point ptUpperLeft, Grupp grupp )
		{
			InitializeComponent();
			_grupp = grupp;
			this.Location = ptUpperLeft;
			this.Height = cmdNewGroup.Top + cmdCancelGroup.Bottom;

			foreach ( Button btn in this.Controls )
				btn.Text = btn.Text.Replace( "||", Environment.NewLine );

			if ( _grupp == null || _grupp.GruppTyp != GruppTyp.GruppNormal )
			{
				//cmdCancelGroup.Enabled = false;
				cmdChangeName.Enabled = false;
				cmdCopyGroup.Enabled = _grupp != null && _grupp.GruppTyp == GruppTyp.GruppPersonal;
			}
		}

		protected override void OnLoad( EventArgs e )
		{
			base.OnLoad( e );
			this.Capture = true;
		}

		protected override void OnMouseDown( MouseEventArgs e )
		{
			if ( e.Button == MouseButtons.Left )
			{
				foreach ( Button btn in this.Controls )
					if ( btn.Bounds.Contains( e.X, e.Y ) )
					{
						btn.PerformClick();
						return;
					}
				this.Close();
			}
		}

		protected override void OnKeyPress( KeyPressEventArgs e )
		{
			base.OnKeyPress( e );
			if ( e.KeyChar == (char)27 )
				this.Close();
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
			this.cmdNewMerged = new System.Windows.Forms.Button();
			this.cmdNewGroup = new System.Windows.Forms.Button();
			this.cmdChangeName = new System.Windows.Forms.Button();
			this.cmdCopyGroup = new System.Windows.Forms.Button();
			this.cmdCancelGroup = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cmdNewMerged
			// 
			this.cmdNewMerged.BackColor = System.Drawing.Color.LightGreen;
			this.cmdNewMerged.Location = new System.Drawing.Point( 12, 12 );
			this.cmdNewMerged.Name = "cmdNewMerged";
			this.cmdNewMerged.Size = new System.Drawing.Size( 218, 122 );
			this.cmdNewMerged.TabIndex = 0;
			this.cmdNewMerged.Tag = "SLÅ IHOP GRUPPER";
			this.cmdNewMerged.Text = "&1.||Används ALLTID när du vill slå ihop grupper/namnlistor från två eller flera " +
					"grupper";
			this.cmdNewMerged.UseVisualStyleBackColor = false;
			this.cmdNewMerged.Paint += new System.Windows.Forms.PaintEventHandler( this.cmd_Paint );
			this.cmdNewMerged.Click += new System.EventHandler( this.cmdNewMerged_Click );
			// 
			// cmdNewGroup
			// 
			this.cmdNewGroup.BackColor = System.Drawing.Color.Orange;
			this.cmdNewGroup.Location = new System.Drawing.Point( 460, 12 );
			this.cmdNewGroup.Name = "cmdNewGroup";
			this.cmdNewGroup.Size = new System.Drawing.Size( 218, 122 );
			this.cmdNewGroup.TabIndex = 2;
			this.cmdNewGroup.Tag = "SKAPA NY GRUPP";
			this.cmdNewGroup.Text = "&3.||Används ENDAST då namnlistor/grupp saknas. Annars använder du \"Skapa ny samm" +
					"anslagen grupp\".";
			this.cmdNewGroup.UseVisualStyleBackColor = false;
			this.cmdNewGroup.Paint += new System.Windows.Forms.PaintEventHandler( this.cmd_Paint );
			this.cmdNewGroup.Click += new System.EventHandler( this.cmdNewGroup_Click );
			// 
			// cmdChangeName
			// 
			this.cmdChangeName.BackColor = System.Drawing.Color.Beige;
			this.cmdChangeName.Location = new System.Drawing.Point( 236, 13 );
			this.cmdChangeName.Name = "cmdChangeName";
			this.cmdChangeName.Size = new System.Drawing.Size( 218, 122 );
			this.cmdChangeName.TabIndex = 1;
			this.cmdChangeName.Tag = "BYT NAMN PÅ GRUPP";
			this.cmdChangeName.Text = "&2.||Byter namn (eller syfte) på den grupp som du just nu har uppe.";
			this.cmdChangeName.UseVisualStyleBackColor = false;
			this.cmdChangeName.Paint += new System.Windows.Forms.PaintEventHandler( this.cmd_Paint );
			this.cmdChangeName.Click += new System.EventHandler( this.cmdChangeName_Click );
			// 
			// cmdCopyGroup
			// 
			this.cmdCopyGroup.BackColor = System.Drawing.Color.Beige;
			this.cmdCopyGroup.Location = new System.Drawing.Point( 236, 141 );
			this.cmdCopyGroup.Name = "cmdCopyGroup";
			this.cmdCopyGroup.Size = new System.Drawing.Size( 218, 122 );
			this.cmdCopyGroup.TabIndex = 3;
			this.cmdCopyGroup.Tag = "KOPIERA GRUPP";
			this.cmdCopyGroup.Text = "&4.||Kopierar den grupp du just nu har uppe.";
			this.cmdCopyGroup.UseVisualStyleBackColor = false;
			this.cmdCopyGroup.Paint += new System.Windows.Forms.PaintEventHandler( this.cmd_Paint );
			this.cmdCopyGroup.Click += new System.EventHandler( this.cmdCopyGroup_Click );
			// 
			// cmdCancelGroup
			// 
			this.cmdCancelGroup.BackColor = System.Drawing.Color.Red;
			this.cmdCancelGroup.Location = new System.Drawing.Point( 460, 141 );
			this.cmdCancelGroup.Name = "cmdCancelGroup";
			this.cmdCancelGroup.Size = new System.Drawing.Size( 218, 122 );
			this.cmdCancelGroup.TabIndex = 4;
			this.cmdCancelGroup.Tag = "MAKULERA GRUPP";
			this.cmdCancelGroup.Text = "&5.||Används när du är säker på att en grupp tillkommit av misstag. (Du kan även " +
					"av-makulera grupper här om du ångrar dig.)";
			this.cmdCancelGroup.UseVisualStyleBackColor = false;
			this.cmdCancelGroup.Paint += new System.Windows.Forms.PaintEventHandler( this.cmd_Paint );
			this.cmdCancelGroup.Click += new System.EventHandler( this.cmdCancelGroup_Click );
			// 
			// FGroupManagement
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size( 689, 275 );
			this.ControlBox = false;
			this.Controls.Add( this.cmdCancelGroup );
			this.Controls.Add( this.cmdCopyGroup );
			this.Controls.Add( this.cmdChangeName );
			this.Controls.Add( this.cmdNewGroup );
			this.Controls.Add( this.cmdNewMerged );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FGroupManagement";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "";
			this.ResumeLayout( false );

		}
		#endregion

		public static DialogResult showDialog(
			Form parent,
			ref Grupp grupp,
			Point ptUpperLeft )
		{
			using ( FGroupManagement dlg = new FGroupManagement( ptUpperLeft, grupp ) )
				if ( dlg.ShowDialog( parent ) == DialogResult.OK )
				{
					grupp = dlg._grupp;
					return DialogResult.OK;
				}
				else
					return DialogResult.Cancel;
		}

		private void cmd_Paint( object sender, PaintEventArgs e )
		{
			Control ctrl = sender as Control;
			Rectangle r = ctrl.ClientRectangle;
			r.Offset( 0, 5 );
			using ( Font fnt = new Font( "Arial", 14, FontStyle.Bold ) )
				e.Graphics.DrawString(
					ctrl.Tag as string,
					fnt,
					ctrl.Enabled ? SystemBrushes.ControlText : SystemBrushes.ControlDark,
					r,
					vdUsr.Util.sfUC );
		}

		private void cmdCancelGroup_Click( object sender, EventArgs e )
		{
			this.DialogResult = FMakuleraGrupper.showDialog( this.Owner );
		}

		private void cmdNewMerged_Click( object sender, EventArgs e )
		{
			string strNewName;
			IList listNormal;
			IList listValda;

			listNormal = new ArrayList();
			foreach ( Grupp g in Global.Skola.Grupper )
				if ( g.GruppTyp == GruppTyp.GruppNormal )
					if ( !g.isAggregate && !g.isAggregated && g.GruppTyp==GruppTyp.GruppNormal )
						listNormal.Add( g );

			this.DialogResult = FSlåSammanGrupper.showDialog(
				this.Owner,
				listNormal,
				out listValda,
				out strNewName );
			if ( this.DialogResult == DialogResult.OK )
			{
				_grupp = Global.Skola.Grupper.Add( strNewName, GruppTyp.GruppNormal );
				foreach ( Grupp g in listValda )
				{
					_grupp.addAggregatedGroup( g );
					foreach ( Person p in g.AllaPersoner )
					{
						Person p2 = _grupp.PersonerNärvarande.Add( p.Personal, p.getInfos() );
						p2.ScanCode = p.ScanCode;
						p2.ProtArchive = p.ProtArchive;
						p2.ProtGroup = p.ProtGroup;
						p2.ProtCatalog = p.ProtCatalog;
					}
				}
			}

		}

		private void cmdCopyGroup_Click( object sender, EventArgs e )
		{
			Grupp grpOrg = _grupp;
			this.DialogResult = GroupManagement.FNewOrRenameGroup.showDialog_Copy( this.Owner, grpOrg, out _grupp );
			if ( this.DialogResult == DialogResult.OK )
				foreach ( Person p in grpOrg.AllaPersoner )
				{
					Person p2 = _grupp.PersonerNärvarande.Add( p.Personal, p.getInfos() );
					p2.ID = p.ID;
					p2.ScanCode = p.ScanCode;
					p2.Kön = p.Kön;
					p2.ProtArchive = p.ProtArchive;
					p2.ProtGroup = p.ProtGroup;
					p2.ProtCatalog = p.ProtCatalog;
				}
		}

		private void cmdChangeName_Click( object sender, EventArgs e )
		{
			this.DialogResult = GroupManagement.FNewOrRenameGroup.showDialog_Rename( this.Owner, _grupp );
		}

		private void cmdNewGroup_Click( object sender, EventArgs e )
		{
			this.DialogResult = GroupManagement.FNewOrRenameGroup.showDialog_New( this.Owner, out _grupp );
		}

	}

}
