using Gadz.Tetris.Data;
using Gadz.Tetris.Domain;
using Gadz.Tetris.Domain.Models;
using Gadz.Tetris.Domain.Models.Boards;
using Gadz.Tetris.Domain.Models.Pieces;
using Gadz.Tetris.Domain.Models.Statistics;

namespace Gadz.Tetris.Application;

/// <summary>
/// Controlador do jogo (Facade): o único ponto de contato da aplicação cliente com o jogo.
/// <para>Monta o domínio com a infraestrutura e repassa comandos, consultas e eventos do tabuleiro.</para>
/// </summary>
public class GameController
{
    /// <summary>
    /// Menor largura e altura aceitas para o tabuleiro.
    /// </summary>
    private const int MINIMUM_BOARD_SIZE = 10;

    /// <summary>
    /// O tabuleiro controlado; nunca é exposto ao cliente.
    /// </summary>
    private readonly Board _board;

    /// <summary>
    /// Ocorre a cada alteração visual, de estado do jogo ou suas estatísticas
    /// </summary>
    public event GameActionEventHandler? OnRefresh;

    /// <summary>
    /// Ocorre quando o jogo é finalizado.
    /// </summary>
    public event GameActionEventHandler? OnFinish;

    /// <summary>
    /// Ocorre quando uma linha é destruída
    /// </summary>
    public event GameActionEventHandler? OnClear;

    /// <summary>
    /// Ocorre quando uma peça é movimentada
    /// </summary>
    public event GameActionEventHandler? OnMove;

    /// <summary>
    /// Ocorre quando uma peça desliza na tela
    /// </summary>
    public event GameActionEventHandler? OnSlide;

    /// <summary>
    /// Ocorre quando uma peça é derrubada até o fundo
    /// </summary>
    public event GameActionEventHandler? OnDrop;

    /// <summary>
    /// Ocorre quando o jogador pausa o jogo.
    /// </summary>
    public event GameActionEventHandler? OnPause;

    /// <summary>
    /// Ocorre quando o jogo é reiniciado após estar pausado.
    /// </summary>
    public event GameActionEventHandler? OnContinue;

    /// <summary>
    /// Ocorre quando o jogador perde.
    /// </summary>
    public event GameActionEventHandler? OnLose;

    /// <summary>
    /// Informa se o jogo está rodando
    /// </summary>
    public bool Playing => _board.IsPlaying;

    /// <summary>
    /// Largura do tabuleiro
    /// </summary>
    public int BoardWidth => _board.Width;

    /// <summary>
    /// Altura do tabuleiro
    /// </summary>
    public int BoardHeight => _board.Height;

    /// <summary>
    /// Velocidade do jogo
    /// </summary>
    public int Speed => _board.Speed;

    /// <summary>
    /// Quantidade de linhas destruídas
    /// </summary>
    public int Lines => _board.Lines;


    /// <summary>
    /// Tempo de duração total do jogo atual
    /// </summary>
    public TimeSpan Duration => _board.Duration;

    /// <summary>
    /// Pontuação do jogador no jogo atual
    /// </summary>
    public int Score => _board.Score;

    /// <summary>
    /// Nível de dificuldade
    /// </summary>
    public int Level => _board.Level;

    /// <summary>
    /// Estado do jogo
    /// </summary>
    public IBoardState State => _board.State;

    /// <summary>
    /// Posição da peça atual
    /// </summary>
    public Point CurrentPiecePosition => _board.CurrentPiece.Position;

    /// <summary>
    /// Pontos obtidos com TETRIS (4 linhas de uma vez)
    /// </summary>
    public int Tetris => _board.Tetris;

    /// <summary>
    /// Fração da pontuação obtida com TETRIS
    /// </summary>
    public float TetrisRate => _board.TetrisRate;

    /// <summary>
    /// Se a partida atual superou o recorde que estava guardado quando ela começou.
    /// </summary>
    public bool IsNewRecord => _board.IsNewRecord;

    /// <summary>
    /// Tabela de pontuação: os pontos de uma jogada, indexados pela quantidade de linhas removidas menos 1.
    /// </summary>
    public static IReadOnlyList<int> ScoreTable => Stats.ScoreTable;

