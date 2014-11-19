using factor10.VisionThing;
using Serpent;
using Serpent.Serpent;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Larv.Serpent
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
            PlayingField pf,
            IVDrawable sphere)
            : base(
                vContent,
                pf,
                sphere,
                pf.PlayerWhereaboutsStart,
                vContent.Load<Texture2D>(@"Textures\sn"),
                vContent.Load<Texture2D>(@"Textures\snakeskinmap"), 
                vContent.Load<Texture2D>(@"Textures\eggshell"))
        {
            //AddTail();
        }

        public void Restart(Whereabouts whereabouts, int length)
        {
            SerpentStatus = SerpentStatus.Alive;
            Restart(whereabouts);
            while(length-->0)
                AddTail();
        }

        protected override float modifySpeed()
        {
            return base.modifySpeed()*Speed;
        }

        public void Update(SerpentCamera serpentCamera, GameTime gameTime)
        {
            UpdateCameraOnly(serpentCamera, gameTime);
            //_turnAround ^= Data.Instance.HasKeyToggled(Keys.Down);
            base.Update(serpentCamera.Camera, gameTime);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            //if (serpentCamera.CameraBehavior != CameraBehavior.Head)
            return base.draw(camera, drawingReason, shadowMap);
        }

        public void UpdateCameraOnly(SerpentCamera serpentCamera, GameTime gameTime)
        {
            serpentCamera.Update(gameTime, LookAtPosition, _headDirection);
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
