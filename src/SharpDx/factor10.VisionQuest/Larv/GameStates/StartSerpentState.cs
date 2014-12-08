using factor10.VisionThing;
using Larv.Serpent;
using Larv.Util;
using SharpDX;
using SharpDX.Toolkit;

namespace Larv.GameStates
{
    internal class StartSerpentState : IGameState
    {
        private readonly Serpents _serpents;
        private readonly SequentialToDoQue _actions = new SequentialToDoQue();

        public StartSerpentState(Serpents serpents)
        {
            _serpents = serpents;

            Vector3 toPosition, toLookAt;
            _serpents.PlayingField.GetCameraPositionForLookingAtPlayerCave(out toPosition, out toLookAt);

            var moveCamera = new MoveCamera(
                serpents.Camera,
                5f.UnitsPerSecond(),
                toLookAt,
                toPosition);

            _actions.Add(moveCamera.Move);
            _actions.Add(() =>
            {
                _serpents.PlayerSerpent.Restart(_serpents.PlayingField, 1);
                foreach (var enemy in _serpents.Enemies)
                    enemy.DirectionTaker = null;
            });
        }

        public void Update(Camera camera, GameTime gameTime, ref IGameState gameState)
        {
            foreach (var enemy in _serpents.Enemies)
                enemy.Update(camera, gameTime);

            if (_actions.Do(gameTime))
                return;

            _serpents.PlayerSerpent.Update(_serpents.Camera, gameTime);
            // farligt - skulle det ske ett "hopp" här så skulle vi inte märka att rutan passerades...
            if (_serpents.PlayingField.FieldValue(_serpents.PlayerSerpent.Whereabouts).Restricted != Direction.None)
                gameState = new PlayingState(_serpents);
        }

        public void Draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            _serpents.Draw(camera, drawingReason, shadowMap);
        }

    }

}
