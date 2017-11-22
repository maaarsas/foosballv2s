using System.Collections.Generic;
using System.IO;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using Emgu.CV.Structure;
using foosballv2s.Source.Activities.Fragments;
using foosballv2s.Source.Activities.Helpers;
using foosballv2s.Source.Entities;
using foosballv2s.Source.Services.FileIO;
using foosballv2s.Source.Services.FoosballWebService.Models;
using foosballv2s.Source.Services.FoosballWebService.Repository;
using foosballv2s.Source.Services.GameRecognition;
using Java.Interop;
using Java.Lang;
using Xamarin.Forms;
using Button = Android.Widget.Button;
using Camera = Android.Hardware.Camera;
using Color = Android.Graphics.Color;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using String = System.String;
using View = Android.Views.View;

namespace foosballv2s.Source.Activities
{
    /// <summary>
    /// </summary>
    [Activity()]
    public class AuthActivity : FragmentActivity
    {
        private Toolbar toolbar;
        private TabLayout tabLayout;
        private ViewPager viewPager;

        private AuthRepository _authRepository;
 
        protected override void OnCreate(Bundle savedInstanceState) 
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Auth);

            _authRepository = DependencyService.Get<AuthRepository>();
 
            viewPager = (ViewPager) FindViewById(Resource.Id.viewpager);
            setupViewPager(viewPager);
 
            tabLayout = (TabLayout) FindViewById(Resource.Id.sliding_tabs);
            tabLayout.SetupWithViewPager(viewPager);
        }

        [Export("SubmitLogin")]
        public async void SubmitLogin(View view)
        {
            EditText emailEditText = (EditText) FindViewById(Resource.Id.login_email);
            EditText passwordEditText = (EditText) FindViewById(Resource.Id.login_password);
            
            LoginViewModel loginModel = new LoginViewModel();
            loginModel.Email = emailEditText.Text;
            loginModel.Password = passwordEditText.Text;
            
            ProgressDialog dialog = ProgressDialog.Show(this, "", 
                Resources.GetString(Resource.String.logging_in), true);

            bool loginResult = await _authRepository.Login(loginModel);
            dialog.Dismiss();

            if (loginResult == false)
            {
                Toast.MakeText(this, Resource.String.login_error, ToastLength.Long).Show();
                return;
            }
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }
        
        [Export("SubmitRegister")]
        public async void SubmitRegister(View view)
        {
            
        }
        
        private void setupViewPager(ViewPager viewPager)
        {
            ViewPagerAdapter adapter = new ViewPagerAdapter(SupportFragmentManager);
            adapter.addFragment(new LoginFragment(), Resources.GetString(Resource.String.login));
            adapter.addFragment(new RegisterFragment(), Resources.GetString(Resource.String.register));
            viewPager.Adapter = adapter;
        }
        
        
        
        private class ViewPagerAdapter : FragmentPagerAdapter
        {
            private List<Android.Support.V4.App.Fragment> mFragmentList = new List<Android.Support.V4.App.Fragment>();
            private List<string> mFragmentTitleList = new List<string>();
 
            public ViewPagerAdapter(Android.Support.V4.App.FragmentManager manager) : base(manager)
            {
                //base.OnCreate(manager);
            }
 
            public override int Count {
                get {
                    return mFragmentList.Count;
                }
            }
            public override Android.Support.V4.App.Fragment GetItem(int postion)
            {
                return mFragmentList[postion];
            }
 
            public override ICharSequence GetPageTitleFormatted(int position)
            {
             
                return new Java.Lang.String(mFragmentTitleList[position].ToLower());// display the title
                //return null;// display only the icon
            }
 
            public void addFragment(Android.Support.V4.App.Fragment fragment, string title)
            {
                mFragmentList.Add(fragment);
                mFragmentTitleList.Add(title);
            }
        }
    }
}