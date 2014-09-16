using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionThing.Terrain
{
    public class SimpleBillboards : ClipDrawable
    {
        public readonly Matrix World;
        private ModelData.VertexBuffer _treeVertexBuffer;
        private readonly Texture2D _treeTexture;

        public readonly float Width;
        public readonly float Height;

        public SimpleBillboards(
            VisionContent vContent, 
            Matrix world,
            Texture2D texture,
            List<Vector3> positions, 
            float width,
            float height)
            : base(vContent.LoadPlainEffect("Effects/BillboardEffect"))
        {
            World = world;
            _treeTexture = texture;
            Width = width;
            Height = height;
            Effect.Parameters["AllowedRotDir"].SetValue(Vector3.Up);
            createBillboardVerticesFromList(positions);
        }

        private void createBillboardVerticesFromList(List<Vector3> treeList)
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

            //TODO
            //_treeVertexBuffer = new ModelData.VertexBuffer(
            //    Effect.GraphicsDevice,
            //    typeof(VertexPositionTexture),
            //    billboardVertices.Length,
            //    BufferUsage.WriteOnly);
            //_treeVertexBuffer.SetData(billboardVertices);
        }

        protected float Time;

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            //TODO
            //camera.UpdateEffect(Effect);
            //if ( drawingReason == DrawingReason.ShadowDepthMap )
            //    Effect.CameraPosition = shadowMap.RealCamera.Position;
            //Effect.World = World;
            //Effect.Parameters["WindTime"].SetValue(Time);
            //Effect.Parameters["BillboardWidth"].SetValue(Width);
            //Effect.Parameters["BillboardHeight"].SetValue(Height);
            //Effect.Texture = _treeTexture;
            //Effect.GraphicsDevice.SetVertexBuffer(_treeVertexBuffer);
            //Effect.Effect.CurrentTechnique.Passes[0].Apply();
            //Effect.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, _treeVertexBuffer.VertexCount / 3);
            return true;
        }

    }

}

