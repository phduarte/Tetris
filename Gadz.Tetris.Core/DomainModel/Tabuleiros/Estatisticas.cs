using System;
using System.Linq;
using System.Collections.Generic;

namespace Gadz.Tetris.Core.DomainModel.Tabuleiros {
    public class Estatisticas {

        #region fields

        const int INTERVALO_ENTRE_VELOCIDADES = 50;

        IDictionary<DateTime, long> _historicoDuracao;
        DateTime? _startTime;
        int stageLines = 0;
        int goal = 20;

        #endregion

        #region properties

        public Identidade Id { get; private set; }
        public int Linhas { get; private set; }
        public int Pontos { get; private set; }
        public int Nivel { get; private set; }
        public int Velocidade { get; private set; }
        public int Movimentos { get; private set; }
        public int Blocos { get; private set; }
        public TimeSpan Duracao => calcularTempo();

        private TimeSpan calcularTempo() {

            long duracao = 0;

            duracao += _historicoDuracao.Sum(_ => _.Value);
            if (_startTime.HasValue)
                duracao += (DateTime.Now - _startTime.Value).Ticks;

            return new TimeSpan(duracao);
        }

        #endregion

        #region constructors

        public Estatisticas() {
            Id = Identidade.New();
            Nivel = 1;
            Velocidade = 1000;
            _historicoDuracao = new Dictionary<DateTime, long>();
        }

        public Estatisticas(int pontos, int linhas, int nivel, int velocidade, int movimentos, int blocos, long duracao) : this() {
            Pontos = pontos;
            Linhas = linhas;
            Nivel = nivel;
            Velocidade = velocidade;
            Movimentos = movimentos;
            Blocos = blocos;
            _historicoDuracao.Add(DateTime.Now, duracao);
        }

        #endregion

        #region public methods

        public void Pontuar(int linhas) {
            Linhas += linhas;
            Pontos += CalcularPontos(linhas);
            stageLines += linhas;
            ValidarNivel();
        }

        public void ContarMovimento() {
            Movimentos++;
        }

        public void Iniciar() {
            _startTime = DateTime.Now;
        }

        public void Pausar() {
            long duracaoEmMilisegundos = DateTime.Now.Ticks - _startTime.Value.Ticks;
            _historicoDuracao.Add(new KeyValuePair<DateTime, long>(_startTime.Value, duracaoEmMilisegundos));
            _startTime = null;
        }

        public void Continuar() {
            _startTime = DateTime.Now;
        }

        public void Terminar() {

            if (_startTime.HasValue) {
                long duracaoEmMilisegundos = DateTime.Now.Ticks - _startTime.Value.Ticks;
                _historicoDuracao.Add(new KeyValuePair<DateTime, long>(_startTime.Value, duracaoEmMilisegundos));
                _startTime = null;
            }
        }

        public void IncrementarQuantidadeDeBlocos() {
            Blocos++;
        }

        #endregion

        #region private methods

        void ValidarNivel() {
            if (stageLines >= goal) {
                AumentarNivel();
                stageLines = 0;
                goal += (int)(goal*0.25);
            }
        }

        void AumentarNivel() {
            Nivel++;
            Velocidade -= INTERVALO_ENTRE_VELOCIDADES;
        }

        int CalcularPontos(int linhas) {
            switch (linhas) {
                case 1:
                    return 40;
                case 2:
                    return 100;
                case 3:
                    return 300;
                case 4:
                    return 1200;
            }

            throw new IndexOutOfRangeException();
        }

        #endregion
    }
}