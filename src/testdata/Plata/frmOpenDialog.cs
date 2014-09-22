using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace Plata
{
	/// <summary>
	/// Summary description for frmOpenDialog.
	/// </summary>
	public class frmOpenDialog : Form
	{
		private Button cmdOK;
		private Button cmdEnd;
		private System.ComponentModel.IContainer components;

		private ListViewItemComparer m_sorter = new ListViewItemComparer(2,SortOrder.Descending);
		private ImageList iml;
		private TabPage tabPageFolder;
		private TabPage tabPageOrder;
		private TabPage tabPageNy;
		private ListView lv;
		private ColumnHeader chSkola;
		private ColumnHeader chOrderNr;
		private ColumnHeader chSkapad;
		private ColumnHeader chAndrad;
		private ColumnHeader chLevererans;
		private TextBox txtOrder;
		private Label label3;
		private TextBox txtOrt;
		private Label label2;
		private TextBox txtOrderNr;
		private Label label1;
		private TextBox txtNamn;
		private TabControl tabVal;
		private TabPage tabPageGranska;
		private Button cmdBrowse;
		private ComboBox cboGranska;
		private Label label4;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.TabPage tabPagePhotographers;
		private System.Windows.Forms.ComboBox cboPhotographers;
		private System.Windows.Forms.ListView lvPWorks;
		private System.Windows.Forms.ImageList imlSmall;
		private System.Windows.Forms.Label label5;

		private bool[] _fNewTextFieldsOK = new bool[2];
		private System.Windows.Forms.TextBox txtPhotographerFolder;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtViewOrderNr;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtPhotWorkFolder;
		private System.Windows.Forms.ListView lvPW;
		private System.Windows.Forms.Panel pnlBrowser;
		private System.Windows.Forms.CheckBox chkReadOnly;
		private System.Windows.Forms.TabPage tabPageCrasch;

		private static int s_nLastPhotograherSelected = 0;

		public frmOpenDialog()
		{
			InitializeComponent();

			lv.DoubleClick +=new EventHandler(lv_DoubleClick);
			lv.ColumnClick +=new ColumnClickEventHandler(lv_ColumnClick);
			lv.KeyUp +=new KeyEventHandler(lv_KeyUp);
			lv.ListViewItemSorter = m_sorter;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmOpenDialog));
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdEnd = new System.Windows.Forms.Button();
			this.tabVal = new System.Windows.Forms.TabControl();
			this.tabPageFolder = new System.Windows.Forms.TabPage();
			this.lv = new System.Windows.Forms.ListView();
			this.chSkola = new System.Windows.Forms.ColumnHeader();
			this.chOrderNr = new System.Windows.Forms.ColumnHeader();
			this.chSkapad = new System.Windows.Forms.ColumnHeader();
			this.chAndrad = new System.Windows.Forms.ColumnHeader();
			this.chLevererans = new System.Windows.Forms.ColumnHeader();
			this.tabPageOrder = new System.Windows.Forms.TabPage();
			this.label4 = new System.Windows.Forms.Label();
			this.txtOrder = new System.Windows.Forms.TextBox();
			this.tabPageNy = new System.Windows.Forms.TabPage();
			this.label3 = new System.Windows.Forms.Label();
			this.txtOrt = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtOrderNr = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtNamn = new System.Windows.Forms.TextBox();
			this.tabPageGranska = new System.Windows.Forms.TabPage();
			this.pnlBrowser = new System.Windows.Forms.Panel();
			this.chkReadOnly = new System.Windows.Forms.CheckBox();
			this.label8 = new System.Windows.Forms.Label();
			this.txtViewOrderNr = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.txtPhotWorkFolder = new System.Windows.Forms.TextBox();
			this.lvPW = new System.Windows.Forms.ListView();
			this.imlSmall = new System.Windows.Forms.ImageList(this.components);
			this.label6 = new System.Windows.Forms.Label();
			this.cmdBrowse = new System.Windows.Forms.Button();
			this.cboGranska = new System.Windows.Forms.ComboBox();
			this.tabPageCrasch = new System.Windows.Forms.TabPage();
			this.tabPagePhotographers = new System.Windows.Forms.TabPage();
			this.txtPhotographerFolder = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.lvPWorks = new System.Windows.Forms.ListView();
			this.cboPhotographers = new System.Windows.Forms.ComboBox();
			this.iml = new System.Windows.Forms.ImageList(this.components);
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.tabVal.SuspendLayout();
			this.tabPageFolder.SuspendLayout();
			this.tabPageOrder.SuspendLayout();
			this.tabPageNy.SuspendLayout();
			this.tabPageGranska.SuspendLayout();
			this.pnlBrowser.SuspendLayout();
			this.tabPagePhotographers.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmdOK
			// 
			this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdOK.Location = new System.Drawing.Point(224, 228);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(80, 28);
			this.cmdOK.TabIndex = 0;
			this.cmdOK.Text = "OK";
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// cmdEnd
			// 
			this.cmdEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdEnd.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdEnd.Location = new System.Drawing.Point(328, 228);
			this.cmdEnd.Name = "cmdEnd";
			this.cmdEnd.Size = new System.Drawing.Size(80, 28);
			this.cmdEnd.TabIndex = 6;
			this.cmdEnd.Text = "Avbryt";
			// 
			// tabVal
			// 
			this.tabVal.Controls.Add(this.tabPageFolder);
			this.tabVal.Controls.Add(this.tabPageOrder);
			this.tabVal.Controls.Add(this.tabPageNy);
			this.tabVal.Controls.Add(this.tabPageGranska);
			this.tabVal.Controls.Add(this.tabPageCrasch);
			this.tabVal.Controls.Add(this.tabPagePhotographers);
			this.tabVal.ImageList = this.iml;
			this.tabVal.ItemSize = new System.Drawing.Size(80, 35);
			this.tabVal.Location = new System.Drawing.Point(4, 8);
			this.tabVal.Name = "tabVal";
			this.tabVal.SelectedIndex = 0;
			this.tabVal.Size = new System.Drawing.Size(628, 212);
			this.tabVal.TabIndex = 1;
			this.tabVal.SelectedIndexChanged += new System.EventHandler(this.tabVal_SelectedIndexChanged);
			// 
			// tabPageFolder
			// 
			this.tabPageFolder.Controls.Add(this.lv);
			this.tabPageFolder.ImageIndex = 0;
			this.tabPageFolder.Location = new System.Drawing.Point(4, 39);
			this.tabPageFolder.Name = "tabPageFolder";
			this.tabPageFolder.Size = new System.Drawing.Size(620, 169);
			this.tabPageFolder.TabIndex = 0;
			this.tabPageFolder.Text = "Befintlig order";
			// 
			// lv
			// 
			this.lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																																				 this.chSkola,
																																				 this.chOrderNr,
																																				 this.chSkapad,
																																				 this.chAndrad,
																																				 this.chLevererans});
			this.lv.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lv.FullRowSelect = true;
			this.lv.HideSelection = false;
			this.lv.Location = new System.Drawing.Point(0, 0);
			this.lv.MultiSelect = false;
			this.lv.Name = "lv";
			this.lv.Size = new System.Drawing.Size(620, 169);
			this.lv.Sorting = System.Windows.Forms.SortOrder.Descending;
			this.lv.TabIndex = 0;
			this.lv.View = System.Windows.Forms.View.Details;
			// 
			// chSkola
			// 
			this.chSkola.Text = "Skola";
			this.chSkola.Width = 272;
			// 
			// chOrderNr
			// 
			this.chOrderNr.Text = "Ordernr";
			this.chOrderNr.Width = 80;
			// 
			// chSkapad
			// 
			this.chSkapad.Text = "Skapad";
			this.chSkapad.Width = 80;
			// 
			// chAndrad
			// 
			this.chAndrad.Text = "Ändrad";
			this.chAndrad.Width = 80;
			// 
			// chLevererans
			// 
			this.chLevererans.Text = "Levererans";
			this.chLevererans.Width = 80;
			// 
			// tabPageOrder
			// 
			this.tabPageOrder.Controls.Add(this.label4);
			this.tabPageOrder.Controls.Add(this.txtOrder);
			this.tabPageOrder.ImageIndex = 1;
			this.tabPageOrder.Location = new System.Drawing.Point(4, 39);
			this.tabPageOrder.Name = "tabPageOrder";
			this.tabPageOrder.Size = new System.Drawing.Size(620, 169);
			this.tabPageOrder.TabIndex = 1;
			this.tabPageOrder.Text = "Från orderfil";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(8, 8);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(532, 52);
			this.label4.TabIndex = 6;
			this.label4.Text = "Placera CD-skivan med orderfilen i CD-spelaren och tryck OK.";
			// 
			// txtOrder
			// 
			this.txtOrder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtOrder.Location = new System.Drawing.Point(8, 64);
			this.txtOrder.Name = "txtOrder";
			this.txtOrder.Size = new System.Drawing.Size(600, 20);
			this.txtOrder.TabIndex = 5;
			this.txtOrder.Text = "d:\\";
			// 
			// tabPageNy
			// 
			this.tabPageNy.Controls.Add(this.label3);
			this.tabPageNy.Controls.Add(this.txtOrt);
			this.tabPageNy.Controls.Add(this.label2);
			this.tabPageNy.Controls.Add(this.txtOrderNr);
			this.tabPageNy.Controls.Add(this.label1);
			this.tabPageNy.Controls.Add(this.txtNamn);
			this.tabPageNy.ImageIndex = 2;
			this.tabPageNy.Location = new System.Drawing.Point(4, 39);
			this.tabPageNy.Name = "tabPageNy";
			this.tabPageNy.Size = new System.Drawing.Size(620, 169);
			this.tabPageNy.TabIndex = 2;
			this.tabPageNy.Text = "Ny order";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 100);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(140, 16);
			this.label3.TabIndex = 11;
			this.label3.Text = "Ort";
			// 
			// txtOrt
			// 
			this.txtOrt.Location = new System.Drawing.Point(12, 116);
			this.txtOrt.Name = "txtOrt";
			this.txtOrt.Size = new System.Drawing.Size(220, 20);
			this.txtOrt.TabIndex = 10;
			this.txtOrt.Text = "";
			this.txtOrt.TextChanged += new System.EventHandler(this.txtOrt_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(140, 16);
			this.label2.TabIndex = 9;
			this.label2.Text = "Ordernummer";
			// 
			// txtOrderNr
			// 
			this.txtOrderNr.Location = new System.Drawing.Point(12, 72);
			this.txtOrderNr.MaxLength = 6;
			this.txtOrderNr.Name = "txtOrderNr";
			this.txtOrderNr.Size = new System.Drawing.Size(220, 20);
			this.txtOrderNr.TabIndex = 4;
			this.txtOrderNr.Text = "";
			this.txtOrderNr.TextChanged += new System.EventHandler(this.txtOrderNr_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(140, 16);
			this.label1.TabIndex = 7;
			this.label1.Text = "Namn";
			// 
			// txtNamn
			// 
			this.txtNamn.Location = new System.Drawing.Point(12, 28);
			this.txtNamn.Name = "txtNamn";
			this.txtNamn.Size = new System.Drawing.Size(220, 20);
			this.txtNamn.TabIndex = 2;
			this.txtNamn.Text = "";
			this.txtNamn.TextChanged += new System.EventHandler(this.txtNamn_TextChanged);
			// 
			// tabPageGranska
			// 
			this.tabPageGranska.Controls.Add(this.pnlBrowser);
			this.tabPageGranska.Controls.Add(this.label6);
			this.tabPageGranska.Controls.Add(this.cmdBrowse);
			this.tabPageGranska.Controls.Add(this.cboGranska);
			this.tabPageGranska.ImageIndex = 3;
			this.tabPageGranska.Location = new System.Drawing.Point(4, 39);
			this.tabPageGranska.Name = "tabPageGranska";
			this.tabPageGranska.Size = new System.Drawing.Size(620, 169);
			this.tabPageGranska.TabIndex = 3;
			this.tabPageGranska.Text = "Granska CD";
			// 
			// pnlBrowser
			// 
			this.pnlBrowser.Controls.Add(this.chkReadOnly);
			this.pnlBrowser.Controls.Add(this.label8);
			this.pnlBrowser.Controls.Add(this.txtViewOrderNr);
			this.pnlBrowser.Controls.Add(this.label7);
			this.pnlBrowser.Controls.Add(this.txtPhotWorkFolder);
			this.pnlBrowser.Controls.Add(this.lvPW);
			this.pnlBrowser.Location = new System.Drawing.Point(0, 56);
			this.pnlBrowser.Name = "pnlBrowser";
			this.pnlBrowser.Size = new System.Drawing.Size(620, 112);
			this.pnlBrowser.TabIndex = 8;
			// 
			// chkReadOnly
			// 
			this.chkReadOnly.Checked = true;
			this.chkReadOnly.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkReadOnly.Location = new System.Drawing.Point(72, 32);
			this.chkReadOnly.Name = "chkReadOnly";
			this.chkReadOnly.Size = new System.Drawing.Size(100, 20);
			this.chkReadOnly.TabIndex = 13;
			this.chkReadOnly.Text = "Skrivskydd";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(4, 16);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(43, 16);
			this.label8.TabIndex = 12;
			this.label8.Text = "Ordernr";
			// 
			// txtViewOrderNr
			// 
			this.txtViewOrderNr.Location = new System.Drawing.Point(4, 32);
			this.txtViewOrderNr.MaxLength = 6;
			this.txtViewOrderNr.Name = "txtViewOrderNr";
			this.txtViewOrderNr.Size = new System.Drawing.Size(48, 20);
			this.txtViewOrderNr.TabIndex = 11;
			this.txtViewOrderNr.Text = "";
			this.txtViewOrderNr.Enter += new System.EventHandler(this.txtViewOrderNr_Enter);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(192, 16);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(92, 16);
			this.label7.TabIndex = 10;
			this.label7.Text = "Mapp för fotojobb";
			// 
			// txtPhotWorkFolder
			// 
			this.txtPhotWorkFolder.Location = new System.Drawing.Point(192, 32);
			this.txtPhotWorkFolder.Name = "txtPhotWorkFolder";
			this.txtPhotWorkFolder.ReadOnly = true;
			this.txtPhotWorkFolder.Size = new System.Drawing.Size(424, 20);
			this.txtPhotWorkFolder.TabIndex = 9;
			this.txtPhotWorkFolder.Text = "";
			this.txtPhotWorkFolder.DoubleClick += new System.EventHandler(this.txtPhotWorkFolder_DoubleClick);
			this.txtPhotWorkFolder.Leave += new System.EventHandler(this.txtPhotWorkFolder_Leave);
			// 
			// lvPW
			// 
			this.lvPW.HideSelection = false;
			this.lvPW.LabelEdit = true;
			this.lvPW.Location = new System.Drawing.Point(0, 56);
			this.lvPW.Name = "lvPW";
			this.lvPW.Size = new System.Drawing.Size(620, 56);
			this.lvPW.SmallImageList = this.imlSmall;
			this.lvPW.TabIndex = 8;
			this.lvPW.View = System.Windows.Forms.View.SmallIcon;
			this.lvPW.DoubleClick += new System.EventHandler(this.lvPW_DoubleClick);
			this.lvPW.SelectedIndexChanged += new System.EventHandler(this.lvPW_SelectedIndexChanged);
			// 
			// imlSmall
			// 
			this.imlSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imlSmall.ImageSize = new System.Drawing.Size(16, 16);
			this.imlSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlSmall.ImageStream")));
			this.imlSmall.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(8, 8);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(164, 16);
			this.label6.TabIndex = 2;
			this.label6.Text = "Öppna Plåta-jobb i denna mapp";
			// 
			// cmdBrowse
			// 
			this.cmdBrowse.Location = new System.Drawing.Point(584, 24);
			this.cmdBrowse.Name = "cmdBrowse";
			this.cmdBrowse.Size = new System.Drawing.Size(28, 21);
			this.cmdBrowse.TabIndex = 1;
			this.cmdBrowse.Text = "...";
			this.cmdBrowse.Click += new System.EventHandler(this.cmdBrowse_Click);
			// 
			// cboGranska
			// 
			this.cboGranska.Location = new System.Drawing.Point(8, 24);
			this.cboGranska.Name = "cboGranska";
			this.cboGranska.Size = new System.Drawing.Size(572, 21);
			this.cboGranska.TabIndex = 0;
			// 
			// tabPageCrasch
			// 
			this.tabPageCrasch.ImageIndex = 5;
			this.tabPageCrasch.Location = new System.Drawing.Point(4, 39);
			this.tabPageCrasch.Name = "tabPageCrasch";
			this.tabPageCrasch.Size = new System.Drawing.Size(620, 169);
			this.tabPageCrasch.TabIndex = 5;
			this.tabPageCrasch.Text = "Rädda jobb";
			// 
			// tabPagePhotographers
			// 
			this.tabPagePhotographers.Controls.Add(this.txtPhotographerFolder);
			this.tabPagePhotographers.Controls.Add(this.label5);
			this.tabPagePhotographers.Controls.Add(this.lvPWorks);
			this.tabPagePhotographers.Controls.Add(this.cboPhotographers);
			this.tabPagePhotographers.ImageIndex = 4;
			this.tabPagePhotographers.Location = new System.Drawing.Point(4, 39);
			this.tabPagePhotographers.Name = "tabPagePhotographers";
			this.tabPagePhotographers.Size = new System.Drawing.Size(620, 169);
			this.tabPagePhotographers.TabIndex = 4;
			this.tabPagePhotographers.Text = "Fotografer";
			// 
			// txtPhotographerFolder
			// 
			this.txtPhotographerFolder.Location = new System.Drawing.Point(416, 4);
			this.txtPhotographerFolder.Name = "txtPhotographerFolder";
			this.txtPhotographerFolder.ReadOnly = true;
			this.txtPhotographerFolder.Size = new System.Drawing.Size(200, 20);
			this.txtPhotographerFolder.TabIndex = 4;
			this.txtPhotographerFolder.TabStop = false;
			this.txtPhotographerFolder.Text = "";
			this.txtPhotographerFolder.DoubleClick += new System.EventHandler(this.txtPhotographerFolder_DoubleClick);
			this.txtPhotographerFolder.Leave += new System.EventHandler(this.txtPhotographerFolder_Leave);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(8, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(68, 16);
			this.label5.TabIndex = 3;
			this.label5.Text = "Välj fotograf:";
			// 
			// lvPWorks
			// 
			this.lvPWorks.HideSelection = false;
			this.lvPWorks.LabelEdit = true;
			this.lvPWorks.Location = new System.Drawing.Point(0, 28);
			this.lvPWorks.Name = "lvPWorks";
			this.lvPWorks.Size = new System.Drawing.Size(620, 140);
			this.lvPWorks.SmallImageList = this.imlSmall;
			this.lvPWorks.TabIndex = 1;
			this.lvPWorks.View = System.Windows.Forms.View.SmallIcon;
			this.lvPWorks.DoubleClick += new System.EventHandler(this.lvPWorks_DoubleClick);
			this.lvPWorks.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lvPWorks_AfterLabelEdit);
			// 
			// cboPhotographers
			// 
			this.cboPhotographers.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.cboPhotographers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPhotographers.Location = new System.Drawing.Point(80, 4);
			this.cboPhotographers.MaxDropDownItems = 12;
			this.cboPhotographers.Name = "cboPhotographers";
			this.cboPhotographers.Size = new System.Drawing.Size(68, 21);
			this.cboPhotographers.TabIndex = 0;
			this.cboPhotographers.SelectedIndexChanged += new System.EventHandler(this.cboPhotographers_SelectedIndexChanged);
			this.cboPhotographers.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cboPhotographers_DrawItem);
			// 
			// iml
			// 
			this.iml.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.iml.ImageSize = new System.Drawing.Size(32, 32);
			this.iml.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iml.ImageStream")));
			this.iml.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// frmOpenDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cmdEnd;
			this.ClientSize = new System.Drawing.Size(638, 264);
			this.Controls.Add(this.tabVal);
			this.Controls.Add(this.cmdEnd);
			this.Controls.Add(this.cmdOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmOpenDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Öppna Skola";
			this.tabVal.ResumeLayout(false);
			this.tabPageFolder.ResumeLayout(false);
			this.tabPageOrder.ResumeLayout(false);
			this.tabPageNy.ResumeLayout(false);
			this.tabPageGranska.ResumeLayout(false);
			this.pnlBrowser.ResumeLayout(false);
			this.tabPagePhotographers.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private string getAttr( XmlNode nod, string attr )
		{
			XmlAttribute nodAttr = nod.Attributes[attr];
			if ( nodAttr == null )
				return "";
			string result = nodAttr.Value;
			if ( result == null )
				return "";
			return result;
		}

		private void addLV( string strDir )
		{
			try 
			{
				string strFN = System.IO.Path.Combine( strDir, "!info.xml" );
				string strÄndrad;
				XmlDocument doc = new XmlDocument();

				doc.Load( strFN );
				XmlNode nod = doc.SelectSingleNode("INFO");
				ListViewItem itm = new ListViewItem( getAttr(nod,"namn") );
				itm.SubItems.Add( getAttr(nod,"ordernr") );
				itm.SubItems.Add( getAttr(nod,"skapad").Replace(' ','\0') );
				strÄndrad = getAttr(nod,"andrad");
				itm.SubItems.Add( strÄndrad.Replace(' ','\0') );
				string strLev = getAttr(nod,"cdp") + "/" + getAttr(nod,"cdt") + " st CD";
				itm.SubItems.Add( strLev );
				itm.Tag = new string[] { strDir, strÄndrad };
				lv.Items.Add( itm );
			} 
			catch ( Exception ex ) 
			{
				ex.ToString();
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			foreach ( string strDir in Directory.GetDirectories( Global.MainPath ) )
				if ( !Path.GetFileName(strDir).StartsWith("_") )
					addLV( strDir );
			lv.Sort();
			if ( lv.Items.Count>0 )
				lv.Items[0].Selected = true;
			foreach ( string s in Global.SenasteGranskningPath )
				cboGranska.Items.Add( s );
			if ( cboGranska.Items.Count>0 )
				cboGranska.SelectedIndex = 0;

			if ( !Util.isEmpty(Global.InternalPhotographerFolder) )
				txtPhotographerFolder.Text = Global.InternalPhotographerFolder;
			else
				txtPhotographerFolder.ReadOnly = false;
			if ( !Util.isEmpty(Global.InternalPhotoWorkFolder) )
				txtPhotWorkFolder.Text = Global.InternalPhotoWorkFolder;
			else
				txtPhotWorkFolder.ReadOnly = false;
			if ( Global.Fotografdator )
			{
				pnlBrowser.Visible = false;
				tabVal.TabPages.Remove( tabPagePhotographers );
				tabVal.SelectedIndex = 0;
				System.Threading.Thread t = new System.Threading.Thread( new System.Threading.ThreadStart(checkDiskSpace) );
				t.Start();
			}
			else if ( s_nLastPhotograherSelected!=0 )
				tabVal.SelectedTab = tabPagePhotographers;

		}

		private void checkDiskSpace()
		{
			try
			{
				string strMyDrive = Global.MainPath.Substring(0,2);
				System.Management.ManagementClass diskClass = new System.Management.ManagementClass("Win32_LogicalDisk");
				foreach ( System.Management.ManagementObject disk in diskClass.GetInstances() )
				{
					string strDiskName = (string)disk["Name"];
					if ( strDiskName.Equals(strMyDrive) )
						{
							ulong lMb = ((ulong)disk["FreeSpace"]) / (ulong)(1024*1024);
							if ( lMb < 1000 )
								Global.showMsgBox( this, "Du börjar få ont om diskutrymme! Överväg att radera en skola. Kontakta Photomic!" );
							break;
						}
				}
			}
			catch
			{
			}
		}

		private void lv_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			setOK();
		}

		private void setOK()
		{
			switch ( tabVal.SelectedIndex )
			{
				case 0:
					cmdOK.Enabled = lv.SelectedItems.Count==1;
					break;
				case 1:
					cmdOK.Enabled = true;
					break;
				case 2:
					if ( txtOrder.Text.Length<2 || txtNamn.Text.Length<2 || txtOrt.Text.Length<2 )
						cmdOK.Enabled = false;
					else
						cmdOK.Enabled = _fNewTextFieldsOK[0] & _fNewTextFieldsOK[1];
					break;
			}
		}

		private void copyPersBunt( string strPath )
		{
			string strBuntFil = "persbunt.txt";
			string strSrc = Path.Combine(strPath,strBuntFil);
			string strDst = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),strBuntFil);
			try 
			{
				if ( !File.Exists(strSrc) )
					return;
				StreamReader sr = new StreamReader( strSrc, System.Text.UnicodeEncoding.Default );
				StreamWriter sw = new StreamWriter( strDst, true, System.Text.UnicodeEncoding.Default );
				string s;
				while (	(s=sr.ReadLine()) != null )
					sw.WriteLine( s );
				sr.Close();
				sw.Close();
			}
			catch
			{
			}

		}

		private void go( int nIndex )
		{
			try 
			{
				PlataDM.Skola skola = new PlataDM.Skola( Global.MainPath, Global.Versionsdatum, Global.FotografNummerEndast );
				switch ( nIndex )
				{
					case 0:
						ListViewItem itm = lv.SelectedItems[0];
						skola.open( ((string[])itm.Tag)[0] );
						break;
					case 1:
						string strEXE, strDate;
						if ( Global.lookForUpgrade( txtOrder.Text, out strEXE, out strDate ) )
						{
							if ( string.Compare(strDate,Global.Versionsdatum)>0 )
								if ( Global.runUpgrade(strEXE) )
								{
									this.Close();
									frmMain f = this.Owner as frmMain;
									if ( f!=null )
									{
										f.endApp(false);
										return;
									}
								}
						}
						copyPersBunt( txtOrder.Text );
						if ( !skola.createFromOrder( txtOrder.Text ) )
						{
							Global.showMsgBox( this, "Mappen du angivit innehåller ingen orderfil!" );
							return;
						}
						break;
					case 2:
						if ( !skola.createNew( txtNamn.Text, txtOrderNr.Text, txtOrt.Text ) )
							return;
						break;
					case 3:
						if ( !skola.öppnaFörGranskning(cboGranska.Text) )
						{
							Global.showMsgBox( this, "Mappen du angivit innehåller inte korrekt information för att kunna öppnas för granskning." );
							return;
						}
						if ( Util.isEmpty(Global.InternalPhotoWorkFolder) || !cboGranska.Text.StartsWith(Global.InternalPhotoWorkFolder) )
						{
							for ( int i=Global.SenasteGranskningPath.Count-1 ; i>=0 ; i-- )
								if ( String.Compare( cboGranska.Text, (string)Global.SenasteGranskningPath[i], true ) == 0 )
									Global.SenasteGranskningPath.RemoveAt( i );
							Global.SenasteGranskningPath.Insert( 0, cboGranska.Text );
							Global.sparaInställningar();
						}
						else if ( !chkReadOnly.Checked )
							skola.ReadOnly = false;
						break;
					case 4:
						if ( lvPWorks.SelectedItems.Count!=1 )
							return;
						ShortCutLib.ShortCut sc = new ShortCutLib.ShortCut();
						sc.LnkPath = lvPWorks.SelectedItems[0].Tag as string;
						if ( !sc.ResolveLink() )
							return;
						if ( !skola.öppnaFörGranskning(sc.FilePath) )
						{
							Global.showMsgBox( this, "Mappen du angivit innehåller inte korrekt information för att kunna öppnas för granskning." );
							return;
						}
						break;
				}
				if ( Global.Skola!=null )
					Global.Skola.Dispose();
				Global.Skola = skola;
				this.DialogResult = DialogResult.OK;
			}
			catch ( Exception ex ) 
			{
				Global.showMsgBox( this, ex.Message );
			}
		}

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			go( tabVal.SelectedIndex );
		}

		private bool containsNonNumericChars( string s )
		{
			foreach ( char c in s.ToCharArray() )
				if ( c<'0' || c>'9' )
					return true;
			return false;
		}

		private void lv_DoubleClick(object sender, EventArgs e)
		{
			go( 0 );
		}

		private void lv_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			if ( m_sorter.ColumnIndex == e.Column )
				m_sorter.toggleSortOrder();
			else
			{
				m_sorter.SortOrder = SortOrder.Ascending;
				m_sorter.ColumnIndex = e.Column;
			}
			lv.Sort();
		}

		private void tabVal_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			setOK();
			switch ( tabVal.SelectedIndex )
			{
				case 0:
					lv.Focus();
					break;
				case 2:
					txtNamn.Focus();
					break;
				case 5:
					if ( cboPhotographers.Items.Count==0 )
						showPhotographers();
					break;
			}
		}

		private void cmdBrowse_Click(object sender, System.EventArgs e)
		{
			using ( FolderBrowserDialog dlg = new FolderBrowserDialog() )
			{
				dlg.Description = "Mapp för CD";
				dlg.ShowNewFolderButton = false;
				try {
					dlg.SelectedPath = cboGranska.Text;
				} catch { }
				if ( dlg.ShowDialog()==DialogResult.OK )
					cboGranska.Text = dlg.SelectedPath;
			}
		}

		private class ListViewItemComparer : IComparer 
		{
			private int m_nColIndex;
			private SortOrder m_SortOrder = SortOrder.Descending;

			public ListViewItemComparer() 
			{
				m_nColIndex=0;
			}
			public ListViewItemComparer(int column,SortOrder sortorder) 
			{
				m_nColIndex=column;
				m_SortOrder = sortorder;
			}
			public int ColumnIndex
			{
				get { return m_nColIndex; }
				set { m_nColIndex=value; }
			}
			public SortOrder SortOrder
			{
				get { return m_SortOrder; }
				set { m_SortOrder=value; }
			}
			public void toggleSortOrder()
			{
				m_SortOrder = m_SortOrder==SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
			}
			public int Compare(object x, object y) 
			{
				ListViewItem l1 = (ListViewItem)x;
				ListViewItem l2 = (ListViewItem)y;
				string s1, s2;
				if ( m_nColIndex!=2 )
				{
					s1 = l1.SubItems[m_nColIndex].Text;
					s2 = l2.SubItems[m_nColIndex].Text;
				}
				else
				{
					s1 = ((string[])l1.Tag)[1];
					s2 = ((string[])l2.Tag)[1];
				}
				return (m_SortOrder==SortOrder.Ascending?1:-1) * String.Compare(s1,s2);
			}

		}

		private void lv_KeyUp(object sender, KeyEventArgs e)
		{
			if ( e.KeyCode==Keys.Delete && e.Modifiers==Keys.Shift )
			{
				if ( lv.SelectedItems.Count != 1 )
					return;
				ListViewItem itm = lv.SelectedItems[0];
				if ( MessageBox.Show( this, "Du vill radera skolan \"" + itm.Text + "\" som senast öppnades " +
					itm.SubItems[2].Text + ".\r\n\r\nOm du raderar den finns det INGET sätt att återskapa den på. " +
					"Kontrollera först med Photomic att det är OK att radera den!!!", "Bekräfta radering",
					MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2 ) != DialogResult.OK )
					return;
				if ( Global.askMsgBox( this, "Är du SÄKER på att du vill radera \"" + itm.Text + "\"?", true ) != DialogResult.Yes )
					return;
				try 
				{
					Directory.Delete( ((string[])itm.Tag)[0], true );
					itm.Remove();
				}
				catch ( Exception ex )
				{
					Global.showMsgBox( this, ex.Message );
				}
			}
			else if (  e.KeyCode==Keys.B && e.Modifiers==Keys.Control && Global.Skola==null )
			{
				if ( lv.SelectedItems.Count != 1 )
					return;
				ListViewItem itm = lv.SelectedItems[0];
				if ( Global.askMsgBox( this, "Vill du göra en nödfallsbränning av \"" + itm.Text + "\"?", false ) == DialogResult.Yes )
				{
					RawBurner rb = new RawBurner();
					rb.RawBurn( this, ((string[])itm.Tag)[0] );
				}
			}
		}

		private void txtNamn_TextChanged(object sender, System.EventArgs e)
		{
			_fNewTextFieldsOK[0] = txtNamn.Text.CompareTo( Global.skapaSäkertFilnamn(txtNamn.Text) ) == 0;
			errorProvider1.SetError( txtNamn, _fNewTextFieldsOK[0] ? "" : "Otillåtet tecken!" );
			setOK();
		}

		private void txtOrderNr_TextChanged(object sender, System.EventArgs e)
		{
			_fNewTextFieldsOK[1] = true;
			foreach ( char c in txtOrderNr.Text.ToCharArray() )
				if ( c<'0' || c>'9' )
					_fNewTextFieldsOK[1] = false;
			errorProvider1.SetError( txtOrderNr, _fNewTextFieldsOK[1] ? "" : "Endast siffror!" );
			setOK();
		}

		private void txtOrt_TextChanged(object sender, System.EventArgs e)
		{
			setOK();
		}

		private void showPhotographers()
		{
			if ( Util.isEmpty(Global.InternalPhotographerFolder) )
				return;
			if ( !Directory.Exists(Global.InternalPhotographerFolder) )
				return;
			ArrayList al = new ArrayList();
			foreach ( string s in Directory.GetDirectories(Global.InternalPhotographerFolder) )
			{
				string strItem = Path.GetFileName(s);
				if ( System.Text.RegularExpressions.Regex.IsMatch(strItem,@"^\d{1,4}$") )
					al.Add( int.Parse(strItem) );
			}
			al.Sort();
			cboPhotographers.Items.Clear();
			cboPhotographers.Items.AddRange( al.ToArray() );

			if ( cboPhotographers.Items.Contains(s_nLastPhotograherSelected) )
				cboPhotographers.SelectedItem = s_nLastPhotograherSelected;
			else if ( cboPhotographers.Items.Count!=0 )
				cboPhotographers.SelectedIndex = 0;
			else
				lvPWorks.Items.Clear();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			switch ( tabVal.SelectedIndex )
			{
				case 4:
					switch ( e.KeyCode )
					{
						case Keys.F2:
							if ( lvPWorks.Focused && lvPWorks.SelectedItems.Count==1 )
								lvPWorks.SelectedItems[0].BeginEdit();
							return;
						case Keys.F5:
							refreshProjects();
							return;
					}
					return;
				case 3:
					if ( e.KeyCode==Keys.Enter && txtViewOrderNr.Focused )
					{
						viewPhotoWorkFolder();
						return;
					}
					break;
			}

			if ( e.KeyCode==Keys.Enter && cmdOK.Enabled )
				cmdOK.PerformClick();
			else
				base.OnKeyDown (e);
		}

		private void cboPhotographers_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			s_nLastPhotograherSelected = cboPhotographers.SelectedItem!=null ? (int)cboPhotographers.SelectedItem: 0;
			refreshProjects();
		}

		private void refreshProjects()
		{
			lvPWorks.Items.Clear();
			if ( s_nLastPhotograherSelected==0 )
				return;
			string strSelectedFolder = Path.Combine(Global.InternalPhotographerFolder, s_nLastPhotograherSelected.ToString() );
			foreach ( string s in Directory.GetFiles( strSelectedFolder, "*.lnk" ) )
				lvPWorks.Items.Add( Path.GetFileNameWithoutExtension(s), 1 ).Tag = s;
		}

		private void cboPhotographers_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			string strText = e.Index>=0 ? ((int)cboPhotographers.Items[e.Index]).ToString() : string.Empty;
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Far;

			e.DrawBackground();
			e.Graphics.DrawImage( imlSmall.Images[0], new Rectangle( e.Bounds.Left+2, e.Bounds.Top, 16, 16 ),
				0, 0, 16, 16, GraphicsUnit.Pixel );

			using ( Brush br = new SolidBrush(e.ForeColor) )
				e.Graphics.DrawString( strText, e.Font, br,
					new Rectangle( e.Bounds.Left, e.Bounds.Top, e.Bounds.Width-0, e.Bounds.Height ), sf );
			e.DrawFocusRectangle();

		}

		private void lvPWorks_DoubleClick(object sender, System.EventArgs e)
		{
			go( 4 );
		}

		private void lvPWorks_AfterLabelEdit(object sender, System.Windows.Forms.LabelEditEventArgs e)
		{
			if ( e.Label==null || e.Label.Length==0 || e.Label.IndexOf('\\')>=0 )
			{
				e.CancelEdit = true;
				return;
			}

			ListViewItem lvi = lvPWorks.Items[e.Item];
			string strOldFN = lvi.Tag as string;
			string strNewFN = Path.Combine( Path.GetDirectoryName(strOldFN), e.Label+".lnk" );

			try 
			{
				File.Move( strOldFN, strNewFN );
				lvi.Tag = strNewFN;
			}
			catch
			{
				e.CancelEdit = true;
			}

		}

		private void txtPhotographerFolder_Leave(object sender, System.EventArgs e)
		{
			if ( string.Compare( Global.InternalPhotographerFolder, txtPhotographerFolder.Text ) != 0 )
				if ( Directory.Exists( txtPhotographerFolder.Text ) )
				{
					Global.InternalPhotographerFolder = txtPhotographerFolder.Text;
					showPhotographers();
					Global.sparaInställningar();
				}
		}

		private void txtPhotographerFolder_DoubleClick(object sender, System.EventArgs e)
		{
			txtPhotographerFolder.ReadOnly = false;
		}

		private void txtPhotWorkFolder_Leave(object sender, System.EventArgs e)
		{
			if ( string.Compare( Global.InternalPhotoWorkFolder, txtPhotWorkFolder.Text ) != 0 )
				if ( Directory.Exists( txtPhotWorkFolder.Text ) )
				{
					Global.InternalPhotoWorkFolder = txtPhotWorkFolder.Text;
					showPhotographers();
					Global.sparaInställningar();
				}
		}

		private void txtPhotWorkFolder_DoubleClick(object sender, System.EventArgs e)
		{
			txtPhotWorkFolder.ReadOnly = false;
		}

		private void txtViewOrderNr_Enter(object sender, System.EventArgs e)
		{
			txtViewOrderNr.SelectAll();
		}

		private void viewPhotoWorkFolder()
		{
			string s = txtViewOrderNr.Text.Trim();
			if ( !System.Text.RegularExpressions.Regex.IsMatch(s,@"^\d+$") )
				return;
			int nOrderNr = int.Parse(s);
			if ( nOrderNr<400000 )
				nOrderNr += 400000;
			txtOrderNr.Text = nOrderNr.ToString();

			lvPW.Items.Clear();
			s = Path.Combine( Global.InternalPhotoWorkFolder, txtOrderNr.Text );
			if ( !Directory.Exists(s) )
				return;
			foreach ( string strWork in Directory.GetDirectories(s) )
				if ( System.Text.RegularExpressions.Regex.IsMatch(strWork,@"\\\d+$") )
					lvPW.Items.Add( Path.GetFileName(strWork), 1 ).Tag = strWork;
			if ( lvPW.Items.Count!=0 )
			{
				lvPW.Items[0].Selected = true;
				lvPW.Focus();
			}
		}

		private void lvPW_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if ( lvPW.SelectedItems.Count==1 )
				cboGranska.Text = (string)lvPW.SelectedItems[0].Tag;
		}

		private void lvPW_DoubleClick(object sender, System.EventArgs e)
		{
			cmdOK.PerformClick();
		}

	}

}
