using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;

namespace factor10.VisionThing
{
    public class NewTerrain : ClipDrawable
    {
        private readonly PlanePrimitive<VertexPositionTexture> _plane;
        private readonly Matrix _world;
        private readonly Vector3 _position;

        private readonly Texture2D _texture0;
        private readonly Texture2D _texture1;
        private readonly Texture2D _texture2;
        private readonly Texture2D _texture3;

        private readonly Texture2D _heightsMap;
        private readonly Texture2D _weightsMap;
        private readonly Texture2D _normalsMap;

        public NewTerrain(
            GraphicsDevice graphicsDevice,
            Texture2D heightMap,
            Texture2D normalsMap,
            Matrix world,
            bool z)
          :  base(VisionContent.LoadPlainEffect("Effects/ReimersTerrainEffects"))
        {
            _plane = new PlanePrimitive<VertexPositionTexture>(
                Effect.GraphicsDevice,
                createVertex,
                128, 128, 4);
            _world = world;
            _position = _world.Translation;

            Ground ground;
            if (z)
            {
                ground = new Ground(VisionContent.Load<Texture2D>("heightmap"));
                ground.LowerEdges();
                _texture0 = VisionContent.Load<Texture2D>("sand");
                _texture1 = VisionContent.Load<Texture2D>("grass");
                _texture2 = VisionContent.Load<Texture2D>("rock");
                _texture3 = VisionContent.Load<Texture2D>("snow");
           }
            else
            {
                ground = Ground.CreateDoubleSize(heightMap);
                _texture0 = VisionContent.Load<Texture2D>("TerrainTextures/texBase");
                _texture1=VisionContent.Load<Texture2D>("TerrainTextures/texR");
                _texture2=VisionContent.Load<Texture2D>("TerrainTextures/texG");
                _texture3=VisionContent.Load<Texture2D>("TerrainTextures/texB");
            }

            //var rnd = new Random();
            //ground.AlterValues(() => 5+5*(float)rnd.NextDouble());
            //ground.FlattenRectangle(42, 42, 32);
            if (!z)
            {
                ground.AlterValues(() => 10);
                ground.Soften();
                ground.Soften();
                ground.ApplyNormalBellShape();
                ground.FlattenRectangle(10, 10, 20);
            }
            _heightsMap = ground.CreateHeightTexture(graphicsDevice);
            _weightsMap = ground.CreateWeigthTexture(graphicsDevice);
            _normalsMap = ground.CreateNormalsTexture(graphicsDevice);

            Effect.Parameters["EnableLighting"].SetValue(true);
            Effect.Parameters["Ambient"].SetValue(0.4f);
            Effect.Parameters["LightDirection"].SetValue(new Vector3(-0.5f, -1, -0.5f));

        }

        private VertexPositionTexture createVertex(float x, float y, int width, int height)
        {
            return new VertexPositionTexture(
                new Vector3(x-width/2, 0, y-height/2),
                new Vector2(x/width, y/height));
        }

        public override void Draw( Camera camera, IEffect effect )
        {
            camera.UpdateEffect(effect);

            effect.Parameters["Texture0"].SetValue(_texture0);
            effect.Parameters["Texture1"].SetValue(_texture1);
            effect.Parameters["Texture2"].SetValue(_texture2);
            effect.Parameters["Texture3"].SetValue(_texture3);

            effect.Parameters["HeightsMap"].SetValue(_heightsMap);
            effect.Parameters["WeightsMap"].SetValue(_weightsMap);
            effect.Parameters["NormalsMap"].SetValue(_normalsMap);

            effect.World = _world;
            var distance = Vector3.Distance(camera.Position, _position);
            var lod = 3;
            if (distance < 1500)
                lod = 2;
            if (distance < 500)
                lod = 1;
            if (distance < 200)
                lod = 0;
            _plane.Draw(effect, lod);
        }

    }

}
