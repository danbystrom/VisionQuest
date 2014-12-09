using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;

namespace Larv
{
    public class LContent : VisionContent, IDisposable
    {
        public readonly SpriteBatch SpriteBatch;
        public readonly SpriteFont Font;
        public readonly VisionEffect SignTextEffect;

        public LContent(GraphicsDevice graphicsDevice, ContentManager content)
            : base(graphicsDevice, content)
        {
            SpriteBatch = new SpriteBatch(graphicsDevice);
            Font = Load<SpriteFont>("fonts/BlackCastle");
            SignTextEffect = LoadEffect("effects/signtexteffect");
        }

        public void Dispose()
        {
            SpriteBatch.Dispose();
            Font.Dispose();
            SignTextEffect.Dispose();
        }

    }

}
