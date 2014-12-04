using System.Drawing;

namespace Exempel1
{
    public class Matrix2
    {
        public float M11;
        public float M12;
        public float M21;
        public float M22;

        public Matrix2()
        {
        }

        public Matrix2(float m11, float m12, float m21, float m22)
        {
            M11 = m11;
            M12 = m12;
            M21 = m21;
            M22 = m22;
        }

        public PointF TransformPoint(PointF p)
        {
            return new PointF(
                M11*p.X + M12*p.Y,
                M21*p.X + M22*p.Y);
        }

        public static Matrix2 operator *(Matrix2 x, Matrix2 y)
        {
            return new Matrix2(
                x.M11*y.M11 + x.M12*y.M21,
                x.M11*y.M21 + x.M12*y.M22,
                x.M21*y.M11 + x.M22*y.M21,
                x.M21*y.M21 + x.M22*y.M22);
        }

    }

}