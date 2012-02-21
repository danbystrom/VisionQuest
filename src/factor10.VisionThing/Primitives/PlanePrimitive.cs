using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factor10.VisionThing.Primitives
{
    /*
    public struct VertexPosition : IVertexType
    {
        public Vector3 Position;
        public static readonly VertexElement[] VertexElements =
        {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
        };

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return new VertexDeclaration(VertexElements); }
        }
    }
    */
    public class PlanePrimitive<T> : GeometricPrimitive<T> where T : struct, IVertexType
    {
        public delegate T CreateVertex(float x, float y);

        public PlanePrimitive(
            GraphicsDevice graphicsDevice,
            CreateVertex createVertex,
            int width,
            int height)
        {
            //_effect = effect;

            for (var y = 0; y <= height; y++)
                for (var x = 0; x <= width; x++)
                    addVertex(createVertex(x, y));

            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                {
                    var start = y*(height + 1) + x;

                    addIndex(start + 0);
                    addIndex(start + 1);
                    addIndex(start + height + 1);

                    addIndex(start + 1);
                    addIndex(start + height +2);
                    addIndex(start + height+1);
                }

            initializePrimitive(graphicsDevice);
        }

    }

}
