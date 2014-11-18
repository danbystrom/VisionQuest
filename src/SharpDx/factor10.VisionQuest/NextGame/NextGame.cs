using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;
using NextGame.GameStates;
using Serpent;
using SharpDX;
using SharpDX.Direct3D11;

namespace NextGame
{
    // Use these namespaces here to override SharpDX.Direct3D11
    using SharpDX.Toolkit;
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;

    /// <summary>
    /// Simple NextGame game using SharpDX.Toolkit.
    /// </summary>
    public class NextGame : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        private SpriteBatch spriteBatch;
        private Texture2D ballsTexture;
        private SpriteFont arial16Font;

        private Model _model;
        private Texture2D _snakeSkin;

        //private Effect bloomEffect;
        //private RenderTarget2D renderTargetOffScreen;
        //private RenderTarget2D[] renderTargetDownScales;
        //private RenderTarget2D renderTargetBlurTemp;

        private VBasicEffect basicEffect;
        private GeometricPrimitive primitive;

        public Data Data;

        public IGeometricPrimitive _sphere;
        public VisionEffect _myEffect;
        private RasterizerState _rasterizerState;

        private IGameState _gameState;

        /// <summary>
        /// Initializes a new instance of the <see cref="NextGame" /> class.
        /// </summary>
        public NextGame()
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            graphicsDeviceManager.DeviceCreationFlags = DeviceCreationFlags.Debug;
            
            //var screen = Screen.AllScreens.First(_ => _.Primary);
            //graphicsDeviceManager.IsFullScreen = true;
            //graphicsDeviceManager.PreferredBackBufferWidth = screen.Bounds.Width;
            //graphicsDeviceManager.PreferredBackBufferHeight = screen.Bounds.Height;
            graphicsDeviceManager.PreferredBackBufferWidth = 1920;
            graphicsDeviceManager.PreferredBackBufferHeight = 1080;

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // Modify the title of the window
            Window.Title = "NextGame";
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Instantiate a SpriteBatch
            spriteBatch = ToDisposeContent(new SpriteBatch(GraphicsDevice));

            ballsTexture = Content.Load<Texture2D>("Balls");
            _snakeSkin = Content.Load<Texture2D>(@"Textures\sn");

            _sphere = new SpherePrimitive<VertexPositionNormalTexture>(GraphicsDevice, (p, n, t) => new VertexPositionNormalTexture(p, n, t), 2);
            _myEffect = new VisionEffect(Content.Load<Effect>(@"Effects\SimpleTextureEffect"));

            Data = new Data(this, new KeyboardManager(this), new MouseManager(this), new PointerManager(this));

            arial16Font = Content.Load<SpriteFont>("Arial16");

            _model = Content.Load<Model>("Models/GalleonModel");
            BasicEffect.EnableDefaultLighting(_model, true);

            basicEffect = ToDisposeContent(new VBasicEffect(GraphicsDevice));
            basicEffect.PreferPerPixelLighting = true;
            basicEffect.EnableDefaultLighting();
            basicEffect.Texture = _snakeSkin;

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

            _gameState = new PlayingState(Data.Serpents);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Data.Update(gameTime);
            _gameState.Update(gameTime, ref _gameState);

