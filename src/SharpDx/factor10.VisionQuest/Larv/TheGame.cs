using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using factor10.VisionThing;
using factor10.VisionThing.Util;
using Larv.Field;
using Larv.GameStates;
using Larv.Serpent;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using System.Text;
using Keys = SharpDX.Toolkit.Input.Keys;

namespace Larv
{
    public class TheGame : Game
    {
        private GraphicsDeviceManager _graphicsDeviceManager;

        private IGameState _gameState;
        private LContent _lcontent;
        private Serpents _serpents;

        private readonly FramesPerSecondCounter _fps = new FramesPerSecondCounter();

        public TheGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            _graphicsDeviceManager.DeviceCreationFlags = DeviceCreationFlags.Debug;
            _graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;            
#if false
            var screen = Screen.AllScreens.First(_ => _.Primary);
            _graphicsDeviceManager.IsFullScreen = true;
            _graphicsDeviceManager.PreferredBackBufferWidth = screen.Bounds.Width;
            _graphicsDeviceManager.PreferredBackBufferHeight = screen.Bounds.Height;
#else
            _graphicsDeviceManager.PreferredBackBufferWidth = 1920;
            _graphicsDeviceManager.PreferredBackBufferHeight = 1080;
#endif

             Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Window.Title = "Larv! by Dan Byström - factor10 Solutions";
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _lcontent = new LContent(GraphicsDevice, Content);

            var camera = new Camera(
                _lcontent.ClientSize,
                new KeyboardManager(this),
                new MouseManager(this),
                new PointerManager(this),
                AttractState.CameraPosition,
                AttractState.CameraLookAt) { MovingSpeed = 8 };
            _serpents = new Serpents(_lcontent, camera, _lcontent.Sphere, 0);

            _lcontent.ShadowMap.ShadowCastingObjects.Add(_serpents);

            _gameState = new AttractState(_serpents);

            base.LoadContent();
        }


        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _fps.Update(gameTime);
            _serpents.Camera.UpdateInputDevices();
            _lcontent.Ground.Update(_serpents.Camera, gameTime);
            _gameState.Update(_serpents.Camera, gameTime, ref _gameState);

            var shadowCameraPos = new Vector3(12, 4, 12) - VisionContent.SunlightDirection*32;
            _lcontent.ShadowMap.Camera.Update(
                shadowCameraPos,
                shadowCameraPos + VisionContent.SunlightDirection);

            if (_serpents.Camera.KeyboardState.IsKeyDown(Keys.Escape))
                Exit();
        }

        protected override void Draw(GameTime gameTime)
        {
            _lcontent.ShadowMap.Draw(_serpents.Camera);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _gameState.Draw(_serpents.Camera, DrawingReason.Normal, _lcontent.ShadowMap);

            // ------------------------------------------------------------------------
            // Draw the some 2d text
            // ------------------------------------------------------------------------

            var text = new StringBuilder();
            text.AppendFormat("FPS: {0}  GameState: {1}", _fps.FrameRate, _gameState.GetType()).AppendLine();
            _lcontent.SpriteBatch.Begin();
            _lcontent.SpriteBatch.DrawString(_lcontent.Font, text.ToString(), Vector2.Zero, Color.White);
            _lcontent.SpriteBatch.End();

            base.Draw(gameTime);
        }

    }

}
