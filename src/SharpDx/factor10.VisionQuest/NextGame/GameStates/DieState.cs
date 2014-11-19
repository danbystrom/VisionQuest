using Larv.Serpent;
using SharpDX.Toolkit;

namespace Larv.GameStates
{
    class DieState : IGameState
    {
        private readonly Serpents _serpents;
        private float _delay;

        public DieState(Serpents serpents)
        {
            _serpents = serpents;

        }

        public void Update(GameTime gameTime, ref IGameState gameState)
        {
            _delay += (float) gameTime.ElapsedGameTime.TotalSeconds;
            _serpents.Update(gameTime);
            if(_delay > 5)
                gameState = new StartSerpentState(_serpents);
        }

        public void Draw(GameTime gameTime)
        {
            _serpents.Draw(gameTime);
        }

    }

}
