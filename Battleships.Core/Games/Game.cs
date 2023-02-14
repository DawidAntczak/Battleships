using Battleships.Core.Boards;
using Battleships.Core.Placings;
using MvvmCross.ViewModels;

namespace Battleships.Core.Games
{
    public class Game : MvxNotifyPropertyChanged
    {
        public IBoard? Board { get => _board; set { _board = value; RaisePropertyChanged(() => Board); } }
        private IBoard? _board;

        public GameState State { get => _state; set { _state = value; RaisePropertyChanged(() => State); } }
        private GameState _state = GameState.Uninitialized;

        private readonly IBoardFactory _boardFactory;
        private readonly IPlacingStrategy _placingStrategy;

        public Game(IBoardFactory boardFactory, IPlacingStrategy placingStrategy)
        {
            _boardFactory = boardFactory;
            _placingStrategy = placingStrategy;
        }

        public void StartNewGame()
        {
            var shipPlacement = _placingStrategy.PlaceShips();
            Board = _boardFactory.Create(shipPlacement);
            State = GameState.Started;
        }

        public void Shoot(Position position)
        {
            GameRulesGuard.Check(() => State == GameState.Started, $"Invalid operation for current game state.");
            GameRulesGuard.Check(() => IsShotTargetValid(position), $"Invalid target!");

            Board.AddShot(position);
            if (Board.ShipsLeft == 0)
            {
                State = GameState.Finished;
            }
        }

        public bool CanShoot(Position position)
        {
            return State == GameState.Started && IsShotTargetValid(position);
        }

        private bool IsShotTargetValid(Position position)
        {
            return Board != null
                && Board.Cells.TryGetValue(position, out var cell)
                && cell.Type == CellType.Undiscovered;
        }
    }
}
