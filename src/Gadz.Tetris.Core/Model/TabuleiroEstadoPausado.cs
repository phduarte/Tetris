namespace Gadz.Tetris.Model {
    internal class TabuleiroEstadoPausado : ITabuleiroEstado {

        public bool EstaJogando => false;
        public bool PodeAlterarEstatisticas => false;
        public bool PodeIniciar => false;
        public bool PodeMovimentarBloco => false;
        public bool PodePausar => false;
        public bool PodeReiniciar => true;
        public bool PodeTerminar => true;
        public override string ToString() => "Parado";
    }
}
