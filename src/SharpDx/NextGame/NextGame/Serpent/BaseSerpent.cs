using System;
using System.Collections.Generic;
using factor10.VisionThing.Effects;
using factor10.VisionThing.Primitives;
using Serpent.Serpent;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Serpent
{
    public enum SerpentStatus
    {
        Alive,
        Ghost,
        Finished
    }

    public abstract class BaseSerpent
    {
        public SerpentStatus SerpentStatus;

        protected readonly PlayingField _pf;

        protected Whereabouts _whereabouts = new Whereabouts();
        protected Direction _headDirection;
        protected SerpentCamera _camera;

        protected double _fractionAngle;

        protected readonly BasicEffect _effect;
        protected readonly factor10.VisionThing.Primitives.GeometricPrimitive<VertexPositionNormal> _sphere;

        protected readonly SerpentTailSegment _tail;
        protected int _serpentLength;

        protected readonly Dictionary<Direction, Matrix> _headRotation = new Dictionary<Direction, Matrix>();

        protected abstract void takeDirection();

        private int _pendingEatenSegments = 6;
        private const int SegmentEatTreshold = 7;

        private float _layingEgg;
        private const float TimeForLayingEggProcess = 5;

        protected BaseSerpent(
            Game game,
            PlayingField pf, factor10.VisionThing.Primitives.GeometricPrimitive<VertexPositionNormal> sphere,
            Whereabouts whereabouts)
        {
            _pf = pf;
            _sphere = sphere;

            //_effect = new PlainEffectWrapper(game.Content.Load<Effect>(@"Effects\SimpleTextureEffect"), "nisse");

            _effect = new BasicEffect(game.GraphicsDevice);
            _effect.EnableDefaultLighting();

            _whereabouts = whereabouts;
            _headDirection = _whereabouts.Direction;

            _headRotation.Add(Direction.West,
                              Matrix.RotationY(MathUtil.PiOverTwo) * Matrix.RotationY(MathUtil.Pi));
            _headRotation.Add(Direction.East,
                              Matrix.RotationY(MathUtil.PiOverTwo));
            _headRotation.Add(Direction.South,
                              Matrix.RotationY(MathUtil.PiOverTwo) * Matrix.RotationY(MathUtil.PiOverTwo));
            _headRotation.Add(Direction.North,
                              Matrix.RotationY(MathUtil.PiOverTwo) * Matrix.RotationY(-MathUtil.PiOverTwo));

            _tail = new SerpentTailSegment(_pf, _whereabouts);
            _serpentLength = 1;

            _layingEgg = (float)(-5 - new Random().NextDouble()*30);
        }

        protected virtual float modifySpeed()
        {
            return 1f;
        }

        public virtual void Update(GameTime gameTime)
        {
            var lengthSpeed = (11 - _serpentLength)/10f;
            var speed = (float) gameTime.ElapsedGameTime.TotalMilliseconds*0.0045f*lengthSpeed*modifySpeed();

            if (_whereabouts.Direction != Direction.None)
            {
                _fractionAngle += speed;
                if (_fractionAngle >= 1)
                {
                    _fractionAngle = 0;
                    _whereabouts.Location = _whereabouts.NextLocation;
                    takeDirection();
                }
                _whereabouts.Fraction = (float) Math.Sin(_fractionAngle*MathUtil.PiOverTwo);
            }
            else
                takeDirection();

            if (_tail != null)
                _tail.Update(gameTime, GetPosition(), speed);

            if (_whereabouts.Direction != Direction.None)
                _headDirection = _whereabouts.Direction;
        }


        protected bool tryMove(Direction dir)
        {
            if (dir == Direction.None)
                return false;
            var possibleLocationTo = _whereabouts.Location.Add(dir);
            if (!_pf.CanMoveHere(ref _whereabouts.Floor, _whereabouts.Location, possibleLocationTo))
                return false;
            _whereabouts.Direction = dir;
            _tail.AddPathToWalk(_whereabouts);
            return true;
        }

        public virtual void Draw(GameTime gameTime)
        {
            var p = GetPosition();

//            _effect.DiffuseColor = Vector4.Lerp(new Vector4(0.5f, 0.5f, 0.5f, 1), tintColor(), 0.5f);
//            _effect.Alpha = SerpentStatus == SerpentStatus.Alive ? 1 : 0.5f;
            _effect.View = _camera.Camera.View;
            _effect.Projection = _camera.Camera.Projection;
            _effect.World = _headRotation[_headDirection]*
                            Matrix.Scaling(0.5f)*
                            Matrix.Translation(p.X, 0.4f + p.Y, p.Z);
            _sphere.Draw(_effect);

            var worlds = new List<Matrix>();

            var segment = _tail;
            while (true)
            {
                var p2 = segment.GetPosition();
                worlds.Add(
                    Matrix.Scaling(0.4f)*
                    Matrix.Translation(
                        (p.X + p2.X)/2,
                        0.3f + (p.Y + p2.Y)/2,
                        (p.Z + p2.Z)/2));
                worlds.Add(
                    Matrix.Scaling(0.4f)*
                    Matrix.Translation(
                        p2.X,
                        0.3f + p2.Y,
                        p2.Z));
                p = p2;
                if (segment.Next == null)
                    break;
                segment = segment.Next;
            }

            if (_pendingEatenSegments <= SegmentEatTreshold/2)
                worlds.RemoveAt(worlds.Count - 1);

            //TODO
            //if (_layingEgg > -500)
            //{
            //    var d = segment.Whereabouts.Direction;
            //    var t = d == Direction.North || d == Direction.South
            //        ? Matrix.Scaling(0.6f, 0.6f, 0.8f)
            //        : Matrix.Scaling(0.8f, 0.6f, 0.6f);
            //    var off = d.DirectionAsVector2()*(-0.3f);
            //    t *= Matrix.Translation(off.X, -0.3f, off.Y);
            //    _modelSegment.Draw(
            //        _camera.Camera,
            //        t*worlds[worlds.Count - 1],
            //        Vector4.One,
            //        1,
            //        1);
            //    _modelSegment.Draw(
            //        _camera.Camera,
            //        worlds[worlds.Count - 1],
            //        tintColor(),
            //        0.5f,
            //        0.9f);
            //    worlds.RemoveAt(worlds.Count - 1);
            //}

            foreach (var world in worlds)
            {
                _effect.World = world;
                _sphere.Draw(_effect);
            }

            //_camera.Camera,
                    //world,
                    //tintColor(),
                    //0.5f,
                    //SerpentStatus == SerpentStatus.Alive ? 1 : 0.5f);
        }

        protected virtual Vector4 tintColor()
        {
            return Vector4.Zero;
        }

        public Vector3 GetPosition()
        {
            return _whereabouts.GetPosition(_pf);
        }

        private void grow(int length)
        {
            _pendingEatenSegments += length;
            for (var count = _pendingEatenSegments / SegmentEatTreshold; count > 0; count--)
                addTail();
            _pendingEatenSegments %= SegmentEatTreshold;
        }

        protected bool _isLonger;

        public bool EatAt(BaseSerpent other)
        {
            _isLonger = _serpentLength >= other._serpentLength;
            if (SerpentStatus != SerpentStatus.Alive)
                return false;
            if (Vector3.DistanceSquared(GetPosition(), other.GetPosition()) < 0.8f)
            {
                if (other._serpentLength > _serpentLength)
                    return false;
                grow(other._serpentLength + 1);
                return true;
            }
            for (var tail = other._tail; tail != null; tail = tail.Next)
                if (Vector3.DistanceSquared(GetPosition(), tail.GetPosition()) < 0.2f)
                {
                    if (tail == other._tail)
                    {
                        grow(other._serpentLength + 1);
                        return true;
                    }
                    grow(other.removeTail(tail));
                    return false;
                }
            return false;
        }

        private int removeTail(SerpentTailSegment tail)
        {
            _pendingEatenSegments = SegmentEatTreshold - 1;
            if (tail != _tail && tail != null)
            {
                var length = 1;
                for (var test = _tail; test != null; test = test.Next, length++)
                    if (test.Next == tail)
                    {
                        test.Next = null;
                        var removedSegments = _serpentLength - length;
                        _serpentLength = length;
                        return removedSegments;
                    }
            }
            throw new Exception();
        }

        protected void addTail()
        {
            var tail = _tail;
            while (tail.Next != null)
                tail = tail.Next;
            tail.Next = new SerpentTailSegment(_pf, tail.Whereabouts);
            _serpentLength++;
        }

    }

}
