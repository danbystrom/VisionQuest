using System;
using System.Collections.Generic;
using System.Linq;
using factor10.VisionThing.Util;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionThing.Terrain
{
    public class GroundMap : Sculptable<float>
    {

        public GroundMap(int width, int height)
            : base(width, height)
        {
        }

        public GroundMap(int width, int height, float fillValue)
            : this(width, height)
        {
            for (var i = 0; i < Values.Length; i++)
                Values[i] = fillValue;
        }

        public GroundMap(Texture heightMap, Func<int, float> fx)
            : this(heightMap.Description.Width, heightMap.Description.Height)
        {
            var oldData = new Color[Width*Height];
            heightMap.GetData(oldData);

            for (var i = 0; i < Values.Length; i++)
                Values[i] = fx(oldData[i].R);
        }

        public static GroundMap CreateDoubleSize(Texture2D heightMap)
        {
            var sz = new Size2(heightMap.Description.Width, heightMap.Description.Height);
            var ground = new GroundMap(sz.Width*2, sz.Height*2);
            var oldData = new Color[sz.Width*sz.Height];
            heightMap.GetData(oldData);

            for (var y = 0; y < sz.Height; y++)
                for (var x = 0; x < sz.Width; x++)
                    ground.Values[y*ground.Width + x]
                        = ground.Values[y*ground.Width + x + 1]
                            = ground.Values[(y + 1)*ground.Width + x]
                                = ground.Values[y*ground.Width + x + 1]
                                    = oldData[y*sz.Width + x].R/10f;
            return ground;
        }

        public static GroundMap CreateDoubleSizeMirrored(Texture2D heightMap)
        {
            var sz = new Size2(heightMap.Description.Width, heightMap.Description.Height);
            var ground = new GroundMap(sz.Width*2, sz.Height*2);
            var oldData = new Color[sz.Width*sz.Height];
            heightMap.GetData(oldData);

            for (var y = 0; y < sz.Height; y++)
                for (var x = 0; x < sz.Width; x++)
                {
                    ground.Values[y*ground.Width + x]
                        = ground.Values[(y + 1)*ground.Width - x - 1]
                            = ground.Values[(ground.Height - y - 1)*ground.Width + x]
                                = ground.Values[(ground.Height - y - 1)*ground.Width - x - 1]
                                    = oldData[y*sz.Width + x].R/10f;
                }
            return ground;
        }

        public void Merge(int x, int y, Texture2D heightMap)
        {
            var oldData = new Color[Width*Height];
            heightMap.GetData(oldData);

            var sz = new Size2(heightMap.Description.Width, heightMap.Description.Height);
            var i = 0;
            for (var yy = 0; yy < sz.Height; yy++)
                for (var xx = 0; xx < sz.Width; xx++)
                    this[x + xx, y + yy] = oldData[i++].R/10f;
        }

        public float GetExactHeight(int x, int y, float fracx, float fracy)
        {
            var topHeight = MathUtil.Lerp(
                this[x, y],
                this[x + 1, y],
                fracx);

            var bottomHeight = MathUtil.Lerp(
                this[x, y + 1],
                this[x + 1, y + 1],
                fracx);

            return MathUtil.Lerp(topHeight, bottomHeight, fracy);
        }

        public float GetExactHeight(float x, float y)
        {
            var ix = (int) x;
            var iy = (int) y;
            return GetExactHeight(ix, iy, x - ix, y - iy);
        }

        public void ApplyNormalBellShape()
        {
            var wh = Width/2f;
            var hh = Height/2f;

            for (int i = 0, x = 0, y = 0; i < Values.Length; i++)
            {
                var dx = (wh - x)/wh;
                var dy = (hh - y)/hh;
                var d = dx*dx + dy*dy;
                d *= d;
                Values[i] *= (float) Math.Exp(-d*d*8);
                if (++x >= Width)
                {
                    x = 0;
                    y++;
                }
            }
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
                    Values[y1*Width + x1] = Values[y1*Width + x1]*0.7f + Values[oldIndex]*0.3f;
                }
                x += dy;
                y += dx;
            }
        }

        public Texture2D CreateHeightsTexture(GraphicsDevice graphicsDevice)
        {
            var result = Texture2D.New(graphicsDevice, Width, Height, false, PixelFormat.R32.Float);
            result.SetData(Values);
            var q = result.GetData<float>();
            for (var i = Width*Height - 1; i >= 0; i--)
                System.Diagnostics.Debug.Assert(Math.Abs(Values[i] - q[i]) < 0.001);
            return result;
        }

        private Vector3 getNormal(int index)
        {
            var h0 = Values[index];
            var h1 = Values[index + 1];
            var h2 = Values[index + Width];
            var v1 = new Vector3(0, h2 - h0, 1);
            var v2 = new Vector3(1, h1 - h0, 0);
            var n = Vector3.Cross(v1, v2);
            n.Normalize();
            return n;
        }

        public Vector3 GetNormal(int x, int y)
        {
            return x < 0 || y < 0 || x >= (Width - 1) || y >= (Height + 1)
                ? Vector3.Up
                : getNormal(y*Width + x);
        }

        public ColorSurface CreateNormalsMap()
        {
            var normals = new Color[Values.Length];
            for (var i = 0; i < normals.Length - Width - 1; i++)
            {
                var n = getNormal(i);
                normals[i] = new Color(n.X/2 + 0.5f, n.Y/2 + 0.5f, n.Z/2 + 0.5f, 0);
            }
            for (var x = 0; x < Width; x++)
                normals[(Height - 1)*Width + x] = normals[(Height - 2)*Width + x];
            for (var y = 0; y < Height; y++)
                normals[y*Width + (Width - 1)] = normals[y*Width + (Width - 2)];

            return new ColorSurface(Width, Height, normals);
        }

        public WeightsMap CreateWeigthsMap(float[] levels = null)
        {
            return new WeightsMap(this, levels);
        }

        public void Soften(int rounds = 1)
        {
            var end = Values.Length - Width - 1;
            while (rounds-- > 0)
            {
                var old = (float[]) Values.Clone();
                for (var i = Width + 1; i < end; i++)
                    Values[i] =
                        (old[i - Width - 1] + old[i - Width] + old[i - Width + 1] +
                         old[i - 1] + old[i] + old[i + 1] +
                         old[i + Width - 1] + old[i + Width] + old[i + Width + 1])/9;
            }
        }

        public void LowerEdges()
        {
            for (var x = 0; x < Width; x++)
                Values[x] = Values[(Height - 1)*Width + x] = 0;
            for (var y = 0; y < Height; y++)
                Values[y*Width] = Values[y*Width + (Width - 1)] = 0;

            for (var j = 5; j > 0; j--)
                for (var i = 1; i < j; i++)
                {
                    for (var x = 1; x < Width - 1; x++)
                    {
                        this[i, x] = (this[i, x] + this[i - 1, x])/2;
                        this[Height - 1 - i, x] = (this[Height - 1 - i, x] + this[Height - i, x])/2;
                    }
                    for (var y = 1; y < Height - 1; y++)
                    {
                        this[y, i] = (this[y, i] + this[y, i - 1])/2;
                        this[y, Width - 1 - i] = (this[y, Width - 1 - i] + this[y, Width - i])/2;
                    }
                }
        }

        public Vector3? HitTest(Matrix worldInverse, Ray rayWorld)
        {
            var ray = new Ray(
                Vector3.TransformCoordinate(rayWorld.Position, worldInverse),
                Vector3.TransformNormal(rayWorld.Direction, worldInverse));
            var pos = ray.Position;

            var p = new Vector2(pos.X, pos.Z); // 2D version of pos

            if (p.X < 0 || p.Y < 0 || p.X > Width || p.Y > Height)
            {
                //the ray does NOT start above the GroundMap - so find where it starts to come above the GroundMap
                var pos2 = pos + ray.Direction*100000; // a point far away along the ray
                var p2 = p + new Vector2(pos2.X, pos2.Z); // the 2D version of pos2
                var c0 = new Vector2(0, 0);
                var c1 = new Vector2(0, Height - 1);
                var c2 = new Vector2(Width - 1, 0);
                var c3 = new Vector2(Width - 1, Height - 1);
                var list = new List<Vector2?>
                {
                    CollisionHelpers.LineLineIntersectionPoint(p, p2, c0, c1),
                    CollisionHelpers.LineLineIntersectionPoint(p, p2, c0, c2),
                    CollisionHelpers.LineLineIntersectionPoint(p, p2, c3, c1),
                    CollisionHelpers.LineLineIntersectionPoint(p, p2, c3, c2)
                }.Where(_ => _ != null).Select(_ => _.Value).ToList();
                if (list.Count < 2)
                    return null; // if the ray starts outside the GroundMap then it should intersect two sides in 2D space - if it touches
                list.Sort((x, y) => Math.Sign(Vector2.DistanceSquared(x, p) - Vector2.DistanceSquared(y, p)));
                var near = list.First(); // the 2D-ray intersects with the GroundMap edge here

                // now calculate the 3d point where the ray enters the GroundMap area
                var d1 = Vector2.Distance(p, near);
                var d2 = Vector2.Distance(p, p2);
                pos = Vector3.Lerp(pos, pos2, d1/d2);

                var box = new BoundingBox(Vector3.Zero, new Vector3(Width-1, Width*Height, Height-1));
                Vector3 q;
                box.Intersects(ref ray, out pos);
            }

            if (pos.Y < 0)
                return null;

            // change the length of the ray so that it is normalized in 2D space, disregarding the Y component
            // reason: take apropriate large steps forward in 2d space
            var ray2DLength = (float) Math.Sqrt(ray.Direction.X*ray.Direction.X + ray.Direction.Z*ray.Direction.Z);
            var direction = ray.Direction/ray2DLength;

            for (; ; pos += direction)
            {
                var x = (int) pos.X;
                var y = (int) pos.Z;
                if (x < 0 || y < 0 || x >= Width || y >= Height)
                    break;
                var height = this[x, y];
                //System.Diagnostics.Debug.Print("({0},{1}): {2}", (int) pos.X, (int) pos.Z, height);
                if (pos.Y > height)
                    continue;  // ray is still too high above the GroundMap

                // now we have found the first point on the GroundMap that is higher than the ray
                // so create a plane using that point and two neightbouring points and find
                // the exact intersection between the ray and that plane
                //var oldPrimitiveResult = new Vector3(pos.X, height, pos.Z);
                var xd = x - Math.Sign(direction.X);
                var yd = y - Math.Sign(direction.Z);
                if (xd < 0 || yd < 0 || xd >= Width || yd >= Height)
                    continue;  // oops, too near an edge

                var p1 = new Vector3(x, height, y);
                var p2 = new Vector3(xd, this[xd, y], y);
                var p3 = new Vector3(x, this[x, yd], yd);
                var plane = new Plane(p1, p2, p3);
                Vector3 result;
                if (plane.Intersects(ref ray, out result))
                    return result;
            }

            return null;
        }

    }

}
