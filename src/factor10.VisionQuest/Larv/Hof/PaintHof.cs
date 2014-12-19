using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace Larv.Hof
{
    public class PaintHof
    {
        public readonly LContent LContent;
        public readonly HallOfFame HallOfFame;

        public PaintHof(LContent lcontent, HallOfFame hallOfFame)
        {
            LContent = lcontent;
            HallOfFame = hallOfFame;
        }

        public void Paint(Color color)
        {
            //LContent.SpriteBatch.DrawString(font, text, (ssize - fsize * font.MeasureString(text)) / 2, color, 0, Vector2.Zero, fsize, SpriteEffects.None, 0);
        }

    }

}
