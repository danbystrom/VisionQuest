using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionThing
{
    public class ShadowMap
    {
        private readonly GraphicsDevice _graphicsDevice;

        public readonly List<ClipDrawable> ShadowCastingObjects = new List<ClipDrawable>();
        public readonly Camera Camera;
        public readonly Camera RealCamera;

        public readonly RenderTarget2D ShadowDepthTarget;

        // Depth texture parameters
        public int ShadowFarPlane = 200;
        public float ShadowMult = 0.75f;

        private readonly SpriteBatch _spriteBatch;
        private readonly RenderTarget2D _shadowBlurTarg;
        private readonly Effect _shadowBlurEffect;

        public ShadowMap(
            VisionContent vContent,
            Camera camera,
            int width,
            int height)
        {
            _graphicsDevice = vContent.GraphicsDevice;
            RealCamera = camera;

            ShadowDepthTarget = RenderTarget2D.New(_graphicsDevice, width, height, PixelFormat.R16G16.Float);

            _spriteBatch = new SpriteBatch(_graphicsDevice);
            _shadowBlurEffect = vContent.Load<Effect>("ShadowEffects/GaussianBlur");
            _shadowBlurTarg = RenderTarget2D.New(_graphicsDevice, width, height, PixelFormat.R16G16.Float);

            Camera = new Camera(
                new Vector2(width, height),
                Vector3.Zero,
                Vector3.Up,
                1,
                ShadowFarPlane);
            Camera.Projection = Matrix.OrthoRH(
                50,
                50,
                1,
                ShadowFarPlane);
        }

        public void Draw()
        {
            _graphicsDevice.SetRenderTargets(_graphicsDevice.DepthStencilBuffer, ShadowDepthTarget);
            _graphicsDevice.Clear(Color.White);  // Clear the render target to 1 (infinite depth)
            foreach (var obj in ShadowCastingObjects)
                obj.Draw(Camera, DrawingReason.ShadowDepthMap, this);
            _graphicsDevice.SetRenderTargets(_graphicsDevice.DepthStencilBuffer, _graphicsDevice.BackBuffer);

            //blurShadow(_shadowBlurTarg, ShadowDepthTarget, 0);
            //blurShadow(ShadowDepthTarget, _shadowBlurTarg, 1);
        }

        private void blurShadow(RenderTarget2D to, RenderTarget2D from, int dir)
        {
            _graphicsDevice.SetRenderTargets(to);
            _graphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, _shadowBlurEffect);
            _shadowBlurEffect.CurrentTechnique.Passes[dir].Apply();
            _spriteBatch.Draw(from, new Rectangle(0, 0, from.Width, from.Height), Color.White);
            _spriteBatch.End();

            // Clean up after the sprite batch
            //TODO
            //_graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            //_graphicsDevice.SetDepthStencilState(DepthStencilState.Default);
            //_graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            //_graphicsDevice.BlendState = BlendState.Opaque;

            //_graphicsDevice.SetRenderTargets(null);
        }

    }

}
