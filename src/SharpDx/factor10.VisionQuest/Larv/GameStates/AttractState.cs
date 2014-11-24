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
        public static readonly Vector3 CameraPosition = new Vector3(12, 12, 35);
        public static readonly Vector3 CameraLookAt = new Vector3(12, 2, 12);

        private readonly Serpents _serpents;
        private MoveCamera _moveCamera;

        private readonly Random _random = new Random();
 
        public AttractState(Serpents serpents)
        {
            _serpents = serpents;
            _moveCamera = new MoveCamera(
                _serpents.Camera,
                4,
                CameraLookAt,
                CameraPosition);
            _serpents.PlayerSerpent.DirectionTaker = this;
        }

        public void Update(Camera camera, GameTime gameTime, ref IGameState gameState)
        {
            if(_moveCamera!=null)
                if (!_moveCamera.Move(gameTime))
                    _moveCamera = null;

            _serpents.Camera.UpdateFreeFlyingCamera(gameTime);

            _serpents.Update(gameTime);
            if (_serpents.Camera.KeyboardState.IsKeyPressed(Keys.Space))
            {
                gameState = new BeginGameState(_serpents);
                _serpents.PlayerSerpent.DirectionTaker = null;
            }
        }

        public void Draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            _serpents.Draw(camera, drawingReason, shadowMap);
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
