﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace factor10.VisionThing.Primitives
{
    /// <summary>
    /// Geometric primitive class for drawing cylinders.
    /// </summary>
    public class CylinderPrimitive<T> : GeometricPrimitive<T> where T : struct, IVertexType
    {
        public delegate T CreateVertex(Vector3 position, Vector3 normal, Vector2 textureCoordinate);

        /// <summary>
        /// Constructs a new cylinder primitive, using default settings.
        /// </summary>
        public CylinderPrimitive(GraphicsDevice graphicsDevice, CreateVertex createVertex)
            : this(graphicsDevice, createVertex, 1, 1, 32)
        {
        }

        /// <summary>
        /// Constructs a new cylinder primitive,
        /// with the specified size and tessellation level.
        /// </summary>
        public CylinderPrimitive(GraphicsDevice graphicsDevice,
                                 CreateVertex createVertex,
                                 float height, float diameter, int tessellation)
        {
            if (tessellation < 3)
                throw new ArgumentOutOfRangeException("tessellation");

            height /= 2;

            float radius = diameter/2;

            // Create a ring of triangles around the outside of the cylinder.
            for (var i = 0; i < tessellation; i++)
            {
                var normal = getCircleVector(i, tessellation);

                addVertex(createVertex(normal*radius + Vector3.Up*height, normal, Vector2.Zero));
                addVertex(createVertex(normal*radius + Vector3.Down*height, normal, Vector2.Zero));

                addIndex(i*2);
                addIndex(i*2 + 1);
                addIndex((i*2 + 2)%(tessellation*2));

                addIndex(i*2 + 1);
                addIndex((i*2 + 3)%(tessellation*2));
                addIndex((i*2 + 2)%(tessellation*2));
            }

            // Create flat triangle fan caps to seal the top and bottom.
            CreateCap(createVertex, tessellation, height, radius, Vector3.Up);
            CreateCap(createVertex, tessellation, height, radius, Vector3.Down);

            initializePrimitive(graphicsDevice);
        }


        /// <summary>
        /// Helper method creates a triangle fan to close the ends of the cylinder.
        /// </summary>
        private void CreateCap(CreateVertex createVertex, int tessellation, float height, float radius, Vector3 normal)
        {
            // Create cap indices.
            for (var i = 0; i < tessellation - 2; i++)
            {
                if (normal.Y > 0)
                {
                    addIndex(CurrentVertex);
                    addIndex(CurrentVertex + (i + 1)%tessellation);
                    addIndex(CurrentVertex + (i + 2)%tessellation);
                }
                else
                {
                    addIndex(CurrentVertex);
                    addIndex(CurrentVertex + (i + 2)%tessellation);
                    addIndex(CurrentVertex + (i + 1)%tessellation);
                }
            }

            // Create cap vertices.
            for (var i = 0; i < tessellation; i++)
            {
                var position = getCircleVector(i, tessellation)*radius +
                               normal*height;

                addVertex(createVertex(position, normal, Vector2.Zero));
            }
        }

        private static Vector3 getCircleVector(int i, int tessellation)
        {
            var angle = i*MathHelper.TwoPi/tessellation;
            var dx = (float) Math.Cos(angle);
            var dz = (float) Math.Sin(angle);
            return new Vector3(dx, 0, dz);
        }

    }

}