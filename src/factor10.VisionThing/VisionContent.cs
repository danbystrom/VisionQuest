using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace factor10.VisionThing
{
    public class VisionContent
    {
        public readonly ContentManager LibContent;

        public VisionContent(Game game, string contentdirectory)
        {
            LibContent = new ContentManager(game.Services, contentdirectory);
        }

    }

}


