using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;

namespace TestBed
{
    public class Island : ClipDrawable
    {
        public const int Width = 16;
        public const int Height = 16;

        private readonly PlanePrimitive<VertexPositionNormalTexture> _plane;
        private readonly Matrix _world;
        private readonly Texture2D _texture;

        public Island( IEffect effect, Matrix world )
            : base(effect)
        {
            _texture = VisionContent.Load<Texture2D>("textures/sand");
            _plane = new PlanePrimitive<VertexPositionNormalTexture>(
                effect.GraphicsDevice,
                createVertex,
                Width, Height);
            _world = world;
        }

        private VertexPositionNormalTexture createVertex( float x , float y, int width, int height)
        {
            var dx = Math.Abs(x - Width/2);
            var dy = Math.Abs(y - Height/2);
            var r = (float)Math.Max(Width, Height);
            var d = (float) Math.Max(0, r - Math.Sqrt(dx*dx + dy*dy))/r;
            var h = MathHelper.SmoothStep(0, 1.5f, d);
            return new VertexPositionNormalTexture(new Vector3(x, h, y), Vector3.Up, new Vector2(x / 10, y / 10));
        }

        public override void Draw( Camera camera, IEffect effect )
        {
            camera.UpdateEffect(effect);
            effect.World = _world;
            effect.Parameters["Texture"].SetValue(_texture);
            _plane.Draw(effect);
        }

    }

}
