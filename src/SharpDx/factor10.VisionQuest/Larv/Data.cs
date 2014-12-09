using System;
using factor10.VisionThing;
using factor10.VisionThing.Primitives;
using Larv.GameStates;
using Larv.Serpent;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace Larv
{
    public class Data : IDisposable
    {
        public static LContent LContent;

        public static Data Instance;

        public static SkySphere Sky;
        public static Ground Ground;

        public readonly IVDrawable Sphere;
        public readonly IVDrawable Cylinder;

        public Serpents Serpents;

        public Matrix WorldPicked;
        public Vector3 PickedNormal;
        public Vector3 PickedQueriedNormal;

        public Camera Camera;
        public static ShadowMap ShadowMap;

        public Data(
            Game game1,
            KeyboardManager keyboardManager,
            MouseManager mouseManager,
            PointerManager pointerManager)
        {
            if (Instance != null)
                Instance.Dispose();
            Instance = this;

            LContent = new LContent(game1.GraphicsDevice, game1.Content);

            Sphere = new SpherePrimitive<VertexPositionNormalTangentTexture>(LContent.GraphicsDevice,
                (p, n, t, tx) => new VertexPositionNormalTangentTexture(p, n, t, tx), 2);
            Cylinder = new CylinderPrimitive<VertexPositionNormalTangentTexture>(LContent.GraphicsDevice,
                (p, n, t, tx) => new VertexPositionNormalTangentTexture(p, n, t, tx), 2, 1, 10);

            //TODO
            if (Sky == null)
                Sky = new SkySphere(LContent, LContent.Load<TextureCube>(@"Textures\clouds"));

            Camera = new Camera(
                LContent.ClientSize,
                keyboardManager,
                mouseManager,
                pointerManager,
                AttractState.CameraPosition,
                AttractState.CameraLookAt) { MovingSpeed = 8 };
            Ground = new Ground(LContent);
            Serpents = new Serpents(LContent, Camera, Sphere, 0);

            Ground.GeneratePlayingField(Serpents.PlayingField);

            ShadowMap = new ShadowMap(LContent, Camera, 768, 768, 1, 50);
            ShadowMap.UpdateProjection(50, 30);
            ShadowMap.ShadowCastingObjects.Add(Serpents);
        }

        public Vector3 Attra { get; set; }


        public bool HasKeyToggled(Keys key)
        {
            return Camera.KeyboardState.IsKeyPressed(key);
        }

        public void Dispose()
        {
            Serpents.Dispose();
        }

        public void Update(GameTime gameTime)
        {
            Camera.UpdateInputDevices();

            Ground.Update(Camera, gameTime);

            //if (Data.HasKeyToggled(Keys.Enter) && Data.KeyboardState.IsKeyDown(Keys.LeftAlt))
            //{
            //    _graphics.IsFullScreen ^= true;
            //    _graphics.ApplyChanges();
            //}

            //GroundMap.Move(camera.Camera.KeyboardState);

            //if (HasKeyToggled(Keys.C))
            //{
            //    var cameraStates = new[] {CameraBehavior.FollowTarget, CameraBehavior.FollowTarget, CameraBehavior.Static, CameraBehavior.Head}.ToList();
            //    var idx = cameraStates.IndexOf(camera.CameraBehavior);
            //    camera.CameraBehavior = cameraStates[(idx + 1)%cameraStates.Count];
            //}
            //if (HasKeyToggled(Keys.P))
            //    _paused ^= true;

            if (HasKeyToggled(Keys.D1))
                Serpents.PlayerSerpent.Speed *= 2;
            if (HasKeyToggled(Keys.D2))
                Serpents.PlayerSerpent.Speed /= 2;

            if (HasKeyToggled(Keys.B))
            {
                Vector3 hit, normal;
                var ray = Camera.GetPickingRay();
                if(Ground.HitTest(ray, out hit, out normal))
                {
                    WorldPicked = Matrix.Translation(hit);

                    var winv = Ground.World;
                    winv.Invert();
                    var gspace = Vector3.TransformCoordinate(hit, winv);
                    PickedNormal = Ground.GroundMap.GetNormal((int)gspace.X, (int)gspace.Z, ref Ground.World);
                    //var worldInv = Ground.World;
                    //worldInv.Invert();
                    //var local = Ground.GroundMap.HitTest(worldInv, ray).Value;
                    //local.Y = Ground.GroundMap.GetExactHeight(local.X, local.Z);
                    //PickedQueriedGroundHeight1 = Vector3.TransformCoordinate(local, Ground.World);
                    //local.Y = Ground.GroundMap.GetExactHeight2(local.X, local.Z);
                    //PickedQueriedGroundHeight2 = Vector3.TransformCoordinate(local, Ground.World);
                }
            }

        }

    }

}