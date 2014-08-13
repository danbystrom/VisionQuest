using System;
using factor10.VisionThing.Primitives;
using Serpent.Serpent;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Serpent
{
    public class EnemySerpent : BaseSerpent
    {
        private readonly Random _rnd = new Random();
 
        public EnemySerpent(
            Game game,
            PlayingField pf,
            factor10.VisionThing.IDrawable sphere,
            SerpentCamera camera,
            Whereabouts whereabouts,
            int x)
            : base(game, pf, sphere, whereabouts, game.Content.Load<Texture2D>(@"Textures\sn"))
        {
            _whereabouts = whereabouts;
             _rnd.NextBytes(new byte[x]);
            _camera = camera;

            addTail();
            addTail();
        }

        protected override void takeDirection()
        {
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
            return _isLonger
                ? new Vector4(0.8f, 0.2f, 0.2f, 1)
                : new Vector4(0.2f, 0.8f, 0.2f, 1);
        }

    }

}
