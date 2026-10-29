using Gadz.Tetris.Application;
using Gadz.Tetris.Application.Commands;
using Gadz.Tetris.Domain.Models;
using Gadz.Tetris.Domain.Models.Statistics;
using Gadz.Tetris.Tests.Model;
using NUnit.Framework;

namespace Gadz.Tetris.Tests;

/// <summary>
/// Testes do <see cref="GameController"/>, a fachada usada pela interface.
/// </summary>
public class GameControllerTests
{
    /// <summary>
    /// Controlador 10×20 recriado a cada teste.
    /// </summary>
    private GameController app;

    /// <summary>
    /// Repositório em memória usado pelo controlador, para não gravar em disco.
    /// </summary>
    private InMemoryStatsRepository repository;

    /// <summary>
    /// Cria o controlador com o repositório em memória.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        repository = new InMemoryStatsRepository();
        app = GameController.Create(10, 20, repository);
    }

    /// <summary>
    /// O controlador é criado com as dimensões pedidas.
    /// </summary>
    [Test]
    public void DeveInstanciar()
    {
        Assert.IsNotNull(app);
        Assert.AreEqual(10, app.BoardWidth);
        Assert.AreEqual(20, app.BoardHeight);
    }

    /// <summary>
    /// Tabuleiros menores que 10×10 são rejeitados.
    /// </summary>
    [Test]
    public void NaoDeveInstanciar()
    {
        Assert.Throws<ArgumentException>(() => GameController.Create(4, 4));
    }

    /// <summary>
    /// Iniciar põe o jogo no estado "Playing".
    /// </summary>
    [Test]
    public void DeveIniciar()
    {
        app.Start();

        Assert.AreEqual("Playing", app.State.ToString());
        Assert.IsTrue(app.Playing);
    }

    /// <summary>
    /// Reiniciar uma partida pausada volta ao estado "Playing".
    /// </summary>
    [Test]
    public void DeveReiniciar()
    {
        app.Start();
        Assert.AreEqual("Playing", app.State.ToString(), "Não está iniciando");
        app.Pause();
        Assert.AreEqual("Parado", app.State.ToString(), "Não está pausando");
        app.Restart();
        Assert.AreEqual("Playing", app.State.ToString(), "Não está continuando");
    }

    /// <summary>
    /// Pausar põe o jogo no estado "Parado" e ele deixa de estar jogando.
    /// </summary>
    [Test]
    public void DevePausar()
    {
        app.Start();
        Assert.AreEqual("Playing", app.State.ToString(), "Não está iniciando");
        app.Pause();
        Assert.AreEqual("Parado", app.State.ToString(), "Não está pausando");
        Assert.IsFalse(app.Playing, "Está informando que está jogando.");
    }

    /// <summary>
    /// Continuar uma partida pausada volta ao estado "Playing".
    /// </summary>
    [Test]
    public void DeveContinuar()
    {
        app.Start();
        Assert.AreEqual("Playing", app.State.ToString(), "Não está iniciando");
        app.Pause();
        Assert.AreEqual("Parado", app.State.ToString(), "Não está pausando");
        app.Continue();
        Assert.AreEqual("Playing", app.State.ToString(), "Não está continuando");
    }

    /// <summary>
    /// Alternar a pausa vai de Playing para Parado e de volta para Playing.
    /// </summary>
    [Test]
    public void DeveAlternarPausa()
    {
        app.Start();
        app.TogglePause();
        Assert.AreEqual("Parado", app.State.ToString(), "Não está pausando");
        app.TogglePause();
        Assert.AreEqual("Playing", app.State.ToString(), "Não está continuando");
    }

    /// <summary>
    /// A tecla Enter corresponde a um único comando, que pausa e continua o jogo.
    /// </summary>
    [Test]
    public void DeveTerUmUnicoComandoParaEnter()
    {
        app.Start();
        var commands = new CommandFactory(app).GetAll(Key.Enter, false).ToList();

        Assert.AreEqual(1, commands.Count);
        commands[0].Execute();
        Assert.AreEqual("Parado", app.State.ToString(), "Não está pausando");
        commands[0].Execute();
        Assert.AreEqual("Playing", app.State.ToString(), "Não está continuando");
    }

    /// <summary>
    /// Os comandos são criados uma única vez, e não a cada tecla pressionada.
    /// </summary>
    [Test]
    public void DeveReaproveitarOsComandosEntreTeclas()
    {
        var factory = new CommandFactory(app);

        Assert.AreSame(factory.Commands, factory.Commands);
        Assert.AreSame(factory.GetAll(Key.Enter, false).Single(), factory.GetAll(Key.Enter, false).Single());
    }

    /// <summary>
    /// A partida começa no nível 1.
    /// </summary>
    [Test]
    public void DeveIniciarComNivelUm()
    {
        app.Start();

        Assert.AreEqual(1, app.Level);
    }

    /// <summary>
    /// A partida começa na velocidade 1.
    /// </summary>
    [Test]
    public void DeveIniciarNaVelocidadeMaisAlta()
    {
        app.Start();

        Assert.AreEqual(1, app.Speed);
    }

    /// <summary>
    /// O tempo da partida só começa a contar depois de iniciar.
    /// </summary>
    [Test]
    public void DeveConseguirMedirOTempoDaPartida()
    {
        Assert.AreEqual(0, app.Duration.Ticks);

        app.Start();

        Thread.Sleep(50);

        Assert.AreNotEqual(0, app.Duration.Ticks);
    }

    /// <summary>
    /// A partida começa com pontuação zero.
    /// </summary>
    [Test]
    public void DeveIniciarComPlacarEmBranco()
    {
        app.Start();
        Assert.AreEqual(0, app.Score);
    }

    /// <summary>
    /// A partida começa sem linhas removidas.
    /// </summary>
    [Test]
    public void DeveIniciarSemNenhumaLinhaPreenchida()
    {
        app.Start();
        Assert.AreEqual(0, app.Lines);
    }

    /// <summary>
    /// A primeira peça entra no canto superior esquerdo.
    /// </summary>
    [Test]
    public void DeveIniciarComBlocoNaPosicaoZero()
    {
        app.Start();
        Assert.AreEqual(0, app.CurrentPiecePosition.X, "Posição X não está em 0");
        Assert.AreEqual(0, app.CurrentPiecePosition.Y, "Posição Y não está em 0");
    }

    /// <summary>
    /// Mover para a direita avança uma coluna.
    /// </summary>
    [Test]
    public void DeveMoverParaDireita()
    {
        app.Start();
        Assert.AreEqual(0, app.CurrentPiecePosition.X);
        app.MoveRight();
        Assert.AreEqual(1, app.CurrentPiecePosition.X);
    }

    /// <summary>
    /// Mover para a esquerda volta uma coluna.
    /// </summary>
    [Test]
    public void DeveMoverParaEsquerda()
    {
        DeveMoverParaDireita();

        app.MoveLeft();
        Assert.AreEqual(0, app.CurrentPiecePosition.X);
    }

    /// <summary>
    /// Descer avança duas linhas: o movimento do jogador e o passo do jogo que vem logo depois.
    /// </summary>
    [Test]
    public void DeveMoverParaBaixo()
    {
        app.Start();
        Assert.AreEqual(0, app.CurrentPiecePosition.Y);
        app.MoveDown();
        Assert.AreEqual(2, app.CurrentPiecePosition.Y);
    }

    /// <summary>
    /// Deslizar para a direita leva a peça até perto da borda.
    /// </summary>
    [Test]
    public void DeveCorrerParaDireita()
    {
        app.Start();
        app.RunRight();
        Thread.Sleep(100);
        Assert.IsTrue(app.CurrentPiecePosition.X > 5);
    }

    /// <summary>
    /// Deslizar para a esquerda leva a peça até a coluna 0.
    /// </summary>
    [Test]
    public void DeveCorrerParaEsquerda()
    {
        DeveCorrerParaDireita();
        app.RunLeft();
        Thread.Sleep(100);
        Assert.AreEqual(0, app.CurrentPiecePosition.X);
    }

    /// <summary>
    /// Derrubar a peça a assenta no fundo e traz a próxima para o topo.
    /// </summary>
    [Test]
    public void DeveCorrerParaBaixo()
    {
        app.Start();
        app.SmashDown();
        Assert.AreEqual(1, app.GetStats().Pieces, "A peça não foi assentada no tabuleiro.");
        Assert.AreEqual(0, app.CurrentPiecePosition.Y, "Uma nova peça não foi introduzida no jogo.");
    }

    /// <summary>
    /// As estatísticas entregues à interface refletem a partida em andamento.
    /// </summary>
    [Test]
    public void DeveInformarAsEstatisticasDaPartida()
    {
        app.Start();

        var stats = app.GetStats();

        Assert.AreEqual(app.Score, stats.Score);
        Assert.AreEqual(app.Level, stats.Level);
        Assert.AreEqual(app.Lines, stats.Lines);
        Assert.AreEqual(0, stats.Pieces);
        Assert.AreEqual(app.TetrisRate, stats.TetrisRate);
    }

    /// <summary>
    /// A próxima peça tem 4 blocos.
    /// </summary>
    [Test]
    public void DeveListarProximosBlocos()
    {
        app.Start();
        Assert.AreEqual(4, app.GetNextBlocks().Count());
    }

    /// <summary>
    /// A peça atual tem 4 blocos.
    /// </summary>
    [Test]
    public void DeveListarOsBlocosAtuais()
    {
        app.Start();
        Assert.AreEqual(4, app.GetActualBlocks().Count());
    }

    /// <summary>
    /// O recorde informado é a maior pontuação já guardada no repositório.
    /// </summary>
    [Test]
    public void DeveGuardarORecordeMaximo()
    {
        const int RECORDE = 1200;
        repository.Save(Stats.Load(new StatsRecord { Id = Identity.New(), Score = RECORDE }));

        var game = GameController.Create(10, 20, repository);

        Assert.AreEqual(RECORDE, game.GetMaxScore());
    }

    /// <summary>
    /// Ao sair, as estatísticas da partida são guardadas no repositório.
    /// </summary>
    [Test]
    public void DeveSalvarAsEstatisticasAoSair()
    {
        app.Start();
        app.Exit();

        Assert.AreEqual(1, repository.All().Count());
    }
}
