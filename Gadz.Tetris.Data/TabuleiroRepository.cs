using Gadz.Tetris.Model;
using Gadz.Tetris.Model.Tabuleiros;
using System.Collections.Generic;
using System.IO;

namespace Gadz.Tetris.Data {
    public class TabuleiroRepository {

        IList<Estatisticas> estatisticas;

        public TabuleiroRepository() {
            estatisticas = new List<Estatisticas>();
        }

        public void Save(ITabuleiro tabuleiro) {
            using (var arquivo = new StreamWriter("data.mc", true)) {

                string[] campos = new string[6];

                campos[0] = Identidade.New();
                campos[1] = tabuleiro.Estatisticas.Pontos.ToString();
                campos[2] = tabuleiro.Estatisticas.Nivel.ToString();
                campos[3] = tabuleiro.Estatisticas.Linhas.ToString();
                campos[4] = tabuleiro.Estatisticas.Velocidade.ToString();
                campos[5] = tabuleiro.Estatisticas.Duracao.ToString(@"hh:mm:ss");

                arquivo.WriteLine(string.Join("\t", campos));
            }
        }

        public Estatisticas Load(Identidade id) {
            using (var arquivo = new StreamReader("data.mc")) {
                var campos = arquivo.ReadLine().Split('\t');
                if(campos[0] == id) {
                    return new Estatisticas();
                }
            }
            return null;
        }

        //void CarregarMemoryCard() {
        //    using (var arquivo = new StreamReader("data.mc")) {
        //        var campos = arquivo.ReadLine().Split('\t');
        //        var estatistica = new Estatisticas();
        //        estatistica.Pontos = int.Parse(campos[1]);

        //        estatisticas.Add(estatistica);
        //    }
        //}
    }
}
