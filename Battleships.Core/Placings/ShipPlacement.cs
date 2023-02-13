using Battleships.Core.Boards;
using Battleships.Core.Ships;

namespace Battleships.Core.Placings
{
    public class ShipPlacement
    {
        public IDictionary<Position, Ship> OccupiedPositions { get; private set; } = new Dictionary<Position, Ship>();

        public bool TryPlace(Ship ship)
        {
            if (CheckIfCanPlace(ship))
            {
                Place(ship);
                return true;
            }
            return false;
        }
        private bool CheckIfCanPlace(Ship ship)
        {
            // if no occupied position intersects
            return !OccupiedPositions.Keys.Intersect(ship.OccupiedPositions).Any();
        }

        private void Place(Ship ship)
        {
            foreach (var position in ship.OccupiedPositions)
            {
                OccupiedPositions.Add(position, ship);
            }
        }
    }
}
