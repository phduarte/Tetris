using Gadz.Tetris.Desktop.Commands;
using Gadz.Tetris.Model;
using Gadz.Tetris.Model.Pieces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Texto = Gadz.Tetris.Resources.Textos.Jogo;

namespace Gadz.Tetris.Desktop
{
    public partial class Play : Form
    {
        #region fields

        private Form _baseForm;
        private static int _index = 0;
        private readonly TaskScheduler _threadPrincipal;
        private readonly GameController _controller;
        private readonly CommandFactory _commandFactory;
        private const int BLOCK_SIZE = 22;
        private const string BLOCK_PREFIX = "block";

        private static IDictionary<string, Image> _imageCache = new Dictionary<string, Image> {
            {string.Empty, Properties.Resources.BLOCK_CLASSIC},
            {"Transparente", Properties.Resources.BLOCK_CLASSIC},
            {"Yellow", Properties.Resources.BLOCK_YELLOW},
            {"Red", Properties.Resources.BLOCK_RED},
            {"Purple", Properties.Resources.BLOCK_PURPLE},
            {"Green", Properties.Resources.BLOCK_GREEN},
            {"Orange", Properties.Resources.BLOCK_ORANGE},
            {"Blue", Properties.Resources.BLOCK_BLUE },
            {"Cyan", Properties.Resources.BLOCK_CYAN }
        };

        #endregion

        public Play(Form formBase)
        {
            _baseForm = formBase;
            _threadPrincipal = TaskScheduler.FromCurrentSynchronizationContext();
            _controller = GameController.Create(10, 20);
            _commandFactory = new CommandFactory(_controller);

            InitializeComponent();
            HidePausedScreen();
            SetScreenText();
            ListenEvents();

            if (!Program.ClassicMode)
            {
                mainBoardPanel.BackgroundImage = Properties.Resources.BACKGROUND_TETRIS;
                BackColor = Color.White;
            }

            Start();
        }

        private void Start()
        {
            _controller.Start();
            DrawScreen();
            PaintScreen();
            PlayStartSound();
        }

        private void SetScreenText()
        {
            label2.Text = Texto.Pontuacao.ToUpper();
            label3.Text = Texto.Tempo.ToUpper();
            label4.Text = Texto.Linhas.ToUpper();
            label5.Text = Texto.Nivel.ToUpper();
            label6.Text = Texto.TaxaTetris.ToUpper();
            label8.Text = Texto.Proximo.ToUpper();
            lbPause.Text = Texto.Pausado;
            lbPauseDescription.Text = Texto.PressioneEnterParaContinuar;
            Text = Texto.Nome.ToUpper();
        }

        private void PlayStartSound()
        {
            Program.SoundPlayer.Start();
        }

        private void ListenEvents()
        {
            _controller.OnRefresh += PaintScreen;
            _controller.OnRefresh += UpdateScreenTextAsync;
            _controller.OnFinish += ExitAsync;
            _controller.OnLose += ShowGameOver;
            _controller.OnLose += Program.SoundPlayer.End;

            _controller.OnClear += PaintScreen;
            _controller.OnClear += Program.SoundPlayer.Clear;

            _controller.OnMove += Program.SoundPlayer.Move;
            _controller.OnSlide += Program.SoundPlayer.Slide;
            _controller.OnDrop += Program.SoundPlayer.Dock;
            _controller.OnDrop += () =>
            {
                this.ShakeDown(2);
            };

            _controller.OnPause += ShowPausedScreen;
            _controller.OnContinue += HidePausedScreen;
        }

