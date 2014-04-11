using System.Collections.Generic;
using System.Linq;
using factor10.VisionaryHeads;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing;
using Mono.Cecil;

namespace TestBed
{
    public class Arcs : ClipDrawable
    {
        private readonly ArcVertex[] _lines;

        public Arcs(IEnumerable<CodeIsland> codeIslands)
            : base(VisionContent.LoadPlainEffect("Effects/ArcsEffect"))
        {
            Effect.World = Matrix.Identity;

            var lines = new List<ArcVertex>();

            var modules = codeIslands.ToDictionary(ci => ci.VAssembly.AssemblyDefinition.MainModule, ci => ci);
            foreach (var island in modules.Values)
                processOneIsland(lines, island, island.VAssembly.VProgram, modules);

            _lines = lines.ToArray();
        }

        private static void processOneIsland(
            List<ArcVertex> lines,
            CodeIsland island,
            VProgram vp,
            Dictionary<ModuleDefinition, CodeIsland> modules)
        {
            var arc = new ArcGenerator(4);

            foreach (var vclass in island.Classes.Values)
            {
                var done = new HashSet<VisualClass> {vclass};
                foreach (var vmethod in vclass.VClass.VMethods)
                    foreach (var name in vmethod.Calling)
                    {
                        VMethod callTo;
                        if (!vp.VMethods.TryGetValue(name, out callTo))
                            continue;
                        CodeIsland islandD;
                        if (!modules.TryGetValue(callTo.MethodDefinition.Module, out islandD))
                            continue;
                        var calledClass = islandD.Classes[callTo.VClass.FullName];
                        if (done.Contains(calledClass))
                            continue;
                        done.Add(calledClass);
                        var v1 = island.World.Translation + new Vector3(vclass.X - 64, vclass.Height, vclass.Y - 64);
                        var v2 = islandD.World.Translation +
                                 new Vector3(calledClass.X - 64, calledClass.Height, calledClass.Y - 64);
                        var distance = Vector3.Distance(v1, v2)/8;
                        arc.CreateArc(
                            v1,
                            v2,
                            Vector3.Up,
                            distance);
                        arc.StoreArc(
                            lines,
                            (p, f, idx) =>
                            {
                                var c = 0.25f + f/2;
                                return new ArcVertex(
                                    p,
                                    new Color(c, c, 1 - c, 1f),
                                    (idx%2) == 1 ? 0 : (MathHelper.TwoPi*(int) (distance/16 + 1)));
                            });
                    }
            }
        }

        private float _time;

        public override void Update(GameTime gameTime)
        {
            _time = MathHelper.WrapAngle(_time + (float) gameTime.ElapsedGameTime.TotalSeconds*10);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            camera.UpdateEffect(Effect);
            Effect.Parameters["Time"].SetValue(_time);
            Effect.Apply();
            Effect.GraphicsDevice.DrawUserPrimitives(
                PrimitiveType.LineList,
                _lines,
                0, _lines.Length / 2 - 1);
            return true;
        }

    }

}
