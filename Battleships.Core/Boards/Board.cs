using Battleships.Core.Placings;
using Battleships.Core.Ships;
using MvvmCross.ViewModels;

namespace Battleships.Core.Boards
{
    public class Board : MvxNotifyPropertyChanged
    {
        public const int Rows = 10;
        public const int Columns = 10;

        public BoardState State { get => _state; set { _state = value; RaisePropertyChanged(() => State); } }
        public Cell[,] Cells { get; init; }

        private readonly IPlacingStrategy _shipPlacingStrategy;
        private BoardState _state = BoardState.Uninitialized;
        private ShipPlacement _shipPlacement;
        private ISet<Ship> _shipsLeft = new HashSet<Ship>();
        private ISet<Position> _hits = new HashSet<Position>();

        public Board(IPlacingStrategy computerShipPlacingStrategy)
        {
            _shipPlacingStrategy = computerShipPlacingStrategy;
            Cells = CreateCells();
        }

        // ship placing may use more complicated algorithms which take some time, so it's not done in the constructor
        public void Initialize()
        {
            ResetCells();
            _shipPlacement = _shipPlacingStrategy.PlaceShips();
            _shipsLeft = _shipPlacement.OccupiedPositions.Values.ToHashSet();
            _hits = new HashSet<Position>();
            State = BoardState.Initialized;
        }

        public void Shot(Position position)
        {
            if (State != BoardState.Initialized)
                throw new BoardException($"Invalid operation for state {State}.");

            var hitCell = Cells[position.Row, position.Column];

            if (hitCell.CellType != CellType.Covered)   // you already hit it
            {
                return;
            }

            var isHit = _shipPlacement.OccupiedPositions.TryGetValue(position, out var hitShip);

            if (!isHit)
            {
                hitCell.CellType = CellType.Missed;
                return;
            }

            _hits.Add(position);

            if (hitShip.OccupiedPositions.Intersect(_hits).Count() == hitShip.OccupiedPositions.Count()) // every ship position was hit
            {
                HandleShipSinking(hitShip);
            }
            else
            {
                hitCell.CellType = CellType.Hit;
            }
        }

        public bool CanShoot(Position _)
        {
            return State == BoardState.Initialized;
        }

        private Cell[,] CreateCells()
        {
            var cells = (
                from row in Enumerable.Range(0, Rows)
                from column in Enumerable.Range(0, Columns)
                select new Cell(row, column, CellType.Covered)
            );

            var cellsArray = new Cell[Rows, Columns];
            foreach (var cell in cells)
            {
                cellsArray[cell.Row, cell.Column] = cell;
            }
            return cellsArray;
        }

        private void ResetCells()
        {
            foreach (var cell in Cells)
            {
                Cells[cell.Row, cell.Column].CellType = CellType.Covered;
            }
        }

        private void HandleShipSinking(Ship hitShip)
        {
            foreach(var position in hitShip.OccupiedPositions)
            {
                Cells[position.Row, position.Column].CellType = CellType.Sunk;
            }

            _shipsLeft.Remove(hitShip);
            if (!_shipsLeft.Any())
            {
                State = BoardState.Finished;
            }
        }
    }
}
