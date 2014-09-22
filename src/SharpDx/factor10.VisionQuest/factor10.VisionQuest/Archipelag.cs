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
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionQuest
{
    public class Archipelag : ClipDrawable
    {
        private SignsBig _signsBig;
        private Arcs _arcs;

        public Archipelag(VisionContent vContent, WaterSurface water, ShadowMap shadow) : base((IEffect)null)
        {
            var vprogram = new VProgram(@"C:\proj\photomic.old\src\Plata\bin\Release\Plåta.exe");
            foreach (var fil in Directory.GetFiles(@"c:\users\dan\desktop\VisionQuest\", "*.metrics.txt"))
                GenerateMetrics.FromPregeneratedFile(fil).UpdateProgramWithMetrics(vprogram);

            var codeIslands = CodeIsland.Create(vContent, vprogram.VAssemblies);
            foreach (var codeIsland in codeIslands)
            {
                water.ReflectedObjects.Add(codeIsland);
                shadow.ShadowCastingObjects.Add(codeIsland);
            }

            _signsBig = new SignsBig(vContent, codeIslands);
            _arcs = new Arcs(vContent, codeIslands);

            Children.Add(_signsBig);
            Children.Add(_arcs);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            return Draw(camera, drawingReason, shadowMap);
        }

        public override void DrawReflection(Vector4? clipPlane, Camera camera)
        {
        }

    }

}