using System;
using SharpDX;
using SharpDX.Toolkit;

namespace factor10.VisionThing
{
    public struct MovementTime
    {
        public readonly bool UnitsPerSecond;
        public readonly float Time;
        public MovementTime(float time, bool unitsPerSecond = false)
        {
            Time = time;
            UnitsPerSecond = unitsPerSecond;
        }

        public float GetTotalTime(float units)
        {
            // never return zero, since that may lead to division by zero later
            return Math.Max(!UnitsPerSecond ? Time : units/Time, 0.0000001f);
        }
    }

    public static class MovementTimeExtensionMethods
    {
        public static MovementTime Time(this float time)
        {
            return new MovementTime(time);
        }
        public static MovementTime UnitsPerSecond(this float time)
        {
            return new MovementTime(time, true);
        }
    }

    public abstract class MoveCameraBase : IVMoveable
    {
        protected readonly Camera Camera;

        protected readonly Vector3 FromLookAt;

        protected float EndTime;
        public float ElapsedTime { get; private set; }

        protected MoveCameraBase(Camera camera)
        {
            Camera = camera;
            FromLookAt = camera.Target;
        }

        public bool Move(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            ElapsedTime += dt;
            return MoveAround();
        }

        public bool Move(float absolutTime)
        {
            ElapsedTime = absolutTime;
            return MoveAround();
        }

        protected abstract bool MoveAround();
    }

}
