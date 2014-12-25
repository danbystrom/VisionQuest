using System.Linq;
using factor10.VisionThing;
using factor10.VisionThing.CameraStuff;
using factor10.VisionThing.Util;
using Larv.GameStates;
using Larv.Serpent;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Color = SharpDX.Color;
using Keys = SharpDX.Toolkit.Input.Keys;

namespace Larv
{
    public class LarvGame : Game
    {
        private GraphicsDeviceManager _graphicsDeviceManager;

        private IGameState _gameState;
        private LContent _lcontent;
        private Serpents _serpents;

        private readonly FramesPerSecondCounter _fps = new FramesPerSecondCounter();

        public LarvGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            //_graphicsDeviceManager.DeviceCreationFlags = DeviceCreationFlags.Debug;
            //_graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;            
#if false
            var screen = Screen.AllScreens.First(_ => !_.Primary);
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
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Larv.app.ico"))
                ((Form)Window.NativeWindow).Icon = new Icon(stream);
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _lcontent = new LContent(GraphicsDevice, Content);

            var shadowCameraPos = new Vector3(12, 4, 12) - VisionContent.SunlightDirection * 32;
            var shadowCameraLookAt = shadowCameraPos + VisionContent.SunlightDirection;
            _lcontent.ShadowMap.Camera.Update(shadowCameraPos, shadowCameraLookAt);

            var camera = new Camera(
                _lcontent.ClientSize,
                new KeyboardManager(this),
                new MouseManager(this),
                new PointerManager(this),
                AttractState.CameraPosition,
                AttractState.CameraLookAt) { MovingSpeed = 8 };
            _serpents = new Serpents(_lcontent, camera, 0);
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

            if (_serpents.Camera.KeyboardState.IsKeyDown(Keys.Escape))
                Exit();
        }

        protected override void Draw(GameTime gameTime)
        {
            _lcontent.ShadowMap.Draw(_serpents.Camera);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.SetRasterizerState(GraphicsDevice.RasterizerStates.CullNone);
            _gameState.Draw(_serpents.Camera, DrawingReason.Normal, _lcontent.ShadowMap);

            // ------------------------------------------------------------------------
            // Draw the some 2d text
            // ------------------------------------------------------------------------

            var text = new StringBuilder();
            text.AppendFormat("FPS: {0}", _fps.FrameRate);
            _lcontent.SpriteBatch.Begin();
            _lcontent.SpriteBatch.DrawString(_lcontent.Font, text.ToString(), Vector2.Zero, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            _lcontent.SpriteBatch.End();

            base.Draw(gameTime);
        }

    }

}
