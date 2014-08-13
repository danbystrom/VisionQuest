﻿using System;
using System.Text;
using System.Xml;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;
using Serpent;
using Serpent.Serpent;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Toolkit.Content;


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

        private Matrix view;
        private Matrix projection;

        private Model model;

        //private Effect bloomEffect;
        //private RenderTarget2D renderTargetOffScreen;
        //private RenderTarget2D[] renderTargetDownScales;
        //private RenderTarget2D renderTargetBlurTemp;

        private BasicEffect basicEffect;
        private GeometricPrimitive primitive;

        private KeyboardManager keyboard;
        private KeyboardState keyboardState;

        private MouseManager mouse;
        private MouseState mouseState;

        private PointerManager pointer;
        private PointerState pointerState;

        public Data Data;

        public IGeometricPrimitive _cylinder;
        public PlainEffectWrapper _myEffect;

        /// <summary>
        /// Initializes a new instance of the <see cref="NextGame" /> class.
        /// </summary>
        public NextGame()
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            graphicsDeviceManager.DeviceCreationFlags = DeviceCreationFlags.Debug;

            //graphicsDeviceManager.IsFullScreen = true;
            graphicsDeviceManager.PreferredBackBufferWidth = 1920;
            graphicsDeviceManager.PreferredBackBufferHeight= 1080;

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";

            // Initialize input keyboard system
            keyboard = new KeyboardManager(this);

            // Initialize input mouse system
            mouse = new MouseManager(this);

            // Initialize input pointer system
            pointer = new PointerManager(this);

        }

        protected override void Initialize()
        {
            // Modify the title of the window
            Window.Title = "NextGame";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Instantiate a SpriteBatch
            spriteBatch = ToDisposeContent(new SpriteBatch(GraphicsDevice));

            ballsTexture = Content.Load<Texture2D>("Balls");

            _cylinder = new CubePrimitive<VertexPositionNormalTexture>(GraphicsDevice, (p, n, t) => new VertexPositionNormalTexture(p, n, t * 2), 2);
            _myEffect = new PlainEffectWrapper(Content.Load<Effect>(@"Effects\SimpleTextureEffect"));
            _myEffect.Texture = Content.Load<Texture2D>(@"Textures\woodfloor");

            Data = new Data(this, keyboard, mouse);

            arial16Font = Content.Load<SpriteFont>("Arial16");

            model = Content.Load<Model>("Ship");

            BasicEffect.EnableDefaultLighting(model, true);

            basicEffect = ToDisposeContent(new BasicEffect(GraphicsDevice));
            basicEffect.PreferPerPixelLighting = true;
            basicEffect.EnableDefaultLighting();

            // Creates torus primitive
            primitive = ToDisposeContent(GeometricPrimitive.Torus.New(GraphicsDevice));

            _rasterizerState = RasterizerState.New(GraphicsDevice, new RasterizerStateDescription
            {
                FillMode = FillMode.Solid,
                CullMode = CullMode.None,
                IsFrontCounterClockwise = false,
                DepthBias = 0,
                SlopeScaledDepthBias = 0.0f,
                DepthBiasClamp = 0.0f,
                IsDepthClipEnabled = true,
                IsScissorEnabled = false,
                IsMultisampleEnabled = false,
                IsAntialiasedLineEnabled = false
            });

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Calculates the world and the view based on the model size
            view = Matrix.LookAtLH(new Vector3(0.0f, 0.0f, -7.0f), new Vector3(0, 0.0f, 0), Vector3.UnitY);
            projection = Matrix.PerspectiveFovLH(0.9f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);

            // Update basic effect for rendering the Primitive
            basicEffect.View = view;
            basicEffect.Projection = projection;

            // Get the current state of the keyboard
            keyboardState = keyboard.GetState();

            // Get the current state of the mouse
            mouseState = mouse.GetState();

            // Get the current state of the pointer
            pointerState = pointer.GetState();

            Data.UpdateKeyboard();

            var camera = Data.PlayerSerpent.Camera;

            if (Data.KeyboardState.IsKeyDown(Keys.Escape))
                Exit();

            //if (Data.HasKeyToggled(Keys.Enter) && Data.KeyboardState.IsKeyDown(Keys.LeftAlt))
            //{
            //    _graphics.IsFullScreen ^= true;
            //    _graphics.ApplyChanges();
            //}

            if (Data.HasKeyToggled(Keys.C))
                Data.PlayerSerpent.Camera.CameraBehavior = camera.CameraBehavior == CameraBehavior.FollowTarget
                    ? CameraBehavior.Static
                    : CameraBehavior.FollowTarget;
            if (Data.HasKeyToggled(Keys.F))
                Data.PlayerSerpent.Camera.CameraBehavior = camera.CameraBehavior == CameraBehavior.FollowTarget
                    ? CameraBehavior.FreeFlying
                    : CameraBehavior.FollowTarget;
            if (Data.HasKeyToggled(Keys.P))
                _paused ^= true;

            if (Data.HasKeyToggled(Keys.D1))
                Data.PlayerSerpent.Speed *= 2;
            if (Data.HasKeyToggled(Keys.D2))
                Data.PlayerSerpent.Speed /= 2;

            if (_paused)
            {
                Data.PlayerSerpent.UpdateCameraOnly(gameTime);
                return;
            }

            Data.PlayerSerpent.Update(gameTime);
            foreach (var enemy in Data.Enemies)
            {
                enemy.Update(gameTime);
                if (enemy.EatAt(Data.PlayerSerpent))
                    /*startGame()*/;
                else if (enemy.SerpentStatus == SerpentStatus.Alive && Data.PlayerSerpent.EatAt(enemy))
                    enemy.SerpentStatus = SerpentStatus.Ghost;
            }
            Data.Enemies.RemoveAll(e => e.SerpentStatus == SerpentStatus.Finished);
            //if (Data.Enemies.Count == 0)
            //    startGame();
        }

        private bool _paused;

        private RasterizerState _rasterizerState;

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRasterizerState(_rasterizerState);

            // Use time in seconds directly
            var time = (float)gameTime.TotalGameTime.TotalSeconds;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Constant used to translate 3d models
            float translateX = 0.0f;

            Data.Sky.Draw(Data.PlayerSerpent.Camera.Camera);

            // ------------------------------------------------------------------------
            // Draw the 3d primitive using BasicEffect
            // ------------------------------------------------------------------------
            basicEffect.World = Matrix.Scaling(2.0f, 2.0f, 2.0f) *
                                Matrix.RotationX(0.8f * (float)Math.Sin(time * 1.45)) *
                                Matrix.RotationY(time * 2.0f) *
                                Matrix.RotationZ(0) *
                                Matrix.Translation(translateX, -1.0f, 0);
            primitive.Draw(basicEffect);

            _myEffect.World = Matrix.Scaling(2.0f, 2.0f, 2.0f) *
                                Matrix.RotationX(0.8f * (float)Math.Sin(time * 1.45)) *
                                Matrix.RotationY(time * 2.0f) *
                                Matrix.RotationZ(0) *
                                Matrix.Translation(-5, -1.0f, 0);
            _myEffect.View = Data.PlayerSerpent.Camera.Camera.View;
            _myEffect.Projection = Data.PlayerSerpent.Camera.Camera.Projection;
            _cylinder.Draw(_myEffect);

            Data.PlayingField.Draw(Data.PlayerSerpent.Camera.Camera);
            Data.PlayerSerpent.Draw(gameTime);
            foreach (var enemy in Data.Enemies)
                enemy.Draw(gameTime);

             // ------------------------------------------------------------------------
            // Draw the some 2d text
            // ------------------------------------------------------------------------
            spriteBatch.Begin();
            var text = new StringBuilder("This text is displayed with SpriteBatch").AppendLine();

            {
                var c = Data.PlayerSerpent.Camera.Camera;
                text.AppendFormat("Camera Yaw/Pitch: {0:0.000}/{1:0.000}  Forward: {2:0.000},{3:0.000},{4:0.000}",
                    c.Yaw, c.Pitch, c.Forward.X, c.Forward.Y, c.Forward.Z).AppendLine();
            }

            {
                var w = Data.PlayerSerpent._whereabouts;
                var cl = w.Location;
                var nl = w.NextLocation;
                text.AppendFormat("Whereabouts: ({0},{1}) ({2}/{3}) {4:0.00}",
                    cl.X, cl.Y, nl.X, nl.Y, w.Fraction).AppendLine();
            }

            // Display pressed keys
            var pressedKeys = keyboardState.GetPressedKeys();
            text.Append("Key Pressed: [");
            foreach (var key in pressedKeys)
            {
                text.Append(key.ToString());
                text.Append(" ");
            }
            text.Append("]").AppendLine();

            // Display mouse coordinates and mouse button status
            text.AppendFormat("Mouse ({0},{1}) Left: {2}, Right {3}", mouseState.X, mouseState.Y, mouseState.Left, mouseState.Right).AppendLine();

            var points = pointerState.Points;
            foreach (var point in points)
                text.AppendFormat("Pointer event: [{0}] {1} {2} ({3}, {4})", point.PointerId, point.DeviceType, point.EventType, point.Position.X, point.Position.Y).AppendLine();

            spriteBatch.DrawString(arial16Font, text.ToString(), new Vector2(16, 16), Color.White);
            spriteBatch.End();

            // ------------------------------------------------------------------------
            // Use SpriteBatch to draw some balls on the screen using NonPremultiplied mode
            // as the sprite texture used is not premultiplied
            // ------------------------------------------------------------------------
            spriteBatch.Begin(SpriteSortMode.Deferred, GraphicsDevice.BlendStates.NonPremultiplied);
            for (int i = 0; i < 40; i++)
            {
                var posX = (float)Math.Cos(time * 4.5f + i * 0.1f) * 60.0f + 136.0f;
                var posY = GraphicsDevice.BackBuffer.Height * 2.0f / 3.0f + 100.0f * (float)Math.Sin(time * 10.0f + i * 0.4f);
 
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
