using Larv.Serpent;
using Larv.Util;

namespace Larv.GameStates
{
    internal class HomingDevice : ITakeDirection
    {
        private readonly PathFinder _playerPathFinder;
        private readonly PathFinder _enemyPathFinder;

        public static void Attach(Serpents serpents)
        {
            var hd = new HomingDevice(serpents);
            serpents.PlayerSerpent.DirectionTaker = hd;
            foreach (var enemy in serpents.Enemies)
                enemy.DirectionTaker = hd;
        }

        public HomingDevice(Serpents serpents)
        {
            _playerPathFinder = new PathFinder(serpents.PlayingField, serpents.PlayingField.PlayerWhereaboutsStart);
            _enemyPathFinder = new PathFinder(serpents.PlayingField, serpents.PlayingField.EnemyWhereaboutsStart);
        }

        RelativeDirection ITakeDirection.TakeDirection(BaseSerpent serpent)
        {
            var pathFinder = serpent is PlayerSerpent ? _playerPathFinder : _enemyPathFinder;
            var direction = pathFinder.WayHome(serpent.Whereabouts, false);
            return serpent.HeadDirection.GetRelativeDirection(direction);
        }

        bool ITakeDirection.CanOverrideRestrictedDirections(BaseSerpent serpent)
        {
            return true;
        }

    }

}
