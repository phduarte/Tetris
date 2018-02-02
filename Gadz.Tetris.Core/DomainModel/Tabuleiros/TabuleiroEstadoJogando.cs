namespace Gadz.Tetris.Core.DomainModel.Tabuleiros {
    public class TabuleiroEstadoJogando : ITabuleiroEstado {

        public bool EstaJogando => true;
        public bool PodeAlterarEstatisticas => true;
        public bool PodeIniciar => false;
        public bool PodeMovimentarBloco => true;
        public bool PodePausar => true;
        public bool PodeReiniciar => true;
        public bool PodeTerminar => true;
        public override string ToString() => "Jogando";
    }
}
