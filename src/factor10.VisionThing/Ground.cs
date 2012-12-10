﻿using System;
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

        public Ground(int width, int height)
        {
            Width = width;
            Height = height;
            _heights = new float[Width*Height];
        }

        public Ground(int width, int height, float fillValue)
            : this(width, height)
        {
            for (var i = 0; i < _heights.Length; i++)
                _heights[i] = fillValue;
        }

        public Ground(Texture2D heightMap, Func<int,float> fx )
            : this(heightMap.Width, heightMap.Height)
        {
            var oldData = new Color[Width*Height];
            heightMap.GetData(oldData);

            for (var i = 0; i < _heights.Length; i++)
                _heights[i] = fx(oldData[i].R);
        }

        public static Ground CreateDoubleSize(Texture2D heightMap)
        {
            var ground = new Ground(heightMap.Width*2, heightMap.Height*2);
            var oldData = new Color[heightMap.Width*heightMap.Height];
            heightMap.GetData(oldData);

            for (var y = 0; y < heightMap.Height; y++)
                for (var x = 0; x < heightMap.Width; x++)
                    ground._heights[y*ground.Width + x]
                        = ground._heights[y*ground.Width + x + 1]
                          = ground._heights[(y + 1)*ground.Width + x]
                            = ground._heights[y*ground.Width + x + 1]
                              = oldData[y*heightMap.Width + x].R/10f;
            return ground;
        }

        public static Ground CreateDoubleSizeMirrored(Texture2D heightMap)
        {
            var ground = new Ground(heightMap.Width*2, heightMap.Height*2);
            var oldData = new Color[heightMap.Width*heightMap.Height];
            heightMap.GetData(oldData);

            for (var y = 0; y < heightMap.Height; y++)
                for (var x = 0; x < heightMap.Width; x++)
                {
                    ground._heights[y*ground.Width + x]
                        = ground._heights[(y + 1)*ground.Width - x - 1]
                          = ground._heights[(ground.Height - y - 1)*ground.Width + x]
                            = ground._heights[(ground.Height - y - 1)*ground.Width - x - 1]
                              = oldData[y*heightMap.Width + x].R/10f;
                }
            return ground;
        }

        public float this[int x, int y]
        {
            get { return _heights[y*Width + x]; }
            set { _heights[y*Width + x] = value; }
        }

        public float GetExactHeight(int x, int y, float fracx, float fracy)
        {
            var topHeight = MathHelper.Lerp(
                this[x, y],
                this[x + 1, y],
                fracx);

            var bottomHeight = MathHelper.Lerp(
                this[x, y + 1],
                this[x + 1, y + 1],
                fracx);

            return MathHelper.Lerp(topHeight, bottomHeight, fracy);
        }

        public void ApplyNormalBellShape()
        {
            var wh = Width/2f;
            var hh = Height/2f;

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

        public void AlterValues(Func<float,float> func)
        {
            for (var i = 0; i < _heights.Length; i++)
                _heights[i] = func(_heights[i]);
        }

        public void FlattenRectangle(int x, int y, int size)
        {
            if (x < 0 || y < 0 || size < 2 || x + size > Width || y + size > Height)
                throw new Exception();

            for (var i = 1; i < size; i++)
            {
                flattenLine(x + i, y + i, size - i, 1, 0);
                flattenLine(x + i, y + i, size - i, 0, 1);
                flattenLine(x + size - i, y + size - i, size - i, -1, 0);
                flattenLine(x + size - i, y + size - i, size - i, 0, -1);
            }
        }

        private void flattenLine(int x, int y, int length, int dx, int dy)
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

        public Texture2D CreateHeightsTexture(GraphicsDevice graphicsDevice)
        {
            var result = new Texture2D(graphicsDevice, Width, Height, false, SurfaceFormat.Single);
            result.SetData(_heights);
            return result;
        }

        public ColorSurface CreateWeigthsMap(float[] levels = null)
        {
            var weights = new Color[_heights.Length];
            var min = _heights.Min();
            var max = _heights.Max();
            var span = max - min;
            if (levels == null)
                levels = new [] {0, 0.33f, 0.76f, 1};
            for (var i = 0; i < 4; i++)
                levels[i] = min + levels[i]*span;
            span /= 4;
            for (var i = 0; i < _heights.Length; i++)
            {
                var val = _heights[i];
                var t0 = MathHelper.Clamp(1.0f - Math.Abs(val - levels[0])/span, 0, 1);
                var t1 = MathHelper.Clamp(1.0f - Math.Abs(val - levels[1])/span, 0, 1);
                var t2 = MathHelper.Clamp(1.0f - Math.Abs(val - levels[2])/span, 0, 1);
                var t3 = MathHelper.Clamp(1.0f - Math.Abs(val - levels[3])/span, 0, 1);
                var tot = t0 + t1 + t2 + t3;
                if (tot > 0.001f)
                    weights[i] = new Color(t0/tot, t1/tot, t2/tot, t3/tot);
            }

            return new ColorSurface(Width, Height, weights);
        }

        public ColorSurface CreateNormalsMap()
        {
            var normals = new Color[_heights.Length];
            for (var i = 0; i < normals.Length - Width - 1; i++)
            {
                var h = _heights[i];
                var v1 = new Vector3(0, _heights[i + Width] - h, 1);
                var v2 = new Vector3(1, _heights[i + 1] - h, 0);
                var n = Vector3.Cross(v1, v2);
                n.Normalize();
                normals[i] = new Color(n.X/2 + 0.5f, n.Y/2 + 0.5f, n.Z/2 + 0.5f, 0);
            }
            for (var x = 0; x < Width; x++)
                normals[(Height - 1)*Width + x] = normals[(Height - 2)*Width + x];
            for (var y = 0; y < Height; y++)
                normals[y*Width + (Width - 1)] = normals[y*Width + (Width - 2)];

            return new ColorSurface(Width, Height, normals);
        }

        public void Soften()
        {
            var old = (float[]) _heights.Clone();
            for (int i = Width + 1; i < _heights.Length - Width - 1; i++)
                _heights[i] =
                    (old[i - Width - 1] + old[i - Width] + old[i - Width + 1] +
                     old[i - 1] + old[i] + old[i + 1] +
                     old[i + Width - 1] + old[i + Width] + old[i + Width + 1])/9;

        }

        public void LowerEdges()
        {
            for (var x = 0; x < Width; x++)
                _heights[x] = _heights[(Height - 1)*Width + x] = 0;
            for (var y = 0; y < Height; y++)
                _heights[y*Width] = _heights[y*Width + (Width - 1)] = 0;

            for (var j = 5; j > 0; j--)
                for (var i = 1; i < j; i++)
                {
                    for (var x = 1; x < Width-1; x++)
                    {
                        this[i, x] = (this[i, x] + this[i - 1, x])/2;
                        this[Height - 1 - i, x] = (this[Height - 1 - i, x] + this[Height - i, x]) / 2;
                    }
                    for (var y = 1; y < Height-1; y++)
                    {
                        this[y, i] = (this[y, i] + this[y, i - 1]) / 2;
                        this[y, Width - 1 - i] = (this[y, Width - 1 - i] + this[y, Width - i]) / 2;
                    }
                }

        }

        public void DrawLine( int x1, int y1, int x2, int y2, int width)
        {
            var dx = Math.Abs(x1 - x2);
            var dy = Math.Abs(y1 - y2);
            if (dx == 0 && dy == 0)
                return;
            if (dy > dx)
                drawMostlyHorizontalLine(x1, y1, dy, dx/(float) dy, width);
            else
                drawMostlyVerticalLine(x1, y1, dx, dy/(float) dx, width);
        }

        private void drawMostlyHorizontalLine(int x, float y, int len, float d, int width)
        {
            for (var i = 0; i < len; i++ )
            {
                y += d;
                x++;
            }
        }

        private void drawMostlyVerticalLine(float x, int y, int len, float d, int width)
        {
            for (var i = 0; i < len; i++)
            {
                x += d;
                y++;
            }
        }

    }

}
