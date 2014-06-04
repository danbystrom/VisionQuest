using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionThing
{
    public class SkySphere : ClipDrawable
    {
        readonly SpherePrimitive<VertexPositionNormal> _sphere;

        public SkySphere(
             TextureCube texture)
            : base( VisionContent.LoadPlainEffect("effects/skysphere"))
        {
            _sphere = new SpherePrimitive<VertexPositionNormal>(Effect.GraphicsDevice, (p, n, t) => new VertexPositionNormal(p, n), 20000, 10);
            Effect.Parameters["CubeMap"].SetResource(texture);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            camera.UpdateEffect(Effect);
            Effect.World = Matrix.Translation(camera.Position);

            //TODO
            //var saveCull = Effect.GraphicsDevice.RasterizerState;
            //_sphere.Draw(Effect);
            //Effect.GraphicsDevice.RasterizerState = saveCull;
            //Effect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            return true;
        }

    }

}
