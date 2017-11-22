using System;
using foosballv2s.Source.Services.CredentialStorage.Models;

namespace foosballv2s.Source.Services.CredentialStorage
{
    public interface ICredentialStorage
    {
        void Save(string email, string token, DateTime expiration);
        Credential Read();
        void Remove();
        bool HasExpired();
    }
}