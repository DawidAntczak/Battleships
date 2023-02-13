using Battleships.Core.Placings;
using MvvmCross.ViewModels;

namespace Battleships.Core.Boards
{
    public class Board : MvxNotifyPropertyChanged
    {
        public const int Rows = 10;
        public const int Columns = 10;

        public IReadOnlyDictionary<Position, Cell> Cells { get; } // dictionary implements IEnumerable and cooperates better than a multidimensional [x,y] array

        public BoardState State { get => _state; set { _state = value; RaisePropertyChanged(() => State); } }
        private BoardState _state = BoardState.Uninitialized;

        public int ShipsLeft { get => _shipsLeft; set { _shipsLeft = value; RaisePropertyChanged(() => ShipsLeft); } }
        private int _shipsLeft = 0;

        private readonly IPlacingStrategy _shipPlacingStrategy;

        private readonly ISet<Position> _hits = new HashSet<Position>();
        private ShipPlacementContainer _shipPlacement = new();

        public Board(IPlacingStrategy computerShipPlacingStrategy)
        {
            _shipPlacingStrategy = computerShipPlacingStrategy;
            Cells = (
                from row in Enumerable.Range(0, Rows)
                from column in Enumerable.Range(0, Columns)
                select new Cell(row, column, CellType.Undiscovered)
            ).ToDictionary(c => new Position(c.Row, c.Column), c => c);
        }

        public static bool IsValidBoardPosition(Position position)
            => position.Row >= 0 && position.Row < Rows && position.Column >= 0 && position.Column < Columns;

        public void Initialize()    // ship placing may use more complicated algorithms which take some time, so it's not done in the constructor
        {
            _hits.Clear();
            _shipPlacement = _shipPlacingStrategy.PlaceShips();
            ShipsLeft = _shipPlacement.Ships.Count();
            foreach (var cell in Cells.Values)
            {
                cell.Type = CellType.Undiscovered;
            }
            State = BoardState.Initialized;
        }

        public void Shoot(Position position)
        {
            GameRulesGuard.Check(() => State == BoardState.Initialized, $"Invalid operation for state {State}.");
            GameRulesGuard.Check(() => IsShotTargetValid(position), $"Invalid target!");

            var hitCell = Cells[position];

            if (_shipPlacement.OccupiedPositions.TryGetValue(position, out var hitShip))
            {
                _hits.Add(position);

                if (hitShip.OccupiedPositions.Intersect(_hits).Count() == hitShip.OccupiedPositions.Count) // every ship position has been hit
                {
                    HandleShipSinking(hitShip);
                }
                else
                {
                    hitCell.Type = CellType.HitShip;
                }
            }
            else
            {
                hitCell.Type = CellType.MissedShot;
            }
        }

        public bool CanShoot(Position position)
        {
            return State == BoardState.Initialized && IsShotTargetValid(position);
        }

        private void HandleShipSinking(Ship hitShip)
        {
            foreach(var position in hitShip.OccupiedPositions)
            {
                Cells[position].Type = CellType.SunkShip;
            }

            ShipsLeft--;
            if (ShipsLeft == 0)
            {
                State = BoardState.Finished;
            }
        }

        private bool IsShotTargetValid(Position position)
        {
            return Cells.TryGetValue(position, out var cell) && cell.Type == CellType.Undiscovered;
        }
    }
}
