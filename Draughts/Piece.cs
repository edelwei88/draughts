using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Draughts
{
    public class Piece
    {
        public Position Position { get; set; }
        public bool IsKing { get; private set; }
        public Piece(Position position)
        {
            Position = position;
        }

        public Piece(int row, int col) : this(new Position(row, col))
        {
        }

        public List<Move> GetMoves(PieceList pieceListAlly, PieceList pieceListEnemy, eMoveDirection moveDirection)
        {
            var moveList = new List<Move>();
            var diagPositions = GetDiagonalPositions(moveDirection).ToArray();

            if (diagPositions.Length == 0)
                return new List<Move>();

            if (!IsKing && !pieceListAlly.PieceExists(diagPositions[0]) && !pieceListEnemy.PieceExists(diagPositions[0]))
                return new List<Move>()
                {
                    new Move(diagPositions[0], false)
                };

            if (!IsKing && diagPositions.Length < 2)
                return new List<Move>();

            if (!IsKing && !pieceListAlly.PieceExists(diagPositions[0]) && pieceListEnemy.PieceExists(diagPositions[0])
                && !pieceListAlly.PieceExists(diagPositions[1]) && !pieceListEnemy.PieceExists(diagPositions[1]))
                return new List<Move>()
                {
                    new Move (diagPositions[1], true, pieceListEnemy[diagPositions[0].Row, diagPositions[0].Column])
                };

            if (IsKing && !diagPositions.Any(pos => pieceListEnemy.PieceExists(pos)))
                return diagPositions.TakeWhile(pos => !pieceListAlly.PieceExists(pos)).Select(pos => new Move(pos, false)).ToList();

            else if (IsKing && diagPositions.TakeWhile(pos => !pieceListEnemy.PieceExists(pos)).Any(pos => pieceListAlly.PieceExists(pos)))
                return diagPositions.TakeWhile(pos => !pieceListAlly.PieceExists(pos)).Select(pos => new Move(pos, false)).ToList();

            else if (IsKing && diagPositions.SkipWhile(pos => !pieceListEnemy.PieceExists(pos)).Skip(1).All(pos => pieceListAlly.PieceExists(pos) || pieceListEnemy.PieceExists(pos)))
                return diagPositions.TakeWhile(pos => !pieceListEnemy.PieceExists(pos)).Select(pos => new Move(pos, false)).ToList();

            else if (IsKing)
            {
                var temp = diagPositions.SkipWhile(pos => !pieceListEnemy.PieceExists(pos)).ToList();
                var pieceToCapture = pieceListEnemy[temp[0].Row, temp[0].Column];

                return temp.Skip(1).TakeWhile(pos => !pieceListAlly.PieceExists(pos) && !pieceListEnemy.PieceExists(pos)).Select(pos => new Move(pos, true, pieceToCapture)).ToList();
            }

            return new List<Move>();
        }

        public void TryToBecomeKing(ePlayer player)
        {
            switch (player)
            {
                case ePlayer.PlayerOne:
                    if (Position.Row == 0)
                        IsKing = true;
                    break;
                case ePlayer.PlayerTwo:
                    if (Position.Row == 7)
                        IsKing = true;
                    break;
            }
        }

        private IEnumerable<Position> GetDiagonalPositions(eMoveDirection direction)
        {

            int row = Position.Row;
            int col = Position.Column;

            switch (direction)
            {
                case eMoveDirection.UpLeft:
                    while (Extension.IsInBound(--row, --col))
                        yield return new Position(row, col);
                    break;
                case eMoveDirection.UpRight:
                    while (Extension.IsInBound(--row, ++col))
                        yield return new Position(row, col);
                    break;
                case eMoveDirection.DownLeft:
                    while (Extension.IsInBound(++row, --col))
                        yield return new Position(row, col);
                    break;
                case eMoveDirection.DownRight:
                    while (Extension.IsInBound(++row, ++col))
                        yield return new Position(row, col);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
