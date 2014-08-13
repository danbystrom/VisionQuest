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
            Game game,
            TextureCube texture)
            : base(new PlainEffectWrapper(game.Content.Load<Effect>("effects/skysphere")))
        {
            _sphere = new SpherePrimitive<VertexPosition>(game.GraphicsDevice, (p, n, t) => new VertexPosition(p), 5, 10);
            Effect.Parameters["Texture"].SetResource(texture);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            camera.UpdateEffect(Effect);
            Effect.World = Matrix.Translation( Vector3.Right*10 /*camera.Position*/);

            //TODO
            //var saveCull = Effect.GraphicsDevice.RasterizerState;
            _sphere.Draw(Effect);
            //Effect.GraphicsDevice.RasterizerState = saveCull;
            //Effect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            return true;
        }

    }

}
