﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Terrain;

namespace TestBed
{
    public class Signs : SimpleBillboards
    {
        private readonly PlainEffectWrapper _signTextEffect;
        private readonly SpriteFont _spriteFont;
        private readonly SpriteBatch _spriteBatch;
        private readonly List<VisualClass> _vclasses;

        public readonly float TextSize = 0.05f;

        public Signs(
            Matrix world,
            Texture2D texture,
            List<VisualClass> vclasses,
            float width,
            float height)
            : base(world, texture, vclasses.Select(vc => vc.Position).ToList(), width, height)
        {
            _signTextEffect = VisionContent.LoadPlainEffect("effects/signtexteffect");
            _spriteBatch = new SpriteBatch(Effect.GraphicsDevice);
            _spriteFont = VisionContent.Load<SpriteFont>("fonts/BlackCastle");
            _vclasses = vclasses;
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            base.draw(camera, drawingReason, shadowMap);

            if (drawingReason != DrawingReason.Normal)
                return true;

            camera.UpdateEffect(_signTextEffect);
            var world = World*Matrix.CreateTranslation(0, 5f, 0);

            foreach (var vc in _vclasses)
            {
                var text = vc.VClass.TypeDefinition.Name;
                var pos = Vector3.Transform(vc.Position, world);

                var viewDirection = Vector3.Normalize(pos - camera.Position);
                _signTextEffect.World = createConstrainedBillboard(pos - viewDirection*0.2f, viewDirection, Vector3.Down);
                _spriteBatch.Begin(0, null, null, DepthStencilState.DepthRead, RasterizerState.CullNone, _signTextEffect.Effect);
                _spriteBatch.DrawString(_spriteFont, text, Vector2.Zero, Color.White, 0, _spriteFont.MeasureString(text) / 2, TextSize, 0, 0);
                _spriteBatch.End();
            }

            Effect.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            Effect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Effect.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            Effect.GraphicsDevice.BlendState = BlendState.Opaque;

            return true;
        }

        private static Matrix createConstrainedBillboard(Vector3 objectPosition, Vector3 viewDirection, Vector3 rotateAxis)
        {
            var vec1 = Vector3.Normalize(Vector3.Cross(rotateAxis, viewDirection));
            var vec2 = Vector3.Normalize(Vector3.Cross(vec1, rotateAxis));
            
            Matrix matrix;
            matrix.M11 = vec1.X;
            matrix.M12 = vec1.Y;
            matrix.M13 = vec1.Z;
            matrix.M14 = 0.0f;
            matrix.M21 = rotateAxis.X;
            matrix.M22 = rotateAxis.Y;
            matrix.M23 = rotateAxis.Z;
            matrix.M24 = 0.0f;
            matrix.M31 = vec2.X;
            matrix.M32 = vec2.Y;
            matrix.M33 = vec2.Z;
            matrix.M34 = 0.0f;
            matrix.M41 = objectPosition.X;
            matrix.M42 = objectPosition.Y;
            matrix.M43 = objectPosition.Z;
            matrix.M44 = 1f;
            return matrix;
        }

    }

}