using Larv.Serpent;
using Serpent;
using SharpDX.Toolkit;

namespace Larv.GameStates
{
    class BeginGameState : IGameState
    {
        private readonly Serpents _serpents;

        public BeginGameState(Serpents serpents)
        {
            _serpents = serpents;
            _serpents.PlayerSerpent.Restart(_serpents.PlayingField.PlayerWhereaboutsStart, 1);
        }

        public void Update(GameTime gameTime, ref IGameState gameState)
        {
            _serpents.PlayerSerpent.Update(_serpents.SerpentCamera, gameTime);
            if (_serpents.PlayingField.FieldValue(_serpents.PlayerSerpent.Whereabouts).Restricted != Direction.None)
                gameState = new PlayingState(_serpents);
        }

        public void Draw(GameTime gameTime)
        {
            _serpents.Draw(gameTime);
        }

    }

}
