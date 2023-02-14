using Battleships.Core.Placings;

namespace Battleships.Core.Boards
{
    internal class BoardFactory : IBoardFactory
    {
        public IBoard Create(IShipPlacementContainer shipPlacementContainer)
        {
            return new Board(shipPlacementContainer);
        }
    }
}
