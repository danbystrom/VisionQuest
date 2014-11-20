using factor10.VisionThing;
using Larv.Serpent;
using Serpent;
using SharpDX.Toolkit;

namespace Larv.GameStates
{
    class BeginGameState : IGameState
    {
        private readonly Serpents _serpents;
        private MoveCamera _moveCamera;

        public BeginGameState(Serpents serpents)
        {
            _serpents = serpents;
            _serpents.PlayerSerpent.Restart(_serpents.PlayingField.PlayerWhereaboutsStart, 1);
            _serpents.SerpentCamera.CameraBehavior = CameraBehavior.FreeFlying;

            var lookAt = serpents.PlayerSerpent.LookAtPosition;
            var cameraPos = lookAt - serpents.PlayerSerpent.Whereabouts.Direction.DirectionAsVector3()*SerpentCamera.CameraDistanceToHeadXz;
            cameraPos.Y += SerpentCamera.CameraDistanceToHeadY;
            _moveCamera = new MoveCamera(
                serpents.SerpentCamera.Camera,
                4,
                lookAt,
                cameraPos);
        }

        public void Update(GameTime gameTime, ref IGameState gameState)
        {
            if (_moveCamera!=null)
            {
                if(_moveCamera.Move(gameTime))
                    return;
                _moveCamera = null;
                _serpents.SerpentCamera.CameraBehavior = CameraBehavior.FollowTarget;
            }

            _serpents.PlayerSerpent.Update(_serpents.SerpentCamera, gameTime);
            if (_serpents.PlayingField.FieldValue(_serpents.PlayerSerpent.Whereabouts).Restricted != Direction.None)
                gameState = new PlayingState(_serpents);
        }

        public void Draw(GameTime gameTime)
        {
            _serpents.Draw(gameTime);
        }

    }

}
