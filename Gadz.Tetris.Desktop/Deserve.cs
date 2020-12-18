using Gadz.Tetris.Model.Statistics;
using System;
using System.Windows.Forms;

namespace Gadz.Tetris.Desktop
{
    public partial class Deserve : Form
    {
        public Deserve(Stats stats)
        {
            InitializeComponent();

            lbScore.Text = stats.Score.ToString();
            lbLevel.Text = stats.Level.ToString();
            lbPieces.Text = stats.Blocks.ToString();
            lbTime.Text = stats.Duration.ToString();
            lbTetris.Text = stats.TetrisRate.ToString("P0");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
