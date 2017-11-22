using System;
using Android.Content;
using foosballv2s.Source.Services.CredentialStorage.Models;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(foosballv2s.Source.Services.CredentialStorage.CredentialStorage))]
namespace foosballv2s.Source.Services.CredentialStorage
{
    public class CredentialStorage : ICredentialStorage
    {
        private const string PREFERENCES_NAME = "webservice_api_credentials";
        private const string USER_TOKEN_SETTING_NAME = "user_token";
        private const string USER_TOKEN_EXPIRATION_SETTING_NAME = "user_token_expiration";

        public void Save(string token, DateTime expiration)
        {
            ISharedPreferences settings = Application.Context.GetSharedPreferences(PREFERENCES_NAME, FileCreationMode.Private);
            ISharedPreferencesEditor editor = settings.Edit();
            editor.PutString(USER_TOKEN_SETTING_NAME, token);
            editor.PutLong(USER_TOKEN_EXPIRATION_SETTING_NAME, expiration.Ticks);
        }

        public Credential Read()
        {
            Credential credential = new Credential();
            ISharedPreferences pref = Application.Context.GetSharedPreferences("PREFERENCE_NAME", FileCreationMode.Private);
            credential.Token = pref.GetString(USER_TOKEN_SETTING_NAME, null);
            credential.Expiration = new DateTime(pref.GetLong(USER_TOKEN_EXPIRATION_SETTING_NAME, 0));
            return credential;
        }
        
       
    }
}