using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factor10.VisionThing.Effects
{
    public class PlainEffectWrapper : IEffect
    {
        public GraphicsDevice GraphicsDevice { get; private set; }
        public Effect Effect { get; private set; }

        public readonly string Name;

        protected readonly EffectParameter _epWorld;
        protected readonly EffectParameter _epView;
        protected readonly EffectParameter _epProjection;
        protected readonly EffectParameter _epCameraPosition;
        protected readonly EffectParameter _epClipPlane;
        protected readonly EffectParameter _epLightingDirection;
        protected readonly EffectParameter _epTexture;

        protected readonly EffectParameter _epDoShadowMapping;
        protected readonly EffectParameter _epShadowMap;
        protected readonly EffectParameter _epShadowViewProjection;
        protected readonly EffectParameter _epShadowFarPlane;
        protected readonly EffectParameter _epShadowMult;

        protected readonly EffectTechnique _techStandard;
        protected readonly EffectTechnique _techClipPlane;
        protected readonly EffectTechnique _techDepthMap;

        public PlainEffectWrapper( Effect effect, string name )
        {
            GraphicsDevice = effect.GraphicsDevice;
            Effect = effect;
            Name = name;

            _epWorld = effect.Parameters["World"];
            _epView = effect.Parameters["View"];
            _epProjection = effect.Parameters["Projection"];
            _epCameraPosition = effect.Parameters["CameraPosition"];
            _epClipPlane = effect.Parameters["ClipPlane"];
            _epLightingDirection = effect.Parameters["LightingDirection"];
            _epTexture = effect.Parameters["Texture"];

            _epDoShadowMapping = effect.Parameters["DoShadowMapping"];
            _epShadowMap = effect.Parameters["ShadowMap"];
            _epShadowViewProjection = effect.Parameters["ShadowViewProjection"];
            _epShadowFarPlane = effect.Parameters["ShadowFarPlane"];
            _epShadowMult = effect.Parameters["ShadowMult"];

            _techStandard = effect.Techniques["TechStandard"];
            _techClipPlane = effect.Techniques["TechClipPlane"];
            _techDepthMap = effect.Techniques["TechDepthMap"];

            Debug.Assert( _epView != null );
            //Debug.Assert(_techDepthMap != null);

            if ( _epLightingDirection != null)
                _epLightingDirection.SetValue(VisionContent.SunlightDirection);
        }

        public Matrix World
        {
            get { return _epWorld.GetValueMatrix(); }
            set { _epWorld.SetValue(value); }
        }

        public Matrix View
        {
            get { return _epView.GetValueMatrix(); }
            set { _epView.SetValue(value); }
        }

        public Matrix Projection
        {
            get { return _epProjection.GetValueMatrix(); }
            set { _epProjection.SetValue(value); }
        }

        public Vector3 CameraPosition
        {
            get { return _epCameraPosition.GetValueVector3(); }
            set
            {
                if ( _epCameraPosition != null)
                    _epCameraPosition.SetValue(value);
            }
        }

        public Vector4? ClipPlane
        {
            set
            {
                if ( _epClipPlane== null )
                    return;
                if (value.HasValue)
                {
                    _epClipPlane.SetValue(value.Value);
                    Effect.CurrentTechnique = _techClipPlane;
                }
                else
                    Effect.CurrentTechnique = _techStandard;
            }
        }

        public Texture2D Texture
        {
            get { return _epTexture.GetValueTexture2D(); }
            set { _epTexture.SetValue(value); }
        }

        public void Apply()
        {
        }

        public EffectParameterCollection Parameters
        {
            get { return Effect.Parameters; }
        }

        public void SetShadowMapping(ShadowMap shadow)
        {
            if (_epDoShadowMapping == null)
                return;  //this effect cannot do shadow mapping

            if (shadow != null)
            {
                _epDoShadowMapping.SetValue(true);
                _epShadowMap.SetValue(shadow.ShadowDepthTarget);
                _epShadowViewProjection.SetValue(shadow.Camera.View * shadow.Camera.Projection);
                _epShadowMult.SetValue(shadow.ShadowMult);
            }
            else
                _epDoShadowMapping.SetValue(false);
        }

        public void SetTechnique(DrawingReason drawingReason )
        {
            switch ( drawingReason )
            {
                case DrawingReason.Normal:
                    Effect.CurrentTechnique = _techStandard;
                    break;
                case DrawingReason.ReflectionMap:
                    Effect.CurrentTechnique = _techClipPlane;
                    break;
                case DrawingReason.ShadowDepthMap:
                    Effect.CurrentTechnique = _techDepthMap;
                    break;
            }
        }
    }

}
