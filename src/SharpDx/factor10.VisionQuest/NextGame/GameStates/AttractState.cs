using System.Collections.Generic;
using System.Linq;
using NextGame.Serpent;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;

namespace NextGame.GameStates
{
    class AttractState : IGameState
    {
        private readonly Serpents _serpents;

        public AttractState(Serpents serpents)
        {
            _serpents = serpents;
        }

        public void Update(GameTime gameTime, ref IGameState gameState)
        {
            _serpents.Update(gameTime);
            var list = new List<Keys>();
            _serpents.PlayerSerpent.Camera.Camera.KeyboardState.GetDownKeys(list);
            if(list.Any())
                gameState = new BeginGameState(_serpents); 
        }

        public void Draw(GameTime gameTime)
        {
            _serpents.Draw(gameTime);
        }

    }

}
