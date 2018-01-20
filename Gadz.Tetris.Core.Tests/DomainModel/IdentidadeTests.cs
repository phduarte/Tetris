using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gadz.Tetris.Core.DomainModel {

    [TestClass]
    public class IdentidadeTests {

        [TestMethod]
        public void DeveInstanciar() {
            var id = new Identidade();
            Assert.IsNotNull(id);
        }

        [TestMethod]
        public void DeveGerarNova() {
            var id = Identidade.New();
            Assert.IsNotNull(id);
        }

        [TestMethod]
        public void DeveConverterIdentidadeParaInteiro() {
            var id = new Identidade(0);
            int inteiro = 1;
            inteiro = id;
            Assert.AreEqual(0, inteiro);
        }

        [TestMethod]
        public void DeveConverterIdentidadeParaTexto() {
            var id = new Identidade("A");
            string texto = "B";
            texto = id;
            Assert.AreEqual("A", texto);
        }

        [TestMethod]
        public void DeveConverterTextoParIdentidade() {
            var id = new Identidade(0);
            string texto = "B";
            id = texto;
            Assert.AreEqual("B", id);
        }

        [TestMethod]
        public void DeveConverterInteiroParaIdentidade() {
            var id = new Identidade("A");
            int inteiro = 1;
            id = inteiro;
            Assert.AreEqual(1, id);
        }
    }
}
