using System;

namespace foosballv2s.Droid.Shared.Source.Services.CredentialStorage.Models
{
    public class Credential
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}