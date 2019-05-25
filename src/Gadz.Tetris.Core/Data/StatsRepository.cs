using Gadz.Tetris.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gadz.Tetris.Data
{
    internal class StatsRepository : IStatsRepository {

        #region fields

        const string FILE_NAME = "stats.mc";
        const char DELIMITADOR = '\t';
        const int COL_ID = 0;
        const int COL_POINTS = 1;
        const int COL_LEVEL = 2;
        const int COL_LINES = 3;
        const int COL_SPEED = 4;
        const int COL_MOVES = 5;
        const int COL_BLOCKS = 6;
        const int COL_TIME = 7;
        static IList<Stats> _cache = LoadCache().Result;

        #endregion

        #region constructors

        public StatsRepository() {
            _cache = new List<Stats>();
        }

        #endregion

        #region public methods

        public async void Save(Stats stats) {

            await Task.Factory.StartNew(() => {

                using (var file = new StreamWriter(FILE_NAME, true)) {

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

        public Stats Load(Identity id) {
            return All().First(x => x.Id.Equals(id));
        }

        public int MaxScore() {
            try {
                return All().Max(x => x.Score);
            } catch (InvalidOperationException) {
                return 0;
            }
        }

        public IEnumerable<Stats> All() {
            return _cache;
        }

        #endregion

        #region private methods

        static async Task<IList<Stats>> LoadCache() {

            var results = new List<Stats>();

            await Task.Factory.StartNew(() => {

                if (!File.Exists(FILE_NAME))
                    return;

                using (var file = new StreamReader(FILE_NAME)) {

                    while (!file.EndOfStream) {

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
            }).ConfigureAwait(false);

            return results;
        }

        #endregion
    }
}
