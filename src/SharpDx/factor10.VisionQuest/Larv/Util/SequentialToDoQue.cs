using System.Linq;
using SharpDX.Toolkit;

namespace Larv.Util
{
    public class SequentialToDoQue : ToDoBase
    {
        private TimedAction _current;

        public override bool Do(GameTime gameTime)
        {
            if(_current==null)
                if (Actions.Any())
                {
                    _current = Actions[0];
                    Actions.RemoveAt(0);
                }
                else
                    return false;

            _current.Time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_current.Action(_current.Time))
                return true;
            _current = null;
            return Actions.Any();
        }

    }

}
