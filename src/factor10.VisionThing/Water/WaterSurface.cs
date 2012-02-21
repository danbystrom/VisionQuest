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
        // Offset of normal maps for scrolling (vary as a function of time)
        private Vector2 _waveNMapOffset0;
        private Vector2 _waveNMapOffset1;
        private readonly Vector2 _waveNMapVelocity0;
        private readonly Vector2 _waveNMapVelocity1;

        // Offset of displacement maps for scrolling (vary as a function of time)
        private Vector2 _waveDMapOffset0;
        private Vector2 _waveDMapOffset1;
        private readonly Vector2 _waveDMapVelocity0;
        private readonly Vector2 _waveDMapVelocity1;

        private readonly PlanePrimitive<WaterVertex> _field;

        public readonly RenderTarget2D _reflectionTarget;

        public readonly List<ClipDrawable> ReflectedObjects = new List<ClipDrawable>();
        private readonly Camera _reflectionCamera;

        public Effect Effect;

        public WaterSurface(
            GraphicsDevice graphicsDevice,
            InitInfo initInfo)
        {
            Effect = initInfo.Fx;

            _waveNMapVelocity0 = initInfo.waveNMapVelocity0;
            _waveNMapVelocity1 = initInfo.waveNMapVelocity1;
            _waveDMapVelocity0 = initInfo.waveDMapVelocity0;
            _waveDMapVelocity1 = initInfo.waveDMapVelocity1;

            _field = new PlanePrimitive<WaterVertex>(
                graphicsDevice,
                (x, y) => new WaterVertex
                              {
                                  Position = new Vector3(x*initInfo.dx, 0, y*initInfo.dz),
                                  ScaledTexC = new Vector2(x/initInfo.Columns, y/initInfo.Rows)*initInfo.texScale,
                                  NormalizedTexC = new Vector2(x/initInfo.Columns, y/initInfo.Rows)
                              },
                initInfo.Columns,
                initInfo.Rows);


            buildFx(initInfo);

            _reflectionTarget = new RenderTarget2D(
                graphicsDevice,
                graphicsDevice.Viewport.Width/2,
                graphicsDevice.Viewport.Height/2,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24);

            _reflectionCamera = new Camera(
                new Vector2(_reflectionTarget.Width, _reflectionTarget.Height),
                Vector3.Zero,
                Vector3.Up);
        }

        public void Update(float dt)
        {
            // Update texture coordinate offsets.  These offsets are added to the
            // texture coordinates in the vertex shader to animate them.
            _waveNMapOffset0 += _waveNMapVelocity0*dt;
            _waveNMapOffset1 += _waveNMapVelocity1*dt;

            _waveDMapOffset0 += _waveDMapVelocity0*dt;
            _waveDMapOffset1 += _waveDMapVelocity1*dt;

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

            Effect.CurrentTechnique = fast ? _fastEffect : _qualityEffect;
            _field.Draw(Effect);
        }

        private EffectParameter _mhWorld;
        private EffectParameter _mhWorldInv;
        private EffectParameter _mhView;
        private EffectParameter _mhProjection;
        private EffectParameter _mhEyePosW;
        private EffectParameter _mhWaveNMapOffset0;
        private EffectParameter _mhWaveNMapOffset1;
        private EffectParameter _mhWaveDMapOffset0;
        private EffectParameter _mhWaveDMapOffset1;

        private EffectTechnique _qualityEffect;
        private EffectTechnique _fastEffect;

        private void buildFx(InitInfo initInfo)
        {
            _mhWorld = Effect.Parameters["World"];
            _mhWorldInv = Effect.Parameters["WorldInv"];
            _mhView = Effect.Parameters["View"];
            _mhProjection = Effect.Parameters["Projection"];
            _mhEyePosW = Effect.Parameters["gEyePosW"];
            _mhWaveNMapOffset0 = Effect.Parameters["gWaveNMapOffset0"];
            _mhWaveNMapOffset1 = Effect.Parameters["gWaveNMapOffset1"];
            _mhWaveDMapOffset0 = Effect.Parameters["gWaveDMapOffset0"];
            _mhWaveDMapOffset1 = Effect.Parameters["gWaveDMapOffset1"];

            Effect.Parameters["gWaveMap0"].SetValue(initInfo.waveMap0);
            Effect.Parameters["gWaveMap1"].SetValue(initInfo.waveMap1);
            Effect.Parameters["gWaveDispMap0"].SetValue(initInfo.dmap0);
            Effect.Parameters["gWaveDispMap1"].SetValue(initInfo.dmap1);
            Effect.Parameters["gLightAmbient"].SetValue(initInfo.DirLight.Ambient);
            Effect.Parameters["gLightDiffuse"].SetValue(initInfo.DirLight.Diffuse);
            Effect.Parameters["gLightDirW"].SetValue(initInfo.DirLight.DirW);
            Effect.Parameters["gLightSpec"].SetValue(initInfo.DirLight.Spec);
            Effect.Parameters["gMtrlAmbient"].SetValue(initInfo.Mtrl.Ambient);
            Effect.Parameters["gMtrlDiffuse"].SetValue(initInfo.Mtrl.Diffuse);
            Effect.Parameters["gMtrlSpec"].SetValue(initInfo.Mtrl.Spec);
            Effect.Parameters["gMtrlSpecPower"].SetValue(initInfo.Mtrl.SpecPower);
            Effect.Parameters["gScaleHeights"].SetValue(initInfo.scaleHeights);
            Effect.Parameters["gGridStepSizeL"].SetValue(new Vector2(initInfo.dx, initInfo.dz));

            _qualityEffect = Effect.Techniques[0];
            _fastEffect = Effect.Techniques[1];
        }

        public void RenderReflection(Camera camera)
        {
            const float waterMeshPositionY = 0.5f; //experimenting with this

            // Reflect the camera's properties across the water plane
            var reflectedCameraPosition = camera.Position;
            reflectedCameraPosition.Y = -reflectedCameraPosition.Y + waterMeshPositionY*2;
            var reflectedCameraTarget = camera.Target;
            reflectedCameraTarget.Y = -reflectedCameraTarget.Y + waterMeshPositionY*2;

            // move reflection camera a bit away in order to get more of the scene into the reflection target
            //reflectedCameraPosition = reflectedCameraPosition + (reflectedCameraPosition - reflectedCameraTarget)*1.2f;

            _reflectionCamera.Update(
                reflectedCameraPosition,
                reflectedCameraTarget);

            Effect.GraphicsDevice.SetRenderTarget(_reflectionTarget);

            var clipPlane = new Vector4(0, 1, 0, waterMeshPositionY);
            foreach (var cd in ReflectedObjects)
                cd.Draw(_reflectionCamera, clipPlane);

            Effect.GraphicsDevice.SetRenderTarget(null);

            Effect.Parameters["ReflectedView"].SetValue(_reflectionCamera.View);
            Effect.Parameters["ReflectedMap"].SetValue(_reflectionTarget);
        }

    }

}