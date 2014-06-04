using System;
using System.Linq;
using System.Runtime.InteropServices;
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
            [FieldOffset(0)] public fixed float X [8];
        }

        public Mt8Surface(int width, int height)
            : base(width, height)
        {
        }

        public Mt8Surface(int width, int height, Mt8[] surface)
            : base(width, height, surface)
        {
        }

        public Texture2D CreateTexture2D(GraphicsDevice graphicsDevice, bool first)
        {
            return Texture2D.New(graphicsDevice, Width, Height, PixelFormat.B8G8R8A8.UNorm,
                first
                    ? Values.Select(mt => new Color(mt.A, mt.B, mt.C, mt.D)).ToArray()
                    : Values.Select(mt => new Color(mt.E, mt.F, mt.G, mt.H)).ToArray());
        }

    }

}
