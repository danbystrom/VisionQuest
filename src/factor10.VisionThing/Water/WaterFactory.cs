using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factor10.VisionThing.Water
{
    public static class WaterFactory
    {
        public static WaterSurface Create(GraphicsDevice graphicsDevice)
        {
            return new WaterSurface(
                graphicsDevice,
                new InitInfo
                    {
                        Fx = VisionContent.Load<Effect>("effects/reflectedwater"),
                        LightDirection = VisionContent.SunlightDirectionReflectedWater,
                        SquareSize = 128,
                        dx = 0.25f,
                        dz = 0.25f,
                        dmap0 = foobar(graphicsDevice, @"textures\waterdmap0"),
                        dmap1 = foobar(graphicsDevice, @"textures\waterdmap1"),
                        waveMap0 = VisionContent.Load<Texture2D>(@"waterbump"),
                        waveMap1 = VisionContent.Load<Texture2D>(@"textures/wave1"),
                        Checker = VisionContent.Load<Texture2D>(@"checker"),
                        waveBumpMapVelocity0 = new Vector2(0.012f, 0.016f),
                        waveBumpMapVelocity1 = new Vector2(0.014f, 0.018f),
                        waveDispMapVelocity0 = new Vector2(0.012f, 0.015f),
                        waveDispMapVelocity1 = new Vector2(0.014f, 0.05f),
                        scaleHeights = new Vector2(0.7f, 1.1f),
                        texScale = 8.0f
                    });
        }

        private static Texture2D foobar(GraphicsDevice graphicsDevice, string name)
        {
            using (var z = VisionContent.Load<Texture2D>(name))
            {
                var oldData = new Color[z.Width*z.Height];
                var newData = new float[z.Width*z.Height];
                z.GetData(oldData);
                for (var i = 0; i < oldData.Length; i++)
                    newData[i] = oldData[i].R/255f;

                var result = new Texture2D(graphicsDevice, z.Width, z.Height, false, SurfaceFormat.Single);
                result.SetData(newData);
                return result;
            }

        }

        public static void DrawWaterSurfaceGrid(
            WaterSurface waterSurface,
            Camera camera,
            ShadowMap shadow,
            int nisse)
        {
            const int waterW = 64;
            const int waterH = 64;
            const int worldW = 32;
            const int worldH = 32;

            var boundingFrustum = camera.BoundingFrustum;

            waterSurface.Effect.SetShadowMapping(shadow);

            var gridStartX = (int) camera.Position.X/waterW - worldW/2;
            var gridStartY = (int) camera.Position.Z/waterH - worldH/2;

            Array.Clear(RenderedWaterPlanes, 0, RenderedWaterPlanes.Length);

            var drawDetails = camera.Position.Y < 300;
            if (drawDetails)
                for (var y = 0; y <= worldH; y++)
                    for (var x = 0; x <= worldW; x++)
                    {
                        var pos1 = new Vector3((gridStartX + x)*waterW, 0, (gridStartY + y)*waterH);
                        var pos2 = pos1 + new Vector3(waterW, 1, waterH);
                        var bb = new BoundingBox(pos1, pos2);
                        if (boundingFrustum.Contains(bb) == ContainmentType.Disjoint)
                            continue;

                        waterSurface.Draw(
                            camera,
                            pos1,
                            Vector3.Distance(camera.Position, pos1 - new Vector3(-32, 0, -32)),
                            x%8,
                            y%8);
                    }

            var size = (int) Math.Sqrt(camera.Position.Y);
            for (var y = -size+1; y < size; y++)
                for (var x = -size+1; x < size; x++)
                {
                    if (x == 0 && y == 0 && drawDetails)
                        continue;
                    var pos1 = new Vector3((gridStartX + x)*waterW +64, -0.5f, (gridStartY + y)*waterH + 64);
                    var pos2 = pos1 + new Vector3(1024, 1, 1024);
                    var bb = new BoundingBox(pos1, pos2);
                    if (boundingFrustum.Contains(bb) == ContainmentType.Disjoint)
                        continue;
                    waterSurface.Draw(camera, pos1, -1, 0, 0);
                }

        }

        public static int[] RenderedWaterPlanes = new int[6];

    }

}
