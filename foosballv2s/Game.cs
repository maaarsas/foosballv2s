using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Emgu.CV.Structure;
using Xamarin.Forms;

[assembly: Dependency(typeof(foosballv2s.Game))]
namespace foosballv2s
{
    public class Game
    {
        private Team team1;
        private Team team2;

        public Hsv BallColor { get; set; }
    }
}
