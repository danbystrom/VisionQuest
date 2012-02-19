using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing.Primitives;

namespace factor10.VisionThing.Water
{
    public struct Mtrl
    {
        public Vector4 Ambient;
        public Vector4 Diffuse;
        public Vector4 Spec;
        public float SpecPower;
    }

    public struct DirLight
    {
        public Vector4 Ambient;
        public Vector4 Diffuse;
        public Vector4 Spec;
        public Vector3 DirW;
    }

    public struct InitInfo
    {
        public DirLight DirLight;
        public Mtrl Mtrl;
        public Effect Fx;
        public int Rows;
        public int Columns;
        public float dx;
        public float dz;
        public Texture waveMap0;
        public Texture waveMap1;
        public Texture dmap0;
        public Texture dmap1;
        public Vector2 waveNMapVelocity0;
        public Vector2 waveNMapVelocity1;
        public Vector2 waveDMapVelocity0;
        public Vector2 waveDMapVelocity1;
        public Vector2 scaleHeights;
        public float texScale;
    }

    public class WaterSurface
    {
        private InitInfo _initInfo;

        // Offset of normal maps for scrolling (vary as a function of time)
        private Vector2 _waveNMapOffset0;
        private Vector2 _waveNMapOffset1;

        // Offset of displacement maps for scrolling (vary as a function of time)
        private Vector2 _waveDMapOffset0;
        private Vector2 _waveDMapOffset1;

        private readonly PlanePrimitive<WaterVertex> _field;

        public readonly RenderTarget2D _reflectionTarget;

        public readonly List<ClipDrawable> ReflectedObjects = new List<ClipDrawable>();
        private readonly Camera _reflectionCamera;

        public WaterSurface(
            GraphicsDevice graphicsDevice,
            InitInfo initInfo)
        {
            _initInfo = initInfo;

            _field = new PlanePrimitive<WaterVertex>(
                graphicsDevice,
                initInfo.Fx,
                (x, y) => new WaterVertex
                              {
                                  Position = new Vector3(x*initInfo.dx, 0, y*initInfo.dz),
                                  ScaledTexC = new Vector2(x/initInfo.Columns, y/initInfo.Rows)*initInfo.texScale,
                                  NormalizedTexC = new Vector2(x/initInfo.Columns, y/initInfo.Rows)
                              },
                initInfo.Columns,
                initInfo.Rows);


            buildFx();

            _reflectionTarget = new RenderTarget2D(
                graphicsDevice,
                graphicsDevice.Viewport.Width/2,
                graphicsDevice.Viewport.Height/2,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24);

            _reflectionCamera = new Camera(
               new Vector2( _reflectionTarget.Width,_reflectionTarget.Height ), 
                Vector3.Zero,
                Vector3.Up);
        }

        public void Update(float dt)
        {
            // Update texture coordinate offsets.  These offsets are added to the
            // texture coordinates in the vertex shader to animate them.
            _waveNMapOffset0 += _initInfo.waveNMapVelocity0*dt;
            _waveNMapOffset1 += _initInfo.waveNMapVelocity1*dt;

            _waveDMapOffset0 += _initInfo.waveDMapVelocity0*dt;
            _waveDMapOffset1 += _initInfo.waveDMapVelocity1*dt;

            // Textures repeat every 1.0 unit, so reset back down to zero
            // so the coordinates do not grow too large.
            if (_waveNMapOffset0.X >= 1.0f || _waveNMapOffset0.X <= -1.0f)
                _waveNMapOffset0.X = 0.0f;
            if (_waveNMapOffset1.X >= 1.0f || _waveNMapOffset1.X <= -1.0f)
                _waveNMapOffset1.X = 0.0f;
            if (_waveNMapOffset0.Y >= 1.0f || _waveNMapOffset0.Y <= -1.0f)
                _waveNMapOffset0.Y = 0.0f;
            if (_waveNMapOffset1.Y >= 1.0f || _waveNMapOffset1.Y <= -1.0f)
                _waveNMapOffset1.Y = 0.0f;

            if (_waveDMapOffset0.X >= 1.0f || _waveDMapOffset0.X <= -1.0f)
                _waveDMapOffset0.X = 0.0f;
            if (_waveDMapOffset1.X >= 1.0f || _waveDMapOffset1.X <= -1.0f)
                _waveDMapOffset1.X = 0.0f;
            if (_waveDMapOffset0.Y >= 1.0f || _waveDMapOffset0.Y <= -1.0f)
                _waveDMapOffset0.Y = 0.0f;
            if (_waveDMapOffset1.Y >= 1.0f || _waveDMapOffset1.Y <= -1.0f)
                _waveDMapOffset1.Y = 0.0f;
        }

        public void Draw(Camera camera, Matrix world, bool fast)
        {
            _mhWorld.SetValue(world);
            _mhWorldInv.SetValue(Matrix.Invert(world));
            _mhView.SetValue(camera.View);
            _mhProjection.SetValue(camera.Projection);
 
            _mhEyePosW.SetValue(camera.Position);
            _mhWaveNMapOffset0.SetValue(_waveNMapOffset0);
            _mhWaveNMapOffset1.SetValue(_waveNMapOffset1);
            _mhWaveDMapOffset0.SetValue(_waveDMapOffset0);
            _mhWaveDMapOffset1.SetValue(_waveDMapOffset1);

            _initInfo.Fx.CurrentTechnique = fast ? _fastEffect : _qualityEffect;
            _field.Draw(_initInfo.Fx);
        }

