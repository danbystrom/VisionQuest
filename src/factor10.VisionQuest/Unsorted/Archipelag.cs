﻿using factor10.VisionaryHeads;
using factor10.VisionThing;
using factor10.VisionThing.CameraStuff;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Util;
using factor10.VisionThing.Water;
using SharpDX;
using SharpDX.Toolkit;
using System.Collections.Generic;
using System.Linq;

namespace factor10.VisionQuest
{
    public class Archipelag : VDrawable
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

        public override void Update(Camera camera, GameTime gameTime)
        {
            if (camera.MouseState.LeftButton.Pressed)
            {
                var ray = camera.GetPickingRay();
                var islandsHit = CollisionHelpers.HitTest(ray, Children.Cast<CodeIsland>(), _ => _.BoundingSphere);
                var signsHit = CollisionHelpers.HitTest(ray, islandsHit.SelectMany(_ => _.Classes.Values), _ => _.SignClickBoundingSphere);
                if (signsHit.Any())
                    signsHit[0].CodeIsland.Archipelag.SelectedClass = signsHit[0];

                foreach (var island in islandsHit)
                {
                    Vector3 hit, normal;
                    if (! island.HitTest(ray, out hit, out normal))
                        continue;
                    System.Diagnostics.Debug.Print("{0}: {1},{2}", island.VAssembly.Name, hit.X, hit.Y);
                }
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
            //foreach (var vc in Children.Cast<CodeIsland>().SelectMany(_ => _.Classes.Values))
            //{
            //    effect.World = Matrix.Scaling(vc.SignClickBoundingSphere.Radius * 2) * Matrix.Translation(vc.SignClickBoundingSphere.Center);
            //    thing.Draw(effect);
            //}
            //foreach (var ci in Children.Cast<CodeIsland>())
            //{
            //    effect.World = Matrix.Scaling(ci.BoundingSphere.Radius * 2) * Matrix.Translation(ci.BoundingSphere.Center);
            //    effect.World = Matrix.Scaling(10) * ci.World;
            //    thing.Draw(effect);
            //}
        }

    }

}