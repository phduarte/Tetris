using Gadz.Tetris.Domain.Models.Pieces;
using Gadz.Tetris.Domain.Models.Statistics;

namespace Gadz.Tetris.Domain.Models.Boards;

/// <summary>
/// Defines the <see cref="Board" />
/// </summary>
public class Board : Entity
{
    private PieceBuilder _pieceBuilder;

    /// <summary>
    /// Defines the _repository
    /// </summary>
    private IStatsRepository _repository;

    /// <summary>
    /// Os blocos já assentados, com a colisão e a limpeza de linhas.
    /// </summary>
    private Playfield _playfield;

    /// <summary>
    /// Serializa as mutações do tabuleiro: o loop roda em background e os comandos chegam pela thread da UI.
    /// </summary>
    private readonly Lock _sync = new();

    /// <summary>
    /// Identifica o loop vigente. Cada <see cref="RefreshAsync"/> incrementa o valor e o loop anterior se encerra,
    /// evitando dois loops simultâneos após <see cref="Restart"/> ou <see cref="Continue"/>.
    /// </summary>
    private int _loopGeneration;

    /// <summary>
    /// Gets the Dimension
    /// </summary>
    public Size Dimension { get; private set; }

    /// <summary>
    /// Gets the State
    /// </summary>
    public IBoardState State { get; private set; }

    /// <summary>
    /// Gets the Stats
    /// </summary>
    public Stats Stats { get; private set; }

    /// <summary>
    /// Gets a value indicating whether IsPlaying
    /// </summary>
    public bool IsPlaying
    {
        get { return State.IsPlaying; }
    }

    /// <summary>
    /// Gets the Matrix
    /// </summary>
    public Block[,] Matrix { get; }

    /// <summary>
    /// Gets the CurrentPiece
    /// </summary>
    public Piece CurrentPiece { get; private set; }

    /// <summary>
    /// Gets the NextPiece
    /// </summary>
    public Piece NextPiece { get; private set; }

    /// <summary>
    /// Gets the Level
    /// </summary>
    public int Level => Stats.Level;

    /// <summary>
    /// Gets the Lines
    /// </summary>
    public int Lines => Stats.Lines;

    /// <summary>
    /// Gets the Blocks
    /// </summary>
    public IReadOnlyList<Block> Blocks => _playfield.Blocks;

    /// <summary>
    /// Gets the Record
    /// </summary>
    public int Record { get; private set; }

    /// <summary>
    /// Gets the Duration
    /// </summary>
    public TimeSpan Duration => Stats.Duration;

    /// <summary>
    /// Gets the Height
    /// </summary>
    public int Height => Dimension.Height;

    /// <summary>
    /// Gets the Width
    /// </summary>
    public int Width => Dimension.Width;

    /// <summary>
    /// Gets the Speed
    /// </summary>
    public int Speed => Stats.Speed;

    /// <summary>
    /// Gets the Score
    /// </summary>
    public int Score => Stats.Score;

    /// <summary>
    /// Intervalo, em milissegundos, entre as descidas automáticas da peça (ver <see cref="Stats.Interval"/>).
    /// </summary>
    public int Frequency => Stats.Interval;

    /// <summary>
    /// 
    /// </summary>
    public int Tetris => Stats.Tetris;

    /// <summary>
    /// 
    /// </summary>
    public float TetrisRate => Stats.TetrisRate;

    /// <summary>
    /// Defines the OnRefresh
    /// </summary>
    public event GameActionEventHandler? OnRefresh;

    /// <summary>
    /// Defines the OnFinish
    /// </summary>
    public event GameActionEventHandler? OnFinish;

    /// <summary>
    /// Defines the OnClear
    /// </summary>
    public event GameActionEventHandler? OnClear;

    /// <summary>
    /// Defines the OnMove
    /// </summary>
    public event GameActionEventHandler? OnMove;

    /// <summary>
    /// Defines the OnSlide
    /// </summary>
    public event GameActionEventHandler? OnSlide;

    /// <summary>
    /// Defines the OnDrop
    /// </summary>
    public event GameActionEventHandler? OnDrop;

    /// <summary>
    /// Defines the OnPause
    /// </summary>
    public event GameActionEventHandler? OnPause;

    /// <summary>
    /// Defines the OnContinue
    /// </summary>
    public event GameActionEventHandler? OnContinue;

    /// <summary>
    /// Defines the OnContinue
    /// </summary>
    public event GameActionEventHandler? OnLose;

