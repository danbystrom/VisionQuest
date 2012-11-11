using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing.Effects;
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
        public int SquareSize;
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

        private readonly PlanePrimitive<WaterVertex> _hiPolyPlane;
        private readonly PlanePrimitive<WaterVertex> _mediumPolyPlane;
        private readonly PlanePrimitive<WaterVertex> _loPolyPlane;

        public readonly RenderTarget2D _reflectionTarget;

        public readonly List<ClipDrawable> ReflectedObjects = new List<ClipDrawable>();
        private readonly Camera _reflectionCamera;

        public IEffect Effect;

        public WaterSurface(
            GraphicsDevice graphicsDevice,
            InitInfo initInfo)
        {
            Effect = new PlainEffectWrapper( initInfo.Fx );

            _waveNMapVelocity0 = initInfo.waveNMapVelocity0;
            _waveNMapVelocity1 = initInfo.waveNMapVelocity1;
            _waveDMapVelocity0 = initInfo.waveDMapVelocity0;
            _waveDMapVelocity1 = initInfo.waveDMapVelocity1;

            _hiPolyPlane = generatePlane(graphicsDevice, initInfo.SquareSize, initInfo.dx, initInfo.dz,
                                         initInfo.texScale);
            _mediumPolyPlane = generatePlane(graphicsDevice, initInfo.SquareSize/4, initInfo.dx * 4, initInfo.dz * 4,
                                         initInfo.texScale);
            _loPolyPlane = generatePlane(graphicsDevice, initInfo.SquareSize / 16, initInfo.dx * 16, initInfo.dz * 16,
                                         initInfo.texScale);

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

        private PlanePrimitive<WaterVertex> generatePlane(GraphicsDevice graphicsDevice, int squareSize, float dx, float dz, float texScale)
        {
            return new PlanePrimitive<WaterVertex>(
                graphicsDevice,
                (x, y) => new WaterVertex
                              {
                                  Position = new Vector3(x*dx, 0, y*dz),
                                  ScaledTexC = new Vector2(x / squareSize, y / squareSize) * texScale,
                                  NormalizedTexC = new Vector2(x / squareSize, y / squareSize)
                              },
                squareSize,
                squareSize);
        }

        public void Update(float dt, Camera camera)
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

        public void Draw(Camera camera, Matrix world, float distance)
        {
            _mhWorld.SetValue(world);
            _mhWorldInv.SetValue(Matrix.Invert(world));
            _mhView.SetValue(camera.View);
            _mhProjection.SetValue(camera.Projection);
            _mhCameraPosition.SetValue(camera.Position);

            _mhWaveNMapOffset0.SetValue(_waveNMapOffset0);
            _mhWaveNMapOffset1.SetValue(_waveNMapOffset1);
            _mhWaveDMapOffset0.SetValue(_waveDMapOffset0);
            _mhWaveDMapOffset1.SetValue(_waveDMapOffset1);

            if (distance < 10000)
                _hiPolyPlane.Draw(Effect);
            else if (distance < 2000000)
                _mediumPolyPlane.Draw(Effect);
            else
                _loPolyPlane.Draw(Effect);
        }


        private EffectParameter _mhWorld;
        private EffectParameter _mhWorldInv;
        private EffectParameter _mhView;
        private EffectParameter _mhProjection;
        private EffectParameter _mhCameraPosition;
        private EffectParameter _mhWaveNMapOffset0;
        private EffectParameter _mhWaveNMapOffset1;
        private EffectParameter _mhWaveDMapOffset0;
        private EffectParameter _mhWaveDMapOffset1;

        private void buildFx(InitInfo initInfo)
        {
            var p = Effect.Effect.Parameters;

            _mhWorld = p["World"];
            _mhWorldInv = p["WorldInv"];
            _mhView = p["View"];
            _mhProjection = p["Projection"];
            _mhCameraPosition = p["gCameraPosition"];
            _mhWaveNMapOffset0 = p["gWaveNMapOffset0"];
            _mhWaveNMapOffset1 = p["gWaveNMapOffset1"];
            _mhWaveDMapOffset0 = p["gWaveDMapOffset0"];
            _mhWaveDMapOffset1 = p["gWaveDMapOffset1"];


            p["gWaveMap0"].SetValue(initInfo.waveMap0);
            p["gWaveMap1"].SetValue(initInfo.waveMap1);
            p["gWaveDispMap0"].SetValue(initInfo.dmap0);
            p["gWaveDispMap1"].SetValue(initInfo.dmap1);
            p["gLightAmbient"].SetValue(initInfo.DirLight.Ambient);
            p["gLightDiffuse"].SetValue(initInfo.DirLight.Diffuse);
            p["gLightDirW"].SetValue(initInfo.DirLight.DirW);
            p["gLightSpec"].SetValue(initInfo.DirLight.Spec);
            p["gMtrlAmbient"].SetValue(initInfo.Mtrl.Ambient);
            p["gMtrlDiffuse"].SetValue(initInfo.Mtrl.Diffuse);
            p["gMtrlSpec"].SetValue(initInfo.Mtrl.Spec);
            p["gMtrlSpecPower"].SetValue(initInfo.Mtrl.SpecPower);
            p["gScaleHeights"].SetValue(initInfo.scaleHeights);
            p["gGridStepSizeL"].SetValue(new Vector2(initInfo.dx, initInfo.dz));
        }

        public void RenderReflection(Camera camera)
        {
            const float waterMeshPositionY = 0.5f; //experimenting with this

            // Reflect the camera's properties across the water plane
            var reflectedCameraPosition = camera.Position;
            reflectedCameraPosition.Y = -reflectedCameraPosition.Y + waterMeshPositionY*2;
            var reflectedCameraTarget = camera.Target;
            reflectedCameraTarget.Y = -reflectedCameraTarget.Y + waterMeshPositionY*2;

            //reflectedCameraPosition = reflectedCameraPosition + (reflectedCameraPosition - reflectedCameraTarget)*1f;
 
            _reflectionCamera.Update(
                reflectedCameraPosition,
                reflectedCameraTarget);

            Effect.GraphicsDevice.SetRenderTarget(_reflectionTarget);

            var clipPlane = new Vector4(0, 1, 0, -waterMeshPositionY);
            foreach (var cd in ReflectedObjects)
                cd.Draw(_reflectionCamera, clipPlane);

            Effect.GraphicsDevice.SetRenderTarget(null);

            Effect.Effect.Parameters["ReflectedView"].SetValue(_reflectionCamera.View);
            Effect.Effect.Parameters["ReflectedMap"].SetValue(_reflectionTarget);
        }

    }

}