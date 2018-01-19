using System.Windows.Forms;

namespace Gadz.Tetris.Desktop {
    public partial class Fim : Form {

        public Fim() {
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
