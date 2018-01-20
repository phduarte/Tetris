using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gadz.Tetris.Core.DomainModel.Tabuleiros {
    [TestClass]
    public class DimensaoTests {

        [TestMethod]
        public void DeveInstanciar() {
            var dimensao = new Dimensao(10,20);
            Assert.AreEqual("10x20", dimensao.ToString());
        }
    }
}
