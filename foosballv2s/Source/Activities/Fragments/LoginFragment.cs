using Android.App;
using Android.OS;
using Android.Views;
using foosballv2s.Resources;
using Fragment = Android.Support.V4.App.Fragment;

namespace foosballv2s.Source.Activities.Fragments
{
    public class LoginFragment : Fragment
    {
        public LoginFragment() {
        }
 
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            // Inflate the layout for this fragment
            return inflater.Inflate(Resource.Layout.fragment_login, container, false);
        }
    }
}