namespace foosballv2s.Droid.Shared.Source.Services.FoosballWebService.Helpers
{
    public class UrlParamsFormatter
    {
        public string UrlParams { get; private set; } = "";

        public void AddParam(string name, string value)
        {
            if (name.Length == 0 || value.Length == 0)
            {
                return;
            }
            if (UrlParams.Length > 0)
            {
                UrlParams += "&";
            }
            UrlParams += name + "=" + value;
        }
    }
}