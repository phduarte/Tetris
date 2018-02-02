namespace Gadz.Tetris.Core.DomainModel.Tabuleiros {
    public interface ITabuleiroEstado {
        bool EstaJogando { get; }
        bool PodeIniciar { get; }
        bool PodePausar { get; }
        bool PodeTerminar { get; }
        bool PodeReiniciar { get; }
        bool PodeMovimentarBloco { get; }
        bool PodeAlterarEstatisticas { get; }
    }
}