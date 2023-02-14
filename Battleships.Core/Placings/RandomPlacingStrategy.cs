using Battleships.Core.Boards;
using Battleships.Core.Placings.Extensions;

namespace Battleships.Core.Placings
{
    internal class RandomPlacingStrategy : IPlacingStrategy
    {
        private readonly IRandomNumberGenerator _randomNumberGenerator;

        public RandomPlacingStrategy(IRandomNumberGenerator randomNumberGenerator)
        {
            _randomNumberGenerator = randomNumberGenerator;
        }

        public IShipPlacementContainer PlaceShips()
        {
            var shipPlacement = new ShipPlacementContainer();

            var shipSizesToGenerate = new int[] { 5, 4, 4 };
            foreach (var shipSize in shipSizesToGenerate)
            {
                PlaceShip(shipSize, shipPlacement);
            }

            return shipPlacement;
        }

        private void PlaceShip(int shipSize, ShipPlacementContainer shipPlacement)
        {
            var placed = false;

            while (!placed)
            {
                var startPosition = PickRandomFreePosition(shipPlacement);
                var possibleEndPositions = CalculateEndPositionsInBoardRange(startPosition, shipSize);

                using var shuffledEndPositionsEnumerator = possibleEndPositions.Shuffle(_randomNumberGenerator).GetEnumerator();
                while (!placed && shuffledEndPositionsEnumerator.MoveNext())
                {
                    placed = shipPlacement.TryPlace(new Ship(startPosition, shuffledEndPositionsEnumerator.Current));
                }
            }
        }

        private Position PickRandomFreePosition(ShipPlacementContainer shipPlacement)
        {
            return RandomPositionOnBoardGenerator()
                .First(position => !shipPlacement.OccupiedPositions.ContainsKey(position));
        }

        private IEnumerable<Position> RandomPositionOnBoardGenerator()
        {
            while(true)
            {
                yield return new Position(
                    _randomNumberGenerator.Generate(0, Board.Rows),
                    _randomNumberGenerator.Generate(0, Board.Columns)
                    );
            }
        }

        private IEnumerable<Position> CalculateEndPositionsInBoardRange(Position startPosition, int shipSize)
        {
            var reducedSize = shipSize - 1;
            var offsets = new (int RowOffset, int ColumnOffset)[] {
                (reducedSize, 0), (0, reducedSize),
                (-reducedSize, 0), (0, -reducedSize)
            };

            return offsets
                .Select(offset => startPosition.Shifted(offset.RowOffset, offset.ColumnOffset))
                .Where(IsValidBoardPosition);
        }

        private bool IsValidBoardPosition(Position position)
            => position.Row >= 0
            && position.Row < Board.Rows
            && position.Column >= 0
            && position.Column < Board.Columns;
    }
}
