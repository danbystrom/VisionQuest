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
    public struct VertexMultitextured : IVertexType
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector4 TextureCoordinate;
        public Vector4 TexWeights;
 
     public static int SizeInBytes = (3 + 3 + 4 + 4) * sizeof(float);
        public static VertexElement[] VertexElements = new[]
              {
                  new VertexElement( 0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0 ),
                  new VertexElement( sizeof(float) * 3, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0 ),
                  new VertexElement( sizeof(float) * 6, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 0 ),
                  new VertexElement( sizeof(float) * 10, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 1 ),
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


    public class Reimer
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly ContentManager _cm;

        int _terrainWidth;
        int _terrainLength;
        float[,] _heightData;

        private VertexBuffer _terrainVertexBuffer;
        private IndexBuffer _terrainIndexBuffer;

        private VertexBuffer _waterVertexBuffer;
        private IndexBuffer _waterIndexBuffer;

        private VertexBuffer _treeVertexBuffer;
        private IndexBuffer _treeIndexBuffer;

        readonly IEffect _effect;

        private Texture2D _grassTexture;
        private Texture2D _sandTexture;
        private Texture2D _rockTexture;
        private Texture2D _snowTexture;
        private Texture2D _waterBumpMap;

        private RenderTarget2D _refractionRenderTarget;
        private RenderTarget2D _reflectionRenderTarget;
        private Matrix _reflectionViewMatrix;
        private float _waterHeight = 5;

        private Vector3 _windDirection = new Vector3(0, 0, 1);
 
        public Reimer(GraphicsDeviceManager graphics, ContentManager content)
        {
            _graphics = graphics;
            _cm = content;

            _effect = VisionContent.LoadPlainEffect("Series4Effects");

            PresentationParameters pp = _graphics.GraphicsDevice.PresentationParameters;
            _refractionRenderTarget = new RenderTarget2D(
                _graphics.GraphicsDevice,
                pp.BackBufferWidth,
                pp.BackBufferHeight,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24);
            _reflectionRenderTarget = new RenderTarget2D(
                _graphics.GraphicsDevice,
                pp.BackBufferWidth,
                pp.BackBufferHeight,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24);

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

            setUpWaterVertices();

            var treeMap = _cm.Load<Texture2D>("treeMap");
            var treeList = generateTreePositions(treeMap, terrainVertices);
            createBillboardVerticesFromList(treeList);

            //_fullScreenVertices = setUpFullscreenVertices();
        }

        private void loadTextures()
        {
            _grassTexture = VisionContent.Load<Texture2D>("grass");
            _sandTexture = VisionContent.Load<Texture2D>("sand");
            _rockTexture = VisionContent.Load<Texture2D>("rock");
            _snowTexture = VisionContent.Load<Texture2D>("snow");
            _waterBumpMap = VisionContent.Load<Texture2D>("waterbump");

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
            {
                for (int x = 0; x < _terrainWidth - 1; x++)
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

        public void Draw(Camera camera, float time, RenderTarget2D xxx)
        {
            drawWater(camera, time, xxx);
            drawTerrain(camera);
        }

        private void drawWater(Camera camera, float time, RenderTarget2D xxx)
        {
            var rcamera = new Camera(
                new Vector2(_reflectionRenderTarget.Width, _reflectionRenderTarget.Height),
                Vector3.Zero,
                Vector3.Up);

            Vector3 reflCameraPosition = camera.Position;
            reflCameraPosition.Y = -camera.Position.Y + _waterHeight * 2;
            Vector3 reflTargetPos = camera.Target;
            reflTargetPos.Y = -camera.Target.Y + _waterHeight * 2;

            rcamera.Update(
                reflCameraPosition,
                reflTargetPos);


            _effect.Effect.CurrentTechnique = _effect.Effect.Techniques["Water"];

            Matrix worldMatrix = Matrix.Identity*Matrix.CreateScale(4, 1, 4);
            _effect.Parameters["World"].SetValue(worldMatrix);
            _effect.Parameters["ReflectionView"].SetValue(rcamera.View);
            _effect.Parameters["ReflectionMap"].SetValue(xxx); // _refractionRenderTarget);
            _effect.Parameters["RefractionMap"].SetValue(_grassTexture); // _reflectionRenderTarget);
            _effect.Parameters["WaterBumpMap"].SetValue(_waterBumpMap);
            _effect.Parameters["WaveLength"].SetValue(0.1f);
            _effect.Parameters["WaveHeight"].SetValue(0.3f);
            _effect.Parameters["CameraPosition"].SetValue(camera.Position);
            _effect.Parameters["Time"].SetValue(time);
            _effect.Parameters["WindForce"].SetValue(0.002f);
            _effect.Parameters["WindDirection"].SetValue(_windDirection);
 
            _effect.GraphicsDevice.SetVertexBuffer(_waterVertexBuffer);
 
            _effect.View = camera.View;
            _effect.Projection = camera.Projection;
            _effect.World = worldMatrix;

            foreach (EffectPass pass in _effect.Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _effect.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, _waterVertexBuffer.VertexCount);
            }
        }

        private void drawTerrain(Camera camera)
        {
            _effect.Effect.CurrentTechnique = _effect.Effect.Techniques["MultiTextured"];

            _effect.Parameters["Texture0"].SetValue(_sandTexture);
            _effect.Parameters["Texture1"].SetValue(_grassTexture);
            _effect.Parameters["Texture2"].SetValue(_rockTexture);
            _effect.Parameters["Texture3"].SetValue(_snowTexture);

            Matrix worldMatrix = Matrix.Identity;

            _effect.Parameters["EnableLighting"].SetValue(true);
            _effect.Parameters["Ambient"].SetValue(0.4f);
            _effect.Parameters["LightDirection"].SetValue(new Vector3(-0.5f, -1, -0.5f));

            _effect.GraphicsDevice.SetVertexBuffer(_terrainVertexBuffer);
            _effect.GraphicsDevice.Indices = _terrainIndexBuffer;

            _effect.View = camera.View;
            _effect.Projection = camera.Projection;
            _effect.World = worldMatrix;

            foreach (EffectPass pass in _effect.Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _effect.GraphicsDevice.DrawIndexedPrimitives(
                    PrimitiveType.TriangleList,
                    0, 0, _terrainVertexBuffer.VertexCount,
                    0, _terrainIndexBuffer.IndexCount/3);

            }

        }

        
        private void setUpWaterVertices()
        {
            var waterVertices = new VertexPositionTexture[6];

            waterVertices[0] = new VertexPositionTexture(new Vector3(0, _waterHeight, 0), new Vector2(0, 1));
            waterVertices[2] = new VertexPositionTexture(new Vector3(_terrainWidth, _waterHeight, -_terrainLength), new Vector2(1, 0));
            waterVertices[1] = new VertexPositionTexture(new Vector3(0, _waterHeight, -_terrainLength), new Vector2(0, 0));

            waterVertices[3] = new VertexPositionTexture(new Vector3(0, _waterHeight, 0), new Vector2(0, 1));
            waterVertices[5] = new VertexPositionTexture(new Vector3(_terrainWidth, _waterHeight, 0), new Vector2(1, 1));
            waterVertices[4] = new VertexPositionTexture(new Vector3(_terrainWidth, _waterHeight, -_terrainLength), new Vector2(1, 0));

            _waterVertexBuffer = new VertexBuffer(_graphics.GraphicsDevice, typeof(VertexPositionTexture), waterVertices.Length, BufferUsage.WriteOnly);
            _waterVertexBuffer.SetData(waterVertices);
        }


        private void createBillboardVerticesFromList(List<Vector3> treeList)
        {
            var billboardVertices = new VertexPositionTexture[treeList.Count * 6];
            int i = 0;
            foreach (Vector3 currentV3 in treeList)
            {
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(0, 0));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(1, 0));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(1, 1));

                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(0, 0));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(1, 1));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(0, 1));
            }

            _treeVertexBuffer = new VertexBuffer(_graphics.GraphicsDevice, typeof(VertexPositionTexture), billboardVertices.Length, BufferUsage.WriteOnly);
            _treeVertexBuffer.SetData(billboardVertices);
        }


        private List<Vector3> generateTreePositions(Texture2D treeMap, VertexMultitextured[] terrainVertices)        {
            Color[] treeMapColors = new Color[treeMap.Width * treeMap.Height];
            treeMap.GetData(treeMapColors);

            int[,] noiseData = new int[treeMap.Width, treeMap.Height];
            for (int x = 0; x < treeMap.Width; x++)
                for (int y = 0; y < treeMap.Height; y++)
                    noiseData[x, y] = treeMapColors[y + x * treeMap.Height].R;


            var treeList = new List<Vector3> ();
            Random random = new Random();

            for (int x = 0; x < _terrainWidth; x++)
            {
                for (int y = 0; y < _terrainLength; y++)
                {
                    float terrainHeight = _heightData[x, y];
                    if ((terrainHeight > 8) && (terrainHeight < 14))
                    {
                        float flatness = Vector3.Dot(terrainVertices[x + y * _terrainWidth].Normal, new Vector3(0, 1, 0));
                        float minFlatness = (float)Math.Cos(MathHelper.ToRadians(15));
                        if (flatness > minFlatness)
                        {
                            float relx = (float)x / (float)_terrainWidth;
                            float rely = (float)y / (float)_terrainLength;

                            float noiseValueAtCurrentPosition = noiseData[(int)(relx * treeMap.Width), (int)(rely * treeMap.Height)];
                            float treeDensity;
                            if (noiseValueAtCurrentPosition > 200)
                                treeDensity = 5;
                            else if (noiseValueAtCurrentPosition > 150)
                                treeDensity = 4;
                            else if (noiseValueAtCurrentPosition > 100)
                                treeDensity = 3;
                            else
                                treeDensity = 0;

                            for (int currDetail = 0; currDetail < treeDensity; currDetail++)
                            {
                                float rand1 = (float)random.Next(1000) / 1000.0f;
                                float rand2 = (float)random.Next(1000) / 1000.0f;
                                Vector3 treePos = new Vector3((float)x - rand1, 0, -(float)y - rand2);
                                treePos.Y = _heightData[x, y];
                                treeList.Add(treePos);
                            }
                        }
                    }
                }
            }

            return treeList;
        }

        private Texture2D CreateStaticMap(int resolution)
        {
            var rand = new Random();
            var noisyColors = new Color[resolution * resolution];
            for (int x = 0; x < resolution; x++)
                for (int y = 0; y < resolution; y++)
                    noisyColors[x + y * resolution] = new Color(new Vector3((float)rand.Next(1000) / 1000.0f, 0, 0));

            var noiseImage = new Texture2D(_graphics.GraphicsDevice, resolution, resolution);
            noiseImage.SetData(noisyColors);
            return noiseImage;
        }

        private VertexPositionTexture[] setUpFullscreenVertices()
        {
            var vertices = new VertexPositionTexture[4];

            vertices[0] = new VertexPositionTexture(new Vector3(-1, 1, 0f), new Vector2(0, 1));
            vertices[1] = new VertexPositionTexture(new Vector3(1, 1, 0f), new Vector2(1, 1));
            vertices[2] = new VertexPositionTexture(new Vector3(-1, -1, 0f), new Vector2(0, 0));
            vertices[3] = new VertexPositionTexture(new Vector3(1, -1, 0f), new Vector2(1, 0));

            return vertices;
        }
        /*
        private void updateViewMatrix()
        {
            Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);

            Vector3 cameraOriginalTarget = new Vector3(0, 0, -1);
            Vector3 cameraOriginalUpVector = new Vector3(0, 1, 0);
            Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);
            Vector3 cameraFinalTarget = cameraPosition + cameraRotatedTarget;
            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);

            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraFinalTarget, cameraRotatedUpVector);

            Vector3 reflCameraPosition = cameraPosition;
            reflCameraPosition.Y = -cameraPosition.Y + waterHeight * 2;
            Vector3 reflTargetPos = cameraFinalTarget;
            reflTargetPos.Y = -cameraFinalTarget.Y + waterHeight * 2;

            Vector3 cameraRight = Vector3.Transform(new Vector3(1, 0, 0), cameraRotation);
            Vector3 invUpVector = Vector3.Cross(cameraRight, reflTargetPos - reflCameraPosition);

            _reflectionViewMatrix = Matrix.CreateLookAt(reflCameraPosition, reflTargetPos, invUpVector);
        }

        private void DrawSkyDome(Matrix currentViewMatrix)
        {
            _graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = false;

            Matrix[] modelTransforms = new Matrix[skyDome.Bones.Count];
            skyDome.CopyAbsoluteBoneTransformsTo(modelTransforms);

            Matrix wMatrix = Matrix.CreateTranslation(0, -0.3f, 0) * Matrix.CreateScale(100) * Matrix.CreateTranslation(cameraPosition);
            foreach (ModelMesh mesh in skyDome.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    Matrix worldMatrix = modelTransforms[mesh.ParentBone.Index] * wMatrix;

                     currentEffect.CurrentTechnique = currentEffect.Techniques["SkyDome"];
                     currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
                     currentEffect.Parameters["xView"].SetValue(currentViewMatrix);
                     currentEffect.Parameters["xProjection"].SetValue(projectionMatrix);
                     currentEffect.Parameters["xTexture"].SetValue(cloudMap);
                 }
                 mesh.Draw();
             }
             _graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
         }
 */
         private Plane CreatePlane(Camera camera, float height, Vector3 planeNormalDirection, Matrix currentViewMatrix, bool clipSide)
         {
             planeNormalDirection.Normalize();
             Vector4 planeCoeffs = new Vector4(planeNormalDirection, height);
             if (clipSide)
                 planeCoeffs *= -1;
 
             Matrix worldViewProjection = currentViewMatrix * camera.Projection;
             Matrix inverseWorldViewProjection = Matrix.Invert(worldViewProjection);
             inverseWorldViewProjection = Matrix.Transpose(inverseWorldViewProjection);
 
             planeCoeffs = Vector4.Transform(planeCoeffs, inverseWorldViewProjection);
             Plane finalPlane = new Plane(planeCoeffs);
 
             return finalPlane;
         }
 
         private void drawRefractionMap(Camera camera)
         {
             Plane refractionPlane = CreatePlane(camera, _waterHeight + 1.5f, new Vector3(0, -1, 0), camera.View, false);
             //_graphics.GraphicsDevice.ClipPlanes[0].Plane = refractionPlane;
             //_graphics.GraphicsDevice.ClipPlanes[0].IsEnabled = true;
             _graphics.GraphicsDevice.SetRenderTarget(_refractionRenderTarget);
             _graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
             drawTerrain(camera);
            // _graphics.GraphicsDevice.ClipPlanes[0].IsEnabled = false;
 
             _graphics.GraphicsDevice.SetRenderTarget(null);
             //refractionMap = refractionRenderTarget.GetTexture();
         }

         private void drawReflectionMap(Camera camera)
         {
             Plane reflectionPlane = CreatePlane(camera, _waterHeight - 0.5f, new Vector3(0, -1, 0), _reflectionViewMatrix, true);
             //_graphics.GraphicsDevice.ClipPlanes[0].Plane = reflectionPlane;
             //_graphics.GraphicsDevice.ClipPlanes[0].IsEnabled = true;
             _graphics.GraphicsDevice.SetRenderTarget(_reflectionRenderTarget);
             _graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
             //DrawSkyDome(reflectionViewMatrix);
        //     DrawTerrain(_reflectionViewMatrix);
             //DrawBillboards(reflectionViewMatrix);
             //_graphics.GraphicsDevice.ClipPlanes[0].IsEnabled = false;
 
             _graphics.GraphicsDevice.SetRenderTarget(null);
         }
 /*
         private void DrawBillboards(Matrix currentViewMatrix)
         {
             bbEffect.CurrentTechnique = bbEffect.Techniques["CylBillboard"];
             bbEffect.Parameters["xWorld"].SetValue(Matrix.Identity);
             bbEffect.Parameters["xView"].SetValue(currentViewMatrix);
             bbEffect.Parameters["xProjection"].SetValue(projectionMatrix);
             bbEffect.Parameters["xCamPos"].SetValue(cameraPosition);
             bbEffect.Parameters["xAllowedRotDir"].SetValue(new Vector3(0, 1, 0));
             bbEffect.Parameters["xBillboardTexture"].SetValue(treeTexture);
 
             bbEffect.Begin();
 
             _graphics.GraphicsDevice.Vertices[0].SetSource(treeVertexBuffer, 0, VertexPositionTexture.SizeInBytes);
             _graphics.GraphicsDevice.VertexDeclaration = treeVertexDeclaration;
             int noVertices = treeVertexBuffer.SizeInBytes / VertexPositionTexture.SizeInBytes;
             int noTriangles = noVertices / 3;
             {
                 _graphics.GraphicsDevice.RenderState.AlphaTestEnable = true;
                 _graphics.GraphicsDevice.RenderState.AlphaFunction = CompareFunction.GreaterEqual;
                 _graphics.GraphicsDevice.RenderState.ReferenceAlpha = 200;
 
                 bbEffect.CurrentTechnique.Passes[0].Begin();
                 _graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, noTriangles);
                 bbEffect.CurrentTechnique.Passes[0].End();
             }
 
             {
                 _graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = false;
 
                 _graphics.GraphicsDevice.RenderState.AlphaBlendEnable = true;
                 _graphics.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
                 _graphics.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
 
                 _graphics.GraphicsDevice.RenderState.AlphaTestEnable = true;
                 _graphics.GraphicsDevice.RenderState.AlphaFunction = CompareFunction.Less;
                 _graphics.GraphicsDevice.RenderState.ReferenceAlpha = 200;
 
                 bbEffect.CurrentTechnique.Passes[0].Begin();
                 _graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, noTriangles);
                 bbEffect.CurrentTechnique.Passes[0].End();
             }
             
             _graphics.GraphicsDevice.RenderState.AlphaBlendEnable = false;
             _graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
             _graphics.GraphicsDevice.RenderState.AlphaTestEnable = false;            
 
             bbEffect.End();
         }
 
         private void GeneratePerlinNoise(float time)
         {
             _graphics.GraphicsDevice.SetRenderTarget(0, cloudsRenderTarget);
             _graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
                         
             effect.CurrentTechnique = effect.Techniques["PerlinNoise"];
             effect.Parameters["xTexture"].SetValue(cloudStaticMap);
             effect.Parameters["xOvercast"].SetValue(1.1f);
             effect.Parameters["xTime"].SetValue(time/1000.0f);
             effect.Begin();
             foreach (EffectPass pass in effect.CurrentTechnique.Passes)
             {
                 pass.Begin();
 
                 _graphics.GraphicsDevice.VertexDeclaration = fullScreenVertexDeclaration;
                 _graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, fullScreenVertices, 0, 2);
 
                 pass.End();
             }
             effect.End();
 
             _graphics.GraphicsDevice.SetRenderTarget(0, null);
             cloudMap = cloudsRenderTarget.GetTexture();
         }
     }
        */

    }
}
