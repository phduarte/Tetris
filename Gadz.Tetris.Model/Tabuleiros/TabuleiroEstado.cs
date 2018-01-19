namespace Gadz.Tetris.Model.Tabuleiros {
    public abstract class TabuleiroEstado {
        public bool EstaJogando { get; protected set; }
        public abstract bool PodeIniciar(ITabuleiro tabuleiro);
        public abstract bool PodePausar(ITabuleiro tabuleiro);
        public abstract bool PodeTerminar(ITabuleiro tabuleiro);
        public abstract bool PodeReiniciar(ITabuleiro tabuleiro);
        public abstract bool PodeMovimentarBloco(ITabuleiro tabuleiro);
        public abstract bool PodeAlterarEstatisticas(ITabuleiro tabuleiro);
    }
}