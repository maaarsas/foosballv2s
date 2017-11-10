using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foosballv2s
{
    [Table("teams")]
    public class Team
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id;
        
        [Column("name")]
        public string TeamName { get; set; }
        
        [NotMapped]
        public int TotalScore { get; set; }
    }
}
