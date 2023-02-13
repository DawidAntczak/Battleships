namespace Battleships.Core.Placings
{
    public interface IPlacingStrategy
    {
        ShipPlacementContainer PlaceShips();
    }
}
