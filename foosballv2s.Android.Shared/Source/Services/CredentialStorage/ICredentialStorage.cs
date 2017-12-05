using System;
using foosballv2s.Droid.Shared.Source.Entities;
using foosballv2s.Droid.Shared.Source.Services.CredentialStorage.Models;

namespace foosballv2s.Droid.Shared.Source.Services.CredentialStorage
{
    public interface ICredentialStorage
    {
        void Save(string id, string email, string token, DateTime expiration);
        Credential Read();
        void Remove();
        bool HasExpired();
        User GetCurrentUser();
        void SaveLanguage(string language);
        string GetSavedLanguage();
    }
}