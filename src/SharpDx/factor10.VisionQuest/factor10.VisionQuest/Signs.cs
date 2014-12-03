using System.Collections.Generic;
using System.Linq;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Terrain;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionQuest
{
    public class Signs : SimpleBillboards
    {
        private readonly VisionEffect _signTextEffect;
        private readonly SpriteFont _spriteFont;
        private readonly SpriteBatch _spriteBatch;
        private readonly List<VisionClass> _vclasses;

        public const float TextSize = 0.05f;
        public const int TextDistanceAboveGround = 5;

        public Signs(
            VisionContent vContent,
            Matrix world,
            Texture2D texture,
            List<VisionClass> vclasses,
            float width,
            float height)
            : base(vContent, world, texture, vclasses.Select(vc => vc.Position).ToList(), width, height)
        {
            _signTextEffect = vContent.LoadPlainEffect("effects/signtexteffect");
            _spriteBatch = new SpriteBatch(Effect.GraphicsDevice);
            _spriteFont = vContent.Load<SpriteFont>("fonts/BlackCastle");
            _vclasses = vclasses;

            foreach (var vc in vclasses)
            {
                vc.GroundBoundingSphere = new BoundingSphere(
                    vc.Position + world.TranslationVector + new Vector3(0, TextDistanceAboveGround/2, 0),
                    vc.R);
                vc.SignClickBoundingSphere = new BoundingSphere(
                    vc.Position + world.TranslationVector + new Vector3(0, TextDistanceAboveGround - 1, 0),
                    2);
            }
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            base.draw(camera, drawingReason, shadowMap);

            if (drawingReason != DrawingReason.Normal)
                return true;

            camera.UpdateEffect(_signTextEffect);
            var world = World*Matrix.Translation(0, TextDistanceAboveGround, 0);
            _signTextEffect.DiffuseColor = Color.WhiteSmoke.ToVector4();

            foreach (var vc in _vclasses)
            {
                var pos = Vector3.TransformCoordinate(vc.Position, world);
                var viewDirection = Vector3.Normalize(pos - camera.Position);

                if (Vector3.DistanceSquared(pos, camera.Position) > 200000 || Vector3.Dot(viewDirection, camera.Front) < 0)
                    continue;

                var text = vc.VClass.Name;
                _signTextEffect.World = createConstrainedBillboard(pos - viewDirection*0.2f, viewDirection, Vector3.Down);
                _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, Effect.GraphicsDevice.DepthStencilStates.DepthRead, null, _signTextEffect.Effect);
                _spriteBatch.DrawString(_spriteFont, text, Vector2.Zero, Color.Black, 0, _spriteFont.MeasureString(text)/2, TextSize, 0, 0);
                _spriteBatch.End();
            }

            Effect.GraphicsDevice.SetDepthStencilState(Effect.GraphicsDevice.DepthStencilStates.Default);
            Effect.GraphicsDevice.SetBlendState(Effect.GraphicsDevice.BlendStates.Opaque);

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
