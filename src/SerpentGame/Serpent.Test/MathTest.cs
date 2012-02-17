using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using Serpent.Util;

namespace Serpent.Test
{
    [TestFixture]
    class MathTest
    {


        [Test]
        public void Z()
        {
            var arc = new ArcGenerator(3);
            arc.CreateArc(
                new Vector3(-1, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(0, 1, 0),
                1);
            var points = new List<Vector3>(arc.Points);
            Assert.IsTrue(points.TrueForAll(v => Math.Abs(v.Length() - 1) < 0.00001f));
        }

        [Test]
        public void ZZ()
        {
            var v1 = new Vector3(1, 0, 0);
            var v2 = new Vector3(0, 0, -1);
            var v3 = Vector3.Cross( v1, v2 );

        }
    }
}
