using System;
using System.Windows.Forms;

namespace Draughts
{
    public partial class Result : Form
    {
        public bool Action { get; private set; }
        public Result(string name, int result, int scorePO, int scorePT)
        {
            InitializeComponent();

            Extension.SaveToFile(new ResultEntry(name, result, scorePO, scorePT)); 

            switch(result)
            {
                case 1:
                    label1.Text = "Поражение";
                    break;
                case 2:
                    label1.Text = "Победа";
                    break;
                case 3:
                    label1.Text = "Ничья";
                    break;
            }

            label2.Text = "Счет компьютера: " + scorePT;
            label3.Text = "Счет игрока: " + scorePO;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Action = true;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Action = false;
            Close();
        }
    }
}
