﻿using System;
using factor10.VisionThing;
using Serpent;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Larv.Serpent
{
    public class EnemySerpent : BaseSerpent
    {
        public static readonly Vector4 ColorWhenLonger = new Vector4(1.5f, 0.6f, 0.6f, 1);
        public static readonly Vector4 ColorWhenShorter = new Vector4(0.6f, 1.5f, 0.6f, 1);

        private readonly Random _rnd = new Random();
        private float _delayBeforeStart;

        public EnemySerpent(
            VisionContent vContent,
            PlayingField pf,
            Whereabouts whereabouts,
            IVDrawable sphere,
            float delayBeforeStart,
            int length)
            : base(
                vContent, pf, sphere, whereabouts, vContent.Load<Texture2D>(@"Textures\sn"), vContent.Load<Texture2D>(@"Textures\snakeskinmap"),
                vContent.Load<Texture2D>(@"Textures\eggshell"))
        {
            _delayBeforeStart = delayBeforeStart;
            for (var i = 0; i < length; i++)
                AddTail();
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
            if (_delayBeforeStart <= 0)
                base.Update(camera, gameTime);
            else
                _delayBeforeStart -= (float) gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void takeDirection()
        {
            if (SerpentStatus != SerpentStatus.Alive)
            {
                TryMove(_whereabouts.Direction);
                return;
            }

            if (TakeDirection())
                return;

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
                return new Vector4(1.1f, 1.1f, 0.4f, AlphaValue());
            return IsLonger ? ColorWhenLonger : ColorWhenShorter;
        }

    }

}
