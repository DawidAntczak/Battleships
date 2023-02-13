using Battleships.Core.Boards;

namespace Battleships.Core.Placings
{
    public class ShipPlacementContainer
    {
        public IReadOnlyDictionary<Position, Ship> OccupiedPositions { get => _occupiedPositions; }
        public IEnumerable<Ship> Ships { get => _occupiedPositions.Values.Distinct(); }

        private readonly Dictionary<Position, Ship> _occupiedPositions = new();

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
            // if no ship position intersects with already occupied positions
            return !_occupiedPositions.Keys.Intersect(ship.OccupiedPositions).Any();
        }

        private void Place(Ship ship)
        {
            foreach (var position in ship.OccupiedPositions)
            {
                _occupiedPositions.Add(position, ship);
            }
        }
    }
}
