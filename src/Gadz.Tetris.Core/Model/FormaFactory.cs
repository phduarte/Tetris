using System;

namespace Gadz.Tetris.Model {
    class FormaFactory {

        public static Forma Desenhar(TiposDePeca tipo, Ponto ponto, int rotacao) {

            var cor = Peca.PegarCorPara(tipo);

            switch (tipo) {
                case TiposDePeca.T:
                    return new FormaTetraminoT(ponto, rotacao, cor);

                case TiposDePeca.O:
                    return new FormaTetraminoO(ponto, cor);

                case TiposDePeca.I:
                    return new FormaTetraminoI(ponto, rotacao, cor);

                case TiposDePeca.L:
                    return new FormaTetraminoL(ponto, rotacao, cor);

                case TiposDePeca.J:
                    return new FormaTetraminoJ(ponto, rotacao, cor);

                case TiposDePeca.S:
                    return new FormaTetraminoS(ponto, rotacao, cor);

                case TiposDePeca.Z:
                    return new FormaTetraminoZ(ponto, rotacao, cor);

                default:
                    throw new NotImplementedException();
            }

        }
    }
}
