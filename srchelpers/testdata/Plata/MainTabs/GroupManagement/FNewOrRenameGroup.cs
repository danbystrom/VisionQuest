using System;
using System.Drawing;
using System.Windows.Forms;
using Photomic.Common;
using PlataDM;

namespace Plata.GroupManagement
{

	public class FNewOrRenameGroup : vdUsr.baseGradientForm
	{
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.TextBox txt;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private CheckBox chkGrupp;
		private CheckBox chkKatalog;
		private CheckBox chkPloj;
		private CheckBox chkSkyddadID;
		private GroupBox groupBox1;
		private GroupBox groupBox2;
		private GroupBox groupBox3;
		private RichTextBox rtf;

		private Grupp _grupp;

		private FNewOrRenameGroup()
		{
			InitializeComponent();
		}

		private FNewOrRenameGroup( Grupp grupp )
		{
			InitializeComponent();

			_grupp = grupp;
			if ( grupp != null )
			{
				txt.Text = grupp.Namn;
                chkGrupp.Checked = (grupp.Special & TypeOfGroupPhoto.Gruppbild) != TypeOfGroupPhoto.Ingen;
                chkKatalog.Checked = (grupp.Special & TypeOfGroupPhoto.Katalog) != TypeOfGroupPhoto.Ingen;
                chkPloj.Checked = (grupp.Special & TypeOfGroupPhoto.Spex) != TypeOfGroupPhoto.Ingen;
                chkSkyddadID.Checked = (grupp.Special & TypeOfGroupPhoto.SkyddadId) != TypeOfGroupPhoto.Ingen;
				if ( grupp.isAggregate )
					displayAggregation( grupp );
				else if ( grupp.isAggregated )
					displayAggregation( grupp.aggregate );
			}
			if ( rtf.Text.Length == 0 )
				rtf.Text = "Gruppen är inte sammanslagen";
		}

		private void displayAggregation( Grupp g )
		{
			appendName( g, false );
			foreach ( Grupp g2 in g.aggregatedGroups )
				appendName( g2, true );
		}

		private void appendName( Grupp g, bool fIndent )
		{
			if ( fIndent )
				rtf.AppendText( "  " );
			if ( g == _grupp )
			{
				rtf.SelectionFont = new Font( rtf.Font, FontStyle.Bold );
				rtf.AppendText( g.Namn );
				rtf.SelectionFont = rtf.Font;
			}
			else
				rtf.AppendText( g.Namn );
			rtf.AppendText( "\r\n" );
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
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.txt = new System.Windows.Forms.TextBox();
			this.chkGrupp = new System.Windows.Forms.CheckBox();
			this.chkKatalog = new System.Windows.Forms.CheckBox();
			this.chkPloj = new System.Windows.Forms.CheckBox();
			this.chkSkyddadID = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.rtf = new System.Windows.Forms.RichTextBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmdOK
			// 
			this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdOK.Location = new System.Drawing.Point( 246, 12 );
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size( 80, 28 );
			this.cmdOK.TabIndex = 2;
			this.cmdOK.Text = "OK";
			this.cmdOK.Click += new System.EventHandler( this.cmdOK_Click );
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point( 246, 46 );
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size( 80, 28 );
			this.cmdCancel.TabIndex = 3;
			this.cmdCancel.Text = "Avbryt";
			// 
			// txt
			// 
			this.txt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txt.Location = new System.Drawing.Point( 6, 17 );
			this.txt.Name = "txt";
			this.txt.Size = new System.Drawing.Size( 208, 20 );
			this.txt.TabIndex = 0;
			// 
			// chkGrupp
			// 
			this.chkGrupp.AutoSize = true;
			this.chkGrupp.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.chkGrupp.Location = new System.Drawing.Point( 6, 19 );
			this.chkGrupp.Name = "chkGrupp";
			this.chkGrupp.Size = new System.Drawing.Size( 71, 17 );
			this.chkGrupp.TabIndex = 0;
			this.chkGrupp.Text = "&Gruppbild";
			this.chkGrupp.UseVisualStyleBackColor = false;
			// 
			// chkKatalog
			// 
			this.chkKatalog.AutoSize = true;
			this.chkKatalog.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.chkKatalog.Checked = true;
			this.chkKatalog.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkKatalog.Location = new System.Drawing.Point( 6, 42 );
			this.chkKatalog.Name = "chkKatalog";
			this.chkKatalog.Size = new System.Drawing.Size( 62, 17 );
			this.chkKatalog.TabIndex = 1;
			this.chkKatalog.Text = "&Katalog";
			this.chkKatalog.UseVisualStyleBackColor = false;
			// 
			// chkPloj
			// 
			this.chkPloj.AutoSize = true;
			this.chkPloj.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.chkPloj.Location = new System.Drawing.Point( 120, 19 );
			this.chkPloj.Name = "chkPloj";
			this.chkPloj.Size = new System.Drawing.Size( 43, 17 );
			this.chkPloj.TabIndex = 2;
			this.chkPloj.Text = "&Ploj";
			this.chkPloj.UseVisualStyleBackColor = false;
			// 
			// chkSkyddadID
			// 
			this.chkSkyddadID.AutoSize = true;
			this.chkSkyddadID.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.chkSkyddadID.Location = new System.Drawing.Point( 120, 42 );
			this.chkSkyddadID.Name = "chkSkyddadID";
			this.chkSkyddadID.Size = new System.Drawing.Size( 82, 17 );
			this.chkSkyddadID.TabIndex = 3;
			this.chkSkyddadID.Text = "&Skyddad ID";
			this.chkSkyddadID.UseVisualStyleBackColor = false;
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.groupBox1.Controls.Add( this.txt );
			this.groupBox1.Location = new System.Drawing.Point( 12, 12 );
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size( 220, 47 );
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Grupp&namn";
			// 
			// groupBox2
			// 
			this.groupBox2.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.groupBox2.Controls.Add( this.chkGrupp );
			this.groupBox2.Controls.Add( this.chkKatalog );
			this.groupBox2.Controls.Add( this.chkSkyddadID );
			this.groupBox2.Controls.Add( this.chkPloj );
			this.groupBox2.Location = new System.Drawing.Point( 12, 65 );
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size( 220, 65 );
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Bilden ska användas till...";
			// 
			// groupBox3
			// 
			this.groupBox3.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.groupBox3.Controls.Add( this.rtf );
			this.groupBox3.Location = new System.Drawing.Point( 12, 136 );
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size( 314, 93 );
			this.groupBox3.TabIndex = 4;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Sammanslagen med";
			// 
			// rtf
			// 
			this.rtf.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rtf.Location = new System.Drawing.Point( 6, 19 );
			this.rtf.Name = "rtf";
			this.rtf.ReadOnly = true;
			this.rtf.Size = new System.Drawing.Size( 302, 68 );
			this.rtf.TabIndex = 0;
			this.rtf.Text = "";
			// 
			// FNewOrRenameGroup
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size( 338, 241 );
			this.Controls.Add( this.groupBox3 );
			this.Controls.Add( this.groupBox2 );
			this.Controls.Add( this.groupBox1 );
			this.Controls.Add( this.cmdCancel );
			this.Controls.Add( this.cmdOK );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FNewOrRenameGroup";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Lägg till ny grupp";
			this.groupBox1.ResumeLayout( false );
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout( false );
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout( false );
			this.ResumeLayout( false );

		}
		#endregion

