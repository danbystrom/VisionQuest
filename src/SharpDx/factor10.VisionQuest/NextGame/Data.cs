using System;
using System.Collections.Generic;
using factor10.VisionThing;
using factor10.VisionThing.Primitives;
using NextGame.Serpent;
using Serpent.Serpent;
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

        public readonly IVDrawable Sphere;

        public Serpents Serpents;

        private bool _paused;

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
                    game1.GraphicsDevice,
                    texture);

            Sphere = new SpherePrimitive<VertexPositionNormalTexture>(VContent.GraphicsDevice, (p, n, t) => new VertexPositionNormalTexture(p, n, t * 2), 2);

            //TODO
            if (Sky == null)
                Sky = new SkySphere(VContent, VContent.Load<TextureCube>(@"Textures\clouds"));

            Serpents = new Serpents(VContent, Sphere, mouseManager, keyboardManager, pointerManager, PlayingField);
        }


        public bool HasKeyToggled( Keys key )
        {
            return Serpents.PlayerSerpent.Camera.Camera.KeyboardState.IsKeyPressed(key);
        }

        public void Dispose()
        {
            PlayingField.Dispose();
        }

        public void Update(GameTime gameTime)
        {
            var camera = Serpents.PlayerSerpent.Camera;
            camera.Camera.UpdateInputDevices();

            //if (Data.HasKeyToggled(Keys.Enter) && Data.KeyboardState.IsKeyDown(Keys.LeftAlt))
            //{
            //    _graphics.IsFullScreen ^= true;
            //    _graphics.ApplyChanges();
            //}

            if (HasKeyToggled(Keys.C))
                camera.CameraBehavior = camera.CameraBehavior == CameraBehavior.FollowTarget
                    ? CameraBehavior.Static
                    : CameraBehavior.FollowTarget;
            if (HasKeyToggled(Keys.F))
                camera.CameraBehavior = camera.CameraBehavior == CameraBehavior.FollowTarget
                    ? CameraBehavior.FreeFlying
                    : CameraBehavior.FollowTarget;
            if (HasKeyToggled(Keys.H))
                camera.CameraBehavior = camera.CameraBehavior == CameraBehavior.FollowTarget
                    ? CameraBehavior.Head
                    : CameraBehavior.FollowTarget;
            if (HasKeyToggled(Keys.P))
                _paused ^= true;

            if (HasKeyToggled(Keys.D1))
                Serpents.PlayerSerpent.Speed *= 2;
            if (HasKeyToggled(Keys.D2))
                Serpents.PlayerSerpent.Speed /= 2;

            if (_paused)
            {
                Serpents.PlayerSerpent.UpdateCameraOnly(gameTime);
                return;
            }

            Serpents.Update(gameTime);
            //if (Data.Enemies.Count == 0)
            //    startGame();
    
        }

    }

}
