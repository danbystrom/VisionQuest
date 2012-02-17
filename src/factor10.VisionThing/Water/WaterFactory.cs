﻿using System;
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
         public static WaterSurface Create(GraphicsDevice graphicsDevice, ContentManager content)
         {

            var lightDirW = new Vector3(0.0f, -1.0f, -3.0f);
            lightDirW.Normalize();
            return new WaterSurface(
                graphicsDevice,
                new InitInfo
                    {
                        Fx = content.Load<Effect>(@"effects\waterdmap"),
                        DirLight = new DirLight
                                       {
                                           Ambient = new Vector4(0.3f, 0.3f, 0.3f, 1.0f),
                                           Diffuse = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                                           Spec = new Vector4(0.7f, 0.7f, 0.7f, 1.0f),
                                           DirW = lightDirW
                                       },
                        Mtrl = new Mtrl
                                   {
                                       Ambient = new Vector4(0.4f, 0.4f, 0.7f, 0.0f),
                                       Diffuse = new Vector4(0.4f, 0.4f, 0.7f, 1.0f),
                                       Spec = new Vector4(0.8f, 0.8f, 0.8f, 0.8f),
                                       SpecPower = 128
                                   },
                        Rows = 128,
                        Columns = 128,
                        dx = 0.25f,
                        dz = 0.25f,
                        waveMap0 = content.Load<Texture2D>(@"textures\wave0"),
                        waveMap1 = content.Load<Texture2D>(@"textures\wave1"),
                        dmap0 = foobar(graphicsDevice, content, @"textures\waterdmap0"),
                        dmap1 = foobar(graphicsDevice, content, @"textures\waterdmap1"),
                        waveNMapVelocity0 = new Vector2(0.05f, 0.07f),
                        waveNMapVelocity1 = new Vector2(-0.01f, 0.13f),
                        waveDMapVelocity0 = new Vector2(0.012f, 0.015f),
                        waveDMapVelocity1 = new Vector2(0.014f, 0.05f),
                        scaleHeights = new Vector2(0.7f, 1.1f),
                        texScale = 8.0f
                    });
        }

        private static Texture2D foobar(GraphicsDevice graphicsDevice, ContentManager content, string name)
        {
            using (var z = content.Load<Texture2D>(name))
            {
                var oldData = new Color[z.Width*z.Height];
                var newData = new float[z.Width*z.Height];
                z.GetData(oldData);
                for (var i = 0; i < oldData.Length; i++)
                    newData[i] = oldData[i].R / 255f; //new Vector4(, oldData[i].G/255f, oldData[i].B/255f, oldData[i].A/255f);

                var result = new Texture2D(graphicsDevice, z.Width, z.Height, false, SurfaceFormat.Single);
                result.SetData(newData);
                return result;
            }

        }

        public static void DrawWaterSurfaceGrid(
            WaterSurface waterSurface,
            Camera camera)
        {
            const int waterW = 32;
            const int waterH = 32;
            const int worldW = 256;
            const int worldH = 256;

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
                    var pos = new Vector3((gridStartX + x)*waterW, -2, (gridStartY + y)*waterH);
                    waterSurface.Draw(
                        camera,
                        Matrix.CreateTranslation(pos),
                        Vector3.DistanceSquared(camera.Position, pos) > 10000);
                }
        }

    }

}
