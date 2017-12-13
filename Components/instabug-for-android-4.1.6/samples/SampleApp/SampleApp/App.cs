using System;
using Android.App;
using Android.Runtime;
using Com.Instabug.Library;
using Com.Instabug.Library.Invocation;
namespace Xamarinsampleandroidapp

{
	[Application]
	public class App : Application
	{
		protected App(IntPtr javaReference, JniHandleOwnership transfer)
		: base(javaReference, transfer)
		{
		}
		public override void OnCreate()
		{
			base.OnCreate();
            new Instabug.Builder(this, "7fc593eabb963bab027feebdca6eeaee")
                        .SetInvocationEvent(InstabugInvocationEvent.FloatingButton)
                        .SetTheme(InstabugColorTheme.InstabugColorThemeDark)
                        .Build();

		}
	}
}