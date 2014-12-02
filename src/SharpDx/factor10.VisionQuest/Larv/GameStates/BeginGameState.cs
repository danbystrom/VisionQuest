﻿using factor10.VisionThing;
using Larv.Serpent;
using Serpent;
using SharpDX;
using SharpDX.Toolkit;

namespace Larv.GameStates
{
    internal class BeginGameState : IGameState, ITakeDirection
    {
        private readonly Serpents _serpents;
        private MoveCamera _moveCamera;

        private readonly PathFinder _playerPathFinder;
        private readonly PathFinder _enemyPathFinder;

        public BeginGameState(Serpents serpents)
        {
            _serpents = serpents;

            _playerPathFinder = new PathFinder(_serpents.PlayingField, _serpents.PlayingField.PlayerWhereaboutsStart);
            _enemyPathFinder = new PathFinder(_serpents.PlayingField, _serpents.PlayingField.EnemyWhereaboutsStart);
            _serpents.PlayerSerpent.DirectionTaker = this;
            foreach (var enemy in _serpents.Enemies)
                enemy.DirectionTaker = this;

            Vector3 toPosition, toLookAt;
            _serpents.PlayingField.GetCammeraPositionForLookingAtPlayerCave(out toPosition, out toLookAt);

            var x = new ArcGenerator(4);
            x.CreateArc(
                serpents.Camera.Position,
                toPosition,
                Vector3.Right,
                SerpentCamera.CameraDistanceToHeadXz);
            _moveCamera = MoveCamera.TotalTime(
                serpents.Camera,
                4,
                toLookAt,
                x.Points);
        }

        public void Update(Camera camera, GameTime gameTime, ref IGameState gameState)
        {
            _serpents.Update(camera, gameTime);

            if (_moveCamera != null)
            {
                if (_moveCamera.Move(gameTime))
                    return;
                _moveCamera = null;
                _serpents.Restart(0);
            }

            if (_serpents.PlayingField.FieldValue(_serpents.PlayerSerpent.Whereabouts).Restricted != Direction.None)
                gameState = new PlayingState(_serpents, _moveCamera);
        }

        public void Draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            _serpents.Draw(camera, drawingReason, shadowMap);
        }

        RelativeDirection ITakeDirection.TakeDirection(BaseSerpent serpent)
        {
            var pathFinder = serpent is PlayerSerpent ? _playerPathFinder : _enemyPathFinder;
            var direction = pathFinder.WayHome(serpent.Whereabouts, false);
            return serpent.HeadDirection.GetRelativeDirection(direction);
        }

        bool ITakeDirection.CanOverrideRestrictedDirections(BaseSerpent serpent)
        {
            return true;
        }

    }

}
