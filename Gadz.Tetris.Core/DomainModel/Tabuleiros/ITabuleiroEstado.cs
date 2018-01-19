namespace Gadz.Tetris.Core.DomainModel.Tabuleiros {
    public interface ITabuleiroEstado {
        bool EstaJogando { get; }
        bool PodeIniciar(ITabuleiro tabuleiro);
        bool PodePausar(ITabuleiro tabuleiro);
        bool PodeTerminar(ITabuleiro tabuleiro);
        bool PodeReiniciar(ITabuleiro tabuleiro);
        bool PodeMovimentarBloco(ITabuleiro tabuleiro);
        bool PodeAlterarEstatisticas(ITabuleiro tabuleiro);
    }
}