using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factor10.VisionThing
{
    public class ColorSurface
    {
        public readonly int Width;
        public readonly int Height;
        public readonly Color[] Surface;

        public ColorSurface(int width, int height, Color[] surface)
        {
            if ( width*height != surface.Length )
                throw new Exception();
            Width = width;
            Height = height;
            Surface = surface;
        }

        public Vector3 AsVector3(int x, int y)
        {
            var c = Surface[y*Width + x];
            return new Vector3(c.R/255f, c.G/255f, c.B/255f);
        }

        public Texture2D CreateTexture2D(GraphicsDevice graphicsDevice)
        {
            var result = new Texture2D(graphicsDevice, Width, Height, false, SurfaceFormat.Color);
            result.SetData(Surface);
            return result;
        }

    }

}
