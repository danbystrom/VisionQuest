using System;
using System.Collections.Generic;
using System.Linq;
using factor10.VisionThing.Effects;
using SharpDX.Direct3D11;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionThing.Primitives
{
    public abstract class GeometricPrimitive<T> : IDisposable, IDrawable where T : struct, IEquatable<T>
    {
        private List<T> _vertices = new List<T>();
        private List<List<uint>> _indicesOfLods = new List<List<uint>>();
        private List<uint> _indices;

        private ModelData.VertexBuffer _vertexBuffer;
        private ModelData.IndexBuffer[] _indexBuffers;

        protected GeometricPrimitive()
        {
            addLevelOfDetail();
        }

        protected void addVertex(T vertex)
        {
            _vertices.Add(vertex);
        }

        protected void addIndex(int index)
        {
            if (index > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("index");
            _indices.Add((ushort) index);
        }

        protected void addLevelOfDetail()
        {
            if (_indices != null && !_indices.Any())
                return;
            _indices = new List<uint>();
            _indicesOfLods.Add(_indices);
        }

        protected void addNullLevelOfDetail()
        {
            _indicesOfLods.Insert(_indicesOfLods.Count - 1, null);
        }

        protected int CurrentVertex
        {
            get { return _vertices.Count; }
        }

        protected void initializePrimitive(GraphicsDevice graphicsDevice)
        {
            //TODO
            //var vertices = SharpDX.Direct3D11.Buffer.Create<T>(
            //    graphicsDevice,
            //    BindFlags.VertexBuffer,
            //    0,
            //    0,
            //    ResourceUsage.Immutable,
            //    CpuAccessFlags.None,
            //    ResourceOptionFlags.None,
            //    0);

            //_vertexBuffer = new ModelData.VertexBuffer(
            //    graphicsDevice,
            //    typeof (T),
            //    _vertices.Count,
            //    BufferUsage.None);
            //_vertexBuffer.SetData(_vertices.ToArray());

            //_indexBuffers = new ModelData.IndexBuffer[_indicesOfLods.Count];
            //for (var i = 0; i < _indexBuffers.Length; i++)
            //    _indexBuffers[i] = createIndexBuffer(graphicsDevice, _indicesOfLods[i]);

            //_vertices = null;
            //_indices = null;
            //_indicesOfLods = null;
        }

        private ModelData.IndexBuffer createIndexBuffer(GraphicsDevice graphicsDevice, List<uint> indices)
        {
            //TODO
            //if (indices == null)
            //    return null;
            //ModelData.IndexBuffer indexBuffer;
            //if (_vertices.Count < 65536)
            //{
            //    indexBuffer = new ModelData.IndexBuffer(
            //        graphicsDevice,
            //        typeof (ushort),
            //        indices.Count, BufferUsage.None);
            //    indexBuffer.SetData(indices.ConvertAll(x => (ushort) x).ToArray());
            //}
            //else
            //{
            //    indexBuffer = new ModelData.IndexBuffer(
            //        graphicsDevice,
            //        typeof (uint),
            //        indices.Count, BufferUsage.None);
            //    indexBuffer.SetData(indices.ToArray());
            //}
            //return indexBuffer;
            return null;
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
            //TODO
            //if (!disposing)
            //    return;
            //if (_vertexBuffer != null)
            //{
            //    _vertexBuffer.Dispose();
            //    _vertexBuffer = null;
            //}
            //if (_indexBuffers != null)
            //{
            //    foreach (var idx in _indexBuffers)
            //        idx.Dispose();
            //    _indexBuffers = null;
            //}
        }

        public void Draw(IEffect effect, int lod = 0)
        {
            var graphicsDevice = effect.GraphicsDevice;

            //TODO
            //graphicsDevice.SetVertexBuffer(_vertexBuffer);
            //graphicsDevice.Indices = _indexBuffers[lod];

            //foreach (var effectPass in effect.Effect.CurrentTechnique.Passes)
            //{
            //    effectPass.Apply();
            //    if (_indexBuffers[lod] != null)
            //    {
            //        graphicsDevice.DrawIndexedPrimitives(
            //            PrimitiveType.TriangleList,
            //            0, 0, _vertexBuffer.VertexCount, 0, _indexBuffers[lod].IndexCount / 3);
            //        VisionContent.RenderedTriangles += _indexBuffers[lod].IndexCount / 3;
            //    }
            //    else
            //    {
            //        graphicsDevice.DrawPrimitives(
            //            PrimitiveType.TriangleList, 0, _vertexBuffer.VertexCount/3);
            //        VisionContent.RenderedTriangles += _vertexBuffer.VertexCount / 3;
            //    }
            //}

        }

    }

}
