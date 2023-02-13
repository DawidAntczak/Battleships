using MvvmCross.ViewModels;

namespace Battleships.Core.Boards
{
    public class Cell : MvxNotifyPropertyChanged
    {
        public int Row { get; }
        public int Column { get; }

        public CellType CellType
        {
            get { return _cellType; }
            internal set { SetProperty(ref _cellType, value); }
        }
        private CellType _cellType;

        public Cell(int row, int column, CellType cellType)
        {
            Row = row;
            Column = column;
            _cellType = cellType;
        }
    }
}
