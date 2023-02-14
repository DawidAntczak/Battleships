namespace Battleships.Core.Placings
{
    public interface IRandomNumberGenerator
    {
        int Generate();
        int Generate(int inclusiveStart, int exclusiveEnd);
    }
}
