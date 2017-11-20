using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;

namespace foosballv2s.WebService.Models
{
    public class User : IdentityUser, IUser<string>
    {
        public DateTime JoinDate { get; set; }
    }
}