using Battleships.Core.Boards;
using Battleships.Core.Placings;

namespace Battleships.Core.Test.Boards
{
    [TestFixture]
    public class BoardTests
    {
        private Mock<IShipPlacementContainer> _shipPlacementContainer;
        private Mock<IPlacingStrategy> _placingStrategy;

        private Dictionary<Position, Ship> _placedShips;

        private Board _board;

        [SetUp]
        public void SetUp()
        {
            _placedShips = new Dictionary<Position, Ship>();

            _shipPlacementContainer = new Mock<IShipPlacementContainer>();
            _shipPlacementContainer
                .Setup(x => x.OccupiedPositions)
                .Returns(() => _placedShips);
            _shipPlacementContainer
                .Setup(x => x.Ships)
                .Returns(() => _placedShips.Values.Distinct());

            _placingStrategy = new Mock<IPlacingStrategy>();
            _placingStrategy
                .Setup(x => x.PlaceShips())
                .Returns(() => _shipPlacementContainer.Object);

            _board = new Board(_placingStrategy.Object);
        }

        [Test]
        public void Shoot_on_uninitialized_board_throws_GameRulesViolationException()
        {
            // Arrange
            var validPosition = new Position(1, 1);

            // Act & Assert
            _board.CanShoot(validPosition).Should().BeFalse();
            _board.State.Should().Be(BoardState.Uninitialized);
            Assert.Throws<GameRulesViolationException>(() => _board.Shoot(validPosition));
        }

        [Test]
        public void Shoot_for_position_outside_of_board_throws_GameRulesViolationException()
        {
            // Arrange
            var invalidPosition = new Position(666, 666);
            _board.Initialize();

            // Act & Assert
            _board.CanShoot(invalidPosition).Should().BeFalse();
            Assert.Throws<GameRulesViolationException>(() => _board.Shoot(invalidPosition));
        }

        [Test]
        public void Shoot_for_already_hit_position_throws_GameRulesViolationException()
        {
            // Arrange
            var position = new Position(1, 1);
            _board.Initialize();

            // Act
            _board.Shoot(position);

            // Assert
            Assert.Throws<GameRulesViolationException>(() => _board.Shoot(position));
        }

        [Test]
        public void Shoot_not_on_target_notices_miss()
        {
            // Arrange
            var ship = new Ship(new Position(1, 1), new Position(1, 2));
            AddShipToContainer(ship);
            var missedPosition = new Position(1, 3);
            _board.Initialize();

            // Act
            _board.Shoot(missedPosition);

            // Assert
            _board.Cells[missedPosition].Type.Should().Be(CellType.MissedShot);
        }

        [Test]
        public void Shoot_on_target_notices_hit()
        {
            // Arrange
            var ship = new Ship(new Position(1, 1), new Position(1, 2));
            AddShipToContainer(ship);
            var someShipPosition = ship.OccupiedPositions.First();
            _board.Initialize();

            // Act
            _board.Shoot(someShipPosition);

            // Assert
            _board.Cells[someShipPosition].Type.Should().Be(CellType.HitShip);
        }

        [Test]
        public void Shoot_on_all_ship_parts_notices_sinking()
        {
            // Arrange
            var ship = new Ship(new Position(1, 1), new Position(1, 3));
            AddShipToContainer(ship);
            _board.Initialize();

            // Act
            BulkShoot(ship.OccupiedPositions);

            // Assert
            var occupiedPositionCellTypes = ship.OccupiedPositions.Select(position => _board.Cells[position].Type);
            occupiedPositionCellTypes.Should().AllBeEquivalentTo(CellType.SunkShip);
        }

        [Test]
        public void Shoot_on_last_part_of_last_ship_switches_to_finished_state()
        {
            // Arrange
            var ship1 = new Ship(new Position(1, 1), new Position(1, 3));
            var ship2 = new Ship(new Position(2, 1), new Position(2, 3));

            AddShipToContainer(ship1);
            AddShipToContainer(ship2);

            _board.Initialize();

            var allShipPositions = ship1.OccupiedPositions.Concat(ship2.OccupiedPositions);

            // prepare a board with only one hit to finish
            BulkShoot(allShipPositions.SkipLast(1));

            _board.ShipsLeft.Should().Be(1);
            _board.State.Should().Be(BoardState.Initialized);

            // Act
            _board.Shoot(allShipPositions.Last());

            // Assert
            _board.ShipsLeft.Should().Be(0);
            _board.State.Should().Be(BoardState.Finished);
        }

        [Test]
        public void Initialize_clears_cells_to_undiscovered()
        {
            // Arrange
            var position = new Position(1, 1);
            _board.Initialize();

            // Act
            _board.Shoot(position);
            _board.Initialize();

            // Assert
            _board.Cells[position].Type.Should().Be(CellType.Undiscovered);
        }

        [Test]
        public void Initialize_clears_ShipsLeft_and_State()
        {
            // Arrange
            var ship = new Ship(new Position(1, 1), new Position(1, 3));

            AddShipToContainer(ship);

            _board.Initialize();

            // prepare a finished board
            BulkShoot(ship.OccupiedPositions);

            _board.ShipsLeft.Should().Be(0);
            _board.State.Should().Be(BoardState.Finished);

            // Act
            _board.Initialize();

            // Assert
            _board.ShipsLeft.Should().Be(1);
            _board.State.Should().Be(BoardState.Initialized);
        }

        #region Helpers

        private void AddShipToContainer(Ship ship)
        {
            foreach (var position in ship.OccupiedPositions)
            {
                _placedShips.Add(position, ship);
            }
        }

        private void BulkShoot(IEnumerable<Position> positions)
        {
            foreach (var position in positions)
            {
                _board.Shoot(position);
            }
        }

        #endregion Helpers
    }
}
