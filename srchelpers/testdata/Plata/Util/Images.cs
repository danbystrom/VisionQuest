using System;
using System.Drawing;

namespace Plata
{
	/// <summary>
	/// Summary description for Images.
	/// </summary>
	public class Images
	{
		public enum Img
		{
			Null,
			FactoryBS,
			FactoryCB,
			FrameBS,
			FrameCB,
			HouseBS,
			HouseCB,
			TreeBS,
			TreeCB,
			Padlock,
			zlastIndex
		}

		private static Bitmap[] _bmp;

		private Images()
		{
		}

		public static void load( Type typ )
		{
			_bmp = new Bitmap[(int)Img.zlastIndex];
			_bmp[(int)Img.FactoryBS] = new Bitmap( typ, "grfx.factory_bs.png" );
			_bmp[(int)Img.FactoryCB] = new Bitmap( typ, "grfx.factory_cb.png" );
			_bmp[(int)Img.FrameBS] = new Bitmap( typ, "grfx.frame_bs.png" );
			_bmp[(int)Img.FrameCB] = new Bitmap( typ, "grfx.frame_cb.png" );
			_bmp[(int)Img.HouseBS] = new Bitmap( typ, "grfx.house_bs.png" );
			_bmp[(int)Img.HouseCB] = new Bitmap( typ, "grfx.house_cb.png" );
			_bmp[(int)Img.TreeBS] = new Bitmap( typ, "grfx.tree_bs.png" );
			_bmp[(int)Img.TreeCB] = new Bitmap( typ, "grfx.tree_cb.png" );
			_bmp[(int)Img.Padlock] = new Bitmap( typ, "grfx.padlock.png" );
		}

		public static Bitmap bmp( Img img )
		{
			return _bmp[(int)img];
		}

	}

}
