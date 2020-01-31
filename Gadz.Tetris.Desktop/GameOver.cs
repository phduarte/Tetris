using System.Windows.Forms;
using Texto = Gadz.Tetris.Resources.Textos.Jogo;

namespace Gadz.Tetris.Desktop
{
    public partial class GameOver : Form
    {
        public GameOver()
        {
            InitializeComponent();
            lbContinue.Text = Texto.PressioneParaContinuar;
        }

        private void GameOver_KeyDown(object sender, KeyEventArgs e)
        {
            Close();
        }

        private void GameOver_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}