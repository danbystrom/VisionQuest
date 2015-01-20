using System;
using System.Collections.Generic;
using System.IO;
using vdCamera;

namespace Plata.Camera
{

	public class eosPresets
	{
		public enum PresetType
		{
			IndoorGroup,
            OutdoorGroup,
            IndoorPortrait,
            OutdoorPortrait,
            IndoorPortraitCompany,
            IndoorInsets,
			OutdoorInsets,
			Environment,
			Unknown
		}

		public class Preset
		{
			public readonly int TV;
			public readonly int AV;
			public readonly int ISO;
			public readonly int WB;
			public readonly int Mode;
			public readonly int ParameterSet;
			public readonly int Sharpness;
			public readonly int Contrast;
            public readonly int Saturation;
            public readonly int ColorTone;
			public readonly int ColorMatrix;
            public readonly int ColorSpace;
            public readonly int Kelvin;
			public readonly string ImageTypeSize;
			public Preset(
				int tv,
				int av,
				int iso,
				int wb,
				int mode,
				int parameterSet,
				int sharpness,
				int contrast,
				int saturation,
				int colorTone,
				int colorMatrix,
                int colorSpace,
                int kelvinValue,
				string its )
			{
				TV = tv;
				AV = av;
				ISO = iso;
				WB = wb;
				Mode = mode;
				ParameterSet = parameterSet;
				Sharpness = sharpness;
				Contrast = contrast;
				Saturation = saturation;
				ColorTone = colorTone;
				ColorMatrix = colorMatrix;
                ColorSpace = colorSpace;
                Kelvin = kelvinValue;
				ImageTypeSize = its;
			}

            private static string toString(int x)
            {
                return (x&0xffff) != 0xffff
                    ? x.ToString()
                    : "na";
            }

            public string KelvinText
            {
                get { return toString(Kelvin); }
            }

            public string SaturationText
            {
                get { return toString(Saturation); }
            }

            public string SharpnessText
            {
                get { return toString(Sharpness); }
            }

            public string ContrastText
            {
                get { return toString(Contrast); }
            }

            public string ColorToneText
            {
                get { return toString(ColorTone ); }
            }

            public void Apply(vdCamera.vdCamera camera)
			{
				if ( !camera.IsConnected )
					return;

				camera.BatchBeginSet();
				if ( Mode != 0xffff )
					camera.SetShootingMode( Mode );
				if ( TV != 0xffff )
					camera.SetTvValue( TV );
				if ( AV != 0xffff )
					camera.SetAvValue( AV );
				if ( ISO != 0xffff )
					camera.SetISONumber( ISO );
				if ( WB != 0xffff )
					camera.SetWhiteBalance( WB, Kelvin  );
				if ( ColorMatrix != 0xffff )
					camera.SetColorMatrix( ColorMatrix );
				camera.SetParameterSharpnessContrast( ParameterSet, Sharpness, Contrast, Saturation, ColorTone );
                camera.SetImageFormatAttribute(ImageTypeSize);
				camera.BatchEnd();
			}

			public static Preset GetCurrentCameraSettings( vdCamera.vdCamera camera )
			{
				int nP, nS1, nC1, nS2, nC2, whiteBalance, kelvin;
				camera.GetParameterSharpnessContrast( out nP, out nS1, out nC1, out nS2, out nC2 );
                camera.GetWhiteBalance(out whiteBalance, out kelvin);
				return new Preset(
					camera.GetTvValue(),
					camera.GetAvValue(),
					camera.GetISONumber(),
                    whiteBalance,
					camera.GetShootingMode(),
					nP,
					nS1,
					nC1,
					nS2,
					nC2,
					camera.GetColorMatrix(),
                    0xffff,
                    kelvin,
					camera.GetImageFormatAttribute() );
			}
		}

        private static readonly Dictionary<CameraType,Dictionary<PresetType,Preset>> _presets = new Dictionary<CameraType,Dictionary<PresetType,Preset>>();

        static eosPresets()
        {
            try
            {
                var lines = File.ReadAllLines(Path.Combine(
                    Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                    "camerasettings.txt"));
                if (lines[0] != "camera\ttab\ttv\tav\tiso\twb\tmode\tparam\tsharpness\tcontrast\tsaturation\tcolortone\tcolormatrix\tcolorspace\tkelvin\timageformat")
                    throw new Exception();
                Dictionary<PresetType, Preset> dic = null;
                for (var i = 1; i < lines.Length; i++)
                {
                    System.Diagnostics.Debug.Print(lines[i]);
                    var line = lines[i].Split('\t');
                    for (var j = 0; j < line.Length; j++)
                        line[j] = line[j].Trim();
                    switch (line[0])
                    {
                        case "":
                            break;
                        case "EOS-1Ds Mark II":
                            _presets.Add(CameraType.EOS_1Ds_MarkII, dic = new Dictionary<PresetType, Preset>());
                            continue;
                        case "EOS 5D":
                            _presets.Add(CameraType.EOS_5D, dic = new Dictionary<PresetType, Preset>());
                            continue;
                        case "EOS 5D Mark II":
                            _presets.Add(CameraType.EOS_5D_MarkII, dic = new Dictionary<PresetType, Preset>());
                            continue;
                        case "Fuji S5":
                            _presets.Add(CameraType.FujiS5, dic = new Dictionary<PresetType, Preset>());
                            continue;
                        default:
                            throw new Exception();
                    }
                    if (line.Length < 3 || line[1].Length == 0)
                        continue;

                    dic.Add(
                        (PresetType)Enum.Parse(typeof(PresetType), line[1]),
                        new Preset(
                            parse(line[2]),
                            parse(line[3]),
                            parse(line[4]),
                            parse(line[5]),
                            parse(line[6]),
                            parse(line[7]),
                            parse(line[8]),
                            parse(line[9]),
                            parse(line[10]),
                            parse(line[11]),
                            parse(line[12]),
                            parse(line[13]),
                            parse(line[14]),
                            line[15]));
                }
            }
            catch ( Exception ex )
            {
                throw new Exception("Fel vid läsning av \"camerasettings.txt\"\r\n\r\n" + ex.ToString());
            }
        }

        private static int parse(string x)
        {
            try
            {
                if (string.IsNullOrEmpty(x))
                    return 0xffff;
                if (x.StartsWith("0x"))
                    return Convert.ToInt32(x.Substring(2), 16);
                return int.Parse(x);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

	    public static Preset GetPreset(PresetType pt, CameraType cameraType)
		{
		    Dictionary<PresetType,Preset> dic;
		    Preset preset;

            if (!_presets.TryGetValue(cameraType, out dic))
                return null;
            if (!dic.TryGetValue(pt, out preset))
                return null;
		    return preset;
		}

        public static void ApplyPreset(PresetType pt, vdCamera.vdCamera camera)
		{
		    var preset = GetPreset(pt,camera.CameraType);
            if ( preset != null )
                preset.Apply( camera );
		}

        public static bool UsesRaw(PresetType pt, vdCamera.vdCamera camera)
        {
            var preset = GetPreset(pt, camera.CameraType);
            return preset != null && preset.ImageTypeSize.StartsWith("raw",StringComparison.OrdinalIgnoreCase);
        }

	}

}
