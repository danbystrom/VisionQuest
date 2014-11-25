using System.Linq;
using factor10.VisionThing;
using Larv.Serpent;
using Serpent;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;

namespace Larv.GameStates
{
    class PlayingState : IGameState, PlayerSerpent.ITakeDirection
    {
        private readonly Serpents _serpents;
        private float _delayAfterLevelComplete = 0;

        public SerpentCamera SerpentCamera = new SerpentCamera(
            CameraBehavior.FollowTarget);

        private MoveCamera _helper;

        public PlayingState(Serpents serpents, MoveCamera helper)
        {
            _serpents = serpents;
            _serpents.PlayerSerpent.DirectionTaker = this;
            SerpentCamera.SetCameraBehavior(_serpents.Camera, CameraBehavior.FollowTarget);
            _helper = helper;
        }

        public void Update(Camera camera, GameTime gameTime, ref IGameState gameState)
        {
            SerpentCamera.Update(gameTime, camera, _serpents.PlayerSerpent.LookAtPosition, _serpents.PlayerSerpent.HeadDirection);
            //camera.UpdateFreeFlyingCamera(gameTime);

            _turnAround ^= _serpents.Camera.KeyboardState.IsKeyPressed(Keys.Down);
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

        RelativeDirection PlayerSerpent.ITakeDirection.TakeDirection(Direction headDirection)
        {
            var pointerPoints = _serpents.Camera.PointerState.Points.Where(_ => _.EventType == PointerEventType.Pressed).ToArray();
            var pointerLeft = pointerPoints.Any(_ => _.Position.X < 0.15f);
            var pointerRight = pointerPoints.Any(_ => _.Position.X > 0.5f);
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
            if (_serpents.Camera.KeyboardState.IsKeyDown(Keys.Left) || pointerLeft)
                nextDirection = RelativeDirection.Left;
            else if (_serpents.Camera.KeyboardState.IsKeyDown(Keys.Right) || pointerRight)
                nextDirection = RelativeDirection.Right;
            return nextDirection;
        }

        bool PlayerSerpent.ITakeDirection.CanOverrideRestrictedDirections()
        {
            return false;
        }

        public void Draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            //var scale = Matrix.Scaling(0.3f);
            //foreach (var v in _helper.Path)
            //{
            //    camera.UpdateEffect(_serpents.Effect);
            //    _serpents.Effect.World = scale*Matrix.Translation(v);
            //    _serpents.Sphere.Draw(_serpents.Effect);
            //}
            _serpents.Draw(camera, drawingReason, shadowMap);
        }

    }

}
