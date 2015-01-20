using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Photomic.Common;
using PlataDM;

namespace Plata
{

	public class FKopieraGrupp : vdUsr.baseGradientForm
	{
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.TextBox txt;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private RadioButton optManual;
		private RadioButton optSkyddad;
		private RadioButton optPloj;
		private RadioButton optKopia;

		private Grupp _grupp;

		private FKopieraGrupp()
		{
			InitializeComponent();
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
			this.optManual = new System.Windows.Forms.RadioButton();
			this.optSkyddad = new System.Windows.Forms.RadioButton();
			this.optPloj = new System.Windows.Forms.RadioButton();
			this.optKopia = new System.Windows.Forms.RadioButton();
			this.SuspendLayout();
			// 
			// cmdOK
			// 
			this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdOK.Location = new System.Drawing.Point( 144, 66 );
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size( 80, 28 );
			this.cmdOK.TabIndex = 1;
			this.cmdOK.Text = "OK";
			this.cmdOK.Click += new System.EventHandler( this.cmdOK_Click );
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point( 144, 100 );
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size( 80, 28 );
			this.cmdCancel.TabIndex = 2;
			this.cmdCancel.Text = "Avbryt";
			// 
			// txt
			// 
			this.txt.Location = new System.Drawing.Point( 35, 36 );
			this.txt.Name = "txt";
			this.txt.Size = new System.Drawing.Size( 189, 20 );
			this.txt.TabIndex = 0;
			this.txt.TextChanged += new System.EventHandler( this.txt_TextChanged );
			// 
			// optManual
			// 
			this.optManual.AutoSize = true;
			this.optManual.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.optManual.Checked = true;
			this.optManual.Location = new System.Drawing.Point( 16, 13 );
			this.optManual.Name = "optManual";
			this.optManual.Size = new System.Drawing.Size( 73, 17 );
			this.optManual.TabIndex = 3;
			this.optManual.TabStop = true;
			this.optManual.Text = "Nytt namn";
			this.optManual.UseVisualStyleBackColor = false;
			this.optManual.CheckedChanged += new System.EventHandler( this.optManual_CheckedChanged );
			// 
			// optSkyddad
			// 
			this.optSkyddad.AutoSize = true;
			this.optSkyddad.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.optSkyddad.Location = new System.Drawing.Point( 16, 62 );
			this.optSkyddad.Name = "optSkyddad";
			this.optSkyddad.Size = new System.Drawing.Size( 81, 17 );
			this.optSkyddad.TabIndex = 4;
			this.optSkyddad.Text = "Skyddad ID";
			this.optSkyddad.UseVisualStyleBackColor = false;
			this.optSkyddad.CheckedChanged += new System.EventHandler( this.opt_CheckedChanged );
			// 
			// optPloj
			// 
			this.optPloj.AutoSize = true;
			this.optPloj.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.optPloj.Location = new System.Drawing.Point( 16, 85 );
			this.optPloj.Name = "optPloj";
			this.optPloj.Size = new System.Drawing.Size( 58, 17 );
			this.optPloj.TabIndex = 5;
			this.optPloj.Text = "Plojbild";
			this.optPloj.UseVisualStyleBackColor = false;
			this.optPloj.CheckedChanged += new System.EventHandler( this.opt_CheckedChanged );
			// 
			// optKopia
			// 
			this.optKopia.AutoSize = true;
			this.optKopia.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.optKopia.Location = new System.Drawing.Point( 16, 107 );
			this.optKopia.Name = "optKopia";
			this.optKopia.Size = new System.Drawing.Size( 52, 17 );
			this.optKopia.TabIndex = 6;
			this.optKopia.Text = "Kopia";
			this.optKopia.UseVisualStyleBackColor = false;
			this.optKopia.CheckedChanged += new System.EventHandler( this.opt_CheckedChanged );
			// 
			// FKopieraGrupp
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size( 243, 132 );
			this.Controls.Add( this.optKopia );
			this.Controls.Add( this.optPloj );
			this.Controls.Add( this.optSkyddad );
			this.Controls.Add( this.optManual );
			this.Controls.Add( this.txt );
			this.Controls.Add( this.cmdCancel );
			this.Controls.Add( this.cmdOK );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FKopieraGrupp";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Kopiera grupp";
			this.ResumeLayout( false );
			this.PerformLayout();

		}
		#endregion

		public static DialogResult showDialog(
			Form parent,
			Grupp grupp )
		{
			using ( FKopieraGrupp dlg = new FKopieraGrupp() )
			{
				dlg._grupp = grupp;
				return dlg.ShowDialog(parent);
			}
		}

		private void txt_TextChanged(object sender, System.EventArgs e)
		{
			if ( txt.Focused )
				optManual.Checked = true;
		}

		private void cmdOK_Click( object sender, EventArgs e )
		{
			string s = txt.Text.Trim();

			if ( s.Length == 0 )
				return;

			foreach ( Grupp grupp in _grupp.Skola.Grupper )
				if ( string.Compare( grupp.Namn, s, true ) == 0 )
				{
					Global.showMsgBox( this, "Det finns redan en grupp med det här namnet!" );
					return;
				}

			Grupp g = _grupp.Skola.Grupper.Add( s, GruppTyp.GruppNormal );
			foreach ( Person p in _grupp.AllaPersoner )
			{
				Person p2 = g.PersonerNärvarande.Add( p.Personal, p.getInfos() );
				p2.ProtArchive = p.ProtArchive;
				p2.ProtGroup = p.ProtGroup;
				p2.ProtCatalog = p.ProtCatalog;
			}

            g.Special = TypeOfGroupPhoto.Katalog;
			if ( optSkyddad.Checked )
                g.Special |= TypeOfGroupPhoto.SkyddadId;
			if ( optPloj.Checked )
                g.Special |= TypeOfGroupPhoto.Spex;

			this.DialogResult = DialogResult.OK;
		}

		private void opt_CheckedChanged( object sender, EventArgs e )
		{
			RadioButton opt = sender as RadioButton;
			if ( !opt.Checked )
				return;
			txt.Text = _grupp.Namn + " " + opt.Text;
		}

		private void optManual_CheckedChanged( object sender, EventArgs e )
		{
			if ( optManual.Checked )
				txt.Text = string.Empty;
		}

	}

}
