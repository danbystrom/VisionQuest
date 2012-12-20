using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factor10.VisionThing.Terrain
{
    public class ReimersBillboards : ClipDrawable
    {
        private readonly Matrix _world;
        private VertexBuffer _treeVertexBuffer;
        private readonly Texture2D _treeTexture;

        public ReimersBillboards(
            Matrix world,
            Texture2D texture)
            : base(VisionContent.LoadPlainEffect("Effects/BillboardEffect"))
        {
            _world = world;
            _treeTexture = texture;
            Effect.Parameters["AllowedRotDir"].SetValue(new Vector3(0, 1, 0));
        }

        public ReimersBillboards(
            Matrix world,
            Ground ground,
            ColorSurface normals,
            Texture2D texture)
            : this(world,texture)
        {
            var treeMap = VisionContent.Load<Texture2D>("treeMap");
            var treeList = generateTreePositions(treeMap, ground, normals);
            CreateBillboardVerticesFromList(treeList);
        }

        public void CreateBillboardVerticesFromList(List<Vector3> treeList)
        {
            if (!treeList.Any())
                return;

            var billboardVertices = new VertexPositionTexture[treeList.Count*6];
            var i = 0;
            foreach (var currentV3 in treeList)
            {
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(0, 0));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(1, 0));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(1, 1));

                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(0, 0));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(1, 1));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(0, 1));
            }

            _treeVertexBuffer = new VertexBuffer(
                Effect.GraphicsDevice,
                typeof(VertexPositionTexture),
                billboardVertices.Length,
                BufferUsage.WriteOnly);
            _treeVertexBuffer.SetData(billboardVertices);
        }


        private List<Vector3> generateTreePositions(Texture2D treeMap, Ground ground, ColorSurface normals)
        {
            var treeMapColors = new Color[treeMap.Width*treeMap.Height];
            treeMap.GetData(treeMapColors);

            var noiseData = new int[treeMap.Width,treeMap.Height];
            for (var x = 0; x < treeMap.Width; x++)
                for (var y = 0; y < treeMap.Height; y++)
                    noiseData[x, y] = treeMapColors[y + x*treeMap.Height].R;

            var treeList = new List<Vector3>();
            var random = new Random();

            var minFlatness = (float) Math.Cos(MathHelper.ToRadians(15));
            for (var y = normals.Height - 2; y > 0; y--)
                for (var x = normals.Width - 2; x > 0; x--)
                {
                    var terrainHeight = ground[x, y];
                    if ((terrainHeight <= 8) || (terrainHeight >= 14))
                        continue;
                    var flatness1 = Vector3.Dot(normals.AsVector3(x, y), Vector3.Up);
                    var flatness2 = Vector3.Dot(normals.AsVector3(x+1, y+1), Vector3.Up);
                    if (flatness1 <= minFlatness || flatness2 <= minFlatness)
                        continue;
                    var relx = (float)x / normals.Width;
                    var rely = (float)y / normals.Height;

                    float noiseValueAtCurrentPosition = noiseData[(int)(relx * treeMap.Width), (int)(rely * treeMap.Height)];
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
                        var rand1 = (float)random.NextDouble();
                        var rand2 = (float)random.NextDouble();
                        treeList.Add(new Vector3(
                                         x + rand1,
                                         ground.GetExactHeight(x,y,rand1,rand2),
                                         y + rand2));
                    }
                }

            return treeList;
        }

        private float _time;
        public override void Update(GameTime gameTime)
        {
            _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            camera.UpdateEffect(Effect);
            Effect.World = _world;
            Effect.Parameters["WindTime"].SetValue(_time);
            Effect.Parameters["AlphaTestDirection"].SetValue(1);
            Effect.Parameters["BillboardWidth"].SetValue(10);
            Effect.Parameters["BillboardHeight"].SetValue(5);
            Effect.Texture = _treeTexture;
            Effect.GraphicsDevice.SetVertexBuffer(_treeVertexBuffer);
            Effect.Effect.CurrentTechnique.Passes[0].Apply();
            Effect.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, _treeVertexBuffer.VertexCount/3);
            return true;
        }

    }

}

