﻿using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using Buffer = SharpDX.Toolkit.Graphics.Buffer;

namespace factor10.VisionThing.Terrain
{
    public class CxBillboard : ClipDrawable
    {
        private readonly Matrix _world;
        private Buffer<BillboardVertex> _vertexBuffer;
        private VertexInputLayout _vertexInputLayout;

        private readonly Texture2D _texture;
        private readonly float _billboardWidth;
        private readonly float _billboardHeight;

        private float _time;

        public CxBillboard(
            VisionContent vContent,
            Matrix world,
            Texture2D texture,
            float width,
            float height)
            : base(vContent.LoadPlainEffect("Billboards/CxBillboard", vContent.GraphicsDevice.SamplerStates.LinearClamp))
        {
            _world = world;
            _texture = texture;
            _billboardWidth = width;
            _billboardHeight = height;
        }

        public void GenerateTreePositions(GroundMap groundMap, ColorSurface normals)
        {
            var treeList = generateTreePositions(groundMap, normals);
            CreateBillboardVerticesFromList(treeList);
        }

        public void CreateBillboardVerticesFromList(List<Tuple<Vector3, Vector3>> treeList)
        {
            if (!treeList.Any())
                return;

            var billboardVertices = new BillboardVertex[treeList.Count * 6];
            int i = 0;
            var random = new Random();
            foreach (var t in treeList)
                createOne(
                    ref i,
                    billboardVertices,
                    t.Item1 + _world.TranslationVector,
                    t.Item2,
                    0.0001f + (float) random.NextDouble());

            _vertexBuffer = Buffer.Vertex.New(Effect.GraphicsDevice, billboardVertices);
            _vertexInputLayout = VertexInputLayout.FromBuffer(0, _vertexBuffer);
        }

        private void createOne(
            ref int i,
            BillboardVertex[] bv,
            Vector3 p,
            Vector3 n,
            float rnd)
        {
            n.Normalize();
            bv[i++] = new BillboardVertex(p, n, new Vector2(0, 0), rnd);
            bv[i++] = new BillboardVertex(p, n, new Vector2(1, 0), rnd);
            bv[i++] = new BillboardVertex(p, n, new Vector2(1, 1), rnd);

            bv[i++] = new BillboardVertex(p, n, new Vector2(0, 0), rnd);
            bv[i++] = new BillboardVertex(p, n, new Vector2(1, 1), rnd);
            bv[i++] = new BillboardVertex(p, n, new Vector2(0, 1), rnd);
        }

        private List<Tuple<Vector3, Vector3>> generateTreePositions(GroundMap groundMap, ColorSurface normals)
        {
            var treeList = new List<Tuple<Vector3,Vector3>>();
            var random = new Random();

            for (var y = normals.Height - 2; y > 0; y--)
                for (var x = normals.Width - 2; x > 0; x--)
                {
                    var height = groundMap[x, y];
                    if ( height <3 || height > 5)
                        continue;
                    for (var currDetail = 0; currDetail < 5; currDetail++)
                    {
                        var rand1 = (float) random.NextDouble();
                        var rand2 = (float) random.NextDouble();
                        treeList.Add(new Tuple<Vector3, Vector3>(
                                         new Vector3(
                                             x + rand1,
                                             groundMap.GetExactHeight(x, y, rand1, rand2),
                                             y + rand2),
                                         normals.AsVector3(x, y)));
                    }
                }

            return treeList;
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
            _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            if (_vertexBuffer == null)
                return false;

            camera.UpdateEffect(Effect);
            Effect.World = _world;
            Effect.Texture = _texture;

            Effect.GraphicsDevice.SetVertexBuffer(_vertexBuffer);
            Effect.GraphicsDevice.SetVertexInputLayout(_vertexInputLayout);

            Effect.Parameters["WindTime"].SetValue(_time);
            Effect.Parameters["BillboardWidth"].SetValue(_billboardWidth);
            Effect.Parameters["BillboardHeight"].SetValue(_billboardHeight);

            //pass one
            Effect.Parameters["AlphaTestDirection"].SetValue(1f);
            Effect.Effect.CurrentTechnique.Passes[0].Apply();
            Effect.GraphicsDevice.Draw(PrimitiveType.TriangleList, _vertexBuffer.ElementCount);

            if (drawingReason == DrawingReason.Normal)
            {
                //pass two
                Effect.GraphicsDevice.SetDepthStencilState(Effect.GraphicsDevice.DepthStencilStates.DepthRead);
                Effect.GraphicsDevice.SetBlendState(Effect.GraphicsDevice.BlendStates.NonPremultiplied);
                Effect.Parameters["AlphaTestDirection"].SetValue(-1f);
                Effect.Effect.CurrentTechnique.Passes[0].Apply();
                Effect.GraphicsDevice.Draw(PrimitiveType.TriangleList, _vertexBuffer.ElementCount);
                Effect.GraphicsDevice.SetDepthStencilState(Effect.GraphicsDevice.DepthStencilStates.Default);
                Effect.GraphicsDevice.SetBlendState(Effect.GraphicsDevice.BlendStates.Default);
            }

            return true;
        }

    }

}

