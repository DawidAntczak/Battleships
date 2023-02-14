using Battleships.Core.Placings;

namespace Battleships.Core.Boards
{
    public interface IBoardFactory
    {
        IBoard Create(IShipPlacementContainer shipPlacementContainer);
    }
}
