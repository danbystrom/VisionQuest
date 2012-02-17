﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factor10.VisionThing.Primitives
{
    /// <summary>
    /// Geometric primitive class for drawing cubes.
    /// </summary>
    public class CubePrimitive : GeometricPrimitive<VertexPositionNormal>
    {
        /// <summary>
        /// Constructs a new cube primitive, using default settings.
        /// </summary>
        public CubePrimitive(GraphicsDevice graphicsDevice)
            : this(graphicsDevice, 1)
        {
        }


        /// <summary>
        /// Constructs a new cube primitive, with the specified size.
        /// </summary>
        public CubePrimitive(GraphicsDevice graphicsDevice, float size)
        {
            // A cube has six faces, each one pointing in a different direction.
            Vector3[] normals =
            {
                new Vector3(0, 0, 1),
                new Vector3(0, 0, -1),
                new Vector3(1, 0, 0),
                new Vector3(-1, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, -1, 0),
            };

            // Create each face in turn.
            foreach (var normal in normals)
            {
                // Get two vectors perpendicular to the face normal and to each other.
                var side1 = new Vector3(normal.Y, normal.Z, normal.X);
                var side2 = Vector3.Cross(normal, side1);

                // Six indices (two triangles) per face.
                addIndex(CurrentVertex + 0);
                addIndex(CurrentVertex + 1);
                addIndex(CurrentVertex + 2);

                addIndex(CurrentVertex + 0);
                addIndex(CurrentVertex + 2);
                addIndex(CurrentVertex + 3);

                // Four vertices per face.
                addVertex(new VertexPositionNormal((normal - side1 - side2) * size / 2, normal));
                addVertex(new VertexPositionNormal((normal - side1 + side2) * size / 2, normal));
                addVertex(new VertexPositionNormal((normal + side1 + side2) * size / 2, normal));
                addVertex(new VertexPositionNormal((normal + side1 - side2) * size / 2, normal));
            }

            initializePrimitive(graphicsDevice);
        }

    }

}
