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
        private readonly Matrix[] _bones;

        public Matrix World = Matrix.Scaling(5f)*Matrix.Translation(-7, 1f, 33);

        private DoubleSin _bob1 = new DoubleSin(0.05f, 0.010f, 0.3f, 0.9f, 0, 1);
        private DoubleSin _bob2 = new DoubleSin(0.04f, 0.008f, 0.5f, 0.8f, 2, 3);

        private static Texture2D _texture;
        private BoundingSphere _boundingSphere;

        public ShipModel(VisionContent vContent)
            : base(vContent.LoadPlainEffect("effects/SimpleTextureEffect"))
        {
            _model = vContent.Load<Model>(@"Models/galleonmodel");
            BasicEffect.EnableDefaultLighting(_model, true);

            _bones = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(_bones);

            foreach (var part in _model.Meshes.SelectMany(mesh => mesh.MeshParts))
            {
                //if (_texture == null)
                //{
                //    var basicEffect = part.Effect as BasicEffect;
                //    if (basicEffect != null)
                //        _texture = (Texture2D)basicEffect.Texture;
                //}
                part.Effect = Effect.Effect;
            }

            _boundingSphere = _model.CalculateBounds();
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
            _bob1.Update(gameTime);
            _bob2.Update(gameTime);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            _model.Draw(Effect.GraphicsDevice, World, camera.View, camera.Projection);
            //var testSphere = new BoundingSphere(Vector3.TransformCoordinate(_boundingSphere.Center, World), _boundingSphere.Radius);
            //if (camera.BoundingFrustum.Contains(testSphere) == ContainmentType.Disjoint)
            //    return false;
            //camera.UpdateEffect(Effect);
            //Effect.Texture = _texture;
            //foreach (var mesh in _model.Meshes)
            //{
            //    Effect.World =
            //        Matrix.RotationZ((float) _bob1.Value)*
            //        Matrix.RotationX((float) _bob2.Value)*
            //        _bones[mesh.ParentBone.Index]*
            //        World;
            //    Effect.Apply();
            //    mesh.Draw(Effect.GraphicsDevice);
            //}
            return true;
        }

    }

}
