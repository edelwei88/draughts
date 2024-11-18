using Draughts;
using System.Windows.Forms;

namespace DraughtsGame
{
    public partial class Results : Form
    {
        public Results()
        {
            InitializeComponent();

            var list = Extension.LoadFromFile();

            foreach (var item in list)
            {
                dataGridView1.Rows.Add(item.Nickname, item.Result, item.ScorePlayerOne, item.ScorePlayerTwo);
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
