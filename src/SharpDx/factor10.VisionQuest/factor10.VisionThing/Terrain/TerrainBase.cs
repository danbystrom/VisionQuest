using System;
using System.Diagnostics;
using System.Linq;
using factor10.VisionThing.Objects;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionThing.Terrain
{
    public class TerrainBase : ClipDrawable
    {
        public const int Side = TerrainPlane.SquareSize;
        public const int HalfSide = Side/2;

        public static TerrainPlane TerrainPlane { get; private set; }
        public static Box Box { get; protected set; }

        public readonly VisionContent VContent;

        public Matrix World;
        private Vector3 _position;

        protected readonly Texture2D[] Textures = new Texture2D[9];

        protected Texture2D HeightsMap;
        protected Texture2D WeightsMap;
        protected Texture2D NormalsMap;

        private terrainSlice[] _slices;

        public int GroundExtentX { get; private set; }
        public int GroundExtentZ { get; private set; }

        public TerrainBase(VisionContent vContent)
            : base(createTerrainPlaneSingleton(vContent).Effect)
        {
            VContent = vContent;
            Effect.Parameters["Ambient"].SetValue(0.6f);
        }

        private static TerrainPlane createTerrainPlaneSingleton(VisionContent vContent)
        {
            if (TerrainPlane == null)
            {
                TerrainPlane = new TerrainPlane(vContent);
                Box = new Box(vContent, Matrix.Identity, new Vector3(1, 50, 1));
            }
            return TerrainPlane;
        }

        protected void initialize(Ground ground)
        {
            initialize(ground, ground.CreateWeigthsMap(), ground.CreateNormalsMap());
        }

        protected void initialize(Ground ground, WeightsMap weights, ColorSurface normals)

        {
            Debug.Assert((ground.Width%64) == 0 && (ground.Height%64) == 0);

            _position = World.TranslationVector;

            Textures[0] = Textures[0] ?? VContent.Load<Texture2D>("terraintextures/dirtground");
            Textures[1] = Textures[1] ?? VContent.Load<Texture2D>("terraintextures/sahara");
            Textures[2] = Textures[2] ?? VContent.Load<Texture2D>("terraintextures/grass");
            Textures[3] = Textures[3] ?? VContent.Load<Texture2D>("terraintextures/rock");
            Textures[4] = Textures[4] ?? VContent.Load<Texture2D>("terraintextures/snow");
            Textures[5] = Textures[5] ?? VContent.Load<Texture2D>("terraintextures/stones");
            Textures[6] = Textures[6] ?? VContent.Load<Texture2D>("terraintextures/sand");
            Textures[7] = Textures[7] ?? VContent.Load<Texture2D>("terraintextures/path");
            Textures[8] = Textures[8] ?? VContent.Load<Texture2D>("terraintextures/wheatfield");

            HeightsMap = ground.CreateHeightsTexture(Effect.GraphicsDevice);
            WeightsMap = weights.CreateTexture2D(Effect.GraphicsDevice);
            NormalsMap = normals.CreateTexture2D(Effect.GraphicsDevice);

            var slicesW = ground.Width/Side;
            var slicesH = ground.Height/Side;
            //TODO - this is wrong - I guess...
            var sliceFracX = 1f/slicesW;
            var sliceFracY = 1f/slicesH;
            _slices = new terrainSlice[slicesW*slicesH];
            var raduis = HalfSide*(float) Math.Sqrt(2);
            var i = 0;
            for (var y = 0; y < slicesW; y++)
                for (var x = 0; x < slicesH; x++)
                {
                    var world = World*Matrix.Translation(Side*x, 0, Side*y);
                    _slices[i++] = new terrainSlice
                    {
                        TexOffsetAndScale = new Vector4(x*sliceFracX, y*sliceFracY, sliceFracX, sliceFracY),
                        World = world,
                        BoundingSphere = new BoundingSphere(world.TranslationVector + new Vector3(HalfSide, 0, HalfSide), raduis)
                    };
                }
            BoundingSphere = new BoundingSphere(
                _position + new Vector3(ground.Width, 0, ground.Height)/2,
                (float) Math.Sqrt(ground.Width*ground.Width + ground.Height*ground.Height)/2);

            GroundExtentX = slicesW;
            GroundExtentZ = slicesH;
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            //if (camera.BoundingFrustum.Intersects(ref _boundingSphere))
            //    return false;
            var anyPartIsVisible = _slices.Aggregate(false,
                (current, slice) => current | (slice.Visible = camera.BoundingFrustum.Contains(slice.BoundingSphere) != ContainmentType.Disjoint));

            if (!anyPartIsVisible)
                return false;

            for (var i = 0; i < 9; i++)
                Effect.Parameters["Texture" + (char) (48 + i)].SetResource(Textures[i]);

            Effect.Parameters["HeightsMap"].SetResource(HeightsMap);
            Effect.Parameters["NormalsMap"].SetResource(NormalsMap);
            Effect.Parameters["WeightsMap"].SetResource(WeightsMap);

            camera.UpdateEffect(Effect);

            foreach (var slice in _slices.Where(slice => slice.Visible))
            {
                Effect.Parameters["TexOffsetAndScale"].SetValue(slice.TexOffsetAndScale);
                TerrainPlane.Draw(camera, slice.World, drawingReason);
                //Box.World = slice.World * Matrix.Translation(0,10,0);
                //Box.Draw(camera, drawingReason, shadowMap);
            }

            return true;
        }

        private class terrainSlice
        {
            public Vector4 TexOffsetAndScale;
            public Matrix World;
            public BoundingSphere BoundingSphere;
            public bool Visible;
        }

        public Point? HitTest(Ray ray)
        {
            return new Ground(HeightsMap, _ => _).HitTest(World.TranslationVector, ray);
        }

    }

}
