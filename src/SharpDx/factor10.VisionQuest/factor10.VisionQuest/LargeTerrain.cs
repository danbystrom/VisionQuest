﻿using factor10.VisionThing.Terrain;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionThing
{
    public class LargeTerrain : TerrainBase
    {

        public LargeTerrain(
            VisionContent vContent,
            Matrix world,
            Texture2D heightsMap)
            : base(vContent)
        {
            World = world;
            var ground = new Ground(heightsMap, h => (255 - h)/15f);
            ground.AlterValues(h => h + 4);
            ground.ApplyNormalBellShape();
            var normals = ground.CreateNormalsMap();

            var ms = new MicrosoftBillboards(vContent, world*Matrix.Translation(-64, 0.05f, -64));
            ms.GenerateTreePositions(ground, normals);
            Children.Add(ms);

            initialize(ground, ground.CreateWeigthsMap(), normals);
        }

    }

}