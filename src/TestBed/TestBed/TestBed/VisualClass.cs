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

        public int InstructionCount { get; set; }
        public int MaintainabilityIndex { get; set; }
        public int CyclomaticComplexity { get; set; }
        public int ClassCoupling { get; set; }
        public int LinesOfCode { get; set; }
        
        public readonly int X;
        public readonly int Y;
        public float Height;

        public VisualClass(VClass vclass, int x, int y)
        {
            VClass = vclass;
            X = x;
            Y = y;
            InstructionCount = vclass.VMethods.Sum(vm => vm.InstructionCount);
            MaintainabilityIndex = vclass.MaintainabilityIndex;
            CyclomaticComplexity = vclass.CyclomaticComplexity;
            ClassCoupling = vclass.ClassCoupling;
            LinesOfCode = vclass.LinesOfCode;
        }

        public Vector3 Position
        {
            get { return new Vector3(X, Height, Y); }
        }

    }

}
