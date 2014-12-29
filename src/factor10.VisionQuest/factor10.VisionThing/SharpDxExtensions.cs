using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SharpDX;
using SharpDX.Toolkit.Input;

namespace factor10.VisionThing
{
    public static class SharpDxExtensions
    {
        [DllImport("user32.dll")]
        public static extern int ToUnicode(uint virtualKeyCode, uint scanCode,
            byte[] keyboardState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)] StringBuilder receivingBuffer,
            int bufferSize, uint flags);

        public static string GetCharsFromKeys(this KeyboardState kbd)
        {
            var buf = new StringBuilder(256);
            var keyboardState = new byte[256];

            var keys = new List<Keys>();
            kbd.GetDownKeys(keys);
            foreach (var key in keys)
                keyboardState[(int)key] = 0xff;
            foreach (var key in keys.Where(kbd.IsKeyPressed))
                ToUnicode((uint)key, 0, keyboardState, buf, 256, 0);
            return buf.ToString();
        }

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
