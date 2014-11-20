using System;
using System.Linq;
using factor10.VisionThing;
using factor10.VisionThing.Primitives;
using Larv;
using Larv.Serpent;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace Serpent
{
    public class Data : IDisposable
    {
        public static VisionContent VContent;

        public static Data Instance;

        public static PlayingField PlayingField;

        public static SkySphere Sky;
        public static Gq Ground;

        public readonly IVDrawable Sphere;

        public Serpents Serpents;

        private bool _paused;

        public Matrix WorldPicked;

        public Camera Camera;
        public ShadowMap ShadowMap;

        public Data(
            Game game1,
            KeyboardManager keyboardManager,
            MouseManager mouseManager,
            PointerManager pointerManager)
        {
            if (Instance != null)
                Instance.Dispose();
            Instance = this;

            VContent = new VisionContent(game1.GraphicsDevice, game1.Content);
            var texture = game1.Content.Load<Texture2D>(@"Textures\woodfloor");

            if (PlayingField == null)
                PlayingField = new PlayingField(
                    VContent.GraphicsDevice,
                    texture);

            Sphere = new SpherePrimitive<VertexPositionNormalTangentTexture>(VContent.GraphicsDevice, (p, n, t, tx) => new VertexPositionNormalTangentTexture(p, n, t, tx * 2), 2);

            //TODO
            if (Sky == null)
                Sky = new SkySphere(VContent, VContent.Load<TextureCube>(@"Textures\clouds"));

            Ground = new Gq(VContent, PlayingField);

            Camera = new Camera(VContent.ClientSize, keyboardManager, mouseManager, pointerManager, new Vector3(10, 5, 10), Vector3.Zero);
            ShadowMap = new ShadowMap(VContent, Camera, 1920, 1080);
            Serpents = new Serpents(VContent, Camera, Sphere, PlayingField, ShadowMap);
        }


        public bool HasKeyToggled( Keys key )
        {
            return Serpents.SerpentCamera.Camera.KeyboardState.IsKeyPressed(key);
        }

        public void Dispose()
        {
            PlayingField.Dispose();
        }

        public void Update(GameTime gameTime)
        {
            var camera = Serpents.SerpentCamera;
            camera.Camera.UpdateInputDevices();

            Ground.Update(camera.Camera, gameTime);

            //if (Data.HasKeyToggled(Keys.Enter) && Data.KeyboardState.IsKeyDown(Keys.LeftAlt))
            //{
            //    _graphics.IsFullScreen ^= true;
            //    _graphics.ApplyChanges();
            //}

            //Ground.Move(camera.Camera.KeyboardState);

            if (HasKeyToggled(Keys.C))
            {
                var cameraStates = new[] {CameraBehavior.FollowTarget, CameraBehavior.FollowTarget, CameraBehavior.Static, CameraBehavior.Head}.ToList();
                var idx = cameraStates.IndexOf(camera.CameraBehavior);
                camera.CameraBehavior = cameraStates[(idx + 1) % cameraStates.Count];
            }
            if (HasKeyToggled(Keys.P))
                _paused ^= true;

            if (HasKeyToggled(Keys.D1))
                Serpents.PlayerSerpent.Speed *= 2;
            if (HasKeyToggled(Keys.D2))
                Serpents.PlayerSerpent.Speed /= 2;

            if (HasKeyToggled(Keys.B))
            {
                var ray = Serpents.SerpentCamera.Camera.GetPickingRay();
                var hit = Ground.HitTest(ray);
                if (hit != null)
                    WorldPicked = Matrix.Translation(hit.Value);
            }

            if (_paused)
            {
                Serpents.PlayerSerpent.UpdateCameraOnly(Serpents.SerpentCamera, gameTime);
                return;
            }


            //Serpents.Update(gameTime);
            //if (Data.Enemies.Count == 0)
            //    startGame();
    
        }

    }

}
