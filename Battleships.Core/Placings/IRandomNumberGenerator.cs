namespace Battleships.Core.Placings
{
    internal interface IRandomNumberGenerator
    {
        int Generate();
        int Generate(int inclusiveStart, int exclusiveEnd);
    }
}
