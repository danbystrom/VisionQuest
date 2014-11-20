using System;
using System.Collections.Generic;
using System.Linq;
using factor10.VisionThing;
using factor10.VisionThing.Effects;
using Serpent;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace Larv.Serpent
{
    public enum SerpentStatus
    {
        Alive,
        Ghost,
        Finished
    }

    public abstract class BaseSerpent : ClipDrawable
    {
        public const float HeadSize = 0.5f;
        public const float SegmentSize = 0.4f;

        public SerpentStatus SerpentStatus;

        protected readonly PlayingField _pf;

        public Whereabouts _whereabouts = new Whereabouts();
        protected Direction _headDirection;

        protected double _fractionAngle;

        protected readonly IVDrawable _sphere;
        protected readonly Texture2D _serpentSkin;
        protected readonly Texture2D _serpentBump;
        protected readonly Texture2D _eggSkin;
        //protected readonly EffectParameter _diffuseParameter;

        protected SerpentTailSegment _tail;
        protected int _serpentLength;

        protected readonly Dictionary<Direction, Matrix> _headRotation = new Dictionary<Direction, Matrix>();

        protected abstract void takeDirection();

        private int _pendingEatenSegments = 6;
        private const int SegmentEatTreshold = 7;

        private float _layingEgg = -1;
        private const float TimeForLayingEggProcess = 10;
        private Matrix _eggWorld;

        private float _ascendToHeaven;

        protected BaseSerpent(
            VisionContent vContent,
            PlayingField pf,
            IVDrawable sphere,
            Whereabouts whereabouts,
            Texture2D serpentSerpentSkin,
            Texture2D serpentBump,
            Texture2D eggSkin) : base(new VisionEffect(vContent.Load<Effect>(@"Effects\SimpleBumpEffect")))
        {
            _pf = pf;
            Restart(whereabouts);
            _sphere = sphere;
            _serpentSkin = serpentSerpentSkin;
            _serpentBump = serpentBump;
            _eggSkin = eggSkin;

            _headRotation.Add(Direction.West,
                              Matrix.RotationY(MathUtil.PiOverTwo) * Matrix.RotationY(MathUtil.Pi));
            _headRotation.Add(Direction.East,
                              Matrix.RotationY(MathUtil.PiOverTwo));
            _headRotation.Add(Direction.South,
                              Matrix.RotationY(MathUtil.PiOverTwo) * Matrix.RotationY(MathUtil.PiOverTwo));
            _headRotation.Add(Direction.North,
                              Matrix.RotationY(MathUtil.PiOverTwo) * Matrix.RotationY(-MathUtil.PiOverTwo));
        }

        protected void Restart(Whereabouts whereabouts)
        {
            _whereabouts = whereabouts;
            _headDirection = _whereabouts.Direction;
            _tail = new SerpentTailSegment(_pf, _whereabouts);
            _serpentLength = 1;
            _ascendToHeaven = 0;
            _layingEgg = -1;
            _pendingEatenSegments = 6;
        }

        protected virtual float modifySpeed()
        {
            return 1f;
        }

        public override void Update(Camera camera, GameTime gameTime)
        {
            switch (SerpentStatus)
            {
                case SerpentStatus.Ghost:
                    _ascendToHeaven += (float) gameTime.ElapsedGameTime.TotalSeconds;
                    if (_ascendToHeaven > 5)
                        SerpentStatus = SerpentStatus.Finished;
                    return;
                case SerpentStatus.Finished:
                    return;
            }

            var lengthSpeed = Math.Max(0.001f, (11 - _serpentLength)/10f);
            var speed = (float) gameTime.ElapsedGameTime.TotalSeconds*5.5f*lengthSpeed*modifySpeed();

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
                _tail.Update(speed, _whereabouts);

            if (_whereabouts.Direction != Direction.None)
                _headDirection = _whereabouts.Direction;

            if (_layingEgg >= 0)
                _layingEgg += (float) gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected bool TryMove(Direction dir, bool ignoreRestriction = false)
        {
            if (dir == Direction.None)
                return false;
            var possibleLocationTo = _whereabouts.Location.Add(dir);
            if (!_pf.CanMoveHere(ref _whereabouts.Floor, _whereabouts.Location, possibleLocationTo, ignoreRestriction))
                return false;
            _whereabouts.Direction = dir;
            _tail.AddPathToWalk(_whereabouts);
            return true;
        }

        private static Vector3 wormTwist(ref float slinger)
        {
            slinger += 1.5f;
            return new Vector3((float)Math.Sin(slinger) * 0.2f, 0, (float)Math.Sin(slinger) * 0.2f);
        }

        protected override bool draw(Camera camera, DrawingReason drawingReason, ShadowMap shadowMap)
        {
            var p = GetPosition();

            var slinger = p.X + p.Z;
            p += wormTwist(ref slinger);

            camera.UpdateEffect(Effect);

            var worlds = new List<Matrix>
            {
                _headRotation[_headDirection]*
                Matrix.Scaling(HeadSize)*
                Matrix.Translation(p.X, HeadSize + p.Y + _ascendToHeaven, p.Z)
            };

            // p is the the loc of the last segement - which is the head on the first round
            var segment = _tail;
            while (true)
            {
                var p2 = segment.GetPosition() + wormTwist(ref slinger);
                worlds.Add(
                    Matrix.Scaling(SegmentSize) *
                    Matrix.Translation(
                        (p.X + p2.X)/2,
                        SegmentSize + (p.Y + p2.Y) / 2 + _ascendToHeaven,
                        (p.Z + p2.Z)/2));
                worlds.Add(
                    Matrix.Scaling(SegmentSize)*
                    Matrix.Translation(
                        p2.X,
                        SegmentSize + p2.Y + _ascendToHeaven,
                        p2.Z));
                p = p2;
                if (segment.Next == null)
                    break;
                segment = segment.Next;
            }

            if (_pendingEatenSegments <= SegmentEatTreshold/2)
                worlds.RemoveAt(worlds.Count - 1);

            if (_layingEgg > 0)
            {
                Effect.Texture = _eggSkin;
                _eggWorld = worlds[worlds.Count - 1];
                Egg.Draw(Effect, _eggSkin, _sphere, _eggWorld, segment.Whereabouts.Direction);

                //move the last two  so that they slowly dissolves
                var factor = MathUtil.Clamp(_layingEgg/TimeForLayingEggProcess - 0.5f, 0, 1);
                var world1 = worlds.Last();
                worlds.RemoveAt(worlds.Count - 1);
                var world2 = worlds.Last();
                worlds.RemoveAt(worlds.Count - 1);
                var world3 = worlds.Last();
                world2.TranslationVector = Vector3.Lerp(world2.TranslationVector, world3.TranslationVector, factor);
                world1.TranslationVector = Vector3.Lerp(world1.TranslationVector, world2.TranslationVector, factor);
                worlds.Add(world2);
                worlds.Add(world1);
            }

            Effect.Texture = _serpentSkin;
            Effect.Parameters["BumpMap"].SetResource(_serpentBump);
            Effect.DiffuseColor = TintColor();
            foreach (var world in worlds)
                drawSphere(world);

            Effect.DiffuseColor = Vector4.One;
            return true;
        }

        private void drawSphere(Matrix world)
        {
            Effect.World = world;
            _sphere.Draw(Effect);
        }

        protected virtual Vector4 TintColor()
        {
            return Vector4.Zero;
        }

        protected float AlphaValue()
        {
            return SerpentStatus == SerpentStatus.Ghost
                ? MathUtil.Clamp(0.8f - _ascendToHeaven/2, 0, 1)
                : 1;
        }

        public Vector3 GetPosition()
        {
            return _whereabouts.GetPosition(_pf);
        }

        public Whereabouts Whereabouts
        {
            get { return _whereabouts; }    
        }

        private void grow(int length)
        {
            _pendingEatenSegments += length;
            for (var count = _pendingEatenSegments / SegmentEatTreshold; count > 0; count--)
                AddTail();
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
            throw new Exception("No tail to remove");
        }

        public void AddTail()
        {
            var tail = _tail;
            while (tail.Next != null)
                tail = tail.Next;
            tail.Next = new SerpentTailSegment(_pf, tail.Whereabouts);
            _serpentLength++;
        }

        public Egg TimeToLayEgg()
        {
            if (_layingEgg < TimeForLayingEggProcess)
                return null;
            _layingEgg = -1;

            var segment = _tail;
            while (segment.Next != null)
                segment = segment.Next;
            if (segment != _tail)
                removeTail(segment);
            else
                SerpentStatus = SerpentStatus.Ghost;
            return new Egg(Effect, _sphere, _eggSkin, _eggWorld, segment.Whereabouts, this is PlayerSerpent ? float.MaxValue : 20);
        }

        public void Fertilize()
        {
            if (_layingEgg < 0)
                _layingEgg = 0;
        }

        public bool IsPregnant
        {
            get { return _layingEgg >= 0; }    
        }

        public int Length
        {
            get
            {
                var length = 0;
                for (var segment = _tail; segment.Next != null; segment = segment.Next)
                    length++;
                return length;
            }            
        }

        public bool EatEgg(Egg egg)
        {
            if (egg==null || Vector3.DistanceSquared(GetPosition(), egg.Position) > 0.3f)
                return false;
            AddTail();
            return true;
        }
        
    }

}