		public static DialogResult showDialog_Rename(
			Form parent,
			Grupp grupp )
		{
			using ( FNewOrRenameGroup dlg = new FNewOrRenameGroup( grupp ) )
			{
				dlg.Text = "Byt namn på grupp";
				return dlg.ShowDialog( parent );
			}
		}

		public static DialogResult showDialog_New(
			Form parent,
			out Grupp grupp )
		{
			grupp = null;
			using ( FNewOrRenameGroup dlg = new FNewOrRenameGroup( grupp ) )
			{
				dlg.Text = "Skapa ny grupp";
				if ( dlg.ShowDialog( parent ) == DialogResult.OK )
				{
					grupp = dlg._grupp;
					return DialogResult.OK;
				}
				else
					return DialogResult.Cancel;
			}
		}

		public static DialogResult showDialog_Copy(
			Form parent,
			Grupp grpOrg,
			out Grupp grpNew )
		{
			grpNew = null;
			using ( FNewOrRenameGroup dlg = new FNewOrRenameGroup( grpOrg ) )
			{
				dlg.Text = "Kopiera grupp";
				dlg._grupp = null;
				if ( dlg.ShowDialog( parent ) == DialogResult.OK )
				{
					grpNew = dlg._grupp;
					return DialogResult.OK;
				}
				else
					return DialogResult.Cancel;
			}
		}

		private void cmdOK_Click( object sender, EventArgs e )
		{
			string s = txt.Text.Trim();

			if ( s.Length <= 1 && vdUsr.Util.safeParse(s) == 0 )
				return;

			foreach ( Grupp grupp in Global.Skola.Grupper )
				if ( (grupp!=_grupp || _grupp==null) && string.Compare( grupp.Namn, s, true ) == 0 )
				{
					Global.showMsgBox( this, "Det finns redan en grupp med det här namnet!" );
					return;
				}

            var gs = TypeOfGroupPhoto.Ingen;
			if ( chkGrupp.Checked )
                gs |= TypeOfGroupPhoto.Gruppbild;
			if ( chkKatalog.Checked )
                gs |= TypeOfGroupPhoto.Katalog;
			if ( chkPloj.Checked )
                gs |= TypeOfGroupPhoto.Spex;
			if ( chkSkyddadID.Checked )
                gs |= TypeOfGroupPhoto.SkyddadId;

			if ( _grupp == null )
				_grupp = Global.Skola.Grupper.Add( s, GruppTyp.GruppNormal );
			else
				_grupp.Namn = s;
			_grupp.Special = gs;

			this.DialogResult = DialogResult.OK;
		}

	}

}
