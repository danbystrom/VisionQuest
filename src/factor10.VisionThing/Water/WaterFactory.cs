using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing.Primitives;

namespace factor10.VisionThing.Water
{
    public static class WaterFactory
    {
         public static WaterSurface Create(GraphicsDevice graphicsDevice)
         {

            var lightDirW = new Vector3(5.0f, -1.0f, -3.0f);
            lightDirW.Normalize();
            return new WaterSurface(
                graphicsDevice,
                new InitInfo
                    {
                        Fx = VisionContent.Load<Effect>(@"effects\reflectedwater"),
                        DirLight = new DirLight
                                       {
                                           Ambient = new Vector4(0.3f, 0.3f, 0.3f, 1.0f),
                                           Diffuse = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                                           Spec = new Vector4(0.7f, 0.7f, 0.7f, 1.0f),
                                           DirW = lightDirW
                                       },
                        Mtrl = new Mtrl
                                   {
                                       Ambient = new Vector4(0.65f, 0.65f, 0.95f, 0.0f),
                                       Diffuse = new Vector4(0.65f, 0.65f, 0.95f, 1.0f),
                                       Spec = new Vector4(0.8f, 0.8f, 0.8f, 0.8f),
                                       SpecPower = 128
                                   },
                        SquareSize = 128,
                        dx = 0.25f,
                        dz = 0.25f,
                        waveMap0 = VisionContent.Load<Texture2D>(@"textures\wave0"),
                        waveMap1 = VisionContent.Load<Texture2D>(@"textures\wave1"),
                        dmap0 = foobar(graphicsDevice, @"textures\waterdmap0"),
                        dmap1 = foobar(graphicsDevice,  @"textures\waterdmap1"),
                        LakeBumpMap = VisionContent.Load<Texture2D>(@"waterbump"),
                        Checker = VisionContent.Load<Texture2D>(@"checker"),
                        waveNMapVelocity0 = new Vector2(0.05f, 0.07f),
                        waveNMapVelocity1 = new Vector2(-0.01f, 0.13f),
                        waveDMapVelocity0 = new Vector2(0.012f, 0.015f),
                        waveDMapVelocity1 = new Vector2(0.014f, 0.05f),
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
                    newData[i] = oldData[i].R / 255f;

                var result = new Texture2D(graphicsDevice, z.Width, z.Height, false, SurfaceFormat.Single);
                result.SetData(newData);
                return result;
            }

        }

        public static void zDrawWaterSurfaceGrid(
            WaterSurface waterSurface,
            Camera camera)
        {
            /*
            for (var y = 0; y < 2; y++)
                for (var x = 0; x < 8; x++)
                waterSurface.Draw(
                    camera,
                    Matrix.CreateTranslation(x*32,0,y*32),
                    y,false);
            for (var y = -1; y > -10; y--)
                for (var x = 0; x < 8; x++)
                    waterSurface.Draw(
                        camera,
                        Matrix.CreateTranslation(x * 256, 0.75f, y * 256),
                        y, true);
*/
            waterSurface.Draw(
                camera,
                1,
                new Vector3(64, 0.75f, 32),
                0, 0 ,0 ,0);
            waterSurface.Draw(
                camera,
                1,
                new Vector3(64 + 256, 0.75f, 32),
                0, 0, 0, 0);

            waterSurface.Draw(
                camera,
                1,
                new Vector3(32, 0, -32),
                0, 1, 0, 0);
            waterSurface.Draw(
                camera,
                1,
                new Vector3(32, 0, -64),
                0, 1, 0, 0);

            waterSurface.Draw(
                camera,
                1,
                new Vector3(32, 0, 0),
                0, 2, 0, 0);
            waterSurface.Draw(
                camera,
                1,
                new Vector3(32, 0, 32),
                0, 3, 0, 1);
        }


        public static void DrawWaterSurfaceGrid(
            WaterSurface waterSurface,
            Camera camera)
        {
            const int waterW = 32;
            const int waterH = 32;
            const int worldW = 32;
            const int worldH = 32;

            var boundingFrustum = new BoundingFrustum(camera.View*camera.Projection);
            var visible = new bool[worldW,worldH];

            var gridStartX = (int) camera.Position.X/waterW - worldW/2;
            var gridStartY = (int) camera.Position.Z/waterH - worldH/2;

            for (var y = 0; y < worldH; y++)
                for (var x = 0; x < worldW; x++)
                    visible[x, y] = boundingFrustum.Contains(new Vector3((gridStartX + x)*waterW, -2, (gridStartY + y)*waterH)) ==
                                    ContainmentType.Contains;
            visible[worldW/2, worldH/2] = true;

            for (var y = 1; y < worldH - 2; y++)
                for (var x = 1; x < worldW - 2; x++)
                {
                    var hit = false;
                    for (var ny = -1; ny < 3; ny++)
                        for (var nx = -1; nx < 3; nx++)
                            hit |= visible[x + nx, y + ny];
                    if (!hit)
                        continue;
                    var pos = new Vector3((gridStartX + x)*waterW, 0, (gridStartY + y)*waterH);
                    waterSurface.Draw(
                        camera,
                        1,
                        pos,
                        Vector3.DistanceSquared(camera.Position, pos),
                        3,
                        x % 8,
                        y % 8);
                }
        }

    }

}
