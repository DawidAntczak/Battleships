using Battleships.Core.Boards;
using Battleships.Core.Games;
using Battleships.Core.Placings;

namespace Battleships.Core.Test.Games
{

    [TestFixture]
    public class GameTest
    {
        private Mock<IPlacingStrategy> _placingStrategy;
        private Dictionary<Position, Cell> _cells;
        private Mock<IBoard> _board;
        private Mock<IBoardFactory> _boardFactory;

        private Game _game;

        [SetUp]
        public void SetUp()
        {
            _placingStrategy = new Mock<IPlacingStrategy>();

            _cells = (
                from row in Enumerable.Range(0, 10)
                from column in Enumerable.Range(0, 10)
                select new Cell(row, column, CellType.Undiscovered)
            ).ToDictionary(c => new Position(c.Row, c.Column), c => c);

            _board = new Mock<IBoard>();
            _board
                .SetupGet(x => x.Cells)
                .Returns(_cells);

            _boardFactory = new Mock<IBoardFactory>();
            _boardFactory
                .Setup(x => x.Create(It.IsAny<IShipPlacementContainer>()))
                .Returns(() => _board.Object);

            _game = new Game(_boardFactory.Object, _placingStrategy.Object);
        }

        [Test]
        public void Shoot_on_uninitialized_game_throws_GameRulesViolationException()
        {
            // Arrange
            var validPosition = new Position(1, 1);

            // Act & Assert
            _game.State.Should().Be(GameState.Uninitialized);
            _game.CanShoot(validPosition).Should().BeFalse();
            Assert.Throws<GameRulesViolationException>(() => _game.Shoot(validPosition));
        }

        [Test]
        public void StartNewGame_on_uninitialized_game_changes_state_to_started()
        {
            // Arrange
            _game.State.Should().Be(GameState.Uninitialized);

            // Act
            _game.StartNewGame();

            // Assert
            _game.State.Should().Be(GameState.Started);
        }

        [Test]
        public void Shoot_for_position_outside_of_board_throws_GameRulesViolationException()
        {
            // Arrange
            var invalidPosition = new Position(666, 666);
            _game.StartNewGame();

            // Act & Assert
            _game.CanShoot(invalidPosition).Should().BeFalse();
            Assert.Throws<GameRulesViolationException>(() => _game.Shoot(invalidPosition));
        }

        [TestCase(CellType.MissedShot)]
        public void Shoot_for_already_shot_position_throws_GameRulesViolationException(CellType cellType)
        {
            // Arrange
            var validPosition = new Position(1, 1);
            _game.StartNewGame();
            _cells[validPosition] = new Cell(validPosition.Row, validPosition.Column, cellType);

            // Act & Assert
            Assert.Throws<GameRulesViolationException>(() => _game.Shoot(validPosition));
        }

        [Test]
        public void Shoot_on_valid_position_for_started_game_adds_shot_to_board()
        {
            // Arrange
            var validPosition = new Position(1, 1);
            _game.StartNewGame();

            // Act & Assert
            _game.CanShoot(validPosition).Should().BeTrue();
            _game.Shoot(validPosition);
            _board.Verify(x => x.AddShot(validPosition), Times.Once);
        }

        [Test]
        public void Shoot_when_no_more_ships_switches_to_finished_state()
        {
            // Arrange
            var validPosition = new Position(1, 1);
            _game.StartNewGame();
            _board
                .SetupGet(x => x.ShipsLeft)
                .Returns(0);

            // Act
            _game.Shoot(validPosition);

            // Assert
            _game.State.Should().Be(GameState.Finished);
        }


        [Test]
        public void Shoot_when_some_more_ships_on_board_does_not_change_state()
        {
            // Arrange
            var validPosition = new Position(1, 1);
            _game.StartNewGame();
            _board
                .SetupGet(x => x.ShipsLeft)
                .Returns(1);

            // Act
            _game.Shoot(validPosition);

            // Assert
            _game.State.Should().Be(GameState.Started);
        }

        [Test]
        public void Shoot_on_finished_game_throws_GameRulesViolationException()
        {
            // Arrange
            var validPosition = new Position(1, 1);
            var otherValidPosition = new Position(2, 2);
            _game.StartNewGame();
            _board
                .SetupGet(x => x.ShipsLeft)
                .Returns(0);
            _game.Shoot(validPosition);

            _game.State.Should().Be(GameState.Finished);

            // Act & Assert
            Assert.Throws<GameRulesViolationException>(() => _game.Shoot(otherValidPosition));
        }


        [Test]
        public void StartNewGame_on_already_started_game_recreates_board()
        {
            // Arrange
            _game.StartNewGame();
            _game.State.Should().Be(GameState.Started);

            // Act
            _game.StartNewGame();

            // Assert
            _game.State.Should().Be(GameState.Started);
            _placingStrategy.Verify(x => x.PlaceShips(), Times.Exactly(2));
            _boardFactory.Verify(x => x.Create(It.IsAny<IShipPlacementContainer>()), Times.Exactly(2));
        }

        [Test]
        public void StartNewGame_on_finished_game_recreates_board()
        {
            // Arrange
            var validPosition = new Position(1, 1);
            var otherValidPosition = new Position(2, 2);
            _game.StartNewGame();
            _board
                .SetupGet(x => x.ShipsLeft)
                .Returns(0);
            _game.Shoot(validPosition);

            _game.State.Should().Be(GameState.Finished);

            // Act
            _game.StartNewGame();

            // Assert
            _game.State.Should().Be(GameState.Started);
            _placingStrategy.Verify(x => x.PlaceShips(), Times.Exactly(2));
            _boardFactory.Verify(x => x.Create(It.IsAny<IShipPlacementContainer>()), Times.Exactly(2));
        }
    }
}
