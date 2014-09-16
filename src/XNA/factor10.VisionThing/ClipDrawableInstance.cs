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

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            camera.UpdateEffect(Effect);
            Effect.World = World;
            Thing.Draw(Effect);
            return true;
        }
    }

}
