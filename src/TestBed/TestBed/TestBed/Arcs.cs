using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing;

namespace TestBed
{
    public class Arcs : ClipDrawable
    {
        private readonly VertexPositionColor[] _lines;

        public Arcs()
            : base(VisionContent.LoadPlainEffect("Effects/ArcsEffect"))
        {
            Effect.World = Matrix.Identity;

            var arc = new ArcGenerator(4);
            arc.CreateArc(
                new Vector3(0, 0, 0),
                new Vector3(-100, 0, -100),
                Vector3.Up,
                20);

            var white = Color.White.ToVector3();
            var black = Color.Black.ToVector3();

            var lines = new List<VertexPositionColor>();
            arc.StoreArc(
                lines,
                (p, f) => new VertexPositionColor(
                              p,
                              new Color(Vector3.Lerp(white, black, f))));

            _lines = lines.ToArray();
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            camera.UpdateEffect(Effect);
            Effect.Apply();
            Effect.GraphicsDevice.DrawUserPrimitives(
                PrimitiveType.LineList,
                _lines,
                0, _lines.Length / 2 - 1);
            return true;
        }

    }

}
