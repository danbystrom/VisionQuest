﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Serpent.Serpent;

namespace Serpent
{
    public class PlayerSerpent : BaseSerpent
    {
        private bool _turnAround;

        public PlayerSerpent(
            Game game,
            PlayingField pf,
            ModelWrapper modelHead,
            ModelWrapper modelSegment)
            : base( game, pf, modelHead, modelSegment, new Whereabouts(0, Point.Zero, Direction.East))
        {
            _camera = new SerpentCamera(
                game.Window.ClientBounds,
                new Vector3(0, 20, 2),
                Vector3.Zero,
                CameraBehavior.FollowTarget);
        }

        public SerpentCamera Camera
        {
            get { return _camera; }
        }

        protected override float modifySpeed()
        {
            return base.modifySpeed()*1.3f;
        }

        public override void Update(GameTime gameTime, KeyboardState kbd)
        {
            UpdateCameraOnly(gameTime, kbd);
            _turnAround ^= Data.Instance.HasKeyToggled(Keys.Down);
            base.Update(gameTime, kbd);
        }

        public void UpdateCameraOnly(GameTime gameTime, KeyboardState kbd)
        {
            _camera.Update(gameTime, LookAtPosition, _headDirection, kbd);
        }

        public Vector3 LookAtPosition
        {
            get
            {
                var d = _whereabouts.Direction.DirectionAsPoint();
                return new Vector3(
                    _whereabouts.Location.X + d.X*(float) _fractionAngle,
                    _pf.GetElevation(_whereabouts),
                    _whereabouts.Location.Y + d.Y*(float) _fractionAngle);
            }
        }

        protected override void takeDirection()
        {
            var nextDirection = _turnAround ? RelativeDirection.Backward : RelativeDirection.Forward;
            _turnAround = false;
            if ( Data.Instance.KeyboardState.IsKeyDown(Keys.Left))
                nextDirection = RelativeDirection.Left;
            else if (Data.Instance.KeyboardState.IsKeyDown(Keys.Right))
                nextDirection = RelativeDirection.Right;
            if (!tryMove(_headDirection.Turn(nextDirection)))
                if (!tryMove(_whereabouts.Direction))
                    _whereabouts.Direction = Direction.None;
        }

        protected override Vector3 tintColor()
        {
            return new Vector3(0.4f, 0.4f, 0.6f);
        }

    }

}
