using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gadz.Tetris.Model
{
    /// <summary>
    /// Defines the <see cref="IdentidadeTests" />
    /// </summary>
    [TestClass]
    public class IdentidadeTests
    {
        /// <summary>
        /// The DeveInstanciar
        /// </summary>
        [TestMethod]
        public void DeveInstanciar()
        {
            var id = new Identity();
            Assert.IsNotNull(id);
        }

        /// <summary>
        /// The DeveGerarNova
        /// </summary>
        [TestMethod]
        public void DeveGerarNova()
        {
            var id = Identity.New();
            Assert.IsNotNull(id);
        }

        /// <summary>
        /// The DeveConverterIdentidadeParaInteiro
        /// </summary>
        [TestMethod]
        public void DeveConverterIdentidadeParaInteiro()
        {
            var id = new Identity(0);
            var inteiro = 1;
            inteiro = id;
            Assert.AreEqual(0, inteiro);
        }

        /// <summary>
        /// The DeveConverterIdentidadeParaTexto
        /// </summary>
        [TestMethod]
        public void DeveConverterIdentidadeParaTexto()
        {
            var id = new Identity("A");
            var texto = "B";
            texto = id;
            Assert.AreEqual("A", texto);
        }

        /// <summary>
        /// The DeveConverterTextoParIdentidade
        /// </summary>
        [TestMethod]
        public void DeveConverterTextoParIdentidade()
        {
            var id = new Identity(0);
            var texto = "B";
            id = texto;
            Assert.IsTrue("B" == id);
        }

        /// <summary>
        /// The DeveConverterInteiroParaIdentidade
        /// </summary>
        [TestMethod]
        public void DeveConverterInteiroParaIdentidade()
        {
            var id = new Identity("A");
            var inteiro = 1;
            id = inteiro;
            Assert.IsTrue(1 == id);
        }
    }
}
