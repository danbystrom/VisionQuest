using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing;

namespace TestBed
{
    class Ship : ClipDrawable
    {
        private readonly Model _model;
        private readonly Matrix[] _bones;

        public Matrix World = Matrix.CreateScale(0.2f) * Matrix.CreateTranslation(7, 1f, 23);

        private DoubleSin _bob1 = new DoubleSin(0.07f, 0.02f, 0.4f, 0.9f, 0, 1);
        private DoubleSin _bob2 = new DoubleSin(0.05f, 0.01f, 0.5f, 0.8f, 2, 3);

        public Ship()
            : base(VisionContent.Load<Effect>(@"Effects\lightingeffect"))
        {
            _model = VisionContent.Load<Model>(@"Models\pirateship");
            _bones = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(_bones);

            foreach (var mesh in _model.Meshes)
                foreach (var part in mesh.MeshParts)
                    if (part.Effect is BasicEffect)
                    {
                        var oldBe = part.Effect as BasicEffect;
                        var newBe = new factor10.VisionThing.StockEffects.BasicEffect(oldBe.GraphicsDevice);
                        newBe.CopyBasicEffect(oldBe);
                        newBe.SpecularPower = 32;
                        newBe.OriginalDiffuseColor = oldBe.DiffuseColor;
                        newBe.EnableDefaultLighting();
                        newBe.TextureEnabled = true;
                        newBe.Texture = oldBe.Texture;
                        part.Effect = newBe;
                    }
        }

        public void Update( GameTime gameTime)
        {
            _bob1.Update(gameTime);
            _bob2.Update(gameTime);
        }

        public override void Draw(Camera camera)
        {
                foreach (var mesh in _model.Meshes)
                {
                    foreach (factor10.VisionThing.StockEffects.BasicEffect e in mesh.Effects)
                    {
                        //e.DiffuseColor = Vector3.Lerp(e.OriginalDiffuseColor, tintColor, tintFactor);
                        //e.Alpha = alpha;
                        e.View = camera.View;
                        e.Projection = camera.Projection;
                        e.World =
                            Matrix.CreateRotationZ((float)_bob1.Value)*
                            Matrix.CreateRotationX((float)_bob2.Value)*
                            _bones[mesh.ParentBone.Index]*
                            World;
                    }
                    mesh.Draw();
                }
       }

        protected override void Draw()
        {
        }

    }
}
