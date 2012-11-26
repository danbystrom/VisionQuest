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
            ShadowMap shadowMap);

        public void Draw(
            Camera camera,
            DrawingReason drawingReason = DrawingReason.Normal,
            ShadowMap shadowMap = null)
        {
            switch ( drawingReason )
            {
                case DrawingReason.Normal:
                    Effect.Effect.CurrentTechnique = Effect.Effect.Techniques[0];
                    break;
                case DrawingReason.ShadowDepthMap:
                    Effect.Effect.CurrentTechnique = Effect.Effect.Techniques[2];
                    break;
            }
            Effect.SetShadowMapping(shadowMap);
            draw(
                camera,
                drawingReason,
                shadowMap);
        }

        public virtual void Draw(
            Vector4? clipPlane,
            Camera camera,
            ShadowMap shadowMap = null)
        {
            Effect.ClipPlane = clipPlane;
            Draw(camera, DrawingReason.ReflectionMap, shadowMap);
            Effect.ClipPlane = null;
        }

    }

}
