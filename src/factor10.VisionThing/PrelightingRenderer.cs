using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace factor10.VisionThing
{
    public class PrelightingRenderer
    {
        // Normal, depth, and light map render targets
        private readonly RenderTarget2D _depthTarg;
        private readonly RenderTarget2D _normalTarg;
        private readonly RenderTarget2D _lightTarg;

        // Depth/normal effect and light mapping effect
        private readonly Effect _depthNormalEffect;
        private readonly Effect _lightingEffect;

        // Point light (sphere) mesh
        private readonly Model _lightMesh;

        // List of models, lights, and the camera
        public List<ClipDrawable> Models { get; set; }
        public List<PPPointLight> Lights { get; set; }
        public Camera Camera { get; set; }

        private readonly GraphicsDevice _graphicsDevice;
        private readonly int _viewWidth = 0;
        private readonly int _viewHeight = 0;

        // Position and target of the shadowing light
        public Vector3 ShadowLightPosition { get; set; }
        public Vector3 ShadowLightTarget { get; set; }

        // Shadow depth target and depth-texture effect
        private readonly RenderTarget2D _shadowDepthTarg;
        private readonly Effect _shadowDepthEffect;

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

        public PrelightingRenderer(
            GraphicsDevice graphicsDevice,
            ContentManager content)
        {
            _viewWidth = graphicsDevice.Viewport.Width;
            _viewHeight = graphicsDevice.Viewport.Height;

            // Create the three render targets
            _depthTarg = new RenderTarget2D(graphicsDevice, _viewWidth,
                                            _viewHeight, false, SurfaceFormat.Single, DepthFormat.Depth24);

            _normalTarg = new RenderTarget2D(graphicsDevice, _viewWidth,
                                             _viewHeight, false, SurfaceFormat.Color, DepthFormat.Depth24);

            _lightTarg = new RenderTarget2D(graphicsDevice, _viewWidth,
                                            _viewHeight, false, SurfaceFormat.Color, DepthFormat.Depth24);

            // Load effects
            _depthNormalEffect = content.Load<Effect>("PPDepthNormal");
            _lightingEffect = content.Load<Effect>("PPLight");

            // Set effect parameters to light mapping effect
            _lightingEffect.Parameters["viewportWidth"].SetValue(_viewWidth);
            _lightingEffect.Parameters["viewportHeight"].SetValue(_viewHeight);

            // Load point light mesh and set light mapping effect to it
            _lightMesh = content.Load<Model>("PPLightMesh");
            _lightMesh.Meshes[0].MeshParts[0].Effect = _lightingEffect;

            _shadowDepthTarg = new RenderTarget2D(graphicsDevice, ShadowMapSize,
                                                  ShadowMapSize, false, SurfaceFormat.HalfVector2, DepthFormat.Depth24);

            _shadowDepthEffect = content.Load<Effect>("ShadowDepthEffect");
            _shadowDepthEffect.Parameters["FarPlane"].SetValue(ShadowFarPlane);

            _spriteBatch = new SpriteBatch(graphicsDevice);
            _shadowBlurEffect = content.Load<Effect>("GaussianBlur");

            _shadowBlurTarg = new RenderTarget2D(graphicsDevice, ShadowMapSize,
                                                 ShadowMapSize, false, SurfaceFormat.HalfVector2, DepthFormat.Depth24);

            _graphicsDevice = graphicsDevice;
        }

        public void Draw()
        {
            drawDepthNormalMap();
            drawLightMap();

            if (DoShadowMapping)
            {
                drawShadowDepthMap();
                blurShadow(_shadowBlurTarg, _shadowDepthTarg, 0);
                blurShadow(_shadowDepthTarg, _shadowBlurTarg, 1);
            }

            //DAN prepareMainPass();
        }

        private void drawDepthNormalMap()
        {
            // Set the render targets to 'slots' 1 and 2
            _graphicsDevice.SetRenderTargets(_normalTarg, _depthTarg);

            // Clear the render target to 1 (infinite depth)
            _graphicsDevice.Clear(Color.White);

            // Draw each model with the PPDepthNormal effect
            foreach (var model in Models)
            {
                //model.CacheEffects();
                //model.SetModelEffect(_depthNormalEffect, false);
                //model.Draw(Camera.View, Camera.Projection,
                //           Camera.Position);
                //model.RestoreEffects();
            }

            // Un-set the render targets
            _graphicsDevice.SetRenderTargets(null);
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
            {
            //    model.CacheEffects();
            //    model.SetModelEffect(_shadowDepthEffect, false);
            //    model.Draw(_shadowView, _shadowProjection, ShadowLightPosition);
            //    model.RestoreEffects();
            }

            // Un-set the render targets
            _graphicsDevice.SetRenderTarget(null);
        }

        private void drawLightMap()
        {
            // Set the depth and normal map info to the effect
            _lightingEffect.Parameters["DepthTexture"].SetValue(_depthTarg);
            _lightingEffect.Parameters["NormalTexture"].SetValue(_normalTarg);

            // Calculate the view * projection matrix
            Matrix viewProjection = Camera.View*Camera.Projection;

            // Set the inverse of the view * projection matrix to the effect
            Matrix invViewProjection = Matrix.Invert(viewProjection);
            _lightingEffect.Parameters["InvViewProjection"].SetValue(
                invViewProjection);

            // Set the render target to the graphics device
            _graphicsDevice.SetRenderTarget(_lightTarg);

            // Clear the render target to black (no light)
            _graphicsDevice.Clear(Color.Black);

            // Set render states to additive (lights will add their influences)
            _graphicsDevice.BlendState = BlendState.Additive;
            _graphicsDevice.DepthStencilState = DepthStencilState.None;

            foreach (PPPointLight light in Lights)
            {
                // Set the light's parameters to the effect
                light.SetEffectParameters(_lightingEffect);

                // Calculate the world * view * projection matrix and set it to 
                // the effect
                var wvp = Matrix.CreateScale(light.Attenuation)*Matrix.CreateTranslation(light.Position)*viewProjection;

                _lightingEffect.Parameters["WorldViewProjection"].SetValue(wvp);

                // Determine the distance between the light and camera
                var dist = Vector3.Distance(Camera.Position, light.Position);

                // If the camera is inside the light-sphere, invert the cull mode
                // to draw the inside of the sphere instead of the outside
                if (dist < light.Attenuation)
                    _graphicsDevice.RasterizerState = RasterizerState.CullClockwise;

                // Draw the point-light-sphere
                _lightMesh.Meshes[0].Draw();

                // Revert the cull mode
                _graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            }

            // Revert the blending and depth render states
            _graphicsDevice.BlendState = BlendState.Opaque;
            _graphicsDevice.DepthStencilState = DepthStencilState.Default;

            // Un-set the render target
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
