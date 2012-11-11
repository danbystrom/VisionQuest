using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing;
using factor10.VisionThing.Effects;

namespace TestBed
{
    class Windmill : ClipDrawable
    {
        private readonly Model _model;
        private readonly Matrix[] _bones;

        public Matrix World = Matrix.CreateRotationY(-MathHelper.PiOver2) * Matrix.CreateScale(0.006f) * Matrix.CreateTranslation(-2, 1.3f, 7);

        private readonly ObjectAnimation _animation;
        private readonly Texture2D _texture;
        private readonly Texture2D _bumpMap;

        public Windmill()
            : base(VisionContent.LoadPlainEffect("effects/lightingeffectbump"))
        {
            _model = VisionContent.Load<Model>("models/windmill");
            _texture = VisionContent.Load<Texture2D>("textures/windmill_diffuse");
            _bumpMap = VisionContent.Load<Texture2D>("textures/windmill_normal");
            _bones = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(_bones);

            foreach (var mesh in _model.Meshes)
                foreach (var part in mesh.MeshParts)
                    part.Effect = Effect.Effect;

            _animation = new ObjectAnimation(new Vector3(0, 875, 0), new Vector3(0, 875, 0),
                Vector3.Zero, new Vector3(0, 0, MathHelper.TwoPi),
                TimeSpan.FromSeconds(10), true);
        }

        public void Update( GameTime gameTime)
        {
            _animation.Update(gameTime.ElapsedGameTime);
            _model.Meshes["Fan"].ParentBone.Transform =
                Matrix.CreateRotationZ(_animation.Rotation.Z) *
                Matrix.CreateTranslation(_animation.Position);
            _model.CopyAbsoluteBoneTransformsTo(_bones);
        }

        public override void Draw(Camera camera, IEffect effect)
        {
            camera.UpdateEffect(effect);
            effect.Parameters["Texture"].SetValue(_texture);
            effect.Parameters["BumpMap"].SetValue(_bumpMap);

            foreach (var mesh in _model.Meshes)
            {
                effect.World =
                    _bones[mesh.ParentBone.Index] *
                    World;
                effect.Apply();
                mesh.Draw();
            }
        }
        
        /*

        public override void Draw(Camera camera, IEffect effect)
        {
            foreach (var pass in effect.Effect.CurrentTechnique.Passes)
            {
                foreach (var mesh in _model.Meshes)
                    foreach (var part in mesh.MeshParts)
                    {
                        camera.UpdateEffect(effect);
                        effect.World =
                            _bones[mesh.ParentBone.Index] *
                            World;
                        effect.Parameters["Texture"].SetValue(_texture);
                        effect.Parameters["BumpMap"].SetValue(_bumpMap);

                        pass.Apply();
                        effect.GraphicsDevice.SetVertexBuffer(part.VertexBuffer);
                        effect.GraphicsDevice.Indices = part.IndexBuffer;
                        effect.GraphicsDevice.DrawIndexedPrimitives(
                            PrimitiveType.TriangleList,
                            0, 0, part.NumVertices,
                            part.StartIndex, part.PrimitiveCount);
                    }
            }

        }
        */
    }

}
