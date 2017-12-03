using System;
using foosballv2s.Droid.Shared.Source.Services.CredentialStorage.Models;

namespace foosballv2s.Droid.Shared.Source.Services.CredentialStorage
{
    public interface ICredentialStorage
    {
        void Save(string email, string token, DateTime expiration);
        Credential Read();
        void Remove();
        bool HasExpired();
    }
}