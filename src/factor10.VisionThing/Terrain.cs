using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;

namespace factor10.VisionThing
{
    public class Terrain : ClipDrawable
    {
        public const int Width = 64;
        public const int Height = 64;

        private readonly PlanePrimitive<VertexPositionTexture> _plane;
        private readonly Matrix _world;

        public Terrain(
            GraphicsDevice graphicsDevice,
            Texture2D heightMap,
            Texture2D terrainMap,
            Texture2D normalsMap,
            Matrix world )
          :  base(VisionContent.LoadPlainEffect("Effects/terrainShader"))
        {
            _plane = new PlanePrimitive<VertexPositionTexture>(
                Effect.GraphicsDevice,
                createVertex,
                64, 64);
            _world = world;

            Effect.Parameters["HeightTexture"].SetValue(foobar(graphicsDevice,heightMap));
            Effect.Parameters["NormalTexture"].SetValue(normalsMap);
            Effect.Parameters["BlendTexture"].SetValue(terrainMap);
            Effect.Parameters["RTexture"].SetValue(VisionContent.Load<Texture2D>("TerrainTextures/TexR"));
            Effect.Parameters["GTexture"].SetValue(VisionContent.Load<Texture2D>("TerrainTextures/TexG"));
            Effect.Parameters["BTexture"].SetValue(VisionContent.Load<Texture2D>("TerrainTextures/TexB"));
            Effect.Parameters["BaseTexture"].SetValue(VisionContent.Load<Texture2D>("TerrainTextures/TexBase"));
            Effect.Parameters["globalScale"].SetValue(640);
        }

        private static Texture2D foobar(GraphicsDevice graphicsDevice, Texture2D z)
        {
            var w = z.Width;
            var h = z.Height;
            var wh = w/2f;
            var hh = h/2f;

            var oldData = new Color[w*h];
            var newData = new float[w*h];
            z.GetData(oldData);
            for (int i = 0, x = 0, y = 0; i < oldData.Length; i++)
            {
                var dx = (wh - x)/wh*5;
                var dy = (hh - y)/hh*5;

                newData[i] = oldData[i].R*(float)Math.Exp(-(dx * dx + dy * dy)/8) / 10;

                if (++x >= w)
                {
                    x = 0;
                    y++;
                }
            }

            var result = new Texture2D(graphicsDevice, z.Width, z.Height, false, SurfaceFormat.Single);
            result.SetData(newData);
            return result;
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
            _plane.Draw(effect);
        }

    }

}
