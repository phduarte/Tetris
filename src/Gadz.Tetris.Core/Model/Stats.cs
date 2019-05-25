using System;
using System.Linq;
using System.Collections.Generic;

namespace Gadz.Tetris.Model
{
    public class Stats : Entity
    {

        #region fields

        const int REFRESH_INTERVAL = 1;

        IDictionary<DateTime, long> _durationHistory;
        DateTime? _startTime;
        int stageLines = 0;
        int goal = 20;

        #endregion

        #region properties

        public int Lines { get; private set; }
        public int Score { get; private set; }
        public int Level { get; private set; }
        public int Speed { get; private set; }
        public int Moves { get; private set; }
        public int Blocks { get; private set; }
        public TimeSpan Duration => CalculateTime();

        #endregion

        #region constructors

        public Stats()
            : this(Identity.New())
        {
            Level = 1;
            Speed = 1;
        }

        public Stats(Identity id) 
            : base(id)
        {
            _durationHistory = new Dictionary<DateTime, long>();
        }

        public Stats(Identity id, int points, int lines, int level, int speed, int moves, int blocks, long duration) : this(id)
        {
            Score = points;
            Lines = lines;
            Level = level;
            Speed = speed;
            Moves = moves;
            Blocks = blocks;
            _durationHistory.Add(DateTime.Now, duration);
        }

        #endregion

        #region public methods

        public void Gain(int lines)
        {
            Lines += lines;
            Score += CalculateScore(lines);
            stageLines += lines;
            CheckLevel();
        }

        public void CountMovement()
        {
            Moves++;
        }

        public void Start()
        {
            _startTime = DateTime.Now;
        }

        public void Pause()
        {
            long milisecondsDuration = DateTime.Now.Ticks - _startTime.Value.Ticks;
            _durationHistory.Add(new KeyValuePair<DateTime, long>(_startTime.Value, milisecondsDuration));
            _startTime = null;
        }

        public void Continue()
        {
            _startTime = DateTime.Now;
        }

        public void Finish()
        {
            if(_startTime.HasValue)
            {
                long milisecondsDuration = DateTime.Now.Ticks - _startTime.Value.Ticks;
                _durationHistory.Add(new KeyValuePair<DateTime, long>(_startTime.Value, milisecondsDuration));
                _startTime = null;
            }
        }

        public void IncludeBlock()
        {
            Blocks++;
        }

        #endregion

        #region private methods

        TimeSpan CalculateTime()
        {

            long duracao = 0;

            duracao += _durationHistory.Sum(_ => _.Value);
            if(_startTime.HasValue)
                duracao += (DateTime.Now - _startTime.Value).Ticks;

            return new TimeSpan(duracao);
        }

        void CheckLevel()
        {
            if(stageLines >= goal)
            {
                LevelUp();
                stageLines = 0;
                goal += (int)(goal * 0.25);
            }
        }

        void LevelUp()
        {
            Level++;

            if(Speed == 1)
                Speed = REFRESH_INTERVAL;
            else
            {
                Speed += REFRESH_INTERVAL;
            }
        }

        int CalculateScore(int linhas)
        {
            switch(linhas)
            {
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