using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using foosballv2s.Source.Activities.Fragments;
using foosballv2s.Source.Services.FoosballWebService.Models;
using foosballv2s.Source.Services.FoosballWebService.Repository;
using Java.Interop;
using Java.Lang;
using Xamarin.Forms;
using Application = Android.App.Application;
using View = Android.Views.View;
using Java.Util;
using Android.Content.Res;

namespace foosballv2s.Source.Activities
{
    /// <summary>
    /// </summary>
    [Activity(WindowSoftInputMode = SoftInput.StateHidden)]
    public class AuthActivity : FragmentActivity
    {
        private Toolbar toolbar;
        private TabLayout tabLayout;
        private ViewPager viewPager;
        private AuthRepository _authRepository;
 
        protected override void OnCreate(Bundle savedInstanceState) 
        {
            base.OnCreate(savedInstanceState);
            
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            
            SetContentView(Resource.Layout.Auth);

            _authRepository = DependencyService.Get<AuthRepository>();
 
            viewPager = (ViewPager) FindViewById(Resource.Id.viewpager);
            setupViewPager(viewPager);
 
            tabLayout = (TabLayout) FindViewById(Resource.Id.sliding_tabs);
            tabLayout.SetupWithViewPager(viewPager);
        }

        [Export("ChangeLanguage")]
        public async void ChangeLanguage(View view)
        {
            Button btn = (Button)FindViewById(Resource.Id.lang_button);
            LayoutInflater inflater = (LayoutInflater)ApplicationContext.GetSystemService(Context.LayoutInflaterService);
            View layout_spinners = inflater.Inflate(Resource.Layout.language_spinner, null);
            Spinner spinner = (Spinner)layout_spinners.FindViewById(Resource.Id.spinner);
            Intent intent = new Intent(this, typeof(AuthActivity));
            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetTitle(Resource.String.choose_lang);
            builder.SetView(layout_spinners);
            builder.SetCancelable(false);
            builder.SetPositiveButton(Resource.String.ok, delegate
            {
                if (spinner.SelectedItem.ToString() == Resources.GetString(Resource.String.item_lt))
                {
                    UpdateResources(this, "lt");
                    builder.Dispose();
                    StartActivity(intent);
                    Finish();
                }
                    
                else if (spinner.SelectedItem.ToString() == Resources.GetString(Resource.String.item_en))
                {
                    UpdateResources(this, "en");
                    builder.Dispose();
                    StartActivity(intent);
                    Finish();
                }
            });
            builder.Show();
        }

        private static bool UpdateResources(Context context, string language)
        {
            Locale locale = new Locale(language);
            Locale.Default = locale;

            Resources resources = context.Resources;

            Configuration configuration = resources.Configuration;
            configuration.Locale = locale;

            resources.UpdateConfiguration(configuration, resources.DisplayMetrics);

            return true;
        }

        [Export("SubmitLogin")]
        public async void SubmitLogin(View view)
        {
            ProgressDialog dialog = ProgressDialog.Show(this, "", 
                Resources.GetString(Resource.String.logging_in), true);
            
            EditText emailEditText = (EditText) FindViewById(Resource.Id.login_email);
            EditText passwordEditText = (EditText) FindViewById(Resource.Id.login_password);
            
            LoginViewModel loginModel = new LoginViewModel();
            loginModel.Email = emailEditText.Text;
            loginModel.Password = passwordEditText.Text;

            bool loginResult = await _authRepository.Login(loginModel);
            dialog.Dismiss();

            if (loginResult == false)
            {
                Toast.MakeText(Application.Context, Resource.String.login_error, ToastLength.Long).Show();
                return;
            }
            Toast.MakeText(Application.Context, Resource.String.login_success, ToastLength.Long).Show();
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }
        
        [Export("SubmitRegister")]
        public async void SubmitRegister(View view)
        {
            ProgressDialog dialog = ProgressDialog.Show(this, "", 
                Resources.GetString(Resource.String.registering), true);
            
            EditText emailEditText = (EditText) FindViewById(Resource.Id.register_email);
            EditText passwordEditText = (EditText) FindViewById(Resource.Id.register_password);
            EditText repeatPasswordEditText = (EditText) FindViewById(Resource.Id.register_repeat_password);
            
            RegisterViewModel registerModel = new RegisterViewModel();
            registerModel.Email = emailEditText.Text;
            registerModel.Password = passwordEditText.Text;
            registerModel.ConfirmPassword = repeatPasswordEditText.Text;

            bool registerResult = await _authRepository.Register(registerModel);
            dialog.Dismiss();

            if (registerResult == false)
            {
                Toast.MakeText(Application.Context, Resource.String.register_error, ToastLength.Long).Show();
                return;
            }
            else
            {
                Toast.MakeText(Application.Context, Resource.String.register_success, ToastLength.Long).Show();
                return;
            }
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