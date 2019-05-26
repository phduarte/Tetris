using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading;

namespace Gadz.Tetris.Tests
{
    [TestClass]
    public class GameControllerTests
    {
        GameController app;

        [TestInitialize]
        public void Setup()
        {
            app = GameController.Create(10, 20);
        }

        [TestMethod]
        public void DeveInstanciar()
        {
            Assert.IsNotNull(app);
            Assert.AreEqual(10, app.BoardWidth);
            Assert.AreEqual(20, app.BoardHeight);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NaoDeveInstanciar()
        {
            var app = GameController.Create(4, 4);
        }

        [TestMethod]
        public void DeveIniciar()
        {
            app.Start();

            Assert.AreEqual("Jogando", app.State.ToString());
            Assert.IsTrue(app.Playing);
        }

        [TestMethod]
        public void DeveReiniciar()
        {
            app.Start();
            Assert.AreEqual("Jogando", app.State.ToString(), "Não está iniciando");
            app.Pause();
            Assert.AreEqual("Parado", app.State.ToString(), "Não está pausando");
            app.Restart();
            Assert.AreEqual("Jogando", app.State.ToString(), "Não está continuando");
        }

        [TestMethod]
        public void DevePausar()
        {
            app.Start();
            Assert.AreEqual("Jogando", app.State.ToString(), "Não está iniciando");
            app.Pause();
            Assert.AreEqual("Parado", app.State.ToString(), "Não está pausando");
            Assert.IsFalse(app.Playing, "Está informando que está jogando.");
        }

        [TestMethod]
        public void DeveContinuar()
        {
            app.Start();
            Assert.AreEqual("Jogando", app.State.ToString(), "Não está iniciando");
            app.Pause();
            Assert.AreEqual("Parado", app.State.ToString(), "Não está pausando");
            app.Continue();
            Assert.AreEqual("Jogando", app.State.ToString(), "Não está continuando");
        }

        [TestMethod]
        public void DeveIniciarComNivelUm()
        {
            app.Start();

            Assert.AreEqual(1, app.Level);
        }

        [TestMethod]
        public void DeveIniciarNaVelocidadeMaisAlta()
        {
            app.Start();

            Assert.AreEqual(1000, app.Speed);
        }
        [TestMethod]
        public void DeveConseguirMedirOTempoDaPartida()
        {

            Assert.AreEqual(0, app.Duration.Ticks);

            app.Start();

            Thread.Sleep(50);

            Assert.AreNotEqual(0, app.Duration.Ticks);
        }

        [TestMethod]
        public void DeveIniciarComPlacarEmBranco()
        {
            app.Start();
            Assert.AreEqual(0, app.Score);
        }

        [TestMethod]
        public void DeveIniciarSemNenhumaLinhaPreenchida()
        {
            app.Start();
            Assert.AreEqual(0, app.Lines);
        }

        [TestMethod]
        public void DeveIniciarComBlocoNaPosicaoZero()
        {
            app.Start();
            Assert.AreEqual(0, app.CurrentPiecePosition.X, "Posição X não está em 0");
            Assert.AreEqual(0, app.CurrentPiecePosition.Y, "Posição Y não está em 0");
        }

        [TestMethod]
        public void DeveMoverParaDireita()
        {
            app.Start();
            Assert.AreEqual(0, app.CurrentPiecePosition.X);
            app.MoveRight();
            Assert.AreEqual(1, app.CurrentPiecePosition.X);
        }

        [TestMethod]
        public void DeveMoverParaEsquerda()
        {

            DeveMoverParaDireita();

            app.MoveLeft();
            Assert.AreEqual(0, app.CurrentPiecePosition.X);
        }

        [TestMethod]
        public void DeveMoverParaBaixo()
        {
            app.Start();
            Assert.AreEqual(0, app.CurrentPiecePosition.Y);
            app.MoveDown();
            Assert.AreEqual(2, app.CurrentPiecePosition.Y);
        }

        [TestMethod]
        public void DeveCorrerParaDireita()
        {
            app.Start();
            app.RunRight();
            Thread.Sleep(100);
            Assert.IsTrue(app.CurrentPiecePosition.X > 5);
        }

        [TestMethod]
        public void DeveCorrerParaEsquerda()
        {
            DeveCorrerParaDireita();
            app.RunLeft();
            Thread.Sleep(100);
            Assert.AreEqual(0, app.CurrentPiecePosition.X);
        }

        [TestMethod]
        public void DeveCorrerParaBaixo()
        {
            app.Start();
            app.SmashDown();
            Thread.Sleep(100);
            Assert.IsTrue(app.CurrentPiecePosition.Y > 15, $"A posição atual da peça no eixo Y é {app.CurrentPiecePosition.Y}");
        }

        [TestMethod]
        public void DeveListarProximosBlocos()
        {
            app.Start();
            Assert.AreEqual(4, app.GetNextBlocks().Count());
        }

        [TestMethod]
        public void DeveListarOsBlocosAtuais()
        {
            app.Start();
            Assert.AreEqual(4, app.GetActualBlocks().Count());
        }

        [TestMethod]
        public void DeveGuardarORecordeMaximo()
        {
            app.Start();
            //app.CurrentBoard.Estatisticas.Pontuar(1);
            app.Exit();
            Assert.AreNotEqual(0, app.GetMaxScore());
        }
    }
}
