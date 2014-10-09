using System.Drawing;
using factor10.VisionaryHeads;

namespace factor10.VisionQuest
{
    sealed class SharedData
    {
        public Size Size { get; set; }
        public Storage Storage { get; set; }
        public VProgram LoadProgram { get; set; }

        public VisionClass SelectedClass;
    }

}