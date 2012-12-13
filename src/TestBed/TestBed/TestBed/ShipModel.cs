using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing;
using factor10.VisionThing.Effects;

namespace TestBed
{
    class ShipModel : ClipDrawable
    {
        private readonly Model _model;
        private readonly Matrix[] _bones;

        public Matrix World = Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(-7, 1f, 33);

        private DoubleSin _bob1 = new DoubleSin(0.05f, 0.010f, 0.3f, 0.9f, 0, 1);
        private DoubleSin _bob2 = new DoubleSin(0.04f, 0.008f, 0.5f, 0.8f, 2, 3);

        private static Texture2D _texture;
        private BoundingSphere _boundingSphere;

        public ShipModel()
            : base(VisionContent.LoadPlainEffect("effects/SimpleTextureEffect"))
        {
            _model = VisionContent.Load<Model>(@"Models/pirateship");
            _bones = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(_bones);

            foreach (var part in _model.Meshes.SelectMany(mesh => mesh.MeshParts))
            {
                if (_texture == null)
                {
                    var basicEffect = part.Effect as BasicEffect;
                    if (basicEffect != null)
                        _texture = basicEffect.Texture;
                }
                part.Effect = Effect.Effect;
            }

            var modelCenter = Vector3.Zero;

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

            foreach (ModelMesh mesh in _model.Meshes)
            {
                var meshBounds = mesh.BoundingSphere;
                var transform = _bones[mesh.ParentBone.Index];
                var meshCenter = Vector3.Transform(meshBounds.Center, transform);

                var transformScale = transform.Forward.Length();

                var meshRadius = (meshCenter - modelCenter).Length() + (meshBounds.Radius*transformScale);

                modelRadius = Math.Max(modelRadius,  meshRadius);
            }

            _boundingSphere = new BoundingSphere(modelCenter, modelRadius);
        }

        public override void Update(GameTime gameTime)
        {
            _bob1.Update(gameTime);
            _bob2.Update(gameTime);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            BoundingSphere testSphere;
            _boundingSphere.Transform(ref World, out testSphere);
            if ( camera.BoundingFrustum.Contains(testSphere) == ContainmentType.Disjoint )
                return false;
            camera.UpdateEffect(Effect);
            Effect.Texture = _texture;
            foreach (var mesh in _model.Meshes)
            {
                Effect.World =
                    Matrix.CreateRotationZ((float) _bob1.Value)*
                    Matrix.CreateRotationX((float) _bob2.Value)*
                    _bones[mesh.ParentBone.Index]*
                    World;
                Effect.Apply();
                mesh.Draw();
            }
            return true;
        }
        
    }

}
