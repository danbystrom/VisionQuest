using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using factor10.VisionThing.Effects;

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

        public readonly Vector2 ClientSize;
 
        public Camera(
            Vector2 clientSize,
            Vector3 position,
            Vector3 target)
        {
            ClientSize = clientSize;

            Update(position, target);
            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                clientSize.X / clientSize.Y,
                1, 10000);
        }

        public Camera(
            Rectangle clientBounds,
            Vector3 position,
            Vector3 target)
                :this(new Vector2(clientBounds.Width,clientBounds.Height) , position, target )
        {
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
            _boundingFrustum = null;
        }

        private BoundingFrustum _boundingFrustum;

        public BoundingFrustum BoundingFrustum
        {
            get { return _boundingFrustum ?? (_boundingFrustum = new BoundingFrustum(View*Projection)); }
        }

        public Vector2 HandleMouse(int mouseX, int mouseY )
        {
            var centerX = ClientSize.X / 2;
            var centerY = ClientSize.Y / 2;

            Mouse.SetPosition((int)centerX, (int)centerY);

            return new Vector2(mouseX - centerX, mouseY - centerY);
        }

        public void UpdateFreeFlyingCamera(GameTime gameTime)
        {
            var mouse = Mouse.GetState();
            var delta = HandleMouse(mouse.X, mouse.Y);
 
            var forward = Vector3.Normalize(new Vector3((float) Math.Sin(-Yaw), (float) Math.Sin(Pitch), (float) Math.Cos(-Yaw)));
            var left = Vector3.Normalize(new Vector3((float) Math.Cos(Yaw), 0f, (float) Math.Sin(Yaw)));

            if (mouse.RightButton == ButtonState.Released)
            {
                Yaw += MathHelper.ToRadians(delta.X*0.50f);
                Pitch += MathHelper.ToRadians(delta.Y*0.50f);
                //if (Yaw < 0 || Yaw > MathHelper.TwoPi)
                //    Yaw -= MathHelper.TwoPi*Math.Sign(Yaw);
               // if (Pitch < 0 || Pitch > MathHelper.TwoPi)
               //     Pitch -= MathHelper.TwoPi*Math.Sign(Pitch);
            }
            else
            {
                Position += forward*delta.Y*0.1f;
                Position += left*delta.X*0.1f;

                //if (Data.Instance.KeyboardState.IsKeyDown(Keys.PageUp))
                //    Position += Vector3.Down*delta;

                //if (Data.Instance.KeyboardState.IsKeyDown(Keys.PageDown))
                //    Position += Vector3.Up*delta;
            }

            var rotation = Matrix.CreateFromYawPitchRoll(-Yaw, -Pitch, 0);
            Target = Position + Vector3.Transform(Vector3.Forward, rotation);
            View = Matrix.CreateLookAt(Position, Target, Vector3.Up);
            _boundingFrustum = null;
        }

        public void UpdateEffect(IEffect effect)
        {
            effect.View = View;
            effect.Projection = Projection;
            effect.CameraPosition = Position;
        }

    }

}
