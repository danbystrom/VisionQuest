﻿using System;
using System.Collections.Generic;
using System.Text;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace DxExempel2
{
    // Use these namespaces here to override SharpDX.Direct3D11
    
    /// <summary>
    /// Simple DxExempel2 game using SharpDX.Toolkit.
    /// </summary>
    public class DxExempel2 : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        private SpriteBatch spriteBatch;
        //private SpriteFont arial16Font;

        private Matrix view;
        private Matrix projection;

        private IVEffect _textureEffect;
        private IVEffect _exampleEffect;
        private IVDrawable primitive;

        private KeyboardManager keyboard;
        private KeyboardState keyboardState;

        private bool _wireframe = true;
        private bool _lighting = false;
        private bool _texture = false;
        private bool _useExampleEffect = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="DxExempel2" /> class.
        /// </summary>
        public DxExempel2()
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";

            // Initialize input keyboard system
            keyboard = new KeyboardManager(this);
        }

        protected override void Initialize()
        {
            // Modify the title of the window
            Window.Title = "DxExempel2";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Instantiate a SpriteBatch
            spriteBatch = ToDisposeContent(new SpriteBatch(GraphicsDevice));

            // Loads a sprite font
            // The [Arial16.xml] file is defined with the build action [ToolkitFont] in the project
            //arial16Font = Content.Load<SpriteFont>("Arial16");

            var lightVec = new Vector3(1, 0, 5);
            lightVec.Normalize();
            // Creates a basic effect
            //basicEffect = ToDisposeContent(new VBasicEffect(GraphicsDevice));
            var vContent = new VisionContent(GraphicsDevice, Content);
            //basicEffect = ToDisposeContent(new VBasicEffect(GraphicsDevice));
            //_textureEffect = vContent.LoadPlainEffect("effects/simpletextureeffect");
            _textureEffect = vContent.LoadPlainEffect("effects/simpletextureeffect");
            _exampleEffect = vContent.LoadPlainEffect("exempeleffect");
            _textureEffect.SunlightDirection = lightVec;
            _exampleEffect.SunlightDirection = lightVec;
            _textureEffect.Texture = Content.Load<Texture2D>("textures/brick_texture_map");
            _exampleEffect.Texture = Content.Load<Texture2D>("textures/brick_texture_map");
             //_textureEffect.Parameters["BumpMap"].SetResource(Content.Load<Texture2D>("textures/brick_normal_map"));
            _exampleEffect.Parameters["BumpMap"].SetResource(Content.Load<Texture2D>("textures/brick_normal_map"));

            //var be = (BasicEffect)basicEffect.Effect;
            //be.PreferPerPixelLighting = true;
            //be.EnableDefaultLighting();
            //be.LightingEnabled = _lighting;
 
            // Creates torus primitive
            primitive =
                ToDisposeContent(new SpherePrimitive<VertexPositionNormalTangentTexture>(GraphicsDevice,
                    (p, n, t, tx) => new VertexPositionNormalTangentTexture(p, n, t, tx), 2));

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Calculates the world and the view based on the model size
            view = Matrix.LookAtRH(new Vector3(0.0f, 0.0f, 7.0f), new Vector3(0, 0.0f, 0), Vector3.UnitY);
            projection = Matrix.PerspectiveFovRH(0.9f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);

            // Update basic effect for rendering the Primitive
            _textureEffect.View = view;
            _textureEffect.Projection = projection;
            _textureEffect.CameraPosition = new Vector3(0.0f, 0.0f, 70.0f);

            _exampleEffect.View = view;
            _exampleEffect.Projection = projection;
            _exampleEffect.CameraPosition = new Vector3(0.0f, 0.0f, 70.0f);

            // Get the current state of the keyboard
            keyboardState = keyboard.GetState();

            if (keyboardState.IsKeyPressed(Keys.D1))
                _wireframe ^= true;
        }

        protected override void Draw(GameTime gameTime)
        {
            // Use time in seconds directly
            var time = (float)gameTime.TotalGameTime.TotalSeconds;

            // Clears the screen with the Color.CornflowerBlue
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SetRasterizerState(_wireframe ? GraphicsDevice.RasterizerStates.WireFrame : GraphicsDevice.RasterizerStates.Default);

            _textureEffect.World = Matrix.Scaling(2.0f, 2.0f, 2.0f) *
                                Matrix.RotationX(0.8f * (float)Math.Sin(time * 1.45)) *
                                Matrix.RotationY(time * 2.0f) *
                                Matrix.RotationZ(0) *
                                Matrix.Translation(-2, 0, 0);
            primitive.Draw(_textureEffect);

            _exampleEffect.World = Matrix.Scaling(2.0f, 2.0f, 2.0f) *
                    Matrix.RotationX(0.8f * (float)Math.Sin(time * 1.45)) *
                    Matrix.RotationY(time * 2.0f) *
                    Matrix.RotationZ(0) *
                    Matrix.Translation(2.5f, 0, 0);
            primitive.Draw(_exampleEffect);

            // ------------------------------------------------------------------------
            // Draw the some 2d text
            // ------------------------------------------------------------------------
            spriteBatch.Begin();
            var text = new StringBuilder("This text is displayed with SpriteBatch").AppendLine();

            var pressedKeys = new List<Keys>();
            keyboardState.GetDownKeys(pressedKeys);
            text.Append("Key Pressed: [");
            foreach (var key in pressedKeys)
            {
                text.Append(key.ToString());
                text.Append(" ");
            }
            text.Append("]").AppendLine();

            //spriteBatch.DrawString(arial16Font, text.ToString(), new Vector2(16, 16), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
