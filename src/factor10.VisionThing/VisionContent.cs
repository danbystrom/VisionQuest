using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing.Effects;

namespace factor10.VisionThing
{
    public static class VisionContent
    {
        public static int RenderedTriangles;
        private static Vector3 _sunlightDirectionReflectedWater;
        private static Vector3 _sunlightDirection;

        public static ContentManager Content { get; private set; }

        static VisionContent()
        {
            SunlightDirectionReflectedWater = new Vector3(11f, -2f, -6f);
            SunlightDirection = new Vector3(11f, -7f, -6f);
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

        public static void Init(Game game, string contentdirectory)
        {
            Content = new ContentManager(game.Services, contentdirectory);
        }

        public static void Init(Game game)
        {
            Content = new ContentManager(game.Services, "Content");
        }

        public static T Load<T>( string name)
        {
            return Content.Load<T>(name);
        }

        public static PlainEffectWrapper LoadPlainEffect(string name)
        {
            return new PlainEffectWrapper( Content.Load<Effect>(name), name );
        }

    }

}


