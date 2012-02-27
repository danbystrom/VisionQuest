using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factor10.VisionThing.Primitives
{
    public abstract class GeometricPrimitive<T> : IDisposable, IDrawable where T: struct, IVertexType
    {
        private List<T> _vertices = new List<T>();
        private List<uint> _indices = new List<uint>();

        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;

        private int _primitiveCount;
        private int _verticesCount;

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
                typeof (T),
                _vertices.Count,
                BufferUsage.None);
            _vertexBuffer.SetData(_vertices.ToArray());

            if (_vertices.Count < 65536)
            {
                _indexBuffer = new IndexBuffer(
                    graphicsDevice,
                    typeof (ushort),
                    _indices.Count, BufferUsage.None);
                _indexBuffer.SetData(_indices.ConvertAll(x => (ushort) x).ToArray());
            }
            else
            {
                _indexBuffer = new IndexBuffer(
                    graphicsDevice,
                    typeof (uint),
                    _indices.Count, BufferUsage.None);
                _indexBuffer.SetData(_indices.ToArray());
            }

            _primitiveCount = _indices.Count / 3;
            _verticesCount = _vertices.Count;

            _vertices = null;
            _indices = null;
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
                graphicsDevice.DrawIndexedPrimitives(
                    PrimitiveType.TriangleList, 0, 0, _verticesCount, 0, _primitiveCount);
            }

        }

        /*
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
        */
    }

}
