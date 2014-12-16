using factor10.VisionaryHeads;
using factor10.VisionThing;

namespace factor10.VisionQuest.Commands
{
    public class LoadProgramCommand : ICommand
    {
        private readonly VProgram _vProgram;

        public LoadProgramCommand(VProgram vProgram)
        {
            _vProgram = vProgram;
        }

        public void Excecute(SharedData data)
        {
            if (data.Archipelag != null)
                data.Archipelag.Kill(data.Water, data.Shadow);
            data.Archipelag = new Archipelag(
                data.VContent,
                _vProgram,
                data.Water,
                data.Shadow);
        }

    }

}
