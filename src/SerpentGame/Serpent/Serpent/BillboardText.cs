using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Serpent.Util;
using factor10.VisionThing;

namespace Serpent
{
    public class BillboardText
    {
        private readonly GraphicsDevice _graphicsDevice;

        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _spriteFont;
        private readonly BasicEffect _basicEffect;

        public BillboardText(
            GraphicsDevice graphicsDevice,
            SpriteFont font)
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _spriteFont = font;

            _basicEffect = new BasicEffect(graphicsDevice)
                               {
                                   TextureEnabled = true,
                                   VertexColorEnabled = true,
                               };
        }

        public void Draw( Camera camera, Vector3 textPosition, Vector3 zzz )
        {
            _basicEffect.World = Matrix.CreateConstrainedBillboard(textPosition, textPosition - camera.Front, Vector3.Down, null, null);
            _basicEffect.View = camera.View;
            _basicEffect.Projection = camera.Projection;

            const string message = "Ellen";
            var textOrigin = _spriteFont.MeasureString(message) / 2;
            const float textSize = 0.025f;

            _basicEffect.TextureEnabled = true;
            _basicEffect.VertexColorEnabled = false;
            _basicEffect.CurrentTechnique.Passes[0].Apply();
            _spriteBatch.Begin(0, null, null, DepthStencilState.DepthRead, RasterizerState.CullNone, _basicEffect);
            _spriteBatch.DrawString(_spriteFont, message, Vector2.Zero, Color.White, 0, textOrigin, textSize, 0, 0);
            _spriteBatch.End();

            _graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            _graphicsDevice.DepthStencilState = DepthStencilState.Default;
            _graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

            _basicEffect.World = Matrix.Identity;
            _basicEffect.TextureEnabled = false;
            _basicEffect.VertexColorEnabled = true;
            _basicEffect.CurrentTechnique.Passes[0].Apply();
            drawArc(textPosition, zzz);

        }

        private void drawArc( Vector3 point1, Vector3 point2 )
        {
            var arc = new ArcGenerator(4);
            arc.CreateArc(point1, point2, Vector3.Up, 2);
            var vpcs = new VertexPositionColor[arc.Points.Length*2 - 2];
            var i = 0;
            var max = vpcs.Length - 1;
            foreach (var t in arc.Points)
            {
                var vx = Vector3.Lerp(Color.White.ToVector3(), Color.Black.ToVector3(), (float) i/max);
                var vpc = new VertexPositionColor(t, new Color(vx));
                if (i != 0 && i != max)
                    vpcs[i++] = vpc;
                vpcs[i++] = vpc;
            }
            _graphicsDevice.DrawUserPrimitives(
                PrimitiveType.LineList,
                vpcs,
                0, arc.Points.Length - 1);
        }

    }

}
