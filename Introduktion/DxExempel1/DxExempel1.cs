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

        private Matrix view;
        private Matrix projection;

        private Effect _fx;

        private IVDrawable primitive;

        private KeyboardManager keyboard;
        private KeyboardState keyboardState;

        private bool _wireframe = true;

        private Buffer<VertexPositionColor> _vertexBuffer;
        private Buffer[] _indexBuffers;
        private VertexInputLayout _vertexInputLayout;

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
            _fx = Content.Load<Effect>("fx1");

            var verticies = new List<VertexPositionColor>();
            addPlane(verticies, Vector3.Up);
            addPlane(verticies, Vector3.Down);
            addPlane(verticies, Vector3.Left);
            addPlane(verticies, Vector3.Right);
            addPlane(verticies, Vector3.ForwardRH);
            addPlane(verticies, Vector3.BackwardRH);

            _vertexBuffer = Buffer.Vertex.New(GraphicsDevice, verticies.ToArray());
            _vertexInputLayout = VertexInputLayout.FromBuffer(0, _vertexBuffer);

            base.LoadContent();
        }

        private void addPlane(List<VertexPositionColor> verticies, Vector3 normal)
        {
            var mask = Vector3.One - new Vector3(Math.Abs(normal.X), Math.Abs(normal.Y), Math.Abs(normal.Z));
            var d = Vector3.Cross(normal, mask);
            var d0 = (mask + d)/2;
            var d1 = (mask - d) / 2;

            var v0 = normal - d0 - d1;
            var v1 = normal - d0 + d1;
            var v2 = normal + d0 - d1;
            var v3 = normal + d0 + d1;

            verticies.Add(new VertexPositionColor(v0, toColor(v0)));
            verticies.Add(new VertexPositionColor(v2, toColor(v2)));
            verticies.Add(new VertexPositionColor(v1, toColor(v1)));

            verticies.Add(new VertexPositionColor(v1, toColor(v1)));
            verticies.Add(new VertexPositionColor(v2, toColor(v2)));
            verticies.Add(new VertexPositionColor(v3, toColor(v3)));
        }

        private static Color toColor(Vector3 v)
        {
            return Color.White;
            return new Color((v + Vector3.One) / 2);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Calculates the world and the view based on the model size
            view = Matrix.LookAtRH(new Vector3(0.0f, 0.0f, 7.0f), new Vector3(0, 0.0f, 0), Vector3.UnitY);
            projection = Matrix.PerspectiveFovRH(0.9f, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);

            // Update basic effect for rendering the Primitive
            _fx.Parameters["View"].SetValue(view);
            _fx.Parameters["Projection"].SetValue(projection);

            // Get the current state of the keyboard
            keyboardState = keyboard.GetState();

            if (keyboardState.IsKeyPressed(Keys.D1))
                _wireframe ^= true;
        }

        protected override void Draw(GameTime gameTime)
        {
            // Use time in seconds directly
            var time = (float)gameTime.TotalGameTime.TotalSeconds;

            // Clears the screen with the Color.CornflowerBlue
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SetRasterizerState(_wireframe ? GraphicsDevice.RasterizerStates.WireFrame : GraphicsDevice.RasterizerStates.Default);

            _fx.Parameters["World"].SetValue(Matrix.Scaling(2.0f, 2.0f, 2.0f)*
                                             Matrix.RotationX(0.4f*(float) Math.Sin(time*1.45))*
                                             Matrix.RotationY(time*0.9f)*
                                             Matrix.RotationZ(0)*
                                             Matrix.Translation(-2, 0, -4));
            foreach (var effectPass in _fx.CurrentTechnique.Passes)
            {
                effectPass.Apply();

                GraphicsDevice.SetVertexBuffer(_vertexBuffer);
                GraphicsDevice.SetVertexInputLayout(_vertexInputLayout);

                GraphicsDevice.Draw(
                    PrimitiveType.TriangleList,
                    _vertexBuffer.ElementCount);
            }

            base.Draw(gameTime);
        }
    }
}
