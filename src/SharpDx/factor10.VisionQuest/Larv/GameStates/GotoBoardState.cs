using System.Linq;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
using Larv.FloatingText;
using Larv.Serpent;
using Larv.Util;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Larv.GameStates
{
    internal class GotoBoardState : IGameState
    {
        private readonly Serpents _serpents;
        private readonly int _scene;
        private readonly SpriteBatch _spriteBatch;
        private readonly IVEffect _signEffect;
        private readonly Vector3 _signPosition;
        private readonly SpriteFont _spriteFont;

        private readonly SequentialToDoQue _actions = new SequentialToDoQue();

        public GotoBoardState(Serpents serpents, int scene)
        {
            _serpents = serpents;
            _scene = scene;
            _spriteBatch = new SpriteBatch(_serpents.VContent.GraphicsDevice);
            _signEffect = _serpents.VContent.LoadPlainEffect("effects/signtexteffect");
            _spriteFont = _serpents.VContent.Load<SpriteFont>("fonts/BlackCastle");

            HomingDevice.Attach(serpents);

            _signPosition = Data.Ground.SignPosition + Vector3.Up*1.5f;
            var direction = Vector3.Left;
            var toCameraPosition = _signPosition + direction*2.3f;
            var cp = serpents.Camera.Position;

            var distanceToCamera = Vector3.Distance(cp, toCameraPosition);
            var straightLineLength = distanceToCamera/3;
            var arcEndPoint = toCameraPosition + direction*straightLineLength;

            var x = new ArcGenerator(4);
            x.CreateArc(
                cp,
                arcEndPoint,
                direction,
                SerpentCamera.CameraDistanceToHeadXz);
            var points = x.Points.ToList();

            var segmentLength = Vector3.Distance(x.Points[0], x.Points[1]);
            var missingPoints = 1 + (int) (straightLineLength/segmentLength);
            for (var i = 1; i <= missingPoints; i++)
                points.Add(Vector3.Lerp(arcEndPoint, toCameraPosition, i/(float) missingPoints));

            var moveCamera1 = new MoveCamera(_serpents.Camera, 8f.UnitsPerSecond(), _signPosition, points.ToArray());

            _actions.Add(moveCamera1.Move);
            _actions.Add(2);
            _actions.Add(() =>
            {
                _serpents.Restart(_scene);
                HomingDevice.Attach(_serpents);
            });

            _actions.Add(() =>
            {
                Vector3 toPosition, toLookAt;
                _serpents.PlayingField.GetCameraPositionForLookingAtPlayerCave(out toPosition, out toLookAt);
                var moveCamera2 = new MoveCamera(
                    _serpents.Camera,
                    3f.Time(),
                    toLookAt,
                    toPosition);
                _actions.Add(moveCamera2.Move);
            });
        }

        public void Update(Camera camera, GameTime gameTime, ref IGameState gameState)
        {
            _serpents.Update(camera, gameTime);

            if (_actions.Do(gameTime))
                return;

            gameState = new StartSerpentState(_serpents);
        }

        public void Draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            _serpents.Draw(camera, drawingReason, shadowMap);

            camera.UpdateEffect(_signEffect);

            _signEffect.World = Matrix.BillboardLH(_signPosition + Vector3.Left * 0.1f, _signPosition + Vector3.Left, -camera.Up, Vector3.Right);
            _signEffect.DiffuseColor = new Vector4(0.1f, 0.5f, 0.3f, 1);
            _spriteBatch.Begin(SpriteSortMode.Deferred, _signEffect.GraphicsDevice.BlendStates.NonPremultiplied, null, _signEffect.GraphicsDevice.DepthStencilStates.DepthRead, null, _signEffect.Effect);
            _spriteBatch.DrawString(_spriteFont, "Entering scene 1", Vector2.Zero, Color.Black, 0, _spriteFont.MeasureString("Entering scene 1") / 2, 0.015f, 0, 0);
            _spriteBatch.End();
        }

    }

}
