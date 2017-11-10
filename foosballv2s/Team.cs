﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foosballv2s
{
    public class Team
    {
        public int id;
        
        public string TeamName { get; set; }
        
        public int TotalScore { get; set; }
    }
}
