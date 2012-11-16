using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factor10.VisionThing.Primitives
{

    public class PlanePrimitive<T> : GeometricPrimitive<T> where T : struct, IVertexType
    {
        public delegate T CreateVertex(float x, float y, int width, int height);

        public PlanePrimitive(
            GraphicsDevice graphicsDevice,
            CreateVertex createVertex,
            int width,
            int height,
            int levels = 1)
        {
            for (var y = 0; y <= height; y++)
                for (var x = 0; x <= width; x++)
                    addVertex(createVertex(x, y, width, height));

            for (var level = 0; level < levels; level++)
            {
                addLevelOfDetail();
                var p = 1 << level;
                for (var y = 0; y < height; y+=p)
                    for (var x = 0; x < width; x+=p)
                    {
                        var start = y*(height + 1) + x;

                        addIndex(start + 0);
                        addIndex(start + p);
                        addIndex(start + height*p + p);

                        addIndex(start + p);
                        addIndex(start + height*p + 2*p);
                        addIndex(start + height*p + p);
                    }
            }

            initializePrimitive(graphicsDevice);
        }

        /*
    for (var level = 0; level < levels; level++)
    {
        addLevelOfDetail();
        var p = 1 << level;
        var pheight = height*p;
        for (var y = 0; y < height; y+=p)
            for (var x = 0; x < width; x+=p)
            {
                var start = y*height + x;
                addIndex(start + 0);
                addIndex(start + p);
                addIndex(start + pheight + p);

                addIndex(start + p);
                addIndex(start + pheight + 2*p);
                addIndex(start + pheight + p);
            }
    }
 */

    }

}
