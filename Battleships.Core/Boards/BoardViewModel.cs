using Battleships.Core.Boards.Extensions;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Collections.Immutable;
using System.ComponentModel;

namespace Battleships.Core.Boards
{
    public class BoardViewModel : MvxViewModel
    {
        public IMvxCommand NewGameCommand => new MvxCommand(Board.Initialize);
        public IMvxCommand ShotCommand => new MvxCommand<Position>(Board.Shot, Board.CanShoot);
        public IEnumerable<Cell> Cells { get; }
        public string StateText { get => _stateText; private set { _stateText = value; RaisePropertyChanged(() => StateText); } }
        public Board Board { get; }

        private string _stateText = string.Empty;

        private readonly IImmutableDictionary<BoardState, string> _topTexts = new Dictionary<BoardState, string>()
        {
            { BoardState.Uninitialized, "Start the game" },
            { BoardState.Initialized, "Shoot ships!" },
            { BoardState.Finished, "You sunk all ships! " }
        }.ToImmutableDictionary();


        public BoardViewModel(Board board)
        {
            Board = board;
            Cells = board.Cells.ToEnumerable();
            StateText = _topTexts[Board.State];
            Board.PropertyChanged += Board_PropertyChanged;
        }

        public override Task Initialize()
        {
            // comment out if you want to press the new game button before the board generates for the first time
            // would be usefull if ship amount and size would be configurable
            Board.Initialize(); 
            return base.Initialize();
        }

        private void Board_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Board.State))
            {
                StateText = _topTexts[Board.State];
            }
        }
    }
}
