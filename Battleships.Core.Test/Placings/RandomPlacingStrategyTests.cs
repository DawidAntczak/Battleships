using Battleships.Core.Boards;
using Battleships.Core.Placings;

namespace Battleships.Core.Test.Placings
{
    [TestFixture]
    public class RandomPlacingStrategyTests
    {
        // because this placing strategy is based on RNG, every test is repeated for seeds in range [0, TestRepetitions)
        private const int TestRepetitions = 100;

        private Random _random;
        private Mock<IRandomNumberGenerator> _randomNumberGenerator;

        private RandomPlacingStrategy _randomPlacingStrategy;

        [SetUp]
        public void SetUp()
        {
            var seed = TestContext.CurrentContext.CurrentRepeatCount;
            _random = new Random(seed);

            _randomNumberGenerator = new Mock<IRandomNumberGenerator>();
            _randomNumberGenerator
                .Setup(x => x.Generate())
                .Returns(_random.Next());
            _randomNumberGenerator
                .Setup(x => x.Generate(It.IsAny<int>(), It.IsAny<int>()))
                .Returns<int, int>((start, end) => _random.Next(start, end));

            _randomPlacingStrategy = new RandomPlacingStrategy(_randomNumberGenerator.Object);
        }

        [Test]
        [Repeat(TestRepetitions)]
        public void PlaceShips_returns_ships_of_sizes_5_4_4()
        {
            // Act
            var shipPlacement = _randomPlacingStrategy.PlaceShips();

            // Assert
            var shipSizes = shipPlacement.Ships.Select(s => s.OccupiedPositions.Count);
            shipSizes.Should().BeEquivalentTo(new[] { 5, 4, 4 });
        }


        [Test]
        [Repeat(TestRepetitions)]
        public void PlaceShips_returns_no_out_of_bound_ship()
        {
            // Act
            var shipPlacement = _randomPlacingStrategy.PlaceShips();

            // Assert
            var occupiedPositions = shipPlacement.OccupiedPositions.Keys;
            occupiedPositions.Should().AllSatisfy(x => Board.IsValidBoardPosition(x));
        }


        [Test]
        [Repeat(TestRepetitions)]
        public void PlaceShips_returns_different_results_every_call()
        {
            // Act
            var firstShipPlacement = _randomPlacingStrategy.PlaceShips();
            var secondShipPlacement = _randomPlacingStrategy.PlaceShips();

            // Assert
            var occupiedPositionsIntersection = firstShipPlacement.OccupiedPositions.Intersect(secondShipPlacement.OccupiedPositions);
            occupiedPositionsIntersection.Count().Should().BeLessThan(firstShipPlacement.OccupiedPositions.Count);
        }
    }
}
