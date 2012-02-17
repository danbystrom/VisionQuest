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
using IDrawable = factor10.VisionThing.IDrawable;

namespace TestBed
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Camera _camera;
        private WaterSurface _water;
        private CylinderPrimitive _cylinder;

        private Effect _effect;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
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
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            var visionContent = new VisionContent(this, "Content").LibContent;
            _camera = new Camera(Window.ClientBounds, new Vector3(0, 4, -20), Vector3.Up);
            _water = WaterFactory.Create(GraphicsDevice, visionContent);
            _cylinder = new CylinderPrimitive(GraphicsDevice, 10, 2, 5);
            //_cylinder.BasicEffect = new BasicEffect(GraphicsDevice);
            //_cylinder.BasicEffect.EnableDefaultLighting();
            _effect = visionContent.Load<Effect>(@"effects\lightingeffect");
            _water.ReflectedObjects.Add(new Tuple<IDrawable, Effect>(_cylinder, _effect));
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _water.PreDraw( _camera, gameTime );
            // TODO: Add your drawing code here
            _water.Draw(_camera, Matrix.Identity, false);
            _effect.Parameters["World"].SetValue(Matrix.CreateRotationX(MathHelper.PiOver4) * Matrix.CreateTranslation(10, 1, 10));
            _effect.Parameters["View"].SetValue(_camera.View);
            _effect.Parameters["Projection"].SetValue(_camera.Projection);
            _effect.Parameters["CameraPosition"].SetValue(_camera.Position);
            //_cylinder.Draw(
            //    ,
            //    _camera.View,_camera.Projection,
            //    Color.Turquoise);
            _cylinder.Draw(_effect);
            base.Draw(gameTime);
        }
    }
}