        private async void UpdateScreenTextAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                lbLevel.Text = _controller.Level.ToString();
                lbPoints.Text = _controller.Score.ToString();
                lbTime.Text = _controller.Duration.ToString(@"hh\:mm\:ss");
                lbLines.Text = _controller.Lines.ToString();
                lbTetris.Text = _controller.TetrisRate.ToString("P0");
            }, CancellationToken.None, TaskCreationOptions.None, _threadPrincipal);
        }

        private async void ShowGameOver()
        {
            await Task.Factory.StartNew(() =>
            {
                MouseMove -= Play_MouseMove;
                Hide();
                new GameOver().ShowDialog();
                new Deserve(_controller.Board.Stats).ShowDialog();
                Close();
            }, CancellationToken.None, TaskCreationOptions.None, _threadPrincipal);
        }

        private async void ExitAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                MouseMove -= Play_MouseMove;
                new Deserve(_controller.Board.Stats).ShowDialog();
                Close();
            }, CancellationToken.None, TaskCreationOptions.None, _threadPrincipal);
        }

        private void DrawScreen()
        {
            mainBoardPanel.Controls.Clear();
            mainBoardPanel.Width = _controller.BoardWidth * BLOCK_SIZE;
            mainBoardPanel.Height = _controller.BoardHeight * BLOCK_SIZE;

            for (int y = 0; y < _controller.BoardHeight; y++)
            {
                for (int x = 0; x < _controller.BoardWidth; x++)
                {
                    mainBoardPanel.Controls.Add(CreateBlock(x, y, string.Empty));
                }
            }

            nextBlockPanel.Controls.Clear();

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    nextBlockPanel.Controls.Add(CreateBlock(x, y, string.Empty));
                }
            }
        }

        private void PaintScreen()
        {
            PaintBoardAsync();
            PaintNextPieceAsync();
        }

        private async void PaintNextPieceAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                ClearNextPiece();
                PaintBlock(_controller.GetNextBlocks(), nextBlockPanel);
            });
        }

        private void ClearNextPiece()
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    PaintBlock(x, y, string.Empty, nextBlockPanel);
                }
            }
        }

        private async void PaintBoardAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                for (int y = 0; y < _controller.BoardHeight; y++)
                {
                    for (int x = 0; x < _controller.BoardWidth; x++)
                    {
                        PaintBlock(x, y, _controller.Matrix[x, y].Color.ToString(), mainBoardPanel);
                    }
                }
            }, CancellationToken.None, TaskCreationOptions.None, _threadPrincipal);
        }

        private static PictureBox CreateBlock(int x, int y, string cor)
        {
            return new PictureBox
            {
                Location = new System.Drawing.Point(x * BLOCK_SIZE, y * BLOCK_SIZE),
                BackgroundImage = GetBackgroundImage(cor),
                BackgroundImageLayout = ImageLayout.Stretch,
                Name = BLOCK_PREFIX + (_index++).ToString(),
                Size = new System.Drawing.Size(BLOCK_SIZE, BLOCK_SIZE),
                BorderStyle = BorderStyle.None,
                TabIndex = 0,
                TabStop = false
            };
        }

        private static Image GetBackgroundImage(string cor)
        {
            if ("None".Equals(cor) || string.Empty.Equals(cor))
            {
                return Program.ClassicMode ? Properties.Resources.BLOCK_CLASSIC_FADED : null;
            }
            else
            {
                return Program.ClassicMode ? Properties.Resources.BLOCK_CLASSIC : _imageCache[cor];
            }
        }

        private void PaintBlock(IEnumerable<Block> blocos, Panel panel)
        {
            foreach (var bloco in blocos)
            {
                PaintBlock(bloco.X, bloco.Y, bloco.Color.ToString(), panel);
            }
        }

        private void PaintBlock(int x, int y, string color, Panel panel)
        {
            foreach (Control i in panel.Controls)
            {
                var isSameLocation = i.Location.X == x * BLOCK_SIZE && i.Location.Y == y * BLOCK_SIZE;
                var isSameBlock = i.Name.StartsWith(BLOCK_PREFIX) && isSameLocation;

                if (isSameBlock)
                {
                    i.BackColor = Color.Transparent;
                    i.BackgroundImage = GetBackgroundImage(color);
                    break;
                }
            }
        }

        private void Jogo_KeyDown(object sender, KeyEventArgs e)
        {
            foreach (var cmd in _commandFactory.GetAll(e.KeyCode, e.Control))
            {
                cmd.Execute();
                break;
            }
        }

        private void Jogo_FormClosed(object sender, FormClosedEventArgs e)
        {
            _baseForm.Show();
        }

        private void Jogo_Load(object sender, EventArgs e)
        {
            Cursor.Hide();
        }

        private void HideMouseWhenIsOverScreen()
        {
            var rect = new Rectangle
            {
                X = Left,
                Width = Width,
                Y = Top,
                Height = Height
            };

            Cursor.Show();

            if (CursorIsOverScreen(rect))
            {
                Cursor.Hide();
            }
        }

        private static bool CursorIsOverScreen(Rectangle rectangle)
        {
            var y = Cursor.Position.Y;
            var x = Cursor.Position.X;

            return (y >= rectangle.Top && y <= rectangle.Bottom) && (x >= rectangle.Left && x <= rectangle.Right);
        }

        private void Play_MouseMove(object sender, MouseEventArgs e)
        {
            HideMouseWhenIsOverScreen();
        }

        private void Play_Deactivate(object sender, EventArgs e)
        {
            if (_controller.Playing)
            {
                _controller.Pause();
                ShowPausedScreen();
            }
        }

        private void ShowPausedScreen()
        {
            picPause.Left = 0;
            picPause.Top = 0;
            lbPause.Visible = picPause.Visible = true;
        }

        private void HidePausedScreen()
        {
            picPause.Left = 0;
            picPause.Top = 0;
            lbPause.Visible = picPause.Visible = false;
        }
    }
}