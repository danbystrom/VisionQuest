using factor10.VisionThing;
using Larv.Serpent;
using Larv.Util;
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

        private ToDoQue _actions = new ToDoQue();
        private readonly ExplanationTexts _explanationTexts;

        public LevelCompleteState(Serpents serpents)
        {
            _serpents = serpents;
            _explanationTexts = new ExplanationTexts(_serpents.VContent);

            _pathFinder = new PathFinder(_serpents.PlayingField, _serpents.PlayingField.PlayerWhereaboutsStart);
            _serpents.PlayerSerpent.DirectionTaker = this;
            _serpents.PlayerSerpent.TimeToLayEgg();  // don't lay egg on the way home...

            Vector3 toPosition, toLookAt;
            _serpents.PlayingField.GetCammeraPositionForLookingAtPlayerCave(out toPosition, out toLookAt);

            _explanationTexts.Items.Add(new ExplanationTexts.Item
            {
                Target = _serpents.PlayerSerpent,
                TimeToLive = 3,
                GetDrawingInfo = _ => new ExplanationTexts.DrawingInfo
                {
                    DiffuseColor = new Vector4(1,1,1,1),
                    Text1 = "Yeah"
                }
            });

            _moveCamera = MoveCamera.UnitsPerSecond(
                _serpents.Camera,
                3,
                toLookAt,
                toPosition);

            _actions.Add(time => !_serpentIsHome);
            _actions.Add(3);
            _actions.Add(() =>
            {
                if (_serpents.PlayerEgg == null)
                    return;
                _serpents.PlayerSerpent.Restart(_serpents.PlayerEgg.Whereabouts, 0);
                _serpents.PlayerSerpent.DirectionTaker = this;
                _serpents.PlayerEgg = null;
                _serpentIsHome = false;
            });
            _actions.Add(time => !_serpentIsHome);
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
            _moveCamera.Move(gameTime);
            if (_actions.Do(gameTime))
                return;
            _serpents.Restart();
            gameState = new StartSerpentState(_serpents);
        }

        public void Draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            _serpents.Draw(camera,drawingReason,shadowMap);
        }

        private class PositionHolder : IPosition
        {
            public Vector3 Position { get; set; }
        }
    }

}
