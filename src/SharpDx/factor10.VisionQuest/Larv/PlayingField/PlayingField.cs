using System;
using System.Collections.Generic;
using factor10.VisionThing;
using Larv.Serpent;
using Larv.Util;
using SharpDX;
using SharpDX.Toolkit.Graphics;
using Buffer = SharpDX.Toolkit.Graphics.Buffer;

namespace Larv
{
    public class PlayingField : ClipDrawable
    {
        public readonly int Floors, Width, Height;
        public readonly Buffer<VertexPositionNormalTexture> VertexBuffer;
        public readonly Buffer<VertexPositionColor> VertexBufferShadow;
        public readonly VertexInputLayout VertexInputLayout;

        public readonly IVDrawable _plane;

        public readonly PlayingFieldSquare[, ,] TheField;

        private readonly Texture2D _texture;

        public readonly Whereabouts PlayerWhereaboutsStart;
        public readonly Whereabouts EnemyWhereaboutsStart;

        public readonly float MiddleX;
        public readonly float MiddleY;

        public PlayingField(VisionContent vContent, Texture2D texture, int level)
            : base(vContent.LoadEffect("effects/simpletextureeffect"))
        {
            _texture = texture;

            var field = PlayingFields.GetLevel(level);

            _plane = new PlanePrimitive<VertexPositionNormalTexture>(vContent.GraphicsDevice, (x, y, w, h) => new VertexPositionNormalTexture(
                new Vector3(x, 0, y), Vector3.Up, new Vector2(x/w, y/h)),
                10, 10);

            Floors = field.Count;
            Height = field[0].Length;
            Width = field[0][0].Length;
            TheField = new PlayingFieldSquare[Floors, Height, Width];

            MiddleX = Width/2f;
            MiddleY = Height/2f;

            var builder = new PlayingFieldBuilder(TheField);
            for (var i = 0; i < field.Count; i++)
                builder.ConstructOneFloor(
                    i,
                    field[i],
                    ref PlayerWhereaboutsStart,
                    ref EnemyWhereaboutsStart);

            var verts = new List<VertexPositionNormalTexture>();
            var vertsShadow = new List<VertexPositionColor>();
            for (var z = 0; z < Floors; z++)
                for (var y = 0; y < Height; y++ )
                     for (var x = 0; x < Width; x++)
                         if (!TheField[z, y, x].IsNone)
                         {
                             var start = x;
                             x++;
                             foobar(verts, vertsShadow, z, start, y, x - start, 1, TheField[z, y, x - 1].Corners);
                             x--;
                         }


            verts.Clear();
            verts.Add(new VertexPositionNormalTexture(new Vector3(-1, 0, -1), Vector3.Up, new Vector2(0, 0)));
            verts.Add(new VertexPositionNormalTexture(new Vector3(Width + 1, 0, -1), Vector3.Up, new Vector2(Width*0.25f, 0)));
            verts.Add(new VertexPositionNormalTexture(new Vector3(-1, 0, Height + 1), Vector3.Up, new Vector2(0, Height * 0.25f)));

            verts.Add(new VertexPositionNormalTexture(new Vector3(Width + 1, 0, -1), Vector3.Up, new Vector2(Width * 0.25f, 0)));
            verts.Add(new VertexPositionNormalTexture(new Vector3(Width + 1, 0, Height + 1), Vector3.Up, new Vector2(Width * 0.25f, Height * 0.25f)));
            verts.Add(new VertexPositionNormalTexture(new Vector3(-1, 0, Height + 1), Vector3.Up, new Vector2(0, Height * 0.25f)));

            //verts.Add(new VertexPositionNormalTexture(new Vector3(-1, 0, -1), Vector3.Up, new Vector2(0, 0)));

            VertexBuffer = Buffer.Vertex.New(vContent.GraphicsDevice, verts.ToArray());
            VertexBufferShadow = Buffer.Vertex.New(vContent.GraphicsDevice, vertsShadow.ToArray());
            VertexInputLayout = VertexInputLayout.FromBuffer(0, VertexBuffer);
        }

        private void foobar(
            IList<VertexPositionNormalTexture> verts,
            IList<VertexPositionColor> vertsShadow,
            int z,
            int x,
            int y,
            int w,
            int h,
            int[] ri)
        {
            z *= 4;
            addVertex(verts, vertsShadow, z, ri[1], x, y, 0, 0); //NW
            addVertex(verts, vertsShadow, z, ri[3], x, y, w, 0); //NE
            addVertex(verts, vertsShadow, z, ri[0], x, y, 0, h); //SW
            addVertex(verts, vertsShadow, z, ri[0], x, y, 0, h); //SW
            addVertex(verts, vertsShadow, z, ri[3], x, y, w, 0); //NE
            addVertex(verts, vertsShadow, z, ri[2], x, y, w, h); //SE
        }

