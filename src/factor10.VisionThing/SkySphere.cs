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
            : base( new PlainEffectWrapper( VisionContent.Load<Effect>("effects/skysphere")))
        {
            _sphere = new SpherePrimitive( Effect.GraphicsDevice, 10000, 10);
            Effect.Parameters["CubeMap"].SetValue(texture);
        }

        protected override void draw(Camera camera, DrawingReason drawingReason, IEffect effect, ShadowMap shadowMap)
        {
            camera.UpdateEffect(effect);
            effect.World = Matrix.CreateTranslation(camera.Position);

            var saveCull = effect.GraphicsDevice.RasterizerState;
            _sphere.Draw(effect);
            effect.GraphicsDevice.RasterizerState = saveCull;
            effect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

    }

}
