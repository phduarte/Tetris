using System;
using System.Windows.Forms;

namespace Gadz.Tetris.Desktop {
    public partial class Inicio : Form {
        public Inicio() {
            InitializeComponent();
            cmbEstilo.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e) {
            
            Program.ClassicMode = "BRICK GAME".Equals(cmbEstilo.SelectedItem.ToString(), StringComparison.InvariantCultureIgnoreCase);

            new Jogo(this).Show();
            Hide();
        }

        private void button2_Click(object sender, EventArgs e) {
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            new Help().ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            new About().ShowDialog();
        }

        private void Inicio_KeyDown(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.ShiftKey) {
                Program.Player.ToggleMute();
            }
        }
    }
}
