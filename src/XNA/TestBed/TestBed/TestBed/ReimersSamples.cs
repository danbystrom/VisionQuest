using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;
using factor10.VisionThing.Terrain;

namespace TestBed
{

    public class ReimersSamples
    {
        private readonly GraphicsDevice _graphics;
 
        private VertexBuffer _treeVertexBuffer;
        private VertexPositionTexture[] _fullScreenVertices;

        private readonly Effect _effect;
        private readonly IEffect _bbEffect;

        private readonly Texture2D _treeTexture;

        public ReimersSamples(
            GraphicsDevice graphics,
            Ground ground,
            ColorSurface normals)
        {
            _graphics = graphics;

            _effect = VisionContent.LoadPlainEffect("Series4Effects").Effect;
            _bbEffect = VisionContent.LoadPlainEffect("Effects/BillboardEffect");

            var treeMap = VisionContent.Load<Texture2D>("treeMap");
            var treeList = generateTreePositions(treeMap, ground, normals);
            createBillboardVerticesFromList(treeList);
            _fullScreenVertices = setUpFullscreenVertices();

            _treeTexture = VisionContent.Load<Texture2D>("tree");

            _bbEffect.Parameters["xAllowedRotDir"].SetValue(new Vector3(0, 1, 0));
            _bbEffect.Texture = _treeTexture;

        }


        private void createBillboardVerticesFromList(List<Vector3> treeList)
        {
            if (!treeList.Any())
                return;

            var billboardVertices = new VertexPositionTexture[treeList.Count*6];
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

            _treeVertexBuffer = new VertexBuffer(_graphics, typeof(VertexPositionTexture), billboardVertices.Length,
                                                 BufferUsage.WriteOnly);
            _treeVertexBuffer.SetData(billboardVertices);
        }


        private List<Vector3> generateTreePositions(Texture2D treeMap, Ground ground, ColorSurface normals)
        {
            var treeMapColors = new Color[treeMap.Width*treeMap.Height];
            treeMap.GetData(treeMapColors);

            int[,] noiseData = new int[treeMap.Width,treeMap.Height];
            for (int x = 0; x < treeMap.Width; x++)
                for (int y = 0; y < treeMap.Height; y++)
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

        private Texture2D CreateStaticMap(int resolution)
        {
            var rand = new Random();
            var noisyColors = new Color[resolution*resolution];
            for (int x = 0; x < resolution; x++)
                for (int y = 0; y < resolution; y++)
                    noisyColors[x + y*resolution] = new Color(new Vector3((float) rand.Next(1000)/1000.0f, 0, 0));

            var noiseImage = new Texture2D(_graphics, resolution, resolution);
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

            Matrix worldViewProjection = currentViewMatrix*camera.Projection;
            Matrix inverseWorldViewProjection = Matrix.Invert(worldViewProjection);
            inverseWorldViewProjection = Matrix.Transpose(inverseWorldViewProjection);

            planeCoeffs = Vector4.Transform(planeCoeffs, inverseWorldViewProjection);
            Plane finalPlane = new Plane(planeCoeffs);

            return finalPlane;
        }


        public void DrawBillboards(Camera camera, Matrix world, DrawingReason drawingReason)
        {
            if (_treeVertexBuffer == null)
                return;

            switch (drawingReason)
            {
                case DrawingReason.Normal:
                    _bbEffect.Effect.CurrentTechnique = _bbEffect.Effect.Techniques[0];
                    break;
                //case DrawingReason.ReflectionMap:
                //    _bbEffect.Effect.CurrentTechnique = _bbEffect.Effect.Techniques[1];
                //    break;
                case DrawingReason.ShadowDepthMap:
                    _bbEffect.Effect.CurrentTechnique = _bbEffect.Effect.Techniques[2];
                    break;
            }

            camera.UpdateEffect(_bbEffect);
            _bbEffect.World = world * Matrix.CreateTranslation(-64, -0.1f, -64);
            _graphics.SetVertexBuffer(_treeVertexBuffer);
            int noVertices = _treeVertexBuffer.VertexCount;
            int noTriangles = noVertices/3;

            _bbEffect.Effect.CurrentTechnique.Passes[0].Apply();
            _graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, noTriangles);
        }

        /*
        private void GeneratePerlinNoise(float time)
        {
            _graphics.GraphicsDevice.SetRenderTarget(cloudsRenderTarget);
            _graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            _effect.CurrentTechnique = _effect.Techniques["PerlinNoise"];
            _effect.Parameters["xTexture"].SetValue(_cloudStaticMap);
            _effect.Parameters["xOvercast"].SetValue(1.1f);
            _effect.Parameters["xTime"].SetValue(time/1000.0f);
            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, _fullScreenVertices, 0, 2);
            }

            _graphics.GraphicsDevice.SetRenderTarget(null);
            _cloudMap = cloudsRenderTarget.GetTexture();
        }
        */
    }

}

