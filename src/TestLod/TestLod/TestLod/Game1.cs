using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;

namespace TestBed
{
    public struct VertexPositionNormalColored : IVertexType
    {
        public Vector3 Position;
        public Color Color;
        public Vector3 Normal;

        public static int SizeInBytes = 7 * 4;
        public static VertexElement[] VertexElements = new []
              {
                  new VertexElement( 0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0 ),
                  new VertexElement( sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0 ),
                  new VertexElement( sizeof(float) * 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0 ),
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





    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        GraphicsDevice device;

        int terrainWidth;
        int terrainLength;
        float[,] heightData;

        VertexBuffer terrainVertexBuffer;
        IndexBuffer terrainIndexBuffer;
        VertexDeclaration terrainVertexDeclaration;

        Effect effect;
        private NewBasicEffect _basicEffect;

        Matrix viewMatrix;
        Matrix projectionMatrix;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "TestLodContent";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 500;

            graphics.ApplyChanges();
            Window.Title = "Riemer's XNA Tutorials -- Series 4";

            base.Initialize();
        }

        private CubePrimitive<VertexPositionColor> _box;
 
        protected override void LoadContent()
        {
            VisionContent.Init(this);

            device = GraphicsDevice;
            var rz = new RasterizerState
                         {
                             CullMode = CullMode.None,
                             FillMode = FillMode.WireFrame
                         };
            device.RasterizerState = rz;

            _basicEffect = new NewBasicEffect(VisionContent.Load<Effect>("effects/NewBasicEffect"));
            _box = new CubePrimitive<VertexPositionColor>(device,
                                                          (a, b, c) =>
                                                          new VertexPositionColor(a, Color.BurlyWood), 10);

            effect = Content.Load<Effect>("Series4Effects");
            viewMatrix = Matrix.CreateLookAt(new Vector3(130, 30, -50), new Vector3(0, 0, -40), new Vector3(0, 1, 0));
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 0.3f, 1000.0f);

            LoadVertices();
        }

        private void LoadVertices()
        {

            Texture2D heightMap = Content.Load<Texture2D>("heightmap"); LoadHeightData(heightMap);

            VertexPositionNormalColored[] terrainVertices = SetUpTerrainVertices();
            int[] terrainIndices = SetUpTerrainIndices();
            terrainVertices = CalculateNormals(terrainVertices, terrainIndices);
            CopyToTerrainBuffers(terrainVertices, terrainIndices);
        }

        private void LoadHeightData(Texture2D heightMap)
        {
            float minimumHeight = float.MaxValue;
            float maximumHeight = float.MinValue;

            terrainWidth = heightMap.Width;
            terrainLength = heightMap.Height;

            Color[] heightMapColors = new Color[terrainWidth * terrainLength];
            heightMap.GetData(heightMapColors);

            heightData = new float[terrainWidth, terrainLength];
            for (int x = 0; x < terrainWidth; x++)
                for (int y = 0; y < terrainLength; y++)
                {
                    heightData[x, y] = heightMapColors[x + y * terrainWidth].R;
                    if (heightData[x, y] < minimumHeight) minimumHeight = heightData[x, y];
                    if (heightData[x, y] > maximumHeight) maximumHeight = heightData[x, y];
                }

            for (int x = 0; x < terrainWidth; x++)
                for (int y = 0; y < terrainLength; y++)
                    heightData[x, y] = (heightData[x, y] - minimumHeight) / (maximumHeight - minimumHeight) * 30.0f;
        }

        private VertexPositionNormalColored[] SetUpTerrainVertices()
        {
            VertexPositionNormalColored[] terrainVertices = new VertexPositionNormalColored[terrainWidth * terrainLength];

            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainLength; y++)
                {
                    terrainVertices[x + y * terrainWidth].Position = new Vector3(x, heightData[x, y], -y);

                    if (heightData[x, y] < 6)
                        terrainVertices[x + y * terrainWidth].Color = Color.Blue;
                    else if (heightData[x, y] < 15)
                        terrainVertices[x + y * terrainWidth].Color = Color.Green;
                    else if (heightData[x, y] < 25)
                        terrainVertices[x + y * terrainWidth].Color = Color.Brown;
                    else
                        terrainVertices[x + y * terrainWidth].Color = Color.White;
                }
            }

            return terrainVertices;
        }

        private int[] SetUpTerrainIndices()
        {
            int[] indices = new int[(terrainWidth - 1) * (terrainLength - 1) * 6];
            int counter = 0;
            for (int y = 0; y < terrainLength - 1; y++)
            {
                for (int x = 0; x < terrainWidth - 1; x++)
                {
                    int lowerLeft = x + y * terrainWidth;
                    int lowerRight = (x + 1) + y * terrainWidth;
                    int topLeft = x + (y + 1) * terrainWidth;
                    int topRight = (x + 1) + (y + 1) * terrainWidth;

                    indices[counter++] = topLeft;
                    indices[counter++] = lowerRight;
                    indices[counter++] = lowerLeft;

                    indices[counter++] = topLeft;
                    indices[counter++] = topRight;
                    indices[counter++] = lowerRight;
                }
            }

            return indices;
        }

        private VertexPositionNormalColored[] CalculateNormals(VertexPositionNormalColored[] vertices, int[] indices)
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

        private void CopyToTerrainBuffers(VertexPositionNormalColored[] vertices, int[] indices)
        {
            terrainVertexBuffer = new VertexBuffer(device, typeof(VertexPositionNormalColored), vertices.Length, BufferUsage.WriteOnly);
            terrainVertexBuffer.SetData(vertices);

            terrainIndexBuffer = new IndexBuffer(device, typeof(int), indices.Length, BufferUsage.WriteOnly);
            terrainIndexBuffer.SetData(indices);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            float time = (float)gameTime.TotalGameTime.TotalMilliseconds / 100.0f;

            //device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            device.Clear(Color.Black);
            DrawTerrain(viewMatrix);

            base.Draw(gameTime);
        }

        //private BasicEffect be = new BasicEffect();

        private void DrawTerrain(Matrix currentViewMatrix)
        {
            //effect.CurrentTechnique = effect.Techniques["Colored"];
            Matrix worldMatrix = Matrix.Identity;
            //effect.Parameters["xWorld"].SetValue(worldMatrix);
            //effect.Parameters["xView"].SetValue(currentViewMatrix);
            //effect.Parameters["xProjection"].SetValue(projectionMatrix);

            //effect.Parameters["xEnableLighting"].SetValue(true);
            //effect.Parameters["xAmbient"].SetValue(0.4f);
            //effect.Parameters["xLightDirection"].SetValue(new Vector3(-0.5f, -1, -0.5f));

            var effect = _basicEffect;
            effect.World = worldMatrix;
            effect.View = currentViewMatrix;
            effect.Projection = projectionMatrix;
            
            effect.GraphicsDevice.SetVertexBuffer(terrainVertexBuffer);
            effect.GraphicsDevice.Indices = terrainIndexBuffer;

            foreach (EffectPass pass in effect.Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                effect.GraphicsDevice.DrawIndexedPrimitives(
                    PrimitiveType.TriangleList,
                    0, 0, terrainVertexBuffer.VertexCount,
                    0, terrainIndexBuffer.IndexCount);

            }

            _box.Draw(effect);

        }
    }
}
