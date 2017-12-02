using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace foosballv2s.WebService.Models
{
    public class Team
    {
        public int Id { get; set; }
        
        public string TeamName { get; set; }
        
        [Required]
        public virtual IdentityUser User { get; set; }
    }
}
