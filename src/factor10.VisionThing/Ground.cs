using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factor10.VisionThing
{
    public class Ground
    {
        public readonly int Width;
        public readonly int Height;

        private readonly float[] _heights;

        public Ground( int width, int height )
        {
            Width = width;
            Height = height;
            _heights = new float[Width * Height];
        }

        public Ground( int width, int height, float fillValue )
            : this(width,height)
        {
            for (var i = 0; i < _heights.Length; i++)
                _heights[i] = fillValue;
        }

        public Ground(Texture2D heightMap)
            : this(heightMap.Width,heightMap.Height)
        {
            var oldData = new Color[ Width * Height];
            heightMap.GetData(oldData);

            for (var i = 0; i < _heights.Length; i++)
                _heights[i] = oldData[i].R / 10f;
        }

        public void ApplyNormalBellShape()
        {
            var wh = Width / 2f;
            var hh = Height / 2f;

            for (int i = 0, x = 0, y = 0; i < _heights.Length; i++)
            {
                var dx = (wh - x)/wh*5;
                var dy = (hh - y)/hh*5;
                _heights[i] *= (float) Math.Exp(-(dx*dx + dy*dy)/8);
                if (++x >= Width)
                {
                    x = 0;
                    y++;
                }
            }
        }

        public Texture2D CreateHeightTexture( GraphicsDevice graphicsDevice )
        {
            var result = new Texture2D(graphicsDevice, Width, Height, false, SurfaceFormat.Single);
            result.SetData(_heights);
            return result;
        }

        public Texture2D CreateWeigthTexture( GraphicsDevice graphicsDevice )
        {
            var weights = new Color[_heights.Length];
            for (var i = 0; i < _heights.Length; i++)
            {
                var val = _heights[i] * 10 / 8.5f;
                var t0 = MathHelper.Clamp(1.0f - Math.Abs(val - 0)/8.0f, 0, 1);
                var t1 = MathHelper.Clamp(1.0f - Math.Abs(val - 12)/6.0f, 0, 1);
                var t2 = MathHelper.Clamp(1.0f - Math.Abs(val - 20)/6.0f, 0, 1);
                var t3 = MathHelper.Clamp(1.0f - Math.Abs(val - 30)/6.0f, 0, 1);
                var tot = t0 + t1 + t2 + t3;
                if ( tot > 0.001f )
                    weights[i] = new Color(t0/tot, t1/tot, t2/tot, t3/tot);
            }

            var result = new Texture2D(graphicsDevice, Width, Height, false, SurfaceFormat.Color);
            result.SetData(weights);
            return result;
        }

    }

}
