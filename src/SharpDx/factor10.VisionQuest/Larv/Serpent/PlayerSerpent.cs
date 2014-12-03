using factor10.VisionThing;
using Serpent;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace Larv.Serpent
{
    public class PlayerSerpent : BaseSerpent
    {
        public float Speed = 1.4f;

        public PlayerSerpent(
            VisionContent vContent,
            PlayingField playingField,
            IVDrawable sphere)
            : base(
                vContent,
                playingField,
                sphere,
                vContent.Load<Texture2D>(@"Textures\snakeskin"),
                vContent.Load<Texture2D>(@"Textures\snakeskinhead"),
                vContent.Load<Texture2D>(@"Textures\snakeskinmap"), 
                vContent.Load<Texture2D>(@"Textures\eggshell"))
        {
            Restart(playingField, 1);
        }

        public void Restart(PlayingField playingField, int length, Whereabouts? whereabouts = null)
        {
            Restart(playingField, whereabouts.GetValueOrDefault(PlayingField.PlayerWhereaboutsStart));
            DirectionTaker = null;
            SerpentStatus = SerpentStatus.Alive;
            while (length-- > 0)
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
                    PlayingField.GetElevation(_whereabouts),
                    _whereabouts.Location.Y + d.Y*(float) _fractionAngle);
            }
        }

        protected override void takeDirection()
        {
            if (TakeDirection())
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