    /// <summary>
    /// Initializes a new instance of the <see cref="Board"/> class.
    /// </summary>
    /// <param name="repository">The repository<see cref="IStatsRepository"/></param>
    /// <param name="width">The width<see cref="int"/></param>
    /// <param name="height">The height<see cref="int"/></param>
    public Board(IStatsRepository repository, int width, int height)
    {
        _repository = repository;
        _pieceBuilder = new PieceBuilder().OnBoard(this);
        Dimension = new Size(width, height);
        _playfield = new Playfield(Dimension);
        Stats = new Stats();
        State = PausedState.Instance;
        Matrix = new Block[width, height];
        Record = _repository.MaxScore();
        CurrentPiece = CreateRandomBlock();
        NextPiece = CreateRandomBlock();
    }

    /// <summary>
    /// The SmashDown
    /// </summary>
    public void SmashDown()
    {
        lock (_sync)
        {
            while (TryMove(0, 1))
            {
                OnSlide?.Invoke();
            }

            Checkout();
            OnDrop?.Invoke();
        }
    }

    /// <summary>
    /// The SlideRight
    /// </summary>
    public void SlideRight()
    {
        lock (_sync)
        {
            if (!TryMove(1, 0))
                return;

            while (TryMove(1, 0))
            {
            }
            Checkout();
            OnSlide?.Invoke();
        }
    }

    /// <summary>
    /// The SlideLeft
    /// </summary>
    public void SlideLeft()
    {
        lock (_sync)
        {
            if (!TryMove(-1, 0))
                return;

            while (TryMove(-1, 0))
            {
            }
            Checkout();
            OnSlide?.Invoke();
        }
    }

    /// <summary>
    /// The Pause
    /// </summary>
    public void Pause()
    {
        lock (_sync)
        {
            if (!State.CanPause)
            {
                return;
            }

            OnPause?.Invoke();
            State = PausedState.Instance;
            Stats.Pause();
        }
    }

    /// <summary>
    /// The Continue
    /// </summary>
    public void Continue()
    {
        lock (_sync)
        {
            if (!State.CanContinue)
            {
                return;
            }

            OnContinue?.Invoke();
            State = PlayingState.Instance;
            Stats.Continue();
            RefreshAsync();
        }
    }

    /// <summary>
    /// Pausa o jogo em andamento ou continua o jogo pausado, conforme o estado atual.
    /// </summary>
    public void TogglePause()
    {
        lock (_sync)
        {
            if (State.CanPause)
            {
                Pause();
            }
            else
            {
                Continue();
            }
        }
    }

    /// <summary>
    /// The Finish
    /// </summary>
    public void Finish()
    {
        lock (_sync)
        {
            OnRefresh?.Invoke();
            Stats.Finish();
            State = FinishedState.Instance;
            OnFinish?.Invoke();
        }
    }

    /// <summary>
    /// The Finish
    /// </summary>
    public void Lose()
    {
        lock (_sync)
        {
            OnRefresh?.Invoke();
            Stats.Finish();
            State = FinishedState.Instance;
            //OnFinish?.Invoke();
            OnLose?.Invoke();
        }
    }

    /// <summary>
    /// The Start
    /// </summary>
    public void Start()
    {
        lock (_sync)
        {
            CurrentPiece = CreateRandomBlock();
            NextPiece = CreateRandomBlock();
            Stats.Start();
            _playfield.Clear();
            ResetMatrix();
            State = PlayingState.Instance;
            RefreshAsync();
        }
    }

    /// <summary>
    /// The Restart
    /// </summary>
    public void Restart()
    {
        lock (_sync)
        {
            Finish();
            Start();
        }
    }

    /// <summary>
    /// The Save
    /// </summary>
    public void Save()
    {
        _repository.Save(Stats);
    }

    /// <summary>
    /// The Load
    /// </summary>
    /// <param name="id">The id<see cref="Identity"/></param>
    public void Load(Identity id)
    {
        lock (_sync)
        {
            Stats = _repository.Load(id);
        }
    }

    /// <summary>
    /// The MoveDown
    /// </summary>
    public void MoveDown()
    {
        lock (_sync)
        {
            if (TryMove(0, 1))
            {
                OnMove?.Invoke();
            }

            Checkout();
        }
    }

    /// <summary>
    /// The MoveRight
    /// </summary>
    public void MoveRight()
    {
        lock (_sync)
        {
            if (TryMove(1, 0))
            {
                OnMove?.Invoke();
                ResetMatrix();
            }
        }
    }

    /// <summary>
    /// The MoveLeft
    /// </summary>
    public void MoveLeft()
    {
        lock (_sync)
        {
            if (TryMove(-1, 0))
            {
                OnMove?.Invoke();
                ResetMatrix();
            }
        }
    }

    /// <summary>
    /// The Rotate
    /// </summary>
    public void Rotate()
    {
        lock (_sync)
        {
            if (!State.CanMove)
            {
                return;
            }

            // a colisão é testada numa cópia já girada; testar a peça atual deixaria a rotação sobrepor blocos
            var rotated = CurrentPiece.Clone();
            rotated.Rotate();

            if (!Fits(rotated))
            {
                return;
            }

            CurrentPiece = rotated;
            OnMove?.Invoke();
            ResetMatrix();
        }
    }

