using System.Windows.Forms;

namespace Gadz.Tetris.Desktop {
    public partial class GameOver : Form {

        public GameOver() {
            InitializeComponent();
        }

        private void Fim_KeyDown(object sender, KeyEventArgs e) {
            Close();
        }

        private void Fim_Click(object sender, System.EventArgs e) {
            Close();
        }
    }
}
