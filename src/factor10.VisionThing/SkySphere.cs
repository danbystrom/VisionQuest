using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using factor10.VisionThing.Primitives;

namespace factor10.VisionThing
{
    // Make SkySphere IRenderable
    public class SkySphere //: IRenderable
    {
        readonly SpherePrimitive _sphere;
        readonly Effect _effect;
        readonly GraphicsDevice _graphics;

        public SkySphere(
            GraphicsDevice graphicsDevice,
            ContentManager contentManager,
            TextureCube texture)
        {
            _sphere = new SpherePrimitive( graphicsDevice, 2000, 10);

            _effect = contentManager.Load<Effect>("effects/skysphere");
            _effect.Parameters["CubeMap"].SetValue(texture);

            _graphics = graphicsDevice;
        }

        public void Draw(Camera camera)
        {
            // Move the model with the sphere
            _effect.Parameters["Projection"].SetValue(camera.Projection);
            _effect.Parameters["View"].SetValue(camera.View);
            _effect.Parameters["CameraPosition"].SetValue(camera.Position);
            var saveCull = _graphics.RasterizerState;
            _sphere.Draw(_effect);
            _graphics.RasterizerState = saveCull;
        }

        public void SetClipPlane(Vector4? plane)
        {
            _effect.Parameters["ClipPlaneEnabled"].SetValue(plane.HasValue);
            
            if (plane.HasValue)
                _effect.Parameters["ClipPlane"].SetValue(plane.Value);
        }
    }
}
