using Battleships.Core.Boards;
using Battleships.Core.Placings;

namespace Battleships.Core.Test.Boards
{
    [TestFixture]
    public class BoardTests
    {
        private Mock<IShipPlacementContainer> _shipPlacementContainer;

        private Dictionary<Position, Ship> _placedShips;

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
        }


        [Test]
        public void Constructor_clears_cells_to_undiscovered()
        {
            // Arrange
            var board = new Board(_shipPlacementContainer.Object);

            // Assert
            board.Cells.Values.Select(x => x.Type).Should().AllBeEquivalentTo(CellType.Undiscovered);
        }

        [Test]
        public void Constructor_counts_ships()
        {
            // Arrange
            var ships = new Ship[] {
                new Ship(new Position(0, 0), new Position(0, 3)),
                new Ship(new Position(1, 0), new Position(1, 3)),
                new Ship(new Position(5, 5), new Position(7, 5))
            };
            AddShipsToContainer(ships);

            var board = new Board(_shipPlacementContainer.Object);

            // Assert
            board.ShipsLeft.Should().Be(ships.Length);
        }

        [Test]
        public void AddShot_not_on_target_notices_miss()
        {
            // Arrange
            var ship = new Ship(new Position(1, 1), new Position(1, 2));
            AddShipsToContainer(ship);
            var missedPosition = new Position(1, 3);

            var board = new Board(_shipPlacementContainer.Object);

            // Act
            board.AddShot(missedPosition);

            // Assert
            board.Cells[missedPosition].Type.Should().Be(CellType.MissedShot);
        }

        [Test]
        public void AddShot_on_target_notices_hit()
        {
            // Arrange
            var ship = new Ship(new Position(1, 1), new Position(1, 2));
            AddShipsToContainer(ship);
            var someShipPosition = ship.OccupiedPositions.First();

            var board = new Board(_shipPlacementContainer.Object);

            // Act
            board.AddShot(someShipPosition);

            // Assert
            board.Cells[someShipPosition].Type.Should().Be(CellType.HitShip);
        }

        [Test]
        public void AddShot_on_all_ship_parts_notices_sinking_for_all_parts()
        {
            // Arrange
            var ship = new Ship(new Position(1, 1), new Position(1, 3));
            AddShipsToContainer(ship);

            var board = new Board(_shipPlacementContainer.Object);

            // Act
            foreach (var position in ship.OccupiedPositions)
            {
                board.AddShot(position);
            }

            // Assert
            var occupiedPositionCellTypes = ship.OccupiedPositions.Select(position => board.Cells[position].Type);
            occupiedPositionCellTypes.Should().AllBeEquivalentTo(CellType.SunkShip);
            board.ShipsLeft.Should().Be(0);
        }

        #region Helpers

        private void AddShipsToContainer(params Ship[] ships)
        {
            foreach (var ship in ships)
            {
                foreach (var position in ship.OccupiedPositions)
                {
                    _placedShips.Add(position, ship);
                }
            }
        }

        #endregion Helpers
    }
}
