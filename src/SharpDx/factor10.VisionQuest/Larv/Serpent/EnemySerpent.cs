using System;
using factor10.VisionThing;
using Serpent;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace Larv.Serpent
{
    public class EnemySerpent : BaseSerpent
    {
        private readonly Random _rnd = new Random();
 
        public EnemySerpent(
            VisionContent vContent,
            PlayingField pf,
            Whereabouts whereabouts,
            IVDrawable sphere,
            int x)
            : base(vContent, pf, sphere, whereabouts, vContent.Load<Texture2D>(@"Textures\sn"), vContent.Load<Texture2D>(@"Textures\snakeskinmap"), vContent.Load<Texture2D>(@"Textures\eggshell"))
        {
             _rnd.NextBytes(new byte[x]);
 
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
                return new Vector4(1.1f, 1.1f, 0.4f, AlphaValue());
            return _isLonger
                ? new Vector4(1.5f, 0.5f, 0.5f, 1)
                : new Vector4(0.5f, 1.5f, 0.5f, 1);
        }

    }

}
