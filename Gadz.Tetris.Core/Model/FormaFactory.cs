using System;

namespace Gadz.Tetris.Model {
    class FormaFactory {

        public static Forma Desenhar(TiposDePecas tipo, Ponto ponto, int rotacao) {

            var cor = Cores.PegarCorPara(tipo);

            switch (tipo) {
                case TiposDePecas.T:
                    return new FormaTetraminoT(ponto, rotacao, cor);

                case TiposDePecas.O:
                    return new FormaTetraminoO(ponto, cor);

                case TiposDePecas.I:
                    return new FormaTetraminoI(ponto, rotacao, cor);

                case TiposDePecas.L:
                    return new FormaTetraminoL(ponto, rotacao, cor);

                case TiposDePecas.J:
                    return new FormaTetraminoJ(ponto, rotacao, cor);

                case TiposDePecas.S:
                    return new FormaTetraminoS(ponto, rotacao, cor);

                case TiposDePecas.Z:
                    return new FormaTetraminoZ(ponto, rotacao, cor);

                default:
                    throw new NotImplementedException();
            }

        }
    }
}
