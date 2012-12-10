using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using factor10.VisionThing;

namespace TestBed
{
    class BillboardSample : ClipDrawable
    {
        public BillboardSample()
            : base(VisionContent.LoadPlainEffect(""))
        {
            
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            // Then we use a two-pass technique to render alpha blended billboards with
            // almost-correct depth sorting. The only way to make blending truly proper for
            // alpha objects is to draw everything in sorted order, but manually sorting all
            // our billboards would be very expensive. Instead, we draw in two passes.
            //
            // The first pass has alpha blending turned off, alpha testing set to only accept
            // ~95% or more opaque pixels, and the depth buffer turned on. Because this is only
            // rendering the solid parts of each billboard, the depth buffer works as
            // normal to give correct sorting, but obviously only part of each billboard will
            // be rendered.
            //
            // Then in the second pass we enable alpha blending, set alpha test to only accept
            // pixels with fractional alpha values, and set the depth buffer to test against
            // the existing data but not to write new depth values. This means the translucent
            // areas of each billboard will be sorted correctly against the depth buffer
            // information that was previously written while drawing the opaque parts, although
            // there can still be sorting errors between the translucent areas of different
            // billboards.
            //
            // In practice, sorting errors between translucent pixels tend not to be too
            // noticable as long as the opaque pixels are sorted correctly, so this technique
            // often looks ok, and is much faster than trying to sort everything 100%
            // correctly. It is particularly effective for organic textures like grass and
            // trees.
            //foreach (ModelMesh mesh in landscape.Meshes)
            //{
            //    if (mesh.Name == "Billboards")
            //    {
            //        // First pass renders opaque pixels.
            //        foreach (Effect effect in mesh.Effects)
            //        {
            //            effect.Parameters["View"].SetValue(view);
            //            effect.Parameters["Projection"].SetValue(projection);
            //            effect.Parameters["LightDirection"].SetValue(lightDirection);
            //            effect.Parameters["WindTime"].SetValue(time);
            //            effect.Parameters["AlphaTestDirection"].SetValue(1f);
            //        }

            //        device.BlendState = BlendState.Opaque;
            //        device.DepthStencilState = DepthStencilState.Default;
            //        device.RasterizerState = RasterizerState.CullNone;
            //        device.SamplerStates[0] = SamplerState.LinearClamp;

            //        mesh.Draw();

            //        // Second pass renders the alpha blended fringe pixels.
            //        foreach (Effect effect in mesh.Effects)
            //        {
            //            effect.Parameters["AlphaTestDirection"].SetValue(-1f);
            //        }

            //        device.BlendState = BlendState.NonPremultiplied;
            //        device.DepthStencilState = DepthStencilState.DepthRead;

            //        mesh.Draw();
            //    }
            //}
            return true;
        }

    }

}
