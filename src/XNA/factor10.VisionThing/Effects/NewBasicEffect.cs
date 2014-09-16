using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factor10.VisionThing.Effects
{
    /// <summary>
    /// Built-in effect that supports optional texturing, vertex coloring, fog, and lighting.
    /// </summary>
    public class NewBasicEffect : IEffect
    {
        public Vector3 OriginalDiffuseColor;

        EffectParameter textureParam;
        EffectParameter diffuseColorParam;
        EffectParameter emissiveColorParam;
        EffectParameter specularColorParam;
        EffectParameter specularPowerParam;
        EffectParameter eyePositionParam;
        EffectParameter fogColorParam;
        EffectParameter fogVectorParam;
        EffectParameter worldParam;
        EffectParameter worldInverseTransposeParam;
        EffectParameter worldViewProjParam;
        EffectParameter shaderIndexParam;

        bool lightingEnabled;
        bool preferPerPixelLighting;
        bool oneLight;
        bool fogEnabled;
        bool textureEnabled;
        bool vertexColorEnabled;

        Matrix world = Matrix.Identity;
        Matrix view = Matrix.Identity;
        Matrix projection = Matrix.Identity;

        Matrix worldView;

        Vector3 diffuseColor = Vector3.One;
        Vector3 emissiveColor = Vector3.Zero;
        Vector3 ambientLightColor = Vector3.Zero;

        float alpha = 1;

        DirectionalLight light0;
        DirectionalLight light1;
        DirectionalLight light2;

        float fogStart = 0;
        float fogEnd = 1;

        private EffectDirtyFlags dirtyFlags = EffectDirtyFlags.All;

        /// <summary>
        /// Gets or sets the world matrix.
        /// </summary>
        public Matrix World
        {
            get { return world; }
            
            set
            {
                world = value;
                dirtyFlags |= EffectDirtyFlags.World | EffectDirtyFlags.WorldViewProj | EffectDirtyFlags.Fog;
            }
        }


        /// <summary>
        /// Gets or sets the view matrix.
        /// </summary>
        public Matrix View
        {
            get { return view; }
            
            set
            {
                view = value;
                dirtyFlags |= EffectDirtyFlags.WorldViewProj | EffectDirtyFlags.EyePosition | EffectDirtyFlags.Fog;
            }
        }


        /// <summary>
        /// Gets or sets the projection matrix.
        /// </summary>
        public Matrix Projection
        {
            get { return projection; }
            
            set
            {
                projection = value;
                dirtyFlags |= EffectDirtyFlags.WorldViewProj;
            }
        }

        public Vector3 CameraPosition
        {
            get { return Vector3.Zero; }
            set {  }
        }

        public Vector4? ClipPlane
        {
            set {  }
        }


        /// <summary>
        /// Gets or sets the material diffuse color (range 0 to 1).
        /// </summary>
        public Vector3 DiffuseColor
        {
            get { return diffuseColor; }
            
            set
            {
                diffuseColor = value;
                dirtyFlags |= EffectDirtyFlags.MaterialColor;
            }
        }


        /// <summary>
        /// Gets or sets the material emissive color (range 0 to 1).
        /// </summary>
        public Vector3 EmissiveColor
        {
            get { return emissiveColor; }
            
            set
            {
                emissiveColor = value;
                dirtyFlags |= EffectDirtyFlags.MaterialColor;
            }
        }


        /// <summary>
        /// Gets or sets the material specular color (range 0 to 1).
        /// </summary>
        public Vector3 SpecularColor
        {
            get { return specularColorParam.GetValueVector3(); }
            set { specularColorParam.SetValue(value); }
        }


        /// <summary>
        /// Gets or sets the material specular power.
        /// </summary>
        public float SpecularPower
        {
            get { return specularPowerParam.GetValueSingle(); }
            set { specularPowerParam.SetValue(value); }
        }


        /// <summary>
        /// Gets or sets the material alpha.
        /// </summary>
        public float Alpha
        {
            get { return alpha; }
            
            set
            {
                alpha = value;
                dirtyFlags |= EffectDirtyFlags.MaterialColor;
            }
        }


        /// <summary>
        /// Gets or sets the lighting enable flag.
        /// </summary>
        public bool LightingEnabled
        {
            get { return lightingEnabled; }
            
            set
            {
                if (lightingEnabled != value)
                {
                    lightingEnabled = value;
                    dirtyFlags |= EffectDirtyFlags.ShaderIndex | EffectDirtyFlags.MaterialColor;
                }
            }
        }


        /// <summary>
        /// Gets or sets the per-pixel lighting prefer flag.
        /// </summary>
        public bool PreferPerPixelLighting
        {
            get { return preferPerPixelLighting; }
            
            set
            {
                if (preferPerPixelLighting != value)
                {
                    preferPerPixelLighting = value;
                    dirtyFlags |= EffectDirtyFlags.ShaderIndex;
                }
            }
        }


        /// <summary>
        /// Gets or sets the ambient light color (range 0 to 1).
        /// </summary>
        public Vector3 AmbientLightColor
        {
            get { return ambientLightColor; }
            
            set
            {
                ambientLightColor = value;
                dirtyFlags |= EffectDirtyFlags.MaterialColor;
            }
        }


        /// <summary>
        /// Gets the first directional light.
        /// </summary>
        public DirectionalLight DirectionalLight0 { get { return light0; } }


        /// <summary>
        /// Gets the second directional light.
        /// </summary>
        public DirectionalLight DirectionalLight1 { get { return light1; } }


        /// <summary>
        /// Gets the third directional light.
        /// </summary>
        public DirectionalLight DirectionalLight2 { get { return light2; } }


        /// <summary>
        /// Gets or sets the fog enable flag.
        /// </summary>
        public bool FogEnabled
        {
            get { return fogEnabled; }
            
            set
            {
                if (fogEnabled != value)
                {
                    fogEnabled = value;
                    dirtyFlags |= EffectDirtyFlags.ShaderIndex | EffectDirtyFlags.FogEnable;
                }
            }
        }


        /// <summary>
        /// Gets or sets the fog start distance.
        /// </summary>
        public float FogStart
        {
            get { return fogStart; }
            
            set
            {
                fogStart = value;
                dirtyFlags |= EffectDirtyFlags.Fog;
            }
        }


        /// <summary>
        /// Gets or sets the fog end distance.
        /// </summary>
        public float FogEnd
        {
            get { return fogEnd; }
            
            set
            {
                fogEnd = value;
                dirtyFlags |= EffectDirtyFlags.Fog;
            }
        }


        /// <summary>
        /// Gets or sets the fog color.
        /// </summary>
        public Vector3 FogColor
        {
            get { return fogColorParam.GetValueVector3(); }
            set { fogColorParam.SetValue(value); }
        }


        /// <summary>
        /// Gets or sets whether texturing is enabled.
        /// </summary>
        public bool TextureEnabled
        {
            get { return textureEnabled; }
            
            set
            {
                if (textureEnabled != value)
                {
                    textureEnabled = value;
                    dirtyFlags |= EffectDirtyFlags.ShaderIndex;
                }
            }
        }


        /// <summary>
        /// Gets or sets the current texture.
        /// </summary>
        public Texture2D Texture
        {
            get { return textureParam.GetValueTexture2D(); }
            set { textureParam.SetValue(value); }
        }


        /// <summary>
        /// Gets or sets whether vertex color is enabled.
        /// </summary>
        public bool VertexColorEnabled
        {
            get { return vertexColorEnabled; }
            
            set
            {
                if (vertexColorEnabled != value)
                {
                    vertexColorEnabled = value;
                    dirtyFlags |= EffectDirtyFlags.ShaderIndex;
                }
            }
        }


        /// <summary>
        /// Creates a new BasicEffect with default parameter settings.
        /// </summary>
        public NewBasicEffect( Effect effect )
        {
            GraphicsDevice = effect.GraphicsDevice;
            Effect = effect;
            
            cacheEffectParameters(effect,null,null,null);

            DirectionalLight0.Enabled = true;

            SpecularColor = Vector3.One;
            SpecularPower = 16;
        }

        public GraphicsDevice GraphicsDevice
        {
            get; private set;
        }

        public Effect Effect
        {
            get; private set;
        }

        /// <summary>
        /// Creates a new BasicEffect by cloning parameter settings from an existing instance.
        /// </summary>
        protected NewBasicEffect( GraphicsDevice graphicsDevice, Effect effect, BasicEffect cloneSource)
        {
            GraphicsDevice = graphicsDevice;
            Effect = effect;

            cacheEffectParameters(
                cloneSource,
                cloneSource.DirectionalLight0,
                cloneSource.DirectionalLight1,
                cloneSource.DirectionalLight2);

            lightingEnabled = cloneSource.LightingEnabled;
            preferPerPixelLighting = cloneSource.PreferPerPixelLighting;
            fogEnabled = cloneSource.FogEnabled;
            textureEnabled = cloneSource.TextureEnabled;
            vertexColorEnabled = cloneSource.VertexColorEnabled;

            world = cloneSource.World;
            view = cloneSource.View;
            projection = cloneSource.Projection;

            diffuseColor = cloneSource.DiffuseColor;
            emissiveColor = cloneSource.EmissiveColor;
            ambientLightColor = cloneSource.AmbientLightColor;

            alpha = cloneSource.Alpha;

            fogStart = cloneSource.FogStart;
            fogEnd = cloneSource.FogEnd;
        }

        public void CopyBasicEffect(BasicEffect cloneSource)
        {
            cacheEffectParameters(
                cloneSource,
                cloneSource.DirectionalLight0,
                cloneSource.DirectionalLight1,
                cloneSource.DirectionalLight2);

            lightingEnabled = cloneSource.LightingEnabled;
            preferPerPixelLighting = cloneSource.PreferPerPixelLighting;
            fogEnabled = cloneSource.FogEnabled;
            textureEnabled = cloneSource.TextureEnabled;
            vertexColorEnabled = cloneSource.VertexColorEnabled;

            world = cloneSource.World;
            view = cloneSource.View;
            projection = cloneSource.Projection;

            diffuseColor = cloneSource.DiffuseColor;
            emissiveColor = cloneSource.EmissiveColor;
            ambientLightColor = cloneSource.AmbientLightColor;

            alpha = cloneSource.Alpha;

            fogStart = cloneSource.FogStart;
            fogEnd = cloneSource.FogEnd;
        }


        /// <summary>
        /// Sets up the standard key/fill/back lighting rig.
        /// </summary>
        public void EnableDefaultLighting()
        {
            LightingEnabled = true;
            AmbientLightColor = EffectHelpers.EnableDefaultLighting(light0, light1, light2);
        }


        /// <summary>
        /// Looks up shortcut references to our effect parameters.
        /// </summary>
        void cacheEffectParameters(
            Effect effect,
            DirectionalLight dlight0,
            DirectionalLight dlight1,
            DirectionalLight dlight2)
        {
            textureParam                = effect.Parameters["Texture"];
            diffuseColorParam           = effect.Parameters["DiffuseColor"];
            emissiveColorParam          = effect.Parameters["EmissiveColor"];
            specularColorParam          = effect.Parameters["SpecularColor"];
            specularPowerParam          = effect.Parameters["SpecularPower"];
            eyePositionParam            = effect.Parameters["EyePosition"];
            fogColorParam               = effect.Parameters["FogColor"];
            fogVectorParam              = effect.Parameters["FogVector"];
            worldParam                  = effect.Parameters["World"];
            worldInverseTransposeParam  = effect.Parameters["WorldInverseTranspose"];
            worldViewProjParam          = effect.Parameters["WorldViewProj"];
            shaderIndexParam            = effect.Parameters["ShaderIndex"];

            light0 = new DirectionalLight(effect.Parameters["DirLight0Direction"],
                                          effect.Parameters["DirLight0DiffuseColor"],
                                          effect.Parameters["DirLight0SpecularColor"],
                                          dlight0);

            light1 = new DirectionalLight(effect.Parameters["DirLight1Direction"],
                                          effect.Parameters["DirLight1DiffuseColor"],
                                          effect.Parameters["DirLight1SpecularColor"],
                                          dlight1);

            light2 = new DirectionalLight(effect.Parameters["DirLight2Direction"],
                                          effect.Parameters["DirLight2DiffuseColor"],
                                          effect.Parameters["DirLight2SpecularColor"],
                                          dlight2);
        }

        /// <summary>
        /// Lazily computes derived parameter values immediately before applying the effect.
        /// </summary>
        public void Apply()
        {
            // Recompute the world+view+projection matrix or fog vector?
            dirtyFlags = EffectHelpers.SetWorldViewProjAndFog(dirtyFlags, ref world, ref view, ref projection, ref worldView, fogEnabled, fogStart, fogEnd, worldViewProjParam, fogVectorParam);
            
            // Recompute the diffuse/emissive/alpha material color parameters?
            if ((dirtyFlags & EffectDirtyFlags.MaterialColor) != 0)
            {
                EffectHelpers.SetMaterialColor(lightingEnabled, alpha, ref diffuseColor, ref emissiveColor, ref ambientLightColor, diffuseColorParam, emissiveColorParam);

                dirtyFlags &= ~EffectDirtyFlags.MaterialColor;
            }

            if (lightingEnabled)
            {
                // Recompute the world inverse transpose and eye position?
                dirtyFlags = EffectHelpers.SetLightingMatrices(dirtyFlags, ref world, ref view, worldParam, worldInverseTransposeParam, eyePositionParam);
                
                // Check if we can use the only-bother-with-the-first-light shader optimization.
                var newOneLight = !light1.Enabled && !light2.Enabled;
                
                if (oneLight != newOneLight)
                {
                    oneLight = newOneLight;
                    dirtyFlags |= EffectDirtyFlags.ShaderIndex;
                }
            }

            // Recompute the shader index?
            if ((dirtyFlags & EffectDirtyFlags.ShaderIndex) != 0)
            {
                int shaderIndex = 0;
                
                if (!fogEnabled)
                    shaderIndex += 1;
                
                if (vertexColorEnabled)
                    shaderIndex += 2;
                
                if (textureEnabled)
                    shaderIndex += 4;

                if (lightingEnabled)
                {
                    if (preferPerPixelLighting)
                        shaderIndex += 24;
                    else if (oneLight)
                        shaderIndex += 16;
                    else
                        shaderIndex += 8;
                }

                shaderIndexParam.SetValue(shaderIndex);

                dirtyFlags &= ~EffectDirtyFlags.ShaderIndex;
            }
        }

        public EffectParameterCollection Parameters
        {
            get { return Effect.Parameters; }
        }

        public void SetShadowMapping(ShadowMap shadow)
        {
            //throw new System.NotImplementedException();
        }

        public void SetTechnique(DrawingReason drawingReason)
        {
            //throw new System.NotImplementedException();
        }
    }

}
