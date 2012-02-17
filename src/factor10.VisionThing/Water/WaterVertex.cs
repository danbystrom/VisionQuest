using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factor10.VisionThing.Water
{
    public struct WaterVertex : IVertexType
    {
        public Vector3 Position;
        public Vector2 ScaledTexC;     // [a, b]
        public Vector2 NormalizedTexC; // [0, 1]

        public static readonly int SizeInBytes = 28;

        public static readonly VertexElement[] VertexElements =
        {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(20, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 1)
        };

        public static VertexDeclaration Declaration
        {
            get { return new VertexDeclaration(VertexElements); }
        }

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return new VertexDeclaration(VertexElements); }
        }

    }
}
