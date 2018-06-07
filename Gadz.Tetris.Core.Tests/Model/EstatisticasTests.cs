using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Gadz.Tetris.Model {
    [TestClass]
    public class EstatisticasTests {

        Estatisticas stats;

        [TestInitialize]
        public void Setup() {
            stats = new Estatisticas();
        }

        [TestMethod]
        public void DeveInstanciar() {
            var stats = new Estatisticas();
            Assert.IsNotNull(stats);
        }

        [TestMethod]
        public void DeveRecuperarValoresDeOutraPartida() {
            var stats = new Estatisticas(Identidade.New(), 1000, 15, 10, 1000, 15, 520, 452165);

            Assert.AreEqual(1000, stats.Pontos);
            Assert.AreEqual(15, stats.Linhas);
            Assert.AreEqual(10, stats.Nivel);
            Assert.AreEqual(1000, stats.Velocidade);
            Assert.AreEqual(15, stats.Movimentos);
            Assert.AreEqual(520, stats.Blocos);
            Assert.AreEqual(452165, stats.Duracao.Ticks);
        }

        [TestMethod]
        public void DevePontuar40PontosPorUmaLinha() {
            stats.Pontuar(1);
            Assert.AreEqual(40, stats.Pontos);
        }

        [TestMethod]
        public void DevePontuar100PontosPorDuasLinhas() {
            stats.Pontuar(2);
            Assert.AreEqual(100, stats.Pontos);
        }

        [TestMethod]
        public void DevePontuar300PontosPorTresLinhas() {
            stats.Pontuar(3);
            Assert.AreEqual(300, stats.Pontos);
        }

        [TestMethod]
        public void DevePontuar1200PontosPorQuatroLinhas() {
            stats.Pontuar(4);
            Assert.AreEqual(1200, stats.Pontos);
        }

        [TestMethod]
        public void DeveContabilizarTempoDaPartida() {
            Assert.AreEqual(0, stats.Duracao.Ticks);
            stats.Iniciar();
            Thread.Sleep(50);
            Assert.AreNotEqual(0, stats.Duracao.Ticks);
        }

        [TestMethod]
        public void DevePararDeContabilizarTempoAoTerminarPartida() {
            DeveContabilizarTempoDaPartida();
            stats.Terminar();
            var x = stats.Duracao.Ticks;
            Thread.Sleep(50);
            Assert.AreEqual(x, stats.Duracao.Ticks);
        }

        [TestMethod]
        public void DeveContabilizarMovimentos() {
            stats.ContarMovimento();
            Assert.AreEqual(1, stats.Movimentos);
        }
        [TestMethod]
        public void DeveConseguirPausarPartida() {
            stats.Iniciar();
            Thread.Sleep(50);
            var x = stats.Duracao.Ticks;
            Assert.IsTrue(x > 0, $"A duração do jogo estava em {x}");
            stats.Pausar();
            x = stats.Duracao.Ticks;
            Thread.Sleep(50);
            Assert.AreEqual(x, stats.Duracao.Ticks);
        }

        [TestMethod]
        public void DeveConseguirContinuarPartida() {
            DeveConseguirPausarPartida();

            var x = stats.Duracao.Ticks;

            stats.Continuar();

            Thread.Sleep(50);

            Assert.IsTrue(x < stats.Duracao.Ticks);
        }
    }
}
