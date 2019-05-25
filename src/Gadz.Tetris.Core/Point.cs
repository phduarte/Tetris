namespace Gadz.Tetris {
    public struct Point {

        public int X, Y;

        public Point(int x, int y) {
            X = x;
            Y = y;
        }

        public override string ToString() {
            return $"{X}+{Y}";
        }

        public override bool Equals(object obj) {
            if (obj is Point u) {
                return ToString().Equals(u.ToString());
            }
            return false;
        }

        public static bool operator ==(Point a, Point b) {
            return a.Equals(b);
        }

        public static bool operator !=(Point a, Point b) {
            return !a.Equals(b);
        }

        public bool ColideCom(Point ponto) {
            return Equals(ponto);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
