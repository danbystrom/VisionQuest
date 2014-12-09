using System.Collections.Generic;
using System.Linq;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionQuest
{
    public class BannerSign : ClipDrawable
    {
        private class TextAndPosAndDistance
        {
            public string Text;
            public Vector3 Pos;
            public float DistanceSquared;
            public float DotProduct;
            public BoundingBox BoundingBox;
        }

        private readonly SpriteFont _spriteFont;
        private readonly SpriteBatch _spriteBatch;
        private readonly List<TextAndPosAndDistance> _tpds;

        private int _lastDistanceUpdateSeconds;

        public const float TextSize = 0.8f;
        public const float HeightAboveOcean = 50;
 
        public BannerSign(VisionContent vContent, IEnumerable<CodeIsland> islands)
            : base(vContent.LoadEffect("effects/signtexteffect"))
        {
            _spriteBatch = new SpriteBatch(Effect.GraphicsDevice);
            _spriteFont = vContent.Load<SpriteFont>("fonts/BlackCastle");
            _tpds = islands.Select(_ =>
                new TextAndPosAndDistance
                {
                    Text = _.VAssembly.Name,
                    Pos = new Vector3(_.World.M41 + (float) _.GroundExtentX/2, HeightAboveOcean, _.World.M43 + (float) _.GroundExtentZ/2)
                }).ToList();

            foreach (var tpd in _tpds)
            {
                var sz = _spriteFont.MeasureString(tpd.Text)/4*TextSize;
                var extent = new Vector3(sz.X, sz.Y, sz.X);
                tpd.BoundingBox = new BoundingBox(tpd.Pos - extent, tpd.Pos + extent);
            }
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
            var totalSeconds = gameTime.TotalGameTime.Seconds;
            if (totalSeconds != _lastDistanceUpdateSeconds)
            {
                // sort once a second
                foreach (var tpd in _tpds)
                {
                    var viewDirection = tpd.Pos - camera.Position;
                    tpd.DistanceSquared = viewDirection.LengthSquared();
                    tpd.DotProduct = Vector3.Dot(viewDirection, camera.Front);
                }
                _tpds.Sort((x, y) => (int) (y.DistanceSquared - x.DistanceSquared));
                _lastDistanceUpdateSeconds = totalSeconds;
            }
            base.Update(camera, gameTime);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            if (drawingReason != DrawingReason.Normal)
                return true;

            var ray = camera.GetPickingRay();

            foreach (var tpd in _tpds)
            {
                if (tpd.DistanceSquared < 5000 || tpd.DotProduct < 0)  // too close or facing away
                    continue;

                Effect.World = Matrix.BillboardRH(tpd.Pos, camera.Position, -camera.Up, camera.Front);
                Effect.DiffuseColor = tpd.BoundingBox.Intersects(ref ray)
                    ? Color.Yellow.ToVector4()
                    : Color.White.ToVector4();
                _spriteBatch.Begin(SpriteSortMode.Deferred, Effect.GraphicsDevice.BlendStates.NonPremultiplied, null, Effect.GraphicsDevice.DepthStencilStates.DepthRead, null, Effect.Effect);
                _spriteBatch.DrawString(_spriteFont, tpd.Text, Vector2.Zero, Color.Black, 0, _spriteFont.MeasureString(tpd.Text) / 2, TextSize, 0, 0);
                _spriteBatch.End();
            }

            Effect.GraphicsDevice.SetDepthStencilState(Effect.GraphicsDevice.DepthStencilStates.Default);
            Effect.GraphicsDevice.SetBlendState(Effect.GraphicsDevice.BlendStates.Opaque);

            return true;
        }

        public void PlayAround(IVEffect effect, IVDrawable sphere)
        {
            //foreach (var tpd in _tpds)
            //{
            //    effect.World = Matrix.Scaling(tpd.BoundingSphere.Radius)*Matrix.Translation(tpd.BoundingSphere.Center);
            //    sphere.Draw(effect);
            //}
        }
    }

}
