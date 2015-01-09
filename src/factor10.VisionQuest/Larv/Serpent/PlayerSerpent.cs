using Larv.Field;
using Larv.Util;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace Larv.Serpent
{
    public class PlayerSerpent : BaseSerpent
    {
        public float Speed = 1.4f;

        public PlayerSerpent(
            LarvContent lcontent,
            PlayingField playingField)
            : base(
                lcontent,
                playingField,
                lcontent.Load<Texture2D>(@"Textures\snakeskin"),
                lcontent.Load<Texture2D>(@"Textures\snakeskinhead"),
                lcontent.Load<Texture2D>(@"Textures\snakeskinmap"), 
                lcontent.Load<Texture2D>(@"Textures\eggshell"))
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

        protected override float ModifySpeed()
        {
            return base.ModifySpeed()*Speed;
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
