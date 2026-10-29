using Gadz.Tetris.Application;
using Gadz.Tetris.Application.Commands;
using Gadz.Tetris.Tests.Model;
using Gadz.Tetris.Web.Input;
using NUnit.Framework;

namespace Gadz.Tetris.Tests.Web;

/// <summary>
/// Testes da tradução das teclas do navegador (<c>KeyboardEvent.key</c>) para as teclas do jogo.
/// </summary>
public class TecladoWebTests
{
    /// <summary>
    /// Cada tecla do navegador usada na partida vira a tecla do jogo equivalente.
    /// </summary>
    [TestCase("ArrowLeft", Key.Left)]
    [TestCase("ArrowRight", Key.Right)]
    [TestCase("ArrowUp", Key.Up)]
    [TestCase("ArrowDown", Key.Down)]
    [TestCase(" ", Key.Space)]
    [TestCase("Enter", Key.Enter)]
    [TestCase("Escape", Key.Escape)]
    [TestCase("Shift", Key.ShiftKey)]
    public void DeveTraduzirATeclaDoNavegadorParaATeclaDoJogo(string key, Key expected)
    {
        Assert.IsTrue(KeyMap.TryGetGameKey(key, out var gameKey));
        Assert.AreEqual(expected, gameKey);
    }

    /// <summary>
    /// Teclas que o jogo não usa não viram teclas do jogo, e o navegador mantém a ação padrão delas.
    /// </summary>
    [TestCase("a")]
    [TestCase("Tab")]
    [TestCase("F5")]
    [TestCase("F12")]
    public void NaoDeveTratarTeclasQueOJogoNaoUsa(string key)
    {
        Assert.IsFalse(KeyMap.TryGetGameKey(key, out _));
        Assert.IsFalse(KeyMap.HandledKeys.Contains(key));
    }

    /// <summary>
    /// Toda tecla do jogo tratada pela Web tem um comando no <see cref="CommandFactory"/>.
    /// </summary>
    [Test]
    public void DeveHaverUmComandoParaCadaTeclaDoJogo()
    {
        var factory = new CommandFactory(GameController.Create(10, 20, new InMemoryStatsRepository(), new FakeSoundPlayer()));

        foreach (var key in KeyMap.HandledKeys)
        {
            if (KeyMap.TryGetGameKey(key, out var gameKey))
            {
                Assert.IsTrue(factory.GetAll(gameKey, false).Any(), $"Sem comando para {key}");
            }
        }
    }

    /// <summary>
    /// Segurar a seta repete a tecla: o primeiro toque e as repetições casam com comandos diferentes.
    /// </summary>
    [Test]
    public void DeveSepararOPrimeiroToqueDasRepeticoesDaSeta()
    {
        var factory = new CommandFactory(GameController.Create(10, 20, new InMemoryStatsRepository(), new FakeSoundPlayer()));
        KeyMap.TryGetGameKey(KeyMap.LEFT, out var left);

        var first = factory.GetAll(left, false, false).First();
        var repeat = factory.GetAll(left, false, true).First();

        Assert.AreNotEqual(first.GetType(), repeat.GetType());
    }
}
