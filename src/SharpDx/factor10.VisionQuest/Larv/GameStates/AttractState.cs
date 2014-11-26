﻿using System;
using factor10.VisionThing;
using Larv.Serpent;
using Serpent;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;

namespace Larv.GameStates
{
    class AttractState : IGameState, ITakeDirection
    {
        public static readonly Vector3 CameraPosition = new Vector3(12, 12, 35);
        public static readonly Vector3 CameraLookAt = new Vector3(12, 2, 12);

        private readonly Serpents _serpents;
        private MoveCamera _moveCamera;

        private readonly Random _random = new Random();
 
        public AttractState(Serpents serpents)
        {
            _serpents = serpents;
            _moveCamera = MoveCamera.UnitsPerSecond(
                _serpents.Camera,
                10,
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

            if (_serpents.Update(gameTime) != Serpents.Result.GameOn)
                _serpents.PlayerSerpent.Restart(_serpents.PlayingField.PlayerWhereaboutsStart, 1);

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

        RelativeDirection ITakeDirection.TakeDirection(BaseSerpent serpent)
        {
            return _random.NextDouble() < 0.5 ? RelativeDirection.Left : RelativeDirection.Right;
        }

        bool ITakeDirection.CanOverrideRestrictedDirections(BaseSerpent serpent)
        {
            return false;
        }

    }

}
