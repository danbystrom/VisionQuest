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
        private readonly CubePrimitive<VertexPositionNormalTexture> _plane;
        private readonly Matrix _world;
        private readonly Texture2D _texture;
        private readonly Texture2D _bumpMap;

        public Box(Matrix world)
            : base(VisionContent.LoadPlainEffect("effects/lightingeffectbump"))
        {
            //Effect.Parameters["viewportWidth"].SetValue(Effect.GraphicsDevice.Viewport.Width);
            //Effect.Parameters["viewportHeight"].SetValue(Effect.GraphicsDevice.Viewport.Height);

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

        protected override void draw(Camera camera, DrawingReason drawingReason, IEffect effect, ShadowMap shadowMap)
        {
            camera.UpdateEffect(effect);
            effect.World = _world;
            if (drawingReason != DrawingReason.ShadowDepthMap)
            {
                effect.SetShadowMapping(shadowMap);
                effect.Texture = _texture;
                effect.Parameters["BumpMap"].SetValue(_bumpMap);
            }
            _plane.Draw(effect);
            effect.SetShadowMapping(null);
        }

    }

}