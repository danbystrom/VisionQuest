using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factor10.VisionThing
{
    public class ClipDrawableInstance : ClipDrawable
    {
        public readonly IDrawable Thing;
        public Matrix World;

        public ClipDrawableInstance(Effect effect, IDrawable thing, Matrix world )
            : base(effect)
        {
            Thing = thing;
            World = world;
        }

        protected override void Draw()
        {
            _epWorld.SetValue(World);
            Thing.Draw(Effect);
        }

        protected override void Draw(Effect effect)
        {
            Thing.Draw(effect);
        }

    }

}
