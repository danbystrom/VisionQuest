﻿using System;
using System.Collections.Generic;
using factor10.VisionThing;
using factor10.VisionThing.Primitives;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace Serpent
{
    public class Data : IDisposable
    {
        public static Data Instance;

        public static PlayingField PlayingField;
        public readonly PlayerSerpent PlayerSerpent;
        public readonly List<EnemySerpent> Enemies = new List<EnemySerpent>();

        public readonly KeyboardManager KeyboardManager;
        public readonly MouseManager MouseManager;

        public KeyboardState KeyboardState;
        public KeyboardState PrevKeyboardState;

        public static SkySphere Sky;

        public Data(
            NextGame.NextGame game1,
            KeyboardManager keyboardManager,
            MouseManager mouseManager)
        {
            if ( Instance != null )
                Instance.Dispose();
            Instance = this;

            KeyboardManager = keyboardManager;
            MouseManager = mouseManager;

            VisionContent.Init(game1);
            var texture = game1.Content.Load<Texture2D>(@"Textures\woodfloor");
 
            if ( PlayingField == null )
                PlayingField = new PlayingField(
                    game1.GraphicsDevice,
                    texture );

            //TODO
            if ( Sky == null )
                Sky = new SkySphere(game1, VisionContent.Load<TextureCube>(@"Textures\clouds"));

            var sphere = new SpherePrimitive<VertexPositionNormalTexture>(game1.GraphicsDevice, (p, n, t) => new VertexPositionNormalTexture(p, n, t*2), 2);

            PlayerSerpent = new PlayerSerpent(
                game1,
                MouseManager,
                PlayingField,
                sphere);

            for (var i = 0; i < 5; i++)
            {
                var enemy = new EnemySerpent(
                    game1,
                    PlayingField,
                    sphere,
                    PlayerSerpent.Camera,
                    new Whereabouts(0, new Point(20, 0), Direction.West),
                    i);
                Enemies.Add(enemy);
            }
        }

        public void UpdateKeyboard()
        {
            PrevKeyboardState = KeyboardState;
            KeyboardState = KeyboardManager.GetState();
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
