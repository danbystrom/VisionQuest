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
                                  ScaledTexC = new Vector2(x/initInfo.Columns, y/initInfo.Rows) * initInfo.texScale,
			                      NormalizedTexC = new Vector2(x/initInfo.Columns, y/initInfo.Rows)
                              },
                initInfo.Columns,
                initInfo.Rows);

 
            buildFx();
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

        public void Draw( Camera camera, Matrix world, bool fast )
        {
            _mhWvp.SetValue(world * camera.View * camera.Projection);

            _mhEyePosW.SetValue( camera.Position );
            _mhWaveNMapOffset0.SetValue( _waveNMapOffset0 );
            _mhWaveNMapOffset1.SetValue( _waveNMapOffset1);
            _mhWaveDMapOffset0.SetValue( _waveDMapOffset0);
            _mhWaveDMapOffset1.SetValue( _waveDMapOffset1);

            _mhWorld.SetValue(world);
            _mhWorldInv.SetValue(Matrix.Invert(world));

            _initInfo.Fx.CurrentTechnique = fast ? _fastEffect : _qualityEffect;
            _field.Draw(_initInfo.Fx);
        }

        private EffectParameter _mhWorld;
        private EffectParameter _mhWorldInv;
        private EffectParameter _mhWvp;
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

            _mhWorld = fx.Parameters["gWorld"];
            _mhWorldInv = fx.Parameters["gWorldInv"];
            _mhWvp = fx.Parameters["gWVP"];
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

    }

}