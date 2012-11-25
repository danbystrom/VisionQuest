using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;

namespace TestBed
{


    public class ReimersTerrain : ClipDrawable
    {
        public struct VertexMultitextured : IVertexType
        {
            public Vector3 Position;
            public Vector3 Normal;
            public Vector4 TextureCoordinate;
            public Vector4 TexWeights;

            public static int SizeInBytes = (3 + 3 + 4 + 4)*sizeof (float);

            public static VertexElement[] VertexElements =
                new[]
                    {
                        new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                        new VertexElement(sizeof (float)*3, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                        new VertexElement(sizeof (float)*6, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 0),
                        new VertexElement(sizeof (float)*10, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 1),
                    };

            public static VertexDeclaration Declaration
            {
                get { return new VertexDeclaration(VertexElements); }
            }

            VertexDeclaration IVertexType.VertexDeclaration
            {
                get { return new VertexDeclaration(VertexElements); }
            }

        }

        private readonly GraphicsDeviceManager _graphics;
        private readonly ContentManager _cm;

        int _terrainWidth;
        int _terrainLength;
        float[,] _heightData;

        private VertexBuffer _terrainVertexBuffer;
        private IndexBuffer _terrainIndexBuffer;

        private Texture2D _grassTexture;
        private Texture2D _sandTexture;
        private Texture2D _rockTexture;
        private Texture2D _snowTexture;

        public ReimersTerrain(GraphicsDeviceManager graphics, ContentManager content)
            : base(VisionContent.LoadPlainEffect("Series4Effects"))
        {
            _graphics = graphics;
            _cm = content;

            loadVertices();
            loadTextures();
        }

        private void loadVertices()
        {
            var heightMap = _cm.Load<Texture2D>("heightmap");
            loadHeightData(heightMap);

            var terrainVertices = setUpTerrainVertices();
            var terrainIndices = setUpTerrainIndices();
            terrainVertices = calculateNormals(terrainVertices, terrainIndices);
            copyToTerrainBuffers(terrainVertices, terrainIndices);
        }

        private void loadTextures()
        {
            _grassTexture = VisionContent.Load<Texture2D>("grass");
            _sandTexture = VisionContent.Load<Texture2D>("sand");
            _rockTexture = VisionContent.Load<Texture2D>("rock");
            _snowTexture = VisionContent.Load<Texture2D>("snow");
        }

        private void loadHeightData(Texture2D heightMap)
        {
            float minimumHeight = float.MaxValue;
            float maximumHeight = float.MinValue;

            _terrainWidth = heightMap.Width;
            _terrainLength = heightMap.Height;

            var heightMapColors = new Color[_terrainWidth * _terrainLength];
            heightMap.GetData(heightMapColors);

            _heightData = new float[_terrainWidth, _terrainLength];
            for (int x = 0; x < _terrainWidth; x++)
                for (int y = 0; y < _terrainLength; y++)
                {
                    _heightData[x, y] = heightMapColors[x + y * _terrainWidth].R;
                    if (_heightData[x, y] < minimumHeight) minimumHeight = _heightData[x, y];
                    if (_heightData[x, y] > maximumHeight) maximumHeight = _heightData[x, y];
                }

            for (var x = 0; x < _terrainWidth; x++)
                for (var y = 0; y < _terrainLength; y++)
                    _heightData[x, y] = (_heightData[x, y] - minimumHeight) / (maximumHeight - minimumHeight) * 30.0f;
        }

        private VertexMultitextured[] setUpTerrainVertices()
        {
            var terrainVertices = new VertexMultitextured[_terrainWidth * _terrainLength];

            for (int x = 0; x < _terrainWidth; x++)
            {
                for (int y = 0; y < _terrainLength; y++)
                {
                    terrainVertices[x + y * _terrainWidth].Position = new Vector3(x, _heightData[x, y], -y);
                    terrainVertices[x + y * _terrainWidth].TextureCoordinate.X = (float)x / 30.0f;
                    terrainVertices[x + y * _terrainWidth].TextureCoordinate.Y = (float)y / 30.0f;

                    terrainVertices[x + y * _terrainWidth].TexWeights.X = MathHelper.Clamp(1.0f - Math.Abs(_heightData[x, y] - 0) / 8.0f, 0, 1);
                    terrainVertices[x + y * _terrainWidth].TexWeights.Y = MathHelper.Clamp(1.0f - Math.Abs(_heightData[x, y] - 12) / 6.0f, 0, 1);
                    terrainVertices[x + y * _terrainWidth].TexWeights.Z = MathHelper.Clamp(1.0f - Math.Abs(_heightData[x, y] - 20) / 6.0f, 0, 1);
                    terrainVertices[x + y * _terrainWidth].TexWeights.W = MathHelper.Clamp(1.0f - Math.Abs(_heightData[x, y] - 30) / 6.0f, 0, 1);

                    float total = terrainVertices[x + y * _terrainWidth].TexWeights.X;
                    total += terrainVertices[x + y * _terrainWidth].TexWeights.Y;
                    total += terrainVertices[x + y * _terrainWidth].TexWeights.Z;
                    total += terrainVertices[x + y * _terrainWidth].TexWeights.W;

                    terrainVertices[x + y * _terrainWidth].TexWeights.X /= total;
                    terrainVertices[x + y * _terrainWidth].TexWeights.Y /= total;
                    terrainVertices[x + y * _terrainWidth].TexWeights.Z /= total;
                    terrainVertices[x + y * _terrainWidth].TexWeights.W /= total;
                }
            }

            return terrainVertices;
        }

