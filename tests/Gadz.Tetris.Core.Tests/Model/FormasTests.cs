using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Gadz.Tetris.Model {

    [TestClass]
    public class FormasTests {

        [TestMethod]
        public void DeveDesenharFormaI() {
            var forma = Forma.Desenhar(TiposDePeca.I, new Ponto(), 0);
            Assert.AreEqual(4, forma.Blocos.Count());
            Assert.AreEqual("0+0,0+1,0+2,0+3", forma.ToString());
        }

        [TestMethod]
        public void DeveDesenharFormaJ() {
            var forma = Forma.Desenhar(TiposDePeca.J, new Ponto(), 0);
            Assert.AreEqual(4, forma.Blocos.Count());
            Assert.AreEqual("1+0,1+1,1+2,0+2", forma.ToString());
        }

        [TestMethod]
        public void DeveDesenharFormaL() {
            var forma = Forma.Desenhar(TiposDePeca.L, new Ponto(), 0);
            Assert.AreEqual("0+0,0+1,0+2,1+2", forma.ToString());
        }

        [TestMethod]
        public void DeveDesenharFormaO() {
            var forma = Forma.Desenhar(TiposDePeca.O, new Ponto(), 0);
            Assert.AreEqual(4, forma.Blocos.Count());
            Assert.AreEqual("0+0,0+1,1+0,1+1", forma.ToString());
        }

        [TestMethod]
        public void DeveDesenharFormaS() {
            var forma = Forma.Desenhar(TiposDePeca.S, new Ponto(), 0);
            Assert.AreEqual("0+0,0+1,1+1,1+2", forma.ToString());
        }

        [TestMethod]
        public void DeveDesenharFormaT() {
            var forma = Forma.Desenhar(TiposDePeca.T, new Ponto(), 0);
            Assert.AreEqual(4, forma.Blocos.Count());
            Assert.AreEqual("0+0,0+1,0+2,1+1", forma.ToString());
        }

        [TestMethod]
        public void DeveDesenharFormaZ() {
            var forma = Forma.Desenhar(TiposDePeca.Z, new Ponto(), 0);
            Assert.AreEqual(4, forma.Blocos.Count());
            Assert.AreEqual("1+0,1+1,0+1,0+2", forma.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void NaoDeveDesenharNenhumaForma() {
            var forma = Forma.Desenhar((TiposDePeca)8, new Ponto(), 0);
        }
    }
}
