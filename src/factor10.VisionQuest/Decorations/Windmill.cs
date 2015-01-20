using System;
using factor10.VisionThing;
using factor10.VisionThing.CameraStuff;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionQuest
{
    internal class Windmill : VDrawable
    {
        private readonly Model _model;
        private readonly Matrix[] _bones;

        public Matrix World;

        private readonly ObjectAnimation _animation;
        private readonly Texture2D _texture;

        public Windmill(VisionContent vContent, Vector3 location)
            : base(vContent.LoadEffect("effects/SimpleTextureEffect"))
        {
            World = Matrix.RotationY(-MathUtil.PiOverTwo)*Matrix.Scaling(0.005f)*Matrix.Translation(location);
            _model = vContent.Load<Model>("models/windmill");
            _texture = vContent.Load<Texture2D>("textures/windmill_diffuse");
            _bones = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(_bones);

            //foreach (var mesh in _model.Meshes)
            //    foreach (var part in mesh.MeshParts)
            //        part.Effect = Effect.Effect;

            _animation = new ObjectAnimation(new Vector3(0, 875, 0), new Vector3(0, 875, 0),
                Vector3.Zero, new Vector3(0, 0, MathUtil.TwoPi),
                TimeSpan.FromSeconds(10), true);
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
            _animation.Update(gameTime.ElapsedGameTime);
            //TODO _model.Meshes["Fan"].ParentBone.Transform =
            //    Matrix.RotationZ(_animation.Rotation.Z) *
            //    Matrix.Translation(_animation.Position);
            _model.CopyAbsoluteBoneTransformsTo(_bones);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            camera.UpdateEffect(Effect);

            if (drawingReason != DrawingReason.ShadowDepthMap)
                Effect.Texture = _texture;

            foreach (var mesh in _model.Meshes)
            {
                Effect.World = _bones[mesh.ParentBone.Index]*World;
                //Effect.Apply();
                mesh.Draw(Effect.GraphicsDevice, null, Effect.Effect);
            }

            return true;
        }

    }

}
