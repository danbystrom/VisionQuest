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
        public const int Width = 64;
        public const int Height = 64;

        private readonly PlanePrimitive<VertexPositionTexture> _plane;
        private readonly Matrix _world;

        public NewTerrain(
            GraphicsDevice graphicsDevice,
            Texture2D heightMap,
            Texture2D normalsMap,
            Matrix world )
          :  base(VisionContent.LoadPlainEffect("Effects/ReimersTerrainEffects"))
        {
            _plane = new PlanePrimitive<VertexPositionTexture>(
                Effect.GraphicsDevice,
                createVertex,
                64, 64, 3);
            _world = world;

            Effect.Parameters["Texture0"].SetValue(VisionContent.Load<Texture2D>("sand"));
            Effect.Parameters["Texture1"].SetValue(VisionContent.Load<Texture2D>("grass"));
            Effect.Parameters["Texture2"].SetValue(VisionContent.Load<Texture2D>("rock"));
            Effect.Parameters["Texture3"].SetValue(VisionContent.Load<Texture2D>("snow"));

            heightMap = VisionContent.Load<Texture2D>("heightmap");
            var ground = new Ground(heightMap);
            //ground.ApplyNormalBellShape();
            Effect.Parameters["HeightsMap"].SetValue(ground.CreateHeightTexture(graphicsDevice));
            Effect.Parameters["WeightsMap"].SetValue(ground.CreateWeigthTexture(graphicsDevice));
            Effect.Parameters["NormalsMap"].SetValue(normalsMap);

            Effect.Parameters["EnableLighting"].SetValue(true);
            Effect.Parameters["Ambient"].SetValue(0.4f);
            Effect.Parameters["LightDirection"].SetValue(new Vector3(-0.5f, -1, -0.5f));

        }

        private VertexPositionTexture createVertex(float x, float y, int width, int height)
        {
            return new VertexPositionTexture(
                new Vector3(x*2, 0, y*2),
                new Vector2(x/width, y/height));
        }

        public override void Draw( Camera camera, IEffect effect )
        {
            camera.UpdateEffect(effect);
            effect.World = _world;
            _plane.Draw(effect,0);
        }

    }

}
