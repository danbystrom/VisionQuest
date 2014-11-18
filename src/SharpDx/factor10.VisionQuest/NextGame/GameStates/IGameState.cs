using SharpDX.Toolkit;

namespace NextGame.GameStates
{
    public interface IGameState
    {
        void Update(GameTime gameTime, ref IGameState gameState);
        void Draw(GameTime gameTime);
    }
}
