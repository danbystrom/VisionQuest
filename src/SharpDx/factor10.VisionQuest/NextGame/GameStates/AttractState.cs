using System;
using factor10.VisionThing;
using Larv.Serpent;
using Serpent;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;

namespace Larv.GameStates
{
    class AttractState : IGameState, PlayerSerpent.ITakeDirection
    {
        private readonly Serpents _serpents;
        private MoveCamera _moveCamera;

        private readonly Random _random = new Random();
 
        public AttractState(Serpents serpents)
        {
            _serpents = serpents;
            _moveCamera = new MoveCamera(
                _serpents.SerpentCamera.Camera,
                5,
                new Vector3(12, 2, 12),
                new Vector3(12, 12, 35));
            _serpents.SerpentCamera.CameraBehavior = CameraBehavior.FreeFlying;
            _serpents.PlayerSerpent.DirectionTaker = this;
        }

        public void Update(GameTime gameTime, ref IGameState gameState)
        {
            if(_moveCamera!=null)
                if (!_moveCamera.Move(gameTime))
                    _moveCamera = null;

            _serpents.Update(gameTime);
            if (_serpents.SerpentCamera.Camera.KeyboardState.IsKeyPressed(Keys.Space))
            {
                gameState = new BeginGameState(_serpents);
                _serpents.PlayerSerpent.DirectionTaker = null;
            }
        }

        public void Draw(GameTime gameTime)
        {
            _serpents.Draw(gameTime);
        }

        RelativeDirection PlayerSerpent.ITakeDirection.TakeDirection(Direction headDirection)
        {
            var result = _random.NextDouble() < 0.5 ? RelativeDirection.Left : RelativeDirection.Right;
            return result;
        }

        bool PlayerSerpent.ITakeDirection.CanOverrideRestrictedDirections()
        {
            return false;
        }

    }

}
