using System.Runtime.Serialization;

namespace Battleships.Core.Games
{
    public class GameRulesViolationException : Exception
    {
        public GameRulesViolationException()
        {
        }

        public GameRulesViolationException(string? message) : base(message)
        {
        }

        public GameRulesViolationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected GameRulesViolationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
