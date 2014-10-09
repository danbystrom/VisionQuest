using factor10.VisionThing.Effects;

namespace factor10.VisionThing
{
    public interface IVDrawable
    {
        void Draw(IVEffect effect, int lod = 0);
    }
}
