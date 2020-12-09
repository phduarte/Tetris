using Gadz.Tetris.Model.Boards;
using Gadz.Tetris.Model.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Gadz.Tetris.Model
{
    /// <summary>
    /// Defines the <see cref="PecaTests" />
    /// </summary>
    [TestClass]
    public class PecaTests
    {
        /// <summary>
        /// Defines the peca
        /// </summary>
        private Piece peca;

        /// <summary>
        /// Defines the tabuleiro
        /// </summary>
        private Board tabuleiro;

        /// <summary>
        /// The Setup
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            tabuleiro = GameController.Create(10, 20).Board;

            peca = new PieceBuilder()
                .OfType(PieceType.I)
                .WithRotation(0)
                .OnBoard(tabuleiro)
                .Build();
        }

        /// <summary>
        /// The DeveInstanciar
        /// </summary>
        [TestMethod]
        public void DeveInstanciar()
        {
            Assert.IsNotNull(peca, "A peça não foi instanciada.");
            Assert.AreEqual(PieceType.I, peca.Type, "O tipo da peça não é I");
            Assert.AreEqual(4, peca.Blocks.Count(), "O tetraminó não possui 4 blocos");
            Assert.AreEqual(Piece.GetPieceColor(PieceType.I), peca.Color, "A cor da peça não é amarela");
            Assert.AreEqual(0, peca.Position.X, "A posição da peça no eixo X não é 0.");
            Assert.AreEqual(0, peca.Position.Y, "A posição da peça no eixo Y não é 0.");
            Assert.AreEqual(0, peca.Rotation, "A rotação da peça não é 0.");
        }

        /// <summary>
        /// The DeveGirar
        /// </summary>
        [TestMethod]
        public void DeveGirar()
        {
            peca.Rotate();
            Assert.AreEqual(1, peca.Rotation);
        }

        /// <summary>
        /// The DeveMoverParaDireita
        /// </summary>
        [TestMethod]
        public void DeveMoverParaDireita()
        {
            peca.MoveRight();
            Assert.AreEqual(1, peca.Position.X);
        }

        /// <summary>
        /// The DeveMoverParaEsquerda
        /// </summary>
        [TestMethod]
        public void DeveMoverParaEsquerda()
        {
            DeveMoverParaDireita();

            peca.MoveLeft();
            Assert.AreEqual(0, peca.Position.X);
        }

        /// <summary>
        /// The DeveMoverParaBaixo
        /// </summary>
        [TestMethod]
        public void DeveMoverParaBaixo()
        {
            peca.MoveDown();
            Assert.AreEqual(1, peca.Position.Y);
        }

        /// <summary>
        /// The DeveSerClonada
        /// </summary>
        [TestMethod]
        public void DeveSerClonada()
        {
            var pecaB = peca.Clone();
            Assert.AreEqual(peca, pecaB, "Não são iguais");
            Assert.AreNotSame(peca, pecaB, "São as mesmas");
        }

        /// <summary>
        /// The NaoDeveSerClonada
        /// </summary>
        [TestMethod]
        public void NaoDeveSerClonada()
        {
            var pecaB = peca;
            Assert.AreEqual(peca, pecaB, "Não são iguais");
            Assert.AreSame(peca, pecaB, "Não são as mesmas");
        }
    }
}
