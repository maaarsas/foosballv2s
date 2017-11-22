using System;
using System.Collections.Generic;

namespace foosballv2s.Source.Services.FoosballWebService.Models
{
    public class RegisterResponse
    {
        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; }
    }
}