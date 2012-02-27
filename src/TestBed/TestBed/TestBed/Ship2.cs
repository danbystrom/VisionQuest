using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing;
using factor10.VisionThing.StockEffects;

namespace TestBed
{
    class Ship2 : ClipDrawable
    {
        private readonly Model _model;
        private readonly Matrix[] _bones;

        public Matrix World = Matrix.CreateScale(0.3f) * Matrix.CreateTranslation(-7, 1f, 23);

        private DoubleSin _bob1 = new DoubleSin(0.07f, 0.02f, 0.4f, 0.9f, 0, 1);
        private DoubleSin _bob2 = new DoubleSin(0.05f, 0.01f, 0.5f, 0.8f, 2, 3);

        private readonly Texture2D _texture;

        private NewBasicEffect _nbe;

        public Ship2()
            : base(VisionContent.Load<Effect>("effects/NewBasicEffect"))
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
                    part.Effect = Effect;
                }

            _nbe = new NewBasicEffect(Effect);
            _nbe.EnableDefaultLighting();
            _nbe.TextureEnabled = true;
        }

        public void Update( GameTime gameTime)
        {
            _bob1.Update(gameTime);
            _bob2.Update(gameTime);
        }

        public override void Draw(Camera camera)
        {
            _nbe.Texture = _texture;
            _nbe.View = camera.View;
            _nbe.Projection = camera.Projection;
            foreach (var mesh in _model.Meshes)
            {
                _nbe.World =
                    Matrix.CreateRotationZ((float) _bob1.Value)*
                    Matrix.CreateRotationX((float) _bob2.Value)*
                    _bones[mesh.ParentBone.Index]*
                    World;
                _nbe.Apply();
                mesh.Draw();
            }
        }

        protected override void Draw()
        {
        }

        protected override void Draw(Effect effect)
        {
        }

    }
}
