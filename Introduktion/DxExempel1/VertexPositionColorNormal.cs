using System;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace DxExempel1
{
    /// <summary>
    /// Custom vertex type for vertices that have just a
    /// position and a normal, without any texture coordinates.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexPositionColorNormal : IEquatable<VertexPositionColorNormal>
    {
        //public static readonly int Size = sizeof(float) * 3 * 2;

        [VertexElement("SV_Position")]
        public readonly Vector3 Position;

        [VertexElement("COLOR")]
        public readonly Color Color;

        [VertexElement("NORMAL")]
        public readonly Vector3 Normal;

        public VertexPositionColorNormal(Vector3 position, Color color, Vector3 normal)
        {
            Position = position;
            Color = color;
            Normal = normal;
        }

        public bool Equals(VertexPositionColorNormal other)
        {
            return Position.Equals(other.Position) && Color.Equals(other.Color) && Normal.Equals(other.Normal);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is VertexPositionColorNormal && Equals((VertexPositionColorNormal)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Position.GetHashCode();
                hashCode = (hashCode * 397) ^ Color.GetHashCode();
                hashCode = (hashCode * 397) ^ Normal.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(VertexPositionColorNormal left, VertexPositionColorNormal right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VertexPositionColorNormal left, VertexPositionColorNormal right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return string.Format("Position: {0}, Normal: {1}", Position, Normal);
        }

    }

}
