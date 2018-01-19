namespace Gadz.Tetris.Model.Pecas.Formas {
    public class Forma : IForma {

        public int Rotacao { get; private set; }
        public Bloco[] Blocos { get; private set; }

        public Forma(int x, int y, TipoPeca tipo, CorPeca cor, int rotacao) {
            Rotacao = rotacao;
            Blocos = Desenhar(x, y, tipo, cor).Blocos;
        }

        public Forma(Bloco[] blocos) {
            Blocos = blocos;
        }

        public IForma Desenhar(int x, int y, TipoPeca tipo, CorPeca cor) {

            switch (tipo) {
                case TipoPeca.T:
                    return TetraminoT.Desenhar(x, y, Rotacao, cor);

                case TipoPeca.O:
                    return TetraminoO.Desenhar(x, y, cor);

                case TipoPeca.I:
                    return TetraminoI.Desenhar(x, y, Rotacao, cor);

                case TipoPeca.L:
                    return TetraminoL.Desenhar(x, y, Rotacao, cor);

                case TipoPeca.J:
                    return TetraminoJ.Desenhar(x, y, Rotacao, cor);

                case TipoPeca.S:
                    return TetraminoS.Desenhar(x, y, Rotacao, cor);

                case TipoPeca.Z:
                    return TetraminoZ.Desenhar(x, y, Rotacao, cor);
            }

            return null;
        }

        public override string ToString() {
            return $"{Blocos[0]},{Blocos[1]},{Blocos[2]},{Blocos[3]}";
        }
    }
}
