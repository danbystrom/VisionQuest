using System;
using System.Collections.Generic;
using System.Text;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;
using Larv.GameStates;
using Serpent;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
using Keys = SharpDX.Toolkit.Input.Keys;
using RasterizerState = SharpDX.Toolkit.Graphics.RasterizerState;
using Texture2D = SharpDX.Toolkit.Graphics.Texture2D;

namespace Larv
{
    // Use these namespaces here to override SharpDX.Direct3D11
    
    /// <summary>
    /// Simple TheGame game using SharpDX.Toolkit.
    /// </summary>
    public class TheGame : Game
    {
        private GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;
        private Texture2D _ballsTexture;
        private SpriteFont _arial16Font;

        private Windmill _windmill;
        private Texture2D _snakeSkin;

        //private Effect bloomEffect;
        //private RenderTarget2D renderTargetOffScreen;
        //private RenderTarget2D[] renderTargetDownScales;
        //private RenderTarget2D renderTargetBlurTemp;

        private GeometricPrimitive primitive;

        public Data Data;

        public IGeometricPrimitive _sphere;
        public VisionEffect _myEffect;
        public VisionEffect _myBumpEffect;
        private RasterizerState _rasterizerState;

        private Texture2D _snakeBump;
        private Texture2D _image1;

        private IGameState _gameState;

        /// <summary>
        /// Initializes a new instance of the <see cref="TheGame" /> class.
        /// </summary>
        public TheGame()
        {
            // Creates a graphics manager. This is mandatory.
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            _graphicsDeviceManager.DeviceCreationFlags = DeviceCreationFlags.Debug;
            
            //var screen = Screen.AllScreens.First(_ => _.Primary);
            //graphicsDeviceManager.IsFullScreen = true;
            //graphicsDeviceManager.PreferredBackBufferWidth = screen.Bounds.Width;
            //graphicsDeviceManager.PreferredBackBufferHeight = screen.Bounds.Height;
            _graphicsDeviceManager.PreferredBackBufferWidth = 1920;
            _graphicsDeviceManager.PreferredBackBufferHeight = 1080;

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";

        }

        protected override void Initialize()
        {
            // Modify the title of the window
            Window.Title = "Larv by Dan Byström - factor10 Solutions";
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Instantiate a SpriteBatch
            _spriteBatch = ToDisposeContent(new SpriteBatch(GraphicsDevice));

            _ballsTexture = Content.Load<Texture2D>("Balls");
            _snakeSkin = Content.Load<Texture2D>(@"Textures\sn");

            _sphere = new SpherePrimitive<VertexPositionNormalTangentTexture>(GraphicsDevice, (p, n, t, tx) => new VertexPositionNormalTangentTexture(p, n, t, tx), 2);
            _myEffect = new VisionEffect(Content.Load<Effect>(@"Effects\SimpleTextureEffect"));
            _myBumpEffect = new VisionEffect(Content.Load<Effect>(@"Effects\SimpleBumpEffect"));
            _image1 = Content.Load<Texture2D>("textures/image1");
            _snakeBump = Content.Load<Texture2D>("textures/snakeskinmap");

            Data = new Data(this, new KeyboardManager(this), new MouseManager(this), new PointerManager(this));

            _arial16Font = Content.Load<SpriteFont>("Arial16");

            _windmill = new Windmill(Data.VContent, Vector3.Zero);
            //BasicEffect.EnableDefaultLighting(_windmill, true);

            // Creates torus primitive
            primitive = ToDisposeContent(GeometricPrimitive.Torus.New(GraphicsDevice, 1, 0.3f, 32, false));

            _rasterizerState = RasterizerState.New(GraphicsDevice, new RasterizerStateDescription
            {
                FillMode = FillMode.Solid,
                CullMode = CullMode.Back,
                IsFrontCounterClockwise = false,
                DepthBias = 0,
                SlopeScaledDepthBias = 0.0f,
                DepthBiasClamp = 0.0f,
                IsDepthClipEnabled = true,
                IsScissorEnabled = false,
                IsMultisampleEnabled = false,
                IsAntialiasedLineEnabled = false
            });

            _gameState = new AttractState(Data.Serpents);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _windmill.Update(Data.Serpents.SerpentCamera.Camera, gameTime);
            Data.Update(gameTime);
            _gameState.Update(gameTime, ref _gameState);
           
            if (Data.Serpents.SerpentCamera.Camera.KeyboardState.IsKeyDown(Keys.Escape))
                Exit();
        }

