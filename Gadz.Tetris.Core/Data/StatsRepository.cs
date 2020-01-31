using Gadz.Tetris.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gadz.Tetris.Data
{
    internal class StatsRepository : IStatsRepository
    {
        #region fields

        private static string StatsFilePath
        {
            get
            {
                var dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Gadesi\Tetris\stats.mc";
                return dir;
            }
        }

        private const char DELIMITADOR = '\t';
        private const int COL_ID = 0;
        private const int COL_POINTS = 1;
        private const int COL_LEVEL = 2;
        private const int COL_LINES = 3;
        private const int COL_SPEED = 4;
        private const int COL_MOVES = 5;
        private const int COL_BLOCKS = 6;
        private const int COL_TIME = 7;
        private static IList<Stats> _cache;

        #endregion

        #region constructors

        static StatsRepository()
        {
            _cache = LoadCache();
        }

        #endregion

        #region public methods

        public async void Save(Stats stats)
        {
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
            });

            _cache.Add(stats);
        }

        public Stats Load(Identity id)
        {
            return All().First(x => x.Id.Equals(id));
        }

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

        public IEnumerable<Stats> All()
        {
            return _cache;
        }

        #endregion

        #region private methods

        private static List<Stats> LoadCache()
        {
            var results = new List<Stats>();

            if (!File.Exists(StatsFilePath))
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

        #endregion
    }
}