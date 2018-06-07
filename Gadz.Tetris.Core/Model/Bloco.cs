namespace Gadz.Tetris.Model {
    public struct Bloco {

        public Ponto Ponto { get; private set; }
        public CoresDasPecas Cor { get; private set; }
        public int X { get { return Ponto.X; } }
        public int Y { get { return Ponto.Y; } }

        public Bloco(int x, int y, CoresDasPecas cor) {
            Ponto = new Ponto(x, y);
            Cor = cor;
        }

        public bool ColideCom(Bloco bloco) {
            return Ponto.ColideCom(bloco.Ponto);
        }

        public override string ToString() {
            return Ponto.ToString();
        }
    }
}
