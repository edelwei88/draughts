using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Draughts
{
    public class Game : Control
    {
        public event Action EventClose;

        BufferedGraphics bg;
        Graphics g;

        public Draughts draughts = new Draughts();
        public string Nickname { get; set; }
        public Color PieceColor { get; set; }

        public Game()
        {
            draughts.EventGameOver += GameOver;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            g.Clear(BackColor);

            g.FillRectangles(Brushes.SandyBrown, Enumerable.Range(0, 64).Where(item => item / 8 % 2 == 1 && item % 8 % 2 == 0 || item / 8 % 2 == 0 && item % 8 % 2 == 1).Select(x => new Rectangle(x / 8 * 80, x % 8 * 80, 80, 80)).ToArray());
            g.FillRectangles(Brushes.PeachPuff, Enumerable.Range(0, 64).Where(item => item / 8 % 2 == 1 && item % 8 % 2 == 1 || item / 8 % 2 == 0 && item % 8 % 2 == 0).Select(x => new Rectangle(x / 8 * 80, x % 8 * 80, 80, 80)).ToArray());

            if (draughts.SelectedPiece != null)
                g.FillRectangle(Brushes.LightGreen, draughts.SelectedPiece.Position.Column * 80, draughts.SelectedPiece.Position.Row * 80, 80, 80);

            foreach (var move in draughts.CurrentMoves)
                g.FillRectangle(Brushes.LightGreen, move.NewPosition.Column * 80, move.NewPosition.Row * 80, 80, 80);

            foreach (var piece in draughts.PlayerOne.PieceList)
                g.FillEllipse(new SolidBrush(PieceColor), piece.Position.Column * 80 + 10, piece.Position.Row * 80 + 10, 60, 60);

            foreach (var piece in draughts.PlayerTwo.PieceList)
                g.FillEllipse(Brushes.Black, piece.Position.Column * 80 + 10, piece.Position.Row * 80 + 10, 60, 60);



            bg.Render(e.Graphics);
        }

        private void GameOver()
        {
            Invalidate();
            using (var dialog = new Result(Nickname, draughts.GameOverState, draughts.PlayerOne.Score, draughts.PlayerTwo.Score))
            {
                dialog.ShowDialog();

                if (dialog.Action)
                    Restart();
                else
                    EventClose?.Invoke();
            }
        }

        private void Restart()
            => draughts.Restart();


        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (draughts.GameOverState != 0) return;
            var row = e.Y / 80;
            var col = e.X / 80;
            Move currentMove;

            if ((currentMove = draughts.CurrentMoves.FirstOrDefault(move => move.NewPosition == new Position(row, col))) != null)
                draughts.ExecuteMove(currentMove);
            else
                draughts.SelectPiece(row, col);

            while (draughts.CurrentPlayer != ePlayer.PlayerOne)
                draughts.ExecuteRandomMove();

            Invalidate();
            base.OnMouseClick(e);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            bg = BufferedGraphicsManager.Current.Allocate(Graphics.FromImage(new Bitmap(Width + 1, Height + 1)), new Rectangle(0, 0, Width + 1, Height + 1));
            g = bg.Graphics;
        }
    }
}
