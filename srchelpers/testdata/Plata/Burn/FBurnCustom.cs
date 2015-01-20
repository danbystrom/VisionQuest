using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using Photomic.ArchiveStuff;
using Photomic.ArchiveStuff.Core;
using Photomic.Common;

namespace Plata.Burn
{
	/// <summary>
	/// Summary description for FBurnCustom.
	/// </summary>
	public class FBurnCustom : vdUsr.baseGradientForm
	{
		private delegate void delegateSetThumbnail( Bitmap bmp );

		private System.Windows.Forms.TreeView tv;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private System.Windows.Forms.CheckBox chkGrupp;
		private System.Windows.Forms.CheckBox chkPorträtt;
		private System.Windows.Forms.CheckBox chkVimmel;
		private System.Windows.Forms.Button cmdAdvancedGrupp;
		private System.Windows.Forms.Button cmdAdvancedPorträtt;
		private System.Windows.Forms.Button cmdAdvancedVimmel;

		private System.Windows.Forms.Label lblInfo;
		private System.Windows.Forms.Button cmdBurn;
		private System.Windows.Forms.Button cmdCancel;
		private vdUsr.vdGroup groupBox1;
		private System.Windows.Forms.ComboBox cboType;

		private Rectangle _rectThumbnail = Rectangle.Empty;

		private readonly CustomGenerator _generator;
		private readonly Queue _queThumbnails;
        private Bitmap _bmp = null;
		private bool _fLoading;

