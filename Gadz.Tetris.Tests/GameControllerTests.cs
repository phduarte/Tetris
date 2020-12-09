using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading;

namespace Gadz.Tetris
{
    /// <summary>
    /// Defines the <see cref="GameControllerTests" />
    /// </summary>
    [TestClass]
    public class GameControllerTests
    {
        /// <summary>
        /// Defines the app
        /// </summary>
        private GameController app;

        /// <summary>
        /// The Setup
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            app = GameController.Create(10, 20);
        }

        /// <summary>
        /// The DeveInstanciar
        /// </summary>
        [TestMethod]
        public void DeveInstanciar()
        {
            Assert.IsNotNull(app);
            Assert.AreEqual(10, app.BoardWidth);
            Assert.AreEqual(20, app.BoardHeight);
        }

        /// <summary>
        /// The NaoDeveInstanciar
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NaoDeveInstanciar()
        {
            var app = GameController.Create(4, 4);
        }

        /// <summary>
        /// The DeveIniciar
        /// </summary>
        [TestMethod]
        public void DeveIniciar()
        {
            app.Start();

            Assert.AreEqual("Playing", app.State.ToString());
            Assert.IsTrue(app.Playing);
        }

        /// <summary>
        /// The DeveReiniciar
        /// </summary>
        [TestMethod]
        public void DeveReiniciar()
        {
            app.Start();
            Assert.AreEqual("Playing", app.State.ToString(), "Não está iniciando");
            app.Pause();
            Assert.AreEqual("Parado", app.State.ToString(), "Não está pausando");
            app.Restart();
            Assert.AreEqual("Playing", app.State.ToString(), "Não está continuando");
        }

        /// <summary>
        /// The DevePausar
        /// </summary>
        [TestMethod]
        public void DevePausar()
        {
            app.Start();
            Assert.AreEqual("Playing", app.State.ToString(), "Não está iniciando");
            app.Pause();
            Assert.AreEqual("Parado", app.State.ToString(), "Não está pausando");
            Assert.IsFalse(app.Playing, "Está informando que está jogando.");
        }

        /// <summary>
        /// The DeveContinuar
        /// </summary>
        [TestMethod]
        public void DeveContinuar()
        {
            app.Start();
            Assert.AreEqual("Playing", app.State.ToString(), "Não está iniciando");
            app.Pause();
            Assert.AreEqual("Parado", app.State.ToString(), "Não está pausando");
            app.Continue();
            Assert.AreEqual("Playing", app.State.ToString(), "Não está continuando");
        }

        /// <summary>
        /// The DeveIniciarComNivelUm
        /// </summary>
        [TestMethod]
        public void DeveIniciarComNivelUm()
        {
            app.Start();

            Assert.AreEqual(1, app.Level);
        }

        /// <summary>
        /// The DeveIniciarNaVelocidadeMaisAlta
        /// </summary>
        [TestMethod]
        public void DeveIniciarNaVelocidadeMaisAlta()
        {
            app.Start();

            Assert.AreEqual(1, app.Speed);
        }

        /// <summary>
        /// The DeveConseguirMedirOTempoDaPartida
        /// </summary>
        [TestMethod]
        public void DeveConseguirMedirOTempoDaPartida()
        {
            Assert.AreEqual(0, app.Duration.Ticks);

            app.Start();

            Thread.Sleep(50);

            Assert.AreNotEqual(0, app.Duration.Ticks);
        }

        /// <summary>
        /// The DeveIniciarComPlacarEmBranco
        /// </summary>
        [TestMethod]
        public void DeveIniciarComPlacarEmBranco()
        {
            app.Start();
            Assert.AreEqual(0, app.Score);
        }

        /// <summary>
        /// The DeveIniciarSemNenhumaLinhaPreenchida
        /// </summary>
        [TestMethod]
        public void DeveIniciarSemNenhumaLinhaPreenchida()
        {
            app.Start();
            Assert.AreEqual(0, app.Lines);
        }

        /// <summary>
        /// The DeveIniciarComBlocoNaPosicaoZero
        /// </summary>
        [TestMethod]
        public void DeveIniciarComBlocoNaPosicaoZero()
        {
            app.Start();
            Assert.AreEqual(0, app.CurrentPiecePosition.X, "Posição X não está em 0");
            Assert.AreEqual(0, app.CurrentPiecePosition.Y, "Posição Y não está em 0");
        }

        /// <summary>
        /// The DeveMoverParaDireita
        /// </summary>
        [TestMethod]
        public void DeveMoverParaDireita()
        {
            app.Start();
            Assert.AreEqual(0, app.CurrentPiecePosition.X);
            app.MoveRight();
            Assert.AreEqual(1, app.CurrentPiecePosition.X);
        }

        /// <summary>
        /// The DeveMoverParaEsquerda
        /// </summary>
        [TestMethod]
        public void DeveMoverParaEsquerda()
        {
            DeveMoverParaDireita();

            app.MoveLeft();
            Assert.AreEqual(0, app.CurrentPiecePosition.X);
        }

        /// <summary>
        /// The DeveMoverParaBaixo
        /// </summary>
        [TestMethod]
        public void DeveMoverParaBaixo()
        {
            app.Start();
            Assert.AreEqual(0, app.CurrentPiecePosition.Y);
            app.MoveDown();
            Assert.AreEqual(2, app.CurrentPiecePosition.Y);
        }

        /// <summary>
        /// The DeveCorrerParaDireita
        /// </summary>
        [TestMethod]
        public void DeveCorrerParaDireita()
        {
            app.Start();
            app.RunRight();
            Thread.Sleep(100);
            Assert.IsTrue(app.CurrentPiecePosition.X > 5);
        }

        /// <summary>
        /// The DeveCorrerParaEsquerda
        /// </summary>
        [TestMethod]
        public void DeveCorrerParaEsquerda()
        {
            DeveCorrerParaDireita();
            app.RunLeft();
            Thread.Sleep(100);
            Assert.AreEqual(0, app.CurrentPiecePosition.X);
        }

        /// <summary>
        /// The DeveCorrerParaBaixo
        /// </summary>
        [TestMethod]
        public void DeveCorrerParaBaixo()
        {
            app.Start();
            var p = app.Board.CurrentPiece;
            app.SmashDown();
            //Thread.Sleep(100);
            Assert.AreNotEqual(app.Board.CurrentPiece, p, $"Uma nova peça não foi introduzida no jogo.");
        }

        /// <summary>
        /// The DeveListarProximosBlocos
        /// </summary>
        [TestMethod]
        public void DeveListarProximosBlocos()
        {
            app.Start();
            Assert.AreEqual(4, app.GetNextBlocks().Count());
        }

        /// <summary>
        /// The DeveListarOsBlocosAtuais
        /// </summary>
        [TestMethod]
        public void DeveListarOsBlocosAtuais()
        {
            app.Start();
            Assert.AreEqual(4, app.GetActualBlocks().Count());
        }

        /// <summary>
        /// The DeveGuardarORecordeMaximo
        /// </summary>
        [TestMethod]
        public void DeveGuardarORecordeMaximo()
        {
            app.Start();
            app.Exit();
            Assert.AreNotEqual(0, app.GetMaxScore());
        }
    }
}
