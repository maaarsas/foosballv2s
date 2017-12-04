using System;
using foosballv2s.Droid.Shared.Source.Entities;
using foosballv2s.Droid.Shared.Source.Helpers;
using foosballv2s.Droid.Shared.Source.Services.CredentialStorage;
using foosballv2s.Droid.Shared.Source.Services.CredentialStorage.Models;
using NUnit.Framework;

namespace foosballv2s.Test.Source.Services.CredentialStorage
{
    [TestFixture]
    public class TestCredentialStorage
    {
        [Test, TestCaseSource("Credentials")]
        public void TestSave_CheckIfSaved(
            string id, string email, string token, DateTime expTime, bool hasExpired)
        {
            var storage = GetStorage();
            storage.Save(id, email, token, expTime);
            Credential credential = storage.Read();
            
            Assert.AreEqual(id, credential.Id);
            Assert.AreEqual(email, credential.Email);
            Assert.AreEqual(token, credential.Token);
            Assert.AreEqual(expTime, credential.Expiration);
        }

        [Test, TestCaseSource("Credentials")]
        public void TestDelete(
            string id, string email, string token, DateTime expTime, bool hasExpired)
        {
            var storage = GetStorage();
            storage.Save(id, email, token, expTime);
            storage.Remove();
            Credential credential = storage.Read();
            
            Assert.Null(credential.Id);
            Assert.Null(credential.Email);
            Assert.Null(credential.Token);
            Assert.AreEqual(DateTime.MinValue, credential.Expiration);
        }
        
        [Test, TestCaseSource("Credentials")]
        public void TestHasExpired(
            string id, string email, string token, DateTime expTime, bool hasExpired)
        {
            var storage = GetStorage();
            storage.Save(id, email, token, expTime);
            bool expiredCheck = storage.HasExpired();
            
            Assert.AreEqual(hasExpired, expiredCheck);
        }
        
        [Test, TestCaseSource("Credentials")]
        public void TestGetCurrentUser(
            string id, string email, string token, DateTime expTime, bool hasExpired)
        {
            var storage = GetStorage();
            storage.Save(id, email, token, expTime);
            User user = storage.GetCurrentUser();
            
            Assert.AreEqual(id, user.Id);
            Assert.AreEqual(email, user.Email);
        }

        private ICredentialStorage GetStorage()
        {
            return new Droid.Shared.Source.Services.CredentialStorage.CredentialStorage();
        }

        private static object[] Credentials =
        {
            new object[] {"2", "lalala@example.com", "faadga6346wegwgweweg253", DateTime.Now.AddSeconds(300), false},
            new object[] {"5", "t@example.com", "faadgaagagag6346253", DateTime.Now, true},
            new object[]
            {
                "3040000", "adadawfaegagaegeg@example.com", "dasfaf3225255gweweg253", DateTime.Now.AddSeconds(-50), true
            },
        };

    }
}