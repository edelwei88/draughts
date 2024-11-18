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
    public partial class Login : Form
    {
        private Color Color => colorDialog1.Color;
        private string Nickname => textBox1.Text;

        public Login()
        {
            InitializeComponent();
            colorDialog1.Color = Color.White;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var dialog = new GameForm(Nickname, Color))
            {
                Hide();
                dialog.ShowDialog();
                Show();

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var dialog = new Results())
            {
                Hide();
                dialog.ShowDialog();
                Show();
            }
        }
    }
}
