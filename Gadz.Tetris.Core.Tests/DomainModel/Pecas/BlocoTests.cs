using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gadz.Tetris.Core.DomainModel.Pecas {

    [TestClass]
    public class BlocoTests {

        [TestMethod]
        public void DeveColidir() {

            var blocoA = new Bloco(0, 0, CorPeca.AMARELO);

            var blocoB = new Bloco(0, 0, CorPeca.AZUL);

            Assert.IsTrue(blocoA.ColideCom(blocoB));
        }

        [TestMethod]
        public void NaoDeveColidir() {

            var blocoA = new Bloco(0, 0, CorPeca.AMARELO);

            var blocoB = new Bloco(1, 0, CorPeca.AZUL);

            Assert.IsFalse(blocoA.ColideCom(blocoB));
        }

        [TestMethod]
        public void DeveInstanciar() {
            var bloco = new Bloco(1, 2, CorPeca.TRANSPARENTE);
            Assert.AreEqual("1+2", bloco.ToString());
        }
    }
}
