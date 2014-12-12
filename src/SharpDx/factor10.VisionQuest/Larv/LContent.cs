using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;
using Larv.Field;
using Larv.Serpent;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;

namespace Larv
{
    public class LContent : VisionContent, IDisposable
    {
        public readonly SpriteBatch SpriteBatch;
        public readonly SpriteFont Font;
        public readonly VisionEffect SignTextEffect;
        public readonly SkySphere Sky;
        public readonly Ground Ground;
        public readonly IVDrawable Sphere;
        public readonly ShadowMap ShadowMap;

        public LContent(GraphicsDevice graphicsDevice, ContentManager content)
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
        }

    }

}
