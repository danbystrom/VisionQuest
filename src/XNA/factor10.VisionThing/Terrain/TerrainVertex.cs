using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factor10.VisionThing.Terrain
{
    public struct TerrainVertex : IVertexType
    {
        public Vector3 Position;
        public Vector2 TexCoord;
        public float NormCoordX;

        public static readonly int SizeInBytes = 4*(3+2+1);

        public static readonly VertexElement[] VertexElements =
        {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(20, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 1)
        };

        public static VertexDeclaration Declaration
        {
            get { return new VertexDeclaration(VertexElements); }
        }

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return new VertexDeclaration(VertexElements); }
        }

        public TerrainVertex( Vector3 positiom, Vector2 texCoord, float normCoordX)
        {
            Position = positiom;
            TexCoord = texCoord;
            NormCoordX = normCoordX;
        }
    }
}
