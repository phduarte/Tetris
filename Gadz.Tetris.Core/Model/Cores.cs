using System.Collections.Generic;

namespace Gadz.Tetris.Model {
    public class Cores {

        static IDictionary<TiposDePecas, CoresDasPecas> _cores = new Dictionary<TiposDePecas, CoresDasPecas> {
                { TiposDePecas.I, CoresDasPecas.Ciano },
                { TiposDePecas.T, CoresDasPecas.Roxo},
                { TiposDePecas.L, CoresDasPecas.Laranja},
                { TiposDePecas.J, CoresDasPecas.Azul},
                { TiposDePecas.O, CoresDasPecas.Amarelo},
                { TiposDePecas.S, CoresDasPecas.Verde},
                { TiposDePecas.Z, CoresDasPecas.Vermelho}
        };

        public static CoresDasPecas PegarCorPara(TiposDePecas index) {
            try {
                return _cores[index];
            } catch (KeyNotFoundException) {
                return CoresDasPecas.Transparente;
            }
        }
    }
}
