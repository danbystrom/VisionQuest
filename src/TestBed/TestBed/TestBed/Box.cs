using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;

namespace TestBed
{
    public class Box : ClipDrawable
    {
        public const int Width = 16;
        public const int Height = 16;

        private readonly CubePrimitive<VertexPositionNormalTexture> _plane;
        private readonly Matrix _world;
        private readonly Texture2D _texture;
        private readonly Texture2D _bumpMap;

        public Box(Matrix world )
            : base(VisionContent.LoadPlainEffect("effects/lightingeffectbump"))
        {
            _texture = VisionContent.Load<Texture2D>("textures/brick_texture_map");
            _bumpMap = VisionContent.Load<Texture2D>("textures/brick_normal_map");
            _plane = new CubePrimitive<VertexPositionNormalTexture>(
                Effect.GraphicsDevice,
                createVertex,
                3);
            _world = world;
        }

        private VertexPositionNormalTexture createVertex( Vector3 position, Vector3 normal, Vector2 textureCoordinate)
        {
            return new VertexPositionNormalTexture(position, normal, textureCoordinate);
        }

        public override void Draw( Camera camera, IEffect effect )
        {
            camera.UpdateEffect(effect);
            effect.World = _world;
            effect.Parameters["Texture"].SetValue(_texture);
            effect.Parameters["BumpMap"].SetValue(_bumpMap);
            _plane.Draw(effect);
        }

    }

}