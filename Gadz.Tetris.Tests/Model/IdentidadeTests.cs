using Gadz.Tetris.Domain.Models;
using NUnit.Framework;

namespace Gadz.Tetris.Tests.Model
{
    /// <summary>
    /// Defines the <see cref="IdentidadeTests" />
    /// </summary>
    public class IdentidadeTests
    {
        /// <summary>
        /// The DeveInstanciar
        /// </summary>
        [Test]
        public void DeveInstanciar()
        {
            var id = new Identity();
            Assert.IsNotNull(id);
        }

        /// <summary>
        /// The DeveGerarNova
        /// </summary>
        [Test]
        public void DeveGerarNova()
        {
            var id = Identity.New();
            Assert.IsNotNull(id);
        }

        /// <summary>
        /// The DeveConverterIdentidadeParaInteiro
        /// </summary>
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
        public void DeveConverterInteiroParaIdentidade()
        {
            var id = new Identity("A");
            var inteiro = 1;
            id = inteiro;
            Assert.IsTrue(1 == id);
        }
    }
}
