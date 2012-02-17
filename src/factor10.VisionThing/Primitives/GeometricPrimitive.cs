using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factor10.VisionThing.Primitives
{
    public abstract class GeometricPrimitive<T> : IDisposable, IDrawable where T: struct, IVertexType
    {
        private readonly List<T> _vertices = new List<T>();
        private readonly List<ushort> _indices = new List<ushort>();

        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;

        public BasicEffect BasicEffect;

        protected void addVertex(T vertex)
        {
            _vertices.Add(vertex);
        }

        protected void addIndex(int index)
        {
            if (index > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("index");
            _indices.Add((ushort)index);
        }

        protected int CurrentVertex
        {
            get { return _vertices.Count; }
        }

        protected void initializePrimitive(GraphicsDevice graphicsDevice)
        {
            _vertexBuffer = new VertexBuffer(
                graphicsDevice,
                typeof(T),
                _vertices.Count,
                BufferUsage.None);
            _vertexBuffer.SetData(_vertices.ToArray());

            _indexBuffer = new IndexBuffer(
                graphicsDevice,
                typeof(ushort),
                _indices.Count, BufferUsage.None);
            _indexBuffer.SetData(_indices.ToArray());
        }

        ~GeometricPrimitive()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            if (_vertexBuffer != null)
                _vertexBuffer.Dispose();
            if (_indexBuffer != null)
                _indexBuffer.Dispose();
        }

        public void Draw(Effect effect)
        {
            var graphicsDevice = effect.GraphicsDevice;

            graphicsDevice.SetVertexBuffer(_vertexBuffer);
            graphicsDevice.Indices = _indexBuffer;            

            foreach (var effectPass in effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
                var primitiveCount = _indices.Count / 3;
                graphicsDevice.DrawIndexedPrimitives(
                    PrimitiveType.TriangleList, 0, 0, _vertices.Count, 0, primitiveCount);
            }

        }

        public void Draw(
            Matrix world,
            Matrix view,
            Matrix projection,
            Color color)
        {
            BasicEffect.World = world;
            BasicEffect.View = view;
            BasicEffect.Projection = projection;
            BasicEffect.DiffuseColor = color.ToVector3();
            BasicEffect.Alpha = color.A / 255.0f;

            var device = BasicEffect.GraphicsDevice;
            device.DepthStencilState = DepthStencilState.Default;
            device.BlendState = color.A < 255
                ? BlendState.AlphaBlend
                : BlendState.Opaque;

            Draw(BasicEffect);
        }

    }

}