		private FBurnCustom()
		{
			InitializeComponent();

		    _generator = new CustomGenerator(
                Global.Skola.Namn,
                new [] { Global.Skola },
                null,
                new Size(Global.Porträttfotobredd, Global.Porträttfotobredd*3/2),
                null);

			cboType.Items.Add( _generator.PresetArkivJpg );
			cboType.Items.Add( _generator.PresetSport );
			cboType.Items.Add( _generator.PresetVimmel );
			cboType.SelectedIndex = 0;

		    var iml = new ImageList
		                  {
		                      ColorDepth = ColorDepth.Depth24Bit,
                              TransparentColor = Color.Magenta,
                              ImageSize = new Size(16, 16)
		                  };
		    iml.Images.AddStrip( new Bitmap( typeof(FMain), "grfx.treeicons.bmp" ) );
			tv.ImageList = iml;

			_queThumbnails = Queue.Synchronized( new Queue() );
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed (e);
			if ( _bmp!=null )
				_bmp.Dispose();
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
            this.chkGrupp = new System.Windows.Forms.CheckBox();
            this.chkPorträtt = new System.Windows.Forms.CheckBox();
            this.chkVimmel = new System.Windows.Forms.CheckBox();
            this.tv = new System.Windows.Forms.TreeView();
            this.lblInfo = new System.Windows.Forms.Label();
            this.cmdAdvancedGrupp = new System.Windows.Forms.Button();
            this.cmdAdvancedPorträtt = new System.Windows.Forms.Button();
            this.cmdAdvancedVimmel = new System.Windows.Forms.Button();
            this.cmdBurn = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new vdUsr.vdGroup();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkGrupp
            // 
            this.chkGrupp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkGrupp.Checked = true;
            this.chkGrupp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGrupp.Location = new System.Drawing.Point(32, 24);
            this.chkGrupp.Name = "chkGrupp";
            this.chkGrupp.Size = new System.Drawing.Size(92, 16);
            this.chkGrupp.TabIndex = 1;
            this.chkGrupp.Text = "Gruppbilder";
            this.chkGrupp.UseVisualStyleBackColor = false;
            this.chkGrupp.CheckedChanged += new System.EventHandler(this.chkGrupp_CheckedChanged);
            // 
            // chkPorträtt
            // 
            this.chkPorträtt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkPorträtt.Checked = true;
            this.chkPorträtt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPorträtt.Location = new System.Drawing.Point(32, 56);
            this.chkPorträtt.Name = "chkPorträtt";
            this.chkPorträtt.Size = new System.Drawing.Size(92, 16);
            this.chkPorträtt.TabIndex = 2;
            this.chkPorträtt.Text = "Porträtt";
            this.chkPorträtt.UseVisualStyleBackColor = false;
            this.chkPorträtt.CheckedChanged += new System.EventHandler(this.chkPorträtt_CheckedChanged);
            // 
            // chkVimmel
            // 
            this.chkVimmel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkVimmel.Location = new System.Drawing.Point(32, 88);
            this.chkVimmel.Name = "chkVimmel";
            this.chkVimmel.Size = new System.Drawing.Size(92, 16);
            this.chkVimmel.TabIndex = 3;
            this.chkVimmel.Text = "Vimmelbilder";
            this.chkVimmel.UseVisualStyleBackColor = false;
            this.chkVimmel.CheckedChanged += new System.EventHandler(this.chkVimmel_CheckedChanged);
            // 
            // tv
            // 
            this.tv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tv.LabelEdit = true;
            this.tv.Location = new System.Drawing.Point(8, 8);
            this.tv.Name = "tv";
            this.tv.Size = new System.Drawing.Size(380, 692);
            this.tv.TabIndex = 4;
            this.tv.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tv_AfterLabelEdit);
            this.tv.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tv_AfterSelect);
            this.tv.DoubleClick += new System.EventHandler(this.tv_DoubleClick);
            this.tv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tv_KeyDown);
            // 
            // lblInfo
            // 
            this.lblInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblInfo.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.Location = new System.Drawing.Point(396, 672);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(240, 32);
            this.lblInfo.TabIndex = 5;
            this.lblInfo.Text = "Dubbelklicka på en bild för att se den som den kommer att brännas";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmdAdvancedGrupp
            // 
            this.cmdAdvancedGrupp.BackColor = System.Drawing.SystemColors.Control;
            this.cmdAdvancedGrupp.Location = new System.Drawing.Point(136, 20);
            this.cmdAdvancedGrupp.Name = "cmdAdvancedGrupp";
            this.cmdAdvancedGrupp.Size = new System.Drawing.Size(88, 20);
            this.cmdAdvancedGrupp.TabIndex = 6;
            this.cmdAdvancedGrupp.Text = "Avancerat...";
            this.cmdAdvancedGrupp.UseVisualStyleBackColor = false;
            this.cmdAdvancedGrupp.Click += new System.EventHandler(this.cmdAdvancedGrupp_Click);
            // 
            // cmdAdvancedPorträtt
            // 
            this.cmdAdvancedPorträtt.BackColor = System.Drawing.SystemColors.Control;
            this.cmdAdvancedPorträtt.Location = new System.Drawing.Point(136, 52);
            this.cmdAdvancedPorträtt.Name = "cmdAdvancedPorträtt";
            this.cmdAdvancedPorträtt.Size = new System.Drawing.Size(88, 20);
            this.cmdAdvancedPorträtt.TabIndex = 7;
            this.cmdAdvancedPorträtt.Text = "Avancerat...";
            this.cmdAdvancedPorträtt.UseVisualStyleBackColor = false;
            this.cmdAdvancedPorträtt.Click += new System.EventHandler(this.cmdAdvancedPorträtt_Click);
            // 
            // cmdAdvancedVimmel
            // 
            this.cmdAdvancedVimmel.BackColor = System.Drawing.SystemColors.Control;
            this.cmdAdvancedVimmel.Location = new System.Drawing.Point(136, 84);
            this.cmdAdvancedVimmel.Name = "cmdAdvancedVimmel";
            this.cmdAdvancedVimmel.Size = new System.Drawing.Size(88, 20);
            this.cmdAdvancedVimmel.TabIndex = 8;
            this.cmdAdvancedVimmel.Text = "Avancerat...";
            this.cmdAdvancedVimmel.UseVisualStyleBackColor = false;
            this.cmdAdvancedVimmel.Click += new System.EventHandler(this.cmdAdvancedVimmel_Click);
            // 
            // cmdBurn
            // 
            this.cmdBurn.Location = new System.Drawing.Point(524, 172);
            this.cmdBurn.Name = "cmdBurn";
            this.cmdBurn.Size = new System.Drawing.Size(108, 40);
            this.cmdBurn.TabIndex = 9;
            this.cmdBurn.Text = "BRÄNN";
            this.cmdBurn.Click += new System.EventHandler(this.cmdBurn_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(524, 224);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(108, 28);
            this.cmdCancel.TabIndex = 10;
            this.cmdCancel.Text = "Avbryt";
            // 
            // cboType
            // 
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType.Location = new System.Drawing.Point(400, 8);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(232, 21);
            this.cboType.TabIndex = 11;
            this.cboType.SelectedIndexChanged += new System.EventHandler(this.cboType_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.groupBox1.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.groupBox1.Caption = null;
            this.groupBox1.Checkable = true;
            this.groupBox1.Controls.Add(this.chkGrupp);
            this.groupBox1.Controls.Add(this.cmdAdvancedGrupp);
            this.groupBox1.Controls.Add(this.cmdAdvancedPorträtt);
            this.groupBox1.Controls.Add(this.chkPorträtt);
            this.groupBox1.Controls.Add(this.cmdAdvancedVimmel);
            this.groupBox1.Controls.Add(this.chkVimmel);
            this.groupBox1.Location = new System.Drawing.Point(400, 36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 116);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Anpassat...";
            // 
            // FBurnCustom
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(640, 706);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cboType);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdBurn);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.tv);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FBurnCustom";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bränn direkt till kund";
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			generateFiles();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);

			if ( _bmp==null )
				return;

			_rectThumbnail = vdUsr.ImgHelper.adaptProportionalRect(
				new Rectangle( tv.Right+5, 0, this.ClientSize.Width-tv.Right-10, 10000 ), 
				_bmp.Width, _bmp.Height );
			_rectThumbnail.Y = lblInfo.Top - _rectThumbnail.Height - 5;
			e.Graphics.DrawImage( _bmp, _rectThumbnail );
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown (e);
			if ( e.Clicks==2 && _rectThumbnail.Contains(e.X,e.Y) && _bmp!=null )
				FViewImage.showDialog( this, _bmp );
		}

		private void generateFiles()
		{
			if ( _fLoading )
				return;
			_generator.GenerateFiles( chkGrupp.Checked, chkPorträtt.Checked, chkVimmel.Checked );
		}

		public static DialogResult showDialog( Form parent )
		{
			using ( var dlg = new FBurnCustom() )
				return dlg.ShowDialog(parent);
		}

		private void chkGrupp_CheckedChanged(object sender, System.EventArgs e)
		{
			generateFiles();
		}

		private void chkPorträtt_CheckedChanged(object sender, System.EventArgs e)
		{
			generateFiles();
		}

		private void chkVimmel_CheckedChanged(object sender, System.EventArgs e)
		{
			generateFiles();
		}

		private void tv_DoubleClick(object sender, System.EventArgs e)
		{
			if ( tv.SelectedNode==null )
				return;
			var cnode = tv.SelectedNode.Tag as CustomGeneratorNode;
			if ( cnode==null )
				return;

			using ( Bitmap bmp = cnode.CBI.CreateImage( cnode.RealFilename, null ) )
				FViewImage.showDialog( this, bmp );
		}

		private void cmdAdvancedGrupp_Click(object sender, System.EventArgs e)
		{
			if ( FAdvancedCustom.showDialog( this, _generator.CustomGrupp, PhotoType.Group ) == DialogResult.OK )
				generateFiles();
		}

		private void cmdAdvancedPorträtt_Click(object sender, System.EventArgs e)
		{
			if ( FAdvancedCustom.showDialog( this, _generator.CustomPorträtt, PhotoType.Portrait )== DialogResult.OK )
				generateFiles();
		}

		private void cmdAdvancedVimmel_Click(object sender, System.EventArgs e)
		{
			if ( FAdvancedCustom.showDialog( this, _generator.CustomVimmel, PhotoType.Vimmel )== DialogResult.OK )
				generateFiles();
		}

		private void tv_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			var cn = e.Node.Tag as CustomGeneratorNode;
			if ( cn==null )
				return;
			_queThumbnails.Enqueue( cn );
			if ( _queThumbnails.Count==1 )
				(new Thread( threadProc )).Start();
		}

		private void setThumbnail( Bitmap bmp )
		{
			if ( _bmp!=null )
				_bmp.Dispose();
			_bmp = bmp;
			Invalidate();
		}

		private void threadProc()
		{
			while ( true )
			{
				CustomGeneratorNode cn = null;
				while ( _queThumbnails.Count!=0 )
					cn = (CustomGeneratorNode)_queThumbnails.Dequeue();
				if ( cn==null )
					return;

				var bmp = cn.CBI.CreateImage(cn.RealFilename,null);
				if ( bmp!=null )
					if ( _queThumbnails.Count==0 )
					    this.Invoke((MethodInvoker) (() => setThumbnail(bmp)));
					else
						bmp.Dispose();
			}
		}

		private void cmdBurn_Click(object sender, System.EventArgs e)
		{
			if ( _generator.ResultArray.Count!=0 )
				FCreateCustomCD.showDialog( this, _generator, "Specialarkiv" );
		}

		private void tv_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			switch ( e.KeyCode )
			{
				case Keys.F2:
					tv.SelectedNode.BeginEdit();
					e.Handled = true;
					break;
				case Keys.Delete:
					if ( tv.SelectedNode == tv.Nodes[0] )
						return;
					if ( Global.askMsgBox( this, "Är du säker på att du vill radera noden?", true ) == DialogResult.Yes )
					{
						deleteTreeNode( tv.SelectedNode );
						tv.SelectedNode.Remove();
						e.Handled = true;
					}
					break;
			}
		}

		private void deleteTreeNode( TreeNode tn )
		{
			foreach ( TreeNode tnChild in tn.Nodes )
				deleteTreeNode( tnChild );
			var cn = tn.Tag as CustomGeneratorNode;
			if ( cn!=null )
				_generator.ResultArray.Remove( cn );
		}

		private void tv_AfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
			if ( !string.IsNullOrEmpty(e.Label) )
				e.Node.Text = Global.skapaSäkertFilnamn(e.Label);
			e.CancelEdit = true;
		}

		private void cboType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			var bp = cboType.SelectedItem as CustomGenerator.BPreset;
			_fLoading = true;
			chkGrupp.Checked = bp.Grupp;
			chkPorträtt.Checked = bp.Porträtt;
			chkVimmel.Checked = bp.Vimmel;
			_generator.selectPreset( bp );
			_fLoading = false;
			generateFiles();
		}

	}

}
