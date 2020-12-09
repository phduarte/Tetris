using Gadz.Tetris.Model.Pieces;
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
            var config = new TetraminoConfiguration { Type = PieceType.I};
            var forma = Tetramino.Draw(config);
            Assert.AreEqual(4, forma.Blocks.Count());
            Assert.AreEqual("0+0,0+1,0+2,0+3", forma.ToString());
        }

        /// <summary>
        /// The DeveDesenharFormaJ
        /// </summary>
        [TestMethod]
        public void DeveDesenharFormaJ()
        {
            var config = new TetraminoConfiguration { Type = PieceType.J};
            var forma = Tetramino.Draw(config);
            Assert.AreEqual(4, forma.Blocks.Count());
            Assert.AreEqual("1+0,1+1,1+2,0+2", forma.ToString());
        }

        /// <summary>
        /// The DeveDesenharFormaL
        /// </summary>
        [TestMethod]
        public void DeveDesenharFormaL()
        {
            var config = new TetraminoConfiguration { Type = PieceType.L};
            var forma = Tetramino.Draw(config);
            Assert.AreEqual("0+0,0+1,0+2,1+2", forma.ToString());
        }

        /// <summary>
        /// The DeveDesenharFormaO
        /// </summary>
        [TestMethod]
        public void DeveDesenharFormaO()
        {
            var config = new TetraminoConfiguration { Type = PieceType.O};
            var forma = Tetramino.Draw(config);
            Assert.AreEqual(4, forma.Blocks.Count());
            Assert.AreEqual("0+0,0+1,1+0,1+1", forma.ToString());
        }

        /// <summary>
        /// The DeveDesenharFormaS
        /// </summary>
        [TestMethod]
        public void DeveDesenharFormaS()
        {
            var config = new TetraminoConfiguration { Type = PieceType.S};
            var forma = Tetramino.Draw(config);
            Assert.AreEqual("0+0,0+1,1+1,1+2", forma.ToString());
        }

        /// <summary>
        /// The DeveDesenharFormaT
        /// </summary>
        [TestMethod]
        public void DeveDesenharFormaT()
        {
            var config = new TetraminoConfiguration { Type = PieceType.T };
            var forma = Tetramino.Draw(config);
            Assert.AreEqual(4, forma.Blocks.Count());
            Assert.AreEqual("0+0,0+1,0+2,1+1", forma.ToString());
        }

        /// <summary>
        /// The DeveDesenharFormaZ
        /// </summary>
        [TestMethod]
        public void DeveDesenharFormaZ()
        {
            var config = new TetraminoConfiguration { Type = PieceType.Z};
            var forma = Tetramino.Draw(config);
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
            var forma = Tetramino.Draw(new TetraminoConfiguration { Type = (PieceType)8});
        }
    }
}