        private int[] setUpTerrainIndices()
        {
            var indices = new int[(_terrainWidth - 1) * (_terrainLength - 1) * 6];
            var counter = 0;
            for (var y = 0; y < _terrainLength - 1; y++)
                for (var x = 0; x < _terrainWidth - 1; x++)
                {
                    int lowerLeft = x + y * _terrainWidth;
                    int lowerRight = (x + 1) + y * _terrainWidth;
                    int topLeft = x + (y + 1) * _terrainWidth;
                    int topRight = (x + 1) + (y + 1) * _terrainWidth;

                    indices[counter++] = topLeft;
                    indices[counter++] = lowerRight;
                    indices[counter++] = lowerLeft;

                    indices[counter++] = topLeft;
                    indices[counter++] = topRight;
                    indices[counter++] = lowerRight;
                }

            return indices;
        }

        private static VertexMultitextured[] calculateNormals(VertexMultitextured[] vertices, int[] indices)
        {
            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal = new Vector3(0, 0, 0);

            for (int i = 0; i < indices.Length / 3; i++)
            {
                int index1 = indices[i * 3];
                int index2 = indices[i * 3 + 1];
                int index3 = indices[i * 3 + 2];

                Vector3 side1 = vertices[index1].Position - vertices[index3].Position;
                Vector3 side2 = vertices[index1].Position - vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                vertices[index1].Normal += normal;
                vertices[index2].Normal += normal;
                vertices[index3].Normal += normal;
            }

            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal.Normalize();

            return vertices;
        }

        private void copyToTerrainBuffers(VertexMultitextured[] vertices, int[] indices)
        {
            _terrainVertexBuffer = new VertexBuffer(_graphics.GraphicsDevice, typeof(VertexMultitextured), vertices.Length, BufferUsage.WriteOnly);
            _terrainVertexBuffer.SetData(vertices);

            _terrainIndexBuffer = new IndexBuffer(_graphics.GraphicsDevice, typeof(int), indices.Length, BufferUsage.WriteOnly);
            _terrainIndexBuffer.SetData(indices);
        }

        protected override void draw(Camera camera, DrawingReason drawingReason, IEffect effect, ShadowMap shadowMap)
        {
            //effect.Effect.CurrentTechnique = effect.Effect.Techniques["MultiTextured"];

            effect.Parameters["Texture0"].SetValue(_sandTexture);
            effect.Parameters["Texture1"].SetValue(_grassTexture);
            effect.Parameters["Texture2"].SetValue(_rockTexture);
            effect.Parameters["Texture3"].SetValue(_snowTexture);

            Matrix worldMatrix = Matrix.Identity * Matrix.CreateTranslation( 128, -3, - 128);

            effect.Parameters["EnableLighting"].SetValue(true);
            effect.Parameters["Ambient"].SetValue(0.4f);
            effect.Parameters["LightDirection"].SetValue(new Vector3(-0.5f, -1, -0.5f));

            effect.GraphicsDevice.SetVertexBuffer(_terrainVertexBuffer);
            effect.GraphicsDevice.Indices = _terrainIndexBuffer;

            effect.View = camera.View;
            effect.Projection = camera.Projection;
            effect.World = worldMatrix;

            foreach (var pass in Effect.Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Effect.GraphicsDevice.DrawIndexedPrimitives(
                    PrimitiveType.TriangleList,
                    0, 0, _terrainVertexBuffer.VertexCount,
                    0, _terrainIndexBuffer.IndexCount/3);

            }

        }

    }

}