        private EffectParameter _mhWorld;
        private EffectParameter _mhWorldInv;
        private EffectParameter _mhView;
        private EffectParameter _mhProjection;
        private EffectParameter _mhEyePosW;
        private EffectParameter _mhWaveMap0;
        private EffectParameter _mhWaveMap1;
        private EffectParameter _mhWaveNMapOffset0;
        private EffectParameter _mhWaveNMapOffset1;
        private EffectParameter _mhWaveDMapOffset0;
        private EffectParameter _mhWaveDMapOffset1;
        private EffectParameter _mhWaveDispMap0;
        private EffectParameter _mhWaveDispMap1;

        private EffectTechnique _qualityEffect;
        private EffectTechnique _fastEffect;

        private void buildFx()
        {
            var fx = _initInfo.Fx;

            _mhWorld = fx.Parameters["World"];
            _mhWorldInv = fx.Parameters["WorldInv"];
            _mhView = fx.Parameters["View"];
            _mhProjection = fx.Parameters["Projection"];
            _mhEyePosW = fx.Parameters["gEyePosW"];
            _mhWaveMap0 = fx.Parameters["gWaveMap0"];
            _mhWaveMap1 = fx.Parameters["gWaveMap1"];
            _mhWaveNMapOffset0 = fx.Parameters["gWaveNMapOffset0"];
            _mhWaveNMapOffset1 = fx.Parameters["gWaveNMapOffset1"];
            _mhWaveDMapOffset0 = fx.Parameters["gWaveDMapOffset0"];
            _mhWaveDMapOffset1 = fx.Parameters["gWaveDMapOffset1"];
            _mhWaveDispMap0 = fx.Parameters["gWaveDispMap0"];
            _mhWaveDispMap1 = fx.Parameters["gWaveDispMap1"];

            _mhWaveMap0.SetValue(_initInfo.waveMap0);
            _mhWaveMap1.SetValue(_initInfo.waveMap1);
            _mhWaveDispMap0.SetValue(_initInfo.dmap0);
            _mhWaveDispMap1.SetValue(_initInfo.dmap1);
            fx.Parameters["gLightAmbient"].SetValue(_initInfo.DirLight.Ambient);
            fx.Parameters["gLightDiffuse"].SetValue(_initInfo.DirLight.Diffuse);
            fx.Parameters["gLightDirW"].SetValue(_initInfo.DirLight.DirW);
            fx.Parameters["gLightSpec"].SetValue(_initInfo.DirLight.Spec);
            fx.Parameters["gMtrlAmbient"].SetValue(_initInfo.Mtrl.Ambient);
            fx.Parameters["gMtrlDiffuse"].SetValue(_initInfo.Mtrl.Diffuse);
            fx.Parameters["gMtrlSpec"].SetValue(_initInfo.Mtrl.Spec);
            fx.Parameters["gMtrlSpecPower"].SetValue(_initInfo.Mtrl.SpecPower);
            fx.Parameters["gScaleHeights"].SetValue(_initInfo.scaleHeights);
            fx.Parameters["gGridStepSizeL"].SetValue(new Vector2(_initInfo.dx, _initInfo.dz));

            _qualityEffect = fx.Techniques[0];
            _fastEffect = fx.Techniques[1];
        }

        public void RenderReflection(Camera camera)
        {
            const float waterMeshPositionY = 0.5f;
            var graphics = _initInfo.Fx.GraphicsDevice;

            // Reflect the camera's properties across the water plane
            var reflectedCameraPosition = camera.Position;
            reflectedCameraPosition.Y = -reflectedCameraPosition.Y + waterMeshPositionY*2;

            var reflectedCameraTarget = camera.Target;
            reflectedCameraTarget.Y = -reflectedCameraTarget.Y + waterMeshPositionY*2;

            reflectedCameraPosition = reflectedCameraPosition + (reflectedCameraPosition - reflectedCameraTarget) * 1.2f;

           _reflectionCamera.Update(
                reflectedCameraPosition,
                reflectedCameraTarget);

            // Create the clip plane
            var clipPlane = new Vector4(0, 1, 0, waterMeshPositionY);

            // Set the render target
            graphics.SetRenderTarget(_reflectionTarget);
            graphics.Clear(new Color(110,130,190));

            // Draw all objects with clip plane
            foreach (var cd in ReflectedObjects)
                cd.Draw(_reflectionCamera, clipPlane);

            graphics.SetRenderTarget(null);

            //reflectedCameraPosition = camera.Position;
            //reflectedCameraPosition.Y = -reflectedCameraPosition.Y + waterMeshPositionY * 2;

             _initInfo.Fx.Parameters["ReflectedView"].SetValue(_reflectionCamera.View);
            _initInfo.Fx.Parameters["ReflectedMap"].SetValue(_reflectionTarget);
        }

    }

}