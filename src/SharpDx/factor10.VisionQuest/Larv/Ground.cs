using System;
using System.Collections.Generic;
using factor10.VisionThing;
using factor10.VisionThing.Terrain;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace Larv
{
    public class Ground : TerrainBase
    {
        public const int Width = 128;
        public const int Height = 128;

        private CxBillboard _cxBillboardGrass;
        private CxBillboard _cxBillboardTrees;

        //public void Move(KeyboardState keyboardState)
        //{
        //    if (keyboardState.IsKeyPressed(Keys.X))
        //        _qx += 0.3333f*(keyboardState.IsKeyDown(Keys.Shift) ? -1 : 1);
        //    if (keyboardState.IsKeyPressed(Keys.Y))
        //        _qy += 0.3333f * (keyboardState.IsKeyDown(Keys.Shift) ? -1 : 1);
        //    World = Matrix.Scaling(1 / 3f) * Matrix.Translation(_qx, -0.5f, _qy);

        //    var slicesW = 2;
        //    var slicesH = 2;
        //    //TODO - this is wrong - I guess...
        //    var sliceFracX = 1f / slicesW;
        //    var sliceFracY = 1f / slicesH;
        //    var raduis = HalfSide * (float)Math.Sqrt(2);
        //    var i = 0;
        //    for (var y = 0; y < slicesH; y++)
        //        for (var x = 0; x < slicesW; x++)
        //        {
        //            var world = Matrix.Translation(Side * x, 0, Side * y) * World;
        //            _slices[i++] = new terrainSlice
        //            {
        //                TexOffsetAndScale = new Vector4(x * sliceFracX, y * sliceFracY, sliceFracX, sliceFracY),
        //                World = world,
        //                BoundingSphere = new BoundingSphere(world.TranslationVector + new Vector3(HalfSide, 0, HalfSide), raduis)
        //            };
        //        }
        //}

        public Ground(VisionContent vContent)
            : base(vContent)
        {
            var rnd = new Random();

            GroundMap = new GroundMap(Width, Height, 15);

            for (var i = 0; i < 5000; i++)
                GroundMap[rnd.Next(3, Width - 5), rnd.Next(3, Height - 5)] += 20;

            // generate mountains

            GroundMap.DrawLine(2, 2, Width - 3, 2, 2, (a, b) => rnd.Next(50, 300));
            GroundMap.DrawLine(2, 2, 2, Height - 3, 2, (a, b) => rnd.Next(50, 300));
            GroundMap.DrawLine(Width - 3, Height - 3, Width - 3, 2, 2, (a, b) => rnd.Next(50, 300));
            GroundMap.DrawLine(Width - 3, Height - 3, 2, Height - 3, 2, (a, b) => rnd.Next(50, 300));

            GroundMap.DrawLine(5, 5, Width - 6, 5, 2, (a, b) => rnd.Next(40, 200));
            GroundMap.DrawLine(5, 5, 5, Height - 6, 2, (a, b) => rnd.Next(40, 200));
            GroundMap.DrawLine(Width - 6, Height - 6, Width - 6, 2, 2, (a, b) => rnd.Next(40, 200));
            GroundMap.DrawLine(Width - 6, Height - 6, 2, Height - 6, 2, (a, b) => rnd.Next(40, 200));
        }

        public void GeneratePlayingField(PlayingField playingField)
        {
            var pfW = playingField.Width;
            var pfH = playingField.Height;

            var qx = -((Width - pfW * 3) / 2f + 0.5f) / 3;  // -9f;  // 128 - 75 = 53 / 2 = (26.5+0.5) / 3 = 9 
            var qy = -((Height - pfH * 3) / 2f + 0.5f) / 3; // -11f;  // 128 - 63 = 65 / 2 = (32.5+0.5) / 3 = 11

            var rnd = new Random();

            World = Matrix.Scaling(1 / 3f, 0.05f, 1 / 3f) * Matrix.Translation(qx, -0.5f, qy);

            //reset everything but the mountains
            GroundMap.AlterValues(8, 8, Width - 16, Height - 16, (a, b, c) => 15);

            for (var i = 0; i < 5000; i++)
                GroundMap[rnd.Next(3, Width - 5), rnd.Next(3, Height - 5)] += 20;

            GroundMap.Soften(2);

            carvePlayingField(GroundMap, playingField, (Width - pfW * 3) / 2, (Height - pfH * 3) / 2, 0);
            var nx = (Width - pfW * 3) / 2 + playingField.PlayerWhereaboutsStart.Location.X * 3 - 2;
            var ny = (Height - pfH * 3) / 2 + playingField.PlayerWhereaboutsStart.Location.Y * 3 - 2;
            GroundMap.AlterValues(nx, ny, 7, 7, (a, b, c) => 30);

            var weights = GroundMap.CreateWeigthsMap(new[] { 0, 0.40f, 0.60f, 0.9f });

            for (var i = 0; i < 1000; i++)
            {
                var m = rnd.Next(9);
                weights.AlterValues(rnd.Next(5, Width - 7), rnd.Next(5, Height - 7), 3, 3, (x, y, mt) =>
                {
                    mt[m] += 0.5f;
                    return mt;
                });
            }

            carvePlayingField(weights, playingField, (Width - pfW * 3) / 2, (Height - pfH * 3) / 2, new Mt9Surface.Mt9 { H = 2 });

            var normals = GroundMap.CreateNormalsMap(ref World);
            initialize(GroundMap, weights, normals);

            var grass = new List<Tuple<Vector3, Vector3>>();
            var trees = new List<Tuple<Vector3, Vector3>>();
            for (var i = 0; i < 100000; i++)
            {
                var gx = rnd.Next(10, Width - 20) + (float)rnd.NextDouble();
                var gy = rnd.Next(10, Height - 20) + (float)rnd.NextDouble();
                var position = Vector3.TransformCoordinate(new Vector3(gx, GroundMap.GetExactHeight(gx, gy), gy), World);
                if (position.Y < 0.7f)
                    continue;
                position.Y -= 0.05f;
                var normal = normals.GetExact(gx, gy).ToVector3();
                if (normal.Y < 0.5f)
                    continue;
                if (rnd.NextDouble() < 0.998)
                    grass.Add(new Tuple<Vector3, Vector3>(position, normal));
                else
                    trees.Add(new Tuple<Vector3, Vector3>(position, Vector3.Up));
            }

            if(_cxBillboardGrass!=null)
                _cxBillboardGrass.Dispose();
            if (_cxBillboardTrees != null)
                _cxBillboardTrees.Dispose();
            _cxBillboardGrass = new CxBillboard(VContent, Matrix.Identity, VContent.Load<Texture2D>("billboards/grass"), 0.3f, 0.3f);
            _cxBillboardGrass.CreateBillboardVerticesFromList(grass);
            _cxBillboardTrees = new CxBillboard(VContent, Matrix.Identity, VContent.Load<Texture2D>("billboards/tree"), 1.5f, 1.5f);
            _cxBillboardTrees.CreateBillboardVerticesFromList(trees);
        }

        private static void carvePlayingField<T>(Sculptable<T> ground, PlayingField playingField, int offx, int offy, T value)
            where T : struct
        {
            for (var y = 0; y < playingField.Height; y++)
                for (var x = 0; x < playingField.Width; x++)
                    if (!playingField.FieldValue(0, new Point(x, y)).IsNone)
                    {
                        if (x == playingField.PlayerWhereaboutsStart.Location.X && y == playingField.PlayerWhereaboutsStart.Location.Y)
                            continue;
                        var nx = offx + x*3;
                        var ny = offy + y*3;
                        for (var i = 0; i < 3; i++)
                            for (var j = 0; j < 3; j++)
                                ground[nx + i, ny + j] = value;
                    }
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
            _cxBillboardGrass.Update(camera, gameTime);
            _cxBillboardTrees.Update(camera, gameTime);
            base.Update(camera, gameTime);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            _cxBillboardGrass.Draw(camera, drawingReason, shadowMap);
            _cxBillboardTrees.Draw(camera, drawingReason, shadowMap);
            return base.draw(camera, drawingReason, shadowMap);
        }
    }

}
