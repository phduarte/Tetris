using System;
using System.Collections.Generic;
using System.Linq;

namespace Gadz.Tetris.Model.Statistics
{
    /// <summary>
    /// Defines the <see cref="Stats" />
    /// </summary>
    public class Stats : Entity
    {
        /// <summary>
        /// Defines the REFRESH_INTERVAL
        /// </summary>
        private const int REFRESH_INTERVAL = 1;

        /// <summary>
        /// Defines the _durationHistory
        /// </summary>
        private IDictionary<DateTime, long> _durationHistory;

        /// <summary>
        /// Defines the _startTime
        /// </summary>
        private DateTime? _startTime;

        /// <summary>
        /// Defines the stageLines
        /// </summary>
        private int stageLines = 0;

        /// <summary>
        /// Defines the goal
        /// </summary>
        private int goal = 20;

        /// <summary>
        /// Gets the Lines
        /// </summary>
        public int Lines { get; private set; }

        /// <summary>
        /// Gets the Score
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// Gets the Level
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// Gets the Speed
        /// </summary>
        public int Speed { get; private set; }

        /// <summary>
        /// Gets the Moves
        /// </summary>
        public int Moves { get; private set; }

        /// <summary>
        /// Gets the Blocks
        /// </summary>
        public int Blocks { get; private set; }

        /// <summary>
        /// Gets the Duration
        /// </summary>
        public TimeSpan Duration => CalculateTime();

        /// <summary>
        /// 
        /// </summary>
        public int Tetris { get; private set; }

        public float TetrisRate => Score > 0 ? (float)Tetris / (float)Score : 0F;

        /// <summary>
        /// Initializes a new instance of the <see cref="Stats"/> class.
        /// </summary>
        public Stats()
            : this(Identity.New())
        {
            Level = 1;
            Speed = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stats"/> class.
        /// </summary>
        /// <param name="id">The id<see cref="Identity"/></param>
        public Stats(Identity id)
            : base(id)
        {
            _durationHistory = new Dictionary<DateTime, long>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stats"/> class.
        /// </summary>
        /// <param name="id">The id<see cref="Identity"/></param>
        /// <param name="points">The points<see cref="int"/></param>
        /// <param name="lines">The lines<see cref="int"/></param>
        /// <param name="level">The level<see cref="int"/></param>
        /// <param name="speed">The speed<see cref="int"/></param>
        /// <param name="moves">The moves<see cref="int"/></param>
        /// <param name="blocks">The blocks<see cref="int"/></param>
        /// <param name="duration">The duration<see cref="long"/></param>
        private Stats(StatsRecord record)
            : this(record.Id)
        {
            Score = record.Score;
            Lines = record.Lines;
            Level = record.Level;
            Speed = record.Speed;
            Moves = record.Moves;
            Blocks = record.Blocks;
            _durationHistory.Add(DateTime.Now, record.Seconds);
        }

        /// <summary>
        /// The Gain
        /// </summary>
        /// <param name="lines">The lines<see cref="int"/></param>
        public void Gain(int lines)
        {
            Lines += lines;
            Score += CalculateScore(lines);
            stageLines += lines;
            CheckLevel();
        }

        /// <summary>
        /// The CountMovement
        /// </summary>
        public void CountMovement()
        {
            Moves++;
        }

        /// <summary>
        /// The Start
        /// </summary>
        public void Start()
        {
            _startTime = DateTime.Now;
        }

        /// <summary>
        /// The Pause
        /// </summary>
        public void Pause()
        {
            long milisecondsDuration = DateTime.Now.Ticks - _startTime.Value.Ticks;
            _durationHistory.Add(new KeyValuePair<DateTime, long>(_startTime.Value, milisecondsDuration));
            _startTime = null;
        }

        /// <summary>
        /// The Continue
        /// </summary>
        public void Continue()
        {
            _startTime = DateTime.Now;
        }

        /// <summary>
        /// The Finish
        /// </summary>
        public void Finish()
        {
            if (_startTime.HasValue)
            {
                long milisecondsDuration = DateTime.Now.Ticks - _startTime.Value.Ticks;
                _durationHistory.Add(new KeyValuePair<DateTime, long>(_startTime.Value, milisecondsDuration));
                _startTime = null;
            }
        }

        /// <summary>
        /// The IncludeBlock
        /// </summary>
        public void IncludeBlock()
        {
            Blocks++;
        }

        /// <summary>
        /// The CalculateTime
        /// </summary>
        /// <returns>The <see cref="TimeSpan"/></returns>
        private TimeSpan CalculateTime()
        {
            long duracao = 0;

            duracao += _durationHistory.Sum(_ => _.Value);
            if (_startTime.HasValue)
                duracao += (DateTime.Now - _startTime.Value).Ticks;

            return new TimeSpan(duracao);
        }

        /// <summary>
        /// The CheckLevel
        /// </summary>
        private void CheckLevel()
        {
            if (stageLines >= goal)
            {
                LevelUp();
                stageLines = 0;
                goal += (int)(goal * 0.25);
            }
        }

        /// <summary>
        /// The LevelUp
        /// </summary>
        private void LevelUp()
        {
            Level++;

            if (Speed == 1)
                Speed = REFRESH_INTERVAL;
            else
            {
                Speed += REFRESH_INTERVAL;
            }
        }

        /// <summary>
        /// The CalculateScore
        /// </summary>
        /// <param name="linhas">The linhas<see cref="int"/></param>
        /// <returns>The <see cref="int"/></returns>
        private int CalculateScore(int linhas)
        {
            switch (linhas)
            {
                case 1:
                    return 40;

                case 2:
                    return 100;

                case 3:
                    return 300;

                case 4:
                    Tetris += 1200;
                    return 1200;
                default:
                    return 0;
            }

            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Carrega uma estatística baseada num registro de estatística salvo anteriormente.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static Stats Load(StatsRecord record)
        {
            return new Stats(record);
        }
    }
}
