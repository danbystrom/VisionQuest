using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Serpent;
using Serpent.Serpent;
using factor10.VisionThing;
using factor10.VisionThing.Water;

namespace factor10.SerpentGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private ModelManager _modelManager;
        private readonly GraphicsDeviceManager _graphics;

        private Data _data;
        private bool _paused;

        private BillboardText _billboardText;

        private WaterSurface _water;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            startGame();
            base.Initialize();
        }

        private void startGame()
        {
            Components.Clear();
            _data = new Data(this);
            _modelManager = new ModelManager(this, _data.PlayerSerpent.Camera.Camera);
            Components.Add(_modelManager);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            _billboardText = new BillboardText(
                GraphicsDevice,
                Content.Load<SpriteFont>(@"Fonts\SpriteFont1"));

            _water = WaterFactory.Create(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _data.UpdateKeyboard();

            var camera = _data.PlayerSerpent.Camera;

            if (_data.KeyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
                return;
            }

            if (_data.HasKeyToggled(Keys.Enter) && _data.KeyboardState.IsKeyDown(Keys.LeftAlt))
            {
                _graphics.IsFullScreen ^= true;
                _graphics.ApplyChanges();
            }

            if (_data.HasKeyToggled(Keys.C))
                _data.PlayerSerpent.Camera.CameraBehavior = camera.CameraBehavior ==
                                                            CameraBehavior.FollowTarget
                                                                ? CameraBehavior.Static
                                                                : CameraBehavior.FollowTarget;
            if (_data.HasKeyToggled(Keys.F))
                _data.PlayerSerpent.Camera.CameraBehavior = camera.CameraBehavior ==
                                                            CameraBehavior.FollowTarget
                                                                ? CameraBehavior.FreeFlying
                                                                : CameraBehavior.FollowTarget;
            if (_data.HasKeyToggled(Keys.P))
                _paused ^= true;

            _water.Update((float)gameTime.ElapsedGameTime.TotalSeconds, _data.PlayerSerpent.Camera.Camera);

            if (_paused)
            {
                _data.PlayerSerpent.UpdateCameraOnly(gameTime);
                return;
            }

            _data.PlayerSerpent.Update(gameTime);
            foreach (var enemy in _data.Enemies)
            {
                enemy.Update(gameTime);
                if (enemy.EatAt(_data.PlayerSerpent))
                    startGame();
                else if (enemy.SerpentStatus == SerpentStatus.Alive && _data.PlayerSerpent.EatAt(enemy))
                    enemy.SerpentStatus = SerpentStatus.Ghost;
            }
            _data.Enemies.RemoveAll(e => e.SerpentStatus == SerpentStatus.Finished);
            if (_data.Enemies.Count == 0)
                startGame();

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            var camera = _data.PlayerSerpent.Camera.Camera;

            Data.Sky.Draw(camera);

            Data.PlayingField.Draw(camera);
            _data.PlayerSerpent.Draw(gameTime);
            foreach (var enemy in _data.Enemies)
                enemy.Draw(gameTime);
            //WaterFactory.DrawWaterSurfaceGrid(_water, camera, null, 0);
             base.Draw(gameTime);
            _billboardText.Draw(
                camera,
                _data.PlayerSerpent.GetPosition() - new Vector3(0, -1.5f, 0),
                _data.Enemies[0].GetPosition() + Vector3.Up
                );
        }
    }
}
