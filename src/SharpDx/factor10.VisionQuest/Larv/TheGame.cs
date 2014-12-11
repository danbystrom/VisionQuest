using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using factor10.VisionThing;
using factor10.VisionThing.Util;
using Larv.GameStates;
using Larv.Serpent;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
using System.Text;
using Keys = SharpDX.Toolkit.Input.Keys;

namespace Larv
{
    // Use these namespaces here to override SharpDX.Direct3D11
    
    /// <summary>
    /// Simple TheGame game using SharpDX.Toolkit.
    /// </summary>
    public class TheGame : Game
    {
        private GraphicsDeviceManager _graphicsDeviceManager;

        private Windmill _windmill;

        //public Data Data;

        //private float _angleXZ = 0;
        //private float _angleY = 0;

        //public IGeometricPrimitive _sphere;
        //public IGeometricPrimitive _cylinder;
        //public VisionEffect _myEffect;
        //public VisionEffect _myBumpEffect;
        //private RasterizerState _rasterizerState;

        //private Texture2D _snakeBump;
        //private Texture2D _image1;

        private IGameState _gameState;
        private LContent _lcontent;
        private Serpents _serpents;

        private readonly FramesPerSecondCounter _fps = new FramesPerSecondCounter();

        /// <summary>
        /// Initializes a new instance of the <see cref="TheGame" /> class.
        /// </summary>
        public TheGame()
        {
            //VisionContent.SunlightDirection = new Vector3(11f, -7f, 2f);

            // Creates a graphics manager. This is mandatory.
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

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            throw new NotImplementedException();
        }

        protected override void Initialize()
        {
            // Modify the title of the window
            Window.Title = "Larv! by Dan Byström - factor10 Solutions";
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Instantiate a SpriteBatch
            //_spriteBatch = ToDisposeContent(new SpriteBatch(GraphicsDevice));

            //_sphere = new SpherePrimitive<VertexPositionNormalTangentTexture>(GraphicsDevice, (p, n, t, tx) => new VertexPositionNormalTangentTexture(p, n, t, tx), 2);
            //_cylinder = new CylinderPrimitive<VertexPositionNormalTangentTexture>(GraphicsDevice, (p, n, t, tx) => new VertexPositionNormalTangentTexture(p, n, t, tx), 1, 1, 10);
            //_myEffect = new VisionEffect(Content.Load<Effect>(@"Effects\SimpleTextureEffect"));
            //_myBumpEffect = new VisionEffect(Content.Load<Effect>(@"Effects\SimpleBumpEffect"));
            //_image1 = Content.Load<Texture2D>("textures/rocknormal");
            //_snakeBump = Content.Load<Texture2D>("textures/snakeskinmap");

            //Data = new Data(this, new KeyboardManager(this), new MouseManager(this), new PointerManager(this));

            _lcontent = new LContent(GraphicsDevice, Content);

            var camera = new Camera(
                _lcontent.ClientSize,
                new KeyboardManager(this),
                new MouseManager(this),
                new PointerManager(this),
                AttractState.CameraPosition,
                AttractState.CameraLookAt) { MovingSpeed = 8 };
            _serpents = new Serpents(_lcontent, camera, _lcontent.Sphere, 0);

            _lcontent.Ground.GeneratePlayingField(_serpents.PlayingField);
            _lcontent.ShadowMap.ShadowCastingObjects.Add(_serpents);

            _windmill = new Windmill(_lcontent, Vector3.Zero);

            _gameState = new AttractState(_serpents);

            _lcontent.ShadowMap.ShadowCastingObjects.Add(_windmill);

            base.LoadContent();
        }


        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _serpents.Camera.UpdateInputDevices();
            _lcontent.Ground.Update(_serpents.Camera, gameTime);

            _fps.Update(gameTime);
            _windmill.Update(_serpents.Camera, gameTime);
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

            // Use time in seconds directly
            var time = (float) gameTime.TotalGameTime.TotalSeconds;

            GraphicsDevice.Clear(Color.CornflowerBlue);
            //GraphicsDevice.SetRasterizerState(_rasterizerState);

            _windmill.Draw(_serpents.Camera, DrawingReason.Normal, _lcontent.ShadowMap);
            _gameState.Draw(_serpents.Camera, DrawingReason.Normal, _lcontent.ShadowMap);

            // ------------------------------------------------------------------------
            // Draw the some 2d text
            // ------------------------------------------------------------------------

            var text = new StringBuilder();
            text.AppendFormat("FPS: {0}  GameState: {1}", _fps.FrameRate, _gameState.GetType()).AppendLine();
            _lcontent.SpriteBatch.Begin();
            _lcontent.SpriteBatch.DrawString(_lcontent.Font, text.ToString(), Vector2.Zero, Color.White);
            _lcontent.SpriteBatch.End();

            //_spriteBatch.Begin(SpriteSortMode.Deferred, GraphicsDevice.BlendStates.Default);
            //var shx = GraphicsDevice.BackBuffer.Width - Data.ShadowMap.ShadowDepthTarget.Width*0.25f - 4;
            //var shy = 4;
            //_spriteBatch.Draw(
            //    Data.ShadowMap.ShadowDepthTarget,
            //    new Vector2(shx, shy),
            //    new Rectangle(0, 0, Data.ShadowMap.ShadowDepthTarget.Width, Data.ShadowMap.ShadowDepthTarget.Height),
            //    Color.White,
            //    0.0f,
            //    new Vector2(0, 0),
            //    new Vector2(0.2f, 0.25f),
            //    SpriteEffects.None,
            //    0f);
            //_spriteBatch.End();

            base.Draw(gameTime);
        }

    }

}
