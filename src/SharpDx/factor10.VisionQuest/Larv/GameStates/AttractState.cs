using System;
using System.Linq;
using factor10.VisionThing;
using Larv.FloatingText;
using Larv.Serpent;
using Larv.Util;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
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

        private readonly SequentialToDoQue _seqCamera = new SequentialToDoQue();

        public AttractState(Serpents serpents)
        {
            _serpents = serpents;
            _moveCamera = new MoveCamera(
                _serpents.Camera,
                10f.UnitsPerSecond(),
                CameraLookAt,
                CameraPosition);
            _serpents.PlayerSerpent.DirectionTaker = this;
        }

        public void Update(Camera camera, GameTime gameTime, ref IGameState gameState)
        {
            addExplanationText();

            if(_moveCamera!=null)
                if (!_moveCamera.Move(gameTime))
                    _moveCamera = null;

            _serpents.Camera.UpdateFreeFlyingCamera(gameTime);
            _serpents.Update(camera, gameTime);
            if (_serpents.GameStatus() != Serpents.Result.GameOn)
            {
                _serpents.PlayerSerpent.Restart(_serpents.PlayingField, 1);
                _serpents.PlayerSerpent.DirectionTaker = this;
            }

            if (_serpents.Camera.KeyboardState.IsKeyPressed(Keys.Space))
            {
                // TODO: reset score and number of lives
                gameState = new GotoBoardState(_serpents, 0);
            }
        }

        public void Draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            _serpents.Draw(camera, drawingReason, shadowMap);

            var lcontent = _serpents.LContent;

            const string text = "LARV!";
            const float fsize = 20;
            var ssize = new Vector2(lcontent.GraphicsDevice.BackBuffer.Width, lcontent.GraphicsDevice.BackBuffer.Height);

            var sb = lcontent.SpriteBatch;
            sb.Begin();
            sb.DrawString(_serpents.LContent.Font, text, (ssize - fsize*lcontent.Font.MeasureString(text))/2, Color.White, 0, Vector2.Zero, fsize,
                SpriteEffects.None, 0);
            sb.End();
        }

        private void addExplanationText()
        {
            FloatingTextItem newItem = null;
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
                        newItem = createExplanationText(enemy, enemy.IsLonger ? "Red Enemy - eat at tail" : "Green Enemy - just eat it");
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
                _serpents.FloatingTexts.Items.Add(newItem);
        }

        private FloatingTextItem createExplanationText(IPosition target, string text)
        {
            return new FloatingTextItem(
                target,
                text,
                5)
            {
                GetOffset = _ => Vector3.Up*1.5f
            }.SetAlphaAnimation(Color.LightYellow);
        }

        private void addCameraActions()
        {
            switch (_random.Next(10))
            {
                case 0:
                    _seqCamera.Add(() => _moveCamera = new MoveCamera(
                        _serpents.Camera,
                        10f.UnitsPerSecond(),
                        new Vector3(_random.NextFloat(-2,25),0,_random.NextFloat(-2,25)),
                        new Vector3(_random.NextFloat(-2, 25), _random.NextFloat(3,6), _random.NextFloat(-2, 25)),
                        CameraPosition));
                    _seqCamera.Add(_random.NextFloat(0, 3));
                    break;

                case 1:
                    _seqCamera.Add(() => _moveCamera = new MoveCamera(
                        _serpents.Camera,
                        10f.UnitsPerSecond(),
                        new Vector3(_random.NextFloat(-2, 25), 0, _random.NextFloat(-2, 25)),
                        new Vector3(_random.NextFloat(-2, 25), _random.NextFloat(3, 6), _random.NextFloat(-2, 25)),
                        CameraPosition));
                    _seqCamera.Add(_random.NextFloat(0, 3));
                    break;

                default:
                    _seqCamera.Add(() => _moveCamera = new MoveCamera(
                        _serpents.Camera,
                        10f.UnitsPerSecond(),
                        CameraLookAt,
                        CameraPosition));
                    _seqCamera.Add(_random.NextFloat(5, 20));
                    break;
            }
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
