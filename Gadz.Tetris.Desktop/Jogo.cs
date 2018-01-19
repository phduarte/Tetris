using Gadz.Tetris.Core.DomainModel.Pecas;
using Gadz.Tetris.Core.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Texto = Gadz.Tetris.Core.CrossCutting.Texto.Jogo;

namespace Gadz.Tetris.Desktop {
    public partial class Jogo : Form {

        Form _baseForm;
        static int _index = 0;
        TaskScheduler threadPrincipal;
        readonly GameController _controller;
        const int BLOCK_SIZE = 22;

        public Jogo(Form formBase) {

            threadPrincipal = TaskScheduler.FromCurrentSynchronizationContext();
            _baseForm = formBase;
            InitializeComponent();

            label2.Text = Texto.Pontuacao.ToUpper();
            label3.Text = Texto.Tempo.ToUpper();
            label4.Text = Texto.Linhas.ToUpper();
            label5.Text = Texto.Nivel.ToUpper();
            label6.Text = Texto.Velocidade.ToUpper();
            label8.Text = Texto.Proximo.ToUpper();
            Text = Texto.Nome.ToUpper();

            if (!Program.ClassicMode) {
                board.BackgroundImage = Properties.Resources.BACKGROUND_TETRIS;
                BackColor = Color.White;
            }

            _controller = GameController.Create(10, 20);

            Iniciar();
        }

        void Iniciar() {

            CriarTabuleiro();

            _controller.Start();
            _controller.QuandoAtualizar += DesenharTela;
            _controller.QuandoAtualizar += AtualizarTextos;
            _controller.QuandoTerminar += Tabuleiro_Terminou;

            _controller.QuandoLimpar += DesenharTela;
            _controller.QuandoLimpar += Program.Player.Clean;

            _controller.QuandoMover += Program.Player.Move;
            _controller.QuandoDeslizar += Program.Player.Slide;
            Program.Player.Intro();

            DesenharTela();
        }

        private void AtualizarTextos() {
            Task.Factory.StartNew(() => {
                lbNivel.Text = _controller.Level.ToString();
                lbPontos.Text = _controller.Score.ToString();
                lbTempo.Text = _controller.Time.ToString(@"hh\:mm\:ss");
                lbLinhas.Text = _controller.Lines.ToString();
                lbVelocidade.Text = _controller.Speed.ToString();
            }, CancellationToken.None, TaskCreationOptions.None, threadPrincipal);
        }

        private void Tabuleiro_Terminou() {
            Task.Factory.StartNew(() => {

                Program.Player.Ending();
                Hide();
                new Fim().ShowDialog();
                Close();
            },
                CancellationToken.None,
                TaskCreationOptions.None,
                threadPrincipal);
        }

        void CriarTabuleiro() {

            board.Controls.Clear();
            board.Width = _controller.BoardHeight * BLOCK_SIZE;
            board.Height = _controller.BoardWidth * BLOCK_SIZE;

            for (int y = 0; y < _controller.BoardWidth; y++) {
                for (int x = 0; x < _controller.BoardHeight; x++) {
                    board.Controls.Add(CreateBlock(x, y, string.Empty));
                }
            }

            painelProximo.Controls.Clear();

            for (int y = 0; y < 4; y++) {
                for (int x = 0; x < 4; x++) {
                    painelProximo.Controls.Add(CreateBlock(x, y, string.Empty));
                }
            }
        }

        void DesenharTela() {
            DesenharPainelPrincipal();
            DesenharPainelProximaPeca();
        }

        private void DesenharPainelProximaPeca() {
            //próximo
            for (int y = 0; y < 4; y++) {
                for (int x = 0; x < 4; x++) {
                    SetBlock(x, y, string.Empty, painelProximo);
                }
            }

            SetBlock(_controller.GetNextBlocks(), painelProximo);
        }

        private void DesenharPainelPrincipal() {

            SetBlock(_controller.GetActualBlocks(), board);

            //background
            for (int y = 0; y < _controller.BoardWidth; y++) {
                for (int x = 0; x < _controller.BoardHeight; x++) {
                    SetBlock(x, y, _controller.Matrix[x, y].Cor.ToString(), board);
                }
            }
        }

        static PictureBox CreateBlock(int x, int y, string cor) {
            var block = new PictureBox {
                Location = new Point(x * BLOCK_SIZE, y * BLOCK_SIZE),
                BackgroundImage = PegarImagem(cor),
                BackgroundImageLayout = ImageLayout.Stretch,
                Name = "bloco" + (_index++).ToString(),
                Size = new Size(BLOCK_SIZE, BLOCK_SIZE),
                BorderStyle = BorderStyle.None,
                TabIndex = 0,
                TabStop = false
            };

            return block;
        }

        static Image PegarImagem(string cor) {

            if (!(cor == "TRANSPARENTE" || cor == string.Empty) && Program.ClassicMode) return Properties.Resources.BLOCK_CLASSIC;
            if (cor == "AMARELO") return Properties.Resources.BLOCK_YELLOW;
            if (cor == "VERMELHO") return Properties.Resources.BLOCK_RED;
            if (cor == "ROXO") return Properties.Resources.BLOCK_PURPLE;
            if (cor == "VERDE") return Properties.Resources.BLOCK_GREEN;
            if (cor == "LARANJA") return Properties.Resources.BLOCK_ORANGE;
            if (cor == "AZUL") return Properties.Resources.BLOCK_BLUE;
            if (cor == "CIANO") return Properties.Resources.BLOCK_CYAN;

            if (Program.ClassicMode) return Properties.Resources.BLOCK_CLASSIC_FADED;

            return null;
        }

        void SetBlock(IEnumerable<Bloco> blocos, Panel panel) {
            foreach (var bloco in blocos) {
                SetBlock(bloco.X, bloco.Y, bloco.Cor.ToString(), panel);
            }
        }

        void SetBlock(int x, int y, string color, Panel panel) {
            foreach (Control i in panel.Controls) {
                if (i.Name.StartsWith("bloco")
                    && i.Location.X == x * BLOCK_SIZE
                    && i.Location.Y == y * BLOCK_SIZE) {
                    i.BackColor = Color.Transparent;
                    i.BackgroundImage = PegarImagem(color);
                    break;
                }
            }
        }

        private void Jogo_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Down:

                    if (e.Control)
                        _controller.SmashDown();
                    else
                        _controller.MoveDown();

                    break;

                case Keys.Left:

                    if (e.Control)
                        _controller.RunLeft();
                    else
                        _controller.MoveLeft();

                    break;
                case Keys.Right:

                    if (e.Control)
                        _controller.RunRight();
                    else
                        _controller.MoveRight();

                    break;

                case Keys.Up:
                    _controller.Rotate();
                    break;

                case Keys.Escape:
                    _controller.Exit();
                    break;

                case Keys.Enter:
                    if (_controller.Playing)
                        _controller.Pause();
                    else
                        _controller.Continue();
                    break;

                case Keys.ShiftKey:
                    Program.Player.ToggleMute();
                    break;

                case Keys.Space:
                     _controller.SmashDown();
                    break;
            }
        }

        private void Jogo_FormClosed(object sender, FormClosedEventArgs e) {
            _baseForm.Show();
        }

        private void Jogo_Load(object sender, EventArgs e) {

            using (var pfc = new PrivateFontCollection()) {

                pfc.AddFontFile(@"Fonts\digital_counter_7.ttf");

                foreach (Control f in Controls) {
                    var actualFontSize = f.Font.Size;
                    f.Font = new Font(pfc.Families[0], actualFontSize, FontStyle.Regular);
                }
            }
        }
    }
}
