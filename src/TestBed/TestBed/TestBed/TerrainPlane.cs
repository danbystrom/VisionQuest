using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TestBed;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;

namespace factor10.VisionThing
{
    public class TerrainPlane
    {
        private readonly PlanePrimitive<VertexPositionTexture> _plane;

        public readonly IEffect Effect;

        public TerrainPlane()
        {
            Effect = VisionContent.LoadPlainEffect("Effects/TerrainEffects");
            _plane = new PlanePrimitive<VertexPositionTexture>(
                Effect.GraphicsDevice,
                createVertex,
                128, 128, 5);
        }

        private VertexPositionTexture createVertex(float x, float y, int width, int height)
        {
            return new VertexPositionTexture(
                new Vector3(x-width/2f, 0, y-height/2f),
                new Vector2(x/width, y/height));
        }

        public void Draw(Camera camera, Matrix world, DrawingReason drawingReason, ReimersBillboards reimersBillboards)
        {
            camera.UpdateEffect(Effect);
            Effect.World = world;

            var distance = Vector3.Distance(camera.Position, world.Translation);
            var lod = 3;
            if (distance < 1800)
                lod = 2;
            if (distance < 600)
                lod = 1;
            if (distance < 300)
                lod = 0;
            if (drawingReason != DrawingReason.Normal)
                lod++;
            _plane.Draw(Effect, lod);

            if ( lod<3)
                reimersBillboards.Draw(camera, drawingReason);
        }

    }

}
