using Gadz.Tetris.Application;
using Gadz.Tetris.Application.Commands;
using Gadz.Tetris.Domain.Models;
using Gadz.Tetris.Domain.Models.Pieces;
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
    /// Quanto os testes do relógio esperam: um pouco mais que o intervalo de descida do nível 1 (1000 ms).
    /// </summary>
    private const int CLOCK_TIMEOUT_MS = 1500;

    /// <summary>
    /// Controlador 10×20 recriado a cada teste.
    /// </summary>
    private GameController app;

    /// <summary>
    /// Repositório em memória usado pelo controlador, para não gravar em disco.
    /// </summary>
    private InMemoryStatsRepository repository;

    /// <summary>
    /// Som de mentira usado pelo controlador, que registra os sons pedidos.
    /// </summary>
    private FakeSoundPlayer sound;

    /// <summary>
    /// Cria o controlador com o repositório em memória e o som de mentira.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        repository = new InMemoryStatsRepository();
        sound = new FakeSoundPlayer();
        app = GameController.Create(10, 20, repository, sound);
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
        Assert.Throws<ArgumentException>(() => GameController.Create(4, 4, repository, sound));
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
        Assert.IsTrue(app.CurrentPiecePosition.X > 5);
    }

    /// <summary>
    /// Deslizar para a esquerda leva a peça até encostar na borda: o bloco mais à esquerda fica na coluna 0.
    /// </summary>
    [Test]
    public void DeveCorrerParaEsquerda()
    {
        DeveCorrerParaDireita();
        app.RunLeft();

        // a posição é a origem da forma, que pode ficar em -1 quando a rotação sorteada começa os blocos na coluna 1
        Assert.AreEqual(0, app.GetActualBlocks().Min(b => b.X));
    }

    /// <summary>
    /// Depois de iniciar, o relógio do controlador desce a peça sozinho, sem nenhum comando do jogador.
    /// </summary>
    [Test]
    public void DeveDescerAPecaSozinhaDepoisDeIniciar()
    {
        // TaskCompletionSource em vez de um evento descartável: o relógio ainda pode disparar OnRefresh depois do teste
        var descended = new TaskCompletionSource();
        app.OnRefresh += () =>
        {
            if (app.CurrentPiecePosition.Y > 0)
            {
                descended.TrySetResult();
            }
        };

        app.Start();

        Assert.IsTrue(descended.Task.Wait(CLOCK_TIMEOUT_MS), "A peça não desceu sozinha.");
        app.Exit();
    }

    /// <summary>
    /// Com o jogo pausado, o relógio do controlador não desce a peça.
    /// </summary>
    [Test]
    public void NaoDeveDescerAPecaComOJogoPausado()
    {
        app.Start();
        app.Pause();
        var y = app.CurrentPiecePosition.Y;

        // espera mais que o intervalo do nível 1, tempo suficiente para um passo do relógio
        Thread.Sleep(CLOCK_TIMEOUT_MS);

        Assert.AreEqual(y, app.CurrentPiecePosition.Y);
        app.Exit();
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

        var game = GameController.Create(10, 20, repository, sound);

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

    /// <summary>
    /// Ao perder, as estatísticas da partida também são guardadas, para contar no recorde.
    /// </summary>
    [Test]
    public void DeveSalvarAsEstatisticasAoPerder()
    {
        var lost = false;
        app.OnLose += () => lost = true;
        app.Start();

        // cada peça ocupa no máximo 4 colunas, então nenhuma linha é completada e a pilha chega ao topo
        for (var i = 0; i < app.BoardHeight && app.Playing; i++)
        {
            app.SmashDown();
        }

        Assert.IsTrue(lost, "O jogo deveria ter terminado.");
        Assert.AreEqual(1, repository.All().Count());
    }

    /// <summary>
    /// Um toque na seta anda 1 bloco; cada repetição da seta segurada anda 2.
    /// </summary>
    /// <param name="key">A seta.</param>
    /// <param name="direction">O sentido do movimento (+1 direita, -1 esquerda).</param>
    [TestCase(Key.Right, 1)]
    [TestCase(Key.Left, -1)]
    public void DeveAndarMaisRapidoComASetaSegurada(Key key, int direction)
    {
        // a peça é sorteada: da coluna 3, até a mais larga (I deitada, 4 blocos) cabe nos 3 passos para qualquer lado
        const int START_COLUMN = 3;
        app.Start();
        var factory = new CommandFactory(app);

        for (var i = 0; i < START_COLUMN; i++)
        {
            app.MoveRight();
        }

        Assert.AreEqual(START_COLUMN, app.CurrentPiecePosition.X, "A peça deveria ter saído do canto.");

        factory.GetAll(key, false, repeat: false).First().Execute();
        Assert.AreEqual(START_COLUMN + direction, app.CurrentPiecePosition.X, "O primeiro toque deveria andar 1 bloco.");

        factory.GetAll(key, false, repeat: true).First().Execute();
        Assert.AreEqual(START_COLUMN + (3 * direction), app.CurrentPiecePosition.X, "A repetição deveria andar 2 blocos.");
    }

    /// <summary>
    /// Segurar ↓ continua descendo como antes: a repetição não muda os outros comandos.
    /// </summary>
    [Test]
    public void DeveManterOsOutrosComandosNaRepeticao()
    {
        var factory = new CommandFactory(app);

        Assert.AreSame(factory.GetAll(Key.Down, false).First(), factory.GetAll(Key.Down, false, repeat: true).First());
        Assert.AreSame(factory.GetAll(Key.Right, true).First(), factory.GetAll(Key.Right, true, repeat: true).First());
    }

    /// <summary>
    /// A tabela de pontuação chega à interface pelo controlador.
    /// </summary>
    [Test]
    public void DeveInformarATabelaDePontuacao()
    {
        CollectionAssert.AreEqual(Stats.ScoreTable, GameController.ScoreTable);
    }

    /// <summary>
    /// A tecla Shift, com o código que o Windows envia (<c>Keys.ShiftKey</c> = 0x10), tem um comando: ligar/desligar o som.
    /// </summary>
    [Test]
    public void DeveTerUmComandoParaShift()
    {
        const int SHIFT_KEY_CODE = 0x10;

        var commands = new CommandFactory(app).GetAll((Key)SHIFT_KEY_CODE, false).ToList();

        Assert.AreEqual(1, commands.Count);
    }

    /// <summary>
    /// O Shift (<c>ToggleMuteCommand</c>) desliga e religa o som recebido pelo controlador.
    /// </summary>
    [Test]
    public void DeveAlternarOSomComOShift()
    {
        var toggleMute = new CommandFactory(app).GetAll(Key.ShiftKey, false).Single();

        toggleMute.Execute();
        Assert.IsTrue(sound.Mute, "O som deveria ter sido desligado.");
        Assert.IsTrue(app.IsMuted, "O controlador deveria informar o som desligado.");

        toggleMute.Execute();
        Assert.IsFalse(sound.Mute, "O som deveria ter sido religado.");
    }

    /// <summary>
    /// Iniciar a partida toca a música de início.
    /// </summary>
    [Test]
    public void DeveTocarAMusicaAoIniciar()
    {
        app.Start();

        Assert.AreEqual(1, sound.Played(nameof(FakeSoundPlayer.Start)));
    }

    /// <summary>
    /// Derrubar a peça toca o som de cada passo deslizando e o da peça encaixando.
    /// </summary>
    [Test]
    public void DeveTocarOSomDeEncaixarAoDerrubar()
    {
        app.Start();

        app.SmashDown();

        Assert.AreEqual(1, sound.Played(nameof(FakeSoundPlayer.Dock)));
        Assert.IsTrue(sound.Played(nameof(FakeSoundPlayer.Slide)) > 0, "A peça deveria ter deslizado até o fundo.");
    }

    /// <summary>
    /// Completar uma linha toca o som de linha removida.
    /// </summary>
    [Test]
    public void DeveTocarOSomDeLimparLinha()
    {
        // cinco quadrados (O, 2 colunas cada) lado a lado completam as duas últimas linhas do tabuleiro de 10 colunas
        const int SQUARES_PER_ROW = 5;
        const int SQUARE_WIDTH = 2;
        var game = GameController.Create(10, 20, repository, sound, new SequencePieceGenerator((PieceType.O, 0)));
        game.Start();

        for (var square = 0; square < SQUARES_PER_ROW; square++)
        {
            for (var step = 0; step < square * SQUARE_WIDTH; step++)
            {
                game.MoveRight();
            }

            game.SmashDown();
        }

        Assert.AreEqual(2, game.Lines, "As duas linhas deveriam ter sido completadas.");
        Assert.AreEqual(1, sound.Played(nameof(FakeSoundPlayer.Clear)));
        game.Exit();
    }

    /// <summary>
    /// Mover a peça toca o som de movimento.
    /// </summary>
    [Test]
    public void DeveTocarOSomDeMoverAoMover()
    {
        app.Start();

        app.MoveRight();

        Assert.AreEqual(1, sound.Played(nameof(FakeSoundPlayer.Move)));
    }
}
