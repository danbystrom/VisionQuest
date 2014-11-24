using factor10.VisionThing;
using Serpent;
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
                if (TryMove(HeadDirection.Turn(DirectionTaker.TakeDirection(HeadDirection)), DirectionTaker.CanOverrideRestrictedDirections()))
                    return;

            if (!TryMove(HeadDirection))
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
