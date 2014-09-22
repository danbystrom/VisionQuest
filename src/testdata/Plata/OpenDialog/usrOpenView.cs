using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace Plata.OpenDialog
{
	/// <summary>
	/// Summary description for usrOpenPhotographers.
	/// </summary>
	public class usrOpenView : baseUsrTab
	{
		private static int _nLastOrderNumber = 0;

		private IContainer components;
        private System.Windows.Forms.Label label8;
		private Label label1;
		private TextBox txtInkommande;
		private ListBox lst;
		private System.Windows.Forms.TextBox txtOrderNr;

		public usrOpenView()
		{
			InitializeComponent();
			Text = "Granska";
			if ( _nLastOrderNumber != 0 )
				txtOrderNr.Text = _nLastOrderNumber.ToString();
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.txtOrderNr = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtInkommande = new System.Windows.Forms.TextBox();
            this.lst = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // txtOrderNr
            // 
            this.txtOrderNr.Location = new System.Drawing.Point(13, 32);
            this.txtOrderNr.Name = "txtOrderNr";
            this.txtOrderNr.Size = new System.Drawing.Size(56, 20);
            this.txtOrderNr.TabIndex = 0;
            this.txtOrderNr.Enter += new System.EventHandler(this.txtOrderNr_Enter);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 14);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 14);
            this.label8.TabIndex = 12;
            this.label8.Text = "Ordernr";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(252, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 14);
            this.label1.TabIndex = 15;
            this.label1.Text = "Inkommande:";
            // 
            // txtInkommande
            // 
            this.txtInkommande.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInkommande.BackColor = System.Drawing.SystemColors.Control;
            this.txtInkommande.Location = new System.Drawing.Point(325, 29);
            this.txtInkommande.Name = "txtInkommande";
            this.txtInkommande.Size = new System.Drawing.Size(270, 20);
            this.txtInkommande.TabIndex = 14;
            this.txtInkommande.TabStop = false;
            this.txtInkommande.Text = "\\\\phmsrv07\\ftpjobb$\\complete";
            // 
            // lst
            // 
            this.lst.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lst.FormattingEnabled = true;
            this.lst.ItemHeight = 14;
            this.lst.Location = new System.Drawing.Point(3, 58);
            this.lst.Name = "lst";
            this.lst.Size = new System.Drawing.Size(592, 130);
            this.lst.TabIndex = 16;
            this.lst.DoubleClick += new System.EventHandler(this.lst_DoubleClick);
            this.lst.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.lst_Format);
            // 
            // usrOpenView
            // 
            this.Controls.Add(this.lst);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtInkommande);
            this.Controls.Add(this.txtOrderNr);
            this.Controls.Add(this.label8);
            this.Name = "usrOpenView";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		public override void activate()
		{
			base.activate();
			txtOrderNr.Focus();
			if ( txtOrderNr.Text.Length != 0 && lst.Items.Count == 0 )
				viewPhotoWorkFolder();
		}

		public override bool openOrder( PlataDM.Skola skola )
		{
			var strPath = lst.SelectedItem as string;
			if ( strPath == null )
				return false;
			skola.Open( strPath );
			skola.ReadOnly = Form.ModifierKeys != Keys.Control;
			return true;
		}

		private void viewPhotoWorkFolder()
		{
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtOrderNr.Text, @"^\d+$"))
				return;
            var nOrderNr = int.Parse(txtOrderNr.Text);

			lst.Items.Clear();
			try
			{
				foreach ( var strFolder in Directory.GetDirectories( txtInkommande.Text, string.Format( "{0}_*", nOrderNr ) ) )
					lst.Items.Add( strFolder );
			}
			catch
			{
			}


			if ( lst.Items.Count != 0 )
			{
				_nLastOrderNumber = nOrderNr;
				lst.SelectedIndex = 0;
				lst.Focus();
			}
		}

		private void txtOrderNr_Enter(object sender, System.EventArgs e)
		{
			txtOrderNr.SelectAll();
		}

		public override bool gotKeyDown(KeyEventArgs e)
		{
			if ( e.KeyCode==Keys.Enter && txtOrderNr.Focused )
			{
				viewPhotoWorkFolder();
				return true;
			}
			return false;
		}

		private void lst_DoubleClick( object sender, EventArgs e )
		{
			fireExecute();
		}

		private void lst_Format( object sender, ListControlConvertEventArgs e )
		{
			e.Value = Path.GetFileName( e.ListItem as string );
		}

	}

}
