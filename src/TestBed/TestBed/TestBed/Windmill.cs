using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing;
using factor10.VisionThing.StockEffects;

namespace TestBed
{
    class Windmill : ClipDrawable
    {
        private readonly Model _model;
        private readonly Matrix[] _bones;

        public Matrix World = Matrix.CreateRotationY(MathHelper.PiOver2) * Matrix.CreateScale(0.006f) * Matrix.CreateTranslation(-2, 1.3f, 7);

        private readonly ObjectAnimation _animation;
        private readonly Texture2D _texture;

        public Windmill()
            : base(VisionContent.Load<Effect>(@"Effects\lightingeffecttexture"))
        {
            _model = VisionContent.Load<Model>(@"Models\windmill");
            _texture = VisionContent.Load<Texture2D>(@"Textures\windmill_diffuse");
            _bones = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(_bones);

            _animation = new ObjectAnimation(new Vector3(0, 875, 0), new Vector3(0, 875, 0),
                Vector3.Zero, new Vector3(0, 0, MathHelper.TwoPi),
                TimeSpan.FromSeconds(10), true);

            foreach (var mesh in _model.Meshes)
                foreach (var part in mesh.MeshParts)
                    if (part.Effect is BasicEffect)
                    {
                        //var oldBe = part.Effect as BasicEffect;
                        part.Effect = Effect;
                        //newBe.CopyBasicEffect(oldBe);
                        //newBe.SpecularPower = oldBe.SpecularPower;
                        //newBe.OriginalDiffuseColor = oldBe.DiffuseColor;
                        //newBe.EnableDefaultLighting();
                        //newBe.TextureEnabled = oldBe.TextureEnabled;
                        //newBe.Texture = oldBe.Texture;
                        //newBe.EnableDefaultLighting();
                        //newBe.PreferPerPixelLighting = true;
                        Effect.Parameters["TextureEnabled"].SetValue(true);
                        //Effect.Parameters["BasicTexture"].SetValue(VisionContent.Load<Texture2D>(@"Textures\windmill_diffuse"));
                    }
        }

        public void Update( GameTime gameTime)
        {
            _animation.Update(gameTime.ElapsedGameTime);
            _model.Meshes["Fan"].ParentBone.Transform =
                Matrix.CreateRotationZ(_animation.Rotation.Z) *
                Matrix.CreateTranslation(_animation.Position);
            _model.CopyAbsoluteBoneTransformsTo(_bones);
        }

        public override void Draw(Camera camera)
        {
            Effect.Parameters["BasicTexture"].SetValue(_texture);
            foreach (var mesh in _model.Meshes)
            {
                _epView.SetValue(camera.View);
                _epProjection.SetValue(camera.Projection);
                _epWorld.SetValue( _bones[mesh.ParentBone.Index] * World);
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
