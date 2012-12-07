using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TestBed;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;

namespace factor10.VisionThing
{
    public class TerrainBase : ClipDrawable
    {
        private static readonly TerrainPlane _terrainPlane;

        private readonly Matrix _world;
        private readonly Vector3 _position;

        private readonly Texture2D _texture0;
        private readonly Texture2D _texture1;
        private readonly Texture2D _texture2;
        private readonly Texture2D _texture3;

        private readonly Texture2D _heightsMap;
        private readonly Texture2D _weightsMap;
        private readonly Texture2D _normalsMap;

        private readonly BoundingSphere _boundingSphere;
        private readonly ReimersBillboards _reimersBillboards;

        static TerrainBase()
        {
            _terrainPlane = new TerrainPlane();
        }

        public TerrainBase(
            Ground ground,
            Matrix world,
            Texture2D texture0 = null,
            Texture2D texture1 = null,
            Texture2D texture2 = null,
            Texture2D texture3 = null)
          :  base(_terrainPlane.Effect)
        {
            _world = world;
            _position = _world.Translation;

            _texture0 = texture0 ?? VisionContent.Load<Texture2D>("sand");
            _texture1 = texture1 ?? VisionContent.Load<Texture2D>("grass");
            _texture2 = texture2 ?? VisionContent.Load<Texture2D>("rock");
            _texture3 = texture3 ?? VisionContent.Load<Texture2D>("snow");

            _heightsMap = ground.CreateHeightTexture(Effect.GraphicsDevice);
            _weightsMap = ground.CreateWeigthsMap().CreateTexture2D(Effect.GraphicsDevice);
            _normalsMap = ground.CreateNormalsMap().CreateTexture2D(Effect.GraphicsDevice);

            Effect.Parameters["Ambient"].SetValue(0.5f);
            Effect.Parameters["LightingDirection"].SetValue(VisionContent.SunlightDirection);

            _boundingSphere = new BoundingSphere( _position, (float)Math.Sqrt(64*64+64*64));

            _reimersBillboards = new ReimersBillboards(
                _world * Matrix.CreateTranslation(-64, -0.1f, -64),
                ground,
                ground.CreateNormalsMap());

            Children.Add(_reimersBillboards);
        }

        protected override void draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            if (camera.BoundingFrustum.Contains(_boundingSphere) == ContainmentType.Disjoint)
                return;

            Effect.Parameters["Texture0"].SetValue(_texture0);
            Effect.Parameters["Texture1"].SetValue(_texture1);
            Effect.Parameters["Texture2"].SetValue(_texture2);
            Effect.Parameters["Texture3"].SetValue(_texture3);

            Effect.Parameters["HeightsMap"].SetValue(_heightsMap);
            Effect.Parameters["WeightsMap"].SetValue(_weightsMap);
            Effect.Parameters["NormalsMap"].SetValue(_normalsMap);

            camera.UpdateEffect(Effect);
            _terrainPlane.Draw(camera, _world, drawingReason, _reimersBillboards);
        }

    }

}
