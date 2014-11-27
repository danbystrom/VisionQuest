using factor10.VisionThing;
using Larv.Serpent;
using Serpent;
using SharpDX;
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

            Vector3 toPosition, toLookAt;
            _serpents.PlayingField.GetCammeraPositionForLookingAtPlayerCave(out toPosition, out toLookAt);

            _moveCamera = MoveCamera.UnitsPerSecond(
                _serpents.Camera,
                3,
                toLookAt,
                toPosition);
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
            _serpents.Update(gameTime);

            if (_moveCamera != null)
            {
                if (_moveCamera.Move(gameTime))
                    return;
                _moveCamera = null;
            }

            if (!_serpentIsHome)
                return;

            // här ska bonus ges

            if (_serpents.PlayerEgg != null)
            {
                _serpents.PlayerSerpent.Restart(_serpents.PlayerEgg.Whereabouts, 0);
                _serpents.PlayerSerpent.DirectionTaker = this;
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
