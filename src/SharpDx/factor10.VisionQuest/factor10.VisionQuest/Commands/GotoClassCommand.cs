using System.Linq;
using factor10.VisionaryHeads;
using factor10.VisionQuest.Actions;

namespace factor10.VisionQuest.Commands
{
    public class GotoClassCommand : ICommand
    {
        private readonly VClass _vclass;

        public GotoClassCommand(VClass vclass)
        {
            _vclass = vclass;
        }

        public void Excecute(SharedData data)
        {
            var visionClass = data.Archipelag.CodeIslands.SelectMany(_ => _.Classes).Single(_ => _.Value.VClass == _vclass).Value;
            var pos = visionClass.Position + visionClass.CodeIsland.World.TranslationVector;
            pos.Y += 5;  // place the camera a bit above
            var directionToCurrentPosition = pos - data.Camera.Position;
            directionToCurrentPosition.Y = 0;
            directionToCurrentPosition.Normalize();
            directionToCurrentPosition.Y = -0.3f;
            data.Actions.Add(new MoveCameraToPositionAction(data.Camera, pos - directionToCurrentPosition*30, pos));
        }

    }

}
