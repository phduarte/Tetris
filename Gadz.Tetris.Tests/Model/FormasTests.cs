using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Gadz.Tetris.Model
{
    /// <summary>
    /// Defines the <see cref="FormasTests" />
    /// </summary>
    [TestClass]
    public class FormasTests
    {
        /// <summary>
        /// The DeveDesenharFormaI
        /// </summary>
        [TestMethod]
        public void DeveDesenharFormaI()
        {
            var forma = Tetramino.Draw(PieceType.I, new Point(), 0);
            Assert.AreEqual(4, forma.Blocks.Count());
            Assert.AreEqual("0+0,0+1,0+2,0+3", forma.ToString());
        }

        /// <summary>
        /// The DeveDesenharFormaJ
        /// </summary>
        [TestMethod]
        public void DeveDesenharFormaJ()
        {
            var forma = Tetramino.Draw(PieceType.J, new Point(), 0);
            Assert.AreEqual(4, forma.Blocks.Count());
            Assert.AreEqual("1+0,1+1,1+2,0+2", forma.ToString());
        }

        /// <summary>
        /// The DeveDesenharFormaL
        /// </summary>
        [TestMethod]
        public void DeveDesenharFormaL()
        {
            var forma = Tetramino.Draw(PieceType.L, new Point(), 0);
            Assert.AreEqual("0+0,0+1,0+2,1+2", forma.ToString());
        }

        /// <summary>
        /// The DeveDesenharFormaO
        /// </summary>
        [TestMethod]
        public void DeveDesenharFormaO()
        {
            var forma = Tetramino.Draw(PieceType.O, new Point(), 0);
            Assert.AreEqual(4, forma.Blocks.Count());
            Assert.AreEqual("0+0,0+1,1+0,1+1", forma.ToString());
        }

        /// <summary>
        /// The DeveDesenharFormaS
        /// </summary>
        [TestMethod]
        public void DeveDesenharFormaS()
        {
            var forma = Tetramino.Draw(PieceType.S, new Point(), 0);
            Assert.AreEqual("0+0,0+1,1+1,1+2", forma.ToString());
        }

        /// <summary>
        /// The DeveDesenharFormaT
        /// </summary>
        [TestMethod]
        public void DeveDesenharFormaT()
        {
            var forma = Tetramino.Draw(PieceType.T, new Point(), 0);
            Assert.AreEqual(4, forma.Blocks.Count());
            Assert.AreEqual("0+0,0+1,0+2,1+1", forma.ToString());
        }

        /// <summary>
        /// The DeveDesenharFormaZ
        /// </summary>
        [TestMethod]
        public void DeveDesenharFormaZ()
        {
            var forma = Tetramino.Draw(PieceType.Z, new Point(), 0);
            Assert.AreEqual(4, forma.Blocks.Count());
            Assert.AreEqual("1+0,1+1,0+1,0+2", forma.ToString());
        }

        /// <summary>
        /// The NaoDeveDesenharNenhumaForma
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void NaoDeveDesenharNenhumaForma()
        {
            var forma = Tetramino.Draw((PieceType)8, new Point(), 0);
        }
    }
}
