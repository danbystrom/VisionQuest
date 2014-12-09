using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX.Toolkit;

namespace Larv.Util
{
    public abstract class ToDoBase
    {
        public class TimedAction
        {
            public float Time;
            public readonly Func<float, bool> Action;
            public readonly ToDoBase ToDoBase;

            public TimedAction(ToDoBase toDoBase, Func<float, bool> action)
            {
                ToDoBase = toDoBase;
                Action = action;
            }

            public virtual bool Do()
            {
                return Action(Time);
            }

        }

        protected readonly List<TimedAction> Actions = new List<TimedAction>();

        public abstract bool Do(GameTime gameTime);

        public void InsertNext(params Func<float, bool>[] actions)
        {
            Actions.InsertRange(0, actions.Select(_ => new TimedAction(this, _)));
        }

        public void Add(TimedAction timedAction)
        {
            Actions.Add(timedAction);
        }

        public void Add(Func<float, bool> action)
        {
            Add(new TimedAction(this, action));
        }

        public void Add(float timeToWait, Func<float, bool> action)
        {
            Add(timeToWait);
            Add(action);
        }

        public void Add(float timeToWait)
        {
            Add(time => time <= timeToWait);
        }

        public void Add(Action action)
        {
            Add(time =>
            {
                action();
                return false;
            });
        }

        public void Add(float timeToWait, Action action)
        {
            Add(timeToWait);
            Add(action);
        }

    }

}
