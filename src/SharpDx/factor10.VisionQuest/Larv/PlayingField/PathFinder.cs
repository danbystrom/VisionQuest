using Serpent;
using SharpDX;

namespace Larv
{
    public class PathFinder
    {
        public readonly int[,,] Distance;
        public PlayingField PlayingField;

        public PathFinder(PlayingField pf, Whereabouts home)
        {
            PlayingField = pf;
            Distance = new int[pf.TheField.GetUpperBound(0)+1, pf.TheField.GetUpperBound(1)+1, pf.TheField.GetUpperBound(2)+1];
            Explore(home.Floor, home.Location, 1, Direction.None);

            //for (var y = 0; y < pf.TheField.GetUpperBound(1) + 1; y++)
            //{
            //    var s = "";
            //    for (var x = 0; x < pf.TheField.GetUpperBound(2) + 1; x++)
            //    {
            //        s += string.Format("{0,4}", Distance[0, y, x]);
            //    }
            //    System.Diagnostics.Debug.Print(s);
            //}
        }

        public void Explore(int floor, Point fromLoc, int distance, Direction direction)
        {
            var toLoc = fromLoc.Add(direction.DirectionAsPoint());
            if (!PlayingField.CanMoveHere(ref floor, fromLoc, toLoc, true))
                return;

            var here = Distance[floor, toLoc.Y, toLoc.X];
            if (here != 0 && here < distance)
                return; // already know shorter path

            Distance[floor, toLoc.Y, toLoc.X] = distance;

            foreach (var newDirection in Direction.AllDirections)
                Explore(floor, toLoc, distance + 1, newDirection);
        }

        private int getDistance(int floor, Point fromLoc, Direction direction)
        {
            var toLoc = fromLoc.Add(direction.DirectionAsPoint());
            return PlayingField.CanMoveHere(ref floor, fromLoc, toLoc, true)
                ? Distance[floor, toLoc.Y, toLoc.X]
                : int.MaxValue;
        }

        private void testDistance(int floor, Point fromLoc, ref Direction bestDirection, ref int bestDistance, Direction direction)
        {
            var here = getDistance(floor, fromLoc, direction);
            if (here >= bestDistance)
                return;
            bestDirection = direction;
            bestDistance = here;
        }

        public Direction WayHome(Whereabouts whereabouts, bool canTurnAround)
        {
            if (getDistance(whereabouts.Floor, whereabouts.Location, Direction.None) <= 2)
                return Direction.None; // is home!

            var bestDirection = whereabouts.Direction.Turn(RelativeDirection.Backward);
            var bestDistance = int.MaxValue;
            foreach (var direction in Direction.AllDirections)
                if (canTurnAround || direction != bestDirection)
                    testDistance(whereabouts.Floor, whereabouts.Location, ref bestDirection, ref bestDistance, direction);
            return bestDirection;
        }

    }

}


