using System;
using System.Linq;
using factor10.VisionThing;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionQuest
{
    class ShipModel : ClipDrawable
    {
        private readonly Model _model;
        private readonly Matrix[] _bones;

        public Matrix World = Matrix.Scaling(0.5f) * Matrix.Translation(-7, 1f, 33);

        private DoubleSin _bob1 = new DoubleSin(0.05f, 0.010f, 0.3f, 0.9f, 0, 1);
        private DoubleSin _bob2 = new DoubleSin(0.04f, 0.008f, 0.5f, 0.8f, 2, 3);

        private static Texture2D _texture;
        private BoundingSphere _boundingSphere;

        public ShipModel(VisionContent vContent)
            : base(vContent.LoadPlainEffect("effects/SimpleTextureEffect"))
        {
            _model = vContent.Load<Model>(@"Models/pirateship");
            _bones = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(_bones);

            foreach (var part in _model.Meshes.SelectMany(mesh => mesh.MeshParts))
            {
                if (_texture == null)
                {
                    var basicEffect = part.Effect as BasicEffect;
                    if (basicEffect != null)
                        _texture = (Texture2D)basicEffect.Texture;
                }
                part.Effect = Effect.Effect;
            }

            var modelCenter = Vector4.Zero;

            foreach (var mesh in _model.Meshes)
            {
                var meshBounds = mesh.BoundingSphere;
                var transform = _bones[mesh.ParentBone.Index];
                var meshCenter = Vector3.Transform(meshBounds.Center, transform);
                modelCenter += meshCenter;
            }

            modelCenter /= _model.Meshes.Count;

            // Now we know the center point, we can compute the model radius
            // by examining the radius of each mesh bounding sphere.
            var modelRadius = 0f;

            foreach (var mesh in _model.Meshes)
            {
                var meshBounds = mesh.BoundingSphere;
                var transform = _bones[mesh.ParentBone.Index];
                var meshCenter = Vector3.Transform(meshBounds.Center, transform);

                var transformScale = transform.Forward.Length();

                var meshRadius = (meshCenter - modelCenter).Length() + (meshBounds.Radius*transformScale);

                modelRadius = Math.Max(modelRadius,  meshRadius);
            }

            _boundingSphere = new BoundingSphere(new Vector3(modelCenter.X, modelCenter.Y, modelCenter.Z), modelRadius);
        }

        public override void Update(GameTime gameTime)
        {
            _bob1.Update(gameTime);
            _bob2.Update(gameTime);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            var testSphere = new BoundingSphere(Vector3.TransformCoordinate(_boundingSphere.Center, World), _boundingSphere.Radius);
            if ( camera.BoundingFrustum.Contains(testSphere) == ContainmentType.Disjoint )
                return false;
            camera.UpdateEffect(Effect);
            Effect.Texture = _texture;
            foreach (var mesh in _model.Meshes)
            {
                Effect.World =
                    Matrix.RotationZ((float) _bob1.Value)*
                    Matrix.RotationX((float) _bob2.Value)*
                    _bones[mesh.ParentBone.Index]*
                    World;
                Effect.Apply();
                mesh.Draw(Effect.GraphicsDevice);
            }
            return true;
        }
        
    }

}
