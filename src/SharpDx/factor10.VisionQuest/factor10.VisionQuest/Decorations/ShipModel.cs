using factor10.VisionThing;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using System.Linq;

namespace factor10.VisionQuest
{
    internal class ShipModel : ClipDrawable
    {
        private readonly Model _model;

        public Matrix World = Matrix.Scaling(10000f)*Matrix.Translation(-7, 1f, 33);

        private DoubleSin _bob1 = new DoubleSin(0.05f, 0.010f, 0.3f, 0.9f, 0, 1);
        private DoubleSin _bob2 = new DoubleSin(0.02f, 0.003f, 0.5f, 0.8f, 2, 3);

        private static Texture2DBase _texture;
        private BoundingSphere _boundingSphere;

        public ShipModel(VisionContent vContent)
            : base(vContent.LoadPlainEffect("effects/SimpleTextureEffect"))
        {
            _model = vContent.Load<Model>(@"Models/galleonmodel");
            BasicEffect.EnableDefaultLighting(_model, true);

            _texture = _model.Meshes.SelectMany(_ => _.MeshParts).Select(_ => _.Effect).OfType<BasicEffect>().Select(_ => _.Texture).FirstOrDefault(_ => _ != null);
            _boundingSphere = _model.CalculateBounds();
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
            _bob1.Update(gameTime);
            _bob2.Update(gameTime);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            var testSphere = new BoundingSphere(Vector3.TransformCoordinate(_boundingSphere.Center, World), _boundingSphere.Radius);
            if (camera.BoundingFrustum.Contains(testSphere) == ContainmentType.Disjoint)
                return false;

            camera.UpdateEffect(Effect);
            Effect.Texture = _texture;

            var world = Matrix.RotationZ((float) _bob1.Value)*Matrix.RotationX((float) _bob2.Value)*World;
            _model.Draw(Effect.GraphicsDevice, world, camera.View, camera.Projection, Effect.Effect);

            return true;
        }

    }

}
