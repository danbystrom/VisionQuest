using factor10.VisionThing;
using Serpent.Serpent;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace Serpent
{
    public class PlayerSerpent : BaseSerpent
    {
        public interface ITakeDirection
        {
            RelativeDirection TakeDirection(Direction headDirection);
            bool CanOverrideRestrictedDirections();
        }

        public float Speed = 1.4f;

        public ITakeDirection DirectionTaker;

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
            //AddTail();
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
            //_turnAround ^= Data.Instance.HasKeyToggled(Keys.Down);
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

        protected override void takeDirection()
        {
            if (DirectionTaker != null)
                if (TryMove(_headDirection.Turn(DirectionTaker.TakeDirection(_headDirection)), DirectionTaker.CanOverrideRestrictedDirections()))
                    return;

            if (!TryMove(_headDirection))
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
