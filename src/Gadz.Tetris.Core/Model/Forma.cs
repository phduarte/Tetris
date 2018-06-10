namespace Gadz.Tetris.Model {
    public abstract class Forma {

        public Bloco[] Blocos { get; protected set; }

        public static Forma Desenhar(TiposDePeca tipo, Ponto ponto, int rotacao) {
            return FormaFactory.Desenhar(tipo, ponto, rotacao);
        }

        public static Forma Desenhar(TiposDePeca tipo) {
            return Desenhar(tipo, new Ponto(0, 0), 0);
        }

        public static Forma Desenhar(TiposDePeca tipo, Ponto ponto) {
            return Desenhar(tipo, ponto, 0);
        }

        public static Forma Desenhar(TiposDePeca tipo, int rotacao) {
            return Desenhar(tipo, new Ponto(0, 0), rotacao);
        }

        #region overrided methods

        public override string ToString() {
            return $"{Blocos[0]},{Blocos[1]},{Blocos[2]},{Blocos[3]}";
        }

        #endregion
    }
}
