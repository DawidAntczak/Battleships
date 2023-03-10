using Battleships.Core.Boards;

namespace Battleships.Core.Games
{
    public static class GameRulesGuard
    {
        public static void Check(Func<bool> rule, string violationMessage)
        {
            ArgumentNullException.ThrowIfNull(nameof(rule));
            ArgumentNullException.ThrowIfNull(nameof(violationMessage));

            if (!rule())
                throw new GameRulesViolationException(violationMessage);
        }
    }
}
