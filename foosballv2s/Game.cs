using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations.Schema;
using Emgu.CV.Fuzzy;
using Emgu.CV.Structure;
using Javax.Security.Auth;
using Xamarin.Forms;

[assembly: Dependency(typeof(foosballv2s.Game))]
namespace foosballv2s
{
    [Table("games")]
    public class Game
    {
        public const int MAX_SCORE = 7;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id;
        
        private int team1Score = 0;
        private int team2Score = 0;

        [NotMapped]
        public Hsv BallColor { get; set; }

        [ForeignKey("Team1")] 
        public int Team1Id { get; set; }
        
        public Team Team1 { get; set; } = new Team();
        
        [ForeignKey("Team2")] 
        public int Team2Id { get; set; }
        
        public Team Team2 { get; set; } = new Team();
        
        [Column("team1_score")]
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

        [Column("team2_score")]
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

        [NotMapped]
        public Boolean HasEnded { get; private set; } = false;

        private void CheckGameEnd()
        {
            if (Team1Score == MAX_SCORE || Team2Score == MAX_SCORE)
            {
                HasEnded = true;
            }
        }
    }
}
