using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionThing
{
    public class SkySphere : ClipDrawable
    {
        readonly IGeometricPrimitive _sphere;

        public SkySphere(
            VisionContent vtContent,
            TextureCube texture)
            : base(new VisionEffect(vtContent.Load<Effect>("effects/skysphere")))
        {
            _sphere = new SpherePrimitive<VertexPosition>(vtContent.GraphicsDevice, (p, n, t) => new VertexPosition(p), 20000, 10, false);
            Effect.Parameters["Texture"].SetResource(texture);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            camera.UpdateEffect(Effect);
            Effect.World = Matrix.Scaling(1, 0.5f, 1)*Matrix.Translation(camera.Position);

            //TODO
            //var saveCull = Effect.GraphicsDevice.RasterizerState;
            _sphere.Draw(Effect);
            //Effect.GraphicsDevice.RasterizerState = saveCull;
            //Effect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            return true;
        }

    }

}
