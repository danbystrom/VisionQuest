using System.Linq;
using factor10.VisionThing;
using NextGame;
using Serpent.Serpent;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace Serpent
{
    public class PlayerSerpent : BaseSerpent
    {
        private bool _turnAround;

        public float Speed = 1.4f;

        public PlayerSerpent(
            VisionContent vContent,
            MouseManager mouseManager,
            KeyboardManager keyboardManager,
            PointerManager pointerManager,
            PlayingField pf,
            IVDrawable sphere)
            : base(
                vContent,
                pf,
                sphere,
                pf.PlayerWhereaboutsStart,
                vContent.Load<Texture2D>(@"Textures\sn"),
                vContent.Load<Texture2D>(@"Textures\eggshell"))
        {
            _camera = new SerpentCamera(
                mouseManager,
                keyboardManager,
                pointerManager,
                vContent.ClientSize,
                new Vector3(0, 20, 2),
                Vector3.Zero,
                CameraBehavior.FollowTarget);
            AddTail();
        }

        public void Restart(Whereabouts whereabouts, int length)
        {
            SerpentStatus = SerpentStatus.Alive;
            Restart(whereabouts);
            while(length-->0)
                AddTail();
        }

        public SerpentCamera Camera
        {
            get { return _camera; }
        }

        protected override float modifySpeed()
        {
            return base.modifySpeed()*Speed;
        }

        public override void Update(GameTime gameTime)
        {
            UpdateCameraOnly(gameTime);
            _turnAround ^= Data.Instance.HasKeyToggled(Keys.Down);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Camera.CameraBehavior != CameraBehavior.Head)
                base.Draw(gameTime);
        }

        public void UpdateCameraOnly(GameTime gameTime)
        {
            _camera.Update(gameTime, LookAtPosition, _headDirection);
        }

        public Vector3 LookAtPosition
        {
            get
            {
                var d = _whereabouts.Direction.DirectionAsPoint();
                return new Vector3(
                    _whereabouts.Location.X + d.X*(float) _fractionAngle,
                    _pf.GetElevation(_whereabouts),
                    _whereabouts.Location.Y + d.Y*(float) _fractionAngle);
            }
        }

        private bool _isHoldingBothPointers;

        protected override void takeDirection()
        {
            if (HomingDevice())
                return;

            if (_camera.Camera.KeyboardState.IsKeyPressed(Keys.O))
                PathFinder = new PathFinder(_pf, _pf.PlayerWhereaboutsStart);

            var pointerLeft = _camera.Camera.PointerState.Points.Any(_ => _.Position.X < 0.1f);
            var pointerRight = _camera.Camera.PointerState.Points.Any(_ => _.Position.X > 0.5f);
            if (pointerLeft && pointerRight)
            {
                pointerLeft = false;
                pointerRight = false;
                _turnAround = !_isHoldingBothPointers;
                _isHoldingBothPointers = true;
            }
            else
                _isHoldingBothPointers = false;

            var nextDirection = _turnAround ? RelativeDirection.Backward : RelativeDirection.Forward;
            _turnAround = false;
            if (_camera.Camera.KeyboardState.IsKeyDown(Keys.Left) || pointerLeft)
                nextDirection = RelativeDirection.Left;
            else if (Camera.Camera.KeyboardState.IsKeyDown(Keys.Right) || pointerRight)
                nextDirection = RelativeDirection.Right;
            if (!TryMove(_headDirection.Turn(nextDirection)))
                if (!TryMove(_whereabouts.Direction))
                    _whereabouts.Direction = Direction.None;
        }

        protected override Vector4 TintColor()
        {
            if (SerpentStatus == SerpentStatus.Ghost)
                return new Vector4(1.2f, 1.2f, 0.5f, AlphaValue());
            return Vector4.One;
        }

    }

}
