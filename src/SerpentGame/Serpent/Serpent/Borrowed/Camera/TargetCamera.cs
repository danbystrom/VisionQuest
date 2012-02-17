using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Serpent.Borrowed.Camera
{
    public class TargetCamera : Camera
    {
        public Vector3 Position { get; set; }
        public Vector3 Target { get; set; }

        public TargetCamera(Vector3 position, Vector3 target,
            GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            this.Position = position;
            this.Target = target;
        }

        public override void Update()
        {
            this.View = Matrix.CreateLookAt(Position, Target, Vector3.Up);
        }

    }
}
