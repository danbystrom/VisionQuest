﻿using System;
using factor10.VisionThing;
using factor10.VisionThing.Terrain;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Larv
{
    public class Ground : TerrainBase
    {
        public const int TotalWidth = 128 + 64;
        public const int TotalHeight = 128 + 64;

        public const int Left = 24;
        public const int Top = 24;
        public const int Right = TotalWidth - Left;
        public const int Bottom = TotalHeight - Top;

        private CxBillboard _cxBillboardGrass;
        private CxBillboard _cxBillboardTrees;
        private StaticBillboard _cxBillboardSigns;

        public Vector3 SignPosition;

        public Ground(VisionContent vContent)
            : base(vContent)
        {
            var rnd = new Random();

            GroundMap = new GroundMap(TotalWidth, TotalHeight, 15);

            // generate mountains

            GroundMap.DrawLine(Left, Top, Right, Top, 2, (a, b) => rnd.Next(50, 300));
            GroundMap.DrawLine(Left, Top, Left, Bottom, 2, (a, b) => rnd.Next(50, 300));
            GroundMap.DrawLine(Right, Bottom, Right, Top, 2, (a, b) => rnd.Next(50, 300));
            GroundMap.DrawLine(Right, Bottom, Left, Bottom, 2, (a, b) => rnd.Next(50, 300));

            GroundMap.DrawLine(Left + 3, Top + 2, Right - 3, Top + 3, 2, (a, b) => rnd.Next(50, 200));
            GroundMap.DrawLine(Left + 3, Top + 2, Left + 3, Bottom - 3, 2, (a, b) => rnd.Next(50, 200));
            GroundMap.DrawLine(Right - 3, Bottom - 2, Right - 3, Top + 3, 2, (a, b) => rnd.Next(50, 200));
            GroundMap.DrawLine(Right - 3, Bottom - 2, Left + 3, Bottom - 3, 2, (a, b) => rnd.Next(50, 200));
        }

        public void GeneratePlayingField(PlayingField playingField)
        {
            var pfW = playingField.Width;
            var pfH = playingField.Height;

            var qx = -((TotalWidth - pfW * 3) / 2f + 0.5f) / 3;  // -9f;  // 128 - 75 = 53 / 2 = (26.5+0.5) / 3 = 9 
            var qy = -((TotalHeight - pfH * 3) / 2f + 0.5f) / 3; // -11f;  // 128 - 63 = 65 / 2 = (32.5+0.5) / 3 = 11

            var rnd = new Random();

            World = Matrix.Scaling(1/3f, 0.05f, 1/3f)*Matrix.Translation(qx - 0.3f, -0.5f, qy - 0.3f);

            //reset everything but the mountains
            GroundMap.AlterValues(Left + 4, Top + 4, Right - Left - 8, Bottom - Top - 8, (a, b, c) => 15);

            for (var i = 0; i < 5000; i++)
                GroundMap[rnd.Next(Left + 4, Right - 4), rnd.Next(Top + 8, Bottom - 8)] += 20;

            GroundMap.Soften(2);

            carvePlayingField(GroundMap, playingField, (TotalWidth - pfW * 3) / 2, (TotalHeight - pfH * 3) / 2, 0);
            createHillForCave(playingField.PlayerWhereaboutsStart.Location, pfW, pfH);
            createHillForCave(playingField.EnemyWhereaboutsStart.Location, pfW, pfH);

            var weights = GroundMap.CreateWeigthsMap(new[] { 0, 0.40f, 0.60f, 0.9f });

            for (var i = 0; i < 10000; i++)
            {
                var m = rnd.Next(9);
                weights.AlterValues(rnd.Next(Left + 8, Right - 8), rnd.Next(Top + 8, Bottom - 8), 3, 3, (x, y, mt) =>
                {
                    mt[m] += 0.5f;
                    return mt;
                });
            }

            carvePlayingField(weights, playingField, (TotalWidth - pfW * 3) / 2, (TotalHeight - pfH * 3) / 2, new Mt9Surface.Mt9 { H = 2 });

            var normals = GroundMap.CreateNormalsMap(ref World);
            initialize(GroundMap, weights, normals);

            SignPosition = Vector3.TransformCoordinate(new Vector3(Right - 10, 20, 10), World);
            SignPosition.Z = playingField.PlayerWhereaboutsStart.Location.Y - 10;

            disposeBillboards();
            _cxBillboardGrass = new CxBillboard(VContent, Matrix.Identity, VContent.Load<Texture2D>("billboards/grass"), 0.3f, 0.3f);
            _cxBillboardTrees = new CxBillboard(VContent, Matrix.Identity, VContent.Load<Texture2D>("billboards/tree"), 1.5f, 1.5f);
            for (var i = 0; i < 150000; i++)
            {
                var gx = rnd.Next(Left + 8, Right - 8) + (float) rnd.NextDouble();
                var gy = rnd.Next(Top + 8, Bottom - 8) + (float) rnd.NextDouble();
                var position = Vector3.TransformCoordinate(new Vector3(gx, GroundMap.GetExactHeight(gx, gy), gy), World);
                if (position.Y < 0.7f)
                    continue;
                position.Y -= 0.05f;
                var normal = normals.GetExact(gx, gy).ToVector3();
                if (normal.Y < 0.5f)
                    continue;
                if (rnd.NextDouble() < 0.996 || Vector3.DistanceSquared(position, SignPosition) < 5)
                    _cxBillboardGrass.Add(position, normal);
                else
                    _cxBillboardTrees.Add(position, Vector3.Up);
            }

            _cxBillboardSigns = new StaticBillboard(VContent, Matrix.Identity, VContent.Load<Texture2D>("billboards/woodensign"), 3.5f, 1.5f);
            _cxBillboardSigns.Add(SignPosition, Vector3.Up, Vector3.Left);

            _cxBillboardGrass.CreateBillboardVertices();
            _cxBillboardSigns.CreateBillboardVertices();
            _cxBillboardTrees.CreateBillboardVertices();
        }

        private void createHillForCave(Point location, int pfW, int pfH)
        {
            var nx = (TotalWidth - pfW*3)/2 + location.X*3 - 3;
            var ny = (TotalHeight - pfH*3)/2 + location.Y*3 - 3;
            GroundMap.AlterValues(nx, ny, 9, 9, (a, b, c) => 35);
            GroundMap.AlterValues(nx + 2, ny + 2, 5, 5, (a, b, c) => 40);
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
            base.draw(camera, drawingReason, shadowMap);
            if (drawingReason != DrawingReason.ShadowDepthMap)
                _cxBillboardGrass.Draw(camera, drawingReason, shadowMap);
            _cxBillboardTrees.Draw(camera, drawingReason, shadowMap);
            _cxBillboardSigns.Draw(camera, drawingReason, shadowMap);
            return true;
        }

        private void disposeBillboards()
        {
            if (_cxBillboardGrass != null)
                _cxBillboardGrass.Dispose();
            if (_cxBillboardTrees != null)
                _cxBillboardTrees.Dispose();
            if (_cxBillboardSigns != null)
                _cxBillboardSigns.Dispose();
            _cxBillboardGrass = null;
            _cxBillboardTrees = null;
            _cxBillboardSigns = null;
        }

        public override void Dispose()
        {
            disposeBillboards();
            base.Dispose();
        }

    }

}
