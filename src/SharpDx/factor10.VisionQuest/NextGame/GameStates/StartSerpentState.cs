using NextGame.Serpent;
using SharpDX.Toolkit;

namespace NextGame.GameStates
{
    class StartSerpentState : IGameState
    {
        private readonly Serpents _serpents;

        public StartSerpentState(Serpents serpents)
        {
            _serpents = serpents;
        }

        public void Update(GameTime gameTime, ref IGameState gameState)
        {
            _serpents.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _serpents.Draw(gameTime);
        }

    }

}
