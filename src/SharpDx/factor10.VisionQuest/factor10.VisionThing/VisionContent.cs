using factor10.VisionThing.Effects;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionThing
{
    public class VisionContent
    {
        public static int RenderedTriangles;

        private static Vector3 _sunlightDirectionReflectedWater;
        private static Vector3 _sunlightDirection;

        public readonly ContentManager Content;
        public readonly GraphicsDevice GraphicsDevice;

        public VisionContent()
        {
            SunlightDirectionReflectedWater = new Vector3(11f, -2f, -6f);
            SunlightDirection = new Vector3(11f, -7f, -6f);
        }

        public VisionContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            GraphicsDevice = graphicsDevice;
            Content = content;
        }

        public static Vector3 SunlightDirectionReflectedWater
        {
            get
            {
                return _sunlightDirectionReflectedWater;
            }
            set
            {
                _sunlightDirectionReflectedWater = value;
                _sunlightDirectionReflectedWater.Normalize();
            }
        }

        public static Vector3 SunlightDirection
        {
            get
            {
                return _sunlightDirection;
            }
            set
            {
                _sunlightDirection = value;
                _sunlightDirection.Normalize();
            }
        }

        public Vector2 ClientSize { get { return new Vector2(GraphicsDevice.BackBuffer.Width, GraphicsDevice.BackBuffer.Height); }}

        public T Load<T>( string name)
        {
            return Content.Load<T>(name);
        }

        public VisionEffect LoadPlainEffect(string name)
        {
            return new VisionEffect( Content.Load<Effect>(name) );
        }

    }

}
