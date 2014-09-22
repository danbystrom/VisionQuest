using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Photomic.ArchiveStuff;
using Photomic.ArchiveStuff.Core;
using Photomic.Common.Img;

namespace Plata.Burn
{
	/// <summary>
	/// Summary description for FBurnCustom.
	/// </summary>
	public class FCreateCustomCD : vdUsr.baseGradientForm
	{
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Timer timer1;

		private List<BurnFileInfo> _alDest = new List<BurnFileInfo>();

		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.ProgressBar pbr;

		private JpegSaver _jpgSaver;
		private CustomGenerator _generator;

		private FCreateCustomCD()
		{
			InitializeComponent();
		}

		private FCreateCustomCD( CustomGenerator generator, string strTitle )
		{
			InitializeComponent();

			if ( strTitle != null )
				this.Text = "Skapar " + strTitle;

			_generator = generator;

			_jpgSaver = new JpegSaver( 88 );
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( components != null )
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
			this.cmdCancel = new System.Windows.Forms.Button();
			this.pbr = new System.Windows.Forms.ProgressBar();
			this.timer1 = new System.Windows.Forms.Timer( this.components );
			this.SuspendLayout();
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point( 484, 56 );
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size( 80, 28 );
			this.cmdCancel.TabIndex = 11;
			this.cmdCancel.Text = "Avbryt";
			// 
			// pbr
			// 
			this.pbr.Location = new System.Drawing.Point( 8, 12 );
			this.pbr.Name = "pbr";
			this.pbr.Size = new System.Drawing.Size( 564, 36 );
			this.pbr.TabIndex = 12;
			// 
			// timer1
			// 
			this.timer1.Interval = 20;
			this.timer1.Tick += new System.EventHandler( this.timer1_Tick );
			// 
			// FCreateCustomCD
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.ClientSize = new System.Drawing.Size( 578, 92 );
			this.Controls.Add( this.pbr );
			this.Controls.Add( this.cmdCancel );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FCreateCustomCD";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Skapar Programanpassat Arkiv";
			this.ResumeLayout( false );

		}
		#endregion

		protected override void OnLoad( EventArgs e )
		{
			base.OnLoad( e );
			pbr.Maximum = _generator.ResultArray.Count;
			timer1.Enabled = true;
		}

		public static void showDialog(
			Form parent,
			CustomGenerator generator,
			string strTitle )
		{
			using ( var dlg = new FCreateCustomCD( generator, strTitle ) )
				if ( dlg.ShowDialog( parent ) == DialogResult.Cancel )
				{
					foreach ( var bfi in dlg._alDest )
						Util.safeKillFile( bfi.LocalFullFileName );
				}
		}

		private void timer1_Tick( object sender, System.EventArgs e )
		{
			var cn = _generator.dequeResultNode();
			if ( cn == null )
			{
				timer1.Enabled = false;
				BurnHelpers.theNewAndFunBurn(
					this,
					_alDest,
					"P",
					_generator.Label );
				this.DialogResult = DialogResult.OK;
			}
			else
			{
				_alDest.Add( BurnFileInfo.CreateImageFile(
					Global.GetTempPath(),
					cn,
					_jpgSaver ) );
				pbr.Increment( 1 );
			}

		}

	}

}
