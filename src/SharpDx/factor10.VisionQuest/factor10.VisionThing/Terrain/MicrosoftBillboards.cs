using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using Buffer = SharpDX.Toolkit.Graphics.Buffer;

namespace factor10.VisionThing.Terrain
{
    public class MicrosoftBillboards : ClipDrawable
    {
        private readonly Matrix _world;
        private Buffer<BillboardVertex> _vertexBuffer;
        private VertexInputLayout _vertexInputLayout;

        private readonly Texture2D _grassTexture;
        private readonly Texture2D _treeTexture;

        public MicrosoftBillboards(
            VisionContent vContent,
            Matrix world)
            : base(vContent.LoadPlainEffect("Billboards/CxBillboard", vContent.GraphicsDevice.SamplerStates.LinearClamp))
            //: base(vContent.LoadPlainEffect("Billboards/MSBillboard", vContent.GraphicsDevice.SamplerStates.LinearClamp))
        {
            _world = world;

            _grassTexture = vContent.Load<Texture2D>("billboards/grass");
            _treeTexture = vContent.Load<Texture2D>("billboards/trees");
        }

        public void GenerateTreePositions(Ground ground, ColorSurface normals)
        {
            var treeList = generateTreePositions(ground, normals);
            CreateBillboardVerticesFromList(treeList);
        }

        public void CreateBillboardVerticesFromList(List<Tuple<Vector3, Vector3>> treeList)
        {
            if (!treeList.Any())
                return;

            var billboardVertices = new BillboardVertex[treeList.Count * 6];
            int v = 0, i = 0;
            var random = new Random();
            foreach (var t in treeList)
                createOne(
                    ref i,
                    billboardVertices,
                    t.Item1 + _world.TranslationVector,
                    t.Item2,
                    (float) random.NextDouble());

            _vertexBuffer = Buffer.Vertex.New(Effect.GraphicsDevice, billboardVertices.ToArray());
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

        private List<Tuple<Vector3, Vector3>> generateTreePositions(Ground ground, ColorSurface normals)
        {
            var treeList = new List<Tuple<Vector3,Vector3>>();
            var random = new Random();

            for (var y = normals.Height - 2; y > 0; y--)
                for (var x = normals.Width - 2; x > 0; x--)
                {
                    var height = ground[x, y];
                    if ( height <3 || height > 5)
                        continue;
                    for (var currDetail = 0; currDetail < 5; currDetail++)
                    {
                        var rand1 = (float) random.NextDouble();
                        var rand2 = (float) random.NextDouble();
                        treeList.Add(new Tuple<Vector3, Vector3>(
                                         new Vector3(
                                             x + rand1,
                                             ground.GetExactHeight(x, y, rand1, rand2),
                                             y + rand2),
                                         normals.AsVector3(x, y)));
                    }
                }

            return treeList;
        }

        private float _time;
        public override void Update(Camera camera, GameTime gameTime)
        {
            _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            if (_vertexBuffer == null)
                return false;

            camera.UpdateEffect(Effect);
            //Effect.World = _world;
            Effect.Texture = _grassTexture;

            Effect.GraphicsDevice.SetVertexBuffer(_vertexBuffer);
            Effect.GraphicsDevice.SetVertexInputLayout(_vertexInputLayout);

            Effect.Parameters["WindTime"].SetValue(_time);
            //Effect.Parameters["BillboardWidth"].SetValue(100);
            //Effect.Parameters["BillboardHeight"].SetValue(100);

            //pass one
            //Effect.GraphicsDevice.SetRasterizerState(RasterizerState.CullNone);
            Effect.Parameters["AlphaTestDirection"].SetValue(1f);
            Effect.Effect.CurrentTechnique.Passes[0].Apply();
            Effect.GraphicsDevice.Draw(PrimitiveType.TriangleList, _vertexBuffer.ElementCount);

            if (drawingReason == DrawingReason.Normal)
            {
                //pass two
                //Effect.GraphicsDevice.BlendState = BlendState.NonPremultiplied;
                //Effect.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
                Effect.GraphicsDevice.SetDepthStencilState(Effect.GraphicsDevice.DepthStencilStates.DepthRead);
                Effect.GraphicsDevice.SetBlendState(Effect.GraphicsDevice.BlendStates.NonPremultiplied);
                Effect.Parameters["AlphaTestDirection"].SetValue(-1f);
                Effect.Effect.CurrentTechnique.Passes[0].Apply();
                Effect.GraphicsDevice.Draw(PrimitiveType.TriangleList, _vertexBuffer.ElementCount);
                Effect.GraphicsDevice.SetDepthStencilState(Effect.GraphicsDevice.DepthStencilStates.Default);
                Effect.GraphicsDevice.SetBlendState(Effect.GraphicsDevice.BlendStates.Default);
            }

            //restore
            //Effect.GraphicsDevice.BlendState = BlendState.Opaque;
            //Effect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            //Effect.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            //Effect.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            return true;
        }

    }

}

