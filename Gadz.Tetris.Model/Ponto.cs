namespace Gadz.Tetris.Model {
    public struct Ponto {

        public int X, Y;

        public Ponto(int x, int y) {
            X = x;
            Y = y;
        }

        public override string ToString() {
            return $"{X}+{Y}";
        }

        public override bool Equals(object obj) {
            if(obj is Ponto u) {
                return ToString().Equals(u.ToString());
            }
            return false;
        }

        public bool ColideCom(Ponto ponto) {
            return Equals(ponto);
        }
    }
}
