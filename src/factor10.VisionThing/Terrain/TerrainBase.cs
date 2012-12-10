using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using factor10.VisionThing.Primitives;

namespace factor10.VisionThing.Terrain
{
    public class TerrainBase : ClipDrawable
    {
        private static readonly TerrainPlane TerrainPlane;
        private static readonly Box Box;

        protected Matrix World;
        private Vector3 _position;

        protected Texture2D Texture0;
        protected Texture2D Texture1;
        protected Texture2D Texture2;
        protected Texture2D Texture3;

        protected Texture2D HeightsMap;
        protected Texture2D WeightsMap;
        protected Texture2D NormalsMap;

        private terrainSlice[] _slices;

        protected Vector4 TexOffsetAndScale = new Vector4(0, 0, 1, 1);

        static TerrainBase()
        {
            TerrainPlane = new TerrainPlane();
            Box = new Box(Matrix.Identity, new Vector3(1, 50, 1));
        }

        public TerrainBase()
          :  base(TerrainPlane.Effect)
        {
            Effect.Parameters["Ambient"].SetValue(0.6f);
        }

        protected void initialize(Ground ground)
        {
            initialize(ground, ground.CreateNormalsMap());
        }

        protected void initialize(Ground ground, ColorSurface normals)
        {
            Debug.Assert((ground.Width%64) == 0 && (ground.Height%64) == 0);

            _position = World.Translation;

            Texture0 = Texture0 ?? VisionContent.Load<Texture2D>("sand");
            Texture1 = Texture1 ?? VisionContent.Load<Texture2D>("grass");
            Texture2 = Texture2 ?? VisionContent.Load<Texture2D>("rock");
            Texture3 = Texture3 ?? VisionContent.Load<Texture2D>("snow");

            HeightsMap = ground.CreateHeightsTexture(Effect.GraphicsDevice);
            WeightsMap = ground.CreateWeigthsMap().CreateTexture2D(Effect.GraphicsDevice);
            NormalsMap = normals.CreateTexture2D(Effect.GraphicsDevice);

            _slices = new terrainSlice[4];
            var raduis = 32*(float)Math.Sqrt(2);
            var i = 0;
            for (var y = 0; y < 2; y++)
                for (var x = 0; x < 2; x++)
                    _slices[i++] = new terrainSlice
                                       {
                                           TexOffsetAndScale = new Vector4(x/2f, y/2f, 0.5f, 0.5f),
                                           World = World*Matrix.CreateTranslation(64*x - 32, 0, 64*y - 32),
                                           BoundingSphere = new BoundingSphere(_position + new Vector3(64 * x - 32, 0, 64 * y - 32), 32)
                                       };

        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            var any = false;
            foreach (var slice in _slices)
                any |= slice.Visible = camera.BoundingFrustum.Contains(slice.BoundingSphere) != ContainmentType.Disjoint;

            if (!any)
                return false;

            Effect.Parameters["Texture0"].SetValue(Texture0);
            Effect.Parameters["Texture1"].SetValue(Texture1);
            Effect.Parameters["Texture2"].SetValue(Texture2);
            Effect.Parameters["Texture3"].SetValue(Texture3);

            Effect.Parameters["HeightsMap"].SetValue(HeightsMap);
            Effect.Parameters["WeightsMap"].SetValue(WeightsMap);
            Effect.Parameters["NormalsMap"].SetValue(NormalsMap);

            camera.UpdateEffect(Effect);

            foreach (var slice in _slices.Where(slice => slice.Visible))
            {
                Effect.Parameters["TexOffsetAndScale"].SetValue(slice.TexOffsetAndScale);
                TerrainPlane.Draw(camera, slice.World, drawingReason);
                Box.World = slice.World * Matrix.CreateTranslation(0,10,0);
                Box.Draw(camera, drawingReason, shadowMap);
            }

            return true;
        }

        private class terrainSlice
        {
            public Vector4 TexOffsetAndScale;
            public Matrix World;
            public BoundingSphere BoundingSphere;
            public bool Visible;
        }

    }

}
