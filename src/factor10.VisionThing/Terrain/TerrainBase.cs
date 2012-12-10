using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factor10.VisionThing.Terrain
{
    public class TerrainBase : ClipDrawable
    {
        private static readonly TerrainPlane TerrainPlane;

        protected Matrix World;
        private Vector3 _position;

        protected Texture2D Texture0;
        protected Texture2D Texture1;
        protected Texture2D Texture2;
        protected Texture2D Texture3;

        protected Texture2D _heightsMap;
        protected Texture2D _weightsMap;
        protected Texture2D _normalsMap;

        private BoundingSphere _boundingSphere;

        protected Vector4 TexOffsetAndScale = new Vector4(0, 0, 1, 1);

        static TerrainBase()
        {
            TerrainPlane = new TerrainPlane();
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
            _position = World.Translation;

            Texture0 = Texture0 ?? VisionContent.Load<Texture2D>("sand");
            Texture1 = Texture1 ?? VisionContent.Load<Texture2D>("grass");
            Texture2 = Texture2 ?? VisionContent.Load<Texture2D>("rock");
            Texture3 = Texture3 ?? VisionContent.Load<Texture2D>("snow");

            _heightsMap = ground.CreateHeightsTexture(Effect.GraphicsDevice);
            _weightsMap = ground.CreateWeigthsMap().CreateTexture2D(Effect.GraphicsDevice);
            _normalsMap = normals.CreateTexture2D(Effect.GraphicsDevice);

            _boundingSphere = new BoundingSphere(_position, (float) Math.Sqrt(64*64 + 64*64));
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            if (camera.BoundingFrustum.Contains(_boundingSphere) == ContainmentType.Disjoint)
                return false;

            Effect.Parameters["TexOffsetAndScale"].SetValue(TexOffsetAndScale);

            Effect.Parameters["Texture0"].SetValue(Texture0);
            Effect.Parameters["Texture1"].SetValue(Texture1);
            Effect.Parameters["Texture2"].SetValue(Texture2);
            Effect.Parameters["Texture3"].SetValue(Texture3);

            Effect.Parameters["HeightsMap"].SetValue(_heightsMap);
            Effect.Parameters["WeightsMap"].SetValue(_weightsMap);
            Effect.Parameters["NormalsMap"].SetValue(_normalsMap);

            camera.UpdateEffect(Effect);
            TerrainPlane.Draw(camera, World, drawingReason);
            return true;
        }

    }

}
