using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factor10.VisionThing.Terrain
{
    public struct BillboardVertex : IVertexType
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TexCoord;
        public Vector2 Random;

        public static readonly int SizeInBytes = sizeof(float)*(3+3+2+2);

        public static readonly VertexElement[] VertexElements =
        {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(32, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 1)
        };

        public static VertexDeclaration Declaration
        {
            get { return new VertexDeclaration(VertexElements); }
        }

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return new VertexDeclaration(VertexElements); }
        }

        public BillboardVertex(Vector3 position, Vector3 normal, Vector2 texCoord, float random)
        {
            normal.Normalize();
            Position = position;
            Normal = normal;
            TexCoord = texCoord;
            Random = new Vector2(random,0);
        }

    }
}
