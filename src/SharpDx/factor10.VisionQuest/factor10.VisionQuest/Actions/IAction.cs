using SharpDX.Toolkit;

namespace factor10.VisionQuest.Actions
{
    public interface IAction
    {
        bool Do(SharedData data, GameTime gameTime);
    }
}
