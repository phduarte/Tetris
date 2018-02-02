namespace Gadz.Tetris.Core.DomainModel.Tabuleiros {
    public class TabuleiroEstadoTerminado : ITabuleiroEstado {

        public bool EstaJogando => false;
        public bool PodeAlterarEstatisticas => false;
        public bool PodeIniciar => true;
        public bool PodeMovimentarBloco => false;
        public bool PodePausar => false;
        public bool PodeReiniciar => true;
        public bool PodeTerminar => false;
        public override string ToString() => "Terminado";
    }
}
