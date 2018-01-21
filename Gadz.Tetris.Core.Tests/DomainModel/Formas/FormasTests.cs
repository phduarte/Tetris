using Gadz.Tetris.Core.DomainModel.Pecas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Gadz.Tetris.Core.DomainModel.Formas {

    [TestClass]
    public class FormasTests {

        [TestMethod]
        public void DeveDesenharFormaI() {
            var forma = FormaFactory.Desenhar(TipoPeca.I, new Ponto(), 0);
            Assert.AreEqual(4, forma.Blocos.Count());
            Assert.AreEqual("0+0,0+1,0+2,0+3", forma.ToString());
        }

        [TestMethod]
        public void DeveDesenharFormaJ() {
            var forma = FormaFactory.Desenhar(TipoPeca.J, new Ponto(), 0);
            Assert.AreEqual(4, forma.Blocos.Count());
            Assert.AreEqual("1+0,1+1,1+2,0+2", forma.ToString());
        }

        [TestMethod]
        public void DeveDesenharFormaL() {
            var forma = FormaFactory.Desenhar(TipoPeca.L, new Ponto(), 0);
            Assert.AreEqual("0+0,0+1,0+2,1+2", forma.ToString());
        }

        [TestMethod]
        public void DeveDesenharFormaO() {
            var forma = FormaFactory.Desenhar(TipoPeca.O, new Ponto(), 0);
            Assert.AreEqual(4, forma.Blocos.Count());
            Assert.AreEqual("0+0,0+1,1+0,1+1", forma.ToString());
        }

        [TestMethod]
        public void DeveDesenharFormaS() {
            var forma = FormaFactory.Desenhar(TipoPeca.S, new Ponto(), 0);
            Assert.AreEqual("0+0,0+1,1+1,1+2", forma.ToString());
        }

        [TestMethod]
        public void DeveDesenharFormaT() {
            var forma = FormaFactory.Desenhar(TipoPeca.T, new Ponto(), 0);
            Assert.AreEqual(4, forma.Blocos.Count());
            Assert.AreEqual("0+0,0+1,0+2,1+1", forma.ToString());
        }

        [TestMethod]
        public void DeveDesenharFormaZ() {
            var forma = FormaFactory.Desenhar(TipoPeca.Z, new Ponto(), 0);
            Assert.AreEqual(4, forma.Blocos.Count());
            Assert.AreEqual("1+0,1+1,0+1,0+2", forma.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void NaoDeveDesenharNenhumaForma() {
            var forma = FormaFactory.Desenhar((TipoPeca)8, new Ponto(), 0);
        }
    }
}
