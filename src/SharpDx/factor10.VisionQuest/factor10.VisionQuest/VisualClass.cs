﻿using System.Linq;
using factor10.VisionaryHeads;
using SharpDX;

namespace factor10.VisionQuest
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
        public readonly int R;
        public float Height;

        public VisualClass(VClass vclass, int x, int y, int r)
        {
            VClass = vclass;
            X = x;
            Y = y;
            R = r;
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