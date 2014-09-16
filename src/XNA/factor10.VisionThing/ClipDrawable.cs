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

        protected abstract bool draw(
            Camera camera,
            DrawingReason drawingReason,
            ShadowMap shadowMap);

        public bool Draw(
            Camera camera,
            DrawingReason drawingReason = DrawingReason.Normal,
            ShadowMap shadowMap = null)
        {
            Effect.SetTechnique(drawingReason);
            Effect.SetShadowMapping(drawingReason != DrawingReason.ShadowDepthMap ? shadowMap : null);
            if (!draw(camera, drawingReason, shadowMap))
                return false;
            Children.ForEach(cd => cd.Draw(camera, drawingReason, shadowMap));
            return true;
        }

        public virtual void DrawReflection(
            Vector4? clipPlane,
            Camera camera)
        {
            Effect.ClipPlane = clipPlane;
            Draw(camera, DrawingReason.ReflectionMap);
        }

        public virtual void Update(GameTime gameTime)
        {
            Children.ForEach(cd => cd.Update(gameTime));
        }

    }

}
