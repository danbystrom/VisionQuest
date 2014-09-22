using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Plata.OpenDialog
{
	/// <summary>
	/// Summary description for baseUsrTab.
	/// </summary>
	public class baseUsrTab : System.Windows.Forms.UserControl
	{
		public event EventHandler Execute;
		public event EventHandler SetOK;
		public event EventHandler SelectionChanged;

		public bool _fImport;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		protected System.ComponentModel.Container components = null;

		protected baseUsrTab()
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// baseUsrTab
			// 
			this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "baseUsrTab";
			this.Size = new System.Drawing.Size(600, 200);

		}
		#endregion

		protected void fireExecute()
		{
			if ( Execute!=null )
				Execute( this, new EventArgs() );
		}

		protected void fireSelectionChanged()
		{
			if ( SelectionChanged!=null )
				SelectionChanged( this, new EventArgs() );
		}

		public virtual bool gotKeyDown(KeyEventArgs e)
		{
			return false;
		}

		public virtual void activate()
		{
		}

		protected void fireOK()
		{
			if ( SetOK!=null )
				SetOK( this, new EventArgs() );
		}

		public virtual bool openOrder( PlataDM.Skola skola )
		{
			return false;
		}

		public virtual bool isOK
		{
			get { return true; }
		}

		public virtual string selectedPath
		{
			get { return null; }
		}

	}

}
