using Android;
using Android.App;
using Emgu.CV.Structure;

namespace foosballv2s
{
    public class App : Application
    {
        public static App Current { get; set; }
        
        internal Team Team1 { get; set; }
        internal Team Team2 { get; set; }
        internal Hsv BallColor { get; set; }
        
        public override void OnCreate()
        {
            base.OnCreate();
            
            Team1 = new Team();
            Team2 = new Team();
            BallColor = new Hsv();

            Current = this;
        }
    }
}
