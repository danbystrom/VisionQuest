using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using factor10.VisionThing;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionQuest
{
    public class SignsBig : ClipDrawable
    {
        private class TextAndPosAndDistance
        {
            public string Text;
            public Vector3 Pos;
            public float DistanceSquared;
            public float DotProduct;
        }

        private readonly SpriteFont _spriteFont;
        private readonly SpriteBatch _spriteBatch;
        private readonly List<TextAndPosAndDistance> _texts;

        private int _lastDistanceUpdateSeconds;

        public readonly float TextSize = 0.8f;

        public SignsBig(VisionContent vContent, IEnumerable<CodeIsland> islands)
            : base(vContent.LoadPlainEffect("effects/signtexteffect"))
        {
            _spriteBatch = new SpriteBatch(Effect.GraphicsDevice);
            _spriteFont = vContent.Load<SpriteFont>("fonts/BlackCastle");
            _texts = islands.Select(_ =>
                new TextAndPosAndDistance
                {
                    Text = _.VAssembly.Name,
                    Pos = new Vector3(_.World.M41 + (float)_.GroundExtentX/2, 40, _.World.M43 + (float)_.GroundExtentZ/2)
                }).ToList();
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
            var totalSeconds = gameTime.TotalGameTime.Seconds;
            if (totalSeconds != _lastDistanceUpdateSeconds)
            {
                foreach (var text in _texts)
                {
                    var viewDirection = text.Pos - camera.Position;
                    text.DistanceSquared = viewDirection.LengthSquared();
                    text.DotProduct = Vector3.Dot(viewDirection, camera.Front);
                }
                _texts.Sort((x, y) => (int) (y.DistanceSquared - x.DistanceSquared));
                _lastDistanceUpdateSeconds = totalSeconds;
            }
            base.Update(camera, gameTime);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            if (drawingReason != DrawingReason.Normal)
                return true;

            foreach (var text in _texts)
            {
                if (text.DistanceSquared < 10000 || text.DotProduct < 0)
                    continue;

                Effect.World = Matrix.BillboardLH(text.Pos, camera.Position, -camera.Up, camera.Front);
                _spriteBatch.Begin(SpriteSortMode.Deferred, Effect.GraphicsDevice.BlendStates.NonPremultiplied, null, Effect.GraphicsDevice.DepthStencilStates.DepthRead, null, Effect.Effect);
                _spriteBatch.DrawString(_spriteFont, text.Text, Vector2.Zero, Color.Brown, 0, _spriteFont.MeasureString(text.Text) / 2, TextSize, 0, 0);
                _spriteBatch.End();
            }

            Effect.GraphicsDevice.SetDepthStencilState(Effect.GraphicsDevice.DepthStencilStates.Default);
            Effect.GraphicsDevice.SetBlendState(Effect.GraphicsDevice.BlendStates.Opaque);

            return true;
        }

    }

}
