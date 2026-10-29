using Gadz.Tetris.Domain.Models.Pieces;
using Gadz.Tetris.Domain.Models.Statistics;

namespace Gadz.Tetris.Domain.Models.Boards;

/// <summary>
/// O tabuleiro de jogo: a peça atual e a próxima, os blocos assentados, o estado e as estatísticas da partida.
/// Todas as regras de movimento, assentamento e fim de jogo passam por aqui.
/// </summary>
public class Board : Entity
{
    /// <summary>
    /// Onde cada nova peça entra no tabuleiro: o canto superior esquerdo.
    /// </summary>
    private static readonly Point SPAWN_POSITION = new(0, 0);

    /// <summary>
    /// Cria as novas peças na posição de entrada.
    /// </summary>
    private PieceBuilder _pieceBuilder;

    /// <summary>
    /// Escolhe o tipo e a rotação de cada nova peça.
    /// </summary>
    private readonly IPieceGenerator _pieceGenerator;

    /// <summary>
    /// Onde as estatísticas são guardadas ao sair e de onde vem o recorde.
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
    /// Largura e altura do tabuleiro, em células.
    /// </summary>
    public Size Dimension { get; private set; }

    /// <summary>
    /// O estado atual (jogando, pausado ou encerrado), que define o que é permitido.
    /// </summary>
    public IBoardState State { get; private set; }

    /// <summary>
    /// As estatísticas da partida em andamento.
    /// </summary>
    public Stats Stats { get; private set; }

    /// <summary>
    /// Se a partida está em andamento.
    /// </summary>
    public bool IsPlaying
    {
        get { return State.IsPlaying; }
    }

    /// <summary>
    /// A peça controlada pelo jogador.
    /// </summary>
    public Piece CurrentPiece { get; private set; }

    /// <summary>
    /// A peça que entra quando a atual for assentada.
    /// </summary>
    public Piece NextPiece { get; private set; }

    /// <summary>
    /// O nível atual.
    /// </summary>
    public int Level => Stats.Level;

    /// <summary>
    /// Linhas removidas na partida.
    /// </summary>
    public int Lines => Stats.Lines;

    /// <summary>
    /// Os blocos já assentados no tabuleiro.
    /// </summary>
    public IReadOnlyList<Block> Blocks => _playfield.Blocks;

    /// <summary>
    /// A maior pontuação guardada no repositório quando o tabuleiro foi criado.
    /// </summary>
    private readonly int _storedRecord;

    /// <summary>
    /// O recorde (HI-SCORE): o guardado no repositório ou, se a partida atual já o superou, a pontuação atual.
    /// </summary>
    public int Record => Math.Max(_storedRecord, Stats.Score);

    /// <summary>
    /// Se a partida atual superou o recorde guardado no repositório.
    /// </summary>
    public bool IsNewRecord => Stats.Score > _storedRecord;

    /// <summary>
    /// Tempo jogado, sem contar as pausas.
    /// </summary>
    public TimeSpan Duration => Stats.Duration;

    /// <summary>
    /// Quantidade de linhas do tabuleiro.
    /// </summary>
    public int Height => Dimension.Height;

    /// <summary>
    /// Quantidade de colunas do tabuleiro.
    /// </summary>
    public int Width => Dimension.Width;

    /// <summary>
    /// A velocidade atual.
    /// </summary>
    public int Speed => Stats.Speed;

    /// <summary>
    /// A pontuação da partida.
    /// </summary>
    public int Score => Stats.Score;

    /// <summary>
    /// Intervalo, em milissegundos, entre as descidas automáticas da peça (ver <see cref="Stats.Interval"/>).
    /// </summary>
    public int Frequency => Stats.Interval;

    /// <summary>
    /// Pontos obtidos com TETRIS (4 linhas de uma vez).
    /// </summary>
    public int Tetris => Stats.Tetris;

    /// <summary>
    /// Fração da pontuação obtida com TETRIS.
    /// </summary>
    public float TetrisRate => Stats.TetrisRate;

    /// <summary>
    /// Ocorre a cada mudança visível no tabuleiro (peça movida ou assentada, linhas removidas, início e fim da partida).
    /// </summary>
    public event GameActionEventHandler? OnRefresh;

    /// <summary>
    /// Ocorre quando o jogador encerra a partida.
    /// </summary>
    public event GameActionEventHandler? OnFinish;

    /// <summary>
    /// Ocorre quando linhas completas são removidas.
    /// </summary>
    public event GameActionEventHandler? OnClear;

    /// <summary>
    /// Ocorre quando a peça atual se move ou gira.
    /// </summary>
    public event GameActionEventHandler? OnMove;

    /// <summary>
    /// Ocorre a cada passo de uma peça deslizando (smash down ou Ctrl+seta).
    /// </summary>
    public event GameActionEventHandler? OnSlide;

    /// <summary>
    /// Ocorre quando a peça é derrubada até o fundo (smash down).
    /// </summary>
    public event GameActionEventHandler? OnDrop;

    /// <summary>
    /// Ocorre quando a partida é pausada.
    /// </summary>
    public event GameActionEventHandler? OnPause;

    /// <summary>
    /// Ocorre quando uma partida pausada continua.
    /// </summary>
    public event GameActionEventHandler? OnContinue;

    /// <summary>
    /// Ocorre quando o jogador perde: a peça que acabou de entrar já nasce sobre blocos assentados.
    /// </summary>
    public event GameActionEventHandler? OnLose;

