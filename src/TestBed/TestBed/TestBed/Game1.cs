using factor10.VisionThing;
using factor10.VisionThing.Primitives;
using factor10.VisionThing.Water;
using LibNoise.Xna;
using LibNoise.Xna.Generator;
using LibNoise.Xna.Operator;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BasicEffect = Microsoft.Xna.Framework.Graphics.BasicEffect;

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
        private Box _box1, _box2;
        private Ship _ship1;
        private MovingShip _ship2;
        private Windmill _windmill;
        private Island _island;

        private BasicEffect _basicEffect;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;

        private SkySphere _sky1;

        private KeyboardState _kbd;
        private bool _controlCameraWithMouse = true;

        private RenderTarget2D _target1;
        private RenderTarget2D _target2;
        private TorusPrimitive<VertexPositionNormal> _torus;

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
            _font = VisionContent.Load<SpriteFont>("fonts/spritefont1");
            var lightingEffectTexture = VisionContent.LoadPlainEffect(@"effects\lightingeffecttexture");

            _camera = new Camera(Window.ClientBounds, new Vector3(0, 4, -20), Vector3.Up);
            _water = WaterFactory.Create(GraphicsDevice);

            _box1 = new Box(Matrix.CreateTranslation(-20, 3, -20));
            _box2 = new Box(Matrix.CreateTranslation(-10, 4, -10));

            _sky1 = new SkySphere(VisionContent.Load<TextureCube>(@"textures\clouds"));
            var shipModel = new ShipModel();
            _ship1 = new Ship(shipModel);
            _ship2 = new MovingShip(shipModel);
            _windmill = new Windmill();
            _island = new Island(lightingEffectTexture, Matrix.CreateTranslation(-10, 0, 0));

            _water.ReflectedObjects.Add(_sky1);
            _water.ReflectedObjects.Add(_box1);
            _water.ReflectedObjects.Add(_box2);
            _water.ReflectedObjects.Add(_ship1);
            _water.ReflectedObjects.Add(_ship2);
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
            _mNoiseMap.GeneratePlanar(-1*zoom, 1*zoom, -1*zoom, 1*zoom);

            VisionContent.Init(this);

            var newTerrain = new NewTerrain(
                GraphicsDevice,
                _mTextures[0],
                Matrix.CreateTranslation(0, -0.5f, -200),
                true);
            _water.ReflectedObjects.Add(newTerrain);

            newTerrain = new NewTerrain(
                GraphicsDevice,
                _mTextures[0],
                Matrix.CreateTranslation(200, -0.5f, 0),
                false);
            _water.ReflectedObjects.Add(newTerrain);

            _torus = new TorusPrimitive<VertexPositionNormal>(
                GraphicsDevice,
                (p, n) => new VertexPositionNormal(p, n),
                50, 10, 32);

            _target1 = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24);

            _target2 = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.Viewport.Width/2,
                GraphicsDevice.Viewport.Height/2,
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

            var dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

            var oldKbd = _kbd;
            _kbd = Keyboard.GetState();

            if (_kbd.IsKeyDown(Keys.C) && !oldKbd.IsKeyDown(Keys.C))
                _controlCameraWithMouse ^= true;

            if (_controlCameraWithMouse)
                _camera.UpdateFreeFlyingCamera(gameTime);
            _water.Update(dt, _camera);
            _ship1.Update(gameTime);
            _ship2.Update(gameTime);
            _windmill.Update(gameTime);

            _frames++;
            _frameTime += dt;
            if (_frameTime > 1)
            {
                _fps = _frames;
                _frames = 0;
                _frameTime = 0;
            }

            base.Update(gameTime);
        }

        private int _frames;
        private float _frameTime;
        private int _fps;


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

            zzz();
            VisionContent.RenderedTriangles = 0;

            base.Draw(gameTime);
        }

        protected void NewDraw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_target1);

            _water.RenderReflection(_camera);
            foreach (var z in _water.ReflectedObjects)
                z.Draw(_camera);
            WaterFactory.DrawWaterSurfaceGrid(_water, _camera);

            GraphicsDevice.SetRenderTarget(_target2);
            GraphicsDevice.Clear(Color.Black);
            _box1.Draw(_camera);

            GraphicsDevice.SetRenderTarget(null);

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

            //_spriteBatch.Draw(_water._reflectionTarget,
            //                  new Rectangle(940, 0, (int) _camera.ClientSize.X/4, (int) _camera.ClientSize.Y/2), Color.White);
            _spriteBatch.DrawString(_font, string.Format("Verticies: {0}", VisionContent.RenderedTriangles), new Vector2(10, 10), Color.Gold);
            _spriteBatch.DrawString(_font, string.Format("FPS: {0}", _fps), new Vector2(10, 30), Color.Gold);
            _spriteBatch.DrawString(_font, string.Format("Water planes: {0}", WaterFactory.RenderedWaterPlanes), new Vector2(10, 50), Color.Gold);
            _spriteBatch.End();

            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.BlendState = BlendState.Opaque;

            //_basicEffect.World = Matrix.Identity;
            //_basicEffect.TextureEnabled = false;
            //_basicEffect.VertexColorEnabled = true;
            //_basicEffect.CurrentTechnique.Passes[0].Apply();
            //drawArc(textPosition, zzz);
        }

    }

}
