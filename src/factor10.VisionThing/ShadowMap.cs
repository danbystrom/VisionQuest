using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using factor10.VisionThing.Effects;

namespace factor10.VisionThing
{
    public class ShadowMap
    {
        // List of models, lights, and the camera
        public readonly List<ClipDrawable> ShadowCastingObjects = new List<ClipDrawable>();
        public readonly Camera Camera;

        private readonly GraphicsDevice _graphicsDevice;

        // Shadow depth target and depth-texture effect
        public readonly RenderTarget2D ShadowDepthTarget;
        private readonly IEffect _shadowDepthEffect;

        // Depth texture parameters
        public int ShadowFarPlane = 200;

        // Shadow properties
        public bool DoShadowMapping { get; set; }
        public float ShadowMult = 0.6f;

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

            _shadowDepthEffect = VisionContent.LoadPlainEffect("ShadowEffects/ShadowDepthEffect");
            _shadowDepthEffect.Parameters["FarPlane"].SetValue(ShadowFarPlane);

            _spriteBatch = new SpriteBatch(graphicsDevice);
            _shadowBlurEffect = VisionContent.Load<Effect>("ShadowEffects/GaussianBlur");
            _shadowBlurTarg = new RenderTarget2D(graphicsDevice, targetWidth, targetHeight, false, SurfaceFormat.HalfVector2,
                                                 DepthFormat.Depth24);

            Camera = new Camera(
                new Vector2(targetWidth, targetHeight),
                Vector3.Zero,
                Vector3.Up,
                100);
        }

        public void Draw()
        {
            _graphicsDevice.SetRenderTarget(ShadowDepthTarget);
            _graphicsDevice.Clear(Color.White);  // Clear the render target to 1 (infinite depth)
            foreach (var obj in ShadowCastingObjects)
                obj.Draw(Camera, DrawingReason.ShadowDepthMap, _shadowDepthEffect);
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
