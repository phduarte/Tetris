using System.Collections.Generic;

namespace Gadz.Tetris.Model {
    public interface IStatsRepository {
        void Save(Stats stats);
        Stats Load(Identity id);
        int MaxScore();
        IEnumerable<Stats> All();
    }
}
