using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing.Effects;

namespace factor10.VisionThing
{
    public static class VisionContent
    {
        public static int RenderedTriangles;
        private static Vector3 _sunlightDirectionWater;
        private static Vector3 _sunlightDirectionShadows;

        public static ContentManager Content { get; private set; }

        static VisionContent()
        {
            SunlightDirectionWater = new Vector3(11f, -3f, -6f);
            SunlightDirectionShadows = new Vector3(11f, -7f, -6f);
        }

        public static Vector3 SunlightDirectionWater
        {
            get
            {
                return _sunlightDirectionWater;
            }
            set
            {
                _sunlightDirectionWater = value;
                _sunlightDirectionWater.Normalize();
            }
        }

        public static Vector3 SunlightDirectionShadows
        {
            get
            {
                return _sunlightDirectionShadows;
            }
            set
            {
                _sunlightDirectionShadows = value;
                _sunlightDirectionShadows.Normalize();
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
            return new PlainEffectWrapper( Content.Load<Effect>(name) );
        }

    }

}


