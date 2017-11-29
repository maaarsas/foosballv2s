using System;
using System.Collections.Generic;
using Emgu.CV.Structure;
using foosballv2s.Source.Activities.Events;
using foosballv2s.Source.Entities;
using foosballv2s.Source.Services.GameLogger;
using Xamarin.Forms;

[assembly: Dependency(typeof(Game))]
namespace foosballv2s.Source.Entities
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

        public Hsv BallColor { get; set; }
        
        public Team Team1 { get; set; } = new Team();
        
        public Team Team2 { get; set; } = new Team();
        
        public int Team1Score
        {
            get { return team1Score; }
            private set
            {
                if (HasEnded)
                {
                    return;
                }
                team1Score = value;
                CheckGameEnd();
            }
        }

        public int Team2Score
        {
            get { return team2Score; }
            private set
            {
                if (HasEnded)
                {
                    return;
                }
                team2Score = value;
                CheckGameEnd();
            }
        }

        public DateTime? StartTime { get; set; } = null;
        public DateTime? EndTime { get; set; } = null;

        public Team WinningTeam { get; set; }
        public Boolean HasEnded { get; private set; } = false;
        
        public IList<GameEvent> GameEvents { get; set; } = new List<GameEvent>();

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
            Team1Score++;
            if (OnGoal != null)
            {
                OnGoal(this, new GameEventArgs(this, Team1));
            }
        }
        
        public void AddTeam2Goal()
        {
            Team2Score++;
            if (OnGoal != null)
            {
                OnGoal(this, new GameEventArgs(this, Team2));
            }
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
        public void End()
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
