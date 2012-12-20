using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using factor10.VisionaryHeads;

namespace TestBed
{
    public class VisualClass
    {
        public readonly VClass VClass;
        public readonly int Instructions;
        public readonly int X;
        public readonly int Y;

        public VisualClass(VClass vclass, int x, int y)
        {
            VClass = vclass;
            X = x;
            Y = y;
            Instructions = vclass.VMethods.Sum(vm => vm.InstructionCount);
        }

    }
}
