using factor10.VisionThing;
using Larv.Serpent;
using Serpent;
using SharpDX.Toolkit;

namespace Larv.GameStates
{
    class LevelCompleteState : IGameState, ITakeDirection
    {
        private readonly Serpents _serpents;
        private readonly PathFinder _pathFinder;
        private bool _serpentIsHome;
        private MoveCamera _moveCamera;

        public LevelCompleteState(Serpents serpents)
        {
            _serpents = serpents;
            _pathFinder = new PathFinder(_serpents.PlayingField, _serpents.PlayingField.PlayerWhereaboutsStart);
            _serpents.PlayerSerpent.DirectionTaker = this;
            _moveCamera = MoveCamera.UnitsPerSecond(
                _serpents.Camera,
                10,
                AttractState.CameraLookAt,
                AttractState.CameraPosition);
        }

        RelativeDirection ITakeDirection.TakeDirection(BaseSerpent serpent)
        {
            var direction = _pathFinder.WayHome(serpent.Whereabouts, false);
            if (direction == Direction.None)
            {
                _serpentIsHome = true;
                return RelativeDirection.None;
            }
            return serpent.HeadDirection.GetRelativeDirection(direction);
        }

        bool ITakeDirection.CanOverrideRestrictedDirections(BaseSerpent serpent)
        {
            return true;
        }

        public void Update(Camera camera, GameTime gameTime, ref IGameState gameState)
        {
            _moveCamera.Move(gameTime);
            _serpents.Update(gameTime);
            if (!_serpentIsHome)
                return;

            if (_serpents.PlayerEgg != null)
            {
                _serpents.PlayerSerpent.Restart(_serpents.PlayerEgg.Whereabouts, 0);
                _serpents.PlayerEgg = null;
                _serpentIsHome = false;
            }
            else
                gameState = new StartSerpentState(_serpents);
        }

        public void Draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            _serpents.Draw(camera,drawingReason,shadowMap);
        }

    }

}
