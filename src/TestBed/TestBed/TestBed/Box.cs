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
    public class Box : ClipDrawable
    {
        private readonly CubePrimitive _box;
        private readonly Matrix _world;

        public Box( Effect effect, Matrix world )
            : base(effect)
        {
            _box = new CubePrimitive(effect.GraphicsDevice, 1);
            _world = world;
        }

        protected override void Draw()
        {
            _epWorld.SetValue(_world);
            _box.Draw(Effect);
        }

    }

}
