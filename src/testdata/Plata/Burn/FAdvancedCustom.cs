using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Photomic.ArchiveStuff;
using Photomic.ArchiveStuff.Core;
using Photomic.Common;

namespace Plata.Burn
{
	public class FAdvancedCustom : vdUsr.baseGradientForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.NumericUpDown numMaxWidth;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button cmdUpdate;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.ListView lv;
		private System.Windows.Forms.RadioButton optJPG;
		private System.Windows.Forms.TextBox txtTemplate;
		private System.Windows.Forms.CheckBox chkWWW;
		private System.Windows.Forms.Button cmdQ;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.RadioButton optBMP;
		private System.Windows.Forms.Button cmdAdd;
		private System.Windows.Forms.Button cmdDelete;
		private System.Windows.Forms.Button cmdOK;

		private List<CustomBurnInfo> _alCustom;
		private System.Windows.Forms.RadioButton optTIF;
		private System.Windows.Forms.RadioButton optRAW;
		private System.Windows.Forms.NumericUpDown numMaxHeight;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private Label label5;
		private PhotoType _photoType;

		private FAdvancedCustom()
		{
			InitializeComponent();
		}

		private FAdvancedCustom(
			List<CustomBurnInfo> alCustom,
			PhotoType photoType )
		{
			InitializeComponent();
			_alCustom = alCustom;
			_photoType = photoType;
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
			this.lv = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.numMaxHeight = new System.Windows.Forms.NumericUpDown();
			this.optRAW = new System.Windows.Forms.RadioButton();
			this.optTIF = new System.Windows.Forms.RadioButton();
			this.cmdQ = new System.Windows.Forms.Button();
			this.cmdUpdate = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.numMaxWidth = new System.Windows.Forms.NumericUpDown();
			this.chkWWW = new System.Windows.Forms.CheckBox();
			this.txtTemplate = new System.Windows.Forms.TextBox();
			this.optBMP = new System.Windows.Forms.RadioButton();
			this.optJPG = new System.Windows.Forms.RadioButton();
			this.cmdAdd = new System.Windows.Forms.Button();
			this.cmdDelete = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numMaxHeight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numMaxWidth)).BeginInit();
			this.SuspendLayout();
			// 
			// lv
			// 
			this.lv.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5} );
			this.lv.FullRowSelect = true;
			this.lv.HideSelection = false;
			this.lv.Location = new System.Drawing.Point( 8, 8 );
			this.lv.Name = "lv";
			this.lv.Size = new System.Drawing.Size( 472, 156 );
			this.lv.TabIndex = 0;
			this.lv.UseCompatibleStateImageBehavior = false;
			this.lv.View = System.Windows.Forms.View.Details;
			this.lv.SelectedIndexChanged += new System.EventHandler( this.lv_SelectedIndexChanged );
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Filnamnsmall";
			this.columnHeader1.Width = 220;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Max bredd";
			this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.columnHeader2.Width = 65;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Max höjd";
			this.columnHeader3.Width = 65;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Filformat";
			this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "www";
			this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader5.Width = 40;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.groupBox1.Controls.Add( this.label5 );
			this.groupBox1.Controls.Add( this.label4 );
			this.groupBox1.Controls.Add( this.numMaxHeight );
			this.groupBox1.Controls.Add( this.optRAW );
			this.groupBox1.Controls.Add( this.optTIF );
			this.groupBox1.Controls.Add( this.cmdQ );
			this.groupBox1.Controls.Add( this.cmdUpdate );
			this.groupBox1.Controls.Add( this.label3 );
			this.groupBox1.Controls.Add( this.label2 );
			this.groupBox1.Controls.Add( this.label1 );
			this.groupBox1.Controls.Add( this.numMaxWidth );
			this.groupBox1.Controls.Add( this.chkWWW );
			this.groupBox1.Controls.Add( this.txtTemplate );
			this.groupBox1.Controls.Add( this.optBMP );
			this.groupBox1.Controls.Add( this.optJPG );
			this.groupBox1.Location = new System.Drawing.Point( 12, 176 );
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size( 468, 119 );
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Parametrar";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point( 164, 76 );
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size( 12, 13 );
			this.label4.TabIndex = 13;
			this.label4.Text = "/";
			// 
			// numMaxHeight
			// 
			this.numMaxHeight.Location = new System.Drawing.Point( 176, 72 );
			this.numMaxHeight.Maximum = new decimal( new int[] {
            2048,
            0,
            0,
            0} );
			this.numMaxHeight.Name = "numMaxHeight";
			this.numMaxHeight.Size = new System.Drawing.Size( 56, 20 );
			this.numMaxHeight.TabIndex = 12;
			// 
			// optRAW
			// 
			this.optRAW.Location = new System.Drawing.Point( 276, 48 );
			this.optRAW.Name = "optRAW";
			this.optRAW.Size = new System.Drawing.Size( 52, 16 );
			this.optRAW.TabIndex = 7;
			this.optRAW.Text = "RAW";
			// 
			// optTIF
			// 
			this.optTIF.Location = new System.Drawing.Point( 220, 48 );
			this.optTIF.Name = "optTIF";
			this.optTIF.Size = new System.Drawing.Size( 52, 16 );
			this.optTIF.TabIndex = 6;
			this.optTIF.Text = "TIF";
			// 
			// cmdQ
			// 
			this.cmdQ.BackColor = System.Drawing.SystemColors.Control;
			this.cmdQ.Location = new System.Drawing.Point( 436, 16 );
			this.cmdQ.Name = "cmdQ";
			this.cmdQ.Size = new System.Drawing.Size( 24, 20 );
			this.cmdQ.TabIndex = 2;
			this.cmdQ.Text = "?";
			this.cmdQ.UseVisualStyleBackColor = false;
			this.cmdQ.Click += new System.EventHandler( this.cmdQ_Click );
			// 
			// cmdUpdate
			// 
			this.cmdUpdate.BackColor = System.Drawing.SystemColors.Control;
			this.cmdUpdate.Location = new System.Drawing.Point( 376, 56 );
			this.cmdUpdate.Name = "cmdUpdate";
			this.cmdUpdate.Size = new System.Drawing.Size( 84, 36 );
			this.cmdUpdate.TabIndex = 11;
			this.cmdUpdate.Text = "Uppdatera";
			this.cmdUpdate.UseVisualStyleBackColor = false;
			this.cmdUpdate.Click += new System.EventHandler( this.cmdUpdate_Click );
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point( 8, 48 );
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size( 46, 13 );
			this.label3.TabIndex = 3;
			this.label3.Text = "Filformat";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point( 8, 76 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 82, 13 );
			this.label2.TabIndex = 8;
			this.label2.Text = "Max bredd/höjd";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 8, 20 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 66, 13 );
			this.label1.TabIndex = 0;
			this.label1.Text = "Filnamnsmall";
			// 
			// numMaxWidth
			// 
			this.numMaxWidth.Location = new System.Drawing.Point( 108, 72 );
			this.numMaxWidth.Maximum = new decimal( new int[] {
            2048,
            0,
            0,
            0} );
			this.numMaxWidth.Name = "numMaxWidth";
			this.numMaxWidth.Size = new System.Drawing.Size( 56, 20 );
			this.numMaxWidth.TabIndex = 9;
			// 
			// chkWWW
			// 
			this.chkWWW.Location = new System.Drawing.Point( 248, 76 );
			this.chkWWW.Name = "chkWWW";
			this.chkWWW.Size = new System.Drawing.Size( 124, 16 );
			this.chkWWW.TabIndex = 10;
			this.chkWWW.Text = "www.photomic.com";
			// 
			// txtTemplate
			// 
			this.txtTemplate.Location = new System.Drawing.Point( 108, 16 );
			this.txtTemplate.Name = "txtTemplate";
			this.txtTemplate.Size = new System.Drawing.Size( 324, 20 );
			this.txtTemplate.TabIndex = 1;
			// 
			// optBMP
			// 
			this.optBMP.Location = new System.Drawing.Point( 164, 48 );
			this.optBMP.Name = "optBMP";
			this.optBMP.Size = new System.Drawing.Size( 52, 16 );
			this.optBMP.TabIndex = 5;
			this.optBMP.Text = "BMP";
			// 
			// optJPG
			// 
			this.optJPG.Location = new System.Drawing.Point( 108, 48 );
			this.optJPG.Name = "optJPG";
			this.optJPG.Size = new System.Drawing.Size( 52, 16 );
			this.optJPG.TabIndex = 4;
			this.optJPG.Text = "JPG";
			// 
			// cmdAdd
			// 
			this.cmdAdd.Location = new System.Drawing.Point( 488, 12 );
			this.cmdAdd.Name = "cmdAdd";
			this.cmdAdd.Size = new System.Drawing.Size( 80, 36 );
			this.cmdAdd.TabIndex = 2;
			this.cmdAdd.Text = "Lägg till nytt format";
			this.cmdAdd.Click += new System.EventHandler( this.cmdAdd_Click );
			// 
			// cmdDelete
			// 
			this.cmdDelete.Location = new System.Drawing.Point( 488, 56 );
			this.cmdDelete.Name = "cmdDelete";
			this.cmdDelete.Size = new System.Drawing.Size( 80, 36 );
			this.cmdDelete.TabIndex = 3;
			this.cmdDelete.Text = "Radera format";
			this.cmdDelete.Click += new System.EventHandler( this.cmdDelete_Click );
			// 
			// cmdOK
			// 
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point( 488, 231 );
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size( 80, 28 );
			this.cmdOK.TabIndex = 4;
			this.cmdOK.Text = "OK";
			// 
			// button3
			// 
			this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button3.Location = new System.Drawing.Point( 488, 267 );
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size( 80, 28 );
			this.button3.TabIndex = 5;
			this.button3.Text = "Avbryt";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point( 105, 95 );
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size( 293, 13 );
			this.label5.TabIndex = 14;
			this.label5.Text = "Sätt både bredd och höjd till noll för att kopiera originalbilden.";
			// 
			// FAdvancedCustom
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.ClientSize = new System.Drawing.Size( 578, 307 );
			this.Controls.Add( this.button3 );
			this.Controls.Add( this.cmdOK );
			this.Controls.Add( this.cmdDelete );
			this.Controls.Add( this.cmdAdd );
			this.Controls.Add( this.groupBox1 );
			this.Controls.Add( this.lv );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FAdvancedCustom";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Anpassade inställningar för bildnamn och bildformat";
			this.groupBox1.ResumeLayout( false );
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numMaxHeight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numMaxWidth)).EndInit();
			this.ResumeLayout( false );

		}
		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			foreach ( var cbi in _alCustom )
				addLine( (CustomBurnInfo)cbi.Clone() );
			lv.Items[0].Selected = true;
			switch ( _photoType )
			{
				case PhotoType.Group:
					optRAW.Enabled = true;
					break;
				case PhotoType.Portrait:
					optRAW.Enabled = Global.Skola.CompanyOrder == PlataDM.CompanyOrder.SmallImage;
					break;
				case PhotoType.Vimmel:
					optRAW.Enabled = false;
					break;
			}
		}

		private ListViewItem addLine( CustomBurnInfo cbi )
		{
			var lvi = new ListViewItem( cbi.CustomName );
			lvi.SubItems.Add( cbi.MaxWidth.ToString() );
			lvi.SubItems.Add( cbi.MaxHeight.ToString() );
			lvi.SubItems.Add( cbi.imageFormatName );
			lvi.SubItems.Add( cbi.PhotomicWww ? "X" : "" );
			lvi.Tag = cbi;
			lv.Items.Add( lvi );
			return lvi;
		}

		public static DialogResult showDialog(
			Form parent,
			List<CustomBurnInfo> alCustom,
			PhotoType photoType )
		{
		    using (var dlg = new FAdvancedCustom(alCustom, photoType))
		        if (dlg.ShowDialog(parent) == DialogResult.OK)
		        {
		            alCustom.Clear();
		            alCustom.AddRange(from ListViewItem lvi in dlg.lv.Items select (CustomBurnInfo) lvi.Tag);
		            return DialogResult.OK;
		        }
		        else
		            return DialogResult.Cancel;
		}

	    private void lv_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if ( lv.SelectedItems.Count!=1 )
				return;
			var cbi = lv.SelectedItems[0].Tag as CustomBurnInfo;
			if ( cbi==null )
				return;
			txtTemplate.Text = cbi.CustomName;
			numMaxWidth.Value = cbi.MaxWidth;
			numMaxHeight.Value = cbi.MaxHeight;
			switch ( cbi.FormatType )
			{
				case ImageFileFormat.BMP:
					optBMP.Checked = true;
					break;
				case ImageFileFormat.JPG:
					optJPG.Checked = true;
					break;
				case ImageFileFormat.TIF:
					optTIF.Checked = true;
					break;
				case ImageFileFormat.RAW:
					optRAW.Checked = true;
					break;
			}
			chkWWW.Checked = cbi.PhotomicWww;
		}

		private void cmdAdd_Click(object sender, System.EventArgs e)
		{
			var cbi = new CustomBurnInfo(
                ImageFileFormat.JPG,
                "",
				(_photoType == PhotoType.Portrait) ? 256 : 768,
				0,
				true, _photoType );
			lv.SelectedItems.Clear();
			addLine( cbi ).Selected = true;
			txtTemplate.Focus();
		}

		private void cmdUpdate_Click(object sender, System.EventArgs e)
		{
			var lvi = lv.SelectedItems[0];
			var cbi = lvi.Tag as CustomBurnInfo;
			cbi.CustomName = txtTemplate.Text;
			cbi.MaxWidth = (int)numMaxWidth.Value;
			cbi.MaxHeight = (int)numMaxHeight.Value;
			if ( optBMP.Checked )
				cbi.FormatType = ImageFileFormat.BMP;
			else if ( optJPG.Checked )
				cbi.FormatType = ImageFileFormat.JPG;
			else if ( optTIF.Checked )
				cbi.FormatType = ImageFileFormat.TIF;
			else if ( optRAW.Checked )
				cbi.FormatType = ImageFileFormat.RAW;
			cbi.PhotomicWww = chkWWW.Checked;
			lvi.Text = cbi.CustomName;
			lvi.SubItems[1].Text = cbi.MaxWidth.ToString();
			lvi.SubItems[2].Text = cbi.MaxHeight.ToString();
			lvi.SubItems[3].Text = cbi.imageFormatName;
			lvi.SubItems[4].Text = cbi.PhotomicWww ? "X" : "";
		}

		private void cmdDelete_Click(object sender, System.EventArgs e)
		{
			if ( lv.Items.Count<2 )
			{
				Global.showMsgBox( this, "Du måste lämna kvar minst ett format!" );
				return;
			}
			lv.Items.Remove( lv.SelectedItems[0] );
			lv.Items[0].Selected = true;
		}

		private void cmdQ_Click(object sender, System.EventArgs e)
		{
			Global.showMsgBox( this,
				"?E = efternamn\r\n" +
				"?F = förnamn\r\n" +
				"?G = gruppnamn\r\n" +
				"?K = IST-kod\r\n" +
				"?L = ett löpnummer\r\n" +
				"?N = efternamn, förnamn\r\n" +
				"?S = kundnummer\r\n" +
				"?P = personnummer\r\n" +
				"?T = titel\r\n" );
		}

	}

}
