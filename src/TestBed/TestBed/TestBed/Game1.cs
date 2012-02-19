using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using factor10.VisionThing;
using factor10.VisionThing.Primitives;
using factor10.VisionThing.StockEffects;
using factor10.VisionThing.Water;
using BasicEffect = Microsoft.Xna.Framework.Graphics.BasicEffect;
using IDrawable = factor10.VisionThing.ClipDrawable;

namespace TestBed
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        private Camera _camera;
        private WaterSurface _water;
        private Pillar _pillar1, _pillar2;
        private Ship _ship;

        private BasicEffect _basicEffect;
        private SpriteBatch _spriteBatch;

        private SkySphere _sky1;
 
        private RenderTarget2D _testTarget;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 800;
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
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            VisionContent.Init(this);
            _camera = new Camera(Window.ClientBounds, new Vector3(0, 4, -20), Vector3.Up);
            _water = WaterFactory.Create(GraphicsDevice);
            _pillar1 = new Pillar(VisionContent.Load<Effect>(@"effects\lightingeffect"), Matrix.CreateRotationX(MathHelper.PiOver4) * Matrix.CreateTranslation(10, 3, 10));
            _pillar2 = new Pillar(VisionContent.Load<Effect>(@"effects\lightingeffect"), Matrix.CreateRotationZ(MathHelper.PiOver4) * Matrix.CreateTranslation(20, 5, 20));
            _sky1 = new SkySphere(VisionContent.Load<TextureCube>(@"textures\clouds"));
            _ship = new Ship();
            //_water.ReflectedObjects.Add(_sky1);
            _water.ReflectedObjects.Add(_pillar1);
            _water.ReflectedObjects.Add(_pillar2);
            _water.ReflectedObjects.Add(_ship);
           _basicEffect = new BasicEffect(GraphicsDevice);
            _camera.HandleMouse(0, 0);

            _testTarget = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24);
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            _camera.UpdateFreeFlyingCamera(gameTime);
            _water.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _water.RenderReflection(_camera);

             //GraphicsDevice.Clear(Color.CornflowerBlue);
             _sky1.Draw(_camera);

            _water.Draw(_camera, Matrix.Identity, false);
            _pillar1.Draw(_camera);
            _pillar2.Draw(_camera);
            _ship.Draw(_camera);
           zzz();
            base.Draw(gameTime);
        }

        private void zzz()
        {
            _basicEffect.World = Matrix.Identity; //Matrix.CreateConstrainedBillboard(textPosition, textPosition - camera.Front, Vector3.Down, null, null);
            _basicEffect.View = Matrix.Identity;
            _basicEffect.Projection = Matrix.Identity;

            //_basicEffect.TextureEnabled = true;
            //_basicEffect.VertexColorEnabled = false;
            _basicEffect.CurrentTechnique.Passes[0].Apply();
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.LinearClamp, DepthStencilState.Default,
                RasterizerState.CullNone);

            _spriteBatch.Draw(_water._reflectionTarget, new Rectangle(0,0,(int)_camera.ClientSize.X/4,(int)_camera.ClientSize.Y/4 ), Color.White);
            _spriteBatch.End();

            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

            //_basicEffect.World = Matrix.Identity;
            //_basicEffect.TextureEnabled = false;
            //_basicEffect.VertexColorEnabled = true;
            //_basicEffect.CurrentTechnique.Passes[0].Apply();
            //drawArc(textPosition, zzz);
        }
    }
}
