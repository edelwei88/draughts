using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Draughts
{
    public class PieceList : IEnumerable<Piece>
    {
        private readonly List<Piece> _pieces;
        public Piece GetRandomPiece
        {
            get
            {
                if (_pieces.Count == 0)
                    return null;
                Random rand = new Random(DateTime.Now.Millisecond);
                return _pieces[rand.Next(_pieces.Count)];
            }
        }

        public Piece this[int row, int col]
        {
            get => _pieces.FirstOrDefault(piece => piece.Position == new Position(row, col));
        }

        public PieceList(ePlayer player)
        {
            _pieces = InitPlayer(player);
        }

        private List<Piece> InitPlayer(ePlayer player)
        {
            switch (player)
            {
                case ePlayer.PlayerOne:
                    return Enumerable.Range(40, 24).Where(item => item / 8 % 2 == 1 && item % 8 % 2 == 0 || item / 8 % 2 == 0 && item % 8 % 2 == 1).Select(item => new Piece(item / 8, item % 8)).ToList();
                case ePlayer.PlayerTwo:
                    return Enumerable.Range(0, 24).Where(item => item / 8 % 2 == 1 && item % 8 % 2 == 0 || item / 8 % 2 == 0 && item % 8 % 2 == 1).Select(item => new Piece(item / 8, item % 8)).ToList();
                default:
                    throw new ArgumentException();
            }
        }

        public void Remove(Piece piece)
        {
            _pieces.Remove(piece);
        }

        public bool PieceExists(Position pos) => _pieces.Any(item => item.Position == pos);
        public bool PieceExists(int row, int col) => PieceExists(new Position(row, col));




        public IEnumerator<Piece> GetEnumerator()
            => ((IEnumerable<Piece>)_pieces).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => ((IEnumerable)_pieces).GetEnumerator();
    }
}
