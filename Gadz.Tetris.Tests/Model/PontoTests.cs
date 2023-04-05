using Gadz.Tetris.Domain.Models;
using NUnit.Framework;

namespace Gadz.Tetris.Tests.Model
{
    /// <summary>
    /// Defines the <see cref="PontoTests" />
    /// </summary>
    public class PontoTests
    {
        /// <summary>
        /// The DeveInstanciar
        /// </summary>
        [Test]
        public void DeveInstanciar()
        {
            var ponto = new Point(0, 0);
            Assert.AreEqual(0, ponto.X);
            Assert.AreEqual(0, ponto.Y);
        }

        /// <summary>
        /// The DeveSerIguais
        /// </summary>
        [Test]
        public void DeveSerIguais()
        {
            var pontoA = new Point(0, 0);
            var pontoB = new Point(0, 0);
            Assert.IsTrue(pontoA.Equals(pontoB));
            //Assert.That(pontoA, Is.Not.SameAs(pontoB));
            Assert.IsTrue(pontoA == pontoB);
        }

        /// <summary>
        /// The DeveSerDiferentes
        /// </summary>
        [Test]
        public void DeveSerDiferentes()
        {
            var pontoA = new Point(0, 0);
            var pontoB = new Point(1, 0);
            Assert.IsFalse(pontoA.Equals(pontoB));
            Assert.IsTrue(pontoA != pontoB);
        }

        /// <summary>
        /// The DevemColidir
        /// </summary>
        [Test]
        public void DevemColidir()
        {
            var pontoA = new Point(0, 0);
            var pontoB = new Point(0, 0);
            Assert.IsTrue(pontoA.ColideCom(pontoB));
        }

        /// <summary>
        /// The NaoDevemColidir
        /// </summary>
        [Test]
        public void NaoDevemColidir()
        {
            var pontoA = new Point(0, 0);
            var pontoB = new Point(1, 0);
            Assert.IsFalse(pontoA.ColideCom(pontoB));
        }
    }
}
