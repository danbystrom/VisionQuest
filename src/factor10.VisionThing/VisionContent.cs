using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace factor10.VisionThing
{
    public static class VisionContent
    {
        public static ContentManager Content { get; private set; }

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

    }

}


