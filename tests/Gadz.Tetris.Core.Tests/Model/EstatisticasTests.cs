using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Gadz.Tetris.Model {
    [TestClass]
    public class EstatisticasTests {

        Stats stats;

        [TestInitialize]
        public void Setup() {
            stats = new Stats();
        }

        [TestMethod]
        public void DeveInstanciar() {
            var stats = new Stats();
            Assert.IsNotNull(stats);
        }

        [TestMethod]
        public void DeveRecuperarValoresDeOutraPartida() {
            var stats = new Stats(Identity.New(), 1000, 15, 10, 1000, 15, 520, 452165);

            Assert.AreEqual(1000, stats.Score);
            Assert.AreEqual(15, stats.Lines);
            Assert.AreEqual(10, stats.Level);
            Assert.AreEqual(1000, stats.Speed);
            Assert.AreEqual(15, stats.Moves);
            Assert.AreEqual(520, stats.Blocks);
            Assert.AreEqual(452165, stats.Duration.Ticks);
        }

        [TestMethod]
        public void DevePontuar40PontosPorUmaLinha() {
            stats.Gain(1);
            Assert.AreEqual(40, stats.Score);
        }

        [TestMethod]
        public void DevePontuar100PontosPorDuasLinhas() {
            stats.Gain(2);
            Assert.AreEqual(100, stats.Score);
        }

        [TestMethod]
        public void DevePontuar300PontosPorTresLinhas() {
            stats.Gain(3);
            Assert.AreEqual(300, stats.Score);
        }

        [TestMethod]
        public void DevePontuar1200PontosPorQuatroLinhas() {
            stats.Gain(4);
            Assert.AreEqual(1200, stats.Score);
        }

        [TestMethod]
        public void DeveContabilizarTempoDaPartida() {
            Assert.AreEqual(0, stats.Duration.Ticks);
            stats.Start();
            Thread.Sleep(50);
            Assert.AreNotEqual(0, stats.Duration.Ticks);
        }

        [TestMethod]
        public void DevePararDeContabilizarTempoAoTerminarPartida() {
            DeveContabilizarTempoDaPartida();
            stats.Finish();
            var x = stats.Duration.Ticks;
            Thread.Sleep(50);
            Assert.AreEqual(x, stats.Duration.Ticks);
        }

        [TestMethod]
        public void DeveContabilizarMovimentos() {
            stats.CountMovement();
            Assert.AreEqual(1, stats.Moves);
        }
        [TestMethod]
        public void DeveConseguirPausarPartida() {
            stats.Start();
            Thread.Sleep(50);
            var x = stats.Duration.Ticks;
            Assert.IsTrue(x > 0, $"A duração do jogo estava em {x}");
            stats.Pause();
            x = stats.Duration.Ticks;
            Thread.Sleep(50);
            Assert.AreEqual(x, stats.Duration.Ticks);
        }

        [TestMethod]
        public void DeveConseguirContinuarPartida() {
            DeveConseguirPausarPartida();

            var x = stats.Duration.Ticks;

            stats.Continue();

            Thread.Sleep(50);

            Assert.IsTrue(x < stats.Duration.Ticks);
        }
    }
}
