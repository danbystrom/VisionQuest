using System;
using System.Collections.Generic;
using factor10.VisionThing;
using SharpDX;
using SharpDX.Toolkit.Graphics;
using Buffer = SharpDX.Toolkit.Graphics.Buffer;

namespace Serpent
{
    public class PlayingField : IDisposable
    {
        public readonly int Floors, Width, Height;
        public readonly Buffer<VertexPositionNormalTexture> VertexBuffer;
        public readonly Buffer<VertexPositionColor> VertexBufferShadow;
        public readonly PlayingFieldSquare[, ,] TheField;

        private readonly BasicEffect _effect;
        private readonly Texture2D _texture;

        public readonly Whereabouts PlayerWhereaboutsStart;
        public readonly Whereabouts EnemyWhereaboutsStart;

        public PlayingField(GraphicsDevice graphicsDevice, Texture2D texture)
        {
            _effect = new BasicEffect(graphicsDevice);
            _texture = texture;

            var field = PlayingFields.GetZ();

            Floors = field.Count;
            Height = field[0].Length;
            Width = field[0][0].Length;
            TheField = new PlayingFieldSquare[Floors, Height, Width];

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


            VertexBuffer = Buffer.Vertex.New(graphicsDevice, verts.ToArray());
            VertexBufferShadow = Buffer.Vertex.New(graphicsDevice, vertsShadow.ToArray());
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
                new Vector2((w%8)*0.25f,(h%8)*0.25f)));//TODOnew Vector2((x+w)/2f, (y+h)/2f)));

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

        public void Draw( Camera camera )
        {
            //Set object and camera info
            _effect.View = camera.View;
            _effect.Projection = camera.Projection;

            _effect.World = Matrix.Translation(-0.5f, 0, -0.5f);
            _effect.Texture = _texture;
            _effect.TextureEnabled = true;
            _effect.VertexColorEnabled = false;

            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _effect.GraphicsDevice.SetVertexBuffer(VertexBuffer);
                _effect.GraphicsDevice.Draw(
                    PrimitiveType.TriangleList,
                    VertexBuffer.ElementCount);
            }

            //TODO
            //_effect.TextureEnabled = false;
            //_effect.VertexColorEnabled = true;
            //foreach (var pass in _effect.CurrentTechnique.Passes)
            //{
            //    pass.Apply();
            //    _effect.GraphicsDevice.SetVertexBuffer(VertexBufferShadow);
            //    _effect.GraphicsDevice.Draw(
            //        PrimitiveType.TriangleList,
            //        VertexBufferShadow.ElementCount/3);
            //}

        }

        private PlayingFieldSquare fieldValue( int floor, Point p)
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
            if (!fieldValue(floor, newLocation).IsNone)
            {
                if (ignoreRestriction)
                    return true;
                var restricted = fieldValue(floor, newLocation).Restricted;
                return restricted == Direction.None || restricted == Direction.FromPoints(currentLocation, newLocation);
            }
            if (fieldValue(floor, currentLocation).IsSlope && fieldValue(floor + 1, newLocation).IsPortal)
            {
                floor++;
                return true;
            }
            if (fieldValue(floor, currentLocation).IsPortal && !fieldValue(floor - 1, newLocation).IsNone)
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
            var square = fieldValue(whereabouts.Floor, p);
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
            //var square = fieldValue(whereabouts.Floor, p);
            //if (square.IsNone)
            //    return 0;
            //var dp = whereabouts.Direction.DirectionAsPoint();
            //var diffX = (square.Corners[2] - square.Corners[0]) * dp.X;
            //var diffY = (square.Corners[3] - square.Corners[1]) * dp.Y;

            //return whereabouts.Floor * 1.3333f + (diffX + diffY);
        }

        public void Dispose()
        {
            _effect.Dispose();
            VertexBuffer.Dispose();
            VertexBufferShadow.Dispose();
        }

    }

}
