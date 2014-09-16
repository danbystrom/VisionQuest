using factor10.VisionThing;
using factor10.VisionThing.Water;
using ShaderLinking;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
using RasterizerState = SharpDX.Toolkit.Graphics.RasterizerState;

namespace factor10.VisionQuest
{

    /// <summary>
    /// Main game class
    /// </summary>
    internal sealed class VisionQuestGame : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private VisionContent VContent;

        private readonly SharedData _data; // holds shader source, parameters and dirty state

        private GeometricPrimitive<Vertex> _cubeGeometry; // cube geometrc primitive, contains vertex and index buffers
        private BasicEffect _effect; // the applied effect, rebuild by ShaderBuilder class

        // transformation matrices
        private Matrix _view;
        private Matrix _projection;
        private Matrix _world;

        private KeyboardManager _keyboardManager;
        private MouseManager _mouseManager;
 
        public Camera Camera;
        private WaterSurface _water;
        public SkySphere Sky;

        public VisionQuestGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _data = new SharedData();
        }

        public SharedData Data { get { return _data; } }

        protected override void LoadContent()
        {
            base.LoadContent();

            // mark data as dirty at loading
            _data.IsDirty = true;

            VContent = new VisionContent(_graphicsDeviceManager.GraphicsDevice, Content);
            _keyboardManager = new KeyboardManager(this);
            _mouseManager = new MouseManager(this);

            // initialize transformation matrices
            _view = Matrix.LookAtRH(new Vector3(0, 1.2f, 3f), new Vector3(0, 0, 0), new Vector3(0f, 1f, 0f));
            _projection = Matrix.PerspectiveFovRH(35f * MathUtil.Pi / 180f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 10.0f);
            _world = Matrix.RotationY(45f * MathUtil.Pi / 180f);

            // load geometry
            LoadGeometry();

            _effect = new BasicEffect(_graphicsDeviceManager.GraphicsDevice);

            Sky = new SkySphere(VContent, VContent.Load<TextureCube>(@"Textures\clouds"));

            _water = WaterFactory.Create(VContent);
            _water.ReflectedObjects.Add(Sky);

            Camera = new Camera(
                VContent.ClientSize,
                new Vector3(0,5,0), 
                Vector3.Left);

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
        }

        private void LoadGeometry()
        {
            // constants for easy manipulation
            const float f = 0.5f;
            const float c = 0.75f;

            var vertices = new[]
                           {
                               new Vertex(new Vector3(f, f, f), new Vector3(c, 0f, 0f)),
                               new Vertex(new Vector3(f, f, -f), new Vector3(0, c, 0f)),
                               new Vertex(new Vector3(f, -f, f), new Vector3(0f, 0f, c)),
                               new Vertex(new Vector3(f, -f, -f), new Vector3(c, c, 0f)),

                               new Vertex(new Vector3(-f, f, f), new Vector3(c, 0f, c)),
                               new Vertex(new Vector3(-f, f, -f), new Vector3(0f, c, c)),
                               new Vertex(new Vector3(-f, -f, f), new Vector3(0f, 0f, 0f)),
                               new Vertex(new Vector3(-f, -f, -f), new Vector3(c, c, c))
                           };

            var indices = new short[]
                          {
                              0,2,1, 1,2,3,
                              1,3,7, 7,5,1,

                              5,7,6, 5,6,4,
                              0,6,2, 0,4,6,

                              0,1,5, 0,5,4,
                              3,7,6, 3,6,2
                          };

            _cubeGeometry = ToDisposeContent(new GeometricPrimitive<Vertex>(GraphicsDevice, vertices, indices, false));
        }

        private bool _isFreeFlying;
        private RasterizerState _rasterizerState;

        protected override void Update(GameTime gameTime)
        {
            if (_keyboardManager.GetState().IsKeyPressed(Keys.F))
                _isFreeFlying ^= true;
            if(_isFreeFlying)
                Camera.UpdateFreeFlyingCamera(gameTime, _mouseManager);

            _water.Update((float)gameTime.ElapsedGameTime.TotalSeconds, Camera);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphicsDeviceManager.GraphicsDevice.SetRasterizerState(_rasterizerState);
            _graphicsDeviceManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            // if the effect is loaded...
            if (_effect != null)
            {
                // update cube rotation
                var rot = Matrix.RotationY((float)gameTime.TotalGameTime.TotalSeconds);

                // set the parameters
                _effect.World = rot*_world;
                _effect.View = _view;
                _effect.Projection = _projection;

                // draw cube
                _cubeGeometry.Draw(_effect);
            }

            WaterFactory.DrawWaterSurfaceGrid(_water, Camera, null, 0);
            //_water.Draw(Camera, Vector3.Zero, 15, 0, 0);

            Sky.Draw(Camera);

            base.Draw(gameTime);
        }
    }
}