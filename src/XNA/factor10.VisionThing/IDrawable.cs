using factor10.VisionThing.Effects;

namespace factor10.VisionThing
{
    public interface IDrawable
    {
        void Draw(IEffect effect, int lod = 0);
    }
}
