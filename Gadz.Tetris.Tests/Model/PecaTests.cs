using Gadz.Tetris.Application;
using Gadz.Tetris.Domain.Models.Boards;
using Gadz.Tetris.Domain.Models.Pieces;
using NUnit.Framework;

namespace Gadz.Tetris.Tests.Model
{
    /// <summary>
    /// Defines the <see cref="PecaTests" />
    /// </summary>
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
        [SetUp]
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
        [Test]
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
        [Test]
        public void DeveGirar()
        {
            peca.Rotate();
            Assert.AreEqual(1, peca.Rotation);
        }

        /// <summary>
        /// The DeveMoverParaDireita
        /// </summary>
        [Test]
        public void DeveMoverParaDireita()
        {
            peca.MoveRight();
            Assert.AreEqual(1, peca.Position.X);
        }

        /// <summary>
        /// The DeveMoverParaEsquerda
        /// </summary>
        [Test]
        public void DeveMoverParaEsquerda()
        {
            DeveMoverParaDireita();

            peca.MoveLeft();
            Assert.AreEqual(0, peca.Position.X);
        }

        /// <summary>
        /// The DeveMoverParaBaixo
        /// </summary>
        [Test]
        public void DeveMoverParaBaixo()
        {
            peca.MoveDown();
            Assert.AreEqual(1, peca.Position.Y);
        }

        /// <summary>
        /// The DeveSerClonada
        /// </summary>
        [Test]
        public void DeveSerClonada()
        {
            var pecaB = peca.Clone();
            Assert.AreEqual(peca, pecaB, "Não são iguais");
            Assert.AreNotSame(peca, pecaB, "São as mesmas");
        }

        /// <summary>
        /// The NaoDeveSerClonada
        /// </summary>
        [Test]
        public void NaoDeveSerClonada()
        {
            var pecaB = peca;
            Assert.AreEqual(peca, pecaB, "Não são iguais");
            Assert.AreSame(peca, pecaB, "Não são as mesmas");
        }
    }
}
