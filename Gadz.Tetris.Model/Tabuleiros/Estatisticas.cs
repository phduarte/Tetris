using System;
using System.Linq;
using System.Collections.Generic;

namespace Gadz.Tetris.Model.Tabuleiros {
    public class Estatisticas {

        #region fields

        const int INTERVALO_ENTRE_VELOCIDADES = 50;

        IDictionary<DateTime, long> _historicoDuracao;
        DateTime? _startTime;
        int stagePoints = 0;

        #endregion

        #region properties

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
            Nivel = 1;
            Velocidade = 1000;
            _historicoDuracao = new Dictionary<DateTime, long>();
        }

        #endregion

        #region public methods

        public void Pontuar(int linhas) {
            Linhas += linhas;
            Pontos += CalcularPontos(linhas);
            stagePoints += Pontos;
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

        #endregion

        #region private methods

        void ValidarNivel() {
            if (stagePoints >= 2000) {
                AumentarNivel();
                stagePoints = 0;
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

        internal void IncrementarQuantidadeDeBlocos() {
            Blocos++;
        }

        #endregion
    }
}