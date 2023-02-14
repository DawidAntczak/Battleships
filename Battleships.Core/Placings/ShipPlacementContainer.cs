using Battleships.Core.Boards;

namespace Battleships.Core.Placings
{
    internal class ShipPlacementContainer : IShipPlacementContainer
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
            return !_occupiedPositions.Keys.Intersect(ship.OccupiedPositions).Any();    // no ship position intersects with already occupied positions
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
