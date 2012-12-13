using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing.Primitives;

namespace factor10.VisionThing.Objects
{
    public class Box : ClipDrawable
    {
        public Matrix World;

        private readonly CubePrimitive<VertexPositionNormalTexture> _cube;
        private readonly Texture2D _texture;
        private readonly Texture2D _bumpMap;

        public Box(Matrix world, Vector3 size, float texScale = 1)
            : base(VisionContent.LoadPlainEffect("effects/SimpleBumpEffect"))
        {
            _texture = VisionContent.Load<Texture2D>("textures/brick_texture_map");
            _bumpMap = VisionContent.Load<Texture2D>("textures/brick_normal_map");
            _cube = new CubePrimitive<VertexPositionNormalTexture>(
                Effect.GraphicsDevice,
                (p,n,t) => createVertex(p,n,t,size,texScale),
                1);
            World = world;
        }

        private VertexPositionNormalTexture createVertex(Vector3 position, Vector3 normal, Vector2 textureCoordinate, Vector3 size, float texScale)
        {
            if ( normal.X != 0 )
                textureCoordinate *= new Vector2(size.Z, size.Y);
            else if (normal.Y != 0)
                textureCoordinate *= new Vector2(size.X, size.Z);
            else if (normal.Z != 0)
                textureCoordinate *= new Vector2(size.Y, size.X);
            return new VertexPositionNormalTexture(
                position * size,
                normal,
                textureCoordinate * texScale);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            camera.UpdateEffect(Effect);
            Effect.World = World;
            if (drawingReason != DrawingReason.ShadowDepthMap)
            {
                Effect.Texture = _texture;
                Effect.Parameters["BumpMap"].SetValue(_bumpMap);
            }
            _cube.Draw(Effect);
            return true;
        }

    }

}