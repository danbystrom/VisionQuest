using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing.Primitives;

namespace factor10.VisionThing.Objects
{
    public class Bridge : ClipDrawable
    {
        public Matrix World;

        private readonly Model _model;
        private readonly Texture2D _texture;

        public Bridge(Matrix world)
            : base(VisionContent.LoadPlainEffect("effects/SimpleTextureEffect"))
        {
            _model = VisionContent.Load<Model>(@"Models/bridge");
            _texture = VisionContent.Load<Texture2D>("textures/bigstone");
            World = world * Matrix.CreateScale(0.05f);

            foreach (var part in _model.Meshes.SelectMany(mesh => mesh.MeshParts))
                part.Effect = Effect.Effect;

        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            camera.UpdateEffect(Effect);
            Effect.World = World;

            if (drawingReason != DrawingReason.ShadowDepthMap)
                Effect.Texture = _texture;

            Effect.Apply();
            foreach (var mesh in _model.Meshes)
                mesh.Draw();

            Effect.World = World*Matrix.CreateTranslation(21.68f, 0, 0);
            foreach (var mesh in _model.Meshes)
                mesh.Draw();

            return true;
        }

    }

}