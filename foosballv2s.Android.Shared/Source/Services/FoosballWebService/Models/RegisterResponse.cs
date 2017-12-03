using System.Collections.Generic;

namespace foosballv2s.Droid.Shared.Source.Services.FoosballWebService.Models
{
    public class RegisterResponse
    {
        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; }
    }
}