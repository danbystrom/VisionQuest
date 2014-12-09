using System;
using System.Collections.Generic;
using System.Text;
using factor10.VisionQuest.Commands;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;
using factor10.VisionThing.Terrain;
using factor10.VisionThing.Water;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
using Color = SharpDX.Color;
using RasterizerState = SharpDX.Toolkit.Graphics.RasterizerState;
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

        private readonly SharedData _data;

        private VBasicEffect _basicEffect;

        private SpriteFont _arial16Font;

        private WaterSurface _water;
        private ShadowMap _shadow;
        public SkySphere Sky;
        private IVDrawable _ball;

        private RasterizerState _rasterizerState;
        private MovingShip _movingShip;
        private SpriteBatch _spriteBatch;
        private ClipDrawableInstance _ballInstance;

        private Camera _camera;

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

            _ball = new SpherePrimitive<VertexPositionNormalTexture>(GraphicsDevice, (p, n, t, tx) => new VertexPositionNormalTexture(p, n, tx), 1);
            var x = _vContent.LoadEffect("effects/simpletextureeffect");
            x.Texture = _vContent.Load<Texture2D>("textures/sand");
            _ballInstance = new ClipDrawableInstance(x, _ball, Matrix.Translation(10, 2, 10));

            Sky = new SkySphere(_vContent, _vContent.Load<TextureCube>(@"Textures\clouds"));
            _movingShip = new MovingShip(new ShipModel(_vContent));

            _water = WaterFactory.Create(_vContent);
            _water.ReflectedObjects.Add(_movingShip._shipModel);
            _water.ReflectedObjects.Add(_ballInstance);
            _water.ReflectedObjects.Add(Sky);

            _camera = new Camera(
                _vContent.ClientSize,
                new KeyboardManager(this),
                new MouseManager(this),
                null, //new PointerManager(this),
                new Vector3(0, 15, 0),
                new Vector3(-10, 15, 0));

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

            _shadow = new ShadowMap(_vContent, _camera, 1024, 1024);
            //_shadow.ShadowCastingObjects.Add(_sailingShip);
            //_shadow.ShadowCastingObjects.Add(reimersTerrain);
            //_shadow.ShadowCastingObjects.Add(generatedTerrain);
            //_shadow.ShadowCastingObjects.Add(bridge);

            //_archipelag = new Archipelag(_vContent, _water, _shadow);

            _data.VContent = _vContent;
            _data.Camera = _camera;
            _data.Water = _water;
            _data.Shadow = _shadow;

            _q = new CxBillboard(_vContent, Matrix.Identity, _vContent.Load<Texture2D>("billboards/wheat_billboard"), 20, 10);
            _q.AddPositionsWithSameNormal(Vector3.Up,
                Vector3.Zero,
                Vector3.Left*10.5f, Vector3.Up,
                Vector3.Right*10.4f, Vector3.Up,
                Vector3.ForwardRH*3.45f, Vector3.Up,
                Vector3.BackwardRH*2.9f, Vector3.Up);

            _qq = new SimpleBillboards(
                _vContent, Matrix.Identity, _vContent.Load<Texture2D>("textures/sand"), (Vector3.Left*10).AsList(), 10, 10);
        }

        private CxBillboard _q;
        private SimpleBillboards _qq;

        protected override void Update(GameTime gameTime)
        {
            _camera.UpdateInputDevices();
            _camera.UpdateFreeFlyingCamera(gameTime);

            ICommand cmd;
            if (_data.Commands.TryDequeue(out cmd))
                cmd.Excecute(_data);

            for (var i = 0; i < _data.Actions.Count;)
                if (_data.Actions[i].Do(_data, gameTime))
                    i++;
                else
                    _data.Actions.RemoveAt(i);

            _movingShip.Update(_camera, gameTime);
            if (_data.Archipelag != null)
                _data.Archipelag.Update(_camera, gameTime);
            _water.Update((float) gameTime.ElapsedGameTime.TotalSeconds);

            _q.Update(_camera, gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphicsDeviceManager.GraphicsDevice.SetRasterizerState(_rasterizerState);

            _water.RenderReflection(_camera);
            GraphicsDevice.SetRenderTargets(GraphicsDevice.DepthStencilBuffer, GraphicsDevice.BackBuffer);

            _graphicsDeviceManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            _movingShip.Draw(_camera);
            if(_data.Archipelag!=null)
                _data.Archipelag.Draw(_camera);

            if(!_data.HiddenWater)
                WaterFactory.DrawWaterSurfaceGrid(_water, _camera, null, 0, _data.WaterSurfaceSize, _data.WaterSurfaceScale);
            Sky.Draw(_camera);

            if (_data.Archipelag != null)
            {
                _data.Archipelag.DrawSignsAndArchs(_camera, _data.Storage.DrawLines);
                _camera.UpdateEffect(_basicEffect);
                _data.Archipelag.PlayAround(_basicEffect, _ball);
            }

            _qq.Draw(_camera);
            _q.Draw(_camera);

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
            sb.AppendFormat("Pos: {0}   Look at: {1}   Yaw: {2:0}  Pitch: {3:0}", _camera.Position, _camera.Target, MathUtil.RadiansToDegrees(_camera.Yaw), MathUtil.RadiansToDegrees(_camera.Pitch));
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_arial16Font, sb.ToString(), new Vector2(16, 16), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }

}