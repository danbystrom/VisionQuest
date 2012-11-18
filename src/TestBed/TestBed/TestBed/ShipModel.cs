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

        private readonly Texture2D _texture;

        public ShipModel()
            : base(new NewBasicEffect(VisionContent.Load<Effect>("effects/NewBasicEffect")))
        {
            _model = VisionContent.Load<Model>(@"Models\pirateship");
            _bones = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(_bones);

            foreach (var mesh in _model.Meshes)
                foreach (var part in mesh.MeshParts)
                {
                    var basicEffect = part.Effect as BasicEffect;
                    if (basicEffect != null)
                         _texture = basicEffect.Texture;
                    part.Effect = Effect.Effect;
                }

            var nbe = (NewBasicEffect)Effect;
            nbe.TextureEnabled = true;
            nbe.EnableDefaultLighting();
        }

        public void Update(GameTime gameTime)
        {
            _bob1.Update(gameTime);
            _bob2.Update(gameTime);
        }
    
        public override void Draw(Camera camera, IEffect effect)
        {
            camera.UpdateEffect(effect);
            effect.Texture = _texture;
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
        }
        
    }

}
