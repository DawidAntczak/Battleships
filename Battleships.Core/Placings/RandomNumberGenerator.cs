namespace Battleships.Core.Placings
{
    internal class RandomNumberGenerator : IRandomNumberGenerator
    {
        private readonly Random _random = new();

        public int Generate()
        {
            return _random.Next();
        }

        public int Generate(int inclusiveStart, int exclusiveEnd)
        {
            return _random.Next(inclusiveStart, exclusiveEnd);
        }
    }
}
