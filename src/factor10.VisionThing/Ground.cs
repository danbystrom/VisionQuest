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

        public static Ground CreateDoubleSize(Texture2D heightMap)
        {
            var ground = new Ground(heightMap.Width*2, heightMap.Height*2);
            var oldData = new Color[heightMap.Width * heightMap.Height];
            heightMap.GetData(oldData);

            for ( var y=0 ; y<heightMap.Height;y++)
                for (var x = 0; x < heightMap.Width; x++)
                    ground._heights[y*ground.Width + x]
                        = ground._heights[y*ground.Width + x + 1]
                          = ground._heights[(y + 1)*ground.Width + x]
                            = ground._heights[y*ground.Width + x + 1]
                              = oldData[y*heightMap.Width + x].R/10f;
            return ground;
        }

        public void ApplyNormalBellShape()
        {
            var wh = Width / 2f;
            var hh = Height / 2f;

            for (int i = 0, x = 0, y = 0; i < _heights.Length; i++)
            {
                var dx = (wh - x)/wh;
                var dy = (hh - y)/hh;
                var d = dx*dx + dy*dy;
                d *= d;
                _heights[i] *= (float) Math.Exp(-d*d*8);
                if (++x >= Width)
                {
                    x = 0;
                    y++;
                }
            }
        }

        public void AlterValues(Func<float> func)
        {
            for (var i = 0; i < _heights.Length; i++)
                _heights[i] += func();
        }

        public void FlattenRectangle(int x, int y, int size)
        {
            if ( x<0 || y<0 || size<2 || x+size>Width || y+size>Height)
                throw new Exception();

            for ( var i = 1 ; i<size ; i++ )
            {
                flattenLine(x+i, y+i, size - i, 1, 0);
                flattenLine(x+i, y+i, size - i, 0, 1);
                flattenLine(x + size - i, y + size - i, size - i, -1, 0);
                flattenLine(x + size - i, y + size - i, size - i, 0, -1);
            }
        }

        private void flattenLine( int x, int y, int length, int dx, int dy )
        {
            for (var j = 0; j < length; j++)
            {
                var x1 = x;
                var y1 = y;
                for (var i = j; i < length; i++)
                {
                    var oldIndex = y1*Width + x1;
                    x1 += dx;
                    y1 += dy;
                    _heights[y1*Width + x1] = _heights[y1*Width + x1]*0.7f + _heights[oldIndex]*0.3f;
                }
                x += dy;
                y += dx;
            }
        }

        public Texture2D CreateHeightTexture( GraphicsDevice graphicsDevice )
        {
            var result = new Texture2D(graphicsDevice, Width, Height, false, SurfaceFormat.Single);
            result.SetData(_heights);
            return result;
        }

        public Texture2D CreateWeigthTexture( GraphicsDevice graphicsDevice, float[] levels = null )
        {
            var weights = new Color[_heights.Length];
            var min = _heights.Min();
            var max = _heights.Max();
            var span = max - min;
            levels = new [] {min, min + span*0.43f, min + span*0.67f, max};
            span /= 4;
            for (var i = 0; i < _heights.Length; i++)
            {
                var val = _heights[i];
                var t0 = MathHelper.Clamp(1.0f - Math.Abs(val - levels[0]) / span, 0, 1);
                var t1 = MathHelper.Clamp(1.0f - Math.Abs(val - levels[1]) / span, 0, 1);
                var t2 = MathHelper.Clamp(1.0f - Math.Abs(val - levels[2]) / span, 0, 1);
                var t3 = MathHelper.Clamp(1.0f - Math.Abs(val - levels[3]) / span, 0, 1);
                var tot = t0 + t1 + t2 + t3;
                if ( tot > 0.001f )
                   weights[i] = new Color(t0/tot, t1/tot, t2/tot, t3/tot);
            }

            var result = new Texture2D(graphicsDevice, Width, Height, false, SurfaceFormat.Color);
            result.SetData(weights);
            return result;
        }

        public Texture2D CreateNormalsTexture(GraphicsDevice graphicsDevice)
        {
            var normals = new Color[_heights.Length];
            for (var i = 0; i < normals.Length; i++)
                normals[i] = new Color(0f, 1f, 0f, 0);
            for (int i = 0, x = 0, y = 0; i < normals.Length - Width - 1; i++)
            {
                var p1 = new Vector3(x, _heights[i], y);
                var p2 = new Vector3(x, _heights[i+Width], y+1);
                var p3 = new Vector3(x+1, _heights[i + 1], y);
                var v1 = p1 - p2;
                var v2 = p1 - p3;
                var n = -Vector3.Cross(v2, v1);
                n.Normalize();
                normals[i] = new Color(n.X/2 + 0.5f, n.Y/2 + 0.5f, n.Z/2 + 0.5f, 0);
                if (++x >= Width)
                {
                    x = 0;
                    y++;
                }
            }
            for (var x = 0;x < Width; x++)
                normals[(Height-1)*Width+x] = normals[(Height-2)*Width+x];
            for (var y = 0; y < Height; y++)
                normals[y * Width + (Width - 1)] = normals[y * Width + (Width - 2)];

            var result = new Texture2D(graphicsDevice, Width, Height, false, SurfaceFormat.Color);
            result.SetData(normals);
            return result;
        }

        public void Soften()
        {
            var old = (float[])_heights.Clone();
            for (int i = Width+1; i < _heights.Length - Width - 1; i++)
                _heights[i] =
                    (old[i - Width - 1] + old[i - Width] + old[i - Width + 1] +
                        old[i  - 1] + old[i ] + old[i  + 1] +
                        old[i + Width - 1] + old[i + Width] + old[i + Width + 1])/9;

        }

        public void LowerEdges()
        {
            for (var x = 0; x < Width; x++)
            {
                _heights[x] = _heights[(Height - 1) * Width + x] = 0;
                _heights[Height + x] *= 0.5f;
                _heights[(Height - 2) * Width + x] *= 0.5f;
            }
            for (var y = 0; y < Height; y++)
            {
                _heights[y*Width] = _heights[y*Width + (Width - 1)] = 0;
                _heights[y*Width + 1] *= 0.5f;
                _heights[y * Width + (Width - 2)] *= 0.5f;
            }
        }

    }

}
