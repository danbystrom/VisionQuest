using System;
using System.Collections;
using System.Linq;
using factor10.VisionThing;
using Larv.Serpent;
using Serpent;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;

namespace Larv.GameStates
{
    class AttractState : IGameState, ITakeDirection
    {
        public static readonly Vector3 CameraPosition = new Vector3(12, 12, 35);
        public static readonly Vector3 CameraLookAt = new Vector3(12, 2, 12);

        private readonly Serpents _serpents;
        private MoveCamera _moveCamera;

        private readonly Random _random = new Random();

        private readonly ExplanationTexts _explanationTexts;

        public AttractState(Serpents serpents)
        {
            _explanationTexts = new ExplanationTexts(serpents.VContent);

            _serpents = serpents;
            _moveCamera = MoveCamera.UnitsPerSecond(
                _serpents.Camera,
                10,
                CameraLookAt,
                CameraPosition);
            _serpents.PlayerSerpent.DirectionTaker = this;
        }

        public void Update(Camera camera, GameTime gameTime, ref IGameState gameState)
        {
            _explanationTexts.Update(camera, gameTime);
            addExplanationText();

            if(_moveCamera!=null)
                if (!_moveCamera.Move(gameTime))
                    _moveCamera = null;

            _serpents.Camera.UpdateFreeFlyingCamera(gameTime);
            _serpents.Update(camera, gameTime);
            if (_serpents.GameStatus() != Serpents.Result.GameOn)
            {
                _serpents.PlayerSerpent.Restart(_serpents.PlayingField.PlayerWhereaboutsStart, 1);
                _serpents.PlayerSerpent.DirectionTaker = this;
            }

            if (_serpents.Camera.KeyboardState.IsKeyPressed(Keys.Space))
                gameState = new BeginGameState(_serpents);
        }

        public void Draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            _serpents.Draw(camera, drawingReason, shadowMap);
            _explanationTexts.Draw(camera, drawingReason, shadowMap);
        }

        private void addExplanationText()
        {
            ExplanationTexts.Item newItem = null;
            switch (_random.Next(0, 500))
            {
                case 0:
                    newItem = createExplanationText(_serpents.PlayerSerpent, "Player");
                    break;
                case 1:
                    if (_serpents.PlayerEgg != null)
                        newItem = createExplanationText(_serpents.PlayerEgg, "Player's egg - protect to get bonus life");
                    break;
                case 2:
                    if (_serpents.Enemies.Any())
                    {
                        var enemy = _serpents.Enemies[_random.Next(0, _serpents.Enemies.Count)];
                        newItem = createExplanationText(enemy, enemy.IsLonger ? "Red Enemy - eat at tail" : "Green Enemy - eat anywhere");
                    }
                    break;
                case 3:
                    if (_serpents.EnemyEggs.Any())
                        newItem = createExplanationText(_serpents.EnemyEggs[_random.Next(0, _serpents.EnemyEggs.Count)], "Enemy egg - eat before it hatches");
                    break;
                case 4:
                    if (_serpents.Frogs.Any())
                        newItem = createExplanationText(_serpents.Frogs[_random.Next(0, _serpents.Frogs.Count)], "Frog is food and eats eggs");
                    break;
            }
            if (newItem != null)
                _explanationTexts.Items.Add(newItem);
        }

        private ExplanationTexts.Item createExplanationText(IPosition target, string text1)
        {
            return new ExplanationTexts.Item
            {
                Target = target,
                TimeToLive = 5,
                GetDrawingInfo = (_) => new ExplanationTexts.DrawingInfo
                {
                    DiffuseColor = textDiffuse(_.Age/_.TimeToLive),
                    Text1 = text1,
                }
            };
        }

        private Vector4 textDiffuse(float factor)
        {
            var alpha = 1f;
            switch((int)(factor*3))
            {
                case 0:
                    alpha = factor*3;
                    break;
                case 2:
                    alpha = (1 - factor) * 3;
                    break;
            }
            return new Vector4(1, 1, 0.8f, alpha);
        }

        RelativeDirection ITakeDirection.TakeDirection(BaseSerpent serpent)
        {
            return _random.NextDouble() < 0.5 ? RelativeDirection.Left : RelativeDirection.Right;
        }

        bool ITakeDirection.CanOverrideRestrictedDirections(BaseSerpent serpent)
        {
            return false;
        }

    }

}
