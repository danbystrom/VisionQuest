using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace factor10.VisionaryHeads
{
    interface IMetrics
    {
        int InstructionCount { get; set; }
        int MaintainabilityIndex { get; set; }
        int CyclomaticComplexity { get; set; }
        int ClassCoupling { get; set; }
        int LinesOfCode { get; set; }
    }
}
