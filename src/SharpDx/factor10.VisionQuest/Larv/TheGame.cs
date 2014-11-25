﻿using System;
using System.Collections.Generic;
using System.Text;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;
using factor10.VisionThing.Util;
using Larv.GameStates;
using Larv.Serpent;
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


        public Data Data;

        public IGeometricPrimitive _sphere;
        public VisionEffect _myEffect;
        public VisionEffect _myBumpEffect;
        private RasterizerState _rasterizerState;

        private Texture2D _snakeBump;
        private Texture2D _image1;

        private IGameState _gameState;

        private readonly FramesPerSecondCounter _fps = new FramesPerSecondCounter();

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

            _sphere = new SpherePrimitive<VertexPositionNormalTangentTexture>(GraphicsDevice, (p, n, t, tx) => new VertexPositionNormalTangentTexture(p, n, t, tx), 2);
            _myEffect = new VisionEffect(Content.Load<Effect>(@"Effects\SimpleTextureEffect"));
            _myBumpEffect = new VisionEffect(Content.Load<Effect>(@"Effects\SimpleBumpEffect"));
            _image1 = Content.Load<Texture2D>("textures/rocknormal");
            _snakeBump = Content.Load<Texture2D>("textures/snakeskinmap");

            Data = new Data(this, new KeyboardManager(this), new MouseManager(this), new PointerManager(this));

            _arial16Font = Content.Load<SpriteFont>("Arial16");

            _windmill = new Windmill(Data.VContent, Vector3.Zero);

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

            Data.ShadowMap.ShadowCastingObjects.Add(_windmill);
            Data.ShadowMap.ShadowCastingObjects.Add(Data.Ground);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _fps.Update(gameTime);
            _windmill.Update(Data.Serpents.Camera, gameTime);
            Data.Update(gameTime);
            _gameState.Update(Data.Serpents.Camera, gameTime, ref _gameState);

            var shadowCameraPos = new Vector3(12, 4, 12) - VisionContent.SunlightDirection*32;
            Data.ShadowMap.Camera.Update(
                shadowCameraPos,
                shadowCameraPos + VisionContent.SunlightDirection);

            if (Data.Serpents.Camera.KeyboardState.IsKeyDown(Keys.Escape))
                Exit();
        }

        protected override void Draw(GameTime gameTime)
        {
            Data.ShadowMap.Draw();

            // Use time in seconds directly
            var time = (float) gameTime.TotalGameTime.TotalSeconds;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _windmill.Draw(Data.Serpents.Camera, DrawingReason.Normal, Data.ShadowMap);
            _gameState.Draw(Data.Serpents.Camera, DrawingReason.Normal, Data.ShadowMap);

            _myBumpEffect.World = Matrix.Scaling(2.0f, 2.0f, 2.0f)*
                                  Matrix.RotationX(0.8f*(float) Math.Sin(time*1.45))*
                                  Matrix.RotationY(time*2.0f)*
                                  Matrix.RotationZ(0)*
                                  Matrix.Translation(Data.ShadowMap.Camera.Position);
            _myBumpEffect.View = Data.Serpents.Camera.View;
            _myBumpEffect.Projection = Data.Serpents.Camera.Projection;
            _myBumpEffect.Texture = _image1;
            _myBumpEffect.Parameters["BumpMap"].SetResource(_snakeBump);
            _myBumpEffect.DiffuseColor = new Vector4(1, 1, 1, 0.9f);
            GraphicsDevice.SetBlendState(GraphicsDevice.BlendStates.AlphaBlend);
            _sphere.Draw(_myBumpEffect);
            GraphicsDevice.SetBlendState(GraphicsDevice.BlendStates.Default);

            _myBumpEffect.World = Matrix.Scaling(0.5f, 0.5f, 0.5f) *
                      Matrix.Translation(Data.ShadowMap.Camera.Position + VisionContent.SunlightDirection * 3);
            _sphere.Draw(_myBumpEffect);

            _myEffect.World = Matrix.Scaling(0.2f) * Data.WorldPicked;
            _sphere.Draw(_myEffect);


            // ------------------------------------------------------------------------
            // Draw the some 2d text
            // ------------------------------------------------------------------------
            _spriteBatch.Begin();

            var text = new StringBuilder("This text is displayed with SpriteBatch").AppendLine();

            text.AppendFormat("FPS: {0}", _fps.FrameRate).AppendLine();

            {
                var c = Data.Serpents.Camera;
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

            //{
            //    text.AppendFormat("Frog + B: ({0})  ({1})  ({2})  ({3})",
            //        _frog.Position, Data.WorldPicked.TranslationVector, Data.PickedQueriedGroundHeight1, Data.PickedQueriedGroundHeight2).AppendLine();
            //}

            // Display pressed keys
            var pressedKeys = new List<Keys>();
            Data.Serpents.Camera.KeyboardState.GetDownKeys(pressedKeys);
            text.Append("Key Pressed: [");
            foreach (var key in pressedKeys)
                text.Append(key).Append(" ");
            text.Append("]").AppendLine();

            var mouseState = Data.Serpents.Camera.MouseState;
            // Display mouse coordinates and mouse button status
            text.AppendFormat("Mouse ({0},{1}) Left: {2}, Right {3}", mouseState.X, mouseState.Y, mouseState.LeftButton, mouseState.RightButton).AppendLine();

            var pointerState = Data.Serpents.Camera.PointerManager.GetState();
            var points = pointerState.Points;
            foreach (var point in points)
                text.AppendFormat("Pointer event: [{0}] {1} {2} ({3}, {4})", point.PointerId, point.DeviceType, point.EventType, point.Position.X,
                    point.Position.Y).AppendLine();

            _spriteBatch.DrawString(_arial16Font, text.ToString(), new Vector2(16, 16), Color.White);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Deferred, GraphicsDevice.BlendStates.Default);
            var shx = GraphicsDevice.BackBuffer.Width - Data.ShadowMap.ShadowDepthTarget.Width*0.25f - 4;
            var shy = 4;
            _spriteBatch.Draw(
                Data.ShadowMap.ShadowDepthTarget,
                new Vector2(shx, shy),
                new Rectangle(0, 0, Data.ShadowMap.ShadowDepthTarget.Width, Data.ShadowMap.ShadowDepthTarget.Height),
                Color.White,
                0.0f,
                new Vector2(0, 0),
                new Vector2(0.2f, 0.25f),
                SpriteEffects.None,
                0f);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }

}