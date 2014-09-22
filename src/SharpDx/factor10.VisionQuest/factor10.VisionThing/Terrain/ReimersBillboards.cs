using System;
using System.Collections.Generic;
using System.Security;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionThing.Terrain
{
    public class ReimersBillboards : SimpleBillboards
    {

        public ReimersBillboards(
            VisionContent vContent,
            Matrix world,
            Ground ground,
            ColorSurface normals,
            Texture2D texture)
            : base(vContent, world, texture, generateTreePositions(vContent.Load<Texture2D>("treeMap"), ground, normals), 6, 7)
        {
        }

        private static List<Vector3> generateTreePositions(Texture2D treeMap, Ground ground, ColorSurface normals)
        {
            var treeMapColors = new Color[treeMap.Description.Width*treeMap.Description.Height];
            treeMap. GetData(treeMapColors);

            var sz = new Size2(treeMap.Description.Width, treeMap.Description.Height);
            var noiseData = new int[sz.Width,sz.Height];
            for (var x = 0; x < sz.Width; x++)
                for (var y = 0; y < sz.Height; y++)
                    noiseData[x, y] = treeMapColors[y + x*sz.Height].R;

            var treeList = new List<Vector3>();
            var random = new Random();

            var minFlatness = (float) Math.Cos(MathUtil.DegreesToRadians(15));
            for (var y = normals.Height - 2; y > 0; y--)
                for (var x = normals.Width - 2; x > 0; x--)
                {
                    var terrainHeight = ground[x, y];
                    if ((terrainHeight <= 8) || (terrainHeight >= 14))
                        continue;
                    var flatness1 = Vector3.Dot(normals.AsVector3(x, y), Vector3.Up);
                    var flatness2 = Vector3.Dot(normals.AsVector3(x + 1, y + 1), Vector3.Up);
                    if (flatness1 <= minFlatness || flatness2 <= minFlatness)
                        continue;
                    var relx = (float) x/normals.Width;
                    var rely = (float) y/normals.Height;

                    float noiseValueAtCurrentPosition = noiseData[(int) (relx*sz.Width), (int) (rely*sz.Height)];
                    float treeDensity;
                    if (noiseValueAtCurrentPosition > 200)
                        treeDensity = 3;
                    else if (noiseValueAtCurrentPosition > 150)
                        treeDensity = 2;
                    else if (noiseValueAtCurrentPosition > 100)
                        treeDensity = 1;
                    else
                        treeDensity = 0;

                    for (var currDetail = 0; currDetail < treeDensity; currDetail++)
                    {
                        var rand1 = (float) random.NextDouble();
                        var rand2 = (float) random.NextDouble();
                        treeList.Add(new Vector3(
                                         x + rand1,
                                         ground.GetExactHeight(x, y, rand1, rand2),
                                         y + rand2));
                    }
                }

            return treeList;
        }

        public override void Update(GameTime gameTime)
        {
            Time += (float) gameTime.ElapsedGameTime.TotalSeconds;
        }

    }

}

