using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using factor10.VisionThing;
using Larv.FloatingText;
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
        private MoveCamera _moveCamera;

        private readonly SequentialToDoQue _actions = new SequentialToDoQue();
        private readonly FloatingTexts _floatingTexts;

        private bool _serpentIsHome;
        private bool _haltSerpents;
        private bool _homeIsNearCaveEntrance;

        public LevelCompleteState(Serpents serpents)
        {
            _serpents = serpents;
            _floatingTexts = new FloatingTexts(_serpents.VContent);

            _pathFinder = new PathFinder(_serpents.PlayingField, _serpents.PlayingField.PlayerWhereaboutsStart);
            _serpents.PlayerSerpent.DirectionTaker = this;

            Vector3 toPosition, toLookAt;
            _serpents.PlayingField.GetCameraPositionForLookingAtPlayerCave(out toPosition, out toLookAt);

            _moveCamera =new MoveCamera(
                _serpents.Camera,
                4f.UnitsPerSecond(),
                () => _serpents.PlayerSerpent.LookAtPosition,
                toPosition);

            // wait until serpent is in cave, then give length bonus
            _actions.Add(() => _homeIsNearCaveEntrance = true);
            _actions.Add(time => !_serpentIsHome);
            _actions.Add(() => _haltSerpents = true);
            for (var i = 0; i < _serpents.PlayerSerpent.Length; i++)
                _actions.Add(1, () =>
                {
                    var tailSegement = _serpents.PlayerSerpent.RemoveTailWhenLevelComplete();
                    if (tailSegement != null)
                        _floatingTexts.Items.Add(new FloatingTextItem(
                            tailSegement,
                            "+500",
                            3).SetAlphaAnimation(Color.WhiteSmoke, 0.1f, 0.2f).SetOffsetMovement(Vector3.Up, Vector3.Up*3));
                });

            // wait until all bonus texts gone
            _actions.Add(time => _floatingTexts.Items.Any());
            _actions.Add(() =>
            {
                _serpentIsHome = false;
                _haltSerpents = false;
                _homeIsNearCaveEntrance = false;
            });
            _actions.Add(time => !_serpentIsHome);

            _actions.Add(() =>
            {
                if (_serpents.PlayerEgg == null)
                {
                    _haltSerpents = false;
                    return;
                }
                _serpents.PlayerSerpent.DirectionTaker = null;
                _moveCamera = new MoveCamera(_serpents.Camera, 2f.Time(), _serpents.PlayerEgg.Position, toPosition);
                // wait two sec (for camera) and then drive the baby home
                _actions.InsertFirst(
                    time => time < 2,
                    time =>
                    {
                        _serpents.PlayerSerpent.Restart(_serpents.PlayingField, 0, _serpents.PlayerEgg.Whereabouts);
                        _serpents.PlayerEgg = null;
                        _serpents.PlayerSerpent.DirectionTaker = this;
                        _haltSerpents = false;
                        _serpentIsHome = false;
                        return false;
                    });
            });

            // make sure the camera aims at a serpent (the original or the new born baby)
            _actions.Add(() =>
            {
                _moveCamera = new MoveCamera(_serpents.Camera, 1f.Time(), () => _serpents.PlayerSerpent.LookAtPosition, toPosition);
                _moveCamera.NeverComplete = true;
            });

            _actions.Add(time => !_serpentIsHome || _floatingTexts.Items.Any());
        }

        RelativeDirection ITakeDirection.TakeDirection(BaseSerpent serpent)
        {
            _serpentIsHome = _pathFinder.GetDistance(_serpents.PlayerSerpent.Whereabouts) < (_homeIsNearCaveEntrance ? 6 : 3);
            _haltSerpents |= _serpentIsHome;
            return serpent.HeadDirection.GetRelativeDirection(_pathFinder.WayHome(serpent.Whereabouts, false));
        }

        bool ITakeDirection.CanOverrideRestrictedDirections(BaseSerpent serpent)
        {
            return true;
        }

        public void Update(Camera camera, GameTime gameTime, ref IGameState gameState)
        {
            if (!_haltSerpents)
                _serpents.Update(camera, gameTime);
            _floatingTexts.Update(camera, gameTime);
            _moveCamera.Move(gameTime);
            if (_actions.Do(gameTime))
                return;
            gameState = new GotoBoardState(_serpents, _serpents.Scene + 1);
        }

        public void Draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            _serpents.Draw(camera,drawingReason,shadowMap);
            _floatingTexts.Draw(camera, drawingReason, shadowMap);
        }

    }

}
