using factor10.VisionThing;
using Larv.Serpent;
using SharpDX.Toolkit;

namespace Larv.GameStates
{
    class StartSerpentState : IGameState
    {
        private readonly Serpents _serpents;

        public StartSerpentState(Serpents serpents)
        {
            _serpents = serpents;
        }

        public void Update(Camera camera, GameTime gameTime, ref IGameState gameState)
        {
            _serpents.Update(gameTime);
        }

        public void Draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            _serpents.Draw(camera,drawingReason, shadowMap);
        }

    }

}
