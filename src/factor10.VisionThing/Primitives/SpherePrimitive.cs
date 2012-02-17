#region File Description
//-----------------------------------------------------------------------------
// SpherePrimitive.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace factor10.VisionThing.Primitives
{
    /// <summary>
    /// Geometric primitive class for drawing spheres.
    /// </summary>
    public class SpherePrimitive : GeometricPrimitive<VertexPositionNormal>
    {
        /// <summary>
        /// Constructs a new sphere primitive, using default settings.
        /// </summary>
        public SpherePrimitive(GraphicsDevice graphicsDevice)
            : this(graphicsDevice, 1, 16)
        {
        }


        /// <summary>
        /// Constructs a new sphere primitive,
        /// with the specified size and tessellation level.
        /// </summary>
        public SpherePrimitive(GraphicsDevice graphicsDevice,
                               float diameter, int tessellation)
        {
            if (tessellation < 3)
                throw new ArgumentOutOfRangeException("tessellation");

            var verticalSegments = tessellation;
            var horizontalSegments = tessellation*2;

            var radius = diameter/2;

            // Start with a single vertex at the bottom of the sphere.
            addVertex(new VertexPositionNormal(Vector3.Down*radius, Vector3.Down));

            // Create rings of vertices at progressively higher latitudes.
            for (var i = 0; i < verticalSegments - 1; i++)
            {
                var latitude = ((i + 1)*MathHelper.Pi/
                                  verticalSegments) - MathHelper.PiOver2;

                var dy = (float) Math.Sin(latitude);
                var dxz = (float) Math.Cos(latitude);

                // Create a single ring of vertices at this latitude.
                for (var j = 0; j < horizontalSegments; j++)
                {
                    var longitude = j*MathHelper.TwoPi/horizontalSegments;

                    var dx = (float) Math.Cos(longitude)*dxz;
                    var dz = (float) Math.Sin(longitude)*dxz;

                    var normal = new Vector3(dx, dy, dz);

                    addVertex(new VertexPositionNormal(normal*radius, normal));
                }
            }

            // Finish with a single vertex at the top of the sphere.
            addVertex(new VertexPositionNormal(Vector3.Up*radius, Vector3.Up));

            // Create a fan connecting the bottom vertex to the bottom latitude ring.
            for (var i = 0; i < horizontalSegments; i++)
            {
                addIndex(0);
                addIndex(1 + (i + 1)%horizontalSegments);
                addIndex(1 + i);
            }

            // Fill the sphere body with triangles joining each pair of latitude rings.
            for (var i = 0; i < verticalSegments - 2; i++)
            {
                for (var j = 0; j < horizontalSegments; j++)
                {
                    var nextI = i + 1;
                    var nextJ = (j + 1)%horizontalSegments;

                    addIndex(1 + i*horizontalSegments + j);
                    addIndex(1 + i*horizontalSegments + nextJ);
                    addIndex(1 + nextI*horizontalSegments + j);

                    addIndex(1 + i*horizontalSegments + nextJ);
                    addIndex(1 + nextI*horizontalSegments + nextJ);
                    addIndex(1 + nextI*horizontalSegments + j);
                }
            }

            // Create a fan connecting the top vertex to the top latitude ring.
            for (var i = 0; i < horizontalSegments; i++)
            {
                addIndex(CurrentVertex - 1);
                addIndex(CurrentVertex - 2 - (i + 1)%horizontalSegments);
                addIndex(CurrentVertex - 2 - i);
            }

            initializePrimitive(graphicsDevice);
        }

    }

}
