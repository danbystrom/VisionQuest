using SharpDX;

namespace factor10.VisionThing
{
    public static class SharpDxExtensions
    {
        public static Matrix AlignObjectToNormal(this Vector3 normal, float angle)
        {
            var rotation = Matrix.RotationY(angle);
            rotation.Up = normal;
            rotation.Right = Vector3.Normalize(Vector3.Cross(rotation.Forward, rotation.Up));
            rotation.Forward = Vector3.Normalize(Vector3.Cross(rotation.Up, rotation.Right));
            return rotation;
        }

    }

}
