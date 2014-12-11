using System;
using factor10.VisionThing;
using Larv.Serpent;
using Larv.Util;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Larv.GameStates
{
    class GameOverState : IGameState
    {
        private readonly Serpents _serpents;
        private readonly SequentialToDoQue _todo  = new SequentialToDoQue();

        private readonly Random _random = new Random();

        public GameOverState(Serpents serpents)
        {
            _serpents = serpents;
            _todo.AddMoveable(AttractState.GetOverviewMoveCamera(_serpents.Camera));
            _todo.Add(2);
        }

        public void Update(Camera camera, GameTime gameTime, ref IGameState gameState)
        {
            _serpents.Update(camera, gameTime);
            if (_todo.Do(gameTime))
                return;
            gameState = new AttractState(_serpents);
        }

        public void Draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            _serpents.Draw(camera, drawingReason, shadowMap);

            var gd = _serpents.LContent.GraphicsDevice;
            var sb = _serpents.LContent.SpriteBatch;
            var font = _serpents.LContent.Font;

            const string text = "GAME OVER";
            const float fsize = 10;
            var ssize = new Vector2(gd.BackBuffer.Width, gd.BackBuffer.Height);

            var factor = 1f;
            //switch ((int)_x)
            //{
            //    case 1:
            //        factor = _x - 1;
            //        break;
            //    case 2:
            //        factor = 1;
            //        break;
            //    case 3:
            //        factor = 4 - _x;
            //        break;
            //    case 10:
            //        factor = _x = 0;
            //        break;
            //}
            var color = new Color(_random.NextFloat(factor, 1), _random.NextFloat(factor, 1), _random.NextFloat(factor, 1), factor) * Color.LightYellow;
            sb.Begin(SpriteSortMode.Deferred, gd.BlendStates.NonPremultiplied);
            sb.DrawString(font, text, (ssize - fsize * font.MeasureString(text)) / 2, color, 0, Vector2.Zero, fsize, SpriteEffects.None, 0);
            sb.End();

            gd.SetDepthStencilState(gd.DepthStencilStates.Default);
            gd.SetBlendState(gd.BlendStates.Opaque);

        }

    }

}
