using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing.Effects;

namespace factor10.VisionThing
{
    public class ClipDrawableInstance : ClipDrawable
    {
        public readonly IDrawable Thing;
        public Matrix World;

        public ClipDrawableInstance(IEffect effect, IDrawable thing, Matrix world )
            : base(effect)
        {
            Thing = thing;
            World = world;
        }

        public override void Draw(Camera camera, IEffect effect)
        {
            camera.UpdateEffect(effect);
            effect.World = World;
            Thing.Draw(effect);
        }

    }

}
