using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing.Effects;

namespace factor10.VisionThing
{
    public abstract class ClipDrawable
    {
        public readonly IEffect Effect;

        protected ClipDrawable(IEffect effect)
        {
            Effect = effect;
        }

        public abstract void Draw(Camera camera, IEffect effect);

        public virtual void Draw(Camera camera)
        {
            Draw(camera, Effect);
        }

        public virtual void Draw(Camera camera, Vector4? clipPlane)
        {
            Effect.ClipPlane = clipPlane;
            Draw(camera);
            Effect.ClipPlane = null;
        }

    }

}
