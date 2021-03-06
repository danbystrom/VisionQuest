﻿using System;
using System.Collections.Generic;
using factor10.VisionThing.Effects;
using SharpDX;
using SharpDX.Toolkit;

namespace factor10.VisionThing
{
    public enum DrawingReason
    {
        Normal,
        ReflectionMap,
        ShadowDepthMap
    }

    public abstract class ClipDrawable : IDisposable
    {
        public readonly List<ClipDrawable> Children = new List<ClipDrawable>();
        public readonly IVEffect Effect;

        public BoundingSphere BoundingSphere { get; protected set; }

        protected ClipDrawable(IVEffect effect)
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
            if (Effect != null)
            {
                Effect.SetTechnique(drawingReason);
                Effect.SetShadowMapping(drawingReason != DrawingReason.ShadowDepthMap ? shadowMap : null);
                var didPaint = draw(camera, drawingReason, shadowMap);
                Effect.SetShadowMapping(null);
                if (!didPaint)
                    return false;
            }
            Children.ForEach(cd => cd.Draw(camera, drawingReason, shadowMap));
            return true;
        }

        public virtual void DrawReflection(
            Vector4? clipPlane,
            Camera camera)
        {
            if(Effect!=null)
                Effect.ClipPlane = clipPlane;
            Draw(camera, DrawingReason.ReflectionMap);
            Children.ForEach(cd => cd.DrawReflection(clipPlane, camera));
        }

        public virtual void Update(Camera camera, GameTime gameTime)
        {
            Children.ForEach(cd => cd.Update(camera, gameTime));
        }

        public virtual void Dispose()
        {
        }

    }

}
