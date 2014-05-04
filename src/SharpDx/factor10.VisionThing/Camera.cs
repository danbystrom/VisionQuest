using System;
using factor10.VisionThing.Effects;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;

namespace factor10.VisionThing
{

    public class Camera
    {
        public Matrix View { get; protected set; }
        public Matrix Projection { get; set; }
        public Vector3 Position { get; protected set; }
        public Vector3 Target { get; protected set; }

        public Vector3 UpVector = Vector3.Up;

        public float Yaw;
        public float Pitch;

        public readonly Vector2 ClientSize;

        public GameTime GameTime;

        public Camera(
            Vector2 clientSize,
            Vector3 position,
            Vector3 target,
            float nearPlane = 1,
            float farPlane = 20000)
        {
            ClientSize = clientSize;

            Update(position, target);
            Projection = Matrix.PerspectiveFovLH(
                MathUtil.PiOverFour,
                clientSize.X / clientSize.Y,
                nearPlane, farPlane);
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

            View = Matrix.LookAtLH(
                Position,
                Target,
                UpVector);
            _boundingFrustum = null;
        }

        private BoundingFrustum? _boundingFrustum;

        public BoundingFrustum BoundingFrustum
        {
            get { return (_boundingFrustum ?? (_boundingFrustum = new BoundingFrustum(View*Projection))).Value; }
        }

        public void UpdateFreeFlyingCamera(GameTime gameTime, MouseManager mouseManager)
        {
            GameTime = gameTime;

            var center = new Vector2(ClientSize.X / 2, ClientSize.Y / 2);
            var mouse = mouseManager.GetState();
            mouseManager.SetPosition(center);
            var delta = center - new Vector2(mouse.X, mouse.Y);

            var forward = Vector3.Normalize(new Vector3((float) Math.Sin(-Yaw), (float) Math.Sin(Pitch), (float) Math.Cos(-Yaw)));
            var left = Vector3.Normalize(new Vector3((float) Math.Cos(Yaw), 0f, (float) Math.Sin(Yaw)));

            if (mouse.Right == ButtonState.Released)
            {
                Yaw += MathUtil.DegreesToRadians(delta.X*0.50f);
                Pitch += MathUtil.DegreesToRadians(delta.Y*0.50f);
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

            var rotation = Matrix.RotationYawPitchRoll(-Yaw, -Pitch, 0);
            var z = Vector3.ForwardLH;
            var r = rotation;
            Target = Position + Vector3.TransformCoordinate(Vector3.ForwardLH, rotation);
            View = Matrix.LookAtLH(Position, Target, Vector3.Up);
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