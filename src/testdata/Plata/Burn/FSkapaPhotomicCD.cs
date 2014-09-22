using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Plata
{
	/// <summary>
	/// Summary description for frmBränningPhotomic.
	/// </summary>
	public class FSkapaPhotomicCD : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.RadioButton optFler;
		private System.Windows.Forms.RadioButton optSista;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FSkapaPhotomicCD()
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
			this.optFler = new System.Windows.Forms.RadioButton();
			this.optSista = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// optFler
			// 
			this.optFler.Location = new System.Drawing.Point(28, 64);
			this.optFler.Name = "optFler";
			this.optFler.Size = new System.Drawing.Size(432, 16);
			this.optFler.TabIndex = 0;
			this.optFler.Text = "NEJ - jag är inte klar ännu och kommer att leverera FLER CD-skivor till Photomic";
			// 
			// optSista
			// 
			this.optSista.Location = new System.Drawing.Point(28, 40);
			this.optSista.Name = "optSista";
			this.optSista.Size = new System.Drawing.Size(432, 16);
			this.optSista.TabIndex = 1;
			this.optSista.Text = "JA - det här blir den alla SISTA CD-skivan som bränns för den här ordern";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(448, 20);
			this.label1.TabIndex = 2;
			this.label1.Text = "Blir arbetet avslutat i och med leverans av den här CD-skivan? ";
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(252, 96);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(80, 28);
			this.cmdCancel.TabIndex = 4;
			this.cmdCancel.Text = "Avbryt";
			// 
			// cmdOK
			// 
			this.cmdOK.Location = new System.Drawing.Point(152, 96);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(80, 28);
			this.cmdOK.TabIndex = 3;
			this.cmdOK.Text = "OK";
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// frmSkapaPhotomicCD
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(478, 136);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOK);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.optSista);
			this.Controls.Add(this.optFler);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmSkapaPhotomicCD";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Skapa Photomic CD";
			this.ResumeLayout(false);

		}
		#endregion

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			if ( optSista.Checked )
			{
				string strOklar = "";
				string strOrdning = "";
				foreach ( PlataDM.Grupp grupp in Global.Skola.Grupper.GrupperIOrdning() )
					if ( !Util.isEmpty(grupp.ThumbnailKey) )
						switch ( grupp.Numrering )
						{
							case PlataDM.GruppNumrering.Klar:
							case PlataDM.GruppNumrering.EjNamnsättning:
							case PlataDM.GruppNumrering.EjNumrering:
								strOrdning += grupp.Namn + "\t";//"  " + (i++).ToString() + ". " + grupp.Namn + "\r\n";
								break;
							default:
								strOklar += "  " + grupp.Namn + "\r\n";
								break;
						}

				if ( strOklar.Length!=0 )
					Global.showMsgBox( this, "Du kan inte bränna sista CD:n eftersom följande gruppfotografier ännu inte numrerats:\r\n\r\n" + strOklar );
				else
					this.DialogResult = (new FBekräftaGruppordning(strOrdning)).ShowDialog(this);
			}
			else if ( optFler.Checked )
				this.DialogResult = DialogResult.OK;
			else
				Global.showMsgBox( this, "Du måste markera ett av alternativen!" );

		}

	}

}
