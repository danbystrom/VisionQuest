using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing;
using factor10.VisionThing.Primitives;

namespace TestBed
{
    public class Pillar : ClipDrawable
    {
        private readonly CylinderPrimitive _cylinder;
        private readonly Matrix _world;

        public Pillar( Effect effect, Matrix world )
            : base(effect)
        {
            _cylinder = new CylinderPrimitive(effect.GraphicsDevice,10,2,10);
            _world = world;
        }

        protected override void Draw()
        {
            _epWorld.SetValue(_world);
            _cylinder.Draw(Effect);
        }

    }

}
