using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing.Effects;

namespace factor10.VisionThing
{
    public enum DrawingReason
    {
        Normal,
        ReflectionMap,
        ShadowDepthMap
    }

    public abstract class ClipDrawable
    {
        public readonly IEffect Effect;

        protected ClipDrawable(IEffect effect)
        {
            Effect = effect;
        }

        protected abstract void draw(
            Camera camera,
            DrawingReason drawingReason,
            IEffect effect,
            ShadowMap shadowMap);

        public void Draw(
            Camera camera,
            DrawingReason drawingReason = DrawingReason.Normal,
            IEffect effect = null,
            ShadowMap shadowMap = null)
        {
            draw(
                camera,
                drawingReason,
                effect ?? Effect,
                shadowMap);
        }

        public virtual void Draw(
            Vector4? clipPlane,
            Camera camera,
            DrawingReason drawingReason = DrawingReason.Normal,
            IEffect effect = null,
            ShadowMap shadowMap = null)
        {
            Effect.ClipPlane = clipPlane;
            Draw(camera, drawingReason, effect, shadowMap);
            Effect.ClipPlane = null;
        }

    }

}
