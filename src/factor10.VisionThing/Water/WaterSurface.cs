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
        public Texture LakeBumpMap;
        public Texture Checker;
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
        private readonly PlanePrimitive<WaterVertex> _lakePlane;

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

            _lakePlane = generatePlane(graphicsDevice, 1, 256, 256, 64);

            buildFx(initInfo);

            var targetWidth = graphicsDevice.Viewport.Width;
            var targetHeight = graphicsDevice.Viewport.Height;
            _reflectionTarget = new RenderTarget2D(
                graphicsDevice,
                targetWidth,
                targetHeight*11/10,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24);

            _reflectionCamera = new Camera(
                new Vector2(targetWidth, targetHeight),
                Vector3.Zero,
                Vector3.Up);
        }

        private PlanePrimitive<WaterVertex> generatePlane(GraphicsDevice graphicsDevice, int squareSize, float dx, float dz, float texScale)
        {
            return new PlanePrimitive<WaterVertex>(
                graphicsDevice,
                (x, y, width, height) => new WaterVertex
                              {
                                  Position = new Vector3(x*dx, 0, y*dz),
                                  NormalizedTexC = new Vector2(x / squareSize, y / squareSize),
                                  ScaledTexC = new Vector2(x / squareSize, y / squareSize) * texScale,
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
            wrap(ref _waveNMapOffset0);
            wrap(ref _waveNMapOffset1);
            wrap(ref _waveDMapOffset0);
            wrap(ref _waveDMapOffset1);

            _time += dt;
        }

        private void wrap(ref Vector2 vec)
        {
            if (vec.X >= 2.0f || vec.X <= -2.0f)
                vec.X -= 2*Math.Sign(_waveNMapOffset0.X);
            if (vec.Y >= 2.0f || vec.Y <= -2.0f)
                vec.Y -= 2*Math.Sign(vec.Y);
        }

        private float _time;

        public void Draw(Camera camera, float scale, Vector3 pos, float distance, int dx, int dy)
        {
            var world = Matrix.CreateTranslation(pos);
            _mhWorld.SetValue(world);
            _mhWorldInv.SetValue(Matrix.Invert(world));
            _mhView.SetValue(camera.View);
            _mhProjection.SetValue(camera.Projection);
            _mhCameraPosition.SetValue(camera.Position);

            _mhWaveNMapOffset0.SetValue(_waveNMapOffset0);
            _mhWaveNMapOffset1.SetValue(_waveNMapOffset1);
            _mhWaveDMapOffset0.SetValue(_waveDMapOffset0);
            _mhWaveDMapOffset1.SetValue(_waveDMapOffset1);

            Effect.Parameters["WaveLength"].SetValue(0.1f);
            Effect.Parameters["WaveHeight"].SetValue(0.3f*2);
            Effect.Parameters["WindForce"].SetValue(0.002f);
            Effect.Parameters["Time"].SetValue(_time*10);
            Effect.Parameters["WindDirection"].SetValue(new Vector3(0, 0, 1));


            Effect.Effect.CurrentTechnique = Effect.Effect.Techniques[2];
            Effect.Parameters["LakeTextureTransformation"].SetValue(new Vector4(-dx, -dy, 2, 2));
            if (distance < 9000)
                _hiPolyPlane.Draw(Effect);
            else if (distance < 30000)
            {
                world = Matrix.CreateScale(scale, 1, scale)*Matrix.CreateTranslation(pos)*
                        Matrix.CreateTranslation(0, -0.10f, 0);
                _mhWorld.SetValue(world);
                _mhWorldInv.SetValue(Matrix.Invert(world));
                _mediumPolyPlane.Draw(Effect);
            }
            else
            {
                //Effect.Effect.CurrentTechnique = Effect.Effect.Techniques[1];
                world = Matrix.CreateScale(scale, 1, scale)*Matrix.CreateTranslation(pos)*
                        Matrix.CreateTranslation(0, -0.40f, 0);
                _mhWorld.SetValue(world);
                _mhWorldInv.SetValue(Matrix.Invert(world));
                _lakePlane.Draw(Effect);
            }

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
            _mhCameraPosition = p["CameraPosition"];
            _mhWaveNMapOffset0 = p["gWaveNMapOffset0"];
            _mhWaveNMapOffset1 = p["gWaveNMapOffset1"];
            _mhWaveDMapOffset0 = p["gWaveDMapOffset0"];
            _mhWaveDMapOffset1 = p["gWaveDMapOffset1"];

            p["WaterBumpMap"].SetValue(initInfo.LakeBumpMap);
            p["Checker"].SetValue(initInfo.Checker);

            p["gWaveMap0"].SetValue(initInfo.waveMap0);
            p["gWaveMap1"].SetValue(initInfo.waveMap1);
            p["gWaveDispMap0"].SetValue(initInfo.dmap0);
            p["gWaveDispMap1"].SetValue(initInfo.dmap1);
            p["gLightAmbient"].SetValue(initInfo.DirLight.Ambient);
            p["gLightDiffuse"].SetValue(initInfo.DirLight.Diffuse);
            p["LightDirection"].SetValue(initInfo.DirLight.DirW);
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
            const float waterMeshPositionY = 0.75f; //experimenting with this

            // Reflect the camera's properties across the water plane
            var reflectedCameraPosition = camera.Position -new Vector3(0, 1, 0);
            reflectedCameraPosition.Y = -reflectedCameraPosition.Y + waterMeshPositionY*2;
            var reflectedCameraTarget = camera.Target - new Vector3(0, 1, 0);
            reflectedCameraTarget.Y = -reflectedCameraTarget.Y + waterMeshPositionY*2;

            //reflectedCameraPosition = reflectedCameraPosition + (reflectedCameraPosition - reflectedCameraTarget)*1f;
 
            _reflectionCamera.Update(
                reflectedCameraPosition,
                reflectedCameraTarget);
            //_reflectionCamera.test(Matrix.CreateTranslation(0, -50, 0)*_reflectionCamera.View);

            Effect.GraphicsDevice.SetRenderTarget(_reflectionTarget);

            var clipPlane = new Vector4(0, 1, 0, -waterMeshPositionY);
            foreach (var cd in ReflectedObjects)
                cd.Draw(_reflectionCamera, clipPlane);

            Effect.GraphicsDevice.SetRenderTarget(null);

            Effect.Parameters["ReflectedView"].SetValue(_reflectionCamera.View);
            Effect.Parameters["ReflectedMap"].SetValue(_reflectionTarget);
        }

    }

}