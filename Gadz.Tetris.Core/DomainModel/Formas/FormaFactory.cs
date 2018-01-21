using Gadz.Tetris.Core.DomainModel.Pecas;
using System;

namespace Gadz.Tetris.Core.DomainModel.Formas {
    public class FormaFactory {

        public static Forma Desenhar(TipoPeca tipo, Ponto ponto, int rotacao) {

            var cor = Cores.PegarCorPara(tipo);

            switch (tipo) {
                case TipoPeca.T:
                    return new TetraminoT(ponto, rotacao, cor);

                case TipoPeca.O:
                    return new TetraminoO(ponto, cor);

                case TipoPeca.I:
                    return new TetraminoI(ponto, rotacao, cor);

                case TipoPeca.L:
                    return new TetraminoL(ponto, rotacao, cor);

                case TipoPeca.J:
                    return new TetraminoJ(ponto, rotacao, cor);

                case TipoPeca.S:
                    return new TetraminoS(ponto, rotacao, cor);

                case TipoPeca.Z:
                    return new TetraminoZ(ponto, rotacao, cor);
            }

            throw new NotImplementedException();
        }
    }
}