    /// <summary>
    /// Valida as dimensões, cria o tabuleiro e passa a repassar os eventos dele. Use <see cref="Create"/>.
    /// </summary>
    /// <param name="width">Largura do tabuleiro</param>
    /// <param name="height">Altura do tabuleiro</param>
    /// <param name="repository">Repositório onde as estatísticas são guardadas</param>
    /// <exception cref="ArgumentException">Se a largura ou a altura for menor que <see cref="MINIMUM_BOARD_SIZE"/>.</exception>
    private GameController(int width, int height, IStatsRepository repository)
    {
        if (width < MINIMUM_BOARD_SIZE)
        {
            throw new ArgumentException($"A largura mínima do tabuleiro é {MINIMUM_BOARD_SIZE}.", nameof(width));
        }

        if (height < MINIMUM_BOARD_SIZE)
        {
            throw new ArgumentException($"A altura mínima do tabuleiro é {MINIMUM_BOARD_SIZE}.", nameof(height));
        }

        _board = new Board(repository, width, height);
        _board.OnRefresh += () => OnRefresh?.Invoke();
        _board.OnFinish += () => OnFinish?.Invoke();
        _board.OnLose += () =>
        {
            // a partida perdida também conta para o recorde, como a encerrada pelo jogador em Exit
            Save();
            OnLose?.Invoke();
        };
        _board.OnClear += () => OnClear?.Invoke();
        _board.OnMove += () => OnMove?.Invoke();
        _board.OnSlide += () => OnSlide?.Invoke();
        _board.OnPause += () => OnPause?.Invoke();
        _board.OnContinue += () => OnContinue?.Invoke();
        _board.OnDrop += () => OnDrop?.Invoke();
    }

    /// <summary>
    /// Cria um novo Controlador do Jogo
    /// </summary>
    /// <param name="width">Largura do tabuleiro</param>
    /// <param name="height">Altura do tabuleiro</param>
    /// <param name="repository">Repositório das estatísticas; se omitido, usa o arquivo em disco (<see cref="StatsRepository"/>)</param>
    /// <returns>Uma instância do Controlador do Jogo</returns>
    /// <exception cref="ArgumentException">Se a largura ou a altura for menor que 10.</exception>
    public static GameController Create(int width, int height, IStatsRepository? repository = null)
    {
        return new GameController(width, height, repository ?? new StatsRepository());
    }

    /// <summary>
    /// Desliza o bloco atual para baixo até encontrar um obstáculo
    /// </summary>
    public void SmashDown() => _board.SmashDown();

    /// <summary>
    /// Desliza o bloco atual para a direita até encontrar um obstáculo
    /// </summary>
    public void RunRight() => _board.SlideRight();

    /// <summary>
    /// Desliza o bloco atual para a esquerda até encontrar um obstáculo
    /// </summary>
    public void RunLeft() => _board.SlideLeft();

    /// <summary>
    /// Inicia o jogo
    /// </summary>
    public void Start() => _board.Start();

    /// <summary>
    /// Movimenta o bloco atual para baixo
    /// </summary>
    public void MoveDown() => _board.MoveDown();

    /// <summary>
    /// Movimenta o bloco atual para a direita
    /// </summary>
    public void MoveRight() => _board.MoveRight();

    /// <summary>
    /// Movimenta o bloco atual para a esquerda
    /// </summary>
    public void MoveLeft() => _board.MoveLeft();

    /// <summary>
    /// Pausa o jogo
    /// </summary>
    public void Pause() => _board.Pause();

    /// <summary>
    /// Salva o progresso do jogo
    /// </summary>
    public void Save() => _board.Save();

    /// <summary>
    /// Sai do jogo e salva as estatísticas da partida
    /// </summary>
    public void Exit()
    {
        _board.Finish();
        Save();
    }

    /// <summary>
    /// Gira a peça atual
    /// </summary>
    public void Rotate() => _board.Rotate();

    /// <summary>
    /// Continua um jogo pausado
    /// </summary>
    public void Continue() => _board.Continue();

    /// <summary>
    /// Pausa o jogo em andamento ou continua o jogo pausado
    /// </summary>
    public void TogglePause() => _board.TogglePause();

    /// <summary>
    /// Reinicia um jogo
    /// </summary>
    public void Restart() => _board.Restart();

    /// <summary>
    /// Blocos da peça atual
    /// </summary>
    /// <returns>Os 4 blocos da peça atual.</returns>
    public IEnumerable<Block> GetActualBlocks() => _board.CurrentPiece.Blocks;

    /// <summary>
    /// Bloco exibido na célula do tabuleiro (peça atual, bloco assentado ou célula vazia com cor <c>None</c>).
    /// </summary>
    /// <param name="x">A coluna.</param>
    /// <param name="y">A linha.</param>
    /// <returns>O bloco da célula.</returns>
    public Block GetBlock(int x, int y) => _board.GetBlock(x, y);

    /// <summary>
    /// Blocos da próxima peça.
    /// </summary>
    /// <returns>Os 4 blocos da próxima peça, na posição de entrada.</returns>
    public IEnumerable<Block> GetNextBlocks() => _board.NextPiece.Blocks;

    /// <summary>
    /// Estatísticas da partida atual, para exibir ao final do jogo.
    /// </summary>
    /// <returns>Um retrato das estatísticas no momento da chamada.</returns>
    public StatsSnapshot GetStats()
    {
        var stats = _board.Stats;
        return new(stats.Score, stats.Level, stats.Lines, stats.Blocks, stats.Duration, stats.TetrisRate);
    }

    /// <summary>
    /// Maior pontuação registrada (HI-SCORE)
    /// </summary>
    /// <returns>O recorde guardado no repositório, ou a pontuação atual se ela já o superou.</returns>
    public int GetMaxScore()
    {
        return _board.Record;
    }
}
