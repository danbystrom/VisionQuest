using factor10.VisionThing.Terrain;
using SharpDX;

namespace factor10.VisionThing
{
    public class ColorSurface : Sculptable<Color>
    {

        public ColorSurface(int width, int height)
            : base(width, height)
        {
        }

        public ColorSurface(int width, int height, Color[] surface)
            : base(width,height,surface)
        {
        }

        public Vector3 AsVector3(int x, int y)
        {
            var c = Values[y*Width + x];
            return new Vector3(c.R/255f, c.G/255f, c.B/255f);
        }

    }

}
