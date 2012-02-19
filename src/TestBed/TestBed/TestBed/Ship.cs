using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing;

namespace TestBed
{
    class Ship : ClipDrawable
    {
        private readonly Model _model;
        private readonly Matrix[] _bones;

        public Matrix World = Matrix.CreateScale(0.15f) * Matrix.CreateTranslation(7, 1f, 23);

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
                        e.World = _bones[mesh.ParentBone.Index]*World;
                    }
                    mesh.Draw();
                }
       }

        protected override void Draw()
        {
        }

    }
}
