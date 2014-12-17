using System;
using factor10.VisionThing.CameraStuff;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;
using SharpDX;

namespace factor10.VisionThing.Terrain
{
    public class TerrainPlane : IDisposable
    {
        private readonly PlanePrimitive<TerrainVertex> _loPlane;

        public const int SquareSize = 64;
        public readonly IVEffect Effect;

        public TerrainPlane(VisionContent vContent)
        {
            Effect = vContent.LoadEffect("Effects/Terrain");
            _loPlane = new PlanePrimitive<TerrainVertex>(
                Effect.GraphicsDevice,
                (x, y, w, h) => new TerrainVertex(
                    new Vector3(x, 0, y),
                    new Vector2(x/SquareSize, y/SquareSize),
                    x/SquareSize),
                SquareSize, SquareSize, 5);
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

        public void Dispose()
        {
            _loPlane.Dispose();
        }
    }

}
