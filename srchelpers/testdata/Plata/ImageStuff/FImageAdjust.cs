using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Plata
{
	/// <summary>
	/// Summary description for FImageAdjust.
	/// </summary>
	public class FImageAdjust : System.Windows.Forms.Form
	{
		public delegate void AdjustmentChange( object sender, ImageAttributes ia );

		private AdjustmentChange _adjustmentChangeCallback;
		private ImageAttributes _ia = new ImageAttributes();
		private Hashtable _hash = new Hashtable();

		private System.Windows.Forms.HScrollBar hscBlue;
		private System.Windows.Forms.HScrollBar hscGreen;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.HScrollBar hscRed;
		private System.Windows.Forms.HScrollBar hscSaturation;
		private System.Windows.Forms.HScrollBar hscContrast;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.HScrollBar hscBrightness;
		private System.Windows.Forms.Label lblBlue;
		private System.Windows.Forms.Label lblGreen;
		private System.Windows.Forms.Label lblRed;
		private System.Windows.Forms.Label lblSaturation;
		private System.Windows.Forms.Label lblContrast;
		private System.Windows.Forms.Label lblBrightness;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCompare;
		private System.Windows.Forms.Button cmdReset;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private FImageAdjust()
		{
			InitializeComponent();
			_hash.Add( hscBrightness, lblBrightness );
			_hash.Add( hscContrast, lblContrast );
			_hash.Add( hscSaturation, lblSaturation );
			_hash.Add( hscRed, lblRed );
			_hash.Add( hscGreen, lblGreen );
			_hash.Add( hscBlue, lblBlue );
			foreach ( ScrollBar scb in _hash.Keys )
				scb.Maximum += scb.LargeChange - 1;
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
			this.lblBlue = new System.Windows.Forms.Label();
			this.lblGreen = new System.Windows.Forms.Label();
			this.lblRed = new System.Windows.Forms.Label();
			this.lblSaturation = new System.Windows.Forms.Label();
			this.lblContrast = new System.Windows.Forms.Label();
			this.lblBrightness = new System.Windows.Forms.Label();
			this.hscBlue = new System.Windows.Forms.HScrollBar();
			this.hscGreen = new System.Windows.Forms.HScrollBar();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.hscRed = new System.Windows.Forms.HScrollBar();
			this.hscSaturation = new System.Windows.Forms.HScrollBar();
			this.hscContrast = new System.Windows.Forms.HScrollBar();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.hscBrightness = new System.Windows.Forms.HScrollBar();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdCompare = new System.Windows.Forms.Button();
			this.cmdReset = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblBlue
			// 
			this.lblBlue.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.lblBlue.Enabled = false;
			this.lblBlue.Location = new System.Drawing.Point( 528, 56 );
			this.lblBlue.Name = "lblBlue";
			this.lblBlue.Size = new System.Drawing.Size( 38, 16 );
			this.lblBlue.TabIndex = 21;
			this.lblBlue.Text = "0%";
			this.lblBlue.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblGreen
			// 
			this.lblGreen.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.lblGreen.Enabled = false;
			this.lblGreen.Location = new System.Drawing.Point( 528, 32 );
			this.lblGreen.Name = "lblGreen";
			this.lblGreen.Size = new System.Drawing.Size( 38, 16 );
			this.lblGreen.TabIndex = 20;
			this.lblGreen.Text = "0%";
			this.lblGreen.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblRed
			// 
			this.lblRed.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.lblRed.Enabled = false;
			this.lblRed.Location = new System.Drawing.Point( 528, 8 );
			this.lblRed.Name = "lblRed";
			this.lblRed.Size = new System.Drawing.Size( 38, 16 );
			this.lblRed.TabIndex = 19;
			this.lblRed.Text = "0%";
			this.lblRed.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblSaturation
			// 
			this.lblSaturation.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.lblSaturation.Location = new System.Drawing.Point( 248, 56 );
			this.lblSaturation.Name = "lblSaturation";
			this.lblSaturation.Size = new System.Drawing.Size( 38, 16 );
			this.lblSaturation.TabIndex = 18;
			this.lblSaturation.Text = "0%";
			this.lblSaturation.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblContrast
			// 
			this.lblContrast.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.lblContrast.Location = new System.Drawing.Point( 248, 32 );
			this.lblContrast.Name = "lblContrast";
			this.lblContrast.Size = new System.Drawing.Size( 38, 16 );
			this.lblContrast.TabIndex = 17;
			this.lblContrast.Text = "0%";
			this.lblContrast.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblBrightness
			// 
			this.lblBrightness.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.lblBrightness.Location = new System.Drawing.Point( 248, 8 );
			this.lblBrightness.Name = "lblBrightness";
			this.lblBrightness.Size = new System.Drawing.Size( 38, 16 );
			this.lblBrightness.TabIndex = 16;
			this.lblBrightness.Text = "0%";
			this.lblBrightness.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// hscBlue
			// 
			this.hscBlue.Enabled = false;
			this.hscBlue.Location = new System.Drawing.Point( 336, 56 );
			this.hscBlue.Minimum = -100;
			this.hscBlue.Name = "hscBlue";
			this.hscBlue.Size = new System.Drawing.Size( 184, 16 );
			this.hscBlue.TabIndex = 11;
			this.hscBlue.TabStop = true;
			this.hscBlue.Scroll += new System.Windows.Forms.ScrollEventHandler( this.hsc_Scroll );
			// 
			// hscGreen
			// 
			this.hscGreen.Enabled = false;
			this.hscGreen.Location = new System.Drawing.Point( 336, 32 );
			this.hscGreen.Minimum = -100;
			this.hscGreen.Name = "hscGreen";
			this.hscGreen.Size = new System.Drawing.Size( 184, 16 );
			this.hscGreen.TabIndex = 9;
			this.hscGreen.TabStop = true;
			this.hscGreen.Scroll += new System.Windows.Forms.ScrollEventHandler( this.hsc_Scroll );
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label4.Enabled = false;
			this.label4.Location = new System.Drawing.Point( 304, 56 );
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size( 28, 13 );
			this.label4.TabIndex = 10;
			this.label4.Text = "&Blått";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label5.Enabled = false;
			this.label5.Location = new System.Drawing.Point( 304, 32 );
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size( 33, 13 );
			this.label5.TabIndex = 8;
			this.label5.Text = "&Grönt";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label6.Enabled = false;
			this.label6.Location = new System.Drawing.Point( 304, 8 );
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size( 27, 13 );
			this.label6.TabIndex = 6;
			this.label6.Text = "&Rött";
			// 
			// hscRed
			// 
			this.hscRed.Enabled = false;
			this.hscRed.Location = new System.Drawing.Point( 336, 8 );
			this.hscRed.Minimum = -100;
			this.hscRed.Name = "hscRed";
			this.hscRed.Size = new System.Drawing.Size( 184, 16 );
			this.hscRed.TabIndex = 7;
			this.hscRed.TabStop = true;
			this.hscRed.Scroll += new System.Windows.Forms.ScrollEventHandler( this.hsc_Scroll );
			// 
			// hscSaturation
			// 
			this.hscSaturation.Location = new System.Drawing.Point( 64, 56 );
			this.hscSaturation.Minimum = -100;
			this.hscSaturation.Name = "hscSaturation";
			this.hscSaturation.Size = new System.Drawing.Size( 184, 16 );
			this.hscSaturation.TabIndex = 5;
			this.hscSaturation.TabStop = true;
			this.hscSaturation.Scroll += new System.Windows.Forms.ScrollEventHandler( this.hsc_Scroll );
			// 
			// hscContrast
			// 
			this.hscContrast.Location = new System.Drawing.Point( 64, 32 );
			this.hscContrast.Minimum = -100;
			this.hscContrast.Name = "hscContrast";
			this.hscContrast.Size = new System.Drawing.Size( 184, 16 );
			this.hscContrast.TabIndex = 3;
			this.hscContrast.TabStop = true;
			this.hscContrast.Scroll += new System.Windows.Forms.ScrollEventHandler( this.hsc_Scroll );
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label3.Location = new System.Drawing.Point( 8, 56 );
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size( 46, 13 );
			this.label3.TabIndex = 4;
			this.label3.Text = "&Mättnad";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label2.Location = new System.Drawing.Point( 8, 32 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 46, 13 );
			this.label2.TabIndex = 2;
			this.label2.Text = "&Kontrast";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label1.Location = new System.Drawing.Point( 8, 8 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 54, 13 );
			this.label1.TabIndex = 0;
			this.label1.Text = "&Ljusstyrka";
			// 
			// hscBrightness
			// 
			this.hscBrightness.Location = new System.Drawing.Point( 64, 8 );
			this.hscBrightness.Minimum = -100;
			this.hscBrightness.Name = "hscBrightness";
			this.hscBrightness.Size = new System.Drawing.Size( 184, 16 );
			this.hscBrightness.TabIndex = 1;
			this.hscBrightness.TabStop = true;
			this.hscBrightness.Scroll += new System.Windows.Forms.ScrollEventHandler( this.hsc_Scroll );
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point( 472, 80 );
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size( 80, 28 );
			this.cmdCancel.TabIndex = 15;
			this.cmdCancel.Text = "Avbryt";
			// 
			// cmdOK
			// 
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point( 384, 80 );
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size( 80, 28 );
			this.cmdOK.TabIndex = 14;
			this.cmdOK.Text = "OK";
			// 
			// cmdCompare
			// 
			this.cmdCompare.Location = new System.Drawing.Point( 200, 80 );
			this.cmdCompare.Name = "cmdCompare";
			this.cmdCompare.Size = new System.Drawing.Size( 80, 28 );
			this.cmdCompare.TabIndex = 12;
			this.cmdCompare.Text = "&Jämför";
			this.cmdCompare.Click += new System.EventHandler( this.cmdCompare_Click );
			// 
			// cmdReset
			// 
			this.cmdReset.Location = new System.Drawing.Point( 288, 80 );
			this.cmdReset.Name = "cmdReset";
			this.cmdReset.Size = new System.Drawing.Size( 80, 28 );
			this.cmdReset.TabIndex = 13;
			this.cmdReset.Text = "&Nollställ";
			this.cmdReset.Click += new System.EventHandler( this.cmdReset_Click );
			// 
			// FImageAdjust
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size( 576, 112 );
			this.Controls.Add( this.cmdCompare );
			this.Controls.Add( this.cmdReset );
			this.Controls.Add( this.cmdCancel );
			this.Controls.Add( this.cmdOK );
			this.Controls.Add( this.lblBlue );
			this.Controls.Add( this.lblGreen );
			this.Controls.Add( this.lblRed );
			this.Controls.Add( this.lblSaturation );
			this.Controls.Add( this.lblContrast );
			this.Controls.Add( this.lblBrightness );
			this.Controls.Add( this.hscBlue );
			this.Controls.Add( this.hscGreen );
			this.Controls.Add( this.label4 );
			this.Controls.Add( this.label5 );
			this.Controls.Add( this.label6 );
			this.Controls.Add( this.hscRed );
			this.Controls.Add( this.hscSaturation );
			this.Controls.Add( this.hscContrast );
			this.Controls.Add( this.label3 );
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.hscBrightness );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.KeyPreview = true;
			this.MinimizeBox = false;
			this.Name = "FImageAdjust";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Bildjustering";
			this.ResumeLayout( false );
			this.PerformLayout();

		}
		#endregion


		public static DialogResult showDialog(
			Form parent,
			AdjustmentChange adjustmentChangeCallback )
		{
			using ( FImageAdjust dlg = new FImageAdjust() )
			{
				dlg.Location = new Point( parent.Left, parent.Bottom-dlg.Height );
				dlg._adjustmentChangeCallback = adjustmentChangeCallback;
				return dlg.ShowDialog();
			}
		}

		private void hsc_Scroll( object sender, ScrollEventArgs args )
		{
			(_hash[sender] as Label).Text = string.Format( "{0}%", args.NewValue );

			float fBrightness = (float)hscBrightness.Value / 100f;
			float fR = fBrightness + (float)hscRed.Value / 100f;
			float fG = fBrightness + (float)hscGreen.Value / 100f;
			float fB = fBrightness + (float)hscBlue.Value / 100f;

			float[][] matrixBrightness =
			{
				new float[] { 1,  0,  0, 0, 0},
				new float[] { 0,  1,  0, 0, 0},
				new float[] { 0,  0,  1, 0, 0},
				new float[] { 0,  0,  0, 1, 0}, 
				new float[] {fR, fG, fB, 0, 1}
			};

			float c = 1 + (float)hscContrast.Value / 100f;
			float[][] matrixContrast =
			{
				new float[] {   c,    0,    0, 0, 0},
				new float[] {   0,    c,    0, 0, 0},
				new float[] {   0,    0,    c, 0, 0},
				new float[] {   0,    0,    0, 1, 0}, 
				new float[] {.05f, .05f, .05f, 0, 1}
			};

			float S = 1 + (float)hscSaturation.Value / 100f;
			float SCompl = 1 - S;
			float SR = 0.3086f * SCompl;
			float SG = 0.6094f * SCompl;
			float SB = 0.0820f * SCompl;
			float[][] matrixSaturation =
			{
				new float[] {SR+S, SR  , SR  , 0, 0 },
				new float[] {SG  , SG+S, SG  , 0, 0 },
				new float[] {SB  , SB  , SB+S, 0, 0 },
				new float[] {0   , 0   , 0   , 1, 0 },
				new float[] {0   , 0   , 0   , 0, 1 }	
			};

			float[][] matrixContrastFix =
			{
				new float[] {    1,     0,     0, 0, 0},
				new float[] {    0,     1,     0, 0, 0},
				new float[] {    0,     0,     1, 0, 0},
				new float[] {    0,     0,     0, 1, 0}, 
				new float[] {-.05f, -.05f, -.05f, 0, 1}
			};

			float[][] matrixAdjust = Multiply(matrixBrightness, matrixContrast );
			matrixAdjust = Multiply( matrixAdjust, matrixSaturation );
			matrixAdjust = Multiply( matrixAdjust, matrixContrastFix );

			_ia.SetColorMatrix(
				new ColorMatrix(matrixAdjust),
				ColorMatrixFlag.Default,
				ColorAdjustType.Bitmap);

			_adjustmentChangeCallback( this, _ia );
		}


		private float[][] Multiply(float[][] f1, float[][] f2)
		{
			float[][] X = new float[5][];
			for (int d = 0; d < 5; d++)
				X[d] = new float[5];
			int size = 5;
			float[] column = new float[5];
			for (int j = 0; j < 5; j++)
			{
				for (int k = 0; k < 5; k++)
					column[k] = f1[k][j];
				for (int i = 0; i < 5; i++)
				{
					float[] row = f2[i];
					float s = 0;
					for (int k = 0; k < size; k++)
						s += row[k] * column[k];
					X[i][j] = s;
				}
			}
			return X;
		}

		private void cmdCompare_Click(object sender, System.EventArgs e)
		{
			_adjustmentChangeCallback( this, new ImageAttributes() );
			vdUsr.vdOneShotTimer.start( 500, new EventHandler(viewSettings), null );
		}

		private void viewSettings( object sender, EventArgs args )
		{
			_adjustmentChangeCallback( this, _ia );
		}

		private void cmdReset_Click(object sender, System.EventArgs e)
		{
			foreach ( ScrollBar scb in _hash.Keys )
				scb.Value = 0;
			foreach ( Label lbl in _hash.Values )
				lbl.Text = "0%";
			_adjustmentChangeCallback( this, _ia=new ImageAttributes() );
		}

	}

}
