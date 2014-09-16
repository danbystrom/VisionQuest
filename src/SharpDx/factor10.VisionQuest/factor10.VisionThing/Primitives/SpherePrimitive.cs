using System;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace factor10.VisionThing.Primitives
{
    /// <summary>
    /// Geometric primitive class for drawing spheres.
    /// </summary>
    public class SpherePrimitive<T> : GeometricPrimitive<T> where T : struct, IEquatable<T>
    {
        public delegate T CreateVertex(Vector3 position, Vector3 normal, Vector2 textureCoordinate);

        /// <summary>
        /// Constructs a new sphere primitive,
        /// with the specified size and tessellation level.
        /// </summary>
        public SpherePrimitive(
            GraphicsDevice graphicsDevice,
            CreateVertex createVertex,
            float diameter = 1,
            int tessellation = 16,
            bool swap = true)
        {
            swap ^= true;

            if (tessellation < 3)
                throw new ArgumentOutOfRangeException("tessellation");

            var verticalSegments = tessellation;
            var horizontalSegments = tessellation*2;

            var radius = diameter/2;

            // Start with a single vertex at the bottom of the sphere.
            addVertex(createVertex(Vector3.Down*radius, Vector3.Down, Vector2.Zero));

            // Create rings of vertices at progressively higher latitudes.
            for (var i = 0; i < verticalSegments - 1; i++)
            {
                var latitude = ((i + 1)*MathUtil.Pi/verticalSegments) - MathUtil.PiOverTwo;

                var dy = (float) Math.Sin(latitude);
                var dxz = (float) Math.Cos(latitude);

                // Create a single ring of vertices at this latitude.
                for (var j = 0; j < horizontalSegments; j++)
                {
                    var longitude = j * MathUtil.TwoPi / horizontalSegments;
                    var dx = (float) Math.Cos(longitude)*dxz;
                    var dz = (float) Math.Sin(longitude)*dxz;
                    var normal = new Vector3(dx, dy, dz);
                    var textureCoordinate = new Vector2((float) (Math.Asin(dx)/Math.PI + 0.5), (float) (Math.Asin(dy)/Math.PI + 0.5));
                    addVertex(createVertex(normal * radius, normal, textureCoordinate));
                }
            }

            // Finish with a single vertex at the top of the sphere.
            addVertex(createVertex(Vector3.Up * radius, Vector3.Up, Vector2.Zero));

            // Create a fan connecting the bottom vertex to the bottom latitude ring.
            for (var i = 0; i < horizontalSegments; i++)
                addTriangle(0, 1 + (i + 1)%horizontalSegments, 1 + i, swap);

            // Fill the sphere body with triangles joining each pair of latitude rings.
            for (var i = 0; i < verticalSegments - 2; i++)
            {
                for (var j = 0; j < horizontalSegments; j++)
                {
                    var nextI = i + 1;
                    var nextJ = (j + 1)%horizontalSegments;

                    addTriangle(1 + i*horizontalSegments + j, 1 + i*horizontalSegments + nextJ, 1 + nextI*horizontalSegments + j, swap);
                    addTriangle(1 + i*horizontalSegments + nextJ, 1 + nextI*horizontalSegments + nextJ, 1 + nextI*horizontalSegments + j, swap);
                }
            }

            // Create a fan connecting the top vertex to the top latitude ring.
            for (var i = 0; i < horizontalSegments; i++)
                addTriangle(CurrentVertex - 1, CurrentVertex - 2 - (i + 1)%horizontalSegments, CurrentVertex - 2 - i, swap);

            initializePrimitive(graphicsDevice);
        }

    }

}
