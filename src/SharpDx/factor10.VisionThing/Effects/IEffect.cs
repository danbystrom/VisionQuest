﻿using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionThing.Effects
{
    public interface IEffect
    {
        GraphicsDevice GraphicsDevice { get; }
        Matrix World { get; set; }
        Matrix View { get; set; }
        Matrix Projection { get; set; }
        Vector3 CameraPosition { get; set; }
        Vector4? ClipPlane { set; }
        Texture2D Texture { get; set; }
        Effect Effect { get; }
        void Apply();
        EffectParameterCollection Parameters { get; }
        void SetShadowMapping(ShadowMap shadow);
        void SetTechnique(DrawingReason drawingReason);
    }
}