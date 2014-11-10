using System;
using System.Collections.Generic;
using factor10.VisionThing;
using factor10.VisionThing.Primitives;
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
        public readonly PlayerSerpent PlayerSerpent;
        public readonly List<EnemySerpent> Enemies = new List<EnemySerpent>();

        public readonly List<Egg> Eggs = new List<Egg>();

        public readonly KeyboardManager KeyboardManager;
        public readonly MouseManager MouseManager;

        public KeyboardState KeyboardState;
        public KeyboardState PrevKeyboardState;

        public static SkySphere Sky;

        public readonly IVDrawable Sphere;

        public Data(
            NextGame.NextGame game1,
            KeyboardManager keyboardManager,
            MouseManager mouseManager)
        {
            if (Instance != null)
                Instance.Dispose();
            Instance = this;

            KeyboardManager = keyboardManager;
            MouseManager = mouseManager;

            VContent = new VisionContent(game1.GraphicsDevice, game1.Content);
            var texture = game1.Content.Load<Texture2D>(@"Textures\woodfloor");

            if (PlayingField == null)
                PlayingField = new PlayingField(
                    game1.GraphicsDevice,
                    texture);

            //TODO
            if (Sky == null)
                Sky = new SkySphere(VContent, VContent.Load<TextureCube>(@"Textures\clouds"));

            Sphere = new SpherePrimitive<VertexPositionNormalTexture>(game1.GraphicsDevice, (p, n, t) => new VertexPositionNormalTexture(p, n, t*2), 2);

            PlayerSerpent = new PlayerSerpent(
                VContent,
                MouseManager,
                KeyboardManager,
                PlayingField,
                Sphere);

            for (var i = 0; i < 5; i++)
            {
                var enemy = new EnemySerpent(
                    VContent,
                    PlayingField,
                    PlayingField.EnemyWhereaboutsStart,
                    Sphere,
                    PlayerSerpent.Camera,
                    i);
                Enemies.Add(enemy);
            }
        }

        public void Update(GameTime gameTime)
        {
            PlayerSerpent.Update(gameTime);

            foreach (var enemy in Enemies)
            {
                enemy.Update(gameTime);
                if (enemy.EatAt(PlayerSerpent))
                {
                    PlayerSerpent.Restart();
                }
                else if (enemy.SerpentStatus == SerpentStatus.Alive && PlayerSerpent.EatAt(enemy))
                    enemy.SerpentStatus = SerpentStatus.Ghost;
            }
            Enemies.RemoveAll(e => e.SerpentStatus == SerpentStatus.Finished);

            for (var i = Eggs.Count - 1; i >= 0; )
            {
                Eggs[i].Update(gameTime);
                if (Eggs[i].TimeToHatch())
                {
                    Enemies.Add(new EnemySerpent(
                        VContent,
                        PlayingField,
                        Eggs[i].Whereabouts,
                        Sphere,
                        PlayerSerpent.Camera,
                        0));
                    Eggs.RemoveAt(i);
                }
                else
                    i--;
            }
            
        }

        public void UpdateKeyboard()
        {
            PrevKeyboardState = KeyboardState;
            KeyboardState = KeyboardManager.GetState();
        }

        public bool HasKeyToggled( Keys key )
        {
            return KeyboardState.IsKeyPressed(key);
        }

        public void Dispose()
        {
            PlayingField.Dispose();
        }

    }

}
