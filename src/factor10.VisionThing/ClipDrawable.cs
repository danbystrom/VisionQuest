using System;
using System.Collections.Generic;
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
        public readonly List<ClipDrawable> Children = new List<ClipDrawable>();
        public readonly IEffect Effect;

        protected ClipDrawable(IEffect effect)
        {
            Effect = effect;
        }

        protected ClipDrawable(ClipDrawable cd)
            : this(cd.Effect)
        {
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
            Effect.SetTechnique(drawingReason);
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
        }

        public virtual void Update(GameTime gameTime)
        {
            Children.ForEach(cd => cd.Update(gameTime));
        }

    }

}
