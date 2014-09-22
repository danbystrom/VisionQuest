using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace Plata.OpenDialog
{
	public class FOpenDialog : Form
	{
		private static int _nLastSelectedTabIndex = 0;

		private Button cmdOK;
		private Button cmdEnd;
		private System.ComponentModel.IContainer components;

		private ImageList iml;
		private TabControl tabVal;

		private baseUsrTab[] _tabs = new baseUsrTab[7];
		private System.Windows.Forms.CheckBox chkPhotoOrder;
		private baseUsrTab _tabActive;

		private FPhotoOrder _dlgPhotoOrder;

		private PlataDM.Skola _skola;
		private bool _fImport;

		private FOpenDialog()
		{
			InitializeComponent();
		}

		private FOpenDialog( bool fImport )
		{
			InitializeComponent();

			_fImport = fImport;
			constructTabPage( 0, new usrOpenWork(false) );
			constructTabPage( 1, new usrOpenFtp() );
			constructTabPage( 2, new usrOpenImportOrderFile() );
			if ( !_fImport )
			{
				constructTabPage( 3, new usrOpenNewManual() );
				constructTabPage( 4, new usrOpenRecovery() );
				constructTabPage( 5, new usrOpenWork(true) );
				//if ( !Global.Fotografdator )
					constructTabPage( 6, new usrOpenView() );
				//if ( !Global.Fotografdator )
				//	constructTabPage( 7, new usrOpenPhotographers() );
			}
			else
				this.Text = "Importera namn";
			tabVal.SelectedIndex = _nLastSelectedTabIndex;
		}

		private void constructTabPage( int nIndex, baseUsrTab tab )
		{
			_tabs[nIndex] = tab;

			tab._fImport = _fImport;
			tab.Execute += new EventHandler(tab_Execute);
			tab.SetOK += new EventHandler(tab_SetOK);
			tab.SelectionChanged += new EventHandler(tab_SelectionChanged);
			TabPage tp = new TabPage( tab.Text );
			tp.ImageIndex = nIndex;
			tabVal.TabPages.Add( tp );
			tp.Controls.Add( tab );
			tab.Dock = DockStyle.Fill;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if ( _dlgPhotoOrder!=null )
					_dlgPhotoOrder.Dispose();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( FOpenDialog ) );
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdEnd = new System.Windows.Forms.Button();
			this.tabVal = new System.Windows.Forms.TabControl();
			this.iml = new System.Windows.Forms.ImageList( this.components );
			this.chkPhotoOrder = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// cmdOK
			// 
			this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdOK.Location = new System.Drawing.Point( 152, 260 );
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size( 80, 28 );
			this.cmdOK.TabIndex = 0;
			this.cmdOK.Text = "OK";
			this.cmdOK.Click += new System.EventHandler( this.cmdOK_Click );
			// 
			// cmdEnd
			// 
			this.cmdEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdEnd.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdEnd.Location = new System.Drawing.Point( 248, 260 );
			this.cmdEnd.Name = "cmdEnd";
			this.cmdEnd.Size = new System.Drawing.Size( 80, 28 );
			this.cmdEnd.TabIndex = 6;
			this.cmdEnd.Text = "Avbryt";
			// 
			// tabVal
			// 
			this.tabVal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.tabVal.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
			this.tabVal.ImageList = this.iml;
			this.tabVal.ItemSize = new System.Drawing.Size( 80, 35 );
			this.tabVal.Location = new System.Drawing.Point( 4, 8 );
			this.tabVal.Multiline = true;
			this.tabVal.Name = "tabVal";
			this.tabVal.SelectedIndex = 0;
			this.tabVal.Size = new System.Drawing.Size( 472, 244 );
			this.tabVal.TabIndex = 1;
			this.tabVal.SelectedIndexChanged += new System.EventHandler( this.tabVal_SelectedIndexChanged );
			// 
			// iml
			// 
			this.iml.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject( "iml.ImageStream" )));
			this.iml.TransparentColor = System.Drawing.Color.Magenta;
			this.iml.Images.SetKeyName( 0, "" );
			this.iml.Images.SetKeyName( 1, "globe.bmp" );
			this.iml.Images.SetKeyName( 2, "" );
			this.iml.Images.SetKeyName( 3, "" );
			this.iml.Images.SetKeyName( 4, "" );
			this.iml.Images.SetKeyName( 5, "" );
			this.iml.Images.SetKeyName( 6, "" );
			this.iml.Images.SetKeyName( 7, "" );
			// 
			// chkPhotoOrder
			// 
			this.chkPhotoOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.chkPhotoOrder.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkPhotoOrder.Location = new System.Drawing.Point( 380, 260 );
			this.chkPhotoOrder.Name = "chkPhotoOrder";
			this.chkPhotoOrder.Size = new System.Drawing.Size( 92, 28 );
			this.chkPhotoOrder.TabIndex = 7;
			this.chkPhotoOrder.Text = "Visa Fotoorder";
			this.chkPhotoOrder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkPhotoOrder.CheckedChanged += new System.EventHandler( this.chkPhotoOrder_CheckedChanged );
			// 
			// frmOpenDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.CancelButton = this.cmdEnd;
			this.ClientSize = new System.Drawing.Size( 478, 296 );
			this.Controls.Add( this.chkPhotoOrder );
			this.Controls.Add( this.tabVal );
			this.Controls.Add( this.cmdEnd );
			this.Controls.Add( this.cmdOK );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmOpenDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Öppna Order";
			this.ResumeLayout( false );

		}
		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			_tabActive = _tabs[tabVal.SelectedIndex];
			_tabActive.Focus();
			_tabActive.activate();

			if ( Global.Fotografdator )
				new GetManagementObject(
					this,
					new GetManagementObject.ManagementClassFound(checkDiskSpace),
					Global.Preferences.MainPath );

		}

		private void checkDiskSpace( System.Management.ManagementObject disk )
		{
			if ( disk==null )
				return;

			try
			{
				var lMb = ((ulong)disk["FreeSpace"]) / (1024*1024);
				if ( lMb < 6000 )
					deleteOldTempFiles();
				if ( lMb < 5000 )
					Global.showMsgBox( this, "Du börjar få ont om diskutrymme! Överväg att radera en order. Kontakta Photomic!" );
			}
			catch
			{
			}
		}

		private static void deleteOldTempFiles()
		{
			var dateTreshold = Global.Now.Subtract( new TimeSpan(4,0,0,0,0) );
			foreach ( var fsi in new DirectoryInfo(Global.GetTempPath()).GetFileSystemInfos() )
				if ( fsi.CreationTime.Date < dateTreshold )
					Util.safeKillFile( fsi.FullName );
		}

		private void go()
		{
			if ( !_tabActive.isOK )
				return;
			try 
			{
				int nFtgNum = Global.Preferences.Fotografnummer;
				if ( _tabActive is usrOpenView )
					nFtgNum = 0;
                _skola = new PlataDM.Skola(AppSpecifics.Version, nFtgNum);
				if ( _tabActive.openOrder(_skola) )
				{
					_skola.Verifierad = false;  //ska endast vara satt vid slutlagring/bränning
					if ( !_fImport )
						Directory.CreateDirectory( _skola.HomePathCombine( "cache" ) );
					this.DialogResult = DialogResult.OK;
				}
			}
			catch ( Exception ex ) 
			{
				Global.showMsgBox( this, ex.ToString() );
			}
		}

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			go();
		}

		private void tab_Execute(object sender, EventArgs e)
		{
			go();
		}

		private void tab_SetOK(object sender, EventArgs e)
		{
			cmdOK.Enabled = ((baseUsrTab)sender).isOK;
		}

		private void tab_SelectionChanged(object sender, EventArgs e)
		{
			if ( chkPhotoOrder.Checked && _dlgPhotoOrder!=null )
				photoOrder();
		}

		private void tabVal_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			_tabActive = _tabs[tabVal.SelectedIndex];
			_tabActive.activate();
			cmdOK.Enabled = _tabActive.isOK;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if ( !_tabActive.gotKeyDown( e ) )
				if ( e.KeyCode==Keys.Enter )
					go();
				else
					base.OnKeyDown( e );
		}

		private void chkPhotoOrder_CheckedChanged(object sender, System.EventArgs e)
		{
			if ( chkPhotoOrder.Checked )
			{
				if ( _dlgPhotoOrder == null )
				{
					Rectangle r = SystemInformation.WorkingArea;
					this.Left = r.Left;

					_dlgPhotoOrder = new FPhotoOrder();
					_dlgPhotoOrder.Closed += new EventHandler(_dlgPhotoOrder_Closed);
					_dlgPhotoOrder.Owner = this.Owner;
					_dlgPhotoOrder.Bounds = new Rectangle( this.Right, r.Top, r.Width-this.Right, r.Height );
					_dlgPhotoOrder.Show();
				}
				photoOrder();
			}
			else
			{
				_dlgPhotoOrder.Close();
				_dlgPhotoOrder.Dispose();
				_dlgPhotoOrder = null;
			}
		}

		private void photoOrder()
		{
			_dlgPhotoOrder.visa( _tabActive.selectedPath );
		}

		private void _dlgPhotoOrder_Closed(object sender, EventArgs e)
		{
			chkPhotoOrder.Checked = false;
		}

		public static DialogResult showDialog( 
			Form parent,
			bool fImport,
			ref PlataDM.Skola skola )
		{
			using( OpenDialog.FOpenDialog dlg = new OpenDialog.FOpenDialog(fImport) )
			{
				if ( dlg.ShowDialog(parent)!=DialogResult.OK )
					return DialogResult.Cancel;
				_nLastSelectedTabIndex = dlg.tabVal.SelectedIndex;
				if ( skola != null )
					skola.Dispose();
				skola = dlg._skola;
				return DialogResult.OK;
			}
		}

	}

}
