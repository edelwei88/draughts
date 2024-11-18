using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DraughtsGame
{
    public partial class GameForm : Form
    {
        public GameForm(string name, Color color)
        {
            InitializeComponent();
            game1.draughts.EventMoveExecuted += UpdateScore;
            game1.EventClose += CloseThis;
            game1.PieceColor = color;
            game1.Nickname = name;
        }

        private void UpdateScore()
        {
            label1.Text = $"Компьютер: {game1.draughts.PlayerTwo.Score}";
            label2.Text = $"Игрок: {game1.draughts.PlayerOne.Score}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CloseThis() => Close();
    }
}
