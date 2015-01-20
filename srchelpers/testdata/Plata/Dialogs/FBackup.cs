using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace Plata
{
	/// <summary>
	/// Summary description for FBackup.
	/// </summary>
	public class FBackup : vdUsr.baseGradientForm
	{
		private System.Windows.Forms.ProgressBar pbr;
		private System.Windows.Forms.Button cmdStart;
		private System.Windows.Forms.Button cmdAbort;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;

		private ArrayList _alFilesToCopy = new ArrayList();
		private double _dblBytes2Copy;
		private double _dblBytesCopied;
		private System.Windows.Forms.Label lblTime;
		private System.Windows.Forms.Label lblFile;
		private DateTime _dateStart;
		private int _nTotEstimate;
		private int _nLeft;

		private bool _fRestore;
		private string _strSrc;
		private string _strDst;

		private FBackup()
		{
			InitializeComponent();
		}

		private FBackup(
			bool fRestore,
			string strText,
			string strSrc,
			string strDst )
		{
			InitializeComponent();

			_fRestore = fRestore;
			_strSrc = strSrc;
			_strDst = strDst;
			this.Text = strText;
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
			this.pbr = new System.Windows.Forms.ProgressBar();
			this.cmdStart = new System.Windows.Forms.Button();
			this.cmdAbort = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblTime = new System.Windows.Forms.Label();
			this.lblFile = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// pbr
			// 
			this.pbr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pbr.Location = new System.Drawing.Point(8, 64);
			this.pbr.Name = "pbr";
			this.pbr.Size = new System.Drawing.Size(448, 32);
			this.pbr.TabIndex = 0;
			// 
			// cmdStart
			// 
			this.cmdStart.Location = new System.Drawing.Point(264, 104);
			this.cmdStart.Name = "cmdStart";
			this.cmdStart.Size = new System.Drawing.Size(88, 32);
			this.cmdStart.TabIndex = 1;
			this.cmdStart.Text = "Starta";
			this.cmdStart.Click += new System.EventHandler(this.cmdStart_Click);
			// 
			// cmdAbort
			// 
			this.cmdAbort.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdAbort.Location = new System.Drawing.Point(360, 104);
			this.cmdAbort.Name = "cmdAbort";
			this.cmdAbort.Size = new System.Drawing.Size(88, 32);
			this.cmdAbort.TabIndex = 2;
			this.cmdAbort.Text = "Avbryt";
			// 
			// timer1
			// 
			this.timer1.Interval = 10;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(16, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(50, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "Kopierar:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(16, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 16);
			this.label2.TabIndex = 4;
			this.label2.Text = "Tid kvar:";
			// 
			// lblTime
			// 
			this.lblTime.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.lblTime.Location = new System.Drawing.Point(80, 32);
			this.lblTime.Name = "lblTime";
			this.lblTime.Size = new System.Drawing.Size(120, 16);
			this.lblTime.TabIndex = 6;
			this.lblTime.Text = "-";
			// 
			// lblFile
			// 
			this.lblFile.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.lblFile.Location = new System.Drawing.Point(80, 8);
			this.lblFile.Name = "lblFile";
			this.lblFile.Size = new System.Drawing.Size(376, 16);
			this.lblFile.TabIndex = 5;
			this.lblFile.Text = "-";
			// 
			// FBackup
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(464, 144);
			this.Controls.Add(this.lblTime);
			this.Controls.Add(this.lblFile);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmdAbort);
			this.Controls.Add(this.cmdStart);
			this.Controls.Add(this.pbr);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FBackup";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Backup";
			this.ResumeLayout(false);

		}
		#endregion

		private void cmdStart_Click(object sender, System.EventArgs e)
		{
			cmdStart.Enabled = false;

			if ( !Directory.Exists(_strDst) )
				Directory.CreateDirectory(_strDst);
			else if ( File.Exists( Path.Combine(_strDst,"!order.plata") ) )
			{
				var strNew = string.Format( "!order_{0:yyyyMMddhhmmss}.plata", Global.Now );
				File.Copy( Path.Combine(_strDst,"!order.plata"), Path.Combine(_strDst,strNew), true );
			}

			var hash = new Hashtable();
			foreach ( var fi in new DirectoryInfo(_strDst).GetFiles() )
				hash.Add( fi.Name, fi );

			foreach ( var fi in new DirectoryInfo(_strSrc).GetFiles() )
				if ( !hash.ContainsKey(fi.Name) )
					_alFilesToCopy.Add( fi );
				else
				{
					var fiE = (FileInfo)hash[fi.Name];
					if ( fiE.Length!=fi.Length || fiE.LastWriteTime!=fi.LastWriteTime )
						_alFilesToCopy.Add( fi );
				}

			foreach ( FileInfo fi in _alFilesToCopy )
				_dblBytes2Copy += fi.Length;
			_nTotEstimate = _nLeft = (int)( _dblBytes2Copy / 4000000 );

			pbr.Maximum = _alFilesToCopy.Count;
			_dateStart = Global.Now;
			timer1.Enabled = true;
		}

		public static DialogResult showDialog(
			Form parent,
			bool fRestore,
			string strText,
			string strSrc,
			string strDst )
		{
			using ( var dlg = new FBackup(fRestore,strText,strSrc,strDst) )
				return dlg.ShowDialog( parent );
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			if ( _alFilesToCopy.Count == 0 )
			{
				this.DialogResult = DialogResult.Cancel;
				return;
			}

			var fi = _alFilesToCopy[0] as FileInfo;
			_alFilesToCopy.RemoveAt( 0 );

			lblFile.Text = fi.Name;
			lblFile.Refresh();
			try
			{
				File.Copy( Path.Combine(_strSrc,fi.Name), Path.Combine(_strDst,fi.Name), true );
			}
			catch ( Exception ex )
			{
				string strErr = string.Format( "Ett fel inträffade vid kopiering av filen \"{0}\"\r\n\r\nVill du fortsätta?\r\n\r\nFelet var:\r\n{1}",
					fi.Name, ex.ToString() );
				if ( Global.askMsgBox( this, strErr, true ) != DialogResult.Yes )
				{
					timer1.Enabled = false;
					this.DialogResult = DialogResult.Cancel;
					return;
				}
			}

			_dblBytesCopied += fi.Length;
			TimeSpan ts = Global.Now - _dateStart;
			if ( ts.TotalSeconds>2 )
			{
				int nTotEstimate = (int)(ts.TotalSeconds * _dblBytes2Copy / _dblBytesCopied);
				_nTotEstimate = (_nTotEstimate*4+nTotEstimate)/5;
				int nLeft = _nTotEstimate - (int)ts.TotalSeconds;
				_nLeft = (_nLeft+nLeft)/2;
				if ( ts.TotalSeconds>5 && _nLeft>=0 )
					lblTime.Text = string.Format( "{0}:{1:00}  ({2:0.0}Mb/s)", _nLeft/60, _nLeft%60,
						(_dblBytesCopied/ts.TotalSeconds)/1048576  );
			}

			pbr.Increment( 1 );
			if ( _alFilesToCopy.Count==0 )
			{
				timer1.Enabled = false;
				if ( !_fRestore )
					Global.showMsgBox( this, "Backuptagning klar!" );
				this.DialogResult = DialogResult.OK;

				var strFN = Path.Combine( Global.Preferences.MainPath, "_backup.log" );
				using ( var sw = new StreamWriter(strFN,true) )
					sw.WriteLine(
						"{0}\t{1}\t{2}",
						Global.Skola.OrderNr,
						Global.Skola.Namn,
						Global.Skola.BackupWhen = vdUsr.DateHelper.YYYYMMDDHHMM( Global.Now ) );
			}
		}

	}

}
