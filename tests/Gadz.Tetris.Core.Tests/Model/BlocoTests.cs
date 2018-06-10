using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gadz.Tetris.Model {

    [TestClass]
    public class BlocoTests {

        [TestMethod]
        public void DeveColidir() {

            var blocoA = new Bloco(0, 0, CoresDePeca.Amarelo);

            var blocoB = new Bloco(0, 0, CoresDePeca.Azul);

            Assert.IsTrue(blocoA.ColideCom(blocoB));
        }

        [TestMethod]
        public void NaoDeveColidir() {

            var blocoA = new Bloco(0, 0, CoresDePeca.Amarelo);

            var blocoB = new Bloco(1, 0, CoresDePeca.Azul);

            Assert.IsFalse(blocoA.ColideCom(blocoB));
        }

        [TestMethod]
        public void DeveInstanciar() {
            var bloco = new Bloco(1, 2, CoresDePeca.Transparente);
            Assert.AreEqual("1+2", bloco.ToString());
        }
    }
}
