using System;
using System.Windows.Forms;
using Texto = Gadz.Tetris.Resources.Textos.Jogo;

namespace Gadz.Tetris.Desktop
{
    public partial class Choose : Form
    {
        public Choose()
        {
            InitializeComponent();
            SetScreenText();
            cmbEstilo.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.ClassicMode = "BRICK GAME".Equals(cmbEstilo.SelectedItem.ToString(), StringComparison.InvariantCultureIgnoreCase);

            new Play(this).Show();
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Help().ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new About().ShowDialog();
        }

        private void Inicio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                chkSound.Checked = !chkSound.Checked;
            }
        }

        private void chkSound_CheckedChanged(object sender, EventArgs e)
        {
            chkSound.Text = chkSound.Checked ? Texto.SomLigado : Texto.SomDesligado;
            if (chkSound.Checked)
            {
                Program.SoundPlayer.TurnSoundOn();
            }
            else
            {
                Program.SoundPlayer.TurnSoundOff();
            }
        }

        private void SetScreenText()
        {
            btnStart.Text = Texto.Iniciar;
            lblChoose.Text = Texto.EscolheEstilo;
            lbSoundKey.Text = Texto.ToggleSoundKey;
            chkSound.Text = Texto.SomLigado;
            lnkAbout.Text = Texto.Sobre;
            lnkHelp.Text = Texto.Ajuda;
        }

        private void Choose_Load(object sender, EventArgs e)
        {
            Cursor.Show();
        }

        private void Choose_Shown(object sender, EventArgs e)
        {
            Cursor.Show();
        }
    }
}