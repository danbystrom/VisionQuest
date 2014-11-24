using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serpent;
using SharpDX;

namespace Larv.Serpent
{
    public class SerpentTailSegment
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

        private readonly List<List<string>> _log = new List<List<string>>(); 
        public void Update(float speed, Whereabouts previous)
        {
            if (update(speed, previous))
            {
                for (var seg = this; seg.Next != null; seg = seg.Next)
                {
                    var a = seg.Whereabouts.Location.X - seg.Next.Whereabouts.Location.X;
                    var b = seg.Whereabouts.Location.Y - seg.Next.Whereabouts.Location.Y;
                    if (a*a + b*b != 1)
                    {
                    }
                }
                var y = string.Join("\r\n", _log.Select(a => string.Join("\r\n", a)));
                File.WriteAllText(@"c:\temp\x.log", y);
                //throw new Exception();
            }

            var x = new List<string>();
            x.Add(string.Format("Speed:{0:0.00000}", speed));
            for (var seg = this; seg != null; seg = seg.Next)
                x.Add(seg.ToString());    
            _log.Add(x);
            if (_log.Count > 500)
                _log.RemoveAt(0);

        }

        private bool update(float speed, Whereabouts previous)
        {
            var last = PathToWalk.Last();
            if (previous.LocationDistanceSquared(last) >= 2)
            {
                var p = new Point((previous.Location.X + last.Location.X)/2, (previous.Location.Y + last.Location.Y)/2);
                PathToWalk[PathToWalk.Count - 1] = new Whereabouts(last.Floor, p, last.Direction);
            }

            if (PathToWalk.Count >= 2)
            {
                var w = PathToWalk[0];
                w.Fraction += speed;
                if (w.Fraction > 0.9999)
                {
                    PathToWalk.RemoveAt(0);
                    var f = w.Fraction;
                    w = PathToWalk[0];
                    w.Fraction = f - 1;
                }
                PathToWalk[0] = w;
            }

            if (Next != null)
            {
                return Next.update(speed, PathToWalk.First()) |
                       Vector3.DistanceSquared(GetPosition(), Next.GetPosition()) > 1.9;
            }
            return false;
        }

        public Vector3 GetPosition()
        {
            return PathToWalk[0].GetPosition(_pf);
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
