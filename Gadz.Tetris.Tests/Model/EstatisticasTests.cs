using Gadz.Tetris.Model.Statistics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Gadz.Tetris.Model
{
    /// <summary>
    /// Defines the <see cref="EstatisticasTests" />
    /// </summary>
    [TestClass]
    public class EstatisticasTests
    {
        /// <summary>
        /// Defines the stats
        /// </summary>
        private Stats stats;

        /// <summary>
        /// The Setup
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            stats = new Stats();
        }

        /// <summary>
        /// The DeveInstanciar
        /// </summary>
        [TestMethod]
        public void DeveInstanciar()
        {
            var stats = new Stats();
            Assert.IsNotNull(stats);
        }

        /// <summary>
        /// The DeveRecuperarValoresDeOutraPartida
        /// </summary>
        [TestMethod]
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
        /// The DevePontuar40PontosPorUmaLinha
        /// </summary>
        [TestMethod]
        public void DevePontuar40PontosPorUmaLinha()
        {
            stats.Gain(1);
            Assert.AreEqual(40, stats.Score);
        }

        /// <summary>
        /// The DevePontuar100PontosPorDuasLinhas
        /// </summary>
        [TestMethod]
        public void DevePontuar100PontosPorDuasLinhas()
        {
            stats.Gain(2);
            Assert.AreEqual(100, stats.Score);
        }

        /// <summary>
        /// The DevePontuar300PontosPorTresLinhas
        /// </summary>
        [TestMethod]
        public void DevePontuar300PontosPorTresLinhas()
        {
            stats.Gain(3);
            Assert.AreEqual(300, stats.Score);
        }

        /// <summary>
        /// The DevePontuar1200PontosPorQuatroLinhas
        /// </summary>
        [TestMethod]
        public void DevePontuar1200PontosPorQuatroLinhas()
        {
            stats.Gain(4);
            Assert.AreEqual(1200, stats.Score);
        }

        /// <summary>
        /// The DeveContabilizarTempoDaPartida
        /// </summary>
        [TestMethod]
        public void DeveContabilizarTempoDaPartida()
        {
            Assert.AreEqual(0, stats.Duration.Ticks);
            stats.Start();
            Thread.Sleep(50);
            Assert.AreNotEqual(0, stats.Duration.Ticks);
        }

        /// <summary>
        /// The DevePararDeContabilizarTempoAoTerminarPartida
        /// </summary>
        [TestMethod]
        public void DevePararDeContabilizarTempoAoTerminarPartida()
        {
            DeveContabilizarTempoDaPartida();
            stats.Finish();
            var x = stats.Duration.Ticks;
            Thread.Sleep(50);
            Assert.AreEqual(x, stats.Duration.Ticks);
        }

        /// <summary>
        /// The DeveContabilizarMovimentos
        /// </summary>
        [TestMethod]
        public void DeveContabilizarMovimentos()
        {
            stats.CountMovement();
            Assert.AreEqual(1, stats.Moves);
        }

        /// <summary>
        /// The DeveConseguirPausarPartida
        /// </summary>
        [TestMethod]
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
        /// The DeveConseguirContinuarPartida
        /// </summary>
        [TestMethod]
        public void DeveConseguirContinuarPartida()
        {
            DeveConseguirPausarPartida();

            var x = stats.Duration.Ticks;

            stats.Continue();

            Thread.Sleep(50);

            Assert.IsTrue(x < stats.Duration.Ticks);
        }
    }
}
