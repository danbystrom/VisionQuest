using SharpDX.Toolkit;

namespace Larv.GameStates
{
    public interface IGameState
    {
        void Update(GameTime gameTime, ref IGameState gameState);
        void Draw(GameTime gameTime);
    }
}
