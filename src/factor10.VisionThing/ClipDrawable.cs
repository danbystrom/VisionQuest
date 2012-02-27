using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factor10.VisionThing
{
    public abstract class ClipDrawable
    {
        public readonly Effect Effect;

        protected readonly EffectParameter _epWorld;
        protected readonly EffectParameter _epView;
        protected readonly EffectParameter _epProjection;
        protected readonly EffectParameter _epCameraPosition;
        protected readonly EffectParameter _epClipPlane;

        protected readonly EffectTechnique _techNormal;
        protected readonly EffectTechnique _techClipPlane;

        protected ClipDrawable(Effect effect)
        {
            Effect = effect;
            _epWorld = effect.Parameters["World"];
            _epView = effect.Parameters["View"];
            _epProjection = effect.Parameters["Projection"];
            _epCameraPosition = effect.Parameters["CameraPosition"];
            _epClipPlane = effect.Parameters["ClipPlane"];

            _techNormal = effect.Techniques[0];
            _techClipPlane = effect.Techniques[1];
        }

        protected abstract void Draw(Effect effect);
        protected abstract void Draw();

        public virtual void Draw(Camera camera)
        {
            _epView.SetValue(camera.View);
            _epProjection.SetValue(camera.Projection);
            _epCameraPosition.SetValue(camera.Position);
            Draw();
        }

        public void Draw(Camera camera, Vector4 clipPlane)
        {
            if ( _techClipPlane != null )
                Effect.CurrentTechnique = _techClipPlane;
            if ( _epClipPlane != null )
                _epClipPlane.SetValue(clipPlane);
            Draw(camera);
            Effect.CurrentTechnique = _techNormal;
        }

    }

}
