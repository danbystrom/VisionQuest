using System.Drawing;
using System.Text;
using factor10.VisionaryHeads;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;
using factor10.VisionThing.Water;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
using TestBed;
using Color = SharpDX.Color;
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

        private VBasicEffect _basicEffect; // the applied effect, rebuild by ShaderBuilder class

        private SpriteFont _arial16Font;

        public Camera Camera;
        private WaterSurface _water;
        public SkySphere Sky;
        private IVDrawable _ball;

        private RasterizerState _rasterizerState;
        private MovingShip _movingShip;
        private SpriteBatch _spriteBatch;
        private ClipDrawableInstance _ballInstance;
        private ShadowMap _shadow;
        private Archipelag _archipelag;
        private int _frames;
        private double _frameTime;
        private int _fps;

        public VisionQuestGame(SharedData data)
        {
            _data = data;
            _graphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = _data.Size.Width,
                PreferredBackBufferHeight = _data.Size.Height                
            };
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
            _arial16Font = Content.Load<SpriteFont>("Fonts/Arial16");

            _basicEffect = new VBasicEffect(_graphicsDeviceManager.GraphicsDevice);
            _basicEffect.EnableDefaultLighting();

            _ball = new SpherePrimitive<VertexPositionNormalTexture>(GraphicsDevice, (p, n, t) => new VertexPositionNormalTexture(p, n, t), 1);
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
                new KeyboardManager(this),
                new MouseManager(this),
                null, //new PointerManager(this),
                new Vector3(0, 15, 0),
                new Vector3(-10, 15, 0));

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

            //_archipelag = new Archipelag(_vContent, _water, _shadow);
        }

        protected override void Update(GameTime gameTime)
        {
            Camera.UpdateInputDevices();
            Camera.UpdateFreeFlyingCamera(gameTime);

            if (_data.LoadProgram != null)
            {
                if (_archipelag != null)
                    _archipelag.Kill(_water, _shadow);
                _archipelag = new Archipelag(
                    _vContent,
                    _data.LoadProgram,
                    _water,
                    _shadow);
                _data.LoadProgram = null;
            }

            _movingShip.Update(Camera, gameTime);
            if(_archipelag!=null)
                _archipelag.Update(Camera, gameTime);
            _water.Update((float) gameTime.ElapsedGameTime.TotalSeconds);

            _frames++;
            _frameTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_frameTime >= 1000)
            {
                _fps = _frames;
                _frames = 0;
                _frameTime = 0;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphicsDeviceManager.GraphicsDevice.SetRasterizerState(_rasterizerState);

            _water.RenderReflection(Camera, _ballInstance);
            GraphicsDevice.SetRenderTargets(GraphicsDevice.DepthStencilBuffer, GraphicsDevice.BackBuffer);

            _graphicsDeviceManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            _movingShip._shipModel.Draw(Camera);
            //_movingShip.Draw(Camera);
            //foreach (var x in _water.ReflectedObjects)
            //    x.Draw(Camera);
            if(_archipelag!=null)
                _archipelag.Draw(Camera);

            WaterFactory.DrawWaterSurfaceGrid(_water, Camera, null, 0);
            Sky.Draw(Camera);

            if (_archipelag != null)
            {
                _archipelag.DrawSignsAndArchs(Camera, _data.Storage.DrawLines);
                Camera.UpdateEffect(_basicEffect);
                _archipelag.PlayAround(_basicEffect, _ball);
            }

            //_spriteBatch.Begin(SpriteSortMode.Deferred, GraphicsDevice.BlendStates.NonPremultiplied);
            //_spriteBatch.Draw(
            //    _water._reflectionTarget,
            //    Vector2.Zero,
            //    new Rectangle(0, 0, 320, 320),
            //    Color.White,
            //    0.0f,
            //    new Vector2(16, 16),
            //    Vector2.One,
            //    SpriteEffects.None,
            //    0f);
            //_spriteBatch.End();

            var sb = new StringBuilder();
            sb.AppendFormat("FPS: {0}", _fps).AppendLine();
            sb.AppendFormat("Pos: {0}   Look at: {1}", Camera.Position, Camera.Target);
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_arial16Font, sb.ToString(), new Vector2(16, 16), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }

}