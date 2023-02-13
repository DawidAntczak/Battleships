using System.Runtime.Serialization;

namespace Battleships.Core.Boards
{
    public class BoardException : Exception
    {
        public BoardException()
        {
        }

        public BoardException(string? message) : base(message)
        {
        }

        public BoardException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BoardException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
