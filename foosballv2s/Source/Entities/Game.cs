using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Android.App;
using Emgu.CV.Fuzzy;
using Emgu.CV.Structure;
using Javax.Security.Auth;
using Xamarin.Forms;

[assembly: Dependency(typeof(foosballv2s.Game))]
namespace foosballv2s
{
    public class Game
    {
        public const int MAX_SCORE = 7;

        public int id;
        
        private int team1Score = 0;
        private int team2Score = 0;

        public Hsv BallColor { get; set; }

        public int Team1Id { get; set; }
        
        public Team Team1 { get; set; } = new Team();
        
        public int Team2Id { get; set; }
        
        public Team Team2 { get; set; } = new Team();
        
        public int Team1Score
        {
            get { return team1Score; }
            set
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
            set
            {
                if (HasEnded)
                {
                    return;
                }
                team2Score = value;
                CheckGameEnd();
            }
        }
        
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Team WinningTeam { get; set; }
        public Boolean HasEnded { get; private set; } = false;

        public void Start()
        {
            StartTime = DateTime.Now;
        }

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
        }
        
        private void CheckGameEnd()
        {
            if (Team1Score == MAX_SCORE || Team2Score == MAX_SCORE)
            {
                End();
            }
        }
    }
}
