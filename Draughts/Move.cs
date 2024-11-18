namespace Draughts
{
    public class Move
    {
        public Position NewPosition { get; }
        public bool IsCaptureMove { get; }
        public Piece PieceToCapture { get; }

        public Move(Position newPosition, bool isCaptureMove, Piece pieceToCapture)
        {
            NewPosition = newPosition;
            IsCaptureMove = isCaptureMove;
            PieceToCapture = pieceToCapture;
        }
        public Move(Position newPosition, bool isCaptureMove) : this (newPosition, isCaptureMove, null)
        {
        }
    }
}
