using factor10.VisionThing;
using factor10.VisionThing.Effects;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Serpent
{
    public class ModelWrapper
    {
        private readonly Model _model;
        private readonly Matrix[] _bones;

        public ModelWrapper( Game game, Model model)
        {
            _model = model;
            _bones = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(_bones);

            foreach (var mesh in _model.Meshes)
                foreach (var part in mesh.MeshParts)
                    if (part.Effect is BasicEffect)
                    {
                        var oldBe = part.Effect as BasicEffect;
                        var newBe = new StockBasicEffect(game.GraphicsDevice);
                        newBe.CopyBasicEffect(oldBe);
                        newBe.SpecularPower = 32;
                        newBe.OriginalDiffuseColor = oldBe.DiffuseColor;
                        newBe.EnableDefaultLighting();
                        //newBe.DiffuseColor = oldBe.DiffuseColor;
                        //newBe.OriginalDiffuseColor = oldBe.DiffuseColor;
                        //newBe.EmissiveColor = oldBe.EmissiveColor;
                        //newBe.AmbientLightColor = oldBe.AmbientLightColor;
                        part.Effect = newBe;
                    }
        }

        public void Draw(
            Camera camera,
            Matrix world,
            Vector4 tintColor,
            float tintFactor,
            float alpha )
        {
                foreach (var mesh in _model.Meshes)
                {
                    foreach (var effect in mesh.Effects)
                    {
                        var e = (StockBasicEffect) effect;
                        e.DiffuseColor = Vector4.Lerp( e.OriginalDiffuseColor,  tintColor, tintFactor);
                        e.Alpha = alpha;
                        e.View = camera.View;
                        e.Projection = camera.Projection;
                        e.World = _bones[mesh.ParentBone.Index]*world;
                    }
                    mesh.Draw(mesh.Effects[0].GraphicsDevice);
                }
        }

    }

}
