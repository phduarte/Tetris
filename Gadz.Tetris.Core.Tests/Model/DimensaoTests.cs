using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gadz.Tetris.Model.Tests
{
    [TestClass]
    public class DimensaoTests
    {
        [TestMethod]
        public void DeveInstanciar()
        {
            var dimensao = new Size(10, 20);
            Assert.AreEqual("10x20", dimensao.ToString());
        }
    }
}