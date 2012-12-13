﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace factor10.VisionThing.Terrain
{
    public class WeightsMap : ColorSurface
    {

       public WeightsMap(int width, int height)
            : base(width,height)
       {
       }

        public WeightsMap(Ground ground, float[] levels = null)
            : base(ground.Width,ground.Height)
        {
            var min = ground.Values.Min();
            var max = ground.Values.Max();
            var span = max - min;
            if (levels == null)
                levels = new[] {0, 0.33f, 0.76f, 1};
            for (var i = 0; i < 4; i++)
                levels[i] = min + levels[i]*span;
            span /= 4;
            for (var i = 0; i < ground.Values.Length; i++)
            {
                var val = ground.Values[i];
                var t0 = MathHelper.Clamp(1.0f - Math.Abs(val - levels[0])/span, 0, 1);
                var t1 = MathHelper.Clamp(1.0f - Math.Abs(val - levels[1])/span, 0, 1);
                var t2 = MathHelper.Clamp(1.0f - Math.Abs(val - levels[2])/span, 0, 1);
                var t3 = MathHelper.Clamp(1.0f - Math.Abs(val - levels[3])/span, 0, 1);
                var tot = 1/(t0 + t1 + t2 + t3 + 0.0001f);
                Values[i] = new Color(t0*tot, t1*tot, t2*tot, t3*tot);
            }
        }

    }

}
