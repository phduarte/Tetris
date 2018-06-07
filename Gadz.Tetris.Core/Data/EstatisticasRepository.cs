using Gadz.Tetris.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gadz.Tetris.Data {
    internal class EstatisticasRepository : IEstatisticasRepository {

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
        static IList<Estatisticas> _cache = LoadCache().Result;

        #endregion

        #region constructors

        public EstatisticasRepository() {
            _cache = new List<Estatisticas>();
        }

        #endregion

        #region public methods

        public async void Save(Estatisticas stats) {

            await Task.Factory.StartNew(() => {

                using (var arquivo = new StreamWriter(FILE_NAME, true)) {

                    string[] campos = new string[8];

                    campos[COL_ID] = stats.Id;
                    campos[COL_POINTS] = stats.Pontos.ToString();
                    campos[COL_LEVEL] = stats.Nivel.ToString();
                    campos[COL_LINES] = stats.Linhas.ToString();
                    campos[COL_SPEED] = stats.Velocidade.ToString();
                    campos[COL_MOVES] = stats.Movimentos.ToString();
                    campos[COL_BLOCKS] = stats.Blocos.ToString();
                    campos[COL_TIME] = stats.Duracao.Ticks.ToString();

                    arquivo.WriteLine(string.Join(DELIMITADOR.ToString(), campos));
                }

            });

            _cache.Add(stats);
        }

        public Estatisticas Load(Identidade id) {
            return All().First(x => x.Id.Equals(id));
        }

        public int MaxScore() {
            try {
                return All().Max(x => x.Pontos);
            } catch (InvalidOperationException) {
                return 0;
            }
        }

        public IEnumerable<Estatisticas> All() {
            return _cache;
        }

        #endregion

        #region private methods

        static async Task<IList<Estatisticas>> LoadCache() {

            var _resultado = new List<Estatisticas>();

            await Task.Factory.StartNew(() => {

                if (!File.Exists(FILE_NAME))
                    return;

                using (var arquivo = new StreamReader(FILE_NAME)) {

                    while (!arquivo.EndOfStream) {

                        var campos = arquivo.ReadLine().Split(DELIMITADOR);

                        Identidade id = campos[COL_ID];
                        int.TryParse(campos[COL_POINTS], out int pontos);
                        int.TryParse(campos[COL_LEVEL], out int nivel);
                        int.TryParse(campos[COL_LINES], out int linhas);
                        int.TryParse(campos[COL_SPEED], out int velocidade);
                        int.TryParse(campos[COL_MOVES], out int movimentos);
                        int.TryParse(campos[COL_BLOCKS], out int blocos);
                        long.TryParse(campos[COL_TIME], out long duracao);

                        _resultado.Add(new Estatisticas(id, pontos, linhas, nivel, velocidade, movimentos, blocos, duracao));
                    }
                }
            }).ConfigureAwait(false);

            return _resultado;
        }

        #endregion
    }
}
