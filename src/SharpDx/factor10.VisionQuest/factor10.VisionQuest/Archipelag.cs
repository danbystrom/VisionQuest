using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using factor10.VisionaryHeads;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;
using factor10.VisionThing.Water;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionQuest
{
    public class Archipelag : ClipDrawable
    {
        private readonly SignsBig _signsBig;
        private readonly Arcs _arcs;

        public Archipelag(
            VisionContent vContent,
            VProgram vprogram,
            WaterSurface water,
            ShadowMap shadow)
            : base((IEffect)null)
        {
            foreach (var fil in Directory.GetFiles(@"c:\users\dan\desktop\VisionQuest\", "*.metrics.txt"))
                GenerateMetrics.FromPregeneratedFile(fil).UpdateProgramWithMetrics(vprogram);

            var codeIslands = CodeIsland.Create(vContent, vprogram.VAssemblies);
            foreach (var codeIsland in codeIslands)
            {
                water.ReflectedObjects.Add(codeIsland);
                shadow.ShadowCastingObjects.Add(codeIsland);
                Children.Add(codeIsland);
            }

            _signsBig = new SignsBig(vContent, codeIslands);
            _arcs = new Arcs(vContent, codeIslands);
        }

        public void Kill(WaterSurface water, ShadowMap shadow)
        {
            foreach (var island in Children)
            {
                water.ReflectedObjects.Remove(island);
                shadow.ShadowCastingObjects.Remove(island);
            }
            Children.Clear();
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            return false;
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
            _signsBig.Update(camera, gameTime);
            _arcs.Update(camera, gameTime);
            base.Update(camera, gameTime);
        }

        public void DrawSignsAndArchs(Camera camera, bool lines)
        {
            if(lines)
                _arcs.Draw(camera);
            _signsBig.Draw(camera);
        }

    }

}