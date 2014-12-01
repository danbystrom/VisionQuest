using SharpDX;

namespace Exempel4
{
    public class Line
    {
        public Vector3 P1;
        public Vector3 P2;

        public Line(Vector3 p1, Vector3 p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public Line(float x1, float y1, float z1, float x2, float y2, float z2)
            : this(new Vector3(x1, y1, z1), new Vector3(x2, y2, z2))
        {
        }

    }

}
