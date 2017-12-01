namespace foosballv2s.WebService.Models
{
    public class Team
    {
        public int Id { get; set; }
        
        public string TeamName { get; set; }
        
        public User User { get; set; }
    }
}
