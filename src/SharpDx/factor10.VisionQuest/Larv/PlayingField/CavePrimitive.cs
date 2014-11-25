﻿using System;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace Larv
{
    /// <summary>
    /// Geometric primitive class for drawing cylinders.
    /// </summary>
    public class CavePrimitive<T> : factor10.VisionThing.Primitives.GeometricPrimitive<T> where T : struct, IEquatable<T>
    {
        public delegate T CreateVertex(Vector3 position, Vector3 normal, Vector3 tangent, Vector2 textureCoordinate);

        /// <summary>
        /// Constructs a new cylinder primitive, using default settings.
        /// </summary>
        public CavePrimitive(GraphicsDevice graphicsDevice, CreateVertex createVertex, bool swap = false)
            : this(graphicsDevice, createVertex, 1, 1, 0.1f, 32, swap)
        {
        }

        /// <summary>
        /// Constructs a new cylinder primitive,
        /// with the specified size and tessellation level.
        /// </summary>
        public CavePrimitive(
            GraphicsDevice graphicsDevice,
            CreateVertex createVertex,
            float height,
            float diameter,
            float roofThickness,
            int tessellation,
            bool swap = false)
        {
            if (tessellation < 3)
                throw new ArgumentOutOfRangeException("tessellation");

            height /= 2;
            var radius = diameter/2;

            // Create a ring of triangles around the outside of the cylinder.
            createRoof(createVertex, tessellation, height, radius, swap);
            createRoof(createVertex, tessellation, height, radius - roofThickness, !swap);

            // Create flat triangle fan caps to seal the top and bottom.
            CreateCap(createVertex, height, radius, roofThickness, tessellation, Vector3.Up, swap);
            CreateCap(createVertex, height, radius, roofThickness, tessellation, Vector3.Down, swap);

            initializePrimitive(graphicsDevice);
        }

        private void createRoof(CreateVertex createVertex, int tessellation, float height, float radius, bool swap)
        {
            var t2 = tessellation*2;
            var cv = CurrentVertex;

            for (var i = tessellation/2; i < tessellation; i++)
            {
                var normal = getCircleVector(i, tessellation);
                var txCoord = new Vector2((float) i/tessellation, 0.0f);

                addVertex(createVertex(normal*radius + Vector3.Up*height, normal, Vector3.Zero, txCoord));
                addVertex(createVertex(normal*radius + Vector3.Down*height, normal, Vector3.Zero, txCoord + Vector2.UnitY));

                addTriangle(cv + i*2, cv + i*2 + 1, cv + i*2 + 2, swap);
                addTriangle(cv + i*2 + 1, cv + i*2 + 3, cv + i*2 + 2, swap);
            }
            var g = new GeometricPrimitive.Cylinder();
        }

        /// <summary>
        /// Helper method creates a triangle fan to close the ends of the cylinder.
        /// </summary>
        private void CreateCap(CreateVertex createVertex, float height, float radius, float roofThickness, int tessellation, Vector3 normal, bool swap)
        {
            // Create cap indices.
            for (var i = 0; i < tessellation - 2; i++)
            {
                var c = CurrentVertex + 1;
                addTriangle(c, c + 1, c + 2, swap ^ normal.Y > 0);
                addTriangle(c + 1, c + 2, c + 3, swap ^ normal.Y > 0);
            }

            var yoffset = normal*height;
            // Create cap vertices.
            for (var i = 0; i < tessellation; i++)
            {
                var cv = getCircleVector(i, tessellation);
                var position = cv * radius + yoffset;
                var txCoord = new Vector2((cv.X + 1)/2, (cv.Z + 1)/2);
                addVertex(createVertex(position, normal, Vector3.Zero, txCoord));
                position = cv * (radius-roofThickness) + yoffset;
                txCoord = new Vector2((cv.X + 1) / 2, (cv.Z + 1) / 2);
                addVertex(createVertex(position, normal, Vector3.Zero, txCoord));
            }
        }

        private static Vector3 getCircleVector(int i, int tessellation)
        {
            var angle = i * MathUtil.TwoPi / tessellation;
            var dx = (float)Math.Cos(angle);
            var dz = (float)Math.Sin(angle);
            return new Vector3(dx, 0, dz);
        }

    }

}