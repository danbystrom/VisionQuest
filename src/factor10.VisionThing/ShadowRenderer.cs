using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using factor10.VisionThing.Effects;

namespace factor10.VisionThing
{
    public class ShadowRenderer
    {
        // List of models, lights, and the camera
        public List<ClipDrawable> Models { get; set; }
        public Camera Camera { get; set; }

        private readonly GraphicsDevice _graphicsDevice;
        private readonly int _viewWidth = 0;
        private readonly int _viewHeight = 0;

        // Position and target of the shadowing light
        public Vector3 ShadowLightPosition { get; set; }
        public Vector3 ShadowLightTarget { get; set; }

        // Shadow depth target and depth-texture effect
        private readonly RenderTarget2D _shadowDepthTarg;
        private readonly IEffect _shadowDepthEffect;

        // Depth texture parameters
        private const int ShadowMapSize = 1024;
        private const int ShadowFarPlane = 10000;

        // Shadow light view and projection
        private Matrix _shadowView;
        private Matrix _shadowProjection;

        // Shadow properties
        public bool DoShadowMapping { get; set; }
        public float ShadowMult { get; set; }

        private readonly SpriteBatch _spriteBatch;
        private readonly RenderTarget2D _shadowBlurTarg;
        private readonly Effect _shadowBlurEffect;

        public ShadowRenderer(
            GraphicsDevice graphicsDevice )
        {
            _viewWidth = graphicsDevice.Viewport.Width;
            _viewHeight = graphicsDevice.Viewport.Height;

            _shadowDepthTarg = new RenderTarget2D(graphicsDevice, ShadowMapSize,
                                                  ShadowMapSize, false, SurfaceFormat.HalfVector2, DepthFormat.Depth24);

            _shadowDepthEffect = VisionContent.LoadPlainEffect("ShadowDepthEffect");
            _shadowDepthEffect.Parameters["FarPlane"].SetValue(ShadowFarPlane);

            _spriteBatch = new SpriteBatch(graphicsDevice);
            _shadowBlurEffect = VisionContent.Load<Effect>("GaussianBlur");

            _shadowBlurTarg = new RenderTarget2D(graphicsDevice, ShadowMapSize,
                                                 ShadowMapSize, false, SurfaceFormat.HalfVector2, DepthFormat.Depth24);

            _graphicsDevice = graphicsDevice;
        }

        public void Draw()
        {
            drawShadowDepthMap();
            blurShadow(_shadowBlurTarg, _shadowDepthTarg, 0);
            blurShadow(_shadowDepthTarg, _shadowBlurTarg, 1);

            //DAN prepareMainPass();
        }

        private void drawShadowDepthMap()
        {
            // Calculate view and projection matrices for the "light"
            // shadows are being calculated for
            _shadowView = Matrix.CreateLookAt(ShadowLightPosition,
                                              ShadowLightTarget, Vector3.Up);

            _shadowProjection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45), 1, 1, ShadowFarPlane);

            // Set render target
            _graphicsDevice.SetRenderTarget(_shadowDepthTarg);

            // Clear the render target to 1 (infinite depth)
            _graphicsDevice.Clear(Color.White);

            // Draw each model with the ShadowDepthEffect effect
            foreach (var model in Models)
                model.Draw(Camera, _shadowDepthEffect);

            // Un-set the render targets
            _graphicsDevice.SetRenderTarget(null);
        }

        /*
        private void prepareMainPass()
        {
            foreach (var model in Models)
                foreach (ModelMesh mesh in model.Model.Meshes)
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        // Set the light map and viewport parameters to each model's effect
                        if (part.Effect.Parameters["LightTexture"] != null)
                            part.Effect.Parameters["LightTexture"].SetValue(_lightTarg);

                        if (part.Effect.Parameters["viewportWidth"] != null)
                            part.Effect.Parameters["viewportWidth"].SetValue(_viewWidth);

                        if (part.Effect.Parameters["viewportHeight"] != null)
                            part.Effect.Parameters["viewportHeight"].SetValue(_viewHeight);

                        if (part.Effect.Parameters["DoShadowMapping"] != null)
                            part.Effect.Parameters["DoShadowMapping"].SetValue(DoShadowMapping);

                        if (!DoShadowMapping)
                            continue;

                        if (part.Effect.Parameters["ShadowMap"] != null)
                            part.Effect.Parameters["ShadowMap"].SetValue(_shadowDepthTarg);

                        if (part.Effect.Parameters["ShadowView"] != null)
                            part.Effect.Parameters["ShadowView"].SetValue(_shadowView);

                        if (part.Effect.Parameters["ShadowProjection"] != null)
                            part.Effect.Parameters["ShadowProjection"].SetValue(_shadowProjection);

                        if (part.Effect.Parameters["ShadowLightPosition"] != null)
                            part.Effect.Parameters["ShadowLightPosition"].SetValue(ShadowLightPosition);

                        if (part.Effect.Parameters["ShadowFarPlane"] != null)
                            part.Effect.Parameters["ShadowFarPlane"].SetValue(ShadowFarPlane);

                        if (part.Effect.Parameters["ShadowMult"] != null)
                            part.Effect.Parameters["ShadowMult"].SetValue(ShadowMult);
                    }
        }
        */
        private void blurShadow(RenderTarget2D to, RenderTarget2D from, int dir)
        {
            // Set the target render target
            _graphicsDevice.SetRenderTarget(to);

            _graphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);

            // Start the Gaussian blur effect
            _shadowBlurEffect.CurrentTechnique.Passes[dir].Apply();

            // Draw the contents of the source render target so they can
            // be blurred by the gaussian blur pixel shader
            _spriteBatch.Draw(from, Vector2.Zero, Color.White);

            _spriteBatch.End();

            // Clean up after the sprite batch
            _graphicsDevice.BlendState = BlendState.Opaque;
            _graphicsDevice.DepthStencilState = DepthStencilState.Default;

            // Remove the render target
            _graphicsDevice.SetRenderTarget(null);
        }

    }

}
