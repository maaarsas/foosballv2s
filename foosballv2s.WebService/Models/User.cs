using System;
using Microsoft.AspNetCore.Identity;

namespace foosballv2s.WebService.Models
{
    public class User : IdentityUser
    {
        public DateTime JoinDate { get; set; }
    }
}