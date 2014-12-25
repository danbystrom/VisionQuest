using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using factor10.VisionThing;
using factor10.VisionThing.CameraStuff;
using Larv.Hof;
using Larv.Serpent;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Larv.GameStates
{
    class AttractBigTexts
    {
        private float _scrollingTextAngle;
        private float _larvText;
        private readonly Hof.PaintHof _hofPainter;

        private LContent _lcontent;

        private Random _random = new Random();

        public AttractBigTexts(LContent lcontent)
        {
            _lcontent = lcontent;
            _hofPainter = new PaintHof(lcontent, new HallOfFame());
        }

        public void Update(GameTime gameTime)
        {
            _larvText += (float) gameTime.ElapsedGameTime.TotalSeconds;
            _scrollingTextAngle = MathUtil.Mod2PI(_scrollingTextAngle + 1.2f*(float) gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void Draw()
        {
            var gd = _lcontent.GraphicsDevice;
            var sb = _lcontent.SpriteBatch;
            var font = _lcontent.Font;

            var text = "LARV!";
            var fsize = _lcontent.FontScaleRatio * 15;
            var ssize = new Vector2(gd.BackBuffer.Width, gd.BackBuffer.Height);

            var factor = 0f;
            switch ((int)_larvText)
            {
                case 1:
                case 10:
                    factor = _larvText - 1;
                    break;
                case 2:
                case 12:
                    factor = 1;
                    break;
                case 3:
                case 11:
                    factor = 4 - _larvText;
                    break;
                case 20:
                    _larvText = 0;
                    break;
            }
            sb.Begin(SpriteSortMode.Deferred, gd.BlendStates.NonPremultiplied);

            if (factor > 0)
            {
                var color = new Color(_random.NextFloat(factor, 1), _random.NextFloat(factor, 1), _random.NextFloat(factor, 1), factor) * Color.LightYellow;
                if (_larvText < 10)
                    sb.DrawString(font, text, (ssize - fsize * font.MeasureString(text)) / 2, color, 0, Vector2.Zero, fsize, SpriteEffects.None, 0);
                else
                    _hofPainter.Paint(color);
            }

            fsize *= 0.1f;
            text = "HIT [SPACE] TO PLAY";
            var measureString = fsize * font.MeasureString(text);
            var halfWidth = (ssize.X - measureString.X) / 2;
            var pos = new Vector2(halfWidth + (float)Math.Sin(_scrollingTextAngle) * halfWidth, ssize.Y - measureString.Y);
            sb.DrawString(font, text, pos, Color.LightYellow, 0, Vector2.Zero, fsize, SpriteEffects.None, 0);

            sb.End();

            gd.SetDepthStencilState(gd.DepthStencilStates.Default);
            gd.SetBlendState(gd.BlendStates.Opaque);
        }

    }
}