        protected override void Draw(GameTime gameTime)
        {
            {
                var camera = Data.Serpents.SerpentCamera.Camera;
                var lookingDirection = camera.Target - camera.Position;
                lookingDirection.Normalize();
                var lookAtFocus = camera.Position + 20 * lookingDirection;
                lookAtFocus.Y = 2;

                Data.ShadowMap.Camera.Update(
                    lookAtFocus - VisionContent.SunlightDirection * 20,
                    lookAtFocus);
                //Data.ShadowMap.Draw();
            }

            // Use time in seconds directly
            var time = (float) gameTime.TotalGameTime.TotalSeconds;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Constant used to translate 3d models
            float translateX = 0.0f;

            _windmill.Draw(Data.Serpents.SerpentCamera.Camera, DrawingReason.Normal, Data.ShadowMap);

            //Data.Serpents.Draw(gameTime);
            _gameState.Draw(gameTime);

            _myBumpEffect.World = Matrix.Scaling(2.0f, 2.0f, 2.0f)*
                              Matrix.RotationX(0.8f*(float) Math.Sin(time*1.45))*
                              Matrix.RotationY(time*2.0f)*
                              Matrix.RotationZ(0)*
                              Matrix.Translation(-1, -3.0f, -10);
            _myBumpEffect.View = Data.Serpents.SerpentCamera.Camera.View;
            _myBumpEffect.Projection = Data.Serpents.SerpentCamera.Camera.Projection;
            _myBumpEffect.Texture = _image1;
            _myBumpEffect.Parameters["BumpMap"].SetResource(_snakeBump);
            _myBumpEffect.DiffuseColor = new Vector4(1, 1, 1, 0.9f);
            GraphicsDevice.SetBlendState(GraphicsDevice.BlendStates.AlphaBlend);
            _sphere.Draw(_myBumpEffect);
            GraphicsDevice.SetBlendState(GraphicsDevice.BlendStates.Default);

            _myEffect.World = Matrix.Scaling(0.5f) * Data.WorldPicked;
            _sphere.Draw(_myEffect);


            // ------------------------------------------------------------------------
            // Draw the some 2d text
            // ------------------------------------------------------------------------
            _spriteBatch.Begin();

            var text = new StringBuilder("This text is displayed with SpriteBatch").AppendLine();

            text.AppendFormat("Slow: {0}", gameTime.IsRunningSlowly).AppendLine();

            {
                var c = Data.Serpents.SerpentCamera.Camera;
                text.AppendFormat("Camera Yaw/Pitch: {0:0.000}/{1:0.000}  Forward: {2}  Position/Target: {3}/{4}",
                    c.Yaw, c.Pitch, c.Forward, c.Position, c.Target).AppendLine();
            }

            {
                var w = Data.Serpents.PlayerSerpent._whereabouts;
                var cl = w.Location;
                var nl = w.NextLocation;
                text.AppendFormat("Whereabouts: ({0},{1}) ({2}/{3}) {4:0.00}",
                    cl.X, cl.Y, nl.X, nl.Y, w.Fraction).AppendLine();
            }

            // Display pressed keys
            var pressedKeys = new List<Keys>();
            Data.Serpents.SerpentCamera.Camera.KeyboardState.GetDownKeys(pressedKeys);
            text.Append("Key Pressed: [");
            foreach (var key in pressedKeys)
            {
                text.Append(key.ToString());
                text.Append(" ");
            }
            text.Append("]").AppendLine();

            var mouseState = Data.Serpents.SerpentCamera.Camera.MouseState;
            // Display mouse coordinates and mouse button status
            text.AppendFormat("Mouse ({0},{1}) Left: {2}, Right {3}", mouseState.X, mouseState.Y, mouseState.LeftButton, mouseState.RightButton).AppendLine();

            var pointerState = Data.Serpents.SerpentCamera.Camera.PointerManager.GetState();
            var points = pointerState.Points;
            foreach (var point in points)
                text.AppendFormat("Pointer event: [{0}] {1} {2} ({3}, {4})", point.PointerId, point.DeviceType, point.EventType, point.Position.X,
                    point.Position.Y).AppendLine();

            _spriteBatch.DrawString(_arial16Font, text.ToString(), new Vector2(16, 16), Color.White);
            _spriteBatch.End();

            // ------------------------------------------------------------------------
            // Use SpriteBatch to draw some balls on the screen using NonPremultiplied mode
            // as the sprite texture used is not premultiplied
            // ------------------------------------------------------------------------
            _spriteBatch.Begin(SpriteSortMode.Deferred, GraphicsDevice.BlendStates.NonPremultiplied);
            for (int i = 0; i < 40; i++)
            {
                var posX = (float) Math.Cos(time*4.5f + i*0.1f)*60.0f + 136.0f;
                var posY = GraphicsDevice.BackBuffer.Height*2.0f/3.0f + 100.0f*(float) Math.Sin(time*10.0f + i*0.4f);

                _spriteBatch.Draw(
                    _ballsTexture,
                    new Vector2(posX, posY),
                    new Rectangle(0, 0, 32, 32),
                    Color.White,
                    0.0f,
                    new Vector2(16, 16),
                    Vector2.One,
                    SpriteEffects.None,
                    0f);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }

}
