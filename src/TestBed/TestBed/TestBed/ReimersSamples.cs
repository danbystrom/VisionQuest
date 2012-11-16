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

    public class ReimersSamples
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly ContentManager _cm;

        int _terrainWidth;
        int _terrainLength;
        float[,] _heightData;

        private VertexBuffer _terrainVertexBuffer;
        private IndexBuffer _terrainIndexBuffer;

        private VertexBuffer _waterVertexBuffer;
        private VertexBuffer _treeVertexBuffer;
        private VertexPositionTexture[] _fullScreenVertices;

        readonly IEffect _effect;

        private Texture2D _grassTexture;
        private Texture2D _sandTexture;
        private Texture2D _rockTexture;
        private Texture2D _snowTexture;
        private Texture2D _waterBumpMap;

        public ReimersSamples(GraphicsDeviceManager graphics, ContentManager content)
        {
            _graphics = graphics;
            _cm = content;

            _effect = VisionContent.LoadPlainEffect("Series4Effects");

            loadVertices();
            loadTextures();
        }

        private void loadVertices()
        {

            var treeMap = _cm.Load<Texture2D>("treeMap");
            //var treeList = generateTreePositions(treeMap, terrainVertices);
            //createBillboardVerticesFromList(treeList);

            _fullScreenVertices = setUpFullscreenVertices();
        }

        private void loadTextures()
        {
            _grassTexture = VisionContent.Load<Texture2D>("grass");
            _sandTexture = VisionContent.Load<Texture2D>("sand");
            _rockTexture = VisionContent.Load<Texture2D>("rock");
            _snowTexture = VisionContent.Load<Texture2D>("snow");
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


        private List<Vector3> generateTreePositions(Texture2D treeMap, ReimersTerrain.VertexMultitextured[] terrainVertices)
        {
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
 

 /*
         private void DrawBillboards(Matrix currentViewMatrix)
         {
             Effect bbEffect = null;

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
