using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using factor10.VisionThing.Primitives;

namespace factor10.VisionThing
{
    public class SkySphere : ClipDrawable
    {
        readonly SpherePrimitive _sphere;

        public SkySphere(
             TextureCube texture)
            : base(VisionContent.Load<Effect>("effects/skysphere"))
        {
            _sphere = new SpherePrimitive( Effect.GraphicsDevice, 1000, 10);
            Effect.Parameters["CubeMap"].SetValue(texture);
        }

        public override void Draw(Camera camera)
        {
            _epWorld.SetValue(Matrix.CreateTranslation(camera.Position));
            base.Draw(camera);
        }

        protected override void Draw()
        {
            Draw(Effect);
        }

        protected override void Draw(Effect effect)
        {
            var saveCull = effect.GraphicsDevice.RasterizerState;
            _sphere.Draw(effect);
            effect.GraphicsDevice.RasterizerState = saveCull;
            effect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

    }

}
