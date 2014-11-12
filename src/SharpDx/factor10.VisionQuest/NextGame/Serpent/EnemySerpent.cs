﻿using factor10.VisionThing;
using Serpent.Serpent;
using SharpDX;
using SharpDX.Toolkit.Graphics;
using System;

namespace Serpent
{
    public class EnemySerpent : BaseSerpent
    {
        private readonly Random _rnd = new Random();
 
        public EnemySerpent(
            VisionContent vContent,
            PlayingField pf,
            Whereabouts whereabouts,
            IVDrawable sphere,
            SerpentCamera camera,
            int x)
            : base(vContent, pf, sphere, whereabouts, vContent.Load<Texture2D>(@"Textures\sn"), vContent.Load<Texture2D>(@"Textures\eggshell"))
        {
             _rnd.NextBytes(new byte[x]);
            _camera = camera;

            AddTail();
            AddTail();
        }

        protected override void takeDirection()
        {
            if (SerpentStatus != SerpentStatus.Alive)
            {
                TryMove(_whereabouts.Direction);
                return;
            }

            if (_rnd.NextDouble() < 0.33 && TryMove(_whereabouts.Direction.Left))
                return;
            if (_rnd.NextDouble() < 0.66 && TryMove(_whereabouts.Direction.Right))
                return;
            if (TryMove(_whereabouts.Direction))
                return;

            if (_rnd.NextDouble() < 0.5 && TryMove(_whereabouts.Direction.Left))
                return;
            if (TryMove(_whereabouts.Direction.Right))
                return;
            if (TryMove(_whereabouts.Direction.Left))
                return;
            TryMove(_whereabouts.Direction.Backward);
        }

        protected override Vector4 TintColor()
        {
            if (SerpentStatus != SerpentStatus.Alive)
                return new Vector4(1.2f, 1.2f, 0.5f, AlphaValue());
            return _isLonger
                ? new Vector4(1.5f, 0.5f, 0.5f, 1)
                : new Vector4(0.5f, 1.5f, 0.5f, 1);
        }

    }

}
