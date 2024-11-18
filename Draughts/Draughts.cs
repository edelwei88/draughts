using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts
{
    public class Draughts
    {
        public event Action EventGameOver;
        public event Action EventMoveExecuted;

        public int GameOverState { get; private set; }
        public Player PlayerOne { get; private set; }
        public Player PlayerTwo { get; private set; }
        public ePlayer CurrentPlayer => _currentPlayer;
        private ePlayer _currentPlayer;
        public Piece SelectedPiece { get; private set; }
        private Dictionary<Piece, List<Move>> _currentMoves;
        public List<Move> CurrentMoves => SelectedPiece == null ? new List<Move>() : _currentMoves[SelectedPiece];

        public Draughts()
        {
            PlayerOne = new Player(ePlayer.PlayerOne);
            PlayerTwo = new Player(ePlayer.PlayerTwo);
            _currentPlayer = ePlayer.PlayerOne;
            _currentMoves = PlayerOne.GetViableMoves(PlayerTwo.PieceList);
        }

        public void Restart()
        {
            PlayerOne = new Player(ePlayer.PlayerOne);
            PlayerTwo = new Player(ePlayer.PlayerTwo);
            _currentPlayer = ePlayer.PlayerOne;
            SelectedPiece = null;
            _currentMoves = PlayerOne.GetViableMoves(PlayerTwo.PieceList);
        }

        public void SelectPiece(int row, int col)
        {
            switch (_currentPlayer)
            {
                case ePlayer.PlayerOne:
                    if (SelectedPiece != null && SelectedPiece.Position == new Position(row, col))
                        SelectedPiece = null;
                    SelectedPiece = PlayerOne.PieceList[row, col];
                    break;
                case ePlayer.PlayerTwo:
                    if (SelectedPiece != null && SelectedPiece.Position == new Position(row, col))
                        SelectedPiece = null;
                    SelectedPiece = PlayerTwo.PieceList[row, col];
                    break;
            }
        }


        public void ExecuteMove(Move move)
        {
            switch (_currentPlayer)
            {
                case ePlayer.PlayerOne:
                    SelectedPiece.Position = move.NewPosition;
                    if (move.IsCaptureMove)
                    {
                        PlayerTwo.PieceList.Remove(move.PieceToCapture);
                        PlayerOne.Score++;
                    }
                    SelectedPiece.TryToBecomeKing(_currentPlayer);
                    _currentMoves = PlayerOne.GetViableMoves(PlayerTwo.PieceList);
                    break;
                case ePlayer.PlayerTwo:
                    SelectedPiece.Position = move.NewPosition;
                    if (move.IsCaptureMove)
                    {
                        PlayerOne.PieceList.Remove(move.PieceToCapture);
                        PlayerTwo.Score++;
                    }
                    SelectedPiece.TryToBecomeKing(_currentPlayer);
                    _currentMoves = PlayerTwo.GetViableMoves(PlayerOne.PieceList);
                    break;
            }

            EventMoveExecuted?.Invoke();

            CheckGameOver();
            if (GameOverState != 0)
                EventGameOver?.Invoke();

            if (move.IsCaptureMove && SelectedPiece != null && _currentMoves[SelectedPiece].Any(item => item.IsCaptureMove))
                return;
            else
                ChangeCurrentPlayer();

        }

        public void ExecuteRandomMove()
        {
            Random rand = new Random();
            bool moveExecuted = false;

            while (!moveExecuted)
            {
                Piece piece = PlayerTwo.PieceList.GetRandomPiece;
                List<Move> moves;
                if (piece == null)
                {
                    _currentPlayer = ePlayer.PlayerOne;
                    return;
                }
                if ((moves = _currentMoves[piece]).Count > 0)
                {
                    SelectedPiece = piece;
                    ExecuteMove(moves[rand.Next(moves.Count)]);
                    moveExecuted = true;
                }
            }
        }

        private void ChangeCurrentPlayer()
        {
            switch (_currentPlayer)
            {
                case ePlayer.PlayerOne:
                    _currentPlayer = ePlayer.PlayerTwo;
                    _currentMoves = PlayerTwo.GetViableMoves(PlayerOne.PieceList);
                    break;
                case ePlayer.PlayerTwo:
                    _currentPlayer = ePlayer.PlayerOne;
                    _currentMoves = PlayerOne.GetViableMoves(PlayerTwo.PieceList);
                    break;
            }

            SelectedPiece = null;
        }

        private void CheckGameOver()
        {
            var playerOneMoves = PlayerOne.GetViableMoves(PlayerTwo.PieceList).Values.All(list => list.Count == 0);
            var playerTwoMoves = PlayerTwo.GetViableMoves(PlayerOne.PieceList).Values.All(list => list.Count == 0);

            if (playerOneMoves && playerTwoMoves)
                GameOverState = 3;
            else if (playerOneMoves)
                GameOverState = 1;
            else if (playerTwoMoves)
                GameOverState = 2;
            else
                GameOverState = 0;
        }
    }
}
