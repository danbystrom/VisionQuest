using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionThing.Effects
{
    public interface IVEffect
    {
        GraphicsDevice GraphicsDevice { get; }
        Matrix World { get; set; }
        Matrix View { get; set; }
        Matrix Projection { get; set; }
        Vector3 CameraPosition { get; set; }
        Vector3 SunlightDirection { get; set; }
        Vector4 DiffuseColor { get; set; }
        Vector4? ClipPlane { set; }
        Texture2DBase Texture { get; set; }
        Effect Effect { get; }
        void Apply();
        EffectParameterCollection Parameters { get; }
        void SetShadowMapping(ShadowMap shadow);
        void SetTechnique(DrawingReason drawingReason);
    }

    public class VBasicEffect : BasicEffect, IVEffect
    {
        public VBasicEffect(GraphicsDevice device)
            : base(device)
        {
        }

        public Vector3 CameraPosition { get; set; }
        public Vector3 SunlightDirection { get; set; }
        public Vector4? ClipPlane { set; get; }
        public Effect Effect {
            get { return this; } 
        }

        public void Apply()
        {
            CurrentTechnique.Passes[0].Apply();
        }

        public void SetShadowMapping(ShadowMap shadow)
        {
        }

        public void SetTechnique(DrawingReason drawingReason)
        {
        }

    }

}
