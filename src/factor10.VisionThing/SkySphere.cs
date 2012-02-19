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
            _epWorld.SetValue(Matrix.Identity);
            _sphere = new SpherePrimitive( Effect.GraphicsDevice, 1000, 10);
            Effect.Parameters["CubeMap"].SetValue(texture);
        }

        protected override void Draw()
        {
            var saveCull = Effect.GraphicsDevice.RasterizerState;
            _sphere.Draw(Effect);
            Effect.GraphicsDevice.RasterizerState = saveCull;
            Effect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

        }

    }

}
