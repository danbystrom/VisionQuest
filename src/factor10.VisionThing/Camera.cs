using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace factor10.VisionThing
{

    public class Camera
    {
        public Matrix View { get; protected set; }
        public Matrix Projection { get; protected set; }
        public Vector3 Position { get; protected set; }
        public Vector3 Target { get; protected set; }

        public Vector3 UpVector = Vector3.Up;

        public float Yaw;
        public float Pitch;

        public readonly Rectangle ClientBounds;
        public readonly BoundingFrustum BoundingFrustum;

        public Camera(
            Rectangle clientBounds,
            Vector3 position,
            Vector3 target)
        {
            ClientBounds = clientBounds;

            Update(position, target);
            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                clientBounds.Width/(float) clientBounds.Height,
                1, 1000);
            BoundingFrustum = new BoundingFrustum(Projection);
        }

        public Vector3 Front
        {
            get
            {
                var front = Target - Position;
                front.Normalize();
                return front;
            }
        }

        public void Update(
            Vector3 position,
            Vector3 target)
        {
            Position = position;
            Target = target;

            View = Matrix.CreateLookAt(
                Position,
                Target,
                UpVector);
        }

        public void UpdateFreeFlyingCamera(GameTime gameTime)
        {
            var centerX = ClientBounds.Width/2;
            var centerY = ClientBounds.Height/2;

            var mouse = Mouse.GetState();
            Mouse.SetPosition(centerX, centerY);

            var deltaX = mouse.X - centerX;
            var deltaY = mouse.Y - centerY;

            var forward = Vector3.Normalize(new Vector3((float) Math.Sin(-Yaw), (float) Math.Sin(Pitch), (float) Math.Cos(-Yaw)));
            var left = Vector3.Normalize(new Vector3((float) Math.Cos(Yaw), 0f, (float) Math.Sin(Yaw)));

            if (mouse.MiddleButton == ButtonState.Released)
            {
                Yaw += MathHelper.ToRadians(deltaX*0.50f);
                Pitch += MathHelper.ToRadians(deltaY*0.50f);
                //if (Yaw < 0 || Yaw > MathHelper.TwoPi)
                //    Yaw -= MathHelper.TwoPi*Math.Sign(Yaw);
               // if (Pitch < 0 || Pitch > MathHelper.TwoPi)
               //     Pitch -= MathHelper.TwoPi*Math.Sign(Pitch);
            }
            else
            {
                Position += forward*deltaY*0.1f;
                Position += left*deltaX*0.1f;

                //if (Data.Instance.KeyboardState.IsKeyDown(Keys.PageUp))
                //    Position += Vector3.Down*delta;

                //if (Data.Instance.KeyboardState.IsKeyDown(Keys.PageDown))
                //    Position += Vector3.Up*delta;
            }

            var rotation = Matrix.CreateFromYawPitchRoll(-Yaw, -Pitch, 0);
            Target = Position + Vector3.Transform(Vector3.Forward, rotation);
            View = Matrix.CreateLookAt(Position, Target, Vector3.Up);
        }

    }

}
