using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;

namespace foosballv2s.Droid.Shared.Source.Helpers
{
    public class ConfigHelper
    {
        public static string GetConfigData(Context context, string name) {
            try 
            {
                ApplicationInfo ai = context.PackageManager.GetApplicationInfo(
                    context.PackageName, PackageInfoFlags.MetaData);
                Bundle bundle = ai.MetaData;
                return bundle.GetString(name);
            } 
            catch (PackageManager.NameNotFoundException e) 
            {
                Log.Error("foosballv2s", "Unable to load meta-data: " + e.Message);
            }
            return null;
        }
    }
}