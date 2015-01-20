using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Plata.Camera;
using vdCamera;

namespace Plata
{

	public class FKollaKamera : System.Windows.Forms.Form
	{
		static private bool s_fKameraInst_VarnaInte = false;
		static private bool s_fKamera_HarTagitPorträtt = false;
		static private bool s_fKamera_HarTagitNågonBild = false;

		private enum AnvändarensSvar
		{
			Kasta,
			OK_ändra_inte_standard,
			OK_ändra_standard,
			OK_varna_inte_alls
		}

		private System.Windows.Forms.ListView lv;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.Button cmdOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private readonly string[] _astrParam = {
			"Slutartid", "Bländarsteg", "ISO-tal", "Vitbalans", "Kelvinvärde", "Mode", "Parameter", "Färgmatris", "Filformat" };
		private readonly string[] _astrCurrent;
		private readonly string[] _astrPreset;
		private System.Windows.Forms.RadioButton optKasta;
		private System.Windows.Forms.RadioButton optShutUp;
		private System.Windows.Forms.RadioButton optOK_newdefault;
		private System.Windows.Forms.RadioButton optOK_single;

		private AnvändarensSvar m_AnvändarensSvar = AnvändarensSvar.Kasta;

		public FKollaKamera( string[] astrCurrent, string[] astrPreset )
		{
			InitializeComponent();
			_astrCurrent = astrCurrent;
			_astrPreset = astrPreset;
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
            this.lv = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.optKasta = new System.Windows.Forms.RadioButton();
            this.optOK_newdefault = new System.Windows.Forms.RadioButton();
            this.cmdOK = new System.Windows.Forms.Button();
            this.optShutUp = new System.Windows.Forms.RadioButton();
            this.optOK_single = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // lv
            // 
            this.lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lv.Location = new System.Drawing.Point(12, 8);
            this.lv.Name = "lv";
            this.lv.Size = new System.Drawing.Size(442, 174);
            this.lv.TabIndex = 0;
            this.lv.UseCompatibleStateImageBehavior = false;
            this.lv.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 140;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Standardvärden";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader2.Width = 140;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Aktuella värden";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader3.Width = 140;
            // 
            // optKasta
            // 
            this.optKasta.AutoSize = true;
            this.optKasta.Checked = true;
            this.optKasta.Location = new System.Drawing.Point(44, 188);
            this.optKasta.Name = "optKasta";
            this.optKasta.Size = new System.Drawing.Size(355, 17);
            this.optKasta.TabIndex = 1;
            this.optKasta.TabStop = true;
            this.optKasta.Text = "Förlåt. Det var inte meningen. Snälla kasta bort bilden så tar jag en ny!";
            // 
            // optOK_newdefault
            // 
            this.optOK_newdefault.AutoSize = true;
            this.optOK_newdefault.Location = new System.Drawing.Point(44, 244);
            this.optOK_newdefault.Name = "optOK_newdefault";
            this.optOK_newdefault.Size = new System.Drawing.Size(330, 17);
            this.optOK_newdefault.TabIndex = 3;
            this.optOK_newdefault.Text = "Gör dessa värden till mina standardvärden för den här sessionen.";
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(196, 312);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(80, 28);
            this.cmdOK.TabIndex = 5;
            this.cmdOK.Text = "OK";
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // optShutUp
            // 
            this.optShutUp.AutoSize = true;
            this.optShutUp.Location = new System.Drawing.Point(44, 272);
            this.optShutUp.Name = "optShutUp";
            this.optShutUp.Size = new System.Drawing.Size(234, 17);
            this.optShutUp.TabIndex = 4;
            this.optShutUp.Text = "Varna mig inte mer under den här sessionen.";
            // 
            // optOK_single
            // 
            this.optOK_single.AutoSize = true;
            this.optOK_single.Location = new System.Drawing.Point(44, 216);
            this.optOK_single.Name = "optOK_single";
            this.optOK_single.Size = new System.Drawing.Size(332, 17);
            this.optOK_single.TabIndex = 2;
            this.optOK_single.Text = "Det är OK för den här bilden - men varna mig om det händer igen.";
            // 
            // FKollaKamera
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(466, 352);
            this.Controls.Add(this.optOK_single);
            this.Controls.Add(this.optShutUp);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.optOK_newdefault);
            this.Controls.Add(this.optKasta);
            this.Controls.Add(this.lv);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FKollaKamera";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Varning: Ny kamerainställning!";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			Font fontBold = new Font(lv.Font,FontStyle.Bold);
			for ( int i=0 ; i<_astrParam.Length ; i++ )
			{
				ListViewItem itm = lv.Items.Add( _astrParam[i] );
				itm.UseItemStyleForSubItems = true;
				if ( string.Compare(_astrCurrent[i],_astrPreset[i])!=0 && i!=7 )
					itm.Font = fontBold;
				itm.SubItems.Add( _astrCurrent[i] );
				itm.SubItems.Add( _astrPreset[i] );
			}
		}

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			if ( optOK_single.Checked )
				m_AnvändarensSvar = AnvändarensSvar.OK_ändra_inte_standard;
			else if ( optOK_newdefault.Checked )
				m_AnvändarensSvar = AnvändarensSvar.OK_ändra_standard;
			else if ( optShutUp.Checked )
				m_AnvändarensSvar = AnvändarensSvar.OK_varna_inte_alls;
		}

		private AnvändarensSvar Svar
		{
			get { return m_AnvändarensSvar; }
		}

