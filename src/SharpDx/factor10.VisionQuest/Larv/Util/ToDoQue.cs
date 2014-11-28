using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX.Toolkit;

namespace Larv.Util
{
    public class ToDoQue
    {
        private readonly List<Func<float, bool>> _actions = new List<Func<float, bool>>();
        private float _time;

        public bool Do(GameTime gameTime)
        {
            _time += (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (_actions.Any() && !_actions[0](_time))
            {
                _time = 0;
                _actions.RemoveAt(0);
            }
            return _actions.Any();
        }

        public void Add(Func<float, bool> action)
        {
            if (!_actions.Any())
                _time = 0;
            _actions.Add(action);
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