        private void addVertex(
            IList<VertexPositionNormalTexture> verts,
            IList<VertexPositionColor> vertsShadow,
            int floor,
            int z,
            int x,
            int y,
            int w,
            int h)
        {
            verts.Add(new VertexPositionNormalTexture(
                new Vector3((x + w), (floor + z)/3f, (y + h)),
                Vector3.Up,
                new Vector2((w%8)*0.25f, (h%8)*0.25f)));

            var dx = w == 0 ? -1 : 1;
            var dy = h == 0 ? -1 : 1;
            float fx = x;
            float fy = y;
            var tmpFloor = floor;
            if (!CanMoveHere(ref tmpFloor, new Point(x,y), new Point(x+dx, y)))
                fx += dx/20f;
            tmpFloor = floor;
            if (!CanMoveHere(ref tmpFloor, new Point(x, y), new Point(x, y+dy)))
                fy += dy/20f;

            vertsShadow.Add(new VertexPositionColor(
                new Vector3(fx+w, (floor+z) / 3f - 0.05f, fy+h),
                Color.DarkSlateBlue  ));
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            camera.UpdateEffect(Effect);

            Effect.World = Matrix.Translation(-0.5f, 0, -0.5f);
            Effect.Texture = _texture;

            foreach (var effectPass in Effect.Effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();

                Effect.GraphicsDevice.SetVertexInputLayout(VertexInputLayout);
                Effect.GraphicsDevice.SetVertexBuffer(VertexBuffer);

                Effect.GraphicsDevice.Draw(
                    PrimitiveType.TriangleList,
                    VertexBuffer.ElementCount);
            }

            //_plane.Draw(_effect);

            return true;
        }

        public PlayingFieldSquare FieldValue(Whereabouts whereabouts)
        {
            return FieldValue(whereabouts.Floor, whereabouts.Location);
        }

        public PlayingFieldSquare FieldValue( int floor, Point p)
        {
            if (floor < 0 || floor >= Floors)
                return new PlayingFieldSquare();
            if (p.Y < 0 || p.Y >= Height)
                return new PlayingFieldSquare();
            if (p.X < 0 || p.X >= Width)
                return new PlayingFieldSquare();
            return TheField[floor, p.Y, p.X];
        }

        public bool CanMoveHere(ref int floor, Point currentLocation, Point newLocation, bool ignoreRestriction = false)
        {
            if (!FieldValue(floor, newLocation).IsNone)
            {
                if (ignoreRestriction)
                    return true;
                var restricted = FieldValue(floor, newLocation).Restricted;
                return restricted == Direction.None || restricted == Direction.FromPoints(currentLocation, newLocation);
            }
            if (FieldValue(floor, currentLocation).IsSlope && FieldValue(floor + 1, newLocation).IsPortal)
            {
                floor++;
                return true;
            }
            if (FieldValue(floor, currentLocation).IsPortal && !FieldValue(floor - 1, newLocation).IsNone)
            {
                floor--;
                return true;
            }
            return false;
        }

        public float GetElevation(
            Whereabouts whereabouts)
        {
            var p = whereabouts.NextLocation;
            var square = FieldValue(whereabouts.Floor, p);
            if (square.IsNone)
                return 0;
            switch (square.PlayingFieldSquareType)
            {
                case PlayingFieldSquareType.None:
                    throw new Exception();
                case PlayingFieldSquareType.Flat:
                    return whereabouts.Floor * 1.3333f;
                default:
                    //här e något som ska ändras
                    var fraction = whereabouts.Fraction;
                    if (square.SlopeDirection.Backward == whereabouts.Direction)
                        fraction = 1 - fraction;
                    else if (square.SlopeDirection != whereabouts.Direction)
                        throw new Exception();
                    return whereabouts.Floor * 1.3333f + (square.Elevation + fraction) / 3f;
            }

            //var p = whereabouts.Location;
            //var square = FieldValue(whereabouts.Floor, p);
            //if (square.IsNone)
            //    return 0;
            //var dp = whereabouts.Direction.DirectionAsPoint();
            //var diffX = (square.Corners[2] - square.Corners[0]) * dp.X;
            //var diffY = (square.Corners[3] - square.Corners[1]) * dp.Y;

            //return whereabouts.Floor * 1.3333f + (diffX + diffY);
        }

        public void GetCameraPositionForLookingAtPlayerCave(out Vector3 toPosition, out Vector3 toLookAt)
        {
            var lookAtDirection = PlayerWhereaboutsStart.Direction.DirectionAsVector3();
            toLookAt = PlayerWhereaboutsStart.GetPosition(this) + lookAtDirection * 4;

            var finalNormal = Vector3.TransformNormal(
                lookAtDirection * SerpentCamera.CameraDistanceToHeadXz * 1.2f,
                Matrix.RotationY(-MathUtil.Pi * 0.2f));
            toPosition = toLookAt + finalNormal;
            toPosition.Y += SerpentCamera.CameraDistanceToHeadY;
        }

        public override void Dispose()
        {
            VertexBuffer.Dispose();
        }

    }

}
