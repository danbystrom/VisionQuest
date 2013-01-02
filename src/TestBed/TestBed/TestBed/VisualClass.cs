using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using factor10.VisionaryHeads;

namespace TestBed
{
    public class VisualClass
    {
        public readonly VClass VClass;
        public readonly int InstructionCount;
        public readonly int X;
        public readonly int Y;
        public float Height;

        public VisualClass(VClass vclass, int x, int y)
        {
            VClass = vclass;
            X = x;
            Y = y;
            InstructionCount = vclass.VMethods.Sum(vm => vm.InstructionCount);
        }

        public Vector3 Position
        {
            get { return new Vector3(X, Height, Y); }
        }

    }

}
