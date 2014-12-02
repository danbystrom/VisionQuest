using factor10.VisionThing.Effects;
using SharpDX;

namespace factor10.VisionThing
{
    public interface IVDrawable
    {
        void Draw(IVEffect effect, int lod = 0);
    }

    public interface IPosition
    {
        Vector3 Position { get; }
    }

}
