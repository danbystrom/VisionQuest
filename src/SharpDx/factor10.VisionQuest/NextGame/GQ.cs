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
    public class Gq : TerrainBase
    {
        private float _qx = -9f;
        private float _qy = -11f;

        private readonly CxBillboard _cxBillboardGrass;
        private readonly CxBillboard _cxBillboardTrees;

        public void Move(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyPressed(Keys.X))
                _qx += 0.3333f*(keyboardState.IsKeyDown(Keys.Shift) ? -1 : 1);
            if (keyboardState.IsKeyPressed(Keys.Y))
                _qy += 0.3333f * (keyboardState.IsKeyDown(Keys.Shift) ? -1 : 1);
            World = Matrix.Scaling(1 / 3f) * Matrix.Translation(_qx, -0.5f, _qy);

            var slicesW = 2;
            var slicesH = 2;
            //TODO - this is wrong - I guess...
            var sliceFracX = 1f / slicesW;
            var sliceFracY = 1f / slicesH;
            var raduis = HalfSide * (float)Math.Sqrt(2);
            var i = 0;
            for (var y = 0; y < slicesH; y++)
                for (var x = 0; x < slicesW; x++)
                {
                    var world = Matrix.Translation(Side * x, 0, Side * y) * World;
                    _slices[i++] = new terrainSlice
                    {
                        TexOffsetAndScale = new Vector4(x * sliceFracX, y * sliceFracY, sliceFracX, sliceFracY),
                        World = world,
                        BoundingSphere = new BoundingSphere(world.TranslationVector + new Vector3(HalfSide, 0, HalfSide), raduis)
                    };
                }

        }

        public Gq(VisionContent vContent, PlayingField playingField)
            : base(vContent)
        {
            var pfW = playingField.Width;
            var pfH = playingField.Height;

            var gqW = 128; // + 64;
            var gqH = 128; // + 64;

            var rnd = new Random();

            World = Matrix.Scaling(1/3f, 0.05f, 1/3f)*Matrix.Translation(_qx, -0.5f, _qy);

            var ground = new Ground(gqW, gqH, 15);

            //ground.AlterValues(100, 200, 10, 10, (x, y, h) => 20);
            //ground.AlterValues(100, 210, 10, 10, (x, y, h) => 30);
            //ground.AlterValues(100, 220, 10, 10, (x, y, h) => 40);

            for (var i = 0; i < 5000; i++)
                ground[rnd.Next(3, gqW - 5), rnd.Next(3, gqH - 5)] += 20;

            ground.DrawLine(2, 2, gqW - 3, 2, 2, (a, b) => rnd.Next(50, 300));
            ground.DrawLine(2, 2, 2, gqH - 3, 2, (a, b) => rnd.Next(50, 300));
            ground.DrawLine(gqW - 3, gqH - 3, gqW - 3, 2, 2, (a, b) => rnd.Next(50, 300));
            ground.DrawLine(gqW - 3, gqH - 3, 2, gqH - 3, 2, (a, b) => rnd.Next(50, 300));

            ground.DrawLine(5, 5, gqW - 6, 5, 2, (a, b) => rnd.Next(40, 200));
            ground.DrawLine(5, 5, 5, gqH - 6, 2, (a, b) => rnd.Next(40, 200));
            ground.DrawLine(gqW - 6, gqH - 6, gqW - 6, 2, 2, (a, b) => rnd.Next(40, 200));
            ground.DrawLine(gqW - 6, gqH - 6, 2, gqH - 6, 2, (a, b) => rnd.Next(40, 200));

            ground.DrawLine(10, 10, 50, 50, 2, (a, b) => 5);
            ground.Soften();
            ground.Soften();
            //ground.Soften();

            carvePlayingField(ground, playingField, (gqW - pfW*3)/2, (gqH - pfH*3)/2);

            var weights = ground.CreateWeigthsMap(new[] {0, 0.40f, 0.60f, 0.9f});

            //weights.AlterValues(20, 20, 20, 20, (x, y, mt) => new Mt9Surface.Mt9 {B = 1});
            //weights.AlterValues(30, 30, 20, 20, (x, y, mt) => new Mt9Surface.Mt9 {C = 1});
            //weights.AlterValues(40, 40, 20, 20, (x, y, mt) => new Mt9Surface.Mt9 {D = 1});
            //weights.AlterValues(50, 50, 20, 20, (x, y, mt) => new Mt9Surface.Mt9 {E = 1});
            //weights.AlterValues(60, 60, 20, 20, (x, y, mt) => new Mt9Surface.Mt9 {F = 1});
            //weights.AlterValues(70, 70, 20, 20, (x, y, mt) => new Mt9Surface.Mt9 {G = 1});

            for (var i = 0; i < 1000; i++)
            {
                var m = rnd.Next(9);
                weights.AlterValues(rnd.Next(5, gqW - 7), rnd.Next(5, gqH - 7), 3, 3, (x, y, mt) =>
                {
                    mt[m] += 0.5f;
                    return mt;
                });
            }

            //weights.DrawLine(10, 10, 100, 100, 4, (a, b) => new Mt9Surface.Mt9 {F = 1});
            carvePlayingField2(weights, playingField, (gqW - pfW*3)/2, (gqH - pfH*3)/2);

            var normals = ground.CreateNormalsMap();
            initialize(ground, weights, normals);

            var grass = new List<Tuple<Vector3, Vector3>>();
            var trees = new List<Tuple<Vector3, Vector3>>();
            for (var i = 0; i < 100000; i++)
            {
                var gx = rnd.Next(10, gqW - 20) + (float) rnd.NextDouble();
                var gy = rnd.Next(10, gqH - 20) + (float) rnd.NextDouble();
                var position = Vector3.TransformCoordinate(new Vector3(gx, ground.GetExactHeight(gx, gy), gy), World);
                if (position.Y < 0.7f)
                    continue;
                position.Y -= 0.05f;
                var normal = normals.GetExact(gx, gy).ToVector3();
                if (normal.Y < 0.5f)
                    continue;
                if (rnd.NextDouble() < 0.999)
                    grass.Add(new Tuple<Vector3, Vector3>(position, normal));
                else
                    trees.Add(new Tuple<Vector3, Vector3>(position, Vector3.Up));
            }

            _cxBillboardGrass = new CxBillboard(vContent, Matrix.Identity, vContent.Load<Texture2D>("billboards/grass"), 0.3f, 0.3f);
            _cxBillboardGrass.CreateBillboardVerticesFromList(grass);
            _cxBillboardTrees = new CxBillboard(vContent, Matrix.Identity, vContent.Load<Texture2D>("billboards/tree"), 1.5f, 1.5f);
            _cxBillboardTrees.CreateBillboardVerticesFromList(trees);
        }

        private static void carvePlayingField(Sculptable<float> ground, PlayingField playingField, int offx, int offy)
        {
            for (var y = 0; y < playingField.Height; y++)
                for (var x = 0; x < playingField.Width; x++)
                    if (!playingField.FieldValue(0, new Point(x, y)).IsNone)
                    {
                        var nx = offx + x*3;
                        var ny = offy + y*3;
                        for(var i=0;i<3;i++)
                            for (var j = 0; j < 3; j++)
                                ground[nx+i, ny+j] = 0;
                    }
        }

        private static void carvePlayingField2(Sculptable<Mt9Surface.Mt9> ground, PlayingField playingField, int offx, int offy)
        {
            for (var y = 0; y < playingField.Height; y++)
                for (var x = 0; x < playingField.Width; x++)
                    if (!playingField.FieldValue(0, new Point(x, y)).IsNone)
                    {
                        var nx = offx + x * 3;
                        var ny = offy + y * 3;
                        for (var i = 0; i < 3; i++)
                            for (var j = 0; j < 3; j++)
                                ground[nx + i, ny + j] = new Mt9Surface.Mt9 {H = 2};
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
