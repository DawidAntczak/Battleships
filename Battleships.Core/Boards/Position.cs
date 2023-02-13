namespace Battleships.Core.Boards
{
    public record class Position(int Row, int Column)
    {
        public Position Translated(int rowOffset, int columnOffset)
        {
            return new Position(Row + rowOffset, Column + columnOffset);
        }
    }
}
