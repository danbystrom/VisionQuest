using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using factor10.VisionThing.Effects;

namespace factor10.VisionThing
{
    public class ShadowMap
    {
        private readonly GraphicsDevice _graphicsDevice;

        public readonly List<ClipDrawable> ShadowCastingObjects = new List<ClipDrawable>();
        public readonly Camera Camera;

        public readonly RenderTarget2D ShadowDepthTarget;

        // Depth texture parameters
        public int ShadowFarPlane = 200;
        public float ShadowMult = 0.4f;

        private readonly SpriteBatch _spriteBatch;
        private readonly RenderTarget2D _shadowBlurTarg;
        private readonly Effect _shadowBlurEffect;

        public ShadowMap(GraphicsDevice graphicsDevice)
        {
            var targetWidth = graphicsDevice.Viewport.Width/1;
            var targetHeight = graphicsDevice.Viewport.Height/1;

            _graphicsDevice = graphicsDevice;
            ShadowDepthTarget = new RenderTarget2D(graphicsDevice, targetWidth, targetHeight, false, SurfaceFormat.HalfVector2,
                                                   DepthFormat.Depth24);

            _spriteBatch = new SpriteBatch(graphicsDevice);
            _shadowBlurEffect = VisionContent.Load<Effect>("ShadowEffects/GaussianBlur");
            _shadowBlurTarg = new RenderTarget2D(graphicsDevice, targetWidth, targetHeight, false, SurfaceFormat.HalfVector2,
                                                 DepthFormat.Depth24);

            Camera = new Camera(
                new Vector2(targetWidth, targetHeight),
                Vector3.Zero,
                Vector3.Up,
                1,
                ShadowFarPlane);
            Camera.Projection = Matrix.CreateOrthographic(
                50,
                50,
                1,
                100);
        }

        public void Draw()
        {
            _graphicsDevice.SetRenderTarget(ShadowDepthTarget);
            _graphicsDevice.Clear(Color.White);  // Clear the render target to 1 (infinite depth)
            foreach (var obj in ShadowCastingObjects)
                obj.Draw(Camera, DrawingReason.ShadowDepthMap);
            _graphicsDevice.SetRenderTarget(null);

            blurShadow(_shadowBlurTarg, ShadowDepthTarget, 0);
            blurShadow(ShadowDepthTarget, _shadowBlurTarg, 1);
        }

        private void blurShadow(RenderTarget2D to, Texture2D from, int dir)
        {
            _graphicsDevice.SetRenderTarget(to);
            _graphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp, null, null, _shadowBlurEffect);
            _shadowBlurEffect.CurrentTechnique.Passes[dir].Apply();
            _spriteBatch.Draw(from, new Rectangle(0, 0, from.Width, from.Height), Color.White);
            _spriteBatch.End();
            
            // Clean up after the sprite batch
            _graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            _graphicsDevice.DepthStencilState = DepthStencilState.Default;
            _graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            _graphicsDevice.BlendState = BlendState.Opaque;

            _graphicsDevice.SetRenderTarget(null);
        }

    }

}
