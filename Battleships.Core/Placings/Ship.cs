using Battleships.Core.Boards;

namespace Battleships.Core.Placings
{
    public class Ship
    {
        public IReadOnlySet<Position> OccupiedPositions { get; }

        public Ship(Position start, Position end)
        {
            // cells are numbered from top to bottom and left to right
            var topLeft = new Position(
                Math.Min(start.Row, end.Row),
                Math.Min(start.Column, end.Column)
                );

            var bottomRight = new Position(
                Math.Max(start.Row, end.Row),
                Math.Max(start.Column, end.Column)
                );

            OccupiedPositions = (
                from row in Enumerable.Range(topLeft.Row, bottomRight.Row - topLeft.Row + 1)
                from column in Enumerable.Range(topLeft.Column, bottomRight.Column - topLeft.Column + 1)
                select new Position(row, column)
                ).ToHashSet();
        }
    }
}
