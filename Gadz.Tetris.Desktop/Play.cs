using Gadz.Tetris.Application;
using Gadz.Tetris.Application.Commands;
using Gadz.Tetris.Desktop.Extensions;
using Gadz.Tetris.Domain.Models.Pieces;
using Gadz.Tetris.SoundPlayer;
using Texto = Gadz.Tetris.Desktop.Textos.Jogo;

namespace Gadz.Tetris.Desktop;

public partial class Play : Form
{
    #region fields

    private Form _baseForm;
    private readonly TaskScheduler _threadPrincipal;
    private readonly GameController _controller;
    private readonly CommandFactory _commandFactory;
    private const int BLOCK_SIZE = 22;

    /// <summary>
    /// Lado, em células, do quadro que mostra a próxima peça (cabe qualquer tetraminó).
    /// </summary>
    private const int NEXT_PIECE_SIZE = 4;

    /// <summary>
    /// As células do tabuleiro, indexadas por [x, y], para pintar sem procurar o controle no painel.
    /// </summary>
    private PictureBox[,] _boardCells = new PictureBox[0, 0];

    /// <summary>
    /// As células do quadro da próxima peça, indexadas por [x, y].
    /// </summary>
    private PictureBox[,] _nextPieceCells = new PictureBox[0, 0];

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

    /// <summary>
    /// Bloco preenchido do modo clássico. Guardado porque cada leitura de <c>Properties.Resources</c> cria um novo <see cref="Image"/>.
    /// </summary>
    private static readonly Image _classicBlock = Properties.Resources.BLOCK_CLASSIC;

    /// <summary>
    /// Célula vazia do modo clássico.
    /// </summary>
    private static readonly Image _classicFadedBlock = Properties.Resources.BLOCK_CLASSIC_FADED;

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
        SoundPlayerAdapter.Start();
    }

    private void ListenEvents()
    {
        _controller.OnRefresh += PaintScreen;
        _controller.OnRefresh += UpdateScreenTextAsync;
        _controller.OnFinish += ExitAsync;
        _controller.OnLose += ShowGameOver;
        _controller.OnLose += SoundPlayerAdapter.End;

        _controller.OnClear += PaintScreen;
        _controller.OnClear += SoundPlayerAdapter.Clear;

        _controller.OnMove += SoundPlayerAdapter.Move;
        _controller.OnSlide += SoundPlayerAdapter.Slide;
        _controller.OnDrop += SoundPlayerAdapter.Dock;
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
            new Deserve(_controller.GetStats()).ShowDialog();
            Close();
        }, CancellationToken.None, TaskCreationOptions.None, _threadPrincipal);
    }

    private async void ExitAsync()
    {
        await Task.Factory.StartNew(() =>
        {
            MouseMove -= Play_MouseMove;
            new Deserve(_controller.GetStats()).ShowDialog();
            Close();
        }, CancellationToken.None, TaskCreationOptions.None, _threadPrincipal);
    }

    private void DrawScreen()
    {
        mainBoardPanel.Width = _controller.BoardWidth * BLOCK_SIZE;
        mainBoardPanel.Height = _controller.BoardHeight * BLOCK_SIZE;

        _boardCells = CreateCells(mainBoardPanel, _controller.BoardWidth, _controller.BoardHeight);
        _nextPieceCells = CreateCells(nextBlockPanel, NEXT_PIECE_SIZE, NEXT_PIECE_SIZE);
    }

    /// <summary>
    /// Substitui o conteúdo do painel por uma grade de células vazias.
    /// </summary>
    /// <param name="panel">O painel que recebe as células.</param>
    /// <param name="width">Quantidade de colunas.</param>
    /// <param name="height">Quantidade de linhas.</param>
    /// <returns>As células criadas, indexadas por [x, y].</returns>
    private static PictureBox[,] CreateCells(Panel panel, int width, int height)
    {
        var cells = new PictureBox[width, height];
        panel.Controls.Clear();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                cells[x, y] = CreateBlock(x, y, string.Empty);
                panel.Controls.Add(cells[x, y]);
            }
        }

        return cells;
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
            PaintBlock(_controller.GetNextBlocks(), _nextPieceCells);
        }, CancellationToken.None, TaskCreationOptions.None, _threadPrincipal);
    }

    private void ClearNextPiece()
    {
        for (int y = 0; y < NEXT_PIECE_SIZE; y++)
        {
            for (int x = 0; x < NEXT_PIECE_SIZE; x++)
            {
                PaintBlock(x, y, string.Empty, _nextPieceCells);
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
                    PaintBlock(x, y, _controller.GetBlock(x, y).Color.ToString(), _boardCells);
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
            BackColor = Color.Transparent,
            Size = new System.Drawing.Size(BLOCK_SIZE, BLOCK_SIZE),
            BorderStyle = BorderStyle.None,
            TabIndex = 0,
            TabStop = false
        };
    }

    private static Image? GetBackgroundImage(string cor)
    {
        if ("None".Equals(cor) || string.Empty.Equals(cor))
        {
            return Program.ClassicMode ? _classicFadedBlock : null;
        }
        else
        {
            return Program.ClassicMode ? _classicBlock : _imageCache[cor];
        }
    }

    private static void PaintBlock(IEnumerable<Block> blocos, PictureBox[,] cells)
    {
        foreach (var bloco in blocos)
        {
            PaintBlock(bloco.X, bloco.Y, bloco.Color.ToString(), cells);
        }
    }

    private static void PaintBlock(int x, int y, string color, PictureBox[,] cells)
    {
        var cell = cells[x, y];
        var image = GetBackgroundImage(color);

        // reatribuir a mesma imagem invalida o controle; pular evita repintar as células que não mudaram (flicker)
        if (cell.BackgroundImage != image)
        {
            cell.BackgroundImage = image;
        }
    }

    private void Jogo_KeyDown(object sender, KeyEventArgs e)
    {
        foreach (var cmd in _commandFactory.GetAll((Key)e.KeyCode, e.Control))
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

    private void Play_MouseMove(object? sender, MouseEventArgs e)
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
