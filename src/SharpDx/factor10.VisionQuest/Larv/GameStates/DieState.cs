using factor10.VisionThing;
using Larv.Serpent;
using Larv.Util;
using SharpDX.Toolkit;

namespace Larv.GameStates
{
    class DieState : IGameState
    {
        private readonly Serpents _serpents;
        private readonly SequentialToDoQue _todo  = new SequentialToDoQue();

        public DieState(Serpents serpents)
        {
            _serpents = serpents;

            HomingDevice.Attach(_serpents, false);

            // zoom out while ghost goes up and enemies goes home
            var forward = _serpents.PlayerSerpent.LookAtPosition - _serpents.Camera.Position;
            forward.Normalize();
            _todo.AddMoveable(new MoveCamera(
                _serpents.Camera,
                5f.Time(),  // time to look at death scene
                _serpents.PlayerSerpent.LookAtPosition,
                _serpents.Camera.Position - forward*8));
        }

        public void Update(Camera camera, GameTime gameTime, ref IGameState gameState)
        {
            _serpents.Update(camera, gameTime);
            if (_todo.Do(gameTime))
                return;
            gameState = new StartSerpentState(_serpents);
        }

        public void Draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            _serpents.Draw(camera, drawingReason, shadowMap);
        }

    }

}
