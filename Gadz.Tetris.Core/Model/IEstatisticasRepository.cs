using System.Collections.Generic;

namespace Gadz.Tetris.Model {
    public interface IEstatisticasRepository {
        void Save(Estatisticas tabuleiro);
        Estatisticas Load(Identidade id);
        int MaxScore();
        IEnumerable<Estatisticas> All();
    }
}
