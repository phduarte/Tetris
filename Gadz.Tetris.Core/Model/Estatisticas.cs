using System;
using System.Linq;
using System.Collections.Generic;

namespace Gadz.Tetris.Model {
    public class Estatisticas : Entidade {

        #region fields

        const int INTERVALO_ENTRE_VELOCIDADES = 1;

        IDictionary<DateTime, long> _historicoDuracao;
        DateTime? _startTime;
        int stageLines = 0;
        int goal = 20;

        #endregion

        #region properties

        public int Linhas { get; private set; }
        public int Pontos { get; private set; }
        public int Nivel { get; private set; }
        public int Velocidade { get; private set; }
        public int Movimentos { get; private set; }
        public int Blocos { get; private set; }
        public TimeSpan Duracao => calcularTempo();

        #endregion

        #region constructors

        public Estatisticas() : this(Identidade.New()) {
            Nivel = 1;
            Velocidade = 1;
        }

        public Estatisticas(Identidade id) : base(id) {
            _historicoDuracao = new Dictionary<DateTime, long>();
        }

        public Estatisticas(Identidade id, int pontos, int linhas, int nivel, int velocidade, int movimentos, int blocos, long duracao) : this(id) {
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

        TimeSpan calcularTempo() {

            long duracao = 0;

            duracao += _historicoDuracao.Sum(_ => _.Value);
            if (_startTime.HasValue)
                duracao += (DateTime.Now - _startTime.Value).Ticks;

            return new TimeSpan(duracao);
        }

        void ValidarNivel() {
            if (stageLines >= goal) {
                AumentarNivel();
                stageLines = 0;
                goal += (int)(goal*0.25);
            }
        }

        void AumentarNivel() {
            Nivel++;

            if (Velocidade == 1)
                Velocidade = INTERVALO_ENTRE_VELOCIDADES;
            else {
                Velocidade += INTERVALO_ENTRE_VELOCIDADES;
            }
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