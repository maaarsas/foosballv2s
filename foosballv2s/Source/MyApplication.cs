using System;
using Android.App;
using Android.Runtime;
using foosballv2s.Droid.Shared.Source.Entities;
using foosballv2s.Droid.Shared.Source.Services.CredentialStorage;
using foosballv2s.Droid.Shared.Source.Services.FoosballWebService;
using foosballv2s.Droid.Shared.Source.Services.FoosballWebService.Repository;
using Xamarin.Forms;
using Application = Android.App.Application;
using Com.Instabug.Library;
using Com.Instabug.Library.Invocation;

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
            DependencyService.Register<AuthRepository>();
            DependencyService.Register<CredentialStorage>();
            DependencyService.Register<User>();
            new Instabug.Builder(this, "37f942cdde9613c5f6bd3acf2de082b3")
                    .SetInvocationEvent(InstabugInvocationEvent.Shake)
                    .Build();
        }
    }
}