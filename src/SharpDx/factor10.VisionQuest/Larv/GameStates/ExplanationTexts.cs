using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using factor10.VisionThing;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Larv.GameStates
{
    public class ExplanationTexts : ClipDrawable
    {
        public const float TextSize = 0.025f;

        public class DrawingInfo
        {
            public string Text1;
            public string Text2;
            public Vector4 DiffuseColor;
        }

        public class Item
        {
            public IPosition Target;
            public float Age;
            public float TimeToLive;
            public Func<Item, DrawingInfo> GetDrawingInfo;
        }

        public readonly List<Item> Items = new List<Item>();

        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _spriteFont;

        public ExplanationTexts(VisionContent vContent)
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
                var drawingInfo = item.GetDrawingInfo(item);
                Effect.World = Matrix.BillboardLH(item.Target.Position - new Vector3(0,-1.5f,0), camera.Position, -camera.Up, camera.Front);
                Effect.DiffuseColor = drawingInfo.DiffuseColor;
                _spriteBatch.Begin(SpriteSortMode.Deferred, Effect.GraphicsDevice.BlendStates.NonPremultiplied, null, Effect.GraphicsDevice.DepthStencilStates.DepthRead, null, Effect.Effect);
                _spriteBatch.DrawString(_spriteFont, drawingInfo.Text1, Vector2.Zero, Color.Black, 0, _spriteFont.MeasureString(drawingInfo.Text1) / 2, TextSize, 0, 0);
                //_spriteBatch.DrawString(_spriteFont, drawingInfo.Text2, Vector2.One, Color.Black, 0, _spriteFont.MeasureString(drawingInfo.Text2) / 2, TextSize, 0, 0);
                _spriteBatch.End();
            }

            Effect.GraphicsDevice.SetDepthStencilState(Effect.GraphicsDevice.DepthStencilStates.Default);
            Effect.GraphicsDevice.SetBlendState(Effect.GraphicsDevice.BlendStates.Opaque);

            return true;
        }

    }

}
