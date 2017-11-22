using System;
using Android.App;
using Android.Runtime;
using foosballv2s.Source.Entities;
using foosballv2s.Source.Services.CredentialStorage;
using foosballv2s.Source.Services.FoosballWebService;
using foosballv2s.Source.Services.FoosballWebService.Repository;
using Xamarin.Forms;
using Application = Android.App.Application;

namespace foosballv2s.Source
{
    /// <summary>
    /// Application class extension
    /// </summary>
    [Application]
    public class MyApplication : Application
    {
        public MyApplication(IntPtr handle, JniHandleOwnership ownerShip) : base(handle, ownerShip)
        {
            
        }
        
        public override void OnCreate()
        {
            base.OnCreate();
            DependencyService.Register<Game>();
            DependencyService.Register<FoosballWebServiceClient>();
            DependencyService.Register<TeamRepository>();
            DependencyService.Register<GameRepository>();
            DependencyService.Register<CredentialStorage>();
            DependencyService.Register<User>();
        }
    }
}