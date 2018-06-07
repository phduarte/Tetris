using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Gadz.Tetris.Model {

    [TestClass]
    public class FormasTests {

        [TestMethod]
        public void DeveDesenharFormaI() {
            var forma = Forma.Desenhar(TiposDePecas.I, new Ponto(), 0);
            Assert.AreEqual(4, forma.Blocos.Count());
            Assert.AreEqual("0+0,0+1,0+2,0+3", forma.ToString());
        }

        [TestMethod]
        public void DeveDesenharFormaJ() {
            var forma = Forma.Desenhar(TiposDePecas.J, new Ponto(), 0);
            Assert.AreEqual(4, forma.Blocos.Count());
            Assert.AreEqual("1+0,1+1,1+2,0+2", forma.ToString());
        }

        [TestMethod]
        public void DeveDesenharFormaL() {
            var forma = Forma.Desenhar(TiposDePecas.L, new Ponto(), 0);
            Assert.AreEqual("0+0,0+1,0+2,1+2", forma.ToString());
        }

        [TestMethod]
        public void DeveDesenharFormaO() {
            var forma = Forma.Desenhar(TiposDePecas.O, new Ponto(), 0);
            Assert.AreEqual(4, forma.Blocos.Count());
            Assert.AreEqual("0+0,0+1,1+0,1+1", forma.ToString());
        }

        [TestMethod]
        public void DeveDesenharFormaS() {
            var forma = Forma.Desenhar(TiposDePecas.S, new Ponto(), 0);
            Assert.AreEqual("0+0,0+1,1+1,1+2", forma.ToString());
        }

        [TestMethod]
        public void DeveDesenharFormaT() {
            var forma = Forma.Desenhar(TiposDePecas.T, new Ponto(), 0);
            Assert.AreEqual(4, forma.Blocos.Count());
            Assert.AreEqual("0+0,0+1,0+2,1+1", forma.ToString());
        }

        [TestMethod]
        public void DeveDesenharFormaZ() {
            var forma = Forma.Desenhar(TiposDePecas.Z, new Ponto(), 0);
            Assert.AreEqual(4, forma.Blocos.Count());
            Assert.AreEqual("1+0,1+1,0+1,0+2", forma.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void NaoDeveDesenharNenhumaForma() {
            var forma = Forma.Desenhar((TiposDePecas)8, new Ponto(), 0);
        }
    }
}
