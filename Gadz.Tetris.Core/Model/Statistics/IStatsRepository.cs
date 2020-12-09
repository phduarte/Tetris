using System.Collections.Generic;

namespace Gadz.Tetris.Model.Statistics
{
    /// <summary>
    /// Defines the <see cref="IStatsRepository" />
    /// </summary>
    public interface IStatsRepository : IRepository
    {
        /// <summary>
        /// The Save
        /// </summary>
        /// <param name="stats">The stats<see cref="Stats"/></param>
        void Save(Stats stats);

        /// <summary>
        /// The Load
        /// </summary>
        /// <param name="id">The id<see cref="Identity"/></param>
        /// <returns>The <see cref="Stats"/></returns>
        Stats Load(Identity id);

        /// <summary>
        /// The MaxScore
        /// </summary>
        /// <returns>The <see cref="int"/></returns>
        int MaxScore();

        /// <summary>
        /// The All
        /// </summary>
        /// <returns>The <see cref="IEnumerable{Stats}"/></returns>
        IEnumerable<Stats> All();
    }
}
