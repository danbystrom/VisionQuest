using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using factor10.VisionaryHeads;
using factor10.VisionThing;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using Buffer = SharpDX.Toolkit.Graphics.Buffer;

namespace factor10.VisionQuest
{
    public struct StartAndCount
    {
        public int Start;
        public int Count;
    }

    public class Arcs : ClipDrawable
    {
        public readonly Archipelag Archipelag;

        private readonly Buffer<ArcVertex> _vertexBuffer;
        private readonly VertexInputLayout _vertexInputLayout;

        public Arcs(
            VisionContent vContent,
            Archipelag archipelag)
            : base(vContent.LoadEffect("Effects/ArcsEffect"))
        {
            Archipelag = archipelag;
            Effect.World = Matrix.Identity;

            var lines = new List<ArcVertex>();

            var modules = archipelag.CodeIslands.ToDictionary(ci => ci.VAssembly.Name, ci => ci);
            foreach (var island in modules.Values)
                processOneIsland(lines, island, island.VAssembly.VProgram, modules);

            if (lines.Any())
            {
                _vertexBuffer = Buffer.Vertex.New(vContent.GraphicsDevice, lines.ToArray());
                _vertexInputLayout = VertexInputLayout.FromBuffer(0, _vertexBuffer);
            }
        }

        private static void processOneIsland(
            List<ArcVertex> lines,
            CodeIsland island,
            VProgram vp,
            Dictionary<string, CodeIsland> modules)
        {
            if (island.VAssembly.Is3DParty)
                return;

            var arc = new ArcGenerator(4);

            foreach (var vclass in island.Classes.Values)
            {
                var arcStart = lines.Count;

                var done = new HashSet<VisionClass> {vclass};
                foreach (var vmethod in vclass.VClass.VMethods)
                    foreach (var name in vmethod.Calling)
                    {
                        VMethod callTo;
                        if (!vp.VMethods.TryGetValue(name, out callTo))
                            continue;
                        CodeIsland islandD;
                        if (!modules.TryGetValue(callTo.AssemblyName, out islandD))
                            continue;
                        var calledClass = islandD.Classes[callTo.VClass.FullName];
                        vclass.CalledClasses.Add(calledClass);
                        if (done.Contains(calledClass))
                            continue;
                        done.Add(calledClass);
                        var v1 = island.World.TranslationVector + new Vector3(vclass.X - 64, vclass.Height, vclass.Y - 64);
                        var v2 = islandD.World.TranslationVector + new Vector3(calledClass.X - 64, calledClass.Height, calledClass.Y - 64);
                        var distance = Vector3.Distance(v1, v2)/8;
                        arc.CreateArc(
                            v1,
                            v2,
                            Vector3.Up,
                            distance);
                        var arcStart2 = lines.Count;
                        arc.StoreVertices(
                            lines,
                            (p, f, idx) =>
                            {
                                var c = 0.25f + f/2;
                                return new ArcVertex(
                                    p,
                                    new Color(c, c, 1 - c, 1f),
                                    (idx%2) == 1 ? 0 : (MathUtil.TwoPi*(int) (distance/16 + 1)));
                            });
                        //calledClass.IncomingArcs.Add(new StartAndCount { Start = arcStart2, Count = lines.Count - arcStart2 });
                    }

                vclass.OutgoingArcs = new StartAndCount {Start = arcStart, Count = lines.Count - arcStart};
            }
        }

        private float _time;

        public override void Update(Camera camera, GameTime gameTime)
        {
            _time = MathUtil.Mod2PI(_time + (float) gameTime.ElapsedGameTime.TotalSeconds*10);
        }

        private void drawArchsFromClass(VisionClass vclass, int level)
        {
            Effect.GraphicsDevice.Draw(PrimitiveType.LineList, vclass.OutgoingArcs.Count, vclass.OutgoingArcs.Start);
            if(level>0)
                foreach (var vc in vclass.CalledClasses)
                    drawArchsFromClass(vc, level - 1);
//                Effect.GraphicsDevice.Draw(PrimitiveType.LineList, x.Count, x.Start);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            try
            {
                camera.UpdateEffect(Effect);
                Effect.Parameters["Time"].SetValue(_time);
                Effect.Apply();

                Effect.GraphicsDevice.SetVertexBuffer(_vertexBuffer);
                Effect.GraphicsDevice.SetVertexInputLayout(_vertexInputLayout);
                Effect.GraphicsDevice.SetDepthStencilState(Effect.GraphicsDevice.DepthStencilStates.DepthRead);

                if (Archipelag.SelectedClass != null)
                    drawArchsFromClass(Archipelag.SelectedClass, 2);
                else
                    Effect.GraphicsDevice.Draw(PrimitiveType.LineList, _vertexBuffer.ElementCount);
            }
            catch (Exception)
            {
            }
            return true;
        }

    }

}
