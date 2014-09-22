using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Photomic.Common;
using PlataDM;
using Xceed.Grid;

namespace Plata
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class FSlåSammanGrupper : vdUsr.baseGradientForm
	{
		private Button cmdCancel;
		private Button cmdOK;
		private CheckedListBox lst;
		private Label label1;
		private TextBox txtNamn;
		private Label label2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private FSlåSammanGrupper()
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
			this.lst = new System.Windows.Forms.CheckedListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtNamn = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point( 211, 102 );
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size( 80, 28 );
			this.cmdCancel.TabIndex = 5;
			this.cmdCancel.Text = "Avbryt";
			// 
			// cmdOK
			// 
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point( 211, 68 );
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size( 80, 28 );
			this.cmdOK.TabIndex = 4;
			this.cmdOK.Text = "OK";
			this.cmdOK.Click += new System.EventHandler( this.cmdOK_Click );
			// 
			// lst
			// 
			this.lst.CheckOnClick = true;
			this.lst.FormattingEnabled = true;
			this.lst.Location = new System.Drawing.Point( 15, 68 );
			this.lst.Name = "lst";
			this.lst.Size = new System.Drawing.Size( 190, 454 );
			this.lst.Sorted = true;
			this.lst.TabIndex = 3;
			this.lst.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler( this.lst_ItemCheck );
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label1.Location = new System.Drawing.Point( 12, 9 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 167, 13 );
			this.label1.TabIndex = 0;
			this.label1.Text = "&Namn på ny sammanslagen grupp";
			// 
			// txtNamn
			// 
			this.txtNamn.Location = new System.Drawing.Point( 12, 25 );
			this.txtNamn.Name = "txtNamn";
			this.txtNamn.Size = new System.Drawing.Size( 279, 20 );
			this.txtNamn.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label2.Location = new System.Drawing.Point( 12, 51 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 110, 13 );
			this.label2.TabIndex = 2;
			this.label2.Text = "&Grupper som ska ingå";
			// 
			// FSlåSammanGrupper
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size( 305, 534 );
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.txtNamn );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.lst );
			this.Controls.Add( this.cmdCancel );
			this.Controls.Add( this.cmdOK );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FSlåSammanGrupper";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Ny sammanslagen grupp";
			this.ResumeLayout( false );
			this.PerformLayout();

		}
		#endregion

		public static DialogResult showDialog(
			Form parent,
			IList grupper,
			out IList valdaGrupper,
			out string strNamn )
		{
			using ( FSlåSammanGrupper dlg = new FSlåSammanGrupper() )
			{
				foreach ( Grupp grupp in grupper )
					if ( grupp.GruppTyp==GruppTyp.GruppNormal && !grupp.isAggregate && !grupp.isAggregated )
					dlg.lst.Items.Add( grupp );
				if ( dlg.ShowDialog( parent ) == DialogResult.OK )
				{
					valdaGrupper = new ArrayList( dlg.lst.CheckedItems );
					strNamn = dlg.txtNamn.Text;
					return DialogResult.OK;
				}
				valdaGrupper = null;
				strNamn = null;
				return DialogResult.Cancel;
			}
		}

		private void lst_ItemCheck( object sender, ItemCheckEventArgs e )
		{
			string strNamn = string.Empty;
			ArrayList al = new ArrayList( lst.CheckedItems );
			try
			{
				if ( e.NewValue == CheckState.Checked )
					al.Add( lst.Items[e.Index] );
				else
					al.Remove( lst.Items[e.Index] );
			}
			catch
			{
			}
			foreach ( Grupp g in al )
			{
				if ( strNamn.Length != 0 )
					strNamn += " + ";
				strNamn += g.Namn;
			}
			txtNamn.Text = strNamn;
		}

		private void cmdOK_Click( object sender, EventArgs e )
		{
			if ( lst.CheckedItems.Count <= 1 || txtNamn.Text.Length < 2 )
				this.DialogResult = DialogResult.None;
		}

	}

}
