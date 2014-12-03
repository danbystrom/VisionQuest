using System.Collections.Generic;
using factor10.VisionThing.Effects;
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
        public int ShadowNearPlane;
        public int ShadowFarPlane;
        public float ShadowMult = 0.75f;

        private readonly SpriteBatch _spriteBatch;
        private readonly RenderTarget2D _shadowBlurTarg;
        private readonly IVEffect _shadowBlurEffect;

        public ShadowMap(
            VisionContent vContent,
            Camera camera,
            int width,
            int height,
            int nearPlane = 1,
            int farPlane = 200)
        {
            _graphicsDevice = vContent.GraphicsDevice;
            RealCamera = camera;

            ShadowDepthTarget = RenderTarget2D.New(_graphicsDevice, width, height, PixelFormat.R16G16.Float);

            _spriteBatch = new SpriteBatch(_graphicsDevice);
            _shadowBlurEffect = vContent.LoadPlainEffect("ShadowEffects/Blur");
            _shadowBlurTarg = RenderTarget2D.New(_graphicsDevice, width, height, PixelFormat.R16G16.Float);

            ShadowNearPlane = nearPlane;
            ShadowFarPlane = farPlane;
            Camera = new Camera(
                new Vector2(width, height),
                Vector3.Zero,
                Vector3.Up,
                ShadowNearPlane,
                ShadowFarPlane);
            UpdateProjection(60, 60);
        }

        public void UpdateProjection(int width, int height, int near = 0, int far = 0)
        {
            Camera.Projection = Matrix.OrthoRH(
                width,
                height,
                near > 0 ? near : ShadowNearPlane,
                far > 0 ? far : ShadowFarPlane);
        }

        public void Draw()
        {
            _graphicsDevice.SetRenderTargets(ShadowDepthTarget);
            _graphicsDevice.Clear(Color.White); // Clear the render target to 1 (infinite depth)
            foreach (var obj in ShadowCastingObjects)
                obj.Draw(Camera, DrawingReason.ShadowDepthMap, this);

            blurShadow(_shadowBlurTarg, ShadowDepthTarget, 0);
            blurShadow(ShadowDepthTarget, _shadowBlurTarg, 0);

            _graphicsDevice.SetDepthStencilState(_graphicsDevice.DepthStencilStates.Default);
            _graphicsDevice.SetBlendState(_graphicsDevice.BlendStates.Opaque);
            _graphicsDevice.SetRenderTargets(_graphicsDevice.DepthStencilBuffer, _graphicsDevice.BackBuffer);
        }

        private void blurShadow(RenderTarget2D to, RenderTarget2D from, int dir)
        {
            _graphicsDevice.SetRenderTargets(to);
            _shadowBlurEffect.Apply();
            _spriteBatch.Begin(SpriteSortMode.Immediate,  _shadowBlurEffect.Effect);
            _spriteBatch.Draw(from, Vector2.Zero, Color.White);
            _spriteBatch.End();
            _shadowBlurEffect.Texture = null;
            //_graphicsDevice.ResetTargets();
        }

    }

}
