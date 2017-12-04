using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Emgu.CV.Structure;
using foosballv2s.Droid.Shared.Source.Entities;
using foosballv2s.Droid.Shared.Source.Events;
using foosballv2s.Droid.Shared.Source.Services.GameLogger;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(Game))]
namespace foosballv2s.Droid.Shared.Source.Entities
{
    public class Game
    {
        public event EventHandler<GameEventArgs> OnStart; 
        public event EventHandler<GameEventArgs> OnGoal; 
        public event EventHandler<GameEventArgs> OnFinish; 
        
        public const int MAX_SCORE = 7;

        public int Id { set; get; }
        private int team1Score = 0;
        private int team2Score = 0;

        [JsonIgnore]
        public Hsv BallColor { get; set; }
        
        public Team Team1 { get; set; } = new Team();
        
        public Team Team2 { get; set; } = new Team();
        
        public int Team1Score
        {
            get { return team1Score; }
            set
            {
                team1Score = value;
            }
        }

        public int Team2Score
        {
            get { return team2Score; }
            set
            {
                team2Score = value;
            }
        }

        public DateTime? StartTime { get; set; } = null;
        public DateTime? EndTime { get; set; } = null;

        [JsonIgnore]
        public Team WinningTeam { get; set; }
        
        [JsonIgnore]
        public Boolean HasEnded { get; private set; } = false;
        
        public ICollection<GameEvent> GameEvents { get; set; } = new Collection<GameEvent>();
        
        public User User { get; set; }

        /// <summary>
        /// Stars the game timer
        /// </summary>
        public void Start(IGameLogger logger)
        {
            HasEnded = false;
            Team1Score = 0;
            Team2Score = 0;
            StartTime = DateTime.Now;
            EndTime = null;
            WinningTeam = null;
            GameEvents.Clear();

            SetupLogger(logger);
            
            if (OnStart != null)
            {
                OnStart(this, new GameEventArgs(this, null));
            }
        }

        public void AddTeam1Goal()
        {
            if (HasEnded)
            {
                return;
            }
            if (OnGoal != null)
            {
                OnGoal(this, new GameEventArgs(this, Team1));
            }
            Team1Score++;
            CheckGameEnd();
        }
        
        public void AddTeam2Goal()
        {
            if (HasEnded)
            {
                return;
            }
            if (OnGoal != null)
            {
                OnGoal(this, new GameEventArgs(this, Team2));
            }
            Team2Score++;
            CheckGameEnd();
        }
        
        /// <summary>
        /// Checks if the end of the game is reached
        /// </summary>
        private void CheckGameEnd()
        {
            if (Team1Score == MAX_SCORE || Team2Score == MAX_SCORE)
            {
                End();
            }
        }
        
        /// <summary>
        /// Ends the game timer and saves the winning team
        /// </summary>
        private void End()
        {
            EndTime = DateTime.Now;
            HasEnded = true;

            if (Team1Score == MAX_SCORE)
            {
                WinningTeam = Team1;
            }
            else if (Team2Score == MAX_SCORE)
            {
                WinningTeam = Team2;
            }
            if (OnFinish != null)
            {
                OnFinish(this, new GameEventArgs(this, WinningTeam));
            }
        }

        private void SetupLogger(IGameLogger logger)
        {
            OnStart += logger.LogStart;
            OnGoal += logger.LogGoal;
            OnFinish += logger.LogEnd;
        }
    }
}
