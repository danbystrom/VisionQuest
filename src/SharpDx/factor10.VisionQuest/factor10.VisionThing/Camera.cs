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
        private Vector2 _lastPointerPosition;

        private readonly List<Keys> _downKeys = new List<Keys>();

        public readonly KeyboardManager KeyboardManager;
        public readonly MouseManager MouseManager;
        public readonly PointerManager PointerManager;

        public KeyboardState KeyboardState;
        public MouseState MouseState;
        public PointerState PointerState;

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

        public Camera(
            Vector2 clientSize,
            KeyboardManager keyboardManager,
            MouseManager mouseManager,
            PointerManager pointerManager,
            Vector3 position,
            Vector3 target,
            float nearPlane = 1,
            float farPlane = 20000)
            :this(clientSize,position,target,nearPlane,farPlane)
        {
            KeyboardManager = keyboardManager;
            MouseManager = mouseManager;
            PointerManager = pointerManager;
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
        private int _lastMouseDelta;

        public BoundingFrustum BoundingFrustum
        {
            get { return (_boundingFrustum ?? (_boundingFrustum = new BoundingFrustum(View*Projection))).Value; }
        }

        public void UpdateInputDevices()
        {
            KeyboardState = KeyboardManager.GetState();
            KeyboardState.GetDownKeys(_downKeys);
            MouseState = MouseManager.GetState();
            if(PointerManager!=null)
                PointerState = PointerManager.GetState();
        }

        public void UpdateFreeFlyingCamera(GameTime gameTime)
        {
            var mouseWheelChanged = MouseState.WheelDelta != _lastMouseDelta;

            if (!MouseState.LeftButton.Down && !MouseState.RightButton.Down && !mouseWheelChanged && !_downKeys.Any())
                return;

            var mousePos = new Vector2(MouseState.X, MouseState.Y);
            if (MouseState.LeftButton.Pressed || MouseState.RightButton.Pressed)
                _lastMousePosition = mousePos;

            var delta = (_lastMousePosition - mousePos)*150;
            if (mouseWheelChanged)
            {
                delta.Y += (MouseState.WheelDelta - _lastMouseDelta)*1.0f;
                _lastMouseDelta = MouseState.WheelDelta;
            }

            var pos = Position;

            //if (PointerState.Points.Any())
            //    foreach(var point in PointerState.Points)
            //        switch (point.EventType)
            //        {
            //            case PointerEventType.Pressed:
            //                _lastPointerPosition = point.Position;
            //                break;
            //            case PointerEventType.Moved:
            //                delta += _lastMousePosition - point.Position;
            //                Yaw += MathUtil.DegreesToRadians(delta.X*0.50f);
            //                Pitch += MathUtil.DegreesToRadians(delta.Y*0.50f);
            //                _lastPointerPosition = point.Position;
            //                break;
            //        }
            //else
                if (MouseState.LeftButton.Down)
            {
                Yaw += MathUtil.DegreesToRadians(delta.X*0.50f);
                Pitch += MathUtil.DegreesToRadians(delta.Y*0.50f);
                MouseManager.SetPosition(_lastMousePosition);
            }
            else if (MouseState.RightButton.Down || mouseWheelChanged)
            {
                pos -= Forward*delta.Y*0.1f;
                pos += Left*delta.X*0.1f;
                MouseManager.SetPosition(_lastMousePosition);
            }

            var step = (float) gameTime.ElapsedGameTime.TotalSeconds*30;

            var rotStep = step*0.05f;
            if (_downKeys.Contains(Keys.Shift))
                step *= 5;

            if (_downKeys.Contains(Keys.R))
                pos.Y += step;
            if (KeyboardState.IsKeyDown(Keys.F))
                pos.Y -= step;
            if (KeyboardState.IsKeyDown(Keys.A))
                pos += Left*step;
            if (KeyboardState.IsKeyDown(Keys.D))
                pos -= Left*step;
            if (KeyboardState.IsKeyDown(Keys.W))
                pos += Forward*step;
            if (KeyboardState.IsKeyDown(Keys.S))
                pos -= Forward*step;
            if (KeyboardState.IsKeyDown(Keys.Left))
                Yaw += rotStep;
            if (KeyboardState.IsKeyDown(Keys.Right))
                Yaw -= rotStep;
            if (KeyboardState.IsKeyDown(Keys.Up))
                Pitch += rotStep;
            if (KeyboardState.IsKeyDown(Keys.Down))
                Pitch -= rotStep;

            var rotation = Matrix.RotationYawPitchRoll(Yaw, Pitch, 0);
            Update(
                pos,
                pos + Vector3.TransformCoordinate(Vector3.ForwardRH*10, rotation));

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