    /// <summary>
    /// Substitui a peça atual e os blocos assentados. Permite que os testes montem cenários determinísticos.
    /// </summary>
    /// <param name="currentPiece">A peça que passa a ser controlada pelo jogador.</param>
    /// <param name="settledBlocks">Os blocos que passam a estar assentados no tabuleiro.</param>
    internal void Arrange(Piece currentPiece, IEnumerable<Block> settledBlocks)
    {
        lock (_sync)
        {
            CurrentPiece = currentPiece;
            _playfield = new Playfield(Dimension, settledBlocks);
            ResetMatrix();
        }
    }

    /// <summary>
    /// Verifica se a peça cabe no tabuleiro: todos os blocos dentro das bordas e sem sobrepor blocos assentados.
    /// </summary>
    /// <param name="piece">A peça a verificar.</param>
    /// <returns><c>true</c> se a peça cabe na posição e rotação informadas.</returns>
    private bool Fits(Piece piece)
    {
        return piece.Blocks.All(_playfield.Contains) && !_playfield.Collides(piece);
    }

    /// <summary>
    /// Desloca a peça atual se o estado permitir movimento e a peça couber na nova posição.
    /// </summary>
    /// <param name="dx">Deslocamento horizontal (negativo = esquerda).</param>
    /// <param name="dy">Deslocamento vertical (positivo = para baixo).</param>
    /// <returns><c>true</c> se a peça foi movida.</returns>
    private bool TryMove(int dx, int dy)
    {
        if (!State.CanMove)
        {
            return false;
        }

        var candidate = CurrentPiece.Clone();
        candidate.MoveBy(dx, dy);

        if (!Fits(candidate))
        {
            return false;
        }

        CurrentPiece.MoveBy(dx, dy);
        return true;
    }

    /// <summary>
    /// Avança o jogo um passo: desce a peça atual ou, se ela não puder descer, assenta-a e traz a próxima.
    /// </summary>
    private void Checkout()
    {
        if (!IsPlaying)
        {
            return;
        }

        if (!TryMove(0, 1))
        {
            LockCurrentPiece();

            // o jogo termina quando a peça que acabou de entrar já nasce sobre blocos assentados
            if (_playfield.Collides(CurrentPiece))
            {
                Lose();
                return;
            }
        }

        ResetMatrix();
        ClearFullLines();
    }

    /// <summary>
    /// Remove as linhas completas e pontua por elas.
    /// </summary>
    private void ClearFullLines()
    {
        var lines = _playfield.ClearFullLines();

        if (lines > 0)
        {
            AddScore(lines);
        }
    }

    /// <summary>
    /// The ResetMatrix
    /// </summary>
    private void ResetMatrix()
    {
        //pintar os blocos que já saíram
        for (int y = 0; y < Dimension.Height; y++)
        {
            for (int x = 0; x < Dimension.Width; x++)
            {
                Matrix[x, y] = _playfield.BlockAt(x, y);
            }
        }

        //pinta o bloco atual
        foreach (var i in CurrentPiece.Blocks)
        {
            Matrix[i.X, i.Y] = i;
        }

        OnRefresh?.Invoke();
    }

    /// <summary>
    /// The RefreshAsync
    /// </summary>
    private void RefreshAsync()
    {
        var generation = ++_loopGeneration;

        new Thread(() =>
        {
            while (true)
            {
                Thread.Sleep(Frequency);

                lock (_sync)
                {
                    if (!IsPlaying || generation != _loopGeneration)
                    {
                        return;
                    }

                    Checkout();
                }
            }
        })
        { IsBackground = true }.Start();
    }

    /// <summary>
    /// The CreateRandomBlock
    /// </summary>
    /// <returns>The <see cref="Piece"/></returns>
    private Piece CreateRandomBlock()
    {
        var type = (PieceType)Random.Shared.Next(0, 7);
        var rotation = Random.Shared.Next(0, 4);
        var position = new Point(0, 0);
        var piece = _pieceBuilder.OfType(type)
            .OnPosition(position)
            .WithRotation(rotation)
            .Build();

        return piece.Clone();
    }

    /// <summary>
    /// Assenta a peça atual no <see cref="Playfield"/> e a substitui pela próxima.
    /// </summary>
    private void LockCurrentPiece()
    {
        _playfield.Lock(CurrentPiece);
        CurrentPiece = NextPiece.Clone();
        NextPiece = CreateRandomBlock();
        Stats.IncludeBlock();
        OnRefresh?.Invoke();
    }

    /// <summary>
    /// Pontua as linhas removidas na jogada.
    /// </summary>
    /// <param name="lines">A quantidade de linhas removidas.</param>
    private void AddScore(int lines)
    {
        Stats.Gain(lines);
        OnClear?.Invoke();
    }
}
