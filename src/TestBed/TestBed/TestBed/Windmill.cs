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

        public Matrix World;

        private readonly ObjectAnimation _animation;
        private readonly Texture2D _texture;
        private readonly Texture2D _bumpMap;

        public Windmill( Vector3 location )
            : base(VisionContent.LoadPlainEffect("effects/SimpleBumpEffect"))
        {
            World = Matrix.CreateRotationY(-MathHelper.PiOver2) * Matrix.CreateScale(0.005f) * Matrix.CreateTranslation(location);
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

        public override void Update(GameTime gameTime)
        {
            _animation.Update(gameTime.ElapsedGameTime);
            _model.Meshes["Fan"].ParentBone.Transform =
                Matrix.CreateRotationZ(_animation.Rotation.Z) *
                Matrix.CreateTranslation(_animation.Position);
            _model.CopyAbsoluteBoneTransformsTo(_bones);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            camera.UpdateEffect(Effect);

            if (drawingReason != DrawingReason.ShadowDepthMap)
            {
                Effect.Texture = _texture;
                Effect.Parameters["BumpMap"].SetValue(_bumpMap);
            }

            foreach (var mesh in _model.Meshes)
            {
                Effect.World =
                    _bones[mesh.ParentBone.Index] *
                    World;
                Effect.Apply();
                 mesh.Draw();
            }

            return true;
        }
        
    }

}
