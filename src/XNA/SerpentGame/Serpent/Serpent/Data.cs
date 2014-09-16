using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using factor10.VisionThing;

namespace Serpent
{
    public class Data : IDisposable
    {
        public static Data Instance;

        public static PlayingField PlayingField;
        public readonly PlayerSerpent PlayerSerpent;
        public readonly List<EnemySerpent> Enemies = new List<EnemySerpent>();

        public KeyboardState KeyboardState;
        public KeyboardState PrevKeyboardState;

        public static SkySphere Sky;

        public Data( Game game1 )
        {
            if ( Instance != null )
                Instance.Dispose();
            Instance = this;

            VisionContent.Init(game1);
            var texture = game1.Content.Load<Texture2D>("Textures/woodfloor");
 
            if ( PlayingField == null )
                PlayingField = new PlayingField(
                    game1.GraphicsDevice,
                    texture );

            if ( Sky == null )
                Sky = new SkySphere(VisionContent.Load<TextureCube>(@"Textures\clouds"));

            var serpentHead = new ModelWrapper( game1, game1.Content.Load<Model>(@"Models\SerpentHead") );
            var serpentSegment = new ModelWrapper( game1, game1.Content.Load<Model>(@"Models\serpentsegment") );

            PlayerSerpent = new PlayerSerpent(
                game1,
                PlayingField,
                serpentHead,
                serpentSegment);

            for (var i = 0; i < 5; i++)
            {
                var enemy = new EnemySerpent(
                    game1,
                    PlayingField,
                    serpentHead,
                    serpentSegment,
                    PlayerSerpent.Camera,
                    new Whereabouts(0, new Point(20, 0), Direction.West),
                    i);
                Enemies.Add(enemy);
            }
        }

        public void UpdateKeyboard()
        {
            PrevKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();
        }

        public bool HasKeyToggled( Keys key )
        {
            return KeyboardState.IsKeyDown(key) && PrevKeyboardState.IsKeyUp(key);
        }

        public void Dispose()
        {
            PlayingField.Dispose();
        }

    }

}
