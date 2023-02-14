using Battleships.Core.Boards;

namespace Battleships.Core.Placings
{
    public interface IShipPlacementContainer
    {
        public IReadOnlyDictionary<Position, Ship> OccupiedPositions { get; }
        public IEnumerable<Ship> Ships { get; }
    }
}