            if (Data.Serpents.PlayerSerpent.Camera.Camera.KeyboardState.IsKeyDown(Keys.Escape))
                Exit();
        }

        protected override void Draw(GameTime gameTime)
        {
            // Use time in seconds directly
            var time = (float) gameTime.TotalGameTime.TotalSeconds;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Constant used to translate 3d models
            float translateX = 0.0f;

            _model.Draw(GraphicsDevice, Matrix.Translation(15, 0, 5)*Matrix.RotationZ(MathUtil.Pi)*Matrix.Scaling(0.2f),
                Data.Serpents.PlayerSerpent.Camera.Camera.View, Data.Serpents.PlayerSerpent.Camera.Camera.Projection);

            // ------------------------------------------------------------------------
            // Draw the 3d primitive using BasicEffect
            // ------------------------------------------------------------------------
            basicEffect.View = Data.Serpents.PlayerSerpent.Camera.Camera.View;
            basicEffect.Projection = Data.Serpents.PlayerSerpent.Camera.Camera.Projection;

            basicEffect.World = Matrix.Scaling(3.0f, 3.0f, 3.0f)*
                                Matrix.RotationX(0.8f*(float) Math.Sin(time*1.45))*
                                Matrix.RotationY(time*2.0f)*
                                Matrix.RotationZ(0)*
                                Matrix.Translation(translateX, -1.0f, 0);
            basicEffect.Texture = _snakeSkin;
            basicEffect.TextureEnabled = true;
            primitive.Draw(basicEffect);

            basicEffect.World = Matrix.Scaling(2.0f, 2.0f, 2.0f)*
                                Matrix.RotationX(0.8f*(float) Math.Sin(time*1.45))*
                                Matrix.RotationY(time*2.0f)*
                                Matrix.RotationZ(0)*
                                Matrix.Translation(-1, -1.0f, -15);
            basicEffect.Texture = _snakeSkin;
            _sphere.Draw(basicEffect);

            //Data.Serpents.Draw(gameTime);
            _gameState.Draw(gameTime);

            _myEffect.World = Matrix.Scaling(2.0f, 2.0f, 2.0f)*
                              Matrix.RotationX(0.8f*(float) Math.Sin(time*1.45))*
                              Matrix.RotationY(time*2.0f)*
                              Matrix.RotationZ(0)*
                              Matrix.Translation(-1, -1.0f, -10);
            _myEffect.View = Data.Serpents.PlayerSerpent.Camera.Camera.View;
            _myEffect.Projection = Data.Serpents.PlayerSerpent.Camera.Camera.Projection;
            _myEffect.Texture = _snakeSkin;
            _myEffect.DiffuseColor = new Vector4(1, 1, 1, 0.2f);
            GraphicsDevice.SetBlendState(GraphicsDevice.BlendStates.AlphaBlend);
            _sphere.Draw(_myEffect);
            GraphicsDevice.SetBlendState(GraphicsDevice.BlendStates.Default);

            _myEffect.World = Matrix.Scaling(2) * Data.WorldPicked;
            _sphere.Draw(_myEffect);


            // ------------------------------------------------------------------------
            // Draw the some 2d text
            // ------------------------------------------------------------------------
            spriteBatch.Begin();
            var text = new StringBuilder("This text is displayed with SpriteBatch").AppendLine();

            {
                var c = Data.Serpents.PlayerSerpent.Camera.Camera;
                text.AppendFormat("Camera Yaw/Pitch: {0:0.000}/{1:0.000}  Forward: {2:0.000},{3:0.000},{4:0.000}",
                    c.Yaw, c.Pitch, c.Forward.X, c.Forward.Y, c.Forward.Z).AppendLine();
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
            Data.Serpents.PlayerSerpent.Camera.Camera.KeyboardState.GetDownKeys(pressedKeys);
            text.Append("Key Pressed: [");
            foreach (var key in pressedKeys)
            {
                text.Append(key.ToString());
                text.Append(" ");
            }
            text.Append("]").AppendLine();

            var mouseState = Data.Serpents.PlayerSerpent.Camera.Camera.MouseState;
            // Display mouse coordinates and mouse button status
            text.AppendFormat("Mouse ({0},{1}) Left: {2}, Right {3}", mouseState.X, mouseState.Y, mouseState.LeftButton, mouseState.RightButton).AppendLine();

            var pointerState = Data.Serpents.PlayerSerpent.Camera.Camera.PointerManager.GetState();
            var points = pointerState.Points;
            foreach (var point in points)
                text.AppendFormat("Pointer event: [{0}] {1} {2} ({3}, {4})", point.PointerId, point.DeviceType, point.EventType, point.Position.X,
                    point.Position.Y).AppendLine();

            spriteBatch.DrawString(arial16Font, text.ToString(), new Vector2(16, 16), Color.White);
            spriteBatch.End();

            // ------------------------------------------------------------------------
            // Use SpriteBatch to draw some balls on the screen using NonPremultiplied mode
            // as the sprite texture used is not premultiplied
            // ------------------------------------------------------------------------
            spriteBatch.Begin(SpriteSortMode.Deferred, GraphicsDevice.BlendStates.NonPremultiplied);
            for (int i = 0; i < 40; i++)
            {
                var posX = (float) Math.Cos(time*4.5f + i*0.1f)*60.0f + 136.0f;
                var posY = GraphicsDevice.BackBuffer.Height*2.0f/3.0f + 100.0f*(float) Math.Sin(time*10.0f + i*0.4f);

                spriteBatch.Draw(
                    ballsTexture,
                    new Vector2(posX, posY),
                    new Rectangle(0, 0, 32, 32),
                    Color.White,
                    0.0f,
                    new Vector2(16, 16),
                    Vector2.One,
                    SpriteEffects.None,
                    0f);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }

}
