using Battleships.Core.Placings;
using MvvmCross.ViewModels;

namespace Battleships.Core.Boards
{
    public class Board : MvxNotifyPropertyChanged, IBoard
    {
        public const int Rows = 10;
        public const int Columns = 10;

        public IReadOnlyDictionary<Position, Cell> Cells { get; } // dictionary implements IEnumerable<T> and cooperates better than a multidimensional [x,y] array

        public int ShipsLeft { get => _shipsLeft; private set { _shipsLeft = value; RaisePropertyChanged(() => ShipsLeft); } }
        private int _shipsLeft = 0;

        private readonly ISet<Position> _hits = new HashSet<Position>();
        private IShipPlacementContainer _shipPlacement;

        public Board(IShipPlacementContainer shipPlacement)
        {
            _shipPlacement = shipPlacement;
            ShipsLeft = _shipPlacement.Ships.Count();
            Cells = (
                from row in Enumerable.Range(0, Rows)
                from column in Enumerable.Range(0, Columns)
                select new Cell(row, column, CellType.Undiscovered)
            ).ToDictionary(c => new Position(c.Row, c.Column), c => c);
        }

        public void AddShot(Position position)
        {
            var shootedCell = Cells[position];

            if (_shipPlacement.OccupiedPositions.TryGetValue(position, out var hitShip))
            {
                _hits.Add(position);

                if (hitShip.OccupiedPositions.Intersect(_hits).Count() == hitShip.OccupiedPositions.Count) // every ship position has been hit
                {
                    HandleShipSinking(hitShip);
                }
                else
                {
                    shootedCell.Type = CellType.HitShip;
                }
            }
            else
            {
                shootedCell.Type = CellType.MissedShot;
            }
        }

        private void HandleShipSinking(Ship hitShip)
        {
            foreach(var position in hitShip.OccupiedPositions)
            {
                Cells[position].Type = CellType.SunkShip;
            }
            ShipsLeft--;
        }
    }
}
