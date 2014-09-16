using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionThing.Terrain
{
    public class TerrainPlane
    {
        private readonly PlaneMeshPrimitive<TerrainVertex> _hiPlane;
        private readonly PlanePrimitive<TerrainVertex> _loPlane;

        public readonly IEffect Effect;

        public TerrainPlane(VisionContent vContent)
        {
            const int sqsz = 64;
            Effect = vContent.LoadPlainEffect("Effects/TerrainEffects");
            _hiPlane = new PlaneMeshPrimitive<TerrainVertex>(
                Effect.GraphicsDevice,
                (x, y, t) => new TerrainVertex(
                new Vector3(x-sqsz/2f, 0, y-sqsz/2f),
                new Vector2(x/sqsz, y/sqsz),
                x / sqsz),
                sqsz, sqsz, 5);
            _loPlane = new PlanePrimitive<TerrainVertex>(
                Effect.GraphicsDevice,
                (x, y, w, h) => new TerrainVertex(
                new Vector3(x - sqsz / 2f, 0, y - sqsz / 2f),
                new Vector2(x / sqsz, y / sqsz),
                x / sqsz),
                sqsz, sqsz, 5);
        }

        private VertexPositionTexture createVertex(float x, float y, int width, int height)
        {
            return new VertexPositionTexture(
                new Vector3(x-width/2f, 0, y-height/2f),
                new Vector2(x/width, y/height));
        }

        public void Draw(Camera camera, Matrix world, DrawingReason drawingReason)
        {
            camera.UpdateEffect(Effect);
            Effect.World = world;

            var distance = Vector3.Distance(camera.Position, world.TranslationVector);
            var lod = 3;
            if (distance < 1800)
                lod = 2;
            if (distance < 600)
                lod = 1;
            if (distance < 300)
                lod = 0;
            if (drawingReason != DrawingReason.Normal)
                lod++;
            _loPlane.Draw(Effect, lod);
        }

    }

}
