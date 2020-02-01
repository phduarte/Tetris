using Gadz.Tetris.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gadz.Tetris.Data
{
    /// <summary>
    /// Defines the <see cref="StatsRepository" />
    /// </summary>
    internal class StatsRepository : IStatsRepository
    {
        /// <summary>
        /// Gets the StatsFilePath
        /// </summary>
        private static string StatsFilePath
        {
            get
            {
                var dir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Tetris\stats.mc";
                return dir;
            }
        }

        /// <summary>
        /// Defines the DELIMITADOR
        /// </summary>
        private const char DELIMITADOR = '\t';

        /// <summary>
        /// Defines the COL_ID
        /// </summary>
        private const int COL_ID = 0;

        /// <summary>
        /// Defines the COL_POINTS
        /// </summary>
        private const int COL_POINTS = 1;

        /// <summary>
        /// Defines the COL_LEVEL
        /// </summary>
        private const int COL_LEVEL = 2;

        /// <summary>
        /// Defines the COL_LINES
        /// </summary>
        private const int COL_LINES = 3;

        /// <summary>
        /// Defines the COL_SPEED
        /// </summary>
        private const int COL_SPEED = 4;

        /// <summary>
        /// Defines the COL_MOVES
        /// </summary>
        private const int COL_MOVES = 5;

        /// <summary>
        /// Defines the COL_BLOCKS
        /// </summary>
        private const int COL_BLOCKS = 6;

        /// <summary>
        /// Defines the COL_TIME
        /// </summary>
        private const int COL_TIME = 7;

        /// <summary>
        /// Defines the _cache
        /// </summary>
        private static IList<Stats> _cache;

        /// <summary>
        /// Initializes static members of the <see cref="StatsRepository"/> class.
        /// </summary>
        static StatsRepository()
        {
            _cache = LoadCache();
        }

        /// <summary>
        /// The Save
        /// </summary>
        /// <param name="stats">The stats<see cref="Stats"/></param>
        public async void Save(Stats stats)
        {
            if (!File.Exists(StatsFilePath))
                return;

            await Task.Factory.StartNew(() =>
            {
                using (var file = new StreamWriter(StatsFilePath, true))
                {
                    var cols = new string[8];

                    cols[COL_ID] = stats.Id;
                    cols[COL_POINTS] = stats.Score.ToString();
                    cols[COL_LEVEL] = stats.Level.ToString();
                    cols[COL_LINES] = stats.Lines.ToString();
                    cols[COL_SPEED] = stats.Speed.ToString();
                    cols[COL_MOVES] = stats.Moves.ToString();
                    cols[COL_BLOCKS] = stats.Blocks.ToString();
                    cols[COL_TIME] = stats.Duration.Ticks.ToString();

                    file.WriteLine(string.Join(DELIMITADOR.ToString(), cols));
                }
            }).ConfigureAwait(false);

            _cache.Add(stats);
        }

        /// <summary>
        /// The Load
        /// </summary>
        /// <param name="id">The id<see cref="Identity"/></param>
        /// <returns>The <see cref="Stats"/></returns>
        public Stats Load(Identity id)
        {
            return All().First(x => x.Id.Equals(id));
        }

        /// <summary>
        /// The MaxScore
        /// </summary>
        /// <returns>The <see cref="int"/></returns>
        public int MaxScore()
        {
            try
            {
                return All().Max(x => x.Score);
            }
            catch (InvalidOperationException)
            {
                return 0;
            }
        }

        /// <summary>
        /// The All
        /// </summary>
        /// <returns>The <see cref="IEnumerable{Stats}"/></returns>
        public IEnumerable<Stats> All()
        {
            return _cache;
        }

        /// <summary>
        /// The LoadCache
        /// </summary>
        /// <returns>The <see cref="List{Stats}"/></returns>
        private static List<Stats> LoadCache()
        {
            var results = new List<Stats>();

            if (!StatsFileExists())
                return results;

            using (var file = new StreamReader(StatsFilePath))
            {
                while (!file.EndOfStream)
                {
                    var cols = file.ReadLine().Split(DELIMITADOR);

                    Identity id = cols[COL_ID];
                    int.TryParse(cols[COL_POINTS], out int score);
                    int.TryParse(cols[COL_LEVEL], out int level);
                    int.TryParse(cols[COL_LINES], out int lines);
                    int.TryParse(cols[COL_SPEED], out int speed);
                    int.TryParse(cols[COL_MOVES], out int moves);
                    int.TryParse(cols[COL_BLOCKS], out int blocks);
                    long.TryParse(cols[COL_TIME], out long duration);

                    results.Add(new Stats(id, score, lines, level, speed, moves, blocks, duration));
                }
            }

            return results;
        }

        /// <summary>
        /// The StatsFileExists
        /// </summary>
        /// <returns>The <see cref="bool"/></returns>
        private static bool StatsFileExists()
        {
            var fi = new FileInfo(StatsFilePath);

            if (!fi.Exists)
            {
                if (!fi.Directory.Exists)
                    fi.Directory.Create();

                fi.Create();
            }

            return fi.Exists;
        }
    }
}
