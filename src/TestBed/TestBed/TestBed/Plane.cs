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
    public class Plane : ClipDrawable
    {
        private readonly PlanePrimitive<VertexPositionNormal> _plane;
        private readonly Matrix _world;

        public Plane( Effect effect, Matrix world )
            : base(effect)
        {
            _plane = new PlanePrimitive<VertexPositionNormal>(
                effect.GraphicsDevice,
                (x,y) => new VertexPositionNormal(new Vector3(x,0,y), Vector3.Up ),
                10, 10);
            _world = world;
        }

        protected override void Draw()
        {
            _epWorld.SetValue(_world);
            _plane.Draw(Effect);
        }

    }

}
