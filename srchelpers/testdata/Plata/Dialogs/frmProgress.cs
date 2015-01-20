using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Plata
{
	/// <summary>
	/// Summary description for frmProgress.
	/// </summary>
	public class frmProgress : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ProgressBar pbr;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private int m_nMax;

		public frmProgress( string strCaption, int nMax )
		{
			InitializeComponent();
			this.Text = strCaption;
			pbr.Maximum = nMax;
			m_nMax = nMax;
		}

		public frmProgress( string strCaption ) : this(strCaption,100)
		{
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
			this.pbr = new System.Windows.Forms.ProgressBar();
			this.SuspendLayout();
			// 
			// pbr
			// 
			this.pbr.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pbr.Location = new System.Drawing.Point(0, 0);
			this.pbr.Name = "pbr";
			this.pbr.Size = new System.Drawing.Size(408, 28);
			this.pbr.TabIndex = 0;
			// 
			// frmProgress
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(408, 28);
			this.ControlBox = false;
			this.Controls.Add(this.pbr);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmProgress";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.ResumeLayout(false);

		}
		#endregion

		public void setProgress( int nValue, int nMax )
		{
			try
			{
				if ( m_nMax!=nMax )
				{
					pbr.Value = 0;
					pbr.Maximum = nMax;
					m_nMax = nMax;
				}
				pbr.Value = nValue;
				pbr.Update();
			} 
			catch
			{
			}
		}

		public void setProgress( int nValue )
		{
			pbr.Value = nValue;
		}

		public void increaseValue()
		{
			setProgress( pbr.Value+1 );
		}

	}

}
