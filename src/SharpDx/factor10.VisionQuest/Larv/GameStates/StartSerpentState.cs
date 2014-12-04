using System;
using factor10.VisionThing;
using Larv.Serpent;
using Larv.Util;
using Serpent;
using SharpDX;
using SharpDX.Toolkit;

namespace Larv.GameStates
{
    internal class StartSerpentState : IGameState
    {
        private readonly Serpents _serpents;
        private MoveCamera _moveCamera;

        private readonly SequentialToDoQue _actions = new SequentialToDoQue();

        public StartSerpentState(Serpents serpents)
        {
            _serpents = serpents;
            _serpents.PlayerSerpent.Restart(_serpents.PlayingField, 1);

            Vector3 toPosition, toLookAt;
            _serpents.PlayingField.GetCameraPositionForLookingAtPlayerCave(out toPosition, out toLookAt);

            // här ska jag ändra så att kameran först går till skylten
            // och sedan långsamt vänder sig mot masken medan den startar upp
            var x = new ArcGenerator(4);
            x.CreateArc(
                serpents.Camera.Position,
                toPosition,
                Vector3.Right,
                SerpentCamera.CameraDistanceToHeadXz);
            _moveCamera = MoveCamera.UnitsPerSecond(
                serpents.Camera,
                5,
                toLookAt,
                x.Points);

            _actions.Add(time => _moveCamera.Move(time));
            _actions.Add(() => _moveCamera = MoveCamera.TotalTime(_serpents.Camera, 1, Data.Ground.SignPosition, _serpents.Camera.Position));
            _actions.Add(time => _moveCamera.Move(time));
        }

        public void Update(Camera camera, GameTime gameTime, ref IGameState gameState)
        {
            if (_actions.Do(gameTime))
                return;

            _serpents.PlayerSerpent.Update(_serpents.Camera, gameTime);
            // farligt - skulle det ske ett "hopp" här så skulle vi inte märka att rutan passerades...
            if (_serpents.PlayingField.FieldValue(_serpents.PlayerSerpent.Whereabouts).Restricted != Direction.None)
                gameState = new PlayingState(_serpents);
        }

        public void Draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            _serpents.Draw(camera, drawingReason, shadowMap);
        }

    }

}