///////////////////////////////////////////////////////////////////////////////////////////

		//i'm not supporting two different camera types during the same session!
		private static readonly Hashtable s_hashDefaultSettings = new Hashtable();

		private static eosPresets.Preset getDefaultSettings( eosPresets.PresetType pt, vdCamera.CameraType cameraType )
		{
			if ( s_hashDefaultSettings.ContainsKey(pt) )
				return (eosPresets.Preset)s_hashDefaultSettings[pt];
			return eosPresets.GetPreset( pt, cameraType );
		}

		private static string[] cameraSettingsToText( vdCamera.vdCamera camera, eosPresets.Preset preset )
		{
			var result = new string[9];

		    var tr = camera.Translation;
            result[0] = tr.ShutterSpeed.Text(preset.TV, string.Empty);
            result[1] = tr.Aperture.Text(preset.AV, string.Empty);
            result[2] = tr.ISO.Text(preset.ISO);
            result[3] = tr.WhiteBalance.Text(preset.WB);
			result[4] = preset.KelvinText;
            result[5] = tr.ShootingMode.Text(preset.Mode);
            result[6] = string.Format("{0}/{1}/{2}/{3}/{4}", tr.Parameters.Text(preset.ParameterSet), preset.SharpnessText, preset.ContrastText, preset.SaturationText, preset.ColorToneText);
            result[7] = tr.ColorMatrix.Text(preset.ColorMatrix);
            result[8] = preset.ImageTypeSize;
			return result;
		}

		public static bool kollaKamera( FMain frmMain, eosPresets.PresetType pt )
		{
			if ( s_fKameraInst_VarnaInte )
				return true;

            var camera = frmMain.Camera;

            if (!s_fKamera_HarTagitNågonBild)
                switch (camera.CameraType)
                {
                    case CameraType.EOS_5D:
                    case CameraType.EOS_5D_MarkII:
                        const string info =
                            "Plåta kan för närvarande inte kontrollera vissa viktiga inställningar på en 5D-kamera. " +
                            "Snälla, kontrollera följande inställning:\r\n" +
                            " * Färgrymd ska vara \"sRGB\"\r\n" +
                            "Stämmer detta?";
                        if (Global.askMsgBox(frmMain, info, true) != DialogResult.Yes)
                        {
                            Global.showMsgBox(frmMain, "Ändra inställningen och ta om bilden!");
                            return false;
                        }
                        break;
                }
		    s_fKamera_HarTagitNågonBild = true;

			if ( pt==eosPresets.PresetType.IndoorPortrait && !s_fKamera_HarTagitPorträtt )
			{
				s_fKamera_HarTagitPorträtt = true;
				if ( Global.askMsgBox( frmMain, "Har du fotograferat ett gråkort och ställt vitbalansen efter det?", true ) != DialogResult.Yes )
				{
					Global.showMsgBox( frmMain, "Gör det först!" );
					return false;
				}
			}

			var currentSettings = eosPresets.Preset.GetCurrentCameraSettings(camera);
			var astrCurrent = cameraSettingsToText(camera,getDefaultSettings(pt, camera.CameraType));
			var astrPreset = cameraSettingsToText(camera, currentSettings);
			var fDiff = false;
            for (var i = 0; i < astrCurrent.Length; i++)
                if ( i != 7 && astrCurrent[i].Length!=0 && astrCurrent[i] != "N/A" )
					if ( string.Compare(astrCurrent[i],astrPreset[i])!=0 )
						fDiff = true;
			if ( fDiff )
				using ( var dlg = new FKollaKamera(astrCurrent,astrPreset) )
				{
					dlg.ShowDialog(frmMain);
					switch ( dlg.Svar )
					{
						case AnvändarensSvar.OK_ändra_inte_standard:
							return true;
						case AnvändarensSvar.OK_ändra_standard:
							if ( s_hashDefaultSettings.ContainsKey(pt) )
								s_hashDefaultSettings.Remove( pt );
							s_hashDefaultSettings.Add( pt, currentSettings );
							return true;
						case AnvändarensSvar.OK_varna_inte_alls:
							s_fKameraInst_VarnaInte = true;
							return true;
						default:
							//kasta bilden
							return false;
					}
				}
			return true;
		}

	}

}
