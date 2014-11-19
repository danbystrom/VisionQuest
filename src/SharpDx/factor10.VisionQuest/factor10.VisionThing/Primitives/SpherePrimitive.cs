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
        public delegate T CreateVertex(Vector3 position, Vector3 normal, Vector3 tangent, Vector2 textureCoordinate);

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
            addVertex(createVertex(Vector3.Down*radius, Vector3.Down, Vector3.BackwardLH, new Vector2(0.5f, 0.5f)));

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
                    var tangent = Vector3.Cross(Vector3.Up, normal);
                    tangent.Normalize();

                    //alternative
                    var theta = Math.Acos(normal.Z);
                    var phi = Math.Atan(normal.Y/normal.X);
                    theta += Math.PI/2;
                    var tangent2 = new Vector3(
                        (float) (radius*Math.Sin(theta)*Math.Cos(phi)),
                        (float) (radius*Math.Sin(theta)*Math.Sin(phi)),
                        (float) (radius*Math.Cos(theta)));
                    //alternative


                    //alternative 2
                    var tangent3 = new Vector3(
                        -radius*(float) Math.Sin(longitude)*dxz,
                        0,
                        radius*(float) Math.Cos(longitude)*dxz);
                    //alternative 2

                    addVertex(createVertex(normal*radius, normal, tangent3, textureCoordinate));
                }
            }

            // Finish with a single vertex at the top of the sphere.
            addVertex(createVertex(Vector3.Up * radius, Vector3.Up, Vector3.ForwardLH, new Vector2(0.5f, 1)));

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
