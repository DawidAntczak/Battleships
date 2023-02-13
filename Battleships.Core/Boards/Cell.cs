using MvvmCross.ViewModels;

namespace Battleships.Core.Boards
{
    public class Cell : MvxNotifyPropertyChanged
    {
        public int Row { get; }
        public int Column { get; }

        public CellType Type
        {
            get { return _type; }
            internal set { SetProperty(ref _type, value); }
        }
        private CellType _type;

        public Cell(int row, int column, CellType cellType)
        {
            Row = row;
            Column = column;
            _type = cellType;
        }
    }
}
