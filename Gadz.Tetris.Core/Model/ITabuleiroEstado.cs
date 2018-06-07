namespace Gadz.Tetris.Model {
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