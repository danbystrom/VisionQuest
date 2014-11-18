﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextGame.Serpent;
using Serpent;
using Serpent.Serpent;
using SharpDX.Toolkit;

namespace NextGame.GameStates
{
    class LevelCompleteState : IGameState, PlayerSerpent.ITakeDirection
    {
        private readonly Serpents _serpents;
        private readonly PathFinder _pathFinder;
        private bool _serpentIsHome;

        public LevelCompleteState(Serpents serpents)
        {
            _serpents = serpents;
            _pathFinder = new PathFinder(_serpents.PlayingField, _serpents.PlayingField.PlayerWhereaboutsStart);
            _serpents.PlayerSerpent.DirectionTaker = this;
            _serpents.PlayerSerpent.Camera.CameraBehavior = CameraBehavior.Static;
        }


        public RelativeDirection TakeDirection(Direction headDirection)
        {
            var direction = _pathFinder.WayHome(_serpents.PlayerSerpent.Whereabouts);
            if (direction == Direction.None)
            {
                _serpentIsHome = true;
                return RelativeDirection.None;
            }
            return headDirection.GetRelativeDirection(direction);
        }


        public void Update(GameTime gameTime, ref IGameState gameState)
        {
            _serpents.Update(gameTime);
            if (!_serpentIsHome)
                return;

            if (_serpents.PlayerEgg != null)
            {
                _serpents.PlayerSerpent.Restart(_serpents.PlayerEgg.Whereabouts, 0);
                _serpents.PlayerEgg = null;
                _serpentIsHome = false;
            }
            else
                gameState = new StartSerpentState(_serpents);
        }

        public void Draw(GameTime gameTime)
        {
            _serpents.Draw(gameTime);
        }

    }

}
