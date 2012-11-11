using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

        [StructLayout(LayoutKind.Explicit,Pack=1)]
        private struct Q
        {
            [FieldOffset(0)]
            public double Val;
        }

        [Test]
        public void ZZ()
        {
            var dic = new Dictionary<int, Q>();
            for ( var i = 0 ; i < 100*1000*100 ; i++)
                dic.Add( i, new Q {Val = i});
            for (var i = 0; i < 100 * 1000 * 100; i++)
                Assert.AreEqual(i, (int)dic[i].Val);
        }
    }
}
