using System;
using factor10.VisionThing;
using Larv.Field;
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
        private readonly SerpentCamera _serpentCamera;

        public StartSerpentState(Serpents serpents)
        {
            _serpents = serpents;

            Vector3 toPosition, toLookAt;
            _serpents.PlayingField.GetCameraPositionForLookingAtPlayerCave(out toPosition, out toLookAt);

            _actions.AddMoveable(new MoveCameraYaw(
                serpents.Camera,
                5f.UnitsPerSecond(),
                toPosition,
                GetPlayerInitialLookAt(_serpents.PlayingField)));
            _actions.Add(() =>
            {
                _serpents.PlayerSerpent.Restart(_serpents.PlayingField, 1);
                while (_serpents.Enemies.Count > 4 + _serpents.Scene)
                    _serpents.Enemies.RemoveAt(0);
                foreach (var enemy in _serpents.Enemies)
                    enemy.DirectionTaker = null;
            });

            _serpentCamera = new SerpentCamera(_serpents.Camera, _serpents.PlayerSerpent, 0, 3, 9);
        }

        public void Update(Camera camera, GameTime gameTime, ref IGameState gameState)
        {
            foreach (var enemy in _serpents.Enemies)
                enemy.Update(camera, gameTime);

            if (_actions.Do(gameTime))
                return;

            _serpentCamera.Move(gameTime);

            _serpents.PlayerSerpent.Update(_serpents.Camera, gameTime);
            // farligt - skulle det ske ett "hopp" här så skulle vi inte märka att rutan passerades...
            if (_serpents.PlayingField.FieldValue(_serpents.PlayerSerpent.Whereabouts).Restricted != Direction.None)
                gameState = new PlayingState(_serpents, _serpentCamera);
        }

        public void Draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            _serpents.Draw(camera, drawingReason, shadowMap);
        }

        public static Vector3 GetPlayerInitialLookAt(PlayingField pf)
        {
            return pf.PlayerWhereaboutsStart.GetPosition(pf);
            ;
        }

    }

}
