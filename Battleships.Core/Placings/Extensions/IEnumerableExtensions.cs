namespace Battleships.Core.Placings.Extensions
{
    internal static class IEnumerableExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable, IRandomNumberGenerator randomNumberGenerator)
        {
            return enumerable.OrderBy(x => randomNumberGenerator.Generate());
        }
    }
}
