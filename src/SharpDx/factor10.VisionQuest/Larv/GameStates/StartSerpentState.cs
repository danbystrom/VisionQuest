using System;
using factor10.VisionThing;
using Larv.Serpent;
using Serpent;
using SharpDX;
using SharpDX.Toolkit;

namespace Larv.GameStates
{
    internal class StartSerpentState : IGameState
    {
        private readonly Serpents _serpents;
        private MoveCamera _moveCamera;

        public StartSerpentState(Serpents serpents)
        {
            _serpents = serpents;
            _serpents.PlayerSerpent.Restart(_serpents.PlayingField.PlayerWhereaboutsStart, 1);

            Vector3 toPosition, toLookAt;
            _serpents.PlayingField.GetCammeraPositionForLookingAtPlayerCave(out toPosition, out toLookAt);

            var x = new ArcGenerator(4);
            x.CreateArc(
                serpents.Camera.Position,
                toPosition,
                Vector3.Right,
                SerpentCamera.CameraDistanceToHeadXz);
            _moveCamera = MoveCamera.TotalTime(
                serpents.Camera,
                4,
                toLookAt,
                x.Points);
        }

        public void Update(Camera camera, GameTime gameTime, ref IGameState gameState)
        {
            if (_moveCamera != null)
            {
                if (_moveCamera.Move(gameTime))
                    return;
                _moveCamera = null;
            }

            _serpents.PlayerSerpent.Update(_serpents.Camera, gameTime);
            if (_serpents.PlayingField.FieldValue(_serpents.PlayerSerpent.Whereabouts).Restricted != Direction.None)
                gameState = new PlayingState(_serpents, _moveCamera);
        }

        public void Draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            _serpents.Draw(camera, drawingReason, shadowMap);
        }

    }

}
