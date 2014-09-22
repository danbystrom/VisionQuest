using System.Drawing;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;
using factor10.VisionThing.Water;
using ShaderLinking;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
using TestBed;
using Color = SharpDX.Color;
using IDrawable = factor10.VisionThing.IDrawable;
using RasterizerState = SharpDX.Toolkit.Graphics.RasterizerState;
using Rectangle = SharpDX.Rectangle;
using Texture2D = SharpDX.Toolkit.Graphics.Texture2D;

namespace factor10.VisionQuest
{

    /// <summary>
    /// Main game class
    /// </summary>
    internal sealed class VisionQuestGame : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private VisionContent _vContent;

        private readonly SharedData _data; // holds shader source, parameters and dirty state

        private SharpDX.Toolkit.Graphics.GeometricPrimitive<VertexPositionNormalTexture> _cubeGeometry;
            // cube geometrc primitive, contains vertex and index buffers

        private VBasicEffect _basicEffect; // the applied effect, rebuild by ShaderBuilder class

        private KeyboardManager _keyboardManager;
        private MouseManager _mouseManager;

        public Camera Camera;
        private WaterSurface _water;
        public SkySphere Sky;
        private IDrawable _ball;

        private RasterizerState _rasterizerState;
        private MovingShip _movingShip;
        private SpriteBatch _spriteBatch;
        private ClipDrawableInstance _ballInstance;
        private ShadowMap _shadow;

        public VisionQuestGame()
        {
            _data = new SharedData
            {
                Size = new Size(1024, 768)
            };
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            _graphicsDeviceManager.PreferredBackBufferWidth = _data.Size.Width;
            _graphicsDeviceManager.PreferredBackBufferHeight = _data.Size.Height;
            Content.RootDirectory = "Content";
        }

        public SharedData Data
        {
            get { return _data; }
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _spriteBatch = ToDisposeContent(new SpriteBatch(GraphicsDevice));

            _vContent = new VisionContent(_graphicsDeviceManager.GraphicsDevice, Content);
            _keyboardManager = new KeyboardManager(this);
            _mouseManager = new MouseManager(this);

            _basicEffect = new VBasicEffect(_graphicsDeviceManager.GraphicsDevice);
            _basicEffect.EnableDefaultLighting();

            _ball = new SpherePrimitive<VertexPositionNormalTexture>(GraphicsDevice, (p, n, t) => new VertexPositionNormalTexture(p, n, t), 4);
            var x = _vContent.LoadPlainEffect("effects/simpletextureeffect");
            x.Texture = _vContent.Load<Texture2D>("textures/sand");
            _ballInstance = new ClipDrawableInstance(x, _ball, Matrix.Translation(10, 2, 10));

            Sky = new SkySphere(_vContent, _vContent.Load<TextureCube>(@"Textures\clouds"));
            _movingShip = new MovingShip(new ShipModel(_vContent));

            _water = WaterFactory.Create(_vContent);
            _water.ReflectedObjects.Add(_movingShip);
            _water.ReflectedObjects.Add(_ballInstance);
            _water.ReflectedObjects.Add(Sky);

            Camera = new Camera(
                _vContent.ClientSize,
                new Vector3(0, 10, 0),
                new Vector3(50, 0, 50));

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

            _shadow = new ShadowMap(_vContent, Camera, 1024, 1024);
            //_shadow.ShadowCastingObjects.Add(_sailingShip);
            //_shadow.ShadowCastingObjects.Add(reimersTerrain);
            //_shadow.ShadowCastingObjects.Add(generatedTerrain);
            //_shadow.ShadowCastingObjects.Add(bridge);

            var archipelag = new Archipelag(_vContent, _water, _shadow);
        }

        protected override void Update(GameTime gameTime)
        {
            Camera.UpdateFreeFlyingCamera(gameTime, _mouseManager, _mouseManager.GetState(), _keyboardManager.GetState());

            _movingShip.Update(gameTime);
            _water.Update((float) gameTime.ElapsedGameTime.TotalSeconds, Camera);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphicsDeviceManager.GraphicsDevice.SetRasterizerState(_rasterizerState);

            _water.RenderReflection(Camera, _ballInstance);
            GraphicsDevice.SetRenderTargets(GraphicsDevice.DepthStencilBuffer, GraphicsDevice.BackBuffer);

            _graphicsDeviceManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (var x in _water.ReflectedObjects)
                x.Draw(Camera);

           // WaterFactory.DrawWaterSurfaceGrid(_water, Camera, null, 0);
            Sky.Draw(Camera);

            _spriteBatch.Begin(SpriteSortMode.Deferred, GraphicsDevice.BlendStates.NonPremultiplied);
            _spriteBatch.Draw(
                _water._reflectionTarget,
                Vector2.Zero,
                new Rectangle(0, 0, 320, 320),
                Color.White,
                0.0f,
                new Vector2(16, 16),
                Vector2.One,
                SpriteEffects.None,
                0f);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }

}