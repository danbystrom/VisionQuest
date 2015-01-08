﻿using System;
using System.Collections.Generic;
using System.Linq;
using factor10.VisionThing;
using Larv.Field;
using Larv.Util;
using SharpDX;

namespace Larv.Serpent
{
    public class SerpentTailSegment : IPosition
    {
        public readonly List<Whereabouts> PathToWalk;
 
        public SerpentTailSegment Next;

        private readonly PlayingField _pf;

        public SerpentTailSegment(PlayingField pf, Whereabouts w)
        {
            _pf = pf;
            PathToWalk = new List<Whereabouts> { w };
        }

        public Whereabouts Whereabouts
        {
            get { return PathToWalk[0]; }
        }

        public void Update(float speed, Whereabouts previous)
        {
            var last = PathToWalk.Last();
            if (Math.Abs(last.Location.X - previous.Location.X) >= 2 || Math.Abs(last.Location.Y - previous.Location.Y) >= 2)
            {
                //bugfix...
                var p = new Point((previous.Location.X + last.Location.X)/2, (previous.Location.Y + last.Location.Y)/2);
                PathToWalk[PathToWalk.Count - 1] = new Whereabouts(last.Floor, p, last.Direction) {Fraction = previous.Fraction};
            }

            if (PathToWalk.Count >= 2)
            {
                var w = PathToWalk[0];
                w.Fraction += speed;
                if (w.Fraction >= 1)
                {
                    PathToWalk.RemoveAt(0);
                    var f = w.Fraction;
                    w = PathToWalk[0];
                    w.Fraction = f - 1;
                }
                PathToWalk[0] = w;
            }

            if (Next != null)
                Next.Update(speed, PathToWalk.First());
        }

        public Vector3 Position
        {
            get { return PathToWalk[0].GetPosition(_pf); }
        }

        public void AddPathToWalk(Whereabouts w)
        {
            if (PathToWalk.Exists(e => e.Location == w.Location))
                return;

            w.Fraction = 0;
            PathToWalk.Add(w);
            if (Next != null)
                Next.AddPathToWalk(PathToWalk[0]);
        }

        public override string ToString()
        {
            return string.Format("({0})", string.Join("),(", PathToWalk));
        }

    }

}