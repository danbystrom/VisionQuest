﻿using System.Linq;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
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
        private readonly IVEffect _signEffect;
        private readonly Vector3 _signPosition;

        private readonly SequentialToDo _actions = new SequentialToDo();

        public GotoBoardState(Serpents serpents, int scene)
        {
            _serpents = serpents;
            _scene = scene;
            _signEffect = _serpents.LContent.LoadEffect("effects/signtexteffect");

            HomingDevice.Attach(serpents);
            _serpents.PlayerCave.OpenDoor = true;

            _signPosition = _serpents.LContent.Ground.SignPosition + Vector3.Up*1.5f;
            var direction = Vector3.Left;
            var toCameraPosition = _signPosition + direction*2.4f;
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


#if !DEBUG
            const float time1 = 10f;
            const float time2 = 0.25f;
#else
            var time1 = 2.5f;
            var time2 = 2f;
#endif
            // move to the board in an arc
            _actions.AddDurable(() => new MoveCameraArc(_serpents.Camera, time1.UnitsPerSecond(), toCameraPosition, Vector3.Right, 5));

            // look at the board for two seconds, while resetting the playing field
            _actions.AddOneShot(time2, () =>
            {
                _serpents.Restart(_scene);
                HomingDevice.Attach(_serpents);
            });

            Vector3 toPosition, toLookAt;
            _serpents.PlayingField.GetCameraPositionForLookingAtPlayerCave(out toPosition, out toLookAt);

            // turn around the camera to look at the cave 
            _actions.AddDurable(
                () => new MoveCameraYaw(_serpents.Camera, 2f.Time(), toPosition, StartSerpentState.GetPlayerInitialLookAt(_serpents.PlayingField)));
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

            var sb = _serpents.LContent.SpriteBatch;
            var font = _serpents.LContent.Font;
            var text = string.Format("Entering scene {0}", 1 + _scene);
            _signEffect.World = Matrix.BillboardRH(_signPosition + Vector3.Left * 0.1f, _signPosition + Vector3.Left, -camera.Up, Vector3.Right);
            _signEffect.DiffuseColor = new Vector4(0.5f, 0.4f, 0.3f, 1);
            sb.Begin(SpriteSortMode.Deferred, _signEffect.GraphicsDevice.BlendStates.NonPremultiplied, null, _signEffect.GraphicsDevice.DepthStencilStates.DepthRead, null, _signEffect.Effect);
            sb.DrawString(font, text, Vector2.Zero, Color.Black, 0, font.MeasureString(text) / 2, 0.010f, 0, 0);
            sb.End();
        }

    }

}
