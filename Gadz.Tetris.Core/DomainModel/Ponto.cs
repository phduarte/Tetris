namespace Gadz.Tetris.Core.DomainModel {
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
            if (obj is Ponto u) {
                return ToString().Equals(u.ToString());
            }
            return false;
        }

        public static bool operator ==(Ponto a, Ponto b) {
            return a.Equals(b);
        }

        public static bool operator !=(Ponto a, Ponto b) {
            return !a.Equals(b);
        }

        public bool ColideCom(Ponto ponto) {
            return Equals(ponto);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
