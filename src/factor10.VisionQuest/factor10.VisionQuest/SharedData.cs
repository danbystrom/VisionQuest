using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using factor10.VisionQuest.Actions;
using factor10.VisionQuest.Commands;
using factor10.VisionThing;
using factor10.VisionThing.CameraStuff;
using factor10.VisionThing.Water;

namespace factor10.VisionQuest
{
    public sealed class SharedData
    {
        public Size Size { get; set; }
        public Storage Storage { get; set; }

        public readonly ConcurrentQueue<ICommand> Commands = new ConcurrentQueue<ICommand>();
        public readonly List<IAction> Actions = new List<IAction>();

        public bool HiddenWater;
        public int WaterSurfaceSize = 10;
        public int WaterSurfaceScale = 3;

        public VisionClass SelectedClass;

        internal VisionContent VContent;
        internal Camera Camera;
        internal Archipelag Archipelag;
        internal WaterSurface Water;
        internal ShadowMap Shadow;
    }

}