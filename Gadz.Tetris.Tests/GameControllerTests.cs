using Gadz.Tetris.Application;
using Gadz.Tetris.Application.Commands;
using Gadz.Tetris.Domain.Models;
using Gadz.Tetris.Domain.Models.Statistics;
using Gadz.Tetris.Tests.Model;
using NUnit.Framework;

namespace Gadz.Tetris.Tests;

/// <summary>
/// Defines the <see cref="GameControllerTests" />
/// </summary>
public class GameControllerTests
{
    /// <summary>
    /// Defines the app
    /// </summary>
    private GameController app;

    /// <summary>
    /// Repositório em memória usado pelo controlador, para não gravar em disco.
    /// </summary>
    private InMemoryStatsRepository repository;

    /// <summary>
    /// The Setup
    /// </summary>
    [SetUp]
    public void Setup()
    {
        repository = new InMemoryStatsRepository();
        app = GameController.Create(10, 20, repository);
    }

    /// <summary>
    /// The DeveInstanciar
    /// </summary>
    [Test]
    public void DeveInstanciar()
    {
        Assert.IsNotNull(app);
        Assert.AreEqual(10, app.BoardWidth);
        Assert.AreEqual(20, app.BoardHeight);
    }

    /// <summary>
    /// The NaoDeveInstanciar
    /// </summary>
    [Test]
    public void NaoDeveInstanciar()
    {
        Assert.Throws<ArgumentException>(() => GameController.Create(4, 4));
    }

    /// <summary>
    /// The DeveIniciar
    /// </summary>
    [Test]
    public void DeveIniciar()
    {
        app.Start();

        Assert.AreEqual("Playing", app.State.ToString());
        Assert.IsTrue(app.Playing);
    }

    /// <summary>
    /// The DeveReiniciar
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
    /// The DevePausar
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
    /// The DeveContinuar
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
    /// The DeveIniciarComNivelUm
    /// </summary>
    [Test]
    public void DeveIniciarComNivelUm()
    {
        app.Start();

        Assert.AreEqual(1, app.Level);
    }

    /// <summary>
    /// The DeveIniciarNaVelocidadeMaisAlta
    /// </summary>
    [Test]
    public void DeveIniciarNaVelocidadeMaisAlta()
    {
        app.Start();

        Assert.AreEqual(1, app.Speed);
    }

    /// <summary>
    /// The DeveConseguirMedirOTempoDaPartida
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
    /// The DeveIniciarComPlacarEmBranco
    /// </summary>
    [Test]
    public void DeveIniciarComPlacarEmBranco()
    {
        app.Start();
        Assert.AreEqual(0, app.Score);
    }

    /// <summary>
    /// The DeveIniciarSemNenhumaLinhaPreenchida
    /// </summary>
    [Test]
    public void DeveIniciarSemNenhumaLinhaPreenchida()
    {
        app.Start();
        Assert.AreEqual(0, app.Lines);
    }

    /// <summary>
    /// The DeveIniciarComBlocoNaPosicaoZero
    /// </summary>
    [Test]
    public void DeveIniciarComBlocoNaPosicaoZero()
    {
        app.Start();
        Assert.AreEqual(0, app.CurrentPiecePosition.X, "Posição X não está em 0");
        Assert.AreEqual(0, app.CurrentPiecePosition.Y, "Posição Y não está em 0");
    }

    /// <summary>
    /// The DeveMoverParaDireita
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
    /// The DeveMoverParaEsquerda
    /// </summary>
    [Test]
    public void DeveMoverParaEsquerda()
    {
        DeveMoverParaDireita();

        app.MoveLeft();
        Assert.AreEqual(0, app.CurrentPiecePosition.X);
    }

    /// <summary>
    /// The DeveMoverParaBaixo
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
    /// The DeveCorrerParaDireita
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
    /// The DeveCorrerParaEsquerda
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
    /// The DeveCorrerParaBaixo
    /// </summary>
    [Test]
    public void DeveCorrerParaBaixo()
    {
        app.Start();
        var p = app.Board.CurrentPiece;
        app.SmashDown();
        //Thread.Sleep(100);
        Assert.AreNotEqual(app.Board.CurrentPiece, p, $"Uma nova peça não foi introduzida no jogo.");
    }

    /// <summary>
    /// The DeveListarProximosBlocos
    /// </summary>
    [Test]
    public void DeveListarProximosBlocos()
    {
        app.Start();
        Assert.AreEqual(4, app.GetNextBlocks().Count());
    }

    /// <summary>
    /// The DeveListarOsBlocosAtuais
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
