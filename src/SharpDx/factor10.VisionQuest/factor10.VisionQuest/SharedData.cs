using System.Drawing;
using factor10.VisionaryHeads;

namespace factor10.VisionQuest
{
    internal sealed class SharedData
    {
        public Size Size { get; set; }
        public Storage Storage { get; set; }
        public VProgram LoadProgram { get; set; }

        public bool HiddenWater;
        public int WaterSurfaceSize = 10;
        public int WaterSurfaceScale = 3;

        public VisionClass SelectedClass;
    }

}