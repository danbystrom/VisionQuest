using System;
using System.Collections.Generic;
using System.Linq;
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
        public Vector3 Forward { get; protected set; }
        public Vector3 Left { get; protected set; }
        public Vector3 Up { get; set; }

        public float Yaw { get; protected set; }
        public float Pitch { get; protected set; }

        public readonly Vector2 ClientSize;

        private Vector2 _lastMousePosition;

        private readonly List<Keys> _downKeys = new List<Keys>();
 
        public Camera(
            Vector2 clientSize,
            Vector3 position,
            Vector3 target,
            float nearPlane = 1,
            float farPlane = 20000)
        {
            ClientSize = clientSize;

            Up = Vector3.Up;
            Update(position, target);
            Projection = Matrix.PerspectiveFovRH(
                MathUtil.PiOverFour,
                clientSize.X / clientSize.Y,
                nearPlane, farPlane);
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

            View = Matrix.LookAtRH(
                Position,
                Target,
                Vector3.Up);
            Yaw = (float)Math.Atan2(position.X - target.X, position.Z - target.Z);
            Pitch = -(float)Math.Asin((position.Y - target.Y) / Vector3.Distance(position, target));
            Forward = Vector3.Normalize(target - position);
            Left = Vector3.Normalize(Vector3.Cross(Up, Forward));

            _boundingFrustum = null;
        }

        private BoundingFrustum? _boundingFrustum;

        public BoundingFrustum BoundingFrustum
        {
            get { return (_boundingFrustum ?? (_boundingFrustum = new BoundingFrustum(View*Projection))).Value; }
        }

        public void UpdateFreeFlyingCamera(GameTime gameTime, MouseManager mouseManager, MouseState mouseState, KeyboardState keyboardState)
        {
            var mousePos = new Vector2(mouseState.X, mouseState.Y);
            if (mouseState.LeftButton.Pressed || mouseState.RightButton.Pressed)
                _lastMousePosition = mousePos;
            
            var delta = (_lastMousePosition - mousePos) * 100;

            var step = (float) gameTime.ElapsedGameTime.TotalSeconds*4;
            var pos = Position;

            if (mouseState.LeftButton.Down)
            {
                Yaw += MathUtil.DegreesToRadians(delta.X*0.50f);
                Pitch += MathUtil.DegreesToRadians(delta.Y*0.50f);
                mouseManager.SetPosition(_lastMousePosition);
            }
            else if (mouseState.RightButton.Down)
            {
                pos -= Forward*delta.Y*0.1f;
                pos += Left*delta.X*0.1f;
                mouseManager.SetPosition(_lastMousePosition);
            }
            else
            {
                keyboardState.GetDownKeys(_downKeys);
                if (!_downKeys.Any())
                    return;
                if (_downKeys.Contains(Keys.Shift))
                    step *= 3;
                if (_downKeys.Contains(Keys.R))
                    pos.Y += step;
                if (keyboardState.IsKeyDown(Keys.F))
                    pos.Y -= step;
                if (keyboardState.IsKeyDown(Keys.A))
                    pos += Left*step;
                if (keyboardState.IsKeyDown(Keys.D))
                    pos -= Left*step;
                if (keyboardState.IsKeyDown(Keys.W))
                    pos += Forward*step;
                if (keyboardState.IsKeyDown(Keys.S))
                    pos -= Forward*step;
                if (keyboardState.IsKeyDown(Keys.Left))
                    Yaw += step*0.1f;
                if (keyboardState.IsKeyDown(Keys.Right))
                    Yaw -= step*0.1f;
                if (keyboardState.IsKeyDown(Keys.Up))
                    Pitch += step*0.1f;
                if (keyboardState.IsKeyDown(Keys.Down))
                    Pitch -= step*0.1f;
            }
            var rotation = Matrix.RotationYawPitchRoll(Yaw, Pitch, 0);
            Update(
                pos,
                pos + Vector3.TransformCoordinate(Vector3.ForwardRH * 10, rotation));

            //System.Diagnostics.Debug.Print("({0:0.0},{1:0.0},{2:0.0}) ({3:0.0},{4:0.0},{5:0.0}) ({6:0.0},{7:0.0},{8:0.0}) ({9:0.0},{10:0.0},{11:0.0})", Position.X, Position.Y, Position.Z, Target.X, Target.Y, Target.Z,
            //    Forward.X, Forward.Y, Forward.Z, Left.X, Left.Y, Left.Z);
        }

        public void UpdateEffect(IEffect effect)
        {
            effect.View = View;
            effect.Projection = Projection;
            effect.CameraPosition = Position;
        }

    }

}
