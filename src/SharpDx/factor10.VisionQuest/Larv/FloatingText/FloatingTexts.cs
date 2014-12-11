using System.Collections.Generic;
using factor10.VisionThing;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Larv.FloatingText
{
    public class FloatingTexts : ClipDrawable
    {
        public readonly LContent LContent;
        public readonly List<FloatingTextItem> Items = new List<FloatingTextItem>();

        public FloatingTexts(LContent lContent)
            : base(lContent.LoadEffect("effects/signtexteffect"))
        {
            LContent = lContent;
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

            var sb = LContent.SpriteBatch;
            var font = LContent.Font;

            camera.UpdateEffect(Effect);
            foreach (var item in Items)
            {
                Effect.World = Matrix.BillboardRH(item.Target.Position + item.GetOffset(item), camera.Position, -camera.Up, camera.Front);
                Effect.DiffuseColor = item.GetColor(item);
                sb.Begin(SpriteSortMode.Deferred, Effect.GraphicsDevice.BlendStates.NonPremultiplied, null, Effect.GraphicsDevice.DepthStencilStates.DepthRead, null, Effect.Effect);
                sb.DrawString(font, item.Text, Vector2.Zero, Color.Black, 0, font.MeasureString(item.Text) / 2, item.GetSize(item), 0, 0);
                sb.End();
            }

            Effect.GraphicsDevice.SetDepthStencilState(Effect.GraphicsDevice.DepthStencilStates.Default);
            Effect.GraphicsDevice.SetBlendState(Effect.GraphicsDevice.BlendStates.Opaque);

            return true;
        }

    }

}
