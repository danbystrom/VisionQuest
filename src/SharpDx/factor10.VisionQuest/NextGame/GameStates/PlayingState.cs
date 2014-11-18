using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextGame.Serpent;
using Serpent;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;

namespace NextGame.GameStates
{
    class PlayingState : IGameState, PlayerSerpent.ITakeDirection
    {
        private readonly Serpents _serpents;
        private float _delayAfterLevelComplete = 0;

        public PlayingState(Serpents serpents)
        {
            _serpents = serpents;
            _serpents.PlayerSerpent.DirectionTaker = this;
        }

        public void Update(GameTime gameTime, ref IGameState gameState)
        {
            _turnAround ^= _serpents.PlayerSerpent.Camera.Camera.KeyboardState.IsKeyPressed(Keys.Down);
            switch (_serpents.Update(gameTime))
            {
                case Serpents.Result.LevelComplete:
                    _delayAfterLevelComplete += (float) gameTime.ElapsedGameTime.TotalSeconds;
                    if (_delayAfterLevelComplete > 3)
                    {
                        _serpents.PlayerSerpent.DirectionTaker = null;
                        gameState = new LevelCompleteState(_serpents);
                    }
                    break;
                case Serpents.Result.PlayerDied:
                    _serpents.PlayerSerpent.DirectionTaker = null;
                    gameState = new DieState(_serpents);
                    break;
            }
        }

        private bool _isHoldingBothPointers;
        private bool _turnAround;

        public RelativeDirection TakeDirection(Direction headDirection)
        {
            var pointerLeft = _serpents.PlayerSerpent.Camera.Camera.PointerState.Points.Any(_ => _.Position.X < 0.15f && _.EventType==PointerEventType.Moved);
            var pointerRight = _serpents.PlayerSerpent.Camera.Camera.PointerState.Points.Any(_ => _.Position.X > 0.5f && _.EventType == PointerEventType.Moved);
            if (pointerLeft && pointerRight)
            {
                pointerLeft = false;
                pointerRight = false;
                _turnAround = !_isHoldingBothPointers;
                _isHoldingBothPointers = true;
            }
            else
                _isHoldingBothPointers = false;

            var nextDirection = _turnAround ? RelativeDirection.Backward : RelativeDirection.Forward;
            _turnAround = false;
            if (_serpents.PlayerSerpent.Camera.Camera.KeyboardState.IsKeyDown(Keys.Left) || pointerLeft)
                nextDirection = RelativeDirection.Left;
            else if (_serpents.PlayerSerpent.Camera.Camera.KeyboardState.IsKeyDown(Keys.Right) || pointerRight)
                nextDirection = RelativeDirection.Right;
            return nextDirection;
        }

        public void Draw(GameTime gameTime)
        {
            _serpents.Draw(gameTime);
        }

    }

}
