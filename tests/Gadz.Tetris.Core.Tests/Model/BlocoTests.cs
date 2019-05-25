using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gadz.Tetris.Model {

    [TestClass]
    public class BlocoTests {

        [TestMethod]
        public void DeveColidir() {

            var blocoA = new Block(0, 0, PieceColor.Yellow);

            var blocoB = new Block(0, 0, PieceColor.Blue);

            Assert.IsTrue(blocoA.ColideCom(blocoB));
        }

        [TestMethod]
        public void NaoDeveColidir() {

            var blocoA = new Block(0, 0, PieceColor.Yellow);

            var blocoB = new Block(1, 0, PieceColor.Blue);

            Assert.IsFalse(blocoA.ColideCom(blocoB));
        }

        [TestMethod]
        public void DeveInstanciar() {
            var bloco = new Block(1, 2, PieceColor.None);
            Assert.AreEqual("1+2", bloco.ToString());
        }
    }
}
