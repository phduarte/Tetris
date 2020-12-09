using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gadz.Tetris.Model
{
    /// <summary>
    /// Defines the <see cref="DimensaoTests" />
    /// </summary>
    [TestClass]
    public class DimensaoTests
    {
        /// <summary>
        /// The DeveInstanciar
        /// </summary>
        [TestMethod]
        public void DeveInstanciar()
        {
            var dimensao = new Size(10, 20);
            Assert.AreEqual("10:20", dimensao.ToString());
        }
    }
}
