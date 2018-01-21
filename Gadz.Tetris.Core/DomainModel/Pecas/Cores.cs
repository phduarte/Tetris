using System.Collections.Generic;

namespace Gadz.Tetris.Core.DomainModel.Pecas {
    public class Cores {

        static IDictionary<TipoPeca, CorPeca> _cores = new Dictionary<TipoPeca, CorPeca> {
                { TipoPeca.I, CorPeca.CIANO },
                { TipoPeca.T, CorPeca.ROXO},
                { TipoPeca.L, CorPeca.LARANJA},
                { TipoPeca.J, CorPeca.AZUL},
                { TipoPeca.O, CorPeca.AMARELO},
                { TipoPeca.S, CorPeca.VERDE},
                { TipoPeca.Z, CorPeca.VERMELHO}
        };

        public static CorPeca PegarCorPara(TipoPeca index) {
            try {
                return _cores[index];
            } catch (KeyNotFoundException) {
                return CorPeca.TRANSPARENTE;
            }
        }
    }
}
