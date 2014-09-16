using System.Collections.Generic;
using System.Linq;
using factor10.VisionThing;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionQuest
{
    public class SignsBig : ClipDrawable
    {
        private struct TextAndPos
        {
            public string Text;
            public Vector3 Pos;
        }

        private readonly SpriteFont _spriteFont;
        private readonly SpriteBatch _spriteBatch;
        private readonly List<TextAndPos> _texts;

        public readonly float TextSize = 0.8f;

        public SignsBig(VisionContent vContent, IEnumerable<CodeIsland> islands)
            : base(vContent.LoadPlainEffect("effects/signtexteffect"))
        {
            _spriteBatch = new SpriteBatch(Effect.GraphicsDevice);
            _spriteFont = vContent.Load<SpriteFont>("fonts/BlackCastle");
            _texts = islands.Select(_ =>
                new TextAndPos
                {
                    Text = _.VAssembly.Name,
                    Pos = new Vector3(_.World.M41 + _.GroundExtentX/2, 40, _.World.M43 + _.GroundExtentZ/2)
                }).ToList();
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            if (drawingReason != DrawingReason.Normal)
                return true;

            //TODO foreach (var text in _texts)
            //{
            //    var viewDirection = text.Pos - camera.Position;
            //    if (viewDirection.LengthSquared() < 20000)
            //        continue;
            //    var dot = Vector3.Dot(viewDirection, camera.Front);
            //    if (dot < 0)
            //        continue;
            //    Effect.World = Matrix.CreateBillboard(text.Pos, camera.Position, -camera.UpVector, camera.Front);
            //    _spriteBatch.Begin(0, null, null, DepthStencilState.DepthRead, RasterizerState.CullNone, Effect.Effect);
            //    _spriteBatch.DrawString(_spriteFont, text.Text, Vector2.Zero, Color.White, 0, _spriteFont.MeasureString(text.Text) / 2, TextSize, 0, 0);
            //    _spriteBatch.End();
            //}

            //Effect.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            //Effect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            //Effect.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            //Effect.GraphicsDevice.BlendState = BlendState.Opaque;

            return true;
        }

    }

}
