using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factor10.VisionThing
{
    public struct ArcVertex : IVertexType
    {
        public Vector3 Position;
        public Color Color;
        public float A;

        public static readonly int SizeInBytes = sizeof (float)*(3 + 1) + 4;

        public static readonly VertexElement[] VertexElements =
        {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0),
            new VertexElement(16, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 0),
        };

        public static VertexDeclaration Declaration
        {
            get { return new VertexDeclaration(VertexElements); }
        }

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return new VertexDeclaration(VertexElements); }
        }

        public ArcVertex(Vector3 position, Color color, float a)
        {
            Position = position;
            Color = color;
            A = a;
        }

    }
}
