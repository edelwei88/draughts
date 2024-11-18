using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Draughts
{
    public class Player
    {
        public ePlayer PlayerID { get; }
        public PieceList PieceList { get; set; }
        public int Score { get; set; }

        public Player(ePlayer player)
        {
            PlayerID = player;
            PieceList = new PieceList(PlayerID);
        }

        public Dictionary<Piece, List<Move>> GetViableMoves(PieceList pieceListEnemy)
        {
            switch (PlayerID)
            {
                case ePlayer.PlayerOne:
                    return GetViableMovesForPlayerOne(pieceListEnemy);
                case ePlayer.PlayerTwo:
                    return GetViableMovesForPlayerTwo(pieceListEnemy);
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        private Dictionary<Piece, List<Move>> GetViableMovesForPlayerOne(PieceList pieceListEnemy)
        {
            var dict = new Dictionary<Piece, List<Move>>();

            foreach (var piece in PieceList)
            {
                var moves = new List<Move>();

                for (var i = 0; i < 4; i++)
                    moves.AddRange(piece.GetMoves(PieceList, pieceListEnemy, (eMoveDirection)i));

                if (!piece.IsKing && !moves.Any(move => move.IsCaptureMove))
                    moves = moves.Where(move => move.NewPosition.Row < piece.Position.Row).ToList();

                dict.Add(piece, moves);
            }


            if (dict.Any(item => item.Value.Any(move => move.IsCaptureMove)))
            {
                var dictTemp = new Dictionary<Piece, List<Move>>();

                foreach (var kvp in dict)
                    dictTemp.Add(kvp.Key, kvp.Value.Where(move => move.IsCaptureMove).ToList());

                dict = dictTemp;
            }

            return dict;
        }
        private Dictionary<Piece, List<Move>> GetViableMovesForPlayerTwo(PieceList pieceListEnemy)
        {
            var dict = new Dictionary<Piece, List<Move>>();

            foreach (var piece in PieceList)
            {
                var moves = new List<Move>();

                for (var i = 0; i < 4; i++)
                    moves.AddRange(piece.GetMoves(PieceList, pieceListEnemy, (eMoveDirection)i));

                if (!piece.IsKing && !moves.Any(move => move.IsCaptureMove))
                    moves = moves.Where(move => move.NewPosition.Row > piece.Position.Row).ToList();

                dict.Add(piece, moves);
            }

            if (dict.Any(item => item.Value.Any(move => move.IsCaptureMove)))
            {
                var dictTemp = new Dictionary<Piece, List<Move>>();

                foreach (var kvp in dict)
                    dictTemp.Add(kvp.Key, kvp.Value.Where(move => move.IsCaptureMove).ToList());

                dict = dictTemp;
            }

            return dict;
        }

    }
}
