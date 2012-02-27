using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factor10.VisionThing
{
    public class PPPointLight
    {
        public Vector3 Position { get; set; }
        public Color Color { get; set; }
        public float Attenuation { get; set; }

        public PPPointLight(Vector3 position, Color color, float attenuation)
        {
            Position = position;
            Color = color;
            Attenuation = attenuation;
        }

        public void SetEffectParameters(Effect effect)
        {
            effect.Parameters["LightPosition"].SetValue(Position);
            effect.Parameters["LightAttenuation"].SetValue(Attenuation);
            effect.Parameters["LightColor"].SetValue(Color.ToVector3());
        }
    }
}