    /// <summary>
    /// Cria o tabuleiro pausado, com a peça atual e a próxima já sorteadas. A partida começa em <see cref="Start"/>.
    /// </summary>
    /// <param name="repository">Onde as estatísticas são guardadas e de onde vem o recorde.</param>
    /// <param name="width">Quantidade de colunas.</param>
    /// <param name="height">Quantidade de linhas.</param>
    /// <param name="pieceGenerator">Gerador das novas peças; se omitido, as peças são sorteadas.</param>
    public Board(IStatsRepository repository, int width, int height, IPieceGenerator? pieceGenerator = null)
    {
        _repository = repository;
        _pieceGenerator = pieceGenerator ?? new RandomPieceGenerator();
        _pieceBuilder = new PieceBuilder();
        Dimension = new Size(width, height);
        _playfield = new Playfield(Dimension);
        Stats = new Stats();
        State = PausedState.Instance;
        _storedRecord = _repository.MaxScore();
        CurrentPiece = CreateNextPiece();
        NextPiece = CreateNextPiece();
    }

    /// <summary>
    /// Derruba a peça atual até onde ela couber e a assenta.
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
    /// Desliza a peça atual para a direita até a borda ou até um bloco.
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
    /// Desliza a peça atual para a esquerda até a borda ou até um bloco.
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
    /// Pausa a partida, se o estado permitir, e para de contar o tempo.
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
    /// Continua a partida pausada, se o estado permitir, e retoma a descida automática.
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
    /// Encerra a partida por decisão do jogador.
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
    /// Encerra a partida por derrota.
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
    /// Começa uma partida nova: limpa o tabuleiro, sorteia as peças e inicia a descida automática.
    /// </summary>
    public void Start()
    {
        lock (_sync)
        {
            CurrentPiece = CreateNextPiece();
            NextPiece = CreateNextPiece();
            Stats.Start();
            _playfield.Clear();
            OnRefresh?.Invoke();
            State = PlayingState.Instance;
            RefreshAsync();
        }
    }

    /// <summary>
    /// Encerra a partida atual e começa outra.
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
    /// Guarda as estatísticas da partida no repositório.
    /// </summary>
    public void Save()
    {
        _repository.Save(Stats);
    }

    /// <summary>
    /// O bloco visível na célula: um bloco da peça atual, um bloco assentado ou, se vazia, um bloco com <see cref="PieceColor.None"/>.
    /// </summary>
    /// <param name="x">A coluna, de 0 a <see cref="Width"/> - 1.</param>
    /// <param name="y">A linha, de 0 a <see cref="Height"/> - 1.</param>
    /// <returns>O bloco da célula.</returns>
    public Block GetBlock(int x, int y)
    {
        lock (_sync)
        {
            foreach (var block in CurrentPiece.Blocks)
            {
                if (block.X == x && block.Y == y)
                {
                    return block;
                }
            }

            return _playfield.BlockAt(x, y);
        }
    }

    /// <summary>
    /// Substitui as estatísticas pelas de uma partida guardada.
    /// </summary>
    /// <param name="id">A identidade da partida no repositório.</param>
    public void Load(Identity id)
    {
        lock (_sync)
        {
            Stats = _repository.Load(id);
        }
    }

    /// <summary>
    /// Desce a peça atual uma linha; se ela não puder descer, é assentada.
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
    /// Move a peça atual uma coluna para a direita, se couber.
    /// </summary>
    public void MoveRight()
    {
        lock (_sync)
        {
            if (TryMove(1, 0))
            {
                OnMove?.Invoke();
                OnRefresh?.Invoke();
            }
        }
    }

    /// <summary>
    /// Move a peça atual uma coluna para a esquerda, se couber.
    /// </summary>
    public void MoveLeft()
    {
        lock (_sync)
        {
            if (TryMove(-1, 0))
            {
                OnMove?.Invoke();
                OnRefresh?.Invoke();
            }
        }
    }

    /// <summary>
    /// Gira a peça atual, se a rotação couber no tabuleiro. Encostada na parede, a peça é empurrada para dentro.
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
            KeepWithinWidth(rotated);

            if (!Fits(rotated))
            {
                return;
            }

            CurrentPiece = rotated;
            OnMove?.Invoke();
            OnRefresh?.Invoke();
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
            OnRefresh?.Invoke();
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
    /// Empurra a peça na horizontal até ela ficar entre as bordas, para que girar encostado na parede seja possível.
    /// </summary>
    /// <param name="piece">A peça recém-girada.</param>
    private void KeepWithinWidth(Piece piece)
    {
        var minX = piece.Blocks.Min(block => block.X);
        var maxX = piece.Blocks.Max(block => block.X);

        if (minX < 0)
        {
            piece.MoveBy(-minX, 0);
        }
        else if (maxX >= Width)
        {
            piece.MoveBy(Width - 1 - maxX, 0);
        }
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

        OnRefresh?.Invoke();
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
    /// Inicia, numa thread de background, o loop que desce a peça a cada <see cref="Frequency"/> ms enquanto a partida estiver em andamento.
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
    /// Cria a próxima peça com o tipo e a rotação escolhidos pelo <see cref="IPieceGenerator"/>.
    /// </summary>
    /// <returns>A peça criada no canto superior esquerdo do tabuleiro.</returns>
    private Piece CreateNextPiece()
    {
        var (type, rotation) = _pieceGenerator.Next();
        var piece = _pieceBuilder.OfType(type)
            .OnPosition(SPAWN_POSITION)
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
        NextPiece = CreateNextPiece();
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
