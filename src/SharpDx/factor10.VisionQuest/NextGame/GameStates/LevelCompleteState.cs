using Larv.Serpent;
using NextGame;
using Serpent;
using Serpent.Serpent;
using SharpDX.Toolkit;

namespace Larv.GameStates
{
    class LevelCompleteState : IGameState, PlayerSerpent.ITakeDirection
    {
        private readonly Serpents _serpents;
        private readonly PathFinder _pathFinder;
        private bool _serpentIsHome;

        public LevelCompleteState(Serpents serpents)
        {
            _serpents = serpents;
            _pathFinder = new PathFinder(_serpents.PlayingField, _serpents.PlayingField.PlayerWhereaboutsStart);
            _serpents.PlayerSerpent.DirectionTaker = this;
            _serpents.SerpentCamera.CameraBehavior = CameraBehavior.Static;
        }


        RelativeDirection PlayerSerpent.ITakeDirection.TakeDirection(Direction headDirection)
        {
            var direction = _pathFinder.WayHome(_serpents.PlayerSerpent.Whereabouts);
            if (direction == Direction.None)
            {
                _serpentIsHome = true;
                return RelativeDirection.None;
            }
            return headDirection.GetRelativeDirection(direction);
        }

        bool PlayerSerpent.ITakeDirection.CanOverrideRestrictedDirections()
        {
            return true;
        }


        public void Update(GameTime gameTime, ref IGameState gameState)
        {
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

        public void Draw(GameTime gameTime)
        {
            _serpents.Draw(gameTime);
        }

    }

}
