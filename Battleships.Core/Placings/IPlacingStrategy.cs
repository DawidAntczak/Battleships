namespace Battleships.Core.Placings
{
    public interface IPlacingStrategy
    {
        IShipPlacementContainer PlaceShips();
    }
}
