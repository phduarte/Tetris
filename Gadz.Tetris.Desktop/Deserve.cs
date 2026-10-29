using Gadz.Tetris.Application;

namespace Gadz.Tetris.Desktop;

public partial class Deserve : Form
{
    public Deserve(StatsSnapshot stats)
    {
        InitializeComponent();

        lbScore.Text = stats.Score.ToString();
        lbLevel.Text = stats.Level.ToString();
        lbPieces.Text = stats.Pieces.ToString();
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
