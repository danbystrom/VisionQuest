using System;
using System.Collections.Generic;
using System.Text;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;
using SharpDX;
using factor10.VisionThing;

namespace DxExempel1
{
    // Use these namespaces here to override SharpDX.Direct3D11
    using SharpDX.Toolkit;
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;

    /// <summary>
    /// Simple DxExempel1 game using SharpDX.Toolkit.
    /// </summary>
    public class DxExempel1 : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        private SpriteBatch spriteBatch;
        //private SpriteFont arial16Font;

        private Matrix view;
        private Matrix projection;

        private IVEffect basicEffect;
        private IVDrawable primitive;

        private KeyboardManager keyboard;
        private KeyboardState keyboardState;

        private bool _wireframe = true;
        private bool _lighting = false;
        private bool _texture = false;

        private Texture2D _bumpMap;
        private bool _bumMapping;

        /// <summary>
        /// Initializes a new instance of the <see cref="DxExempel1" /> class.
        /// </summary>
        public DxExempel1()
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
            Window.Title = "DxExempel1";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Instantiate a SpriteBatch
            spriteBatch = ToDisposeContent(new SpriteBatch(GraphicsDevice));

            // Loads a sprite font
            // The [Arial16.xml] file is defined with the build action [ToolkitFont] in the project
            //arial16Font = Content.Load<SpriteFont>("Arial16");

            // Creates a basic effect
            //basicEffect = ToDisposeContent(new VBasicEffect(GraphicsDevice));
            var vContent = new VisionContent(GraphicsDevice, Content);
            //basicEffect = ToDisposeContent(new VBasicEffect(GraphicsDevice));
            basicEffect = vContent.LoadPlainEffect("exempeleffect");
            basicEffect.SunlightDirection = new Vector3(1, 0, 1);
            basicEffect.Texture = Content.Load<Texture2D>("textures/brick_texture_map");
            _bumpMap = Content.Load<Texture2D>("textures/brick_normal_map");

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
            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.CameraPosition = new Vector3(0.0f, 0.0f, 7.0f);

            // Get the current state of the keyboard
            keyboardState = keyboard.GetState();

            if (keyboardState.IsKeyPressed(Keys.D1))
                _wireframe ^= true;
            if (keyboardState.IsKeyPressed(Keys.D2))
                ((BasicEffect) basicEffect).LightingEnabled ^= true;
            if (keyboardState.IsKeyPressed(Keys.D3))
                ((BasicEffect)basicEffect).TextureEnabled ^= true;
            if (keyboardState.IsKeyPressed(Keys.D4))
                basicEffect.Parameters["BumpMap"].SetResource((_bumMapping ^= true) ? _bumpMap : null);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Use time in seconds directly
            var time = (float)gameTime.TotalGameTime.TotalSeconds;

            // Clears the screen with the Color.CornflowerBlue
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SetRasterizerState(_wireframe ? GraphicsDevice.RasterizerStates.WireFrame : GraphicsDevice.RasterizerStates.Default);

            // Constant used to translate 3d models
            float translateX = 0.0f;

            // ------------------------------------------------------------------------
            // Draw the 3d primitive using BasicEffect
            // ------------------------------------------------------------------------
            basicEffect.World = Matrix.Scaling(2.0f, 2.0f, 2.0f) *
                                Matrix.RotationX(0.8f * (float)Math.Sin(time * 1.45)) *
                                Matrix.RotationY(time * 2.0f) *
                                Matrix.RotationZ(0) *
                                Matrix.Translation(translateX, -1.0f, 0);
            primitive.Draw(basicEffect);

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
