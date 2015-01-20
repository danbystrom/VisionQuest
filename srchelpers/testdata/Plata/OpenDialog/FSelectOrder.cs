using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Plata.OpenDialog
{
	/// <summary>
	/// Summary description for FSelectPerson.
	/// </summary>
	public class FSelectOrder : vdUsr.baseGradientForm
	{
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
		private Label label1;
		private ListBox lst;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private FSelectOrder()
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
			this.label1 = new System.Windows.Forms.Label();
			this.lst = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// cmdOK
			// 
			this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Enabled = false;
			this.cmdOK.Location = new System.Drawing.Point( 38, 249 );
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size( 80, 28 );
			this.cmdOK.TabIndex = 1;
			this.cmdOK.Text = "OK";
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point( 134, 249 );
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size( 80, 28 );
			this.cmdCancel.TabIndex = 2;
			this.cmdCancel.Text = "Avbryt";
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label1.Location = new System.Drawing.Point( 12, 9 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 228, 33 );
			this.label1.TabIndex = 3;
			this.label1.Text = "Det finns mer än ett orderunderlag på den angivna platsen. Välj vilket du vill hä" +
					"mta:";
			// 
			// lst
			// 
			this.lst.FormattingEnabled = true;
			this.lst.Location = new System.Drawing.Point( 15, 45 );
			this.lst.Name = "lst";
			this.lst.Size = new System.Drawing.Size( 225, 186 );
			this.lst.Sorted = true;
			this.lst.TabIndex = 4;
			this.lst.DoubleClick += new System.EventHandler( this.lst_DoubleClick );
			this.lst.SelectedIndexChanged += new System.EventHandler( this.lst_SelectedIndexChanged );
			this.lst.Format += new System.Windows.Forms.ListControlConvertEventHandler( this.lst_Format );
			// 
			// FSelectOrder
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size( 252, 289 );
			this.Controls.Add( this.lst );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.cmdCancel );
			this.Controls.Add( this.cmdOK );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FSelectOrder";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Välj order";
			this.ResumeLayout( false );

		}
		#endregion

		public static DialogResult showDialog(
			Form parent,
			IList listFilnamn,
			out string strValdFil )
		{
			using ( FSelectOrder dlg = new FSelectOrder() )
			{
				foreach ( string s in listFilnamn )
					dlg.lst.Items.Add( s );
				DialogResult retVal = dlg.ShowDialog(parent);
				strValdFil = dlg.lst.SelectedItem as string;
				return retVal;
			}
		}

		private void lst_SelectedIndexChanged( object sender, EventArgs e )
		{
			cmdOK.Enabled = lst.SelectedItem != null;
		}

		private void lst_DoubleClick( object sender, EventArgs e )
		{
			if ( cmdOK.Enabled )
				cmdOK.PerformClick();
		}

		private void lst_Format( object sender, ListControlConvertEventArgs e )
		{
			string s = e.ListItem as string;
			e.Value = s.Substring(s.IndexOf("order_")+ 6 );
		}

	}

}
