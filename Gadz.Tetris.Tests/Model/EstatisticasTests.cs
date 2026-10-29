using Gadz.Tetris.Domain.Models;
using Gadz.Tetris.Domain.Models.Statistics;
using NUnit.Framework;

namespace Gadz.Tetris.Tests.Model;

/// <summary>
/// Testes das regras de <see cref="Stats"/>: pontuação, níveis, velocidade e tempo.
/// </summary>
public class EstatisticasTests
{
    /// <summary>
    /// Estatísticas novas, recriadas a cada teste.
    /// </summary>
    private Stats stats;

    /// <summary>
    /// Cria estatísticas de uma partida nova.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        stats = new Stats();
    }

    /// <summary>
    /// As estatísticas de uma partida nova podem ser criadas.
    /// </summary>
    [Test]
    public void DeveInstanciar()
    {
        var stats = new Stats();
        Assert.IsNotNull(stats);
    }

    /// <summary>
    /// Carregar um registro guardado recupera os valores da partida.
    /// </summary>
    [Test]
    public void DeveRecuperarValoresDeOutraPartida()
    {
        var stats = Stats.Load(new StatsRecord
        {
            Id = Identity.New(),
            Score = 1000,
            Lines = 15,
            Level = 10,
            Speed = 1000,
            Moves = 15,
            Blocks = 520,
            Seconds = 452165
        });

        Assert.AreEqual(1000, stats.Score);
        Assert.AreEqual(15, stats.Lines);
        Assert.AreEqual(10, stats.Level);
        Assert.AreEqual(1000, stats.Speed);
        Assert.AreEqual(15, stats.Moves);
        Assert.AreEqual(520, stats.Blocks);
        Assert.AreEqual(452165, stats.Duration.Ticks);
    }

    /// <summary>
    /// Remover 1 linha numa jogada vale 40 pontos.
    /// </summary>
    [Test]
    public void DevePontuar40PontosPorUmaLinha()
    {
        stats.Gain(1);
        Assert.AreEqual(40, stats.Score);
    }

    /// <summary>
    /// Remover 2 linhas numa jogada vale 100 pontos.
    /// </summary>
    [Test]
    public void DevePontuar100PontosPorDuasLinhas()
    {
        stats.Gain(2);
        Assert.AreEqual(100, stats.Score);
    }

    /// <summary>
    /// Remover 3 linhas numa jogada vale 300 pontos.
    /// </summary>
    [Test]
    public void DevePontuar300PontosPorTresLinhas()
    {
        stats.Gain(3);
        Assert.AreEqual(300, stats.Score);
    }

    /// <summary>
    /// Remover 4 linhas numa jogada (TETRIS) vale 1200 pontos.
    /// </summary>
    [Test]
    public void DevePontuar1200PontosPorQuatroLinhas()
    {
        stats.Gain(4);
        Assert.AreEqual(1200, stats.Score);
    }

    /// <summary>
    /// A tabela de pontuação exposta à interface é a mesma usada para pontuar: 1 a 4 linhas.
    /// </summary>
    [Test]
    public void DeveExporATabelaDePontuacao()
    {
        CollectionAssert.AreEqual(new[] { 40, 100, 300, 1200 }, Stats.ScoreTable);
    }

    /// <summary>
    /// O intervalo do nível 1 é de 1000 ms.
    /// </summary>
    [Test]
    public void DeveIniciarComIntervaloDeMilMilissegundos()
    {
        Assert.AreEqual(1000, stats.Interval);
    }

    /// <summary>
    /// Completar a meta de linhas sobe o nível e reduz o intervalo em 50 ms.
    /// </summary>
    [Test]
    public void DeveReduzirOIntervaloAoSubirDeNivel()
    {
        for (var i = 0; i < 5; i++)
        {
            stats.Gain(4);
        }

        Assert.AreEqual(2, stats.Level);
        Assert.AreEqual(950, stats.Interval);
    }

    /// <summary>
    /// O intervalo nunca fica abaixo do mínimo de 100 ms.
    /// </summary>
    [Test]
    public void NaoDeveReduzirOIntervaloAbaixoDoMinimo()
    {
        var stats = Stats.Load(new StatsRecord { Id = Identity.New(), Level = 50 });

        Assert.AreEqual(100, stats.Interval);
    }

    /// <summary>
    /// O tempo começa a contar ao iniciar a partida.
    /// </summary>
    [Test]
    public void DeveContabilizarTempoDaPartida()
    {
        Assert.AreEqual(0, stats.Duration.Ticks);
        stats.Start();
        Thread.Sleep(50);
        Assert.AreNotEqual(0, stats.Duration.Ticks);
    }

    /// <summary>
    /// O tempo para de contar ao terminar a partida.
    /// </summary>
    [Test]
    public void DevePararDeContabilizarTempoAoTerminarPartida()
    {
        DeveContabilizarTempoDaPartida();
        stats.Finish();
        var x = stats.Duration.Ticks;
        Thread.Sleep(50);
        Assert.AreEqual(x, stats.Duration.Ticks);
    }

    /// <summary>
    /// Cada movimento do jogador é contado.
    /// </summary>
    [Test]
    public void DeveContabilizarMovimentos()
    {
        stats.CountMovement();
        Assert.AreEqual(1, stats.Moves);
    }

    /// <summary>
    /// O tempo para de contar enquanto a partida está pausada.
    /// </summary>
    [Test]
    public void DeveConseguirPausarPartida()
    {
        stats.Start();
        Thread.Sleep(50);
        var x = stats.Duration.Ticks;
        Assert.IsTrue(x > 0, $"A duração do jogo estava em {x}");
        stats.Pause();
        x = stats.Duration.Ticks;
        Thread.Sleep(50);
        Assert.AreEqual(x, stats.Duration.Ticks);
    }

    /// <summary>
    /// O tempo volta a contar ao continuar a partida.
    /// </summary>
    [Test]
    public void DeveConseguirContinuarPartida()
    {
        DeveConseguirPausarPartida();

        var x = stats.Duration.Ticks;

        stats.Continue();

        Thread.Sleep(50);

        Assert.IsTrue(x < stats.Duration.Ticks);
    }

    /// <summary>
    /// Uma partida carregada continua contando a partir do tempo guardado.
    /// </summary>
    [Test]
    public void DeveSomarOTempoGuardadoAoContinuar()
    {
        const long TEMPO_GUARDADO = 452165;
        var stats = Stats.Load(new StatsRecord { Id = Identity.New(), Seconds = TEMPO_GUARDADO });

        stats.Continue();
        Thread.Sleep(50);

        Assert.IsTrue(stats.Duration.Ticks > TEMPO_GUARDADO);
    }

    /// <summary>
    /// Iniciar de novo com o tempo já contando não descarta o trecho em andamento.
    /// </summary>
    [Test]
    public void NaoDevePerderTempoAoIniciarDuasVezes()
    {
        stats.Start();
        Thread.Sleep(50);
        var x = stats.Duration.Ticks;

        stats.Start();

        Assert.IsTrue(stats.Duration.Ticks >= x);
    }
}
