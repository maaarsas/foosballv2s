﻿using System;

namespace foosballv2s.Source.Services.FoosballWebService.Models
{
    public class LoginResponse
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}