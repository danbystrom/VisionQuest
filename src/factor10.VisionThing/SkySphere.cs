using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;

namespace factor10.VisionThing
{
    public class SkySphere : ClipDrawable
    {
        readonly SpherePrimitive _sphere;

        public SkySphere(
             TextureCube texture)
            : base( VisionContent.LoadPlainEffect("effects/skysphere"))
        {
            _sphere = new SpherePrimitive( Effect.GraphicsDevice, 10000, 10);
            Effect.Parameters["CubeMap"].SetValue(texture);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            camera.UpdateEffect(Effect);
            Effect.World = Matrix.CreateTranslation(camera.Position);

            var saveCull = Effect.GraphicsDevice.RasterizerState;
            _sphere.Draw(Effect);
            Effect.GraphicsDevice.RasterizerState = saveCull;
            Effect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            return true;
        }

    }

}
