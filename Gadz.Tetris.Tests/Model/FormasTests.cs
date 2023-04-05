using Gadz.Tetris.Application;
using Gadz.Tetris.Domain.Models.Pieces;
using NUnit.Framework;

namespace Gadz.Tetris.Tests.Model
{
    /// <summary>
    /// Defines the <see cref="FormasTests" />
    /// </summary>
    public class FormasTests
    {
        private PieceBuilder _pieceBuilder;

        [SetUp]
        public void Setup()
        {
            var game = GameController.Create(10, 10);
            _pieceBuilder = new PieceBuilder()
                .OnBoard(game.Board);
        }

        /// <summary>
        /// The DeveDesenharFormaI
        /// </summary>
        [Test]
        public void DeveDesenharFormaI()
        {
            var piece = _pieceBuilder.OfType(PieceType.I).Build();
            Assert.AreEqual(4, piece.Blocks.Count());
            Assert.AreEqual("0x0,0x1,0x2,0x3", piece.ToString());
        }

        /// <summary>
        /// The DeveDesenharFormaJ
        /// </summary>
        [Test]
        public void DeveDesenharFormaJ()
        {
            var forma = _pieceBuilder.OfType(PieceType.J).Build();
            Assert.AreEqual(4, forma.Blocks.Count());
            Assert.AreEqual("1x0,1x1,1x2,0x2", forma.ToString());
        }

        /// <summary>
        /// The DeveDesenharFormaL
        /// </summary>
        [Test]
        public void DeveDesenharFormaL()
        {
            var forma = _pieceBuilder.OfType(PieceType.L).Build();
            Assert.AreEqual("0x0,0x1,0x2,1x2", forma.ToString());
        }

        /// <summary>
        /// The DeveDesenharFormaO
        /// </summary>
        [Test]
        public void DeveDesenharFormaO()
        {
            var piece = _pieceBuilder.OfType(PieceType.O).Build();
            Assert.AreEqual(4, piece.Blocks.Count());
            Assert.AreEqual("0x0,0x1,1x0,1x1", piece.ToString());
        }

        /// <summary>
        /// The DeveDesenharFormaS
        /// </summary>
        [Test]
        public void DeveDesenharFormaS()
        {
            var piece = _pieceBuilder.OfType(PieceType.S).Build();
            Assert.AreEqual("0x0,0x1,1x1,1x2", piece.ToString());
        }

        /// <summary>
        /// The DeveDesenharFormaT
        /// </summary>
        [Test]
        public void DeveDesenharFormaT()
        {
            var piece = _pieceBuilder.OfType(PieceType.T).Build();
            Assert.AreEqual(4, piece.Blocks.Count());
            Assert.AreEqual("0x0,0x1,0x2,1x1", piece.ToString());
        }

        /// <summary>
        /// The DeveDesenharFormaZ
        /// </summary>
        [Test]
        public void DeveDesenharFormaZ()
        {
            var piece = _pieceBuilder.OfType(PieceType.Z).Build();
            Assert.AreEqual(4, piece.Blocks.Count());
            Assert.AreEqual("1x0,1x1,0x1,0x2", piece.ToString());
        }

        /// <summary>
        /// The NaoDeveDesenharNenhumaForma
        /// </summary>
        [Test]
        public void NaoDeveDesenharNenhumaForma()
        {
            Assert.Throws<NotImplementedException>(() => _pieceBuilder.OfType((PieceType)8).Build());
        }
    }
}
