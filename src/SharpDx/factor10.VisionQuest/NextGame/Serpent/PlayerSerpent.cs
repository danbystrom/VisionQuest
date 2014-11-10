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
        private bool _turnAround;

        public float Speed = 1.4f;

        public PlayerSerpent(
            VisionContent vContent,
            MouseManager mouseManager,
            KeyboardManager keyboardManager,
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
                vContent.ClientSize,
                new Vector3(0, 20, 2),
                Vector3.Zero,
                CameraBehavior.FollowTarget);
            addTail();
        }

        public void Restart()
        {
            Restart(_pf.PlayerWhereaboutsStart);
            addTail();
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
            if(Camera.CameraBehavior!=CameraBehavior.Head)
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
            var nextDirection = _turnAround ? RelativeDirection.Backward : RelativeDirection.Forward;
            _turnAround = false;
            if ( Data.Instance.KeyboardState.IsKeyDown(Keys.Left))
                nextDirection = RelativeDirection.Left;
            else if (Data.Instance.KeyboardState.IsKeyDown(Keys.Right))
                nextDirection = RelativeDirection.Right;
            if (!tryMove(_headDirection.Turn(nextDirection)))
                if (!tryMove(_whereabouts.Direction))
                    _whereabouts.Direction = Direction.None;
        }

        protected override Vector4 tintColor()
        {
            return new Vector4(1, 1, 1, 1);
        }

    }

}
