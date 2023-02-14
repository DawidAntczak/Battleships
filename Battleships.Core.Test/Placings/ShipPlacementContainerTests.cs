using Battleships.Core.Boards;
using Battleships.Core.Placings;

namespace Battleships.Core.Test.Placings
{
    [TestFixture]
    public class ShipPlacementContainerTests
    {
        private ShipPlacementContainer _container;

        [SetUp]
        public void SetUp()
        {
            _container = new ShipPlacementContainer();
        }

        [Test]
        public void TryPlace_is_successful_for_empty_container()
        {
            // Arrange
            var ship = new Ship(new Position(0, 0), new Position(0, 3));

            // Act
            var result = _container.TryPlace(ship);

            // Assert
            result.Should().BeTrue();
            _container.Ships.Should().BeEquivalentTo(new[] { ship });
            _container.OccupiedPositions.Keys.Should().BeEquivalentTo(ship.OccupiedPositions);
        }

        [Test]
        public void TryPlace_is_successful_when_ships_do_not_collide()
        {
            // Arrange
            var ships = new Ship[] {
                new Ship(new Position(0, 0), new Position(0, 3)),
                new Ship(new Position(1, 0), new Position(1, 3)),
                new Ship(new Position(5, 5), new Position(7, 5))
            };

            // Act
            var results = ships.Select(_container.TryPlace).ToArray();

            // Assert
            results.Should().AllBeEquivalentTo(true);
            _container.Ships.Should().BeEquivalentTo(ships);
            _container.OccupiedPositions.Keys.Should().BeEquivalentTo(ships.SelectMany(x => x.OccupiedPositions));
        }

        [Test]
        public void TryPlace_is_not_successful_when_ships_collide()
        {
            // Arrange
            var validShip = new Ship(new Position(0, 0), new Position(0, 3));
            _container.TryPlace(validShip);
            var collidingShip = new Ship(new Position(0, 3), new Position(0, 6));

            // Act
            var result = _container.TryPlace(collidingShip);

            // Assert
            result.Should().BeFalse();
            _container.Ships.Should().BeEquivalentTo(new[] { validShip });
            _container.OccupiedPositions.Keys.Should().BeEquivalentTo(validShip.OccupiedPositions);
        }
    }
}
