using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gadz.Tetris.Model.Tests
{
    [TestClass]
    public class PontoTests
    {
        [TestMethod]
        public void DeveInstanciar()
        {
            var ponto = new Point(0, 0);
            Assert.AreEqual(0, ponto.X);
            Assert.AreEqual(0, ponto.Y);
        }

        [TestMethod]
        public void DeveSerIguais()
        {
            var pontoA = new Point(0, 0);
            var pontoB = new Point(0, 0);
            Assert.IsTrue(pontoA.Equals(pontoB));
            Assert.AreNotSame(pontoA, pontoB);
            Assert.IsTrue(pontoA == pontoB);
        }

        [TestMethod]
        public void DeveSerDiferentes()
        {
            var pontoA = new Point(0, 0);
            var pontoB = new Point(1, 0);
            Assert.IsFalse(pontoA.Equals(pontoB));
            Assert.IsTrue(pontoA != pontoB);
        }

        [TestMethod]
        public void DevemColidir()
        {
            var pontoA = new Point(0, 0);
            var pontoB = new Point(0, 0);
            Assert.IsTrue(pontoA.ColideCom(pontoB));
        }

        [TestMethod]
        public void NaoDevemColidir()
        {
            var pontoA = new Point(0, 0);
            var pontoB = new Point(1, 0);
            Assert.IsFalse(pontoA.ColideCom(pontoB));
        }
    }
}