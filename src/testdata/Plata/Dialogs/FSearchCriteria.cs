using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Plata
{
	/// <summary>
	/// Summary description for FPassword.
	/// </summary>
	public class FSearchCriteria : vdUsr.baseGradientForm
	{
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.TextBox txtF�rnamn;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtEfternamn;
		private System.Windows.Forms.Label label4;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private static int s_S�kGruppIndex = 0;
		private static int s_S�kPersonIndex = 0;

		private static string s_S�kF�rnamn = string.Empty;
		private static string s_S�kEfternamn = string.Empty;
		private static string s_S�kBlippkod= string.Empty;
		private Label label1;
		private TextBox txtBlippkod;

		private FSearchCriteria()
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
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.txtF�rnamn = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtEfternamn = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtBlippkod = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point( 300, 104 );
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size( 80, 28 );
			this.cmdCancel.TabIndex = 8;
			this.cmdCancel.Text = "Avbryt";
			// 
			// cmdOK
			// 
			this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdOK.Location = new System.Drawing.Point( 208, 104 );
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size( 80, 28 );
			this.cmdOK.TabIndex = 7;
			this.cmdOK.Text = "S�k";
			this.cmdOK.Click += new System.EventHandler( this.cmdOK_Click );
			// 
			// txtF�rnamn
			// 
			this.txtF�rnamn.Location = new System.Drawing.Point( 12, 24 );
			this.txtF�rnamn.Name = "txtF�rnamn";
			this.txtF�rnamn.Size = new System.Drawing.Size( 108, 20 );
			this.txtF�rnamn.TabIndex = 1;
			this.txtF�rnamn.Enter += new System.EventHandler( this.txtF�rnamn_Enter );
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label2.Location = new System.Drawing.Point( 12, 6 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 48, 13 );
			this.label2.TabIndex = 0;
			this.label2.Text = "&F�rnamn";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label3.Location = new System.Drawing.Point( 12, 50 );
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size( 55, 13 );
			this.label3.TabIndex = 2;
			this.label3.Text = "&Efternamn";
			// 
			// txtEfternamn
			// 
			this.txtEfternamn.Location = new System.Drawing.Point( 12, 68 );
			this.txtEfternamn.Name = "txtEfternamn";
			this.txtEfternamn.Size = new System.Drawing.Size( 108, 20 );
			this.txtEfternamn.TabIndex = 3;
			this.txtEfternamn.Enter += new System.EventHandler( this.txtEfternamn_Enter );
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label4.Location = new System.Drawing.Point( 200, 8 );
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size( 188, 72 );
			this.label4.TabIndex = 6;
			this.label4.Text = "Fyll i ett eller flera av f�lten. Det g�r ocks� bra att s�ka p� b�rjan av namn - " +
					"du beh�ver inte skriva ut hela.";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label1.Location = new System.Drawing.Point( 12, 94 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 50, 13 );
			this.label1.TabIndex = 4;
			this.label1.Text = "&Kundnr";
			// 
			// txtBlippkod
			// 
			this.txtBlippkod.Location = new System.Drawing.Point( 12, 112 );
			this.txtBlippkod.Name = "txtBlippkod";
			this.txtBlippkod.Size = new System.Drawing.Size( 108, 20 );
			this.txtBlippkod.TabIndex = 5;
			this.txtBlippkod.Enter += new System.EventHandler( this.txtBlippkod_Enter );
			// 
			// FSearchCriteria
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size( 390, 140 );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.txtBlippkod );
			this.Controls.Add( this.label4 );
			this.Controls.Add( this.label3 );
			this.Controls.Add( this.txtEfternamn );
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.txtF�rnamn );
			this.Controls.Add( this.cmdCancel );
			this.Controls.Add( this.cmdOK );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FSearchCriteria";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Ange s�kkriterier";
			this.ResumeLayout( false );
			this.PerformLayout();

		}
		#endregion

		public static bool canSearch
		{
			get
			{
				if ( s_S�kBlippkod.Length != 0 )
					return Util.verifyScanCode( s_S�kBlippkod );
				return s_S�kF�rnamn.Length!=0 || s_S�kEfternamn.Length!=0;
			}
		}

		static public void search( FMain parent )
		{
			using ( FSearchCriteria dlg = new FSearchCriteria() )
				dlg.ShowDialog(parent);
		}

		static public bool searchNext( FMain parent )
		{
			if ( Global.Skola==null )
				return false;

			var grupper = Global.Skola.Grupper;
			int nS�kGruppIndex = s_S�kGruppIndex;
			int nS�kPersonIndex = s_S�kPersonIndex+1;
			for ( int nMaxIterations=grupper.Count ; nMaxIterations>=0 ; nMaxIterations-- )
			{
				if ( nS�kGruppIndex>=grupper.Count )
				{
					nS�kGruppIndex = 0;
					nS�kPersonIndex = 0;
				}
				var grupp = grupper[nS�kGruppIndex];

				var list = new System.Collections.Generic.List<PlataDM.Person>( grupp.AllaPersoner );

				for ( int i = nS�kPersonIndex ; i < list.Count ; i++ )
					if ( isMatch( list[i] ) )
					{
						s_S�kGruppIndex = nS�kGruppIndex;
						s_S�kPersonIndex = i;
						parent.jumpToForm_Group_Person( FlikTyp._S�kHopp, grupp, list[i] );
						return true;
					}

				nS�kGruppIndex++;
				nS�kPersonIndex = 0;
			}

			Global.showMsgBox( parent, "Ingen tr�ff!" );
			return false;
		}

		private static bool isMatch( PlataDM.Person person )
		{
			bool f2, f3;

			if ( s_S�kBlippkod.Length != 0 )
				return s_S�kBlippkod.CompareTo( person.ScanCode ) == 0;
			f2 = person.F�rnamn.Trim().ToUpper().StartsWith( s_S�kF�rnamn.ToUpper() );
			f3 = person.Efternamn.Trim().ToUpper().StartsWith( s_S�kEfternamn.ToUpper() );

			return f2 && f3;
		}

		private void txtF�rnamn_Enter(object sender, System.EventArgs e)
		{
			txtF�rnamn.SelectAll();
		}

		private void txtEfternamn_Enter(object sender, System.EventArgs e)
		{
			txtEfternamn.SelectAll();
		}

		private void txtBlippkod_Enter( object sender, EventArgs e )
		{
			txtBlippkod.SelectAll();
		}

		private void cmdOK_Click( object sender, System.EventArgs e )
		{
			s_S�kF�rnamn = txtF�rnamn.Text.Trim();
			s_S�kEfternamn = txtEfternamn.Text.Trim();
			s_S�kBlippkod = txtBlippkod.Text.Trim();

			if ( !canSearch )
				return;

			s_S�kGruppIndex = 0;
			s_S�kPersonIndex = -1;

			if ( searchNext( this.Owner as FMain ) )
				this.DialogResult = DialogResult.OK;

		}

	}

}
