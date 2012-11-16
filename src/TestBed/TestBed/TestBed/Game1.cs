using System;
using System.Collections.Generic;
using System.Linq;
using LibNoise.Xna;
using LibNoise.Xna.Generator;
using LibNoise.Xna.Operator;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using factor10.VisionThing;
using factor10.VisionThing.Primitives;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Water;
using BasicEffect = Microsoft.Xna.Framework.Graphics.BasicEffect;
using IDrawable = factor10.VisionThing.ClipDrawable;

namespace TestBed
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>

    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;

        private Camera _camera;
        private WaterSurface _water;
        private ClipDrawableInstance _pillar1, _pillar2;
        private Box _box1, _box2;
        private Ship _ship;
        private Windmill _windmill;
        private Island _island;
        private Terrain _terrain;

        private BasicEffect _basicEffect;
        private SpriteBatch _spriteBatch;

        private SkySphere _sky1;

        private KeyboardState _kbd;
        private bool _controlCameraWithMouse = true;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 800;
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
            var lightingEffect = VisionContent.LoadPlainEffect(@"effects\lightingeffect");
            var lightingEffectTexture = VisionContent.LoadPlainEffect(@"effects\lightingeffecttexture");

            _camera = new Camera(Window.ClientBounds, new Vector3(0, 4, -20), Vector3.Up);
            _water = WaterFactory.Create(GraphicsDevice);

            _pillar1 = new ClipDrawableInstance(
                lightingEffect,
                new CylinderPrimitive<VertexPositionNormal>(GraphicsDevice, (a, b, c) => new VertexPositionNormal(a, b), 10, 2, 10),
                Matrix.CreateRotationX(MathHelper.PiOver4)*Matrix.CreateTranslation(28, 3, 15));
            _pillar2 = new ClipDrawableInstance(
                lightingEffect,
                new CylinderPrimitive<VertexPositionNormal>(GraphicsDevice, (a, b, c) => new VertexPositionNormal(a, b), 10, 2, 10),
                Matrix.CreateRotationZ(MathHelper.PiOver4)*Matrix.CreateTranslation(20, 5, 28));
            _box1 = new Box(
                Matrix.CreateTranslation(-20, 3, -20));
            _box2 = new Box(
                Matrix.CreateTranslation(-10, 4, -10));

            _sky1 = new SkySphere(VisionContent.Load<TextureCube>(@"textures\clouds"));
            _ship = new Ship();
            _windmill = new Windmill();
            _island = new Island(lightingEffectTexture, Matrix.CreateTranslation(-10, 0, 0));

            _water.ReflectedObjects.Add(_sky1);
            _water.ReflectedObjects.Add(_pillar1);
            _water.ReflectedObjects.Add(_pillar2);
            _water.ReflectedObjects.Add(_box1);
            _water.ReflectedObjects.Add(_box2);
            _water.ReflectedObjects.Add(_ship);
            _water.ReflectedObjects.Add(_windmill);
            _water.ReflectedObjects.Add(_island);
            _basicEffect = new BasicEffect(GraphicsDevice);
            _camera.HandleMouse(0, 0);

            // Create the module network
            var perlin = new Perlin();
            var rigged = new RiggedMultifractal();
            var add = new Add(perlin, rigged);

            // Initialize the noise map
            var _mNoiseMap = new Noise2D(64, 64, add);
            _mNoiseMap.GeneratePlanar(-1, 1, -1, 1);

            // Generate the textures
            var _mTextures = new Texture2D[3];
            _mTextures[0] = _mNoiseMap.GetTexture(this._graphics.GraphicsDevice, Gradient.Grayscale);
            _mTextures[1] = _mNoiseMap.GetTexture(this._graphics.GraphicsDevice, Gradient.Terrain);
            _mTextures[2] = _mNoiseMap.GetNormalMap(this._graphics.GraphicsDevice, 3.0f);

            // Zoom in or out do something like this.
            float zoom = 0.5f;
            _mNoiseMap.GeneratePlanar(-1 * zoom, 1 * zoom, -1 * zoom, 1 * zoom);

            VisionContent.Init(this);

            _terrain = new Terrain(
                GraphicsDevice,
                _mTextures[0],
                _mTextures[1],
                _mTextures[2],
                Matrix.CreateTranslation(0, -0.2f, 200));
            _water.ReflectedObjects.Add(_terrain);

            _reimer = new ReimersTerrain(_graphics, Content);
            _water.ReflectedObjects.Add(_reimer);

            var newTerrain = new NewTerrain(
               GraphicsDevice,
                _mTextures[0],
                _mTextures[2],
                Matrix.CreateTranslation(0, -0.5f, -200));
            _water.ReflectedObjects.Add(newTerrain);
        }

        private ReimersTerrain _reimer;

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

            var dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

            var oldKbd = _kbd;
            _kbd = Keyboard.GetState();

            if (_kbd.IsKeyDown(Keys.C) && !oldKbd.IsKeyDown(Keys.C))
                _controlCameraWithMouse ^= true;

            if (_controlCameraWithMouse)
                _camera.UpdateFreeFlyingCamera(gameTime);
            _water.Update(dt, _camera);
            _ship.Update(gameTime);
            _windmill.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _water.RenderReflection(_camera);
            foreach (var z in _water.ReflectedObjects)
                z.Draw(_camera);
           WaterFactory.DrawWaterSurfaceGrid(_water, _camera);

            base.Draw(gameTime);
        }

        private void zzz()
        {
            _basicEffect.World = Matrix.Identity;
                //Matrix.CreateConstrainedBillboard(textPosition, textPosition - camera.Front, Vector3.Down, null, null);
            _basicEffect.View = Matrix.Identity;
            _basicEffect.Projection = Matrix.Identity;

            //_basicEffect.TextureEnabled = true;
            //_basicEffect.VertexColorEnabled = false;
            _basicEffect.CurrentTechnique.Passes[0].Apply();
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                               SamplerState.LinearClamp, DepthStencilState.Default,
                               RasterizerState.CullNone);

            _spriteBatch.Draw(_water._reflectionTarget,
                              new Rectangle(0, 0, (int) _camera.ClientSize.X/4, (int) _camera.ClientSize.Y/4), Color.White);
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
