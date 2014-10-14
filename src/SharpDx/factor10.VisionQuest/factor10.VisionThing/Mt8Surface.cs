using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using factor10.VisionThing.Terrain;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionThing
{

    public unsafe class Mt8Surface : Sculptable<Mt8Surface.Mt8>
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct Mt8
        {
            [FieldOffset(0)] public float A;
            [FieldOffset(4)] public float B;
            [FieldOffset(8)] public float C;
            [FieldOffset(12)] public float D;
            [FieldOffset(16)] public float E;
            [FieldOffset(20)] public float F;
            [FieldOffset(24)] public float G;
            [FieldOffset(28)] public float H;
            [FieldOffset(32)] public float I;
            [FieldOffset(0)] public fixed float X [9];

            public Color ToArgb()
            {
                var x = new[]
                {
                    A,
                    B > F ? B : 0,
                    C > G ? C : 0,
                    D > H ? D : 0,
                    E > I ? E : 0,
                    B > F ? 0 : F,
                    C > G ? 0 : G,
                    D > H ? 0 : H,
                    E > I ? 0 : I
                };
                //x[8] = 100;

                var f = 0.5f/(x.Sum() + 0.000001f);
                var q = combine(x[4], x[8], f);
                var c = new Color(
                    combine(x[1], x[5], f),
                    combine(x[2], x[6], f),
                    combine(x[3], x[7], f),
                    combine(x[4], x[8], f));
                return c;
            }

            private static float combine(float x, float y, float f)
            {
                //note that x*f or y*f will always be in [0,0.5]
                return x > y
                    ? x*f + 0.5f
                    : 0.5f - y*f;
            }
        }

        public Mt8Surface(int width, int height)
            : base(width, height)
        {
        }

        public Mt8Surface(int width, int height, Mt8[] surface)
            : base(width, height, surface)
        {
        }

        public Texture2D CreateTexture2D(GraphicsDevice graphicsDevice)
        {
            return Texture2D.New(graphicsDevice, Width, Height, PixelFormat.B8G8R8A8.UNorm,
                Values.Select(_ => _.ToArgb()).ToArray());
        }

    }

}
