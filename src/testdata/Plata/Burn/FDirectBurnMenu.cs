using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Photomic.ArchiveStuff.Core;
using PlataDM;
using Photomic.ArchiveStuff;
using Photomic.Common;

namespace Plata.Burn
{
	/// <summary>
	/// Summary description for FPassword.
	/// </summary>
	public class FDirectBurnMenu : vdUsr.baseGradientForm
	{
		private System.Windows.Forms.Button cmdCancel;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private static string s_SökFörnamn = string.Empty;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button cmdBurnPhotoArkiv;
		private System.Windows.Forms.Button cmdBurnProgArkiv;
		private System.Windows.Forms.Button cmdBurnEnvironment;
		private System.Windows.Forms.Button cmdBurnSport;
		private System.Windows.Forms.Button cmdBurnSpecial;
		private System.Windows.Forms.CheckBox chkUnlock1;
		private System.Windows.Forms.CheckBox chkUnlock2;
		private System.Windows.Forms.Label lblPhotoArkiv;
		private System.Windows.Forms.Label lblProgArkiv;
		private System.Windows.Forms.Label label1;
		private Label label2;
		private Button cmdBurnPEAB;
		private System.Windows.Forms.TextBox txtStatus;

		private FDirectBurnMenu()
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
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdBurnPhotoArkiv = new System.Windows.Forms.Button();
			this.cmdBurnProgArkiv = new System.Windows.Forms.Button();
			this.cmdBurnEnvironment = new System.Windows.Forms.Button();
			this.cmdBurnSport = new System.Windows.Forms.Button();
			this.cmdBurnSpecial = new System.Windows.Forms.Button();
			this.lblPhotoArkiv = new System.Windows.Forms.Label();
			this.lblProgArkiv = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.chkUnlock1 = new System.Windows.Forms.CheckBox();
			this.chkUnlock2 = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtStatus = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cmdBurnPEAB = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point( 372, 504 );
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size( 80, 28 );
			this.cmdCancel.TabIndex = 16;
			this.cmdCancel.Text = "Stäng";
			// 
			// cmdBurnPhotoArkiv
			// 
			this.cmdBurnPhotoArkiv.Location = new System.Drawing.Point( 8, 258 );
			this.cmdBurnPhotoArkiv.Name = "cmdBurnPhotoArkiv";
			this.cmdBurnPhotoArkiv.Size = new System.Drawing.Size( 152, 36 );
			this.cmdBurnPhotoArkiv.TabIndex = 5;
			this.cmdBurnPhotoArkiv.Text = "Bränn &PhotoArkiv";
			this.cmdBurnPhotoArkiv.Click += new System.EventHandler( this.cmdBurnPhotoArkiv_Click );
			// 
			// cmdBurnProgArkiv
			// 
			this.cmdBurnProgArkiv.Location = new System.Drawing.Point( 8, 208 );
			this.cmdBurnProgArkiv.Name = "cmdBurnProgArkiv";
			this.cmdBurnProgArkiv.Size = new System.Drawing.Size( 152, 36 );
			this.cmdBurnProgArkiv.TabIndex = 2;
			this.cmdBurnProgArkiv.Text = "Bränn Program&anpassat Arkiv";
			this.cmdBurnProgArkiv.Click += new System.EventHandler( this.cmdBurnProgArkiv_Click );
			// 
			// cmdBurnEnvironment
			// 
			this.cmdBurnEnvironment.Location = new System.Drawing.Point( 8, 308 );
			this.cmdBurnEnvironment.Name = "cmdBurnEnvironment";
			this.cmdBurnEnvironment.Size = new System.Drawing.Size( 152, 36 );
			this.cmdBurnEnvironment.TabIndex = 8;
			this.cmdBurnEnvironment.Text = "Bränn &Vimmel-CD och/eller Fotoboksprogram";
			this.cmdBurnEnvironment.Click += new System.EventHandler( this.cmdBurnEnvironment_Click );
			// 
			// cmdBurnSport
			// 
			this.cmdBurnSport.Location = new System.Drawing.Point( 8, 358 );
			this.cmdBurnSport.Name = "cmdBurnSport";
			this.cmdBurnSport.Size = new System.Drawing.Size( 152, 36 );
			this.cmdBurnSport.TabIndex = 10;
			this.cmdBurnSport.Text = "Bränn &Sportfotoarkiv";
			this.cmdBurnSport.Click += new System.EventHandler( this.cmdBurnSport_Click );
			// 
			// cmdBurnSpecial
			// 
			this.cmdBurnSpecial.Location = new System.Drawing.Point( 8, 458 );
			this.cmdBurnSpecial.Name = "cmdBurnSpecial";
			this.cmdBurnSpecial.Size = new System.Drawing.Size( 152, 36 );
			this.cmdBurnSpecial.TabIndex = 14;
			this.cmdBurnSpecial.Text = "Bränn Spe&cialarkiv...";
			this.cmdBurnSpecial.Click += new System.EventHandler( this.cmdBurnSpecial_Click );
			// 
			// lblPhotoArkiv
			// 
			this.lblPhotoArkiv.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.lblPhotoArkiv.Location = new System.Drawing.Point( 168, 262 );
			this.lblPhotoArkiv.Name = "lblPhotoArkiv";
			this.lblPhotoArkiv.Size = new System.Drawing.Size( 216, 28 );
			this.lblPhotoArkiv.TabIndex = 6;
			this.lblPhotoArkiv.Text = "Enligt fotoordern skall PhotoArkiv ¤brännas.";
			// 
			// lblProgArkiv
			// 
			this.lblProgArkiv.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.lblProgArkiv.Location = new System.Drawing.Point( 168, 212 );
			this.lblProgArkiv.Name = "lblProgArkiv";
			this.lblProgArkiv.Size = new System.Drawing.Size( 216, 28 );
			this.lblProgArkiv.TabIndex = 3;
			this.lblProgArkiv.Text = "Enligt fotoordern skall Programanpassat Arkiv ¤brännas.";
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label3.Location = new System.Drawing.Point( 168, 312 );
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size( 288, 28 );
			this.label3.TabIndex = 9;
			this.label3.Text = "Vimmel-CD skall alltid brännas som en gåva till skolan.";
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label4.Location = new System.Drawing.Point( 168, 362 );
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size( 288, 28 );
			this.label4.TabIndex = 11;
			this.label4.Text = "Sportfotoarkiv bränns endast vid sportfotojobb.";
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label5.Location = new System.Drawing.Point( 168, 462 );
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size( 288, 28 );
			this.label5.TabIndex = 15;
			this.label5.Text = "Här kan du anpassa ett arkiv för att tillgodose önskemål om filformat, bildstorle" +
					"k och avancerad namnsättning.";
			// 
			// chkUnlock1
			// 
			this.chkUnlock1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.chkUnlock1.Enabled = false;
			this.chkUnlock1.Location = new System.Drawing.Point( 408, 262 );
			this.chkUnlock1.Name = "chkUnlock1";
			this.chkUnlock1.Size = new System.Drawing.Size( 48, 28 );
			this.chkUnlock1.TabIndex = 7;
			this.chkUnlock1.Text = "Lås upp";
			this.chkUnlock1.UseVisualStyleBackColor = false;
			this.chkUnlock1.CheckedChanged += new System.EventHandler( this.chkUnlock1_CheckedChanged );
			// 
			// chkUnlock2
			// 
			this.chkUnlock2.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.chkUnlock2.Enabled = false;
			this.chkUnlock2.Location = new System.Drawing.Point( 408, 212 );
			this.chkUnlock2.Name = "chkUnlock2";
			this.chkUnlock2.Size = new System.Drawing.Size( 48, 28 );
			this.chkUnlock2.TabIndex = 4;
			this.chkUnlock2.Text = "Lås upp";
			this.chkUnlock2.UseVisualStyleBackColor = false;
			this.chkUnlock2.CheckedChanged += new System.EventHandler( this.chkUnlock2_CheckedChanged );
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label1.Location = new System.Drawing.Point( 8, 4 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 40, 13 );
			this.label1.TabIndex = 0;
			this.label1.Text = "Status:";
			// 
			// txtStatus
			// 
			this.txtStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtStatus.Location = new System.Drawing.Point( 8, 22 );
			this.txtStatus.Multiline = true;
			this.txtStatus.Name = "txtStatus";
			this.txtStatus.ReadOnly = true;
			this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtStatus.Size = new System.Drawing.Size( 444, 180 );
			this.txtStatus.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))) );
			this.label2.Location = new System.Drawing.Point( 168, 412 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 288, 28 );
			this.label2.TabIndex = 13;
			this.label2.Text = "Anpassade inställningar för PEAB-företagsfoto";
			// 
			// cmdBurnPEAB
			// 
			this.cmdBurnPEAB.Location = new System.Drawing.Point( 8, 408 );
			this.cmdBurnPEAB.Name = "cmdBurnPEAB";
			this.cmdBurnPEAB.Size = new System.Drawing.Size( 152, 36 );
			this.cmdBurnPEAB.TabIndex = 12;
			this.cmdBurnPEAB.Text = "Bränn arkiv åt PEAB";
			this.cmdBurnPEAB.Click += new System.EventHandler( this.cmdBurnPEAB_Click );
			// 
			// FDirectBurnMenu
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size( 462, 541 );
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.cmdBurnPEAB );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.txtStatus );
			this.Controls.Add( this.chkUnlock2 );
			this.Controls.Add( this.chkUnlock1 );
			this.Controls.Add( this.label5 );
			this.Controls.Add( this.label4 );
			this.Controls.Add( this.label3 );
			this.Controls.Add( this.lblProgArkiv );
			this.Controls.Add( this.lblPhotoArkiv );
			this.Controls.Add( this.cmdBurnSpecial );
			this.Controls.Add( this.cmdBurnSport );
			this.Controls.Add( this.cmdBurnEnvironment );
			this.Controls.Add( this.cmdBurnProgArkiv );
			this.Controls.Add( this.cmdBurnPhotoArkiv );
			this.Controls.Add( this.cmdCancel );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FDirectBurnMenu";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Bränn direkt till kund";
			this.ResumeLayout( false );
			this.PerformLayout();

		}
		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			string strText;
			bool fSevere;

			checkErrorsX( Global.Skola, null, out strText, out fSevere );
			txtStatus.Text = strText;
			txtStatus.SelectionStart = 0;

			if ( !Global.Skola.ShallBurnPhotoCD )
			{
				cmdBurnPhotoArkiv.Enabled = false;
				chkUnlock1.Enabled = true;
			}
			lblPhotoArkiv.Text = lblPhotoArkiv.Text.Replace( "¤", Global.Skola.ShallBurnPhotoCD ? "" : "inte " );

			if ( !Global.Skola.ShallBurnProgCD )
			{
				cmdBurnProgArkiv.Enabled = false;
				chkUnlock2.Enabled = true;
			}
			lblProgArkiv.Text = lblProgArkiv.Text.Replace( "¤", Global.Skola.ShallBurnProgCD ? "" : "inte " );

		}

		private void cmdBurnPhotoArkiv_Click(object sender, System.EventArgs e)
		{
			FCreatePhotoArkiv.showDialog( this, Global.Skola, false );
		}

		private void cmdBurnEnvironment_Click( object sender, System.EventArgs e )
		{
			FCreatePhotoArkiv.showDialog( this, Global.Skola, true );
		}

		private void cmdBurnProgArkiv_Click( object sender, System.EventArgs e )
		{
			var sk = Global.Skola;

			var iff = sk.CustomProgFormat!=ImageFileFormat.Null ? sk.CustomProgFormat : ImageFileFormat.JPG;
			var customName = !string.IsNullOrEmpty(sk.CustomProgNaming) ? sk.CustomProgNaming : "bilder\\?K";
			int nMaxWidth, nMaxHeight;
			if ( sk.CustomProgWidth!=0 || sk.CustomProgHeight!=0 )
			{
				nMaxWidth = sk.CustomProgWidth;
				nMaxHeight = sk.CustomProgHeight;
			}
			else
			{
				nMaxWidth = 256;
				nMaxHeight = 384;
			}

			var generator = new CustomGenerator(
                sk.Namn,
                new [] {sk},
                null,
                new Size(Global.Porträttfotobredd, Global.Porträttfotobredd*3/2),
                null);
			generator.CustomPorträtt.Add( new CustomBurnInfo(
				iff,
				customName,
				nMaxWidth,
				nMaxHeight,
				false,
				PhotoType.Portrait ) );
			generator.GenerateFiles( false, true, true );
			FCreateCustomCD.showDialog( this, generator, null );
		}

		private void cmdBurnSport_Click(object sender, System.EventArgs e)
		{
		    var generator = new CustomGenerator(
		        Global.Skola.Namn,
		        new[] {Global.Skola},
		        null,
		        new Size(Global.Porträttfotobredd, Global.Porträttfotobredd*3/2),
		        null);
		    generator.selectPreset(generator.PresetSport);
		    generator.GenerateFiles(true, true, true);
		    FCreateCustomCD.showDialog(this, generator, "Sportfotoarkiv");
		}

	    private void cmdBurnSpecial_Click(object sender, System.EventArgs e)
		{
			FBurnCustom.showDialog( this );
		}

		public static DialogResult showDialog( Form parent )
		{
			using ( var dlg = new FDirectBurnMenu() )
				return dlg.ShowDialog(parent);
		}

		private void chkUnlock1_CheckedChanged(object sender, System.EventArgs e)
		{
			cmdBurnPhotoArkiv.Enabled = chkUnlock1.Checked;
		}

		private void chkUnlock2_CheckedChanged(object sender, System.EventArgs e)
		{
			cmdBurnProgArkiv.Enabled = chkUnlock2.Checked;
		}

		public static void checkErrorsX(
			Skola skola,
			vdXceed.vdPlainGrid grd,
			out string strText,
			out bool fSevere )
		{
		    var sb = new System.Text.StringBuilder();
		    fSevere = false;

		    {
		        var strOklar = "";
		        var strBeskrSakn = "";
		        var fHasAtLeastOneGroupPicture = false;
		        var dicScanCodesWithPortraits = new Dictionary<string, Person>();

		        foreach (Grupp grupp in skola.Grupper.GrupperIOrdning())
		        {
		            if (grupp.GruppTyp == GruppTyp.GruppInfällning)
		                continue;

		            if (!string.IsNullOrEmpty(grupp.ThumbnailKey) /*&& grupp.Thumbnails.Count != 0*/)
		            {
		                switch (grupp.Numrering)
		                {
		                    case GruppNumrering.Klar:
		                    case GruppNumrering.EjNamnsättning:
		                    case GruppNumrering.EjNumrering:
		                        if (grd == null)
		                            break;
		                        var strTyp = string.Empty;
		                        if ((grupp.Special & TypeOfGroupPhoto.Gruppbild) != 0)
		                            strTyp += "G";
		                        if ((grupp.Special & TypeOfGroupPhoto.Katalog) != 0)
		                            strTyp += "K";
		                        if ((grupp.Special & TypeOfGroupPhoto.Spex) != 0)
		                            strTyp += "P";
		                        if ((grupp.Special & TypeOfGroupPhoto.SkyddadId) != 0)
		                            strTyp += "S";
		                        var row = grd.addRow();
		                        row.Cells[0].Value = grd.G.DataRows.Count;
		                        row.Cells[1].Value = (int) grupp.Numrering;
		                        row.Cells[2].Value = strTyp;
		                        row.Cells[3].Value = grupp.Namn;
		                        row.Cells[4].Value = grupp.Slogan;
		                        row.Cells[5].Value = grupp.PersonerNärvarande.Count;
		                        row.Cells[6].Value = grupp.PersonerFrånvarande.Count;
		                        row.Cells[7].Value = grupp.PersonerSlutat.Count;
		                        if (grupp.AntalGratisEx >= 0)
		                            row.Cells[8].Value = grupp.AntalGratisEx;
		                        row.Cells[9].Value = string.IsNullOrEmpty(grupp.ThumbnailGrayKey) ? "" : "G";
		                        row.EndEdit();
		                        row.Tag = grupp;
		                        break;
		                    default:
		                        strOklar += "  " + grupp.Namn + "\r\n";
		                        break;
		                }
		                fHasAtLeastOneGroupPicture = true;

		                var tn = grupp.Thumbnails[grupp.ThumbnailKey];
		                if (tn == null || !grupp.ThumbnailLocked )
		                {
		                    fSevere = true;
		                    sb.AppendFormat("FEL:\r\nSaknar vald och låst bild för gruppen \"{0}\"! Du måste välja en bild och sätta hänglås på den!\r\n", grupp.Namn);
		                }
		            }
		            else if (grupp.GruppTyp == GruppTyp.GruppNormal)
		            {
		                if (grupp.isAggregated && (grupp.Special & TypeOfGroupPhoto.Gruppbild) == 0)
		                {
		                    grupp.EjFotoOrsak1 = "Övrigt";
		                    grupp.EjFotoOrsak2 = "Samfotad: " + grupp.aggregate.Namn;
		                }
		                else if (!Global.GrpNoPhoto_Reasons_Complete.Contains(grupp.EjFotoOrsak1))
		                    if (grupp.EjFotoOrsak2 == null || grupp.EjFotoOrsak2.Length < 5)
		                        strBeskrSakn += "  " + grupp.Namn + "\r\n";
		            }

                    foreach (var person in grupp.AllaPersoner)
                    {
                        if (string.IsNullOrEmpty(person.ThumbnailKey) && person.Thumbnails.Count == 0)
                            continue;

                        var tn = person.Thumbnails[person.ThumbnailKey];
                        if (tn == null || !person.ThumbnailLocked)
                        {
                            fSevere = true;
                            sb.AppendFormat(
                                "FEL:\r\nSaknar vald och låst bild för \"{0}\" i \"{1}\"! Du måste välja en bild och sätta hänglås på den!\r\n",
                                person.Namn, grupp.Namn);
                        }

                        if (!string.IsNullOrEmpty(person.ScanCode))
                        {
                            Person p;
                            if (dicScanCodesWithPortraits.TryGetValue(person.ScanCode, out p))
                            {
                                fSevere = true;
                                sb.AppendFormat(
                                    "FEL:\r\n\"{0}\" i grupp \"{1}\" och \"{2}\" i grupp \"{3}\" är båda porträttfotade med samma kundid!\r\n",
                                    p.Namn, p.Grupp.Namn,
                                    person.Namn, person.Grupp.Namn);
                            }
                            else
                                dicScanCodesWithPortraits.Add(person.ScanCode, person);
                        }
                    }
		        }

		        foreach (var grupp in skola.MakuleradeGrupper)
		            if (!Global.GrpNoPhoto_Reasons_Complete.Contains(grupp.EjFotoOrsak1))
		                if (grupp.EjFotoOrsak2 == null || grupp.EjFotoOrsak2.Length < 5)
		                    strBeskrSakn += "  " + grupp.Namn + "\r\n";
		        if (strOklar.Length != 0)
		        {
		            sb.Append("FEL:\r\nDu kan inte bränna eftersom följande grupper ännu inte numrerats:\r\n" +
		                      strOklar + "\r\n");
		            fSevere = true;
		        }
		        if (strBeskrSakn.Length != 0 && fHasAtLeastOneGroupPicture)
		        {
		            sb.Append("FEL:\r\nDu kan inte bränna innan du angett skäl till varför följande grupper inte fotograferats:\r\n" +
		                      strBeskrSakn + "\r\n");
		            fSevere = true;
		        }
		    }

		    {
		        var gruppInf = skola.Grupper.GruppMedTyp(GruppTyp.GruppInfällning);
		        if (gruppInf != null)
		            foreach (var person in gruppInf.PersonerNärvarande)
		            {
		                bool fCorrectGroupName = false;
		                foreach (var grupp in skola.Grupper)
		                    if (string.Compare(person.Titel, grupp.Namn, true) == 0)
		                        fCorrectGroupName = true;
		                if (!fCorrectGroupName)
		                {
		                    sb.AppendFormat(
		                        "FEL:\r\nInfällningen {0} är placerad i en grupp som inte finns!!! Åtgärda detta först!\r\n\r\n",
		                        person.Namn);
		                    fSevere = true;
		                }
		            }
		    }

		    {
		        int nAntal, nLovade, nValda;
		        Global.Skola.Vimmel.räkna(out nAntal, out nLovade, out nValda);
		        if (nAntal != 0 && (nLovade + nValda) < 15)
		            sb.Append("VARNING:\r\nDu har inte valt femton vimmelbilder!\r\n\r\n");
		    }

            //if (Global.Preferences.Brand == Brand.Photomic)
            //{
            //    var hasGray = false;
            //    foreach (var grupp in skola.Grupper)
            //        if (grupp.Thumbnails[grupp.ThumbnailGrayKey] != null)
            //            hasGray = true;
            //    if (!hasGray)
            //        sb.Append("VARNING:\r\nDu har inte markerat något gråkort!\r\n\r\n");
            //}

		    strText = sb.ToString();
		}

	    private void cmdBurnPEAB_Click( object sender, EventArgs e )
	    {
	        var generator = new CustomGenerator(
	            Global.Skola.Namn,
	            new [] {Global.Skola},
	            null,
	            new Size(Global.Porträttfotobredd, Global.Porträttfotobredd*3/2),
	            null);
	        generator.CustomPorträtt.Add(new CustomBurnInfo(
	                                         ImageFileFormat.JPG,
	                                         @"?F,?E,?P",
	                                         0,
	                                         0,
	                                         false,
	                                         PhotoType.Portrait));

	        generator.GenerateFiles(false, true, false);
	        FCreateCustomCD.showDialog(this, generator, "PEAB-arkiv");
	    }

	}

}
