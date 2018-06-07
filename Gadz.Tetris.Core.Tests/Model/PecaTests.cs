using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Gadz.Tetris.Model;

namespace Gadz.Tetris.Model {

    [TestClass]
    public class PecaTests {

        Peca peca;
        ITabuleiro tabuleiro;

        [TestInitialize]
        public void Setup() {

            tabuleiro = GameController.Create(10, 20).CurrentBoard;

            peca = new PecaBuilder()
                .DoTipo(TiposDePecas.I)
                .ComRotacao(0)
                .NoTabuleiro(tabuleiro)
                .Construir();
        }

        [TestMethod]
        public void DeveInstanciar() {
            Assert.IsNotNull(peca,"A peça não foi instanciada.");
            Assert.AreEqual(TiposDePecas.I, peca.Tipo, "O tipo da peça não é I");
            Assert.AreEqual(4, peca.Blocos.Count(), "O tetraminó não possui 4 blocos");
            Assert.AreEqual(Cores.PegarCorPara(TiposDePecas.I), peca.Cor, "A cor da peça não é amarela");
            Assert.AreEqual(0, peca.Posicao.X,"A posição da peça no eixo X não é 0.");
            Assert.AreEqual(0, peca.Posicao.Y,"A posição da peça no eixo Y não é 0.");
            Assert.AreEqual(0, peca.Rotacao, "A rotação da peça não é 0.");
        }

        [TestMethod]
        public void DeveGirar() {
            peca.Girar();
            Assert.AreEqual(1, peca.Rotacao);
        }

        [TestMethod]
        public void DeveMoverParaDireita() {
            peca.MoverDireita();
            Assert.AreEqual(1, peca.Posicao.X);
        }

        [TestMethod]
        public void DeveMoverParaEsquerda() {
            DeveMoverParaDireita();

            peca.MoverEsquerda();
            Assert.AreEqual(0, peca.Posicao.X);
        }

        [TestMethod]
        public void DeveMoverParaBaixo() {
            peca.MoverBaixo();
            Assert.AreEqual(1, peca.Posicao.Y);
        }

        [TestMethod]
        public void DeveSerClonada() {
            var pecaB = peca.Clonar();
            Assert.AreEqual(peca, pecaB,"Não são iguais");
            Assert.AreNotSame(peca, pecaB, "São as mesmas");
        }

        [TestMethod]
        public void NaoDeveSerClonada() {
            var pecaB = peca;
            Assert.AreEqual(peca, pecaB, "Não são iguais");
            Assert.AreSame(peca, pecaB, "Não são as mesmas");
        }
    }
}
