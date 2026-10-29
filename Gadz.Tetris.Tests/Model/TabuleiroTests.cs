using Gadz.Tetris.Domain.Models;
using Gadz.Tetris.Domain.Models.Boards;
using Gadz.Tetris.Domain.Models.Pieces;
using Gadz.Tetris.Domain.Models.Statistics;
using NUnit.Framework;

namespace Gadz.Tetris.Tests.Model;

/// <summary>
/// Testes das regras do <see cref="Board"/>.
/// </summary>
public class TabuleiroTests
{
    /// <summary>
    /// Por quanto tempo o teste de concorrência dispara comandos.
    /// </summary>
    private const int STRESS_DURATION_MS = 500;

    /// <summary>
    /// Tabuleiro 10×20 em jogo, recriado a cada teste.
    /// </summary>
    private Board _board;

    /// <summary>
    /// Cria e inicia um tabuleiro novo.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _board = new Board(new InMemoryStatsRepository(), 10, 20);
        _board.Start();
    }

    /// <summary>
    /// Encerra o jogo para que o loop em background pare.
    /// </summary>
    [TearDown]
    public void TearDown()
    {
        _board.Finish();
    }

    /// <summary>
    /// Uma peça I vertical encostada numa coluna de blocos não pode girar para a horizontal por cima dela.
    /// </summary>
    [Test]
    public void NaoDeveGirarSobreBlocosAssentados()
    {
        var column = Enumerable.Range(8, 12).Select(y => new Block(1, y, PieceColor.Blue));
        _board.Arrange(CreatePiece(PieceType.I, new Point(0, 8), 0), column);

        _board.Rotate();

        Assert.AreEqual(0, _board.CurrentPiece.Rotation);
        Assert.IsFalse(HasOverlap(), "A peça atual sobrepôs blocos assentados.");
    }

    /// <summary>
    /// Uma peça I horizontal na última linha não pode girar para a vertical para fora do tabuleiro.
    /// </summary>
    [Test]
    public void NaoDeveGirarParaForaDoTabuleiro()
    {
        _board.Arrange(CreatePiece(PieceType.I, new Point(3, 19), 1), []);

        _board.Rotate();

        Assert.AreEqual(1, _board.CurrentPiece.Rotation);
    }

    /// <summary>
    /// Uma peça I vertical encostada na borda direita gira para a horizontal e é empurrada para dentro do tabuleiro.
    /// </summary>
    [TestCase(9)]
    [TestCase(8)]
    [TestCase(7)]
    public void DeveGirarEncostadoNaBordaDireita(int column)
    {
        _board.Arrange(CreatePiece(PieceType.I, new Point(column, 10), 0), []);

        _board.Rotate();

        Assert.AreEqual(1, _board.CurrentPiece.Rotation);
        Assert.AreEqual(9, _board.CurrentPiece.Blocks.Max(block => block.X), "A peça não ficou encostada na borda.");
        Assert.AreEqual(6, _board.CurrentPiece.Position.X);
    }

    /// <summary>
    /// Em espaço livre a rotação cicla 0→1→2→3→0.
    /// </summary>
    [Test]
    public void DeveGirarEmEspacoLivre()
    {
        _board.Arrange(CreatePiece(PieceType.I, new Point(3, 5), 0), []);

        foreach (var expected in new[] { 1, 2, 3, 0 })
        {
            _board.Rotate();
            Assert.AreEqual(expected, _board.CurrentPiece.Rotation);
        }
    }

    /// <summary>
    /// Depois de perder, reiniciar o jogo volta ao estado <c>Playing</c> com o tabuleiro vazio e a peça descendo.
    /// </summary>
    [Test]
    public void DeveVoltarAJogarAoReiniciarDepoisDePerder()
    {
        LoseGame();

        _board.Restart();
        var y = _board.CurrentPiece.Position.Y;
        _board.MoveDown();

        Assert.AreEqual("Playing", _board.State.ToString());
        Assert.AreEqual(0, _board.Blocks.Count, "Os blocos da partida anterior continuaram no tabuleiro.");
        Assert.IsTrue(_board.CurrentPiece.Position.Y > y, "A peça não desceu após o reinício.");
    }

    /// <summary>
    /// Quando a peça seguinte entra sobre blocos assentados, o jogo termina.
    /// </summary>
    [Test]
    public void DevePerderQuandoANovaPecaNasceSobreBlocos()
    {
        // toda peça nasce dentro do quadrado 4×4 do canto superior esquerdo
        var spawnArea = from x in Enumerable.Range(0, 4)
                        from y in Enumerable.Range(0, 4)
                        select new Block(x, y, PieceColor.Red);
        _board.Arrange(CreatePiece(PieceType.I, new Point(9, 16), 0), spawnArea);

        _board.MoveDown();

        Assert.IsFalse(_board.IsPlaying);
        Assert.AreEqual("Finished", _board.State.ToString());
    }

    /// <summary>
    /// Com um único bloco assentado, a peça não pode se mover para a célula ocupada por ele.
    /// </summary>
    [Test]
    public void NaoDeveMoverSobreUmUnicoBlocoAssentado()
    {
        _board.Arrange(CreatePiece(PieceType.I, new Point(1, 5), 0), [new Block(0, 6, PieceColor.Red)]);

        _board.MoveLeft();

        Assert.AreEqual(1, _board.CurrentPiece.Position.X);
        Assert.IsFalse(HasOverlap(), "A peça atual sobrepôs o bloco assentado.");
    }

    /// <summary>
    /// Uma peça encostada na borda esquerda não sai do tabuleiro ao mover ou deslizar para a esquerda.
    /// </summary>
    [Test]
    public void NaoDeveSairPelaBordaEsquerda()
    {
        _board.Arrange(CreatePiece(PieceType.I, new Point(0, 5), 0), []);

        _board.MoveLeft();
        _board.SlideLeft();

        Assert.AreEqual(0, _board.CurrentPiece.Position.X);
    }

    /// <summary>
    /// Uma peça encostada na borda direita não sai do tabuleiro ao mover ou deslizar para a direita.
    /// </summary>
    [Test]
    public void NaoDeveSairPelaBordaDireita()
    {
        _board.Arrange(CreatePiece(PieceType.I, new Point(_board.Width - 1, 5), 0), []);

        _board.MoveRight();
        _board.SlideRight();

        Assert.AreEqual(_board.Width - 1, _board.CurrentPiece.Position.X);
    }

    /// <summary>
    /// Deslizar para a direita leva a peça até a borda, sem ultrapassá-la.
    /// </summary>
    [Test]
    public void DeveDeslizarAteABordaDireita()
    {
        _board.Arrange(CreatePiece(PieceType.I, new Point(3, 5), 1), []);

        _board.SlideRight();

        Assert.AreEqual(_board.Width - 1, _board.CurrentPiece.Blocks.Max(b => b.X));
    }

    /// <summary>
    /// Derrubar a peça a assenta na última linha, sem sair pelo fundo do tabuleiro.
    /// </summary>
    [Test]
    public void NaoDeveSairPeloFundo()
    {
        _board.Arrange(CreatePiece(PieceType.I, new Point(3, 5), 0), []);

        _board.SmashDown();

        Assert.AreEqual(4, _board.Blocks.Count);
        Assert.AreEqual(_board.Height - 1, _board.Blocks.Max(b => b.Y), "A peça não chegou à última linha.");
        Assert.IsTrue(_board.Blocks.All(b => b.X == 3 && b.Y < _board.Height), "A peça saiu do tabuleiro.");
    }

    /// <summary>
    /// Cada célula mostra o bloco da peça atual, o bloco assentado ou fica vazia.
    /// </summary>
    [Test]
    public void DeveDevolverOBlocoDeCadaCelula()
    {
        _board.Arrange(CreatePiece(PieceType.O, new Point(4, 5), 0), [new Block(0, 19, PieceColor.Red)]);

        Assert.AreEqual(PieceColor.Yellow, _board.GetBlock(4, 5).Color, "A peça atual não aparece na célula.");
        Assert.AreEqual(PieceColor.Red, _board.GetBlock(0, 19).Color, "O bloco assentado não aparece na célula.");
        Assert.AreEqual(PieceColor.None, _board.GetBlock(9, 0).Color, "A célula vazia deveria ficar sem cor.");
    }

    /// <summary>
    /// Pausar um jogo finalizado não altera o estado.
    /// </summary>
    [Test]
    public void NaoDevePausarJogoFinalizado()
    {
        _board.Finish();

        _board.Pause();

        Assert.AreEqual("Finished", _board.State.ToString());
    }

    /// <summary>
    /// Continuar um jogo finalizado não altera o estado nem reinicia o loop.
    /// </summary>
    [Test]
    public void NaoDeveContinuarJogoFinalizado()
    {
        _board.Finish();

        _board.Continue();

        Assert.AreEqual("Finished", _board.State.ToString());
        Assert.IsFalse(_board.IsPlaying);
    }

    /// <summary>
    /// Alternar a pausa de um jogo finalizado não altera o estado.
    /// </summary>
    [Test]
    public void NaoDeveAlternarPausaDeJogoFinalizado()
    {
        _board.Finish();

        _board.TogglePause();

        Assert.AreEqual("Finished", _board.State.ToString());
    }

    /// <summary>
    /// Comandos disparados de várias threads ao mesmo tempo que o loop não lançam exceção nem sobrepõem blocos.
    /// </summary>
    [Test]
    public void DeveSuportarComandosConcorrentesComOLoop()
    {
        var until = DateTime.Now.AddMilliseconds(STRESS_DURATION_MS);
        Action[] commands =
        [
            _board.MoveLeft,
            _board.MoveRight,
            _board.Rotate,
            _board.MoveDown,
            _board.SmashDown,
            () =>
            {
                if (!_board.IsPlaying)
                {
                    _board.Restart();
                }
            }
        ];

        var tasks = commands.Select(command => Task.Run(() =>
        {
            while (DateTime.Now < until)
            {
                command();
            }
        })).ToArray();

        Assert.DoesNotThrow(() => Task.WaitAll(tasks));

        // ao perder, a peça que nasceu sobre a pilha fica sobreposta por definição
        if (_board.IsPlaying)
        {
            Assert.IsFalse(HasOverlap(), "A peça atual sobrepôs blocos assentados.");
        }

        Assert.AreEqual(_board.Blocks.Count, _board.Blocks.Select(b => b.Point).Distinct().Count(), "Há blocos assentados na mesma célula.");
    }

    /// <summary>
    /// As peças atual e seguinte vêm do gerador injetado, tanto ao iniciar quanto ao trocar de peça.
    /// </summary>
    [Test]
    public void DeveCriarAsPecasPeloGeradorInjetado()
    {
        // o construtor consome as duas primeiras peças; o Start, as duas seguintes; a troca, a quinta
        var generator = new SequencePieceGenerator(
            (PieceType.O, 0), (PieceType.O, 0), (PieceType.I, 1), (PieceType.T, 2), (PieceType.Z, 3));
        var board = new Board(new InMemoryStatsRepository(), 10, 20, generator);

        try
        {
            board.Start();

            Assert.AreEqual(PieceType.I, board.CurrentPiece.Type);
            Assert.AreEqual(1, board.CurrentPiece.Rotation);
            Assert.AreEqual(PieceType.T, board.NextPiece.Type);
            Assert.AreEqual(2, board.NextPiece.Rotation);

            board.SmashDown();

            Assert.AreEqual(PieceType.T, board.CurrentPiece.Type);
            Assert.AreEqual(2, board.CurrentPiece.Rotation);
            Assert.AreEqual(PieceType.Z, board.NextPiece.Type);
            Assert.AreEqual(3, board.NextPiece.Rotation);
        }
        finally
        {
            board.Finish();
        }
    }

    /// <summary>
    /// O recorde é a maior pontuação guardada enquanto a partida atual não a supera.
    /// </summary>
    [Test]
    public void DeveInformarORecordeGuardado()
    {
        const int RECORDE = 1200;
        var board = CreateBoardWithRecord(RECORDE);

        try
        {
            board.Start();
            CompleteOneLine(board);

            Assert.AreEqual(RECORDE, board.Record);
        }
        finally
        {
            board.Finish();
        }
    }

    /// <summary>
    /// Quando a partida atual supera o recorde guardado, o recorde passa a ser a pontuação atual.
    /// </summary>
    [Test]
    public void DeveAtualizarORecordeAoSuperarAPontuacaoGuardada()
    {
        const int RECORDE = 30;
        var board = CreateBoardWithRecord(RECORDE);

        try
        {
            board.Start();
            CompleteOneLine(board);

            Assert.AreEqual(board.Score, board.Record);
            Assert.IsTrue(board.Record > RECORDE);
        }
        finally
        {
            board.Finish();
        }
    }

    /// <summary>
    /// A partida é um novo recorde só quando supera a pontuação guardada.
    /// </summary>
    /// <param name="record">A pontuação guardada.</param>
    /// <param name="esperado">Se a partida, depois de remover 1 linha (40 pontos), é um novo recorde.</param>
    [TestCase(30, true)]
    [TestCase(40, false)]
    [TestCase(1200, false)]
    public void DeveInformarSeAPartidaEUmNovoRecorde(int record, bool esperado)
    {
        var board = CreateBoardWithRecord(record);

        try
        {
            board.Start();
            Assert.IsFalse(board.IsNewRecord, "Sem pontos, a partida não pode ser um novo recorde.");

            CompleteOneLine(board);

            Assert.AreEqual(esperado, board.IsNewRecord);
        }
        finally
        {
            board.Finish();
        }
    }

    /// <summary>
    /// Cria um tabuleiro cujo repositório já guarda uma partida com a pontuação informada.
    /// </summary>
    /// <param name="record">A pontuação da partida guardada.</param>
    /// <returns>O tabuleiro, ainda não iniciado.</returns>
    private static Board CreateBoardWithRecord(int record)
    {
        var repository = new InMemoryStatsRepository();
        repository.Save(Stats.Load(new StatsRecord { Id = Identity.New(), Score = record }));
        return new Board(repository, 10, 20);
    }

    /// <summary>
    /// Completa a última linha: preenche as colunas 1 a 9 e derruba uma peça I vertical na coluna 0.
    /// </summary>
    /// <param name="board">O tabuleiro em jogo.</param>
    private static void CompleteOneLine(Board board)
    {
        var row = Enumerable.Range(1, board.Width - 1).Select(x => new Block(x, board.Height - 1, PieceColor.Blue));
        board.Arrange(CreatePiece(PieceType.I, new Point(0, 0), 0), row);

        board.SmashDown();

        Assert.AreEqual(1, board.Lines, "A última linha deveria ter sido removida.");
    }

    /// <summary>
    /// Derruba peças na coluna inicial até o jogo acabar.
    /// </summary>
    private void LoseGame()
    {
        // cada peça ocupa no máximo 4 colunas, então nenhuma linha é completada e a pilha chega ao topo
        for (var i = 0; i < _board.Height && _board.IsPlaying; i++)
        {
            _board.SmashDown();
        }

        Assert.IsFalse(_board.IsPlaying, "O jogo deveria ter terminado.");
    }

    /// <summary>
    /// Cria uma peça para posicionar no tabuleiro do teste.
    /// </summary>
    /// <param name="type">O tipo da peça.</param>
    /// <param name="position">A posição da peça.</param>
    /// <param name="rotation">A rotação da peça.</param>
    /// <returns>A peça criada.</returns>
    private static Piece CreatePiece(PieceType type, Point position, int rotation)
    {
        return new PieceBuilder().OfType(type).OnPosition(position).WithRotation(rotation).Build();
    }

    /// <summary>
    /// Informa se algum bloco da peça atual ocupa a mesma célula de um bloco assentado.
    /// </summary>
    /// <returns><c>true</c> se houver sobreposição.</returns>
    private bool HasOverlap()
    {
        return _board.CurrentPiece.Blocks.Any(piece => _board.Blocks.Any(settled => settled.CollideWith(piece)));
    }
}
