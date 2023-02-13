using Battleships.Core.Boards;
using Battleships.Core.Ships;

namespace Battleships.Core.Placings
{
    internal class RandomPlacingStrategy : IPlacingStrategy
    {
        private readonly IRandomNumberGenerator _randomNumberGenerator;

        public RandomPlacingStrategy(IRandomNumberGenerator randomNumberGenerator)
        {
            _randomNumberGenerator = randomNumberGenerator;
        }

        public ShipPlacement PlaceShips()
        {
            var shipPlacement = new ShipPlacement();

            var _shipSizesToGenerate = new int[] { 5, 4, 4 };
            foreach (var shipSize in _shipSizesToGenerate)
            {
                PlaceShip(shipSize, shipPlacement);
            }

            return shipPlacement;
        }

        private void PlaceShip(int shipSize, ShipPlacement shipPlacement)
        {
            bool placed = false;
            
            while (!placed)
            {
                var startPosition = PickRandomFreePosition(shipPlacement);
                var shuffledEndPositions = Shuffle(CalculateEndPositionsInBoardRange(startPosition, shipSize)).GetEnumerator();

                using var endPositionsEnumerator = shuffledEndPositions;
                while (!placed && endPositionsEnumerator.MoveNext())
                {
                    placed = shipPlacement.TryPlace(new Ship(startPosition, endPositionsEnumerator.Current));
                }
            }
        }

        private Position PickRandomFreePosition(ShipPlacement shipPlacement)
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
            var offsets = new[] { (reducedSize, 0), (0, reducedSize), (-reducedSize, 0), (0, -reducedSize) };

            return offsets
                .Select(offset => startPosition.Translated(offset.Item1, offset.Item2))
                .Where(IsPositionInBoardRange);
        }

        private bool IsPositionInBoardRange(Position position)
        {
            // idk but should be probably shifted to Board class
            return position.Row >= 0 && position.Row < Board.Rows && position.Column >= 0 && position.Column < Board.Columns;
        }

        private IEnumerable<T> Shuffle<T>(IEnumerable<T> enumerable)
        {
            return enumerable.OrderBy(x => _randomNumberGenerator.Generate());
        }
    }
}
