using factor10.VisionThing;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;
using Larv.Field;
using Larv.Hof;
using SharpDX;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;
using System;

namespace Larv
{
    public class LarvContent : VisionContent, IDisposable
    {
        public readonly SpriteBatch SpriteBatch;
        public readonly SpriteFont Font;
        public readonly VisionEffect SignTextEffect;
        public readonly SkySphere Sky;
        public readonly Ground Ground;
        public readonly IVDrawable Sphere;
        public readonly ShadowMap ShadowMap;
        public readonly HallOfFame HallOfFame;

        public LarvContent(GraphicsDevice graphicsDevice, ContentManager content)
            : base(graphicsDevice, content)
        {
            SpriteBatch = new SpriteBatch(graphicsDevice);
            Font = Load<SpriteFont>("fonts/BlackCastle");
            SignTextEffect = LoadEffect("effects/signtexteffect");
            Sphere = new SpherePrimitive<VertexPositionNormalTangentTexture>(GraphicsDevice,
                (p, n, t, tx) => new VertexPositionNormalTangentTexture(p, n, t, tx), 2);
            Sky = new SkySphere(this, Load<TextureCube>(@"Textures\clouds"));
            Ground = new Ground(this);
            ShadowMap = new ShadowMap(this, 800, 800, 1, 50);
            ShadowMap.UpdateProjection(50, 30);
            HallOfFame = new HallOfFame();
        }

        public float FontScaleRatio
        {
            get { return GraphicsDevice.BackBuffer.Width/1920f; }
        }

        public void Dispose()
        {
            SpriteBatch.Dispose();
            Font.Dispose();
            SignTextEffect.Dispose();
            Sphere.Dispose();
            Ground.Dispose();
            Sky.Dispose();
            ShadowMap.Dispose();
        }

        private class EndSpriteBatch : IDisposable
        {
            public SpriteBatch SpriteBatch;
            public void Dispose()
            {
                SpriteBatch.End();
            }
        }

        public IDisposable UsingSpriteBatch()
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, GraphicsDevice.BlendStates.NonPremultiplied);
            return new EndSpriteBatch {SpriteBatch = SpriteBatch};
        }

        public void DrawString(string text, Vector2 pos, float size, float align = 0, Color? color = null)
        {
            SpriteBatch.DrawString(
                Font,
                text,
                pos,
                color.GetValueOrDefault(Color.LightYellow), 0,
                new Vector2(Font.MeasureString(text).X*align, 0),
                size,
                SpriteEffects.None, 0);
        }

    }

}
