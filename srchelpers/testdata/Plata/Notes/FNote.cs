using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace Plata.Notes
{

	public class FNote : vdUsr.baseGradientForm
	{
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
		private vdUsr.vdDatePicker dtp;
		private RadioButton optNoDate;
		private RadioButton optDatum;
		private ComboBox cboOrder;
		private Label label2;
		private TextBox txt;
		private Button cmdDelete;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private FNote()
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.dtp = new vdUsr.vdDatePicker();
            this.optNoDate = new System.Windows.Forms.RadioButton();
            this.optDatum = new System.Windows.Forms.RadioButton();
            this.cboOrder = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt = new System.Windows.Forms.TextBox();
            this.cmdDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(437, 12);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(80, 28);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "OK";
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(523, 12);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(80, 28);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "Avbryt";
            // 
            // dtp
            // 
            this.dtp.AutoSize = true;
            this.dtp.BackColor = System.Drawing.SystemColors.Control;
            this.dtp.Location = new System.Drawing.Point(32, 35);
            this.dtp.Name = "dtp";
            this.dtp.Size = new System.Drawing.Size(130, 20);
            this.dtp.TabIndex = 5;
            this.dtp.DateChanged += new System.EventHandler(this.dtp_DateChanged);
            // 
            // optNoDate
            // 
            this.optNoDate.AutoSize = true;
            this.optNoDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.optNoDate.Location = new System.Drawing.Point(12, 12);
            this.optNoDate.Name = "optNoDate";
            this.optNoDate.Size = new System.Drawing.Size(81, 17);
            this.optNoDate.TabIndex = 3;
            this.optNoDate.TabStop = true;
            this.optNoDate.Text = "&Inget datum";
            this.optNoDate.UseVisualStyleBackColor = false;
            // 
            // optDatum
            // 
            this.optDatum.AutoSize = true;
            this.optDatum.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.optDatum.Location = new System.Drawing.Point(12, 35);
            this.optDatum.Name = "optDatum";
            this.optDatum.Size = new System.Drawing.Size(14, 13);
            this.optDatum.TabIndex = 4;
            this.optDatum.TabStop = true;
            this.optDatum.UseVisualStyleBackColor = false;
            // 
            // cboOrder
            // 
            this.cboOrder.FormattingEnabled = true;
            this.cboOrder.Location = new System.Drawing.Point(197, 35);
            this.cboOrder.MaxLength = 6;
            this.cboOrder.Name = "cboOrder";
            this.cboOrder.Size = new System.Drawing.Size(81, 21);
            this.cboOrder.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label2.Location = new System.Drawing.Point(194, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "&Ordernummer";
            // 
            // txt
            // 
            this.txt.AcceptsReturn = true;
            this.txt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt.Location = new System.Drawing.Point(12, 61);
            this.txt.Multiline = true;
            this.txt.Name = "txt";
            this.txt.Size = new System.Drawing.Size(591, 278);
            this.txt.TabIndex = 0;
            // 
            // cmdDelete
            // 
            this.cmdDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDelete.DialogResult = System.Windows.Forms.DialogResult.No;
            this.cmdDelete.Location = new System.Drawing.Point(328, 12);
            this.cmdDelete.Name = "cmdDelete";
            this.cmdDelete.Size = new System.Drawing.Size(80, 28);
            this.cmdDelete.TabIndex = 8;
            this.cmdDelete.TabStop = false;
            this.cmdDelete.Text = "Radera";
            // 
            // FNote
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(615, 351);
            this.Controls.Add(this.cmdDelete);
            this.Controls.Add(this.txt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboOrder);
            this.Controls.Add(this.optDatum);
            this.Controls.Add(this.optNoDate);
            this.Controls.Add(this.dtp);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FNote";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Notering";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		public static DialogResult showDialog(
			Form parent,
			Note note,
			bool fCanDelete )
		{
			using ( FNote dlg = new FNote() )
			{
				dlg.cmdDelete.Visible = fCanDelete;
				foreach ( string s in Directory.GetDirectories( Global.Preferences.MainPath ) )
				{
					int i;
					if ( int.TryParse( s.Substring( s.LastIndexOf('_') + 1 ), out i ) )
						if ( i > 400000 )
							dlg.cboOrder.Items.Add( i.ToString() );
				}
				if ( note.RegardingDate != DateTime.MinValue )
					dlg.dtp.Date = note.RegardingDate;
				if ( note.OrderNumber != 0 )
					dlg.cboOrder.Text = note.OrderNumber.ToString();
				dlg.txt.Text = note.Text;
				DialogResult retVal = dlg.ShowDialog( parent );
				if ( retVal == DialogResult.OK )
				{
					note.Text = dlg.txt.Text;
					note.RegardingDate = dlg.optDatum.Checked ?
						dlg.dtp.Date : DateTime.MinValue;
					int.TryParse( dlg.cboOrder.Text, out note.OrderNumber ); 
				}
				return retVal;
			}
		}

		private void dtp_DateChanged( object sender, EventArgs e )
		{
			optDatum.Checked = true;
		}


	}

}
