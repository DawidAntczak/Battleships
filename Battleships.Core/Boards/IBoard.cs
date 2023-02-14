using MvvmCross.ViewModels;

namespace Battleships.Core.Boards
{
    public interface IBoard : IMvxNotifyPropertyChanged
    {
        int ShipsLeft { get; }
        IReadOnlyDictionary<Position, Cell> Cells { get; }
        void AddShot(Position position);
    }
}
