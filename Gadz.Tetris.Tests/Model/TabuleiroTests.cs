using Gadz.Tetris.Domain.Models;
using Gadz.Tetris.Domain.Models.Boards;
using Gadz.Tetris.Domain.Models.Pieces;
using NUnit.Framework;

namespace Gadz.Tetris.Tests.Model;

/// <summary>
/// Testes das regras do <see cref="Board"/>.
/// </summary>
public class TabuleiroTests
{
    /// <summary>
    /// Tabuleiro 10×20 em jogo, recriado a cada teste.
    /// </summary>
    private Board board;

    /// <summary>
    /// Cria e inicia um tabuleiro novo.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        board = new Board(new FakeStatsRepository(), 10, 20);
        board.Start();
    }

    /// <summary>
    /// Encerra o jogo para que o loop em background pare.
    /// </summary>
    [TearDown]
    public void TearDown()
    {
        board.Finish();
    }

    /// <summary>
    /// Uma peça I vertical encostada numa coluna de blocos não pode girar para a horizontal por cima dela.
    /// </summary>
    [Test]
    public void NaoDeveGirarSobreBlocosAssentados()
    {
        var column = Enumerable.Range(8, 12).Select(y => new Block(1, y, PieceColor.Blue));
        board.Arrange(CreatePiece(PieceType.I, new Point(0, 8), 0), column);

        board.Rotate();

        Assert.AreEqual(0, board.CurrentPiece.Rotation);
        Assert.IsFalse(HasOverlap(), "A peça atual sobrepôs blocos assentados.");
    }

    /// <summary>
    /// Uma peça I horizontal na última linha não pode girar para a vertical para fora do tabuleiro.
    /// </summary>
    [Test]
    public void NaoDeveGirarParaForaDoTabuleiro()
    {
        board.Arrange(CreatePiece(PieceType.I, new Point(3, 19), 1), []);

        board.Rotate();

        Assert.AreEqual(1, board.CurrentPiece.Rotation);
    }

    /// <summary>
    /// Em espaço livre a rotação cicla 0→1→2→3→0.
    /// </summary>
    [Test]
    public void DeveGirarEmEspacoLivre()
    {
        board.Arrange(CreatePiece(PieceType.I, new Point(3, 5), 0), []);

        foreach (var expected in new[] { 1, 2, 3, 0 })
        {
            board.Rotate();
            Assert.AreEqual(expected, board.CurrentPiece.Rotation);
        }
    }

    /// <summary>
    /// Depois de perder, reiniciar o jogo volta ao estado <c>Playing</c> com o tabuleiro vazio e a peça descendo.
    /// </summary>
    [Test]
    public void DeveVoltarAJogarAoReiniciarDepoisDePerder()
    {
        LoseGame();

        board.Restart();
        var y = board.CurrentPiece.Position.Y;
        board.MoveDown();

        Assert.AreEqual("Playing", board.State.ToString());
        Assert.AreEqual(0, board.Blocks.Count, "Os blocos da partida anterior continuaram no tabuleiro.");
        Assert.IsTrue(board.CurrentPiece.Position.Y > y, "A peça não desceu após o reinício.");
    }

    /// <summary>
    /// Derruba peças na coluna inicial até o jogo acabar.
    /// </summary>
    private void LoseGame()
    {
        // cada peça ocupa no máximo 4 colunas, então nenhuma linha é completada e a pilha chega ao topo
        for (var i = 0; i < board.Height && board.IsPlaying; i++)
        {
            board.SmashDown();
        }

        Assert.IsFalse(board.IsPlaying, "O jogo deveria ter terminado.");
    }

    /// <summary>
    /// Cria uma peça no tabuleiro do teste.
    /// </summary>
    /// <param name="type">O tipo da peça.</param>
    /// <param name="position">A posição da peça.</param>
    /// <param name="rotation">A rotação da peça.</param>
    /// <returns>A peça criada.</returns>
    private Piece CreatePiece(PieceType type, Point position, int rotation)
    {
        return new PieceBuilder().OfType(type).OnPosition(position).WithRotation(rotation).OnBoard(board).Build();
    }

    /// <summary>
    /// Informa se algum bloco da peça atual ocupa a mesma célula de um bloco assentado.
    /// </summary>
    /// <returns><c>true</c> se houver sobreposição.</returns>
    private bool HasOverlap()
    {
        return board.CurrentPiece.Blocks.Any(piece => board.Blocks.Any(settled => settled.CollideWith(piece)));
    }
}
