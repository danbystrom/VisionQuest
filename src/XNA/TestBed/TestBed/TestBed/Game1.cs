using System.Collections.Generic;
using System.IO;
using System.Linq;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Objects;
using factor10.VisionThing.Primitives;
using factor10.VisionThing.Water;
using LibNoise.Xna;
using LibNoise.Xna.Generator;
using LibNoise.Xna.Operator;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using factor10.VisionaryHeads;
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
        private MovingShip _sailingShip;

        private BasicEffect _basicEffect;
        private IEffect _lightingffect;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;

        private SkySphere _sky1;

        private KeyboardState _kbd;
        private bool _controlCameraWithMouse = true;

        private TorusPrimitive<VertexPositionNormal> _torus;
        private SpherePrimitive _sphere;

        private ShadowMap _shadow;

        private Arcs _arcs;
        private SignsBig _signsBig;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 800;
            Content.RootDirectory = "Content";
            IsFixedTimeStep = false;
            _graphics.SynchronizeWithVerticalRetrace = false;
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
            var planeTest = new PlaneMeshPrimitive<VertexPositionTexture>(
                GraphicsDevice,
                (x, y, t) => new VertexPositionTexture(new Vector3(x, 0, y), new Vector2(x, y)),
                4, 4, 3);

            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            VisionContent.Init(this);
            _font = VisionContent.Load<SpriteFont>("fonts/BlackCastle");

            _camera = new Camera(Window.ClientBounds, new Vector3(10, 4, -20), Vector3.Up);
            _water = WaterFactory.Create(GraphicsDevice);

            _sky1 = new SkySphere(VisionContent.Load<TextureCube>("textures/clouds"));
            var shipModel = new ShipModel();
            _sailingShip = new MovingShip(shipModel);

            _water.ReflectedObjects.Add(_sky1);
            _water.ReflectedObjects.Add(_sailingShip);
            _basicEffect = new BasicEffect(GraphicsDevice);
            _camera.HandleMouse(0, 0);

            // Create the module network
            var perlin = new Perlin();
            var rigged = new RiggedMultifractal();
            var add = new Add(perlin, rigged);

            // Initialize the noise map
            var noiseMap = new Noise2D(512, 512, add);
            noiseMap.GeneratePlanar(-1, 1, -1, 1);

            // Zoom in or out do something like this.
            float zoom = 0.5f;
            //_mNoiseMap.GeneratePlanar(-1*zoom, 1*zoom, -1*zoom, 1*zoom);

            VisionContent.Init(this);

            var reimersTerrain = new ReimersTerrain(
                Matrix.CreateTranslation(200, -0.5f, 0));
            _water.ReflectedObjects.Add(reimersTerrain);

            var generatedTerrain = new GeneratedTerrain(
                Matrix.CreateTranslation(0, -0.5f, 100),
                noiseMap.GetTexture(_graphics.GraphicsDevice, Gradient.Grayscale));
            _water.ReflectedObjects.Add(generatedTerrain);
            var bridge = new Bridge(Matrix.Identity);
            _water.ReflectedObjects.Add(bridge);

            rigged.Seed = 42;
            noiseMap = new Noise2D(128, 128, add);
            noiseMap.GeneratePlanar(-1, 1, -1, 1);
            var generatedTerrain2 = new LargeTerrain(
                Matrix.CreateTranslation(500, -0.5f, 200),
                noiseMap.GetTexture(_graphics.GraphicsDevice, Gradient.Grayscale));
            _water.ReflectedObjects.Add(generatedTerrain2);

            _torus = new TorusPrimitive<VertexPositionNormal>(
                GraphicsDevice,
                (p, n) => new VertexPositionNormal(p, n),
                50, 10, 32);

            var vprogram = new VProgram(@"C:\proj\photomic.old\src\Plata\bin\Release\Pl�ta.exe");
            foreach(var fil in Directory.GetFiles(@"c:\users\dan\desktop\VisionQuest\","*.metrics.txt"))
                GenerateMetrics.FromPregeneratedFile(fil).UpdateProgramWithMetrics(vprogram);

            _shadow = new ShadowMap(GraphicsDevice, _camera, 1024, 1024);
            _shadow.ShadowCastingObjects.Add(_sailingShip);
            _shadow.ShadowCastingObjects.Add(reimersTerrain);
            _shadow.ShadowCastingObjects.Add(generatedTerrain);
            _shadow.ShadowCastingObjects.Add(bridge);

            var codeIslands = CodeIsland.Create(vprogram.VAssemblies);
            foreach (var codeIsland in codeIslands)
            {
                _water.ReflectedObjects.Add(codeIsland);
                _shadow.ShadowCastingObjects.Add(codeIsland);
            }

            _sphere = new SpherePrimitive(GraphicsDevice, 0.25f);
            _lightingffect = VisionContent.LoadPlainEffect("effects/LightingEffect");

            _signsBig = new SignsBig(codeIslands);
            _arcs = new Arcs(codeIslands);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.shadow
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

            if (_kbd.IsKeyDown(Keys.R) && !oldKbd.IsKeyDown(Keys.R))
                _controlCameraWithMouse ^= true;

            if (_kbd.IsKeyDown(Keys.D1) && !oldKbd.IsKeyDown(Keys.D1))
                _nisse++;
            if (_kbd.IsKeyDown(Keys.D2) && !oldKbd.IsKeyDown(Keys.D2))
                _nisse--;

            if (_controlCameraWithMouse)
                _camera.UpdateFreeFlyingCamera(gameTime, _kbd);
            _water.Update(dt, _camera);

            foreach(var cd in _water.ReflectedObjects)
                cd.Update(gameTime);
            _arcs.Update(gameTime);

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

        private int _nisse;

        private int _frames;
        private float _frameTime;
        private int _fps;

        private bool _eachOther;


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _eachOther ^= true;
            if (_eachOther)
            {
                var lookingDirection = _camera.Target - _camera.Position;
                lookingDirection.Normalize();
                var lookAtFocus = _camera.Position + 20*lookingDirection;
                lookAtFocus.Y = 2;

                _shadow.Camera.Update(
                    lookAtFocus - VisionContent.SunlightDirection * 20,
                    lookAtFocus);
                _shadow.Draw();
            }
            else
                _water.RenderReflection(_camera);

            VisionContent.RenderedTriangles = 0;

            foreach (var z in _water.ReflectedObjects)
                z.Draw(_camera, DrawingReason.Normal, _shadow);
            WaterFactory.DrawWaterSurfaceGrid(_water, _camera, _shadow, _nisse);

            if ( _nisse == 0)
                _arcs.Draw(_camera);

            _signsBig.Draw(_camera);

            //_camera.UpdateEffect(_lightingffect);
            //_lightingffect.World = Matrix.CreateTranslation(_shadow.Camera.Target);
            //_sphere.Draw(_lightingffect);
            //_lightingffect.World = Matrix.CreateTranslation(_shadow.Camera.Position);
            //_sphere.Draw(_lightingffect);

            //zzz();

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
                               SamplerState.PointClamp, DepthStencilState.Default,
                               RasterizerState.CullNone);

            var w = _shadow.ShadowDepthTarget.Width/4;
            var h = _shadow.ShadowDepthTarget.Height/4;
            //_spriteBatch.Draw(_shadow.ShadowDepthTarget, new Rectangle(1270-w, 10, w, h), Color.White);
            _spriteBatch.DrawString(_font, string.Format("Triangles: {0}", VisionContent.RenderedTriangles), new Vector2(10, 10), Color.Gold);
            _spriteBatch.DrawString(_font, string.Format("FPS: {0}", _fps), new Vector2(10, 30), Color.Gold);
            _spriteBatch.DrawString(_font, string.Format("Water planes: {0}",
                string.Join(",",WaterFactory.RenderedWaterPlanes.Select(e => e.ToString()))),
                new Vector2(10, 50), Color.Gold);
            var p = _camera.Position;
            _spriteBatch.DrawString(_font, string.Format("Campera pos: {0:0.0} / {1:0.0} / {2:0.0}", p.X, p.Y, p.Z ), new Vector2(10, 70), Color.Gold);
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