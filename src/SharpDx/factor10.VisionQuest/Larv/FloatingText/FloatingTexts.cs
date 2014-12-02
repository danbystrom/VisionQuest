using System.Collections.Generic;
using factor10.VisionThing;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Larv.FloatingText
{
    public class FloatingTexts : ClipDrawable
    {
        public readonly List<FloatingTextItem> Items = new List<FloatingTextItem>();

        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _spriteFont;

        public FloatingTexts(VisionContent vContent)
            : base(vContent.LoadPlainEffect("effects/signtexteffect"))
        {
            _spriteBatch = new SpriteBatch(vContent.GraphicsDevice);
            _spriteFont = vContent.Load<SpriteFont>("fonts/BlackCastle");
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
            base.Update(camera, gameTime);
            foreach (var item in Items)
                item.Age += (float) gameTime.ElapsedGameTime.TotalSeconds;
            Items.RemoveAll(_ => _.Age > _.TimeToLive);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            if (drawingReason != DrawingReason.Normal)
                return true;

            camera.UpdateEffect(Effect);
            foreach (var item in Items)
            {
                Effect.World = Matrix.BillboardLH(item.Target.Position + item.GetOffset(item), camera.Position, -camera.Up, camera.Front);
                Effect.DiffuseColor = item.GetColor(item);
                _spriteBatch.Begin(SpriteSortMode.Deferred, Effect.GraphicsDevice.BlendStates.NonPremultiplied, null, Effect.GraphicsDevice.DepthStencilStates.DepthRead, null, Effect.Effect);
                _spriteBatch.DrawString(_spriteFont, item.Text, Vector2.Zero, Color.Black, 0, _spriteFont.MeasureString(item.Text)/2, item.GetSize(item), 0, 0);
                _spriteBatch.End();
            }

            Effect.GraphicsDevice.SetDepthStencilState(Effect.GraphicsDevice.DepthStencilStates.Default);
            Effect.GraphicsDevice.SetBlendState(Effect.GraphicsDevice.BlendStates.Opaque);

            return true;
        }

    }

}
