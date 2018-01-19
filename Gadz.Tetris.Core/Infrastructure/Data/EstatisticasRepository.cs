using Gadz.Tetris.Core.DomainModel;
using Gadz.Tetris.Core.DomainModel.Tabuleiros;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gadz.Tetris.Core.Infrastructure.Data {
    internal class EstatisticasRepository : IEstatisticasRepository {

        private const string FILE_NAME = "stats.mc";
        private const char DELIMITADOR = '\t';

        static IList<Estatisticas> _cache = LoadCache().Result;

        public EstatisticasRepository() {
            _cache = new List<Estatisticas>();
        }

        public async void Save(Estatisticas stats) {

            await Task.Factory.StartNew(() => {

                using (var arquivo = new StreamWriter(FILE_NAME, true)) {

                    string[] campos = new string[8];

                    campos[0] = stats.Id;
                    campos[1] = stats.Pontos.ToString();
                    campos[2] = stats.Nivel.ToString();
                    campos[3] = stats.Linhas.ToString();
                    campos[4] = stats.Velocidade.ToString();
                    campos[5] = stats.Movimentos.ToString();
                    campos[6] = stats.Blocos.ToString();
                    campos[7] = stats.Duracao.Ticks.ToString();

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

        static async Task<IList<Estatisticas>> LoadCache() {

            var _resultado = new List<Estatisticas>();

            await Task.Factory.StartNew(() => {
                using (var arquivo = new StreamReader(FILE_NAME)) {

                    while (!arquivo.EndOfStream) {

                        var campos = arquivo.ReadLine().Split(DELIMITADOR);

                        int.TryParse(campos[1], out int pontos);
                        int.TryParse(campos[2], out int nivel);
                        int.TryParse(campos[3], out int linhas);
                        int.TryParse(campos[4], out int velocidade);
                        int.TryParse(campos[5], out int movimentos);
                        int.TryParse(campos[6], out int blocos);
                        long.TryParse(campos[7], out long duracao);

                        _resultado.Add(new Estatisticas(pontos, linhas, nivel, velocidade, movimentos, blocos, duracao));
                    }
                }
            }).ConfigureAwait(false);

            return _resultado;
        }
    }
}
