using System;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionThing.Terrain
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BillboardVertex : IEquatable<BillboardVertex>
    {
        public static readonly int Size = sizeof (float)*(3 + 2 + 2);

        [VertexElement("POSITION")]
        public readonly Vector3 Position;

        [VertexElement("NORMAL")]
        public readonly Vector3 Normal;

        [VertexElement("TEXCOORD0")]
        public readonly Vector2 TexCoord;

        [VertexElement("TEXCOORD1")]
        public readonly Vector2 Random;

        public BillboardVertex(Vector3 position, Vector3 normal, Vector2 texCoord, float random)
        {
            normal.Normalize();
            Position = position;
            Normal = normal;
            TexCoord = texCoord;
            Random = new Vector2(random, 0);
        }

        public bool Equals(BillboardVertex other)
        {
            return Position.Equals(other.Position) && Normal.Equals(other.Normal) && TexCoord.Equals(other.TexCoord) &&
                   Random.Equals(other.Random);
            ;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is BillboardVertex && Equals((BillboardVertex) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Position.GetHashCode();
                hashCode = (hashCode*397) ^ Normal.GetHashCode();
                hashCode = (hashCode*397) ^ TexCoord.GetHashCode();
                hashCode = (hashCode*397) ^ Random.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(BillboardVertex left, BillboardVertex right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BillboardVertex left, BillboardVertex right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return string.Format("Position: {0}, Normal: {1}, TexCoord: {2}, Random: {3}", Position, Normal,
                TexCoord, Random);
        }

    }

}
