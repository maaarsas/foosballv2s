Instabug SDK aims to save developers time. We believe that you should spend your time creating awesome features and improving your application instead of chasing information from your testers and users. We believe that spending hours and hours debugging to locate the source of a problem is a waste of time.


## Supported Features 
This can be done through our main products:
1. **Bug Reporting:** With each bug report, we automatically capture a screenshot that your users can draw on to highlight or blur parts of it. Also, your users can attach other screenshots, voice notes, or even attach a screen recording of the app. Moreoevr, with each bug report we attach information about the device, the user, user steps, user events, network logs and the console logs.

2. **Crash Reporting:** Whenever your app crashes, Instabug reports this with all the stack trace information on all the running threads. With each crash report, Instabug captures a detailed report of the running environment, the different threads’ states, the steps to reproduce the crash, and the network request logs. All the data is grabbed automatically with no need for breadcrumbs. 
3. **In-App Chat:** With Instabug, your users can communicate with you from within your app with minimal effort for your users providing a much better experience compared to the decade old email composer that has the reputation of being discarded.
4. **In-App Surveys:** This product gives you the ability to send targeted surveys to your users within your app in a simple, easy, and fast way for you as well as your customers. 

With each crash or bug report sent from your application, we send all the information needed to help you debug the problem easily and communicate with the user reporting the problem. Here, we discuss all the information that you may attach to the reports.

1. **Identify User:** This section discusses how to bind each report to the identity of the user reporting the problem. You have the flexibility to add some extra attributes about the device as well as the user.
2. **Logging:** Instead of having to reproduce the crash or bug on your device to check the logs, we automatically attach the console logs, the verbose logs, all the steps done by the user until the report was sent, the user events as well as the network logs.
3. **View Hierarchy:** In case of UI bugs, you will find the view hierarchy of the screen content sent with each bug report.
4. **Attachments:** The SDK gives you the freedom to send different types of attachments with each bug report. You can add a screenshot from the app with the user’s annotation, extra image from the gallery, audio recording, screen recording or an extra file that you create and attach yourself.
5. **Tags:** Last, to be able to categorize the reports on the dashboard and filter them you can add some relevant tags to the reports.

## Set Up
To start integrating the SDK, you can follow the instruction mentioned here. 
### iOS
To start using Instabug in your application you will need to initialize it. Add the following line in the `FinishedLaunching` method inside the `AppDelegate` class.                                             
~~~~
Instabug.StartWithToken("IOS_APP_TOKEN", IBGInvocationEvent.Shake);
~~~~
### Android
To start using Instabug in your application you will need to initialize it. Add the following line in the `OnCreate` method inside the `MainApplication` class.  
~~~~
new Instabug.Builder(this, "ANDROID_APP_TOKEN")
  	.SetInvocationEvent(InstabugInvocationEvent.FloatingButton)
  	.Build();
~~~~
You can find your app token by selecting the SDK tab from your [**Instabug dashboard**](https://instabug.com/app/sdk/).

## Documentation
Form more details about the supported API and how to use them, you can visit our [**documentation page**](https://docs.instabug.com/docs/xamarin-overview). 

## Contact US 
If you have any questions or feedback to hesitate to get in touch. You can reach us at any time through **support@instabug.com**.