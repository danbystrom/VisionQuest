﻿using factor10.VisionThing;
using Larv.Serpent;
using Larv.Util;
using Serpent;
using SharpDX.Toolkit;

namespace Larv.GameStates
{
    class DieState : IGameState, ITakeDirection
    {
        private readonly Serpents _serpents;
        private readonly MoveCamera _moveCamera;
        private readonly PathFinder _pathFinder;

        public DieState(Serpents serpents)
        {
            _serpents = serpents;

            // zoom out while ghost goes up and enemies goes home
            var forward = _serpents.PlayerSerpent.LookAtPosition - _serpents.Camera.Position;
            forward.Normalize();
            _moveCamera = new MoveCamera(
                _serpents.Camera,
                5f.Time(),  // time to look at death scene
                _serpents.PlayerSerpent.LookAtPosition,
                _serpents.Camera.Position - forward * 8);

            _pathFinder = new PathFinder(_serpents.PlayingField, _serpents.PlayingField.EnemyWhereaboutsStart);
            foreach (var enemy in _serpents.Enemies)
                enemy.DirectionTaker = this;
        }

        public void Update(Camera camera, GameTime gameTime, ref IGameState gameState)
        {
            _serpents.Update(camera, gameTime);
            if (_moveCamera.Move(gameTime))
                return;
            //_serpents.Restart(_serpents.Scene);
            gameState = new StartSerpentState(_serpents);
        }

        public void Draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            _serpents.Draw(camera, drawingReason, shadowMap);
        }

        RelativeDirection ITakeDirection.TakeDirection(BaseSerpent serpent)
        {
            var direction = _pathFinder.WayHome(serpent.Whereabouts, false);
            return serpent.HeadDirection.GetRelativeDirection(direction);
        }

        bool ITakeDirection.CanOverrideRestrictedDirections(BaseSerpent serpent)
        {
            return true;
        }

    }

}
