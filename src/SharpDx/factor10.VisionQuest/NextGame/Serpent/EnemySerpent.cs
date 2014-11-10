using factor10.VisionThing;
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

            addTail();
            addTail();
        }

        protected override void takeDirection()
        {
            if (SerpentStatus != SerpentStatus.Alive)
                return;

            if (_rnd.NextDouble() < 0.33 && tryMove(_whereabouts.Direction.Left))
                return;
            if (_rnd.NextDouble() < 0.66 && tryMove(_whereabouts.Direction.Right))
                return;
            if (tryMove(_whereabouts.Direction))
                return;

            if (_rnd.NextDouble() < 0.5 && tryMove(_whereabouts.Direction.Left))
                return;
            if (tryMove(_whereabouts.Direction.Right))
                return;
            if (tryMove(_whereabouts.Direction.Left))
                return;
            tryMove(_whereabouts.Direction.Backward);
        }

        protected override Vector4 tintColor()
        {
            if (SerpentStatus != SerpentStatus.Alive)
                return new Vector4(1, 1, 1, 0.5f);
            return _isLonger
                ? new Vector4(1f, 0.5f, 0.5f, 1)
                : new Vector4(0.5f, 1f, 0.5f, 1);
        }

    }

}
