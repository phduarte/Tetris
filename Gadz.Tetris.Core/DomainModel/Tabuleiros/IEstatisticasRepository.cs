using System.Collections.Generic;

namespace Gadz.Tetris.Core.DomainModel.Tabuleiros {
    public interface IEstatisticasRepository {
        void Save(Estatisticas tabuleiro);
        Estatisticas Load(Identidade id);
        int MaxScore();
        IEnumerable<Estatisticas> All();
    }
}
