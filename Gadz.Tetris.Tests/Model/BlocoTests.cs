using Gadz.Tetris.Domain.Models.Pieces;
using NUnit.Framework;

namespace Gadz.Tetris.Tests.Model
{
    /// <summary>
    /// Defines the <see cref="BlocoTests" />
    /// </summary>
    public class BlocoTests
    {
        /// <summary>
        /// The DeveColidir
        /// </summary>
        [Test]
        public void DeveColidir()
        {
            var blocoA = new Block(0, 0, PieceColor.Yellow);

            var blocoB = new Block(0, 0, PieceColor.Blue);

            Assert.IsTrue(blocoA.CollideWith(blocoB));
        }

        /// <summary>
        /// The NaoDeveColidir
        /// </summary>
        [Test]
        public void NaoDeveColidir()
        {
            var blocoA = new Block(0, 0, PieceColor.Yellow);

            var blocoB = new Block(1, 0, PieceColor.Blue);

            Assert.IsFalse(blocoA.CollideWith(blocoB));
        }

        /// <summary>
        /// The DeveInstanciar
        /// </summary>
        [Test]
        public void DeveInstanciar()
        {
            var bloco = new Block(1, 2, PieceColor.None);
            Assert.AreEqual("1x2", bloco.ToString());
        }
    }
}
