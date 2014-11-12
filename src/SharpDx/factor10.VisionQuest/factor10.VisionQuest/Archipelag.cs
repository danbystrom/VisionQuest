using factor10.VisionaryHeads;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Water;
using SharpDX;
using SharpDX.Toolkit;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace factor10.VisionQuest
{
    public class Archipelag : ClipDrawable
    {
        private readonly BannerSign _bannerSign;
        private readonly Arcs _arcs;

        public VisionClass SelectedClass { get; set; }

        public Archipelag(
            VisionContent vContent,
            VProgram vprogram,
            WaterSurface water,
            ShadowMap shadow)
            : base((IVEffect)null)
        {
            foreach (var fil in Directory.GetFiles(@"c:\users\dan\desktop\VisionQuest\", "*.metrics.txt"))
                GenerateMetrics.FromPregeneratedFile(fil).UpdateProgramWithMetrics(vprogram);

            var codeIslands = CodeIsland.Create(vContent, this, vprogram.VAssemblies);
            foreach (var codeIsland in codeIslands)
            {
                water.ReflectedObjects.Add(codeIsland);
                shadow.ShadowCastingObjects.Add(codeIsland);
                Children.Add(codeIsland);
            }

            _bannerSign = new BannerSign(vContent, codeIslands);
            _arcs = new Arcs(vContent, this);
        }

        public IEnumerable<CodeIsland> CodeIslands
        {
            get { return Children.Cast<CodeIsland>(); }
        }

        public void Kill(WaterSurface water, ShadowMap shadow)
        {
            foreach (var island in Children)
            {
                water.ReflectedObjects.Remove(island);
                shadow.ShadowCastingObjects.Remove(island);
                island.Dispose();
            }
            Children.Clear();
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            return false;
        }

        private VisionClass hitTestSign(Ray ray)
        {
            var hits = (from codeIsland in Children.Cast<CodeIsland>() from vc in codeIsland.Classes.Values where ray.Intersects(vc.SignClickBoundingSphere) select vc).ToList();
            foreach (var hit in hits)
                hit.DistanceFromCameraOnHit = (int)Vector3.DistanceSquared(ray.Position, hit.SignClickBoundingSphere.Center);
            hits.Sort((x, y) => x.DistanceFromCameraOnHit - y.DistanceFromCameraOnHit);
            return hits.FirstOrDefault();
        }

        private VisionClass hitTestGround(Ray ray)
        {
            var hits = (from codeIsland in Children.Cast<CodeIsland>() from vc in codeIsland.Classes.Values where ray.Intersects(vc.GroundBoundingSphere) select vc).ToList();
            foreach (var hit in hits)
                hit.DistanceFromCameraOnHit = (int) Vector3.DistanceSquared(ray.Position, hit.GroundBoundingSphere.Center);
            hits.Sort((x, y) =>  x.DistanceFromCameraOnHit - y.DistanceFromCameraOnHit);
            return hits.FirstOrDefault();
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
            if (camera.MouseState.LeftButton.Pressed)
            {
                var hit = hitTestSign(camera.GetPickingRay());
                if (hit != null)
                    hit.CodeIsland.Archipelag.SelectedClass = hit;
                //var q = hitTestGround(camera.GetPickingRay());
                //if(q!=null)
                //    q.CodeIsland
            }

            _bannerSign.Update(camera, gameTime);
            _arcs.Update(camera, gameTime);
            base.Update(camera, gameTime);
        }

        public void DrawSignsAndArchs(Camera camera, bool lines)
        {
            if(lines)
                _arcs.Draw(camera);
            _bannerSign.Draw(camera);
        }

        public void PlayAround(IVEffect effect, IVDrawable thing)
        {
            foreach (var vc in Children.Cast<CodeIsland>().SelectMany(_ => _.Classes.Values))
            {
                effect.World = Matrix.Scaling(vc.SignClickBoundingSphere.Radius*2) * Matrix.Translation(vc.SignClickBoundingSphere.Center);
                //thing.Draw(effect);
            }
        }

    }

}