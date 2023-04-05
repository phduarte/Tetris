using Gadz.Tetris.Application;
using NUnit.Framework;

namespace Gadz.Tetris.Tests
{
    /// <summary>
    /// Defines the <see cref="GameControllerTests" />
    /// </summary>
    public class GameControllerTests
    {
        /// <summary>
        /// Defines the app
        /// </summary>
        private GameController app;

        /// <summary>
        /// The Setup
        /// </summary>
        [SetUp]
        public void Setup()
        {
            app = GameController.Create(10, 20);
        }

        /// <summary>
        /// The DeveInstanciar
        /// </summary>
        [Test]
        public void DeveInstanciar()
        {
            Assert.IsNotNull(app);
            Assert.AreEqual(10, app.BoardWidth);
            Assert.AreEqual(20, app.BoardHeight);
        }

        /// <summary>
        /// The NaoDeveInstanciar
        /// </summary>
        [Test]
        public void NaoDeveInstanciar()
        {
            Assert.Throws<ArgumentException>(() => GameController.Create(4, 4));
        }

        /// <summary>
        /// The DeveIniciar
        /// </summary>
        [Test]
        public void DeveIniciar()
        {
            app.Start();

            Assert.AreEqual("Playing", app.State.ToString());
            Assert.IsTrue(app.Playing);
        }

        /// <summary>
        /// The DeveReiniciar
        /// </summary>
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
        public void DeveIniciarComNivelUm()
        {
            app.Start();

            Assert.AreEqual(1, app.Level);
        }

        /// <summary>
        /// The DeveIniciarNaVelocidadeMaisAlta
        /// </summary>
        [Test]
        public void DeveIniciarNaVelocidadeMaisAlta()
        {
            app.Start();

            Assert.AreEqual(1, app.Speed);
        }

        /// <summary>
        /// The DeveConseguirMedirOTempoDaPartida
        /// </summary>
        [Test]
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
        [Test]
        public void DeveIniciarComPlacarEmBranco()
        {
            app.Start();
            Assert.AreEqual(0, app.Score);
        }

        /// <summary>
        /// The DeveIniciarSemNenhumaLinhaPreenchida
        /// </summary>
        [Test]
        public void DeveIniciarSemNenhumaLinhaPreenchida()
        {
            app.Start();
            Assert.AreEqual(0, app.Lines);
        }

        /// <summary>
        /// The DeveIniciarComBlocoNaPosicaoZero
        /// </summary>
        [Test]
        public void DeveIniciarComBlocoNaPosicaoZero()
        {
            app.Start();
            Assert.AreEqual(0, app.CurrentPiecePosition.X, "Posição X não está em 0");
            Assert.AreEqual(0, app.CurrentPiecePosition.Y, "Posição Y não está em 0");
        }

        /// <summary>
        /// The DeveMoverParaDireita
        /// </summary>
        [Test]
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
        [Test]
        public void DeveMoverParaEsquerda()
        {
            DeveMoverParaDireita();

            app.MoveLeft();
            Assert.AreEqual(0, app.CurrentPiecePosition.X);
        }

        /// <summary>
        /// The DeveMoverParaBaixo
        /// </summary>
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
        public void DeveListarProximosBlocos()
        {
            app.Start();
            Assert.AreEqual(4, app.GetNextBlocks().Count());
        }

        /// <summary>
        /// The DeveListarOsBlocosAtuais
        /// </summary>
        [Test]
        public void DeveListarOsBlocosAtuais()
        {
            app.Start();
            Assert.AreEqual(4, app.GetActualBlocks().Count());
        }

        /// <summary>
        /// The DeveGuardarORecordeMaximo
        /// </summary>
        [Test]
        public void DeveGuardarORecordeMaximo()
        {
            app.Start();
            app.Exit();
            Assert.AreNotEqual(0, app.GetMaxScore());
        }
    }
}
