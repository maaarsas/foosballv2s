using System;
using Android.Content;
using foosballv2s.Droid.Shared.Source.Entities;
using foosballv2s.Droid.Shared.Source.Services.CredentialStorage;
using foosballv2s.Droid.Shared.Source.Services.CredentialStorage.Models;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(CredentialStorage))]
namespace foosballv2s.Droid.Shared.Source.Services.CredentialStorage
{
    public class CredentialStorage : ICredentialStorage
    {
        private const string PREFERENCES_NAME = "webservice_api_credentials";
        private const string USER_ID_SETTING_NAME = "user_id";
        private const string USER_EMAIL_SETTING_NAME = "user_email";
        private const string USER_TOKEN_SETTING_NAME = "user_token";
        private const string USER_TOKEN_EXPIRATION_SETTING_NAME = "user_token_expiration";
        private const string USER_LANGUAGE_SETTING_NAME = "user_language";

        private ISharedPreferences _settings;

        public CredentialStorage()
        {
            _settings = Application.Context.GetSharedPreferences(PREFERENCES_NAME, FileCreationMode.Private);
        }

        public void Save(string id, string email, string token, DateTime expiration)
        {
            ISharedPreferencesEditor editor = _settings.Edit();
            editor.PutString(USER_ID_SETTING_NAME, id);
            editor.PutString(USER_EMAIL_SETTING_NAME, email);
            editor.PutString(USER_TOKEN_SETTING_NAME, token);
            editor.PutLong(USER_TOKEN_EXPIRATION_SETTING_NAME, expiration.Ticks);
            editor.Commit();
        }

        public Credential Read()
        {
            Credential credential = new Credential();
            credential.Id = _settings.GetString(USER_ID_SETTING_NAME, null);
            credential.Email = _settings.GetString(USER_EMAIL_SETTING_NAME, null);
            credential.Token = _settings.GetString(USER_TOKEN_SETTING_NAME, null);
            credential.Expiration = new DateTime(_settings.GetLong(USER_TOKEN_EXPIRATION_SETTING_NAME, 0));
            return credential;
        }

        public void Remove()
        {
            ISharedPreferencesEditor editor = _settings.Edit();
            editor.Remove(USER_ID_SETTING_NAME);
            editor.Remove(USER_EMAIL_SETTING_NAME);
            editor.Remove(USER_TOKEN_SETTING_NAME);
            editor.Remove(USER_TOKEN_EXPIRATION_SETTING_NAME);
            editor.Commit();
        }

        public bool HasExpired()
        {
            Credential credential = Read();
            // expiration time reached
            if (credential.Token == null || credential.Expiration < DateTime.Now)
            {
                return true;
            }
            // or something is missing, relogin then also
            if (credential.Id == null || credential.Email == null || credential.Token == null)
            {
                return true;
            }
            return false;
        }

        public User GetCurrentUser()
        {
            Credential credential = Read();
            User user = new User();
            user.Id = credential.Id;
            user.Email = credential.Email;
            return user;
        }

        public void SaveLanguage(string language)
        {
            ISharedPreferencesEditor editor = _settings.Edit();
            editor.Remove(USER_LANGUAGE_SETTING_NAME);
            editor.PutString(USER_LANGUAGE_SETTING_NAME, language);
            editor.Commit();
        }

        public string GetSavedLanguage()
        {
            return _settings.GetString(USER_LANGUAGE_SETTING_NAME, null);
        }
    }
}